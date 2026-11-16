# Baseline results — EX-EDF-2026-OD1

Baseline: EDF v0.3 at commit `41fea6f`, embedded verbatim (`experiments/prompts/B_EDF03.module.txt`).
Analyzers: Claude Code workflow subagents. The model tiers were Sonnet (primary) and Haiku (replication); exact version strings are not exposed.
Scoring: deterministic (`harness/Od1.fsx`), 0–100 per case run (spec §5). Unit of analysis: the case, with trials averaged and conditions paired by case.
CIs are seeded paired bootstraps (10,000 resamples). Full tables: `experiments/scored/{dev,heldout,haiku}/stats.md`.

## Conditions compared

| Id | Mission approach | What it is | Prompt chars (held-out mean) |
|---|---|---|---|
| A_GENERAL | A — general-purpose AI | shared task frame + "think carefully" | 20,830 |
| B_EDF03 | B — current EDF | v0.3 spec verbatim | 26,061 |
| B0_EDF0 | simplification probe | EDF-0 quick card verbatim | 23,471 |
| C_KTNT | C — established method | Kepner-Tregoe problem/decision analysis + Nadler-Tushman congruence | 23,946 |
| N_NEUTRAL | matched-structure control | generic 10-step review protocol | 23,775 |

Every condition shares the ~1.9k-char task frame and the case file (~18–21k chars). The case dominates prompt size.

## Recognition format — mean score by condition

| Condition | Dev (10 cases, Sonnet) | Held-out (12 cases, Sonnet) | Held-out (12 cases, Haiku) |
|---|---|---|---|
| A_GENERAL | 91.4 | 93.1 | **94.3** |
| B_EDF03 | 93.5 | 94.5 (2 trials) | 92.7 |
| B0_EDF0 | 93.1 | 92.4 | – |
| C_KTNT | 92.8 | 93.9 | – |
| N_NEUTRAL | **94.8** | 91.7 | 87.2 |

## Paired contrasts against the baseline

| Contrast | Dev (Sonnet) | Held-out (Sonnet) | Held-out (Haiku) |
|---|---|---|---|
| B − A (EDF vs general AI) | +2.1 [−0.3, 5.3] | +1.4 [−0.7, 3.1] | −1.6 [−3.9, 0.9] |
| B − N (EDF vs matched structure) | −1.3 [−5.3, 3.0] | +2.8 [0.8, 5.3] | +5.4 [1.2, 9.6] |
| C − B (established method vs EDF) | −0.7 [−3.2, 1.4] | −0.6 [−2.0, 0.7] | – |
| B0 − B (EDF-0 vs full EDF) | −0.4 [−2.0, 1.3] | −2.1 [−3.9, −0.5] | – |
| N − A | +3.4 [0.7, 6.4] | −1.4 [−3.2, −0.1] | −7.0 [−11.2, −3.2] |

## Open-generation format (held-out, Sonnet, blind grader)

| Condition | OG score | Item accuracy | Discriminating-request hit | Missed real problems | False diagnoses | Sufficiency correct |
|---|---|---|---|---|---|---|
| A_GENERAL | 85.7 | 0.869 | 0.75 | 0/18 | 1/49 | 11/12 |
| B_EDF03 | 87.4 | 0.891 | 0.72 | 1/18 | 0/49 | 12/12 |
| C_KTNT | **90.0** | 0.911 | 0.81 | 1/18 | 0/49 | 12/12 |
| N_NEUTRAL | 87.6 | 0.885 | 0.79 | 0/18 | 0/49 | 12/12 |

The paired contrasts B − A (+1.7 [−1.5, 5.8]), B − N (−0.2 [−3.9, 3.6]) and C − B (+2.6 [−3.3, 7.5]) all have CIs that include 0.

## Preregistered hypotheses for the baseline

| Hypothesis | Criterion | Result | Verdict |
|---|---|---|---|
| H-OD1-1: B outperforms A | mean ≥ +5 and CI > 0 | +1.4 [−0.7, 3.1] Sonnet; −1.6 Haiku; +1.7 OG | **Not supported** |
| H-OD1-2: B adds value beyond matched structure | mean ≥ +5 and CI > 0 | +2.8 [0.8, 5.3] Sonnet (CI > 0, below margin); +5.4 [1.2, 9.6] Haiku; −0.2 OG; −1.3 dev | **Not supported** at the preregistered margin. The direction is inconsistent across phases. Where B beats N, it is because N under-performs the general-AI prompt, not because B beats it. |
| H-OD1-3: C differs from B | two-sided | −0.6 [−2.0, 0.7] recognition; +2.6 [−3.3, 7.5] OG | **No detectable difference** |
| H-OD1-4: EDF-0 non-inferior to EDF v0.3 | mean > −3 | −2.1 [−3.9, −0.5] held-out; −0.4 dev | **Inconclusive.** The point estimate passes but the CI lower bound crosses the margin. |
| H-OD1-9: recognition format saturates | condition means within 5 points | Sonnet held-out range 91.7–95.9; Haiku 87.2–94.3 | **Supported** (descriptive) |

## Interpretation

1. **Current EDF v0.3 shows no material diagnostic advantage over a general-purpose prompt** on organizational cases. This holds across recognition and open generation and across two model tiers. This replicates, in a new domain and with the verbatim specification, the EX-EDF-001 null result (which had used a paraphrased "EDF" prompt).
2. **Generic structure can hurt.** The 10-step neutral protocol was the worst held-out condition on both tiers, and markedly so on Haiku (−7.0 vs A). Elaborate procedure in the prompt is not free and can displace attention from the evidence.
3. **The established method (Kepner-Tregoe + Nadler-Tushman) is at least as good as EDF.** In open generation it was nominally the best condition. That is not significant, but it is consistent with KT's IS/IS-NOT contrast testing being the most explicitly *discriminating* procedure among the methods tested.
4. **Ceiling.** With curated evidence and ≤18 evidence items, Sonnet- and Haiku-tier analyzers get ~90–95% of items right whatever the method. The benchmark therefore mostly measures boundary judgements: premature vs harmful, contradicted vs association-only, and over-abstention. See `diagnostic-validity/false-positive-negative-analysis.md`.

## Validity limits of these results

The same model family authored the cases, reviewed them and analyzed them. All cases are synthetic, with 10 dev and 12 held-out cases, so power is low. Trials: 1 per condition except B and D (2). The candidate-recognition format offers the true answer among the options. See `experiments/replication-results.md` for the independent re-analysis.
