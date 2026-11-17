---
id: EDF-COMPARATIVE-2026-002-IMPROVEMENTS
title: Evidence-based improvement proposals (not applied to the canonical specification)
status: proposed — requires owner approval
created: 2026-11-17
---

# Improvement proposals

None of these changes `docs/edf/specification.md`. Each is graded by the evidence in
`../REPORT.md`.

| # | Proposal | Evidence | Strength | Recommendation |
|---|---|---|---|---|
| P1 | **Stop presenting EDF as a reasoning procedure that improves automated (LLM) diagnosis.** Describe it as a representational and reporting grammar whose diagnostic value is unproven. | Four nulls; OD2 equivalence within about ±2; components removable | Strong (within scope) | Adopt in README, website and claim registry |
| P2 | For automated diagnosis, **spend compute rather than structure**: effort high, or sampled self-consistency. | Effort high − EDF medium +4.4 [0.4, 9.4], cheaper | Moderate (one benchmark, one family) | Adopt as operational guidance |
| P3 | **Do not add** decision gates, a VOI collection rule or orchestration to EDF for model use. | F − B −0.17 [−2.19, 1.81]; SEL_EDFO − B −0.86 [−3.88, 1.67] | Moderate | Reject for now |
| P4 | Avoid long procedural prompts for **small models**. | Haiku: EDF − KT −3.3 [−5.6, −1.4]; OD1 N − A −7.0 | Moderate | Adopt as guidance; investigate the mechanism |
| P5 | Re-target EDF research at **human** use: a short output contract (scope, causes with evidence grades, unknowns, next action), evaluated for reviewer error detection, agreement and time to action. | Untested; the only remaining plausible value | Hypothesis | Fund a human-analyst study (UQ-02) |
| P6 | Make budgets and consent limits hard constraints in any evidence-acquisition rule. | OD1-S breach; OD2 at budget 2 had ≤ 1 breach per arm | Moderate | Keep |
| P7 | Retire the claim that EDF's Diagnostic Sufficiency improves stopping. | Every arm stopped optimally on STOP cases without it | Moderate | Adopt |
