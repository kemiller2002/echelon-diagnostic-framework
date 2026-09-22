# EX-EDF-2026-A003 Case Authoring Contract

Case authoring occurs before analyzer execution. Each provider authors exactly one case for each of six constructs. Every case has an analyzer-visible `<CASE>.case.json` and hidden `<CASE>.truth.json`.

Cases must satisfy `case-authoring-assignments.json`: at least 12 evidence items, 4 candidate hypotheses, 6 candidate edges, 5 candidate actions, 3 red herrings, 2 conflicting-evidence pairs, 2 expected-unknown items, 1 plausible harmful action, and a 3-hop reasoning chain. No single evidence item may reveal the complete answer. Candidate wording must not leak expected state.

At least four scored items must be `constructCritical: true`.

A case freezes only after schema/hardness validation and opposite-provider review for internal consistency, answer leakage, unsupported truth, and accidental triviality. All revisions must finish before the first analyzer run.