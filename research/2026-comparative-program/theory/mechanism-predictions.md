---
id: EDF-COMPARATIVE-2026-002-THEORY
title: Candidate mechanisms for any EDF effect, with falsifiable predictions mapped to OD2 tests
status: predictions-registered-before-heldout-results
created: 2026-11-17
work_item: WI-0014
---

# Candidate mechanisms and their predictions

Each row states a mechanism by which a structured framework such as EDF *could*
improve diagnosis. It gives the theory behind it, a prediction that would be false
if the mechanism were not operating, and the OD2 measurement that tests it. These
predictions are written before any held-out result exists. They are hypotheses, not
claims. A plausible mechanism is not evidence that it operates.

| # | Mechanism | Theoretical basis | Falsifiable prediction | OD2 test |
|---|---|---|---|---|
| M1 | **Search-space reduction**: the grammar narrows the hypotheses considered | Bounded rationality; structured elicitation | B reads fewer items, at equal or better accuracy, than A | `reads` and `acc`, B − A |
| M2 | **Externalized state**: fields hold intermediate conclusions, reducing context loss | Working-memory offloading; agent scratchpads | The effect grows with case length and evidence count, and appears in E_NOSEQ (same fields, no order) | B − A by case length tercile; B − E_NOSEQ ≈ 0 |
| M3 | **Evidence-to-conclusion linkage**: the "Evaluate Evidence" step forces observation/inference separation | Evidence-based reasoning; ACH diagnosticity | B has higher evidence F1 and lower unsupported certainty than A; removing step 6 (E_NOEVID) lowers both | `evf1`, `unsupportedCertainty`; B − E_NOEVID |
| M4 | **Reduced premature convergence**: an origin *network* keeps rivals live | Confirmation-bias literature; ACH | B has lower false-diagnosis and causal-overclaim rates on MISLEAD, OBVWRONG and CONFOUND cases; E_NOCAUSAL raises them | class-specific `falseDiagnosis`, `causalOverclaim` |
| M5 | **Effort allocation / stopping**: Diagnostic Sufficiency approximates a value-of-information stop | Howard (1966) VOI | B stops earlier on STOP cases and reads more on SEQ cases than A; E_NOSUFF removes the difference; F (explicit VOI) strengthens it | `reads` by class, B vs A vs E_NOSUFF vs F |
| M6 | **Context-window robustness** | Long-context degradation | Not testable in OD2: cases are short relative to context. Recorded as untested. | — |
| M7 | **Calibration vs conviction**: structure improves calibration, or only makes output more convincing | Turpin et al. (2023), unfaithful explanations | If calibration: B has lower Brier and high-confidence error. If conviction: B's briefs are rated more persuasive at equal accuracy (positive persuasion gap) | `cal`, `highConfidenceError`; Track B gap |
| M8 | **Causal vs narrative discrimination** | Causal inference (Pearl); CLadder | B has higher accuracy on `association-only` edges than A | edge accuracy on association-only keys |
| M9 | **Variance reduction**: structure makes runs more repeatable | Noise literature (Mandel and Tetlock, 2018, predict the *opposite* for multistep techniques) | Competing predictions: B test–retest > A (variance reduction) or B < A (added noise). Either outcome falsifies one side. | Item agreement, trials 1–2 |
| M10 | **Compute-normalized value** | Snell et al. (2024); Wang et al. (2023) | If EDF only spends compute, A_VOTE5 or A at higher effort matches B at equal cost; B adds no value per dollar | Track A curves; A_VOTE5 − B per USD |
| M11 | **Orchestration** | Cynefin-style fit of method to problem | SEL_EDFO > B, and SEL_EDFO chooses differently by class; if the choices do not vary with class, there is no orchestration | Track C choices × class; SEL_EDFO − B |

## How results will be read

- A mechanism is **supported** only if its prediction holds on held-out data and
  the comparison condition does not show the same effect.
- A mechanism is **refuted for this benchmark** if its prediction is contradicted
  with a CI that excludes the predicted direction.
- Everything else is **untested** or **inconclusive**.
- If accuracy is at ceiling, M3, M4 and M8 may be untestable. The report will
  say so rather than read a null as refutation.

## Results (added after held-out analysis, 2026-11-17)

Outcomes for each mechanism are in `../REPORT.md` § Mechanisms. In summary: M1, M3,
M5 and M8 are refuted for this benchmark. M4 is untestable because of the floor.
M7, M9 (for EDF) and M11 are not supported. **M10 is supported**: compute beats
structure per dollar. KT + ACH showed the added-noise pattern M9 anticipated, with
test–retest 0.911 against 0.963 for no framework.
