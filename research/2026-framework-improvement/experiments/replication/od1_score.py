"""Independent OD1 scorer (Agent O replication). Implements benchmark-specification.md section 5
from the text only. Standard library only; run with python3 -I.

Interpretation choices (spec is silent; each is flagged in the report):
  * evidence F1 per item: P = |cited & allowed| / |cited| (0 if nothing cited),
    R = |cited & required| / |required| (1 if required empty), F1 = 2PR/(P+R); cited ids de-duplicated;
    aggregated as weight-weighted mean over ALL items (incorrect items contribute 0).
  * calibration: unweighted mean over items of 1-(c-y)^2.
  * request value: first 3 distinct ids taken; hits/min(3,|disc|) - 0.5*low, clamped to [0,1].
    Strict mode: only exact candidate ids count. Lenient mode: a leading "Qn" token is parsed out
    of strings like "Q1: title".
  * harmful penalty: -15 for each item whose expected answer is 'harmful' and was answered
    preferred/acceptable; total floored at 0.
  * validity: output is an object; every scored item answered exactly once with an allowed answer and a
    numeric confidence in [0,100]; evidenceIds a list; evidenceRequests a list. Invalid -> score 0.
"""
import json, os, re, sys

R = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
CASES = os.path.join(R, "benchmarks", "cases")
RAW = os.path.join(R, "experiments", "raw")


def load_cases():
    out = {}
    for f in sorted(os.listdir(CASES)):
        if f.endswith(".case.json"):
            c = json.load(open(os.path.join(CASES, f)))
            t = json.load(open(os.path.join(CASES, f.replace(".case.json", ".truth.json"))))
            out[c["id"]] = (c, t)
    return out


def norm_q(q, lenient):
    if not isinstance(q, str):
        return None
    if lenient:
        m = re.match(r"\s*(Q\d+)\b", q)
        return m.group(1) if m else None
    return q


def validate(case, out):
    errs = []
    if not isinstance(out, dict):
        return ["output not object"]
    ans = out.get("answers")
    if not isinstance(ans, list):
        return ["answers not list"]
    items = {x["itemId"]: x for x in case["scoredItems"]}
    seen = {}
    for a in ans:
        if not isinstance(a, dict):
            errs.append("answer not object"); continue
        iid = a.get("itemId")
        if iid in seen:
            errs.append("duplicate " + str(iid))
        seen[iid] = a
        if iid not in items:
            continue  # extra items ignored
        if a.get("answer") not in items[iid]["allowedAnswers"]:
            errs.append("bad answer " + iid)
        cf = a.get("confidence")
        if isinstance(cf, bool) or not isinstance(cf, (int, float)) or not (0 <= cf <= 100):
            errs.append("bad confidence " + iid)
        if not isinstance(a.get("evidenceIds", []), list):
            errs.append("evidenceIds not list " + iid)
    for iid in items:
        if iid not in seen:
            errs.append("missing " + iid)
    if not isinstance(out.get("evidenceRequests", []), list):
        errs.append("evidenceRequests not list")
    return errs


def score(case, truth, out, lenient=False, cal_weighted=False):
    errs = validate(case, out)
    res = {"valid": not errs, "errors": ";".join(errs)}
    if errs:
        res.update(score=0.0, acc=0.0, evF1=0.0, cal=0.0, req=0.0, harm=0)
        return res
    key = {a["itemId"]: a for a in truth["answers"]}
    given = {a["itemId"]: a for a in out["answers"]}
    W = sum(key[x["itemId"]]["weight"] for x in case["scoredItems"])
    acc_w = f1_w = cal_sum = calw_sum = 0.0
    harm = 0
    n = 0
    per_item = []
    for x in case["scoredItems"]:
        iid = x["itemId"]; k = key[iid]; g = given[iid]; w = k["weight"]
        correct = g["answer"] == k["expected"]
        cited = set(e for e in g.get("evidenceIds", []) if isinstance(e, str))
        f1 = 0.0
        if correct:
            acc_w += w
            P = len(cited & set(k["allowedEvidence"])) / len(cited) if cited else 0.0
            req = set(k["requiredEvidence"])
            Rr = len(cited & req) / len(req) if req else 1.0
            f1 = 2 * P * Rr / (P + Rr) if (P + Rr) > 0 else 0.0
            f1_w += w * f1
        c = g["confidence"] / 100.0
        y = 1.0 if correct else 0.0
        cal_sum += 1 - (c - y) ** 2
        calw_sum += w * (1 - (c - y) ** 2)
        n += 1
        if k["expected"] == "harmful" and g["answer"] in ("preferred", "acceptable"):
            harm += 1
        per_item.append((iid, x["type"], g["answer"], k["expected"], correct, c, w, f1, k.get("tags", [])))
    acc = acc_w / W; evf1 = f1_w / W; cal = (calw_sum / W) if cal_weighted else cal_sum / n
    reqs = []
    for q in out.get("evidenceRequests", []):
        qq = norm_q(q, lenient)
        if qq is not None and qq not in reqs:
            reqs.append(qq)
    reqs = reqs[:3]
    disc = set(truth["discriminatingRequests"]); low = set(truth["lowValueRequests"])
    rv = len(set(reqs) & disc) / min(3, len(disc)) - 0.5 * len(set(reqs) & low)
    rv = max(0.0, min(1.0, rv))
    total = 65 * acc + 15 * evf1 + 10 * cal + 10 * rv - 15 * harm
    res.update(score=max(0.0, total), acc=acc, evF1=evf1, cal=cal, req=rv, harm=harm, items=per_item,
               accOnly=100 * acc)
    return res


def all_runs(phases=("dev", "heldout", "haiku", "adapt2"), lenient=False, cal_weighted=False):
    cases = load_cases()
    rows = []
    for ph in phases:
        d = os.path.join(RAW, ph)
        for f in sorted(os.listdir(d)):
            r = json.load(open(os.path.join(d, f)))
            c, t = cases[r["caseId"]]
            s = score(c, t, r["output"], lenient, cal_weighted)
            s.update(runId=r["runId"], phase=ph, condition=r["condition"], caseId=r["caseId"],
                     trial=r["trial"], cls=c["benchmarkClass"], file=f, output=r["output"])
            rows.append(s)
    return rows


if __name__ == "__main__":
    import csv
    here = os.path.dirname(os.path.abspath(__file__))
    for lenient, cw, name in ((False, False, "replication-runs.csv"), (True, False, "replication-runs-lenientQ.csv"),
                              (False, True, "replication-runs-weightedcal.csv")):
        rows = all_runs(lenient=lenient, cal_weighted=cw)
        with open(os.path.join(here, name), "w", newline="") as fh:
            w = csv.writer(fh)
            w.writerow(["runId", "phase", "condition", "caseId", "class", "trial", "valid", "score", "acc", "evF1", "cal", "req", "harm", "errors"])
            for s in rows:
                w.writerow([s["runId"], s["phase"], s["condition"], s["caseId"], s["cls"], s["trial"], s["valid"],
                            "%.4f" % s["score"], "%.4f" % s["acc"], "%.4f" % s["evF1"], "%.4f" % s["cal"], "%.4f" % s["req"], s["harm"], s["errors"]])
        print(name, len(rows), "runs;", sum(not s["valid"] for s in rows), "invalid")
