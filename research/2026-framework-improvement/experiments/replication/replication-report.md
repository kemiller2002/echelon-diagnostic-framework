# EX-EDF-2026-OD1: independent replication and statistical review (Agent O)

Date: 2026-11-16. Scope: re-derive every primary number from the frozen cases, truth keys and raw analyzer outputs, and look for errors, fragile results and validity threats.

Independence: I wrote my scorer (`od1_score.py`) from `benchmark-specification.md` §5 alone and compared its output with `experiments/scored/*/runs.csv`. I read `harness/Od1.fsx` only after that comparison, to find where the discrepancy came from. I did not open anything under `strategy/` or `improvements/`. Everything runs with `python3 -I` and the standard library only.

| file | purpose |
|---|---|
| `od1_score.py` | independent §5 scorer. Writes `replication-runs.csv` (spec-literal), `replication-runs-lenientQ.csv` and `replication-runs-weightedcal.csv` (orchestrator convention) |
| `compare_runs.py` | per-run comparison with the orchestrator. Writes `run-score-comparison.csv` |
| `od1_stats.py` | paired bootstrap, exact sign-flip test, Holm adjustment, sensitivity analyses and power. Writes `stats-output.md`, `contrasts.json` and `contrasts-per-case.csv` |
| `od1_secondary.py` | pooled safety and action metrics with Wilson intervals. Writes `secondary-output.md` |
| `od1_audit.py` | leakage and bias audit. Writes `audit-output.md` |
| `og_score.py` | §8 open-generation re-score and grader agreement. Writes `og-output.md` and `og-replication-runs.csv` |

---

## 1. Frozen hashes

- `benchmarks/frozen-cases-dev.sha256`: 30/30 files OK.
- `benchmarks/frozen-cases-heldout.sha256`: 36/36 files OK.
- Each manifest lists every case, truth and review file in `benchmarks/cases/`: 66 files, none extra and none missing.
- `experiments/frozen-prompts-{dev,heldout}.sha256`: every listed prompt is OK.
- `SHARED_OG.txt`, the shared prompt for the open-generation arm, is in neither prompt manifest. It is therefore not hash-frozen. This is a minor gap in the audit trail.
- I ran no git commands, so I cannot verify commit order (freeze before run).

## 2. Independent scoring of all 320 recognition-format runs

The 320 runs are dev 80, held-out 144, haiku 48 and adapt2 48. Every run parses, answers every scored item exactly once with an allowed label and gives a valid confidence. Under my validity rule no run is schema-invalid.

| component | max abs diff, mine vs orchestrator |
|---|---|
| weighted accuracy | 0.0000 |
| evidence F1 | 0.0000 |
| request value | 0.0000 |
| harmful count | 0 |
| **calibration** | **0.0386** (308/320 runs differ) |
| **total score** | **0.386 points** (`adapt2/SEL_D_EDFX/OD-HO-E`) |

**Discrepancy 1 (calibration weighting): the orchestrator deviates from the spec.**
- §5 says calibration is the "mean over items of `1 − (c − y)²`".
- `Od1.fsx` `scoreRun` instead computes `wsum(...)/wtot`, which weights each item by its truth weight.
- When I switch to weight-weighted calibration (`replication-runs-weightedcal.csv`), all 320 runs match to within 0.0001 (rounding). This is the only cause of the score differences.
- **Impact:** at most 0.39 points on any run and no more than 0.02 points on any contrast mean.
- One win/loss tally changes. Under the spec rule held-out D−B is 6 wins / 6 losses, against the orchestrator's 7/5. The flip is OD-HO-K, where the difference is −0.010 under the spec rule and +0.009 under the orchestrator's.

**Discrepancy 2 (latent; does not bite on this data): a weak validity rule in `Od1.fsx`.**
- The rule is `valid = run.Answers.IsSome`.
- Missing items count as wrong rather than making the output schema-invalid (score 0).
- A missing confidence defaults to 50, and out-of-range confidences are clamped.
- Off-list answers are lower-cased and scored as wrong.
- This is more lenient than "schema-invalid outputs are terminal and score 0". No current run triggers any of these paths, so no number changes, but the rule should be tightened or documented.

**Discrepancy 3 (interpretation; haiku N_NEUTRAL only): request IDs given as full titles.**
- Two runs (`haiku/N_NEUTRAL` on OD-HO-E and OD-HO-I) returned request IDs as `"Q1: <title>"`.
- Both implementations (strict matching) score request value 0 for these.
- Parsing out the leading `Qn` adds about 10 points to each run.
- Effect on haiku N−A: −7.04 under strict matching, −5.37 under lenient parsing.
- I treat strict matching as correct under the spec. Either way, the haiku N_NEUTRAL deficit is partly a formatting artefact.

Other interpretation choices agree between the two implementations:
- F1 precision is taken against allowed evidence and recall against required evidence, with weighted aggregation.
- Request value takes the first three distinct valid IDs, clamped to [0, 1].
- The harmful penalty applies to the total, floored at 0.

## 3. Paired contrasts

Method:
- Unit of analysis is the case, with trials averaged.
- 95% intervals are percentile bootstrap: 10,000 resamples, seed 20261116, Python RNG (the orchestrator used .NET).
- p-values are exact two-sided sign-flip tests over all 2ⁿ sign patterns.
- Holm adjustment is across all 15 contrasts.
- Scores use the spec-literal (unweighted) calibration.

| phase | contrast | n | diff | 95% CI | SD diff | dz | p (sign-flip) | Holm p | W/L |
|---|---|---|---|---|---|---|---|---|---|
| held-out | B_EDF03 − A_GENERAL | 12 | +1.40 | [−0.72, +3.13] | 3.59 | +0.39 | 0.203 | 1.00 | 9/3 |
| held-out | B_EDF03 − N_NEUTRAL | 12 | +2.77 | [+0.76, +5.33] | 4.23 | +0.66 | 0.020 | 0.254 | 10/2 |
| held-out | D_EDFX − B_EDF03 | 12 | +1.48 | [−0.31, +3.59] | 3.63 | +0.41 | 0.210 | 1.00 | 6/6 |
| held-out | D_EDFX − N_NEUTRAL | 12 | +4.25 | [+1.53, +7.90] | 5.82 | +0.73 | 0.006 | 0.095 | 11/1 |
| held-out | C_KTNT − B_EDF03 | 12 | −0.59 | [−2.02, +0.68] | 2.49 | −0.24 | 0.464 | 1.00 | 8/4 |
| held-out | B0_EDF0 − B_EDF03 | 12 | −2.10 | [−3.92, −0.48] | 3.20 | −0.66 | 0.055 | 0.662 | 3/9 |
| held-out | B_CLARITY − B_EDF03 | 12 | −0.80 | [−2.25, +0.79] | 2.80 | −0.29 | 0.335 | 1.00 | 3/9 |
| held-out | BI_CLARITY − B_EDF03 | 12 | −0.04 | [−1.69, +1.62] | 3.09 | −0.01 | 0.977 | 1.00 | 6/6 |
| held-out | D_EDFX − D_NOGATE | 12 | +2.80 | [+0.44, +5.65] | 4.85 | +0.58 | 0.056 | 0.662 | 8/4 |
| held-out | D_EDFX − D_NOORG | 12 | +1.53 | [−0.23, +3.62] | 3.55 | +0.43 | 0.190 | 1.00 | 7/5 |
| haiku | B_EDF03 − A_GENERAL | 12 | −1.60 | [−3.92, +0.94] | 4.44 | −0.36 | 0.221 | 1.00 | 4/8 |
| haiku | D_EDFX − B_EDF03 | 12 | +0.13 | [−1.94, +2.37] | 4.03 | +0.03 | 0.950 | 1.00 | 4/8 |
| haiku | N_NEUTRAL − A_GENERAL | 12 | −7.04 | [−11.16, −3.14] | 7.31 | −0.96 | 0.006 | 0.095 | 2/10 |
| dev | B_EDF03 − A_GENERAL | 10 | +2.10 | [−0.35, +5.24] | 4.84 | +0.43 | 0.207 | 1.00 | 6/4 |
| dev | B_EDF03 − N_NEUTRAL | 10 | −1.25 | [−5.39, +3.08] | 7.14 | −0.18 | 0.565 | 1.00 | 3/7 |

**My numbers match the orchestrator's `stats.md`.** The means agree to within 0.02 and the CI bounds to within about 0.05; the small gaps come from the calibration convention and the different RNG.

**No contrast survives Holm correction at α = 0.05.** The smallest adjusted p is 0.095. The preregistration specified no multiplicity correction, but the unadjusted "CI excludes 0" findings are fragile:
- D−N and B−N exclude 0 by bootstrap.
- With n = 12, the minimum achievable two-sided sign-flip p is 2/4096. Each p above rests on two or three cases.

### Verdicts against the preregistered criteria

These come from my numbers. Primary contrasts are held-out unless noted.

| hypothesis | criterion | replicated result | verdict |
|---|---|---|---|
| H-OD1-1 (B > A) | mean ≥ +5 and CI > 0 | +1.40, CI includes 0 | **Not supported. Replicates.** |
| H-OD1-2 (B > N) | mean ≥ +5 and CI > 0 | +2.77, CI > 0, but below +5 and Holm p 0.25 | **Not supported (mean < +5). Replicates.** Small positive B−N effect, not robust to multiplicity. |
| H-OD1-3 (C vs B) | two-sided | −0.59 [−2.02, +0.68] | No difference. Replicates. |
| H-OD1-4 (B0 non-inferior, > −3) | mean > −3 | held-out −2.10 (CI to −3.92); dev −0.39 | Point estimate meets the criterion; the CI does not exclude −3. **Weak or inconclusive.** |
| H-OD1-5 (D > B) | mean ≥ +5, CI > 0, both tiers | Sonnet +1.48 (CI includes 0); haiku +0.13 | **Not supported. Replicates.** The EDF-specific test D−N = +4.25 [+1.53, +7.90] has CI > 0, but because D−B is null it does not rescue H5. |
| H-OD1-6 (Clarity) | action accuracy higher, harm not higher | action accuracy B 0.952, B_CLARITY 0.968, BI_CLARITY 0.968; harmful promotion 0 everywhere; total score −0.80 and −0.04 | Exploratory. Action accuracy is +1 item in 62 and within noise. **Inconclusive.** |
| H-OD1-7 (D components) | removal lowers score ≥ 2 | NOGATE +2.80 (meets); NOORG +1.53 (does not) | Replicates as exploratory. |
| H-OD1-8 (OG D > B) | mean ≥ +5 and CI > 0 | +0.89 [−4.38, +6.51] (section 7) | **Not supported. Replicates.** |
| H-OD1-9 (saturation) | held-out condition means within 5 points | 91.69 to 95.95 (spread 4.26) | **Supported. Replicates.** |

Further results:
- **Safety secondaries** (`secondary-output.md`): harmful-action promotion is 0 in every condition and tier, and premature promotion is 0 everywhere. The −15 penalty never fires, so these metrics are at floor and cannot discriminate.
- **Where D's advantage comes from:** D_EDFX has the best abstention (SUFF) accuracy, 24/24, against N_NEUTRAL 8/12 and B 22/24. Sufficiency and edge items account for most of the D−N and B−N gaps:
  - D−N accuracy contribution: edge +1.24, SUFF +1.47, action +0.73, hypothesis −0.02.
  - B−N accuracy contribution: edge +0.84, SUFF +1.07, action +0.30, hypothesis −0.17.

## 4. Sensitivity analyses

**(a) Items that every condition answered wrong (possible truth-key ambiguity).**
- Dev: OD-DEV-I/I7 is wrong in all 8 dev runs. The key says *contradicted*; every run answered *association-only*.
- Held-out: OD-HO-J2/I7 is wrong in all 144 Sonnet held-out runs, every one answering the identical *association-only* against a key of *contradicted*. It is the only held-out item wrong in ≥ 90% of runs.
- The same contradicted-versus-association-only boundary appears in both splits, which is consistent with the dev observation recorded in Amendment 2.
- Removing OD-HO-J2/I7 from all scoring components changes no contrast by more than 0.05:
  - B−A: +1.40
  - B−N: +2.77 [+0.75, +5.32]
  - D−B: +1.53 [−0.31, +3.72]
  - D−N: +4.30 [+1.55, +7.95]
  - Haiku contrasts: −1.60, +0.11, −6.58
- Every verdict is unchanged. The ambiguous item lowers all conditions equally, so it compresses scores but does not bias contrasts.

**(b) Accuracy-only score (100 × weighted accuracy).**
- Contrasts grow slightly:
  - B−A +1.59
  - B−N +3.13 [+0.81, +6.18]
  - D−B +2.13 [+0.02, +4.67]
  - D−N +5.26 [+2.12, +9.60]
  - D−NOGATE +3.56
  - Haiku: B−A −2.47 [−4.92, −0.31], D−B +0.23, N−A −6.13
- **This is the one place where a headline result moves:**
  - Accuracy-only D−N crosses +5.
  - Accuracy-only D−B has a CI that barely excludes 0 (lower bound +0.02). Its sign-flip p is 0.156, so it is not robust.
  - Haiku B−A becomes significantly negative by bootstrap (p 0.0625).
- The secondary components (evidence F1, calibration and requests) dilute the accuracy differences. The request component is nearly constant (section 6). No verdict flips under the preregistered criteria, except that D−N on accuracy alone would meet the "+5 and CI > 0" bar. That bar is not the preregistered outcome.

**(c) Per-class breakdown.** There is one case per class, and two for J. The full table is in `stats-output.md`.
- Held-out D−B is driven by three cases: D (+5.94), E (+7.17) and J2 (+8.41). Six cases fall within ±0.5. There are losses on F (−2.35) and J1 (−2.03).
- Held-out B−A is driven by C, D, G, I and J1 (+3 to +5.6 each), against E (−7.67).
- Haiku B−A is negative in classes A to E (−4 to −9) and positive in F (+8.6).
- Classes do not replicate across tiers. Haiku D−B on class D is +8.96 (same sign as Sonnet), but on class E it is −1.01 (opposite sign).
- With one case per class, per-class effects are anecdotes and cannot be separated from case idiosyncrasy.

## 5. Power and noise

| contrast | SD of case differences | n for +5 at 80% power | power for +5 at n = 12 |
|---|---|---|---|
| held-out B−A | 3.59 | 6 | 0.99 |
| held-out B−N | 4.23 | 8 | 0.96 |
| held-out D−B | 3.63 | 6 | 0.99 |
| held-out D−N | 5.82 | 13 | 0.77 |
| held-out D−NOGATE | 4.85 | 10 | 0.90 |
| haiku N−A | 7.31 | 19 | 0.57 |
| dev B−N | 7.14 | 18 | 0.48 |

Power is for a paired t test, α = 0.05, using the observed SD.

**Is the 12-case set adequate for a true +5 effect?**
- For most contrasts, yes.
- Detecting +3 would need 13 to 32 cases.
- The SD estimate from n = 12 is itself uncertain: the upper 95% bound is about 1.7 times the point estimate. Under that bound the requirement roughly triples, to about 15 to 40 cases for +5.

**The binding constraint is the ceiling, not n.**
- The preregistered support rule is "mean ≥ +5 and CI > 0". Even with a true effect of exactly +5, the chance of observing a mean ≥ +5 is only about 50%.
- B averages 94.5 held-out, so D−B can be at most +5.5. Reaching it would require D to score about 100 on every case.
- The +5 thresholds for D−B, B−A and B−N were therefore close to unattainable on this instrument. A null on these hypotheses is mostly a statement about saturation, not evidence that the methods are equivalent.

**Run-to-run noise is as large as the effects.**
- Item-level test–retest agreement is 0.975 for both B and D.
- The trial-1 minus trial-2 score difference has SD 3.33 for B and 4.33 for D. The mean absolute difference is 1.89 for B and 2.75 for D.
- Those values equal or exceed most between-condition differences.
- A, C and N have one trial each, while B and D have two, so contrasts involving A, C or N carry more noise on one side.

## 6. Leakage and bias audit

The full output is in `audit-output.md`.

1. **The evidence-request menu order leaks the key. This is the most concrete measurement defect.**
   - Discriminating requests sit at menu positions Q1 (20 of 22 cases), Q2 (21/22), Q3 (12/22), Q5 (1/22) and Q6 (3/22).
   - Low-value requests are mostly Q4 and Q5.
   - In 18 of 22 cases the discriminating set is exactly the first k menu items.
   - A content-blind "always request Q1, Q2, Q3" strategy earns a request value of 0.795 out of 1.
   - The share of runs that requested exactly {Q1, Q2, Q3}: 119/144 held-out, 32/48 haiku and 32/80 dev.
   - The 10-point request component therefore rewards position-following and carries almost no method signal. It should be re-randomized, or reported as non-diagnostic.
2. **Ceiling or saturation.**
   - Held-out condition means range from 91.7 to 96.0.
   - 33% to 67% of runs score ≥ 95.
   - Mean item accuracy is 0.925 to 0.977.
   - Across all condition pairs, 54% of same-case runs have identical answer vectors.
   - Contrasts are therefore decided by a handful of items per case, often the sufficiency item and one or two edge items.
3. **Copied truth text.**
   - I searched for 8-word strings that appear in an analyzer output and in the truth narrative or rationales but nowhere in the case file. They occur in 24 of 332 outputs.
   - At 12 words, 5 outputs match.
   - Every hit is a natural recombination of case facts (for example "the PeopleCore manager field is stale for the seven branches"). The hits are spread across all conditions, including A_GENERAL.
   - No evidence of truth-key exposure.
4. **Identical outputs.** No output is byte-identical to another, either across conditions or between trials. No caching or copy artefacts.
5. **Schema anomalies.** All of these are scored identically by both implementations:
   - Two haiku N_NEUTRAL runs with titled request IDs (section 2).
   - One haiku run with a duplicated evidence ID.
   - One adapt2 answer containing an extra `evidenceRequests` key.
   - 29 adapt2 answers with empty `evidenceIds` (consistent with the restricted-evidence design).
   - Many summaries over the 120-word limit (dev 40, held-out 57, haiku 23 runs). The limit is not enforced. This is harmless to the primary score but means the contract is unchecked.
   - No item IDs are missing, duplicated or invalid.
6. **Author and analyzer family.**
   - The spec and preregistration state that the case authors, blind reviewers, analyzers and OG graders are all Claude-family models.
   - The data contain no author or model metadata, so I cannot verify which models were used.
   - Author–analyzer affinity is untestable here and may inflate absolute scores. It also makes the saturation partly circular: Claude reads cases the way Claude wrote them.
7. **Does the key's vocabulary favour a condition?**
   - I measured how often each prompt module's distinctive words appear in held-out truth rationales but nowhere in the held-out case text.
   - D_EDFX scores highest on held-out keys: 9.1 per 1,000 tokens, word coverage 0.60. B_EDF03 scores 5.1 and 0.41, N_NEUTRAL 5.3 and 0.58, A_GENERAL 0.
   - Every non-D module matches dev keys more than held-out keys (B 10.4 vs 5.1, N 13.1 vs 5.3). D_EDFX is the only module whose match is higher on held-out keys (9.1) than on the dev keys it was designed from (7.5). D_NOGATE shows the same pattern (8.1 vs 6.7).
   - The overlapping words are generic: *explains, direct, controls, missing, silence, strategy*.
   - D-only words that appear in held-out cases but in no dev case and no other module (*allow, damage, deterioration, easy, grade, intentional, miss, supposed*) are also generic.
   - **Interpretation:** this is not evidence of held-out tuning. It is a weak signal that D's wording ("missing evidence makes a claim unknown", "silence is not health", "intentional tradeoff") echoes the reasoning in the truth keys, so D may partly be learning the key-writers' rubric. That is expected when D is designed from dev failures of the same authoring process, but it makes the result less likely to transfer to other key-writers or to real cases.
8. **Batch confound.** Held-out B_EDF03 and D_EDFX (`phase: heldout`, 48 runs) were executed in a separate batch from A, C, N, B0, the Clarity variants and the D ablations (`phase: heldout1`, 96 runs). Any drift in the model alias or host between batches is confounded with the B and D versus others contrasts. B−A, B−N and D−N are cross-batch; D−B is within-batch.
9. **Deviations from the preregistration.**
   - Haiku N_NEUTRAL was not in the preregistered replication phase (A, B, D). It was added, and it produces the largest haiku effect.
   - Held-out trials were reduced from 2 to 1 for all conditions except B and D (Amendment 2).
   - Both changes are disclosed, but the haiku N−A result is post-hoc.

## 7. Open-generation arm (§8)

I re-scored all 80 gradings (60 by g1, 20 by g2) directly from the four journals and `grade-map.txt`.
- No early-stage items were sent to graders.
- No scorable item is missing from any grading.
- My OG scores equal `scored/og/og-runs.csv` exactly (max diff 0.0000).

The spec says items the grader omitted count as *not-addressed*, while `Od1.fsx` drops them from the denominator. On this data the two rules give identical results.

| condition (g1) | n | OG score | item accuracy | request hit rate |
|---|---|---|---|---|
| A_GENERAL | 12 | 85.75 | 0.869 | 0.750 |
| B_EDF03 | 12 | 87.44 | 0.891 | 0.722 |
| C_KTNT | 12 | 90.05 | 0.911 | 0.806 |
| D_EDFX | 12 | 88.33 | 0.900 | 0.736 |
| N_NEUTRAL | 12 | 87.60 | 0.885 | 0.792 |

Paired OG contrasts, using g1 only:

| contrast | diff | 95% CI | p | dz |
|---|---|---|---|---|
| D−B | +0.89 | [−4.38, +6.51] | 0.73 | 0.09 |
| B−A | +1.69 | [−1.47, +5.85] | 0.50 | 0.25 |
| B−N | −0.16 | [−3.70, +3.65] | 0.94 | −0.02 |
| D−N | +0.73 | [−3.90, +5.25] | 0.78 | 0.09 |
| D−A | +2.58 | [−3.22, +9.28] | 0.49 | 0.23 |
| C−B | +2.60 | [−3.46, +7.55] | 0.39 | 0.26 |

- **H-OD1-8 is not supported.** None of the OG contrasts is significant, before or after Holm.
- The case-level SD of OG differences is 7 to 11, so OG needs about 15 to 40 cases to detect +5. **The 12-case OG arm is underpowered.**
- The Sonnet recognition-format D−N advantage (+4.25) **does not carry over to open generation** (+0.73).
- No condition was ever graded as recommending a harmful action in OG.

**Agreement between graders g1 and g2.** 20 runs were double-graded, giving 316 item readings.

| measure | value |
|---|---|
| raw agreement | 0.956 |
| Cohen's κ | 0.949 |
| κ, actions | 0.90 |
| κ, edges | 0.94 |
| κ, hypotheses | 0.96 |
| κ, sufficiency | 1.00 |
| mean abs run-score difference | 1.92 (max 12.0) |
| Pearson r, run scores | 0.90 |

- The commonest disagreements are acceptable→harmful, acceptable→preferred and association-only→unknown.
- The double-graded subset is unbalanced: 7 N_NEUTRAL, 5 B, 4 D, 3 C and 1 A.
- Both graders are Sonnet-tier Claude, so their agreement does not demonstrate validity.
- Grader noise (mean absolute run-score difference about 2 points, maximum 12) is comparable to the OG effects being tested.
- Blinding: one C_KTNT OG output contains "congruence", which reveals the method. No other method names appear.

## 8. Verdict

- **Scoring replicates.**
  - Every accuracy, evidence-F1, request and harm number is reproduced exactly.
  - The only discrepancy is a spec deviation in `Od1.fsx`: calibration is weighted by item weight instead of the specified unweighted mean. It moves no score by more than 0.39 points or any contrast by more than 0.02, and it flips one win/loss tally (D−B 7/5 → 6/6).
  - The OG scores replicate exactly.
- **The headline conclusions replicate:**
  - No support for B > A, B > N (≥ +5), D > B, or D > B in open generation.
  - Saturation is supported.
  - The only intervals excluding zero (B−N +2.8, D−N +4.3, B0−B −2.1, D−NOGATE +2.8, haiku N−A −7.0) are fragile. None survives Holm correction (minimum adjusted p 0.095), each rests on a few items, and the haiku N−A result is partly a request-ID formatting artefact.
- **Threats, most serious first:**
  1. The ceiling makes the preregistered +5 thresholds nearly unattainable.
  2. Menu-order leakage in the request component.
  3. Run-to-run noise as large as the effects. Only B and D have two trials.
  4. Single provider: authors, analyzers and graders are all Claude.
  5. D's wording is closer to the truth-key rubric, which may limit transfer to other key-writers and to real cases.
  6. Batch confound between B and D and the other conditions.
  7. Truth-key ambiguity on the contradicted-versus-association-only boundary (DEV-I/I7, HO-J2/I7). It is harmless to the contrasts.
  8. Lenient validity rule in `Od1.fsx`; summary length is not enforced.
  9. The OG arm is underpowered, and SHARED_OG.txt is not hash-frozen.
