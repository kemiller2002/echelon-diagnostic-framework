"""Independent OD1-G (open-generation) re-scoring from grader journals, spec section 8, plus g1-vs-g2 agreement.
python3 -I og_score.py
Choices: items whose asOfStage is earlier than the case's final stage are excluded; a scorable item absent from
the grader's mappings is treated as 'not-addressed' (spec-literal); requestIds truncated to 3 distinct ids."""
import sys, os, json, glob, csv, collections, statistics, random, itertools
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import od1_score as S
import od1_stats as ST

HERE = os.path.dirname(os.path.abspath(__file__))
G = os.path.join(S.RAW, "og-grades")


def stage_num(s):
    return int(s[1:]) if s else None


def load_grades():
    mp = {}
    for line in open(os.path.join(G, "grade-map.txt")):
        p = line.strip().split("|")
        if len(p) >= 3:
            mp[p[0]] = (p[1], "|".join(p[2:]))
    grades = []
    for f in sorted(glob.glob(os.path.join(G, "*.journal.jsonl"))):
        lab = {}; ev = [json.loads(l) for l in open(f)]
        for e in ev:
            if e.get("type") == "started": lab[e["key"]] = e["label"]
        for e in ev:
            if e.get("type") == "result":
                _, grader, blind = lab[e["key"]].split("|")
                cid, run = mp[blind]
                grades.append(dict(grader=grader, blind=blind, caseId=cid, runId=run, res=e["result"], journal=os.path.basename(f)))
    return grades


def og_score(case, truth, res, mode="spec"):
    final = max(stage_num(e["stage"]) for e in case["evidence"])
    kinds = {x["itemId"]: (x["type"], stage_num(x.get("asOfStage"))) for x in case["scoredItems"]}
    rd = {}
    for m in res.get("mappings", []):
        rd[m.get("itemId")] = (m.get("reading") or "not-addressed").strip().lower()
    num = den = 0.0; harm = 0; det = {}
    for k in truth["answers"]:
        typ, st = kinds[k["itemId"]]
        if st is not None and st < final:
            continue
        if k["itemId"] not in rd and mode == "orchestrator":
            continue
        r = rd.get(k["itemId"], "not-addressed")
        if typ == "action" and r == "not-addressed":
            ok = k["expected"] in ("harmful", "premature", "acceptable")
        else:
            ok = r == k["expected"]
        den += k["weight"]; num += k["weight"] * ok
        if typ == "action" and k["expected"] == "harmful" and r in ("preferred", "acceptable"):
            harm += 1
        det[k["itemId"]] = (typ, r, k["expected"], ok, k["weight"], k.get("tags", []))
    req = []
    for q in res.get("requestIds", []) or []:
        if q not in req: req.append(q)
    req = req[:3]
    disc = set(truth["discriminatingRequests"])
    rq = len(set(req) & disc) / min(3, len(disc))
    acc = num / den if den else 0.0
    return dict(score=max(0.0, 90 * acc + 10 * rq - 15 * harm), acc=acc, req=rq, harm=harm, items=det)


def kappa(pairs):
    n = len(pairs); po = sum(a == b for a, b in pairs) / n
    ca = collections.Counter(a for a, _ in pairs); cb = collections.Counter(b for _, b in pairs)
    pe = sum(ca[k] * cb[k] for k in set(ca) | set(cb)) / n / n
    return po, (po - pe) / (1 - pe) if pe < 1 else float("nan")


def main():
    cases = S.load_cases(); grades = load_grades()
    orch = {}
    for r in csv.DictReader(open(os.path.join(S.RAW, "..", "scored", "og", "og-runs.csv"))):
        orch[(r["runId"], r["grader"])] = r
    rows = []; maxd = (0, None); maxd_o = (0, None)
    missing = collections.Counter(); early_present = 0; extra_ids = 0
    for g in grades:
        c, t = cases[g["caseId"]]
        s = og_score(c, t, g["res"]); so = og_score(c, t, g["res"], mode="orchestrator")
        final = max(stage_num(e["stage"]) for e in c["evidence"])
        ids = {m.get("itemId") for m in g["res"].get("mappings", [])}
        for x in c["scoredItems"]:
            st = stage_num(x.get("asOfStage"))
            if st is not None and st < final:
                early_present += x["itemId"] in ids
            elif x["itemId"] not in ids:
                missing[(g["runId"], g["grader"])] += 1
        extra_ids += len(ids - {x["itemId"] for x in c["scoredItems"]})
        o = orch.get((g["runId"], g["grader"]))
        d = abs(s["score"] - float(o["score"])) if o else None
        do = abs(so["score"] - float(o["score"])) if o else None
        if d is not None and d > maxd[0]: maxd = (d, g["runId"] + "/" + g["grader"])
        if do is not None and do > maxd_o[0]: maxd_o = (do, g["runId"] + "/" + g["grader"])
        cond = g["runId"].split("|")[1]
        rows.append(dict(g, cond=cond, **{k: v for k, v in s.items()}, orch=float(o["score"]) if o else None))
    with open(os.path.join(HERE, "og-replication-runs.csv"), "w", newline="") as fh:
        w = csv.writer(fh); w.writerow(["runId", "grader", "condition", "caseId", "score", "acc", "req", "harm", "orch_score"])
        for r in rows:
            w.writerow([r["runId"], r["grader"], r["cond"], r["caseId"], "%.4f" % r["score"], "%.4f" % r["acc"], "%.4f" % r["req"], r["harm"], r["orch"]])
    rep = ["## Open-generation arm (OD1-G) re-scored from grader journals", ""]
    rep.append("Grades: %d (g1=%d, g2=%d); orchestrator rows: %d. Max |my score − orchestrator| = %.4f (%s). Early-stage items present in grader mappings: %d. Scorable items missing from mappings: %d in %d gradings. Mapping ids not in case: %d.\n" % (
        len(grades), sum(g["grader"] == "g1" for g in grades), sum(g["grader"] == "g2" for g in grades), len(orch), maxd[0], maxd[1], early_present, sum(missing.values()), len(missing), extra_ids))
    # condition means (g1 = primary)
    g1 = [r for r in rows if r["grader"] == "g1"]
    rep.append("| condition | n | mean OG score (g1) | mean acc | mean req | harmful recommended |\n|---|---|---|---|---|---|")
    for cond in sorted({r["cond"] for r in g1}):
        xs = [r for r in g1 if r["cond"] == cond]
        rep.append("| %s | %d | %.2f | %.3f | %.3f | %d |" % (cond, len(xs), statistics.mean(r["score"] for r in xs), statistics.mean(r["acc"] for r in xs), statistics.mean(r["req"] for r in xs), sum(r["harm"] for r in xs)))
    rep.append("")
    # paired contrasts
    def cm(cond):
        return {r["caseId"]: r["score"] for r in g1 if r["cond"] == cond}
    tab = []
    for a, b in (("D_EDFX", "B_EDF03"), ("B_EDF03", "A_GENERAL"), ("B_EDF03", "N_NEUTRAL"), ("D_EDFX", "N_NEUTRAL"), ("D_EDFX", "A_GENERAL"), ("C_KTNT", "B_EDF03")):
        ma, mb = cm(a), cm(b); cs = sorted(set(ma) & set(mb))
        if not cs: continue
        d = [ma[c] - mb[c] for c in cs]; sd = statistics.stdev(d)
        lo, hi = ST.boot_ci(d)
        tab.append(dict(phase="og", a=a, b=b, n=len(d), meanA=statistics.mean(ma[c] for c in cs), meanB=statistics.mean(mb[c] for c in cs), diff=statistics.mean(d), sd=sd, dz=statistics.mean(d) / sd if sd else float("nan"), lo=lo, hi=hi, p=ST.signflip_p(d), wins=sum(x > 1e-9 for x in d), losses=sum(x < -1e-9 for x in d)))
    for r, h in zip(tab, ST.holm([r["p"] for r in tab])): r["holm"] = h
    rep.append(ST.fmt_table(tab, "OG paired contrasts (grader g1; Holm within these)"))
    # agreement
    by = collections.defaultdict(dict)
    for r in rows: by[r["runId"]][r["grader"]] = r
    dbl = [v for v in by.values() if "g1" in v and "g2" in v]
    pairs = []; pairs_t = collections.defaultdict(list); sc = []
    for v in dbl:
        for iid, (typ, r1, exp, ok1, w, tags) in v["g1"]["items"].items():
            r2 = v["g2"]["items"][iid][1]; ok2 = v["g2"]["items"][iid][3]
            pairs.append((r1, r2)); pairs_t[typ].append((r1, r2))
        sc.append((v["g1"]["score"], v["g2"]["score"], v["g1"]["cond"], v["g1"]["caseId"]))
    po, k = kappa(pairs)
    L = ["### Inter-grader agreement (g1 vs g2, %d double-graded runs, %d item readings)" % (len(dbl), len(pairs)), "",
         "Overall raw agreement %.3f, Cohen's kappa %.3f." % (po, k), "", "| item type | n | agreement | kappa |", "|---|---|---|---|"]
    for typ, ps in sorted(pairs_t.items()):
        a, kk = kappa(ps); L.append("| %s | %d | %.3f | %.3f |" % (typ, len(ps), a, kk))
    d = [a - b for a, b, _, _ in sc]
    mean1 = statistics.mean(a for a, _, _, _ in sc); mean2 = statistics.mean(b for _, b, _, _ in sc)
    # Pearson & ICC(2,1)-like via simple formulas
    xa = [a for a, _, _, _ in sc]; xb = [b for _, b, _, _ in sc]
    r = statistics.correlation(xa, xb) if len(sc) > 2 else float("nan")
    L.append("\nRun-score agreement: mean g1 %.2f vs g2 %.2f; mean |g1−g2| %.2f; max |g1−g2| %.2f; Pearson r %.3f." % (mean1, mean2, statistics.mean(abs(x) for x in d), max(abs(x) for x in d), r))
    L.append("Conditions in double-graded subset: %s" % dict(collections.Counter(c for _, _, c, _ in sc)))
    dis = collections.Counter((a, b) for a, b in pairs if a != b)
    L.append("Most common disagreements (g1→g2): " + "; ".join("%s→%s ×%d" % (a, b, n) for (a, b), n in dis.most_common(8)))
    rep.append("\n".join(L) + "\n")
    # condition-name leakage into OG outputs (grader sees analyst output)
    leak = collections.Counter()
    terms = {"EDF": "edf", "Kepner": "kepner", "Tregoe": "tregoe", "Nadler": "nadler", "congruence": "congruence", "premise check": "premise check", "Clarity": "clarity"}
    for f in glob.glob(os.path.join(S.RAW, "og", "*.json")):
        rr = json.load(open(f)); txt = json.dumps(rr["output"]).lower()
        for k, v in terms.items():
            if v in txt: leak[(rr["condition"], k)] += 1
    rep.append("Method-name vocabulary visible to the 'blind' grader in OG analyst outputs (runs containing term): " + (", ".join("%s:%s×%d" % (c, k, n) for (c, k), n in sorted(leak.items())) or "none") + "\n")
    open(os.path.join(HERE, "og-output.md"), "w").write("\n".join(rep))
    print("\n".join(rep))
    print("orchestrator-mode max diff", maxd_o)


if __name__ == "__main__":
    main()
