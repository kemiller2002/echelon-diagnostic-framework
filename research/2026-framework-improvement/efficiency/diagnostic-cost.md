# Diagnostic cost and burden

## Burden imposed by each method (human analyst, structural count)

| Method | Required steps | Required output fields | Notes |
|---|---|---|---|
| EDF v0.3 | 9 | 15 (incl. System Context triple, control-point ranking on 4 criteria) | Same sequence at every complexity level; depth varies |
| EDF-0 card | 9 prompts | 15 fields (short) | Same fields, less depth |
| D (variant) | 8 rules | Uses OD1 output contract | Organization-specific guards; no System Context triple or origin taxonomy |
| Kepner-Tregoe + Nadler-Tushman | 4 parts (deviation, IS/IS-NOT specification, cause testing, decision analysis) | Specification matrix | Heavier for humans; strongest contrast testing |
| General prompt | 0 | Output contract only | — |

## Measured analysis cost (AI analyzers)

See `token-analysis.md`. Framework text adds 1–2% to a run. Wall-clock time per diagnosis was about 12–15 s of agent time, with no meaningful difference between methods.

## Evidence-collection burden (the dominant real cost)

| Strategy | Evidence read | Score | Note |
|---|---|---|---|
| Full file | ~17 items (100%) | 96.0 (D) | Curated: every item relevant |
| Model-selected | 6 items (35%) | 84.4 | Adaptive choice from a catalogue |
| Fixed first-6 | 6 items (35%) | 69.9 | Sequence-driven collection |
| Random-6 | 6 items (35%) | 72.7 | — |

In real engagements evidence collection means interviews, document requests, data extracts and observation, and that is where organizational disruption occurs. The experiment supports **adaptive, discriminating evidence requests** as the largest efficiency lever. Fixed-sequence collection (the default in templated diagnostics, including the v0.3 sequence) is the worst strategy tested.

## Time to actionable findings

Not measurable without human analysts. Proxy: sufficiency accuracy. D reached the correct "sufficient" decision in 24/24 held-out runs; N declared false insufficiency in 4/10. Methods that over-abstain delay action.

## Organizational disruption

Not measured. Protocol for real-world measurement: `intervention-effectiveness/evaluation-design.md`.
