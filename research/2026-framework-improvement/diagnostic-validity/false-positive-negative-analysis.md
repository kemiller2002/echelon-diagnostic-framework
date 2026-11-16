# False-positive and false-negative analysis

Source data: `experiments/scored/{dev,heldout,haiku}/runs.csv` and `items.csv`, and `experiments/scored/og/og-runs.csv`. Rates are pooled across runs with Wilson 95% intervals in the stats files.

## Definitions (benchmark-specification.md §5)

| Error | Operational definition |
|---|---|
| False diagnosis (FP) | Hypothesis whose correct answer is contradicted or unknown, answered `supported` |
| Missed problem (FN) | Hypothesis whose correct answer is `supported`, answered otherwise |
| Unsupported certainty | Correct answer `unknown`, answered supported or contradicted |
| Causal overclaim | Edge whose correct answer is not `causal`, answered `causal` |
| False abstention | SUFF expected `sufficient`, answered `insufficient` |
| Missed abstention | SUFF expected `insufficient`, answered `sufficient` |
| Harmful promotion | Action expected `harmful`, answered preferred or acceptable |
| Leadership-narrative adoption | Item tagged `leadership-narrative` whose correct answer ≠ supported, answered supported |

## Held-out rates (Sonnet, recognition)

| Error | A | B (EDF v0.3) | C | N | D |
|---|---|---|---|---|---|
| False diagnosis | 2/51 | 0/102 | 1/51 | 1/51 | 0/102 |
| Missed problem | 0/20 | 1/40 | 0/20 | 1/20 | 2/40 |
| Unsupported certainty | 4/22 | 2/44 | 2/22 | 2/22 | 2/44 |
| Causal overclaim | 1/32 | 1/64 | 0/32 | 1/32 | 0/64 |
| False abstention | 1/10 | 2/20 | 2/10 | 4/10 | 0/20 |
| Missed abstention | 0/2 | 0/4 | 0/2 | 0/2 | 0/4 |
| Harmful promotion | 0/22 | 0/44 | 0/22 | 0/22 | 0/44 |
| Leadership-narrative adoption | 1/50 | 0/100 | 1/50 | 0/50 | 0/100 |

Open generation, where analyzers must name problems themselves: missed real problems ranged 0–1/18 per condition, false diagnoses 0–1/49 and harmful recommendations 0 in every condition.

## Findings

1. **Classic false positives and false negatives are rare with curated evidence.** No condition adopted a leadership false narrative more than once, and none promoted a harmful action. The traps the mission worried about most (executive narratives, gamed metrics, silence) were mostly caught by every condition when the countering evidence was present in the file.
2. **The commonest error is over-caution, not over-claiming.** The most frequent held-out errors were:
   - premature actions labeled harmful (19 runs);
   - contradicted links labeled association-only (15);
   - sufficient evidence labeled insufficient (14).

   These reflect a bias toward hedging, which matches the "abstain when unsure" norm built into all prompts. **False abstention** varied most by method: 0/20 for D, 2/20 for B, 4/10 for N.
3. **Unsupported certainty is the main residual risk.** 9–18% of `unknown` items were answered decisively; on Haiku it was 18–23%. These are mostly hidden-problem and measurement-artifact items (DEV-C, HO-C, HO-J2), where analyzers resolved a genuinely open question. Tag-level error rates across all held-out runs: organizational-silence 13%, measurement-artifact 10%, causal-overclaim traps 9%, leadership-narrative 6%.
4. **Base rates.** Each case has 1–4 supported hypotheses among 4–6 candidates. Because the candidate list itself signals that some are wrong, specificity is inflated relative to open-ended practice. The open-generation arm reduces but does not remove this.
5. **Calibration.** High-confidence (≥ 80) errors occurred in 0–2.3% of high-confidence answers. Confidence is well calibrated at the top of the scale on these cases, but the cases are easy for these analyzers.

## What this does and does not show

- It shows that, **given the evidence, frontier models with or without EDF rarely make the gross diagnostic errors** EDF is meant to prevent.
- It does not show that EDF prevents these errors in practice. In real engagements the countering evidence is often absent, unrequested or politically filtered. The adaptive-evidence experiment shows that losing the decisive items costs 12–26 points.
- The truth keys are judgements by Claude-family authors and reviewers. Boundary errors (premature vs harmful) partly reflect truth-key granularity, not analyzer error. See the replication report's sensitivity analysis.
