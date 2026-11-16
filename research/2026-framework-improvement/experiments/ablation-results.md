# Ablation and simplification results — EX-EDF-2026-OD1

Held-out recognition, Sonnet, 12 cases. Paired case-level bootstrap CIs.

## Ablations of the experimental variant D

| Contrast | Mean diff | 95% CI | Wins/losses | Reading |
|---|---|---|---|---|
| D − D_NOGATE (action/sufficiency/request rules replaced with v0.3 wording) | +2.8 | [0.5, 5.7] | 8/4 | The gate (harm under every live hypothesis; "no intervention" counts as sufficient; discriminating requests) contributes ≥ 2 points. H-OD1-7 criterion met for this component (exploratory). |
| D − D_NOORG (premise check, metric check and evidence ledger removed) | +1.5 | [−0.2, 3.6] | 7/5 | The organizational guards do not reach the preregistered +2 threshold. Inconclusive / candidate for removal in this benchmark. Caveat: the benchmark's leadership-narrative traps were already rarely failed by any condition (0–2%), so this component had little room to show value. |

## Simplification of current EDF

| Contrast | Dev | Held-out | Reading |
|---|---|---|---|
| B0 (EDF-0 quick card) − B (full v0.3) | −0.4 [−2.0, 1.3] | −2.1 [−3.9, −0.5] | The shorter card is slightly worse on held-out. Non-inferiority at −3 is not established at CI level. |
| B (full v0.3) − A (no framework) | +2.1 [−0.3, 5.3] | +1.4 [−0.7, 3.1] | The whole framework adds ≤ 2 points and is not significant. Removing it entirely costs about as much as reducing it to EDF-0. |
| D (3.0k chars, organization-specific) − B (5.3k chars) | – | +1.5 [−0.3, 3.6] | A shorter, rewritten method is at least as good as v0.3. |

## Which v0.3 components add value? (indirect evidence)

No component-level ablation of v0.3 itself was run; doing so is left for future work because the whole-framework effect was too small to decompose. Indirect evidence:

- **Diagnostic Sufficiency** is the component most clearly linked to measured differences. Abstention errors (sufficient answered insufficient) were the third most common held-out error, at 14 runs. D's reformulated sufficiency rule ("no intervention warranted" can be a sufficient conclusion) achieved 24/24, B 22/24 and N 8/12.
- **Control-point ranking** (influence, controllability, cost, confidence): replacing D's gate with the v0.3 ranking wording cost 2.8 points (D − D_NOGATE). The four-criterion ranking does not substitute for an explicit harm-under-every-hypothesis test.
- **System Context (primary system / context / focus)**, **five verbs** and **origin-type taxonomy**: D omits them and was not worse. There is no evidence in this benchmark that they add diagnostic accuracy.
- **Origin Network vs Propagation**: no separate test. D keeps the origin network but drops propagation as a separate step and was not worse.

## Clarity integration (see `integration/clarity-comparison.md`)

| Contrast | Dev | Held-out |
|---|---|---|
| B_CLARITY (EDF → Clarity) − B | +0.5 [−0.5, 1.7] | −0.8 [−2.2, 0.8] |
| BI_CLARITY (Clarity inside EDF evidence step) − B | +1.5 [−0.2, 3.8] | −0.0 [−1.7, 1.7] |
| CL_ALONE (Clarity without EDF) − A | +2.1 [−0.0, 4.8] | not run |

## Conclusions

1. The measurable value sits in **decision-gate semantics**: what counts as harmful, when "no intervention" is a sufficient conclusion, and how evidence requests are chosen. It does not sit in EDF's descriptive vocabulary.
2. EDF's descriptive scaffolding (System Context triple, origin-type taxonomy, separate propagation step, four-criterion ranking) **shows no measurable contribution** and could be made optional. Removing it should still be tested on harder cases with human analysts, where vocabulary may aid communication and reviewability (not measured here).
3. Adding Clarity to EDF does not improve item-level diagnostic or action accuracy in this benchmark.
