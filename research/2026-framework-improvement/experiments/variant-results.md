# Experimental variant results — EX-EDF-2026-OD1

## Variant D (`D_EDFX`, "evidence-disciplined EDF")

D was designed after the dev phase, from dev failures plus the literature. It was frozen before any held-out run (`experiments/frozen-prompts-heldout.sha256`; amendment 2). D has 8 steps and 3,034 characters, against 5,316 for EDF v0.3:

1. Treat the presenting problem as one hypothesis. Name rivals: no real problem, an intentional tradeoff, an external cause, a measurement artifact, the wrong unit.
2. State the outcome factually against a baseline, and check how each metric is produced, who controls it, and whether it is rewarded.
3. Keep an evidence ledger: who produced each item, whether they have a stake, whether it is independent. Silence is not health. Missing evidence makes a claim unknown, not contradicted.
4. Origin network with interactions, loops and delays. Separate the mechanism from protective or detection controls.
5. Grade every causal link. Causal requires temporal order, a mechanism, covariation and the main rival excluded, or an intervention result.
6. Judge each action under every live hypothesis (harmful / premature / preferred).
7. Sufficiency includes "no intervention warranted". Evidence is insufficient only when the best action depends on which live hypothesis is true.
8. Choose next evidence by its power to discriminate between live hypotheses.

## Results

| Contrast | Held-out recognition (Sonnet) | Held-out recognition (Haiku) | Open generation (Sonnet) |
|---|---|---|---|
| D − B (vs EDF v0.3) | +1.5 [−0.3, 3.6], 6 wins / 6 losses | +0.1 [−1.9, 2.5] | +0.9 [−4.3, 6.6] |
| D − A (vs general AI) | +2.9 [0.8, 5.2] | −1.5 [−3.5, 0.9] | +2.6 [−3.3, 9.2] |
| D − N (vs matched structure) | +4.3 [1.6, 7.8] | +5.6 [2.5, 9.0] | +0.7 [−3.9, 5.2] |
| D − C (vs KT + Nadler-Tushman) | +2.1 [−0.3, 4.7] | – | −1.7 [−6.2, 3.8] |

Secondary metrics, Sonnet held-out:

| Metric | D | B |
|---|---|---|
| Abstention accuracy | 24/24 | 22/24 |
| Unsupported certainty | 2/44 | 2/44 |
| Causal overclaim | 0/64 | 1/64 |
| False diagnosis | 0/102 | 0/102 |
| Harmful-action promotion | 0/44 | 0/44 |
| High-confidence errors | 1/259 | 2/262 |
| Trial-to-trial item agreement | 97.5% | 97.5% |

## Preregistered verdicts

**H-OD1-5 (D improves on B): not supported.** The +5 margin was missed in every arm. The Sonnet CI includes 0, and the effect is absent on Haiku. The safety constraints held: D was not worse on harmful promotion or false diagnoses.

**H-OD1-8 (open generation: D detects more real problems than B without more false diagnoses): not supported.** D missed 0/18 real problems against B's 1/18, which is too few events to distinguish the two.

The only consistent advantage is **D − N**, and it reflects N's weakness, not D's strength.

## Adaptive evidence selection (efficiency experiment)

Analyzers saw only a catalogue of each case's evidence: ID, stage, source and source type, with content hidden. They chose k = 6 of the ~17 items, then diagnosed from those items alone with the D module.

| Selection strategy | Mean score | Required-evidence coverage | Δ vs full evidence (D, 95.9) |
|---|---|---|---|
| Model-selected with D guidance | 84.2 | 48% | −11.7 [−14.4, −9.0] |
| Model-selected with general prompt | 84.4 | 48% | −11.5 [−14.8, −8.4] |
| First 6 items (fixed sequence) | 69.8 | 35% | −26.1 [−31.3, −21.1] |
| Seeded random 6 | 72.6 | 35% | −23.4 [−29.9, −17.1] |

- Model-selected beats random by +11.6 [5.9, 17.9]. Adaptive (value-of-information) selection substantially beats fixed or random collection.
- D guidance vs general prompt: −0.2 [−3.8, 3.4]. EDF semantics add nothing to evidence selection.
- Reading ~35% of the evidence by adaptive choice retains about 88% of full-evidence performance. It still loses about 12 points, mostly because the selector cannot see content and misses some decisive items.

## Interpretation

What made a measurable difference in this programme was **evidence acquisition**: which items were read. The label set or procedure in the prompt made very little difference. This is consistent with the literature on sequential diagnosis and value of information (`efficiency/adaptive-diagnostics.md`). It implies that EDF's leverage, if any, lies in structuring *what evidence to collect next and when to stop*. Prescribing how to reason over a fixed evidence file adds little.
