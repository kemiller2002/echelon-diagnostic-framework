"""Independent paired statistics, sensitivity analyses and power for OD1 (Agent O). python3 -I od1_stats.py"""
import sys, os, csv, json, math, random, itertools, statistics, collections
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import od1_score as S

HERE = os.path.dirname(os.path.abspath(__file__))
SEED = 20261116
NBOOT = 10000

CONTRASTS = [
    ("heldout", "B_EDF03", "A_GENERAL"), ("heldout", "B_EDF03", "N_NEUTRAL"), ("heldout", "D_EDFX", "B_EDF03"),
    ("heldout", "D_EDFX", "N_NEUTRAL"), ("heldout", "C_KTNT", "B_EDF03"), ("heldout", "B0_EDF0", "B_EDF03"),
    ("heldout", "B_CLARITY", "B_EDF03"), ("heldout", "BI_CLARITY", "B_EDF03"), ("heldout", "D_EDFX", "D_NOGATE"),
    ("heldout", "D_EDFX", "D_NOORG"), ("haiku", "B_EDF03", "A_GENERAL"), ("haiku", "D_EDFX", "B_EDF03"),
    ("haiku", "N_NEUTRAL", "A_GENERAL"), ("dev", "B_EDF03", "A_GENERAL"), ("dev", "B_EDF03", "N_NEUTRAL"),
]


def case_means(rows, phase, cond, key="score"):
    d = collections.defaultdict(list)
    for r in rows:
        if r["phase"] == phase and r["condition"] == cond:
            d[r["caseId"]].append(r[key])
    return {k: sum(v) / len(v) for k, v in d.items()}


def boot_ci(diffs, seed=SEED, n=NBOOT):
    rng = random.Random(seed)
    k = len(diffs)
    ms = sorted(sum(diffs[rng.randrange(k)] for _ in range(k)) / k for _ in range(n))
    return ms[int(0.025 * n)], ms[int(0.975 * n) - 1]


def signflip_p(diffs):
    """Exact two-sided sign-flip permutation p (all 2^n sign patterns; n <= 12)."""
    obs = abs(sum(diffs))
    n = len(diffs); cnt = 0; tot = 0
    for signs in itertools.product((1, -1), repeat=n):
        tot += 1
        if abs(sum(s * d for s, d in zip(signs, diffs))) >= obs - 1e-9:
            cnt += 1
    return cnt / tot


def holm(ps):
    m = len(ps); order = sorted(range(m), key=lambda i: ps[i]); adj = [0] * m; run = 0
    for rank, i in enumerate(order):
        run = max(run, min(1.0, (m - rank) * ps[i])); adj[i] = run
    return adj


def contrast_table(rows, key="score", contrasts=CONTRASTS):
    out = []
    for ph, a, b in contrasts:
        ma, mb = case_means(rows, ph, a, key), case_means(rows, ph, b, key)
        cs = sorted(set(ma) & set(mb))
        diffs = [ma[c] - mb[c] for c in cs]
        m = statistics.mean(diffs); sd = statistics.stdev(diffs) if len(diffs) > 1 else float("nan")
        lo, hi = boot_ci(diffs)
        p = signflip_p(diffs)
        out.append(dict(phase=ph, a=a, b=b, n=len(diffs), meanA=statistics.mean(ma[c] for c in cs), meanB=statistics.mean(mb[c] for c in cs),
                        diff=m, sd=sd, dz=(m / sd if sd > 0 else float("nan")), lo=lo, hi=hi, p=p,
                        wins=sum(d > 1e-9 for d in diffs), losses=sum(d < -1e-9 for d in diffs), diffs=dict(zip(cs, diffs))))
    for r, a in zip(out, holm([r["p"] for r in out])):
        r["holm"] = a
    return out


def fmt_table(tab, title):
    L = ["### " + title, "", "| phase | contrast | n | mean A | mean B | diff | 95% boot CI | SD diff | dz | sign-flip p | Holm p | W/L/T |", "|---|---|---|---|---|---|---|---|---|---|---|---|"]
    for r in tab:
        L.append("| %s | %s − %s | %d | %.2f | %.2f | **%+.2f** | [%+.2f, %+.2f] | %.2f | %+.2f | %.4f | %.4f | %d/%d/%d |" % (
            r["phase"], r["a"], r["b"], r["n"], r["meanA"], r["meanB"], r["diff"], r["lo"], r["hi"], r["sd"], r["dz"], r["p"], r["holm"],
            r["wins"], r["losses"], r["n"] - r["wins"] - r["losses"]))
    return "\n".join(L) + "\n"


def rescore_excluding(rows, cases, exclude):
    """Re-score each run with items in exclude[caseId] removed from every component."""
    new = []
    for r in rows:
        c, t = cases[r["caseId"]]
        ex = exclude.get(r["caseId"], set())
        if ex:
            c2 = dict(c); c2["scoredItems"] = [x for x in c["scoredItems"] if x["itemId"] not in ex]
            out = dict(r["output"]); out["answers"] = [a for a in out["answers"] if a.get("itemId") not in ex]
            s = S.score(c2, t, out)
            r = dict(r); r.update(score=s["score"], acc=s["acc"], accOnly=s["accOnly"])
        new.append(r)
    return new


def n_needed(sd, delta, alpha=0.05, power=0.80):
    z = 1.959964 + 0.841621
    n = (z * sd / delta) ** 2
    # one-step t correction
    for _ in range(20):
        df = max(1, math.ceil(n) - 1)
        tcrit = tq(1 - alpha / 2, df) + tq(power, df)
        n = (tcrit * sd / delta) ** 2
    return math.ceil(n)


def tq(p, df):
    # inverse Student t via bisection on CDF (regularized incomplete beta through continued fraction)
    lo, hi = -50.0, 50.0
    for _ in range(200):
        mid = (lo + hi) / 2
        if tcdf(mid, df) < p: lo = mid
        else: hi = mid
    return (lo + hi) / 2


def tcdf(t, df):
    x = df / (df + t * t)
    ib = betainc(df / 2, 0.5, x)
    return 1 - 0.5 * ib if t > 0 else 0.5 * ib


def betainc(a, b, x):
    if x <= 0: return 0.0
    if x >= 1: return 1.0
    lbeta = math.lgamma(a + b) - math.lgamma(a) - math.lgamma(b) + a * math.log(x) + b * math.log(1 - x)
    if x < (a + 1) / (a + b + 2):
        return math.exp(lbeta) * cf(a, b, x) / a
    return 1 - math.exp(lbeta) * cf(b, a, 1 - x) / b


def cf(a, b, x):
    qab, qap, qam = a + b, a + 1, a - 1; c = 1.0; d = 1 - qab * x / qap
    d = 1 / (d if abs(d) > 1e-30 else 1e-30); h = d
    for m in range(1, 300):
        m2 = 2 * m; aa = m * (b - m) * x / ((qam + m2) * (a + m2))
        d = 1 + aa * d; d = 1 / (d if abs(d) > 1e-30 else 1e-30); c = 1 + aa / c; h *= d * c
        aa = -(a + m) * (qab + m) * x / ((a + m2) * (qap + m2))
        d = 1 + aa * d; d = 1 / (d if abs(d) > 1e-30 else 1e-30); c = 1 + aa / c; dl = d * c; h *= dl
        if abs(dl - 1) < 1e-12: break
    return h


def power_at(sd, delta, n, alpha=0.05):
    # approximate power of paired t at n via noncentral normal approx with t critical value
    df = n - 1; ncp = delta / (sd / math.sqrt(n)); tc = tq(1 - alpha / 2, df)
    return 1 - tcdf(tc - ncp, df)


def main():
    cases = S.load_cases()
    rows = S.all_runs(phases=("dev", "heldout", "haiku"))
    rep = []
    # --- condition means / distribution
    rep.append("## Condition score distributions (run level)\n\n| phase | condition | runs | mean | SD | min | median | max | share ≥95 | mean item acc |\n|---|---|---|---|---|---|---|---|---|---|")
    for ph in ("dev", "heldout", "haiku"):
        for cond in sorted({r["condition"] for r in rows if r["phase"] == ph}):
            xs = [r["score"] for r in rows if r["phase"] == ph and r["condition"] == cond]
            ac = [r["acc"] for r in rows if r["phase"] == ph and r["condition"] == cond]
            rep.append("| %s | %s | %d | %.2f | %.2f | %.2f | %.2f | %.2f | %.0f%% | %.3f |" % (ph, cond, len(xs), statistics.mean(xs), statistics.stdev(xs), min(xs), statistics.median(xs), max(xs), 100 * sum(x >= 95 for x in xs) / len(xs), statistics.mean(ac)))
    rep.append("")
    main_tab = contrast_table(rows)
    rep.append(fmt_table(main_tab, "Primary score: paired case-level contrasts (trials averaged; bootstrap seed %d, %d resamples; exact sign-flip p over 2^n patterns; Holm over these 15)" % (SEED, NBOOT)))
    # per-case diffs CSV
    with open(os.path.join(HERE, "contrasts-per-case.csv"), "w", newline="") as fh:
        w = csv.writer(fh); w.writerow(["phase", "contrast", "caseId", "diff"])
        for r in main_tab:
            for c, d in sorted(r["diffs"].items()):
                w.writerow([r["phase"], r["a"] + "-" + r["b"], c, "%.4f" % d])
    # --- lenient Q parsing variant for haiku
    rows_len = S.all_runs(phases=("haiku",), lenient=True)
    ht = contrast_table(rows_len, contrasts=[c for c in CONTRASTS if c[0] == "haiku"])
    rep.append(fmt_table(ht, "Haiku contrasts with lenient request-id parsing (\"Q1: title\" counted as Q1) — Holm within these 3 only"))
    # --- (b) accuracy-only
    acc_tab = contrast_table(rows, key="accOnly")
    rep.append(fmt_table(acc_tab, "Sensitivity (b): accuracy-only score (100 × weighted item accuracy)"))
    # --- (a) items all conditions identically wrong
    def all_wrong(phase, identical):
        per = collections.defaultdict(list)
        for r in rows:
            if r["phase"] == phase:
                for (iid, typ, ans, exp, ok, c, w, f1, tags) in r["items"]:
                    per[(r["caseId"], iid)].append((ans, exp, ok))
        ex = collections.defaultdict(set)
        for (cid, iid), v in per.items():
            if not any(ok for _, _, ok in v) and (not identical or len({a for a, _, _ in v}) == 1):
                ex[cid].add(iid)
        return ex, per
    dev_ex, dev_per = all_wrong("dev", False)
    ho_ex_id, ho_per = all_wrong("heldout", True)
    ho_ex_any, _ = all_wrong("heldout", False)
    lines = ["### Sensitivity (a): items every condition got wrong", ""]
    lines.append("Dev items wrong in all 8 dev runs (possible truth-key ambiguity): " + ", ".join("%s/%s (expected %s; given %s)" % (c, i, dev_per[(c, i)][0][1], "/".join(sorted({a for a, _, _ in dev_per[(c, i)]}))) for c in sorted(dev_ex) for i in sorted(dev_ex[c])) + "\n")
    lines.append("Held-out items wrong in all 144 Sonnet held-out runs with the identical wrong answer: " + (", ".join("%s/%s (expected %s; all answered %s)" % (c, i, ho_per[(c, i)][0][1], ho_per[(c, i)][0][0]) for c in sorted(ho_ex_id) for i in sorted(ho_ex_id[c])) or "none") + "\n")
    lines.append("Held-out items wrong in all held-out runs (any wrong answer): " + (", ".join("%s/%s (expected %s; given %s)" % (c, i, ho_per[(c, i)][0][1], "/".join(sorted({a for a, _, _ in ho_per[(c, i)]}))) for c in sorted(ho_ex_any) for i in sorted(ho_ex_any[c])) or "none") + "\n")
    # near-unanimous (>=90% of runs wrong)
    near = []
    for (cid, iid), v in sorted(ho_per.items()):
        fr = sum(not ok for _, _, ok in v) / len(v)
        if fr >= 0.9:
            near.append("%s/%s %.0f%% wrong (expected %s; modal answer %s)" % (cid, iid, 100 * fr, v[0][1], collections.Counter(a for a, _, _ in v).most_common(1)[0][0]))
    lines.append("Held-out items wrong in ≥90% of Sonnet runs: " + ("; ".join(near) or "none") + "\n")
    rep.append("\n".join(lines))
    ho_contr = [c for c in CONTRASTS if c[0] == "heldout"]
    rows_ex = rescore_excluding([r for r in rows if r["phase"] == "heldout"], cases, ho_ex_any)
    rep.append(fmt_table(contrast_table(rows_ex, contrasts=ho_contr), "Sensitivity (a): held-out contrasts excluding items wrong in every held-out run (Holm within these 10)"))
    # also exclude >=90% wrong items
    ex90 = collections.defaultdict(set)
    for (cid, iid), v in ho_per.items():
        if sum(not ok for _, _, ok in v) / len(v) >= 0.9: ex90[cid].add(iid)
    rows_ex90 = rescore_excluding([r for r in rows if r["phase"] == "heldout"], cases, ex90)
    rep.append(fmt_table(contrast_table(rows_ex90, contrasts=ho_contr), "Sensitivity (a'): held-out contrasts excluding items wrong in ≥90% of held-out runs (Holm within these 10)"))
    # haiku: same exclusion using sonnet-derived set
    rows_hex = rescore_excluding([r for r in rows if r["phase"] == "haiku"], cases, ho_ex_any)
    rep.append(fmt_table(contrast_table(rows_hex, contrasts=[c for c in CONTRASTS if c[0] == "haiku"]), "Sensitivity (a): haiku contrasts excluding the same held-out items"))
    # --- (c) per class
    L = ["### Sensitivity (c): per-benchmark-class differences (case-level, trials averaged)", "", "| phase | case | class | D_EDFX − B_EDF03 | B_EDF03 − A_GENERAL | B_EDF03 − N_NEUTRAL |", "|---|---|---|---|---|---|"]
    for ph in ("heldout", "haiku"):
        sc = {c: case_means(rows, ph, c) for c in ("A_GENERAL", "B_EDF03", "D_EDFX", "N_NEUTRAL")}
        for cid in sorted(sc["B_EDF03"]):
            L.append("| %s | %s | %s | %+.2f | %+.2f | %+.2f |" % (ph, cid, cases[cid][0]["benchmarkClass"], sc["D_EDFX"][cid] - sc["B_EDF03"][cid], sc["B_EDF03"][cid] - sc["A_GENERAL"][cid], sc["B_EDF03"][cid] - sc["N_NEUTRAL"][cid]))
    sc = {c: case_means(rows, "dev", c) for c in ("A_GENERAL", "B_EDF03", "N_NEUTRAL")}
    for cid in sorted(sc["B_EDF03"]):
        L.append("| dev | %s | %s | – | %+.2f | %+.2f |" % (cid, cases[cid][0]["benchmarkClass"], sc["B_EDF03"][cid] - sc["A_GENERAL"][cid], sc["B_EDF03"][cid] - sc["N_NEUTRAL"][cid]))
    rep.append("\n".join(L) + "\n")
    # --- power
    L = ["### Power for detecting +5 points (paired t, two-sided α=0.05, 80% power)", "", "| contrast | observed SD of case diffs | n needed for +5 | power at n=12 (or observed n) for +5 | n needed for +3 |", "|---|---|---|---|---|"]
    for r in main_tab:
        L.append("| %s %s − %s | %.2f | %d | %.2f | %d |" % (r["phase"], r["a"], r["b"], r["sd"], n_needed(r["sd"], 5), power_at(r["sd"], 5, r["n"]), n_needed(r["sd"], 3)))
    rep.append("\n".join(L) + "\n")
    # --- test-retest of trials (B, D)
    L = ["### Trial-to-trial noise (held-out, B_EDF03 and D_EDFX two trials)", "", "| condition | item agreement t1 vs t2 | mean |score t1 − t2| | SD of (t1 − t2) |", "|---|---|---|---|"]
    for cond in ("B_EDF03", "D_EDFX"):
        t1 = {r["caseId"]: r for r in rows if r["phase"] == "heldout" and r["condition"] == cond and r["trial"] == 1}
        t2 = {r["caseId"]: r for r in rows if r["phase"] == "heldout" and r["condition"] == cond and r["trial"] == 2}
        agree = tot = 0
        for cid in t1:
            a1 = {x[0]: x[2] for x in t1[cid]["items"]}; a2 = {x[0]: x[2] for x in t2[cid]["items"]}
            for i in a1: tot += 1; agree += a1[i] == a2[i]
        d = [t1[c]["score"] - t2[c]["score"] for c in t1]
        L.append("| %s | %.3f (%d/%d) | %.2f | %.2f |" % (cond, agree / tot, agree, tot, statistics.mean(abs(x) for x in d), statistics.stdev(d)))
    rep.append("\n".join(L) + "\n")
    with open(os.path.join(HERE, "stats-output.md"), "w") as fh:
        fh.write("\n".join(rep))
    json.dump([{k: v for k, v in r.items() if k != "diffs"} for r in main_tab], open(os.path.join(HERE, "contrasts.json"), "w"), indent=1)
    print("\n".join(rep))


if __name__ == "__main__":
    main()
