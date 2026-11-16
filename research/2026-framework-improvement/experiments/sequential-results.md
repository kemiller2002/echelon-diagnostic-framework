# OD1-S — sequential evidence acquisition (results)

Preregistration: `preregistration-OD1S.json`, committed `d4c9a90` before any run. Raw outputs: `raw/seq/`, journals `raw/seq-journals/`. Scores: `scored/seq/` (`runs.csv`, `reads.csv`, `stats.md`, `analysis.md`). Held-out cases, Sonnet tier, 96 runs.

## Integrity

- **Tool audit.** 1,079 Read calls: 96 task-file reads and 983 evidence reads. **None** fell outside the case's own evidence directory, and no other tools were used.
- **Request menus.** Shuffled and relabeled X1..Xn per run (fix R-5). Maps are stored in each raw record.
- **Truth sealing (protocol deviation).** Held-out truth files were sealed at launch. Because sealed (deleted) files repeatedly tripped the repository's uncommitted-changes hook, they were restored to disk at 13:08:44 UTC, while 82 of the 96 runs were still in progress. Hiding the deletions from git was refused as undermining oversight, so restoring them was the correct course. Truth files are not in any evidence directory, and the transcript audit above shows no run read them.
- **Ingest bug.** A harness bug truncated JSON when attaching request maps. It was fixed (`Substring` instead of `TrimEnd`), and raw records were regenerated from the journals before any scoring. All 96 parse and score as valid.

## Results

| Condition | Runs | Score | Items read | Share of catalogue | Required-evidence coverage | Full-evidence score | Δ vs full [95% CI] |
|---|---|---|---|---|---|---|---|
| General AI, free stopping | 24 | 91.8 | 11.8 | 70% | 82% | 93.1 | −1.3 [−3.5, 1.9] |
| EDF v0.3, free | 12 | 91.9 | 12.3 | 73% | 87% | 94.5 | −2.5 [−6.6, 2.1] |
| KT + Nadler-Tushman, free | 12 | 90.9 | 10.1 | **60%** | 75% | 93.9 | −2.9 [−6.9, 1.2] |
| Variant D, free | 24 | 93.2 | 11.0 | 66% | 80% | 95.9 | −2.7 [−5.2, −0.6] |
| General AI, budget 6 | 12 | 85.7 | 6.5 | 39% | 51% | 93.1 | −7.3 [−11.7, −3.2] |
| Variant D, budget 6 | 12 | 89.9 | 7.4 | 44% | 59% | 95.9 | −6.1 [−9.9, −2.8] |

Test–retest item agreement (two trials): general AI 96.0%, D 97.0%.

**Budget compliance.** General AI exceeded 6 items in 4 of 12 runs (reads: 7, 8, 8, 8). Variant D exceeded it in **10 of 12** (reads 7–9).

### Between methods (free stopping)

| Contrast | Score | Items read |
|---|---|---|
| D − general AI | +1.5 [−0.3, 3.4] | −0.7 [−1.7, 0.3] |
| EDF v0.3 − general AI | +0.2 [−1.1, 1.2] | +0.6 [−0.5, 2.0] |
| KT + NT − general AI | −0.8 [−2.6, 0.6] | **−1.7 [−2.6, −0.7]** |
| D − EDF v0.3 | +1.3 [−1.0, 3.8] | −1.3 [−2.9, 0.2] |

### Sequential vs batch selection at k = 6

| Contrast | All runs | Budget-compliant runs only |
|---|---|---|
| General AI: sequential − batch (84.4) | +1.3 [−2.7, 5.7] | −1.7 [−5.0, 1.6] (8 runs) |
| D: sequential − batch (84.2) | +5.7 [0.8, 10.0] — **confounded**: 10/12 runs read 7–9 items | +7.3 [−0.4, 15.1] (2 runs; uninformative) |

## Preregistered verdicts

| Hypothesis | Criterion | Result | Verdict |
|---|---|---|---|
| HS-1: free sequential approaches full-evidence accuracy with ≤ 60% of items | Δ ≥ −3 and share ≤ 60% (A and D) | A: −1.3 with 70% read; D: −2.7 with 66% read | **Not supported.** Accuracy is nearly retained, but analyzers read two-thirds of the evidence. (Exploratory: KT + NT met both criteria, −2.9 at 60%.) |
| HS-2: sequential beats batch at budget 6 | ≥ +3, CI > 0 | General AI +1.3 [−2.7, 5.7]; D confounded by budget violation | **Not supported / inconclusive** |
| HS-3: EDF-derived methods acquire more efficiently than general AI | equal score (± 2) with ≥ 2 fewer items, or higher score with no more items | EDF v0.3: no. D: −0.7 items, +1.5 score (n.s.). KT + NT: −1.7 items [−2.6, −0.7] at −0.8 score. | **Not supported for EDF.** The established method (KT + NT) was the most evidence-efficient, though just short of the preregistered 2-item threshold. |
| HS-4: explicit sufficiency rules reduce over- and under-collection | descriptive | Items read are uncorrelated with case difficulty (r = −0.24 to +0.08); all methods read 60–73% of evidence | **Not supported.** Stopping does not adapt to difficulty under any method. |

## Interpretation

1. **Sequential acquisition with free stopping nearly preserves full-evidence accuracy** (−1 to −3 points) while skipping about 30–40% of the evidence. Analyzers over-collect, and how much they read does not track how hard the case is. No method, EDF included, produced adaptive stopping.
2. **The established Kepner-Tregoe method was the most evidence-efficient**: 1.7 fewer items than the general prompt for no significant accuracy loss. This adds to OD1's signal that KT's contrast-based specification (IS / IS-NOT) is the most diagnostic procedure tested.
3. **New failure mode, budget non-compliance (FT-20).** D's instruction to choose evidence for discriminating power coincided with breaking an explicit collection limit in 10 of 12 runs, against 4 of 12 for the general prompt. In practice this is the analogue of a consultant exceeding an agreed data-request scope. Any EDF evidence-acquisition rule must make budget and consent limits explicit constraints, not just preferences.
4. Sequential acquisition did not beat one-shot batch selection at the same budget in budget-compliant runs. The batch experiment's ~12-point loss at k = 6 is mostly a budget effect, not a selection-strategy effect.
