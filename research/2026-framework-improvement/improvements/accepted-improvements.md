# Accepted improvements

Under the mission's decision gates (all ten criteria, including independent review), **one change is accepted**. It is evaluation infrastructure, not a change to EDF's diagnostic semantics.

## ADOPT

### CI-14 — OD1 evaluation infrastructure as EDF's standing test bed

- **Problem demonstrated.** EDF's historical performance claims cannot be recomputed. EX-EDF-001's +0.56 has no raw outputs in the repository, and its "EDF" condition paraphrased v0.3 (`baseline/implementation-audit.md`).
- **Mechanism.** Preregistration; SHA-256 freezing of cases, truth and prompts; truth sealed during analyzer runs; deterministic F# scoring; append-only raw outputs and workflow journals; held-out split; blind graders with an agreement check; independent re-scoring.
- **Baseline and comparison.** Not applicable as an accuracy claim. As a validity claim: an independent reimplementation reproduced all 320 run scores and every contrast, and the inter-grader κ was 0.95.
- **Costs.** About 37M host-reported subagent tokens for the full programme. A single condition × 12 held-out cases × 1 trial is about 12 runs (≈ 0.9M tokens).
- **Regressions and risks.** The benchmark saturates for current models, and the request menu leaked answers (18/22 cases). Both are documented and must be fixed in OD2.
- **Required fixes before reuse:** (1) randomize request-menu order; (2) align calibration weighting with spec §5 (R-1); (3) add harder cases (OD2).

## Not accepted for production EDF

No semantic change to EDF meets the adoption gates. In particular:

- the experimental variant D;
- the decision-gate semantics (CI-01);
- typed causal edges (CI-02);
- adaptive evidence acquisition (CI-03).

The strongest of these (CI-01, CI-03) are classified EXPERIMENTAL and prioritized for confirmation in `strategy/improvement-priorities.md`.
