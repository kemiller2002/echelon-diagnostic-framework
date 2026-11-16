# EDF and Clarity — comparison

## What Clarity is (as available to this research)

- **clarity-framework** (content repository, not attached to this session): a decision posture built on question, evidence, assumptions, consequence, reversibility, decision, ownership, learning and reassessment. Its operating rule is summarized in `echelon-organization-administration/business-plan/02-company-foundation/02.03-clarity-operating-model.md`.
- **clarity-service** (F# CLI, not attached): deterministically inventories a *software repository* and extracts evidence. It is not an organizational diagnosis tool and was not testable here.
- In EDF's documents: v0.3 places Clarity *after* diagnosis ("EDF answers what is happening; Clarity answers what should we do"). The v1 draft makes Clarity *govern* EDF's depth and mode. The baseline audit records this as an unresolved conflict.

Conditions were built from Clarity's documented elements only (`experiments/prompts/CLARITY.fragment.txt`). They were frozen before any analyzer output.

| Condition | Workflow |
|---|---|
| CL_ALONE | Clarity alone, no EDF (dev only) |
| B_CLARITY | EDF v0.3, then a Clarity decision pass over actions and sufficiency |
| BI_CLARITY | Clarity's evidence discipline inserted into EDF step 6, plus the decision pass |

## Results

| Contrast | Dev (10 cases) | Held-out (12 cases) |
|---|---|---|
| CL_ALONE − A_GENERAL | +2.1 [−0.0, 4.8] | – |
| B_CLARITY − B_EDF03 | +0.5 [−0.5, 1.7] | −0.8 [−2.2, 0.8] |
| BI_CLARITY − B_EDF03 | +1.5 [−0.2, 3.8] | −0.0 [−1.7, 1.7] |

| Held-out metric | B_EDF03 | B_CLARITY | BI_CLARITY |
|---|---|---|---|
| Action accuracy | 0.952 | 0.968 | 0.968 |
| Harmful promotion | 0/44 | 0/22 | 0/22 |
| Abstention accuracy | 0.917 | 0.833 | 0.917 |
| Hidden-problem detection | 0.957 | 0.913 | 1.000 |
| Mean prompt chars | 26,061 | 27,492 (+5.5%) | 27,795 (+6.7%) |

## Did Clarity improve the dimensions the mission asked about?

| Dimension | Finding |
|---|---|
| Evidence evaluation | No measurable change (hypothesis accuracy B 0.979, BI 0.972) |
| Assumption identification | Not separately measurable in the item format |
| Causal reasoning | No change (causal accuracy 0.94 vs 0.92–0.94) |
| Contradiction handling | No change on class D cases |
| Uncertainty representation | No change in unsupported certainty (2/44 vs 2/22 each) |
| Intervention selection | Action accuracy +1.6 points nominally; harmful promotion unchanged at 0 |
| Decision documentation | Not measured; plausibly Clarity's real value (ownership, reassessment triggers), which item scoring does not capture |

**H-OD1-6 (Clarity improves action selection):** nominally met under the weak preregistered criterion (action accuracy higher, harmful not higher). The difference is 1–2 items and is not meaningful. Verdict: **INCONCLUSIVE / no measurable diagnostic benefit**.

## Conclusion

Integrating Clarity into EDF's diagnostic prompt does not measurably improve diagnostic or action-classification accuracy, and it costs about 6% more prompt. Per the mission rule ("reject integrations that add complexity without measurable value"), **reject prompt-level integration of Clarity into EDF diagnosis**. Keep the handoff described in v0.3: EDF produces the diagnosis, and Clarity governs the decision record (ownership, reversibility, reassessment). The decision record is where Clarity's documented value lies, and this benchmark does not measure it.

Resolve the v0.3 / v1-draft conflict in favour of v0.3's sequential handoff. There is no evidence that Clarity governing EDF's depth improves diagnosis.
