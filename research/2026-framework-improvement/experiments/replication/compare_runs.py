import csv, os, sys
here = os.path.dirname(os.path.abspath(__file__)); E = os.path.dirname(here)
mine = {r["runId"]: r for r in csv.DictReader(open(os.path.join(here, sys.argv[1] if len(sys.argv) > 1 else "replication-runs.csv")))}
theirs = {}
for ph in ("dev", "heldout", "haiku", "adapt2"):
    for r in csv.DictReader(open(os.path.join(E, "scored", ph, "runs.csv"))):
        theirs[r["runId"]] = r
print("mine", len(mine), "theirs", len(theirs), "only mine", len(set(mine) - set(theirs)), "only theirs", len(set(theirs) - set(mine)))
out = []
for comp, tc in (("score", "score"), ("acc", "acc"), ("evF1", "evF1"), ("cal", "cal"), ("req", "req"), ("harm", "harmCount")):
    diffs = [(abs(float(mine[k][comp]) - float(theirs[k][tc])), k) for k in mine if k in theirs]
    diffs.sort(reverse=True)
    print(comp, "max|diff|=%.4f" % diffs[0][0], "n>0.01:", sum(d > 0.01 for d, _ in diffs), diffs[:3])
with open(os.path.join(here, "run-score-comparison.csv"), "w", newline="") as fh:
    w = csv.writer(fh); w.writerow(["runId", "mine_score", "orch_score", "diff", "mine_req", "orch_req", "mine_evF1", "orch_evF1"])
    for k in sorted(mine):
        if k in theirs:
            w.writerow([k, mine[k]["score"], theirs[k]["score"], "%.4f" % (float(mine[k]["score"]) - float(theirs[k]["score"])), mine[k]["req"], theirs[k]["req"], mine[k]["evF1"], theirs[k]["evF1"]])
