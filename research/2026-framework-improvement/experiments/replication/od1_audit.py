"""Leakage / bias audit for OD1 (Agent O). python3 -I od1_audit.py"""
import sys, os, json, re, glob, collections, statistics, itertools
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import od1_score as S

HERE = os.path.dirname(os.path.abspath(__file__))
P = os.path.join(S.R, "experiments", "prompts")
STOP = set("""the a an and or of to in on for with by from as at is are was were be been being that this these those it its not no
than then but if into over under more most less least any all each every only also can could would should may might must will
their there they them have has had which who whom what when where while about between after before because such other same
one two three your you our we his her he she do does did done so very just much many some own make made""".split())


def words(s):
    return re.findall(r"[a-z][a-z\-]+", s.lower())


def shingles(s, k):
    w = words(s)
    return {" ".join(w[i:i + k]) for i in range(len(w) - k + 1)}


def strings(o):
    if isinstance(o, str): yield o
    elif isinstance(o, dict):
        for v in o.values(): yield from strings(v)
    elif isinstance(o, list):
        for v in o: yield from strings(v)


def main():
    cases = S.load_cases()
    rows = S.all_runs(phases=("dev", "heldout", "haiku"))
    rep = ["## Audit", ""]
    # 1. outputs quoting truth rationale (8-word shingles in output, in truth rationale/narrative, not in case text)
    K = 8
    case_sh = {}; truth_sh = {}
    for cid, (c, t) in cases.items():
        case_sh[cid] = set().union(*(shingles(s, K) for s in strings(c)))
        truth_sh[cid] = set().union(*(shingles(s, K) for s in [t["groundTruthNarrative"]] + [a["rationale"] for a in t["answers"]]))
    hits = []
    for ph in ("dev", "heldout", "haiku", "og"):
        for f in sorted(glob.glob(os.path.join(S.RAW, ph, "*.json"))):
            r = json.load(open(f)); cid = r["caseId"]
            osh = set().union(*(shingles(s, K) for s in strings(r["output"]))) if r["output"] else set()
            h = (osh & truth_sh[cid]) - case_sh[cid]
            if h: hits.append((ph, r["condition"], cid, sorted(h)[:3], len(h)))
    rep.append("**Truth-text quotation.** 8-word shingles present in an analyzer output and in the truth narrative/rationales but absent from the analyzer-visible case file: %d runs affected out of %d checked (dev, held-out, haiku, OG)." % (len(hits), sum(len(glob.glob(os.path.join(S.RAW, p, '*.json'))) for p in ('dev', 'heldout', 'haiku', 'og'))))
    for h in hits[:10]:
        rep.append("- %s %s %s: %d shingles, e.g. \"%s\"" % (h[0], h[1], h[2], h[4], h[3][0]))
    rep.append("")
    # 2. identical outputs across conditions
    by_case = collections.defaultdict(list)
    for r in rows: by_case[(r["phase"], r["caseId"])].append(r)
    full_ident = ans_ident = ans_pairs = 0; ident_examples = []
    for (ph, cid), rs in by_case.items():
        for a, b in itertools.combinations(rs, 2):
            if a["condition"] == b["condition"]: continue
            ans_pairs += 1
            va = [(x[0], x[2]) for x in a["items"]]; vb = [(x[0], x[2]) for x in b["items"]]
            if va == vb: ans_ident += 1
            if json.dumps(a["output"], sort_keys=True) == json.dumps(b["output"], sort_keys=True):
                full_ident += 1; ident_examples.append((a["runId"], b["runId"]))
    # within-condition trial duplicates
    trial_dup = 0
    for (ph, cid), rs in by_case.items():
        for a, b in itertools.combinations(rs, 2):
            if a["condition"] == b["condition"] and json.dumps(a["output"], sort_keys=True) == json.dumps(b["output"], sort_keys=True): trial_dup += 1
    rep.append("**Identical outputs.** Cross-condition run pairs on the same case: %d; byte-identical outputs: %d %s; identical answer vectors (all items same label): %d (%.0f%%). Byte-identical trial pairs within a condition: %d." % (ans_pairs, full_ident, ident_examples[:3], ans_ident, 100 * ans_ident / ans_pairs, trial_dup))
    rep.append("")
    # 3. request-position bias
    pos = collections.Counter(); low = collections.Counter(); nc = 0
    for cid, (c, t) in cases.items():
        nc += 1
        for q in t["discriminatingRequests"]: pos[q] += 1
        for q in t["lowValueRequests"]: low[q] += 1
    firstk = 0; base = []
    for cid, (c, t) in cases.items():
        disc = set(t["discriminatingRequests"]); k = len(disc)
        firstk += disc == {"Q%d" % i for i in range(1, k + 1)}
        ch = {"Q1", "Q2", "Q3"}
        base.append(max(0, min(1, len(ch & disc) / min(3, k) - 0.5 * len(ch & set(t["lowValueRequests"])))))
    chose123 = collections.Counter(); totr = collections.Counter()
    for r in rows:
        totr[r["phase"]] += 1
        if sorted(r["output"].get("evidenceRequests", [])) == ["Q1", "Q2", "Q3"]: chose123[r["phase"]] += 1
    rep.append("**Evidence-request position leak.** Discriminating requests by menu position across all 22 cases: %s; low-value: %s. In %d/22 cases the discriminating set is exactly the first k menu items. A content-blind strategy 'always request Q1,Q2,Q3' earns mean request value %.3f (max 1). Share of runs requesting exactly {Q1,Q2,Q3}: %s. The 10-point request component therefore measures little beyond menu order." % (
        dict(sorted(pos.items())), dict(sorted(low.items())), firstk, statistics.mean(base), {p: "%d/%d" % (chose123[p], totr[p]) for p in totr}))
    rep.append("")
    # 4. answer-label base rates and content-blind baselines
    lab = collections.defaultdict(collections.Counter)
    for split in ("dev", "heldout"):
        for cid, (c, t) in cases.items():
            if c["split"] != split: continue
            types = {x["itemId"]: x["type"] for x in c["scoredItems"]}
            for a in t["answers"]: lab[(split, types[a["itemId"]])][a["expected"]] += a["weight"]
    rep.append("**Expected-label base rates (weight-weighted).**")
    for k in sorted(lab): rep.append("- %s %s: %s" % (k[0], k[1], dict(lab[k].most_common())))
    # positional majority baseline trained on dev, tested on heldout: (type, ordinal within type)
    trainpos = collections.defaultdict(collections.Counter); traintype = collections.defaultdict(collections.Counter)
    for cid, (c, t) in cases.items():
        if c["split"] != "dev": continue
        exp = {a["itemId"]: a["expected"] for a in t["answers"]}; cnt = collections.Counter()
        for x in c["scoredItems"]:
            cnt[x["type"]] += 1; trainpos[(x["type"], cnt[x["type"]])][exp[x["itemId"]]] += 1; traintype[x["type"]][exp[x["itemId"]]] += 1
    accs = {"type-majority": [], "type+position-majority": []}
    for cid, (c, t) in cases.items():
        if c["split"] != "heldout": continue
        key = {a["itemId"]: a for a in t["answers"]}; cnt = collections.Counter(); W = sum(a["weight"] for a in t["answers"])
        s1 = s2 = 0
        for x in c["scoredItems"]:
            cnt[x["type"]] += 1; k = key[x["itemId"]]
            g1 = traintype[x["type"]].most_common(1)[0][0]
            tp = trainpos.get((x["type"], cnt[x["type"]])) or traintype[x["type"]]
            g2 = tp.most_common(1)[0][0]
            s1 += k["weight"] * (g1 == k["expected"]); s2 += k["weight"] * (g2 == k["expected"])
        accs["type-majority"].append(s1 / W); accs["type+position-majority"].append(s2 / W)
    rep.append("- Content-blind baselines (learned on dev keys, applied to held-out): weighted accuracy %s. Analyzer accuracy is ~0.93–0.98, so the format is not trivially guessable, but the room between a careful reader and the ceiling is small.\n" % {k: round(statistics.mean(v), 3) for k, v in accs.items()})
    # 5. prompt vocabulary vs truth rationale vocabulary
    shared = set(words(open(os.path.join(P, "SHARED.txt")).read()))
    def vocab(text): return {w for w in words(text) if len(w) >= 4 and w not in STOP}
    case_vocab = {sp: set() for sp in ("dev", "heldout")}; truth_tok = {sp: collections.Counter() for sp in ("dev", "heldout")}
    for cid, (c, t) in cases.items():
        case_vocab[c["split"]] |= vocab(" ".join(strings(c)))
        for s in [t["groundTruthNarrative"]] + [a["rationale"] for a in t["answers"]]:
            for w in words(s):
                if len(w) >= 4 and w not in STOP: truth_tok[c["split"]][w] += 1
    rep.append("**Prompt-vocabulary affinity with truth rationales.** For each condition module: distinct content words (≥4 letters, not in SHARED.txt); `rate` = share of truth-rationale tokens (held-out keys) that are module words *and not present anywhere in held-out case text* (i.e. reasoning vocabulary the key uses that the case does not), per 1,000 tokens; `cover` = share of module words found in held-out truth rationales; same for dev keys.\n")
    rep.append("| module | module words | held-out rate /1k | held-out cover | dev rate /1k | dev cover | top shared rationale-only words (held-out) |\n|---|---|---|---|---|---|---|")
    for f in sorted(glob.glob(os.path.join(P, "*.module.txt"))):
        name = os.path.basename(f).replace(".module.txt", "")
        mv = vocab(open(f).read()) - shared
        out = [name, str(len(mv))]
        tops = ""
        for sp in ("heldout", "dev"):
            tt = truth_tok[sp]; tot = sum(tt.values())
            only = {w for w in tt if w not in case_vocab[sp]}
            rate = 1000 * sum(tt[w] for w in mv & only) / tot
            cover = len(mv & set(tt)) / len(mv) if mv else 0
            out += ["%.1f" % rate, "%.2f" % cover]
            if sp == "heldout": tops = ", ".join(w for w, _ in sorted(((w, tt[w]) for w in mv & only), key=lambda x: -x[1])[:8])
        rep.append("| " + " | ".join(out) + " | " + tops + " |")
    rep.append("")
    # does D prompt contain held-out case-specific terms?
    dv = vocab(open(os.path.join(P, "D_EDFX.module.txt")).read()) - shared
    allp = set()
    for f in glob.glob(os.path.join(P, "*.module.txt")):
        if "D_" not in os.path.basename(f): allp |= vocab(open(f).read())
    only_ho = (dv & case_vocab["heldout"]) - case_vocab["dev"] - allp
    rep.append("D_EDFX words that occur in held-out case text but in no dev case and no non-D module (possible held-out tuning signal): %s\n" % (sorted(only_ho) or "none"))
    # 6. prompt size by condition
    pc = collections.defaultdict(list)
    for r in json.load(open(os.devnull)) if False else []: pass
    for ph in ("heldout",):
        for f in glob.glob(os.path.join(S.RAW, ph, "*.json")):
            r = json.load(open(f)); pc[r["condition"]].append(r["promptChars"])
    rep.append("Mean prompt chars (held-out): " + ", ".join("%s %.0f" % (k, statistics.mean(v)) for k, v in sorted(pc.items())) + "\n")
    open(os.path.join(HERE, "audit-output.md"), "w").write("\n".join(rep))
    print("\n".join(rep))


if __name__ == "__main__":
    main()
