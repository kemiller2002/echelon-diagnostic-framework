# EDF Framework Improvement Research (EDF-IMPROVEMENT-2026-001)

Status: **PARTIALLY COMPLETE.** Real-world, human-analyst and cross-provider studies could not be performed here; protocols are preserved (`continuation/`).
Work item: `WI-0013`. Baseline: EDF v0.3 at `41fea6f`, frozen in `baseline/baseline-freeze.sha256`. Date: 2026-11-16.

## Central question

What measurable changes make EDF better at identifying real organizational problems, explaining their causes, and supporting effective interventions than it is today?

## Answer in brief

- **EDF v0.3 shows no measurable advantage** over a general-purpose AI prompt or an established diagnostic method (Kepner-Tregoe + Nadler-Tushman) on 12 held-out synthetic organizational cases. This held across recognition and open-generation formats and two model tiers, and an independent replication confirmed it.
- **No rewritten EDF variant** met its preregistered improvement margin.
- **Evidence acquisition** was the largest measurable lever. Adaptive (model-chosen) evidence beat random or fixed-order collection by about 12–14 points at the same budget. EDF guidance did not improve the choice.
- The one framework component with a significant ablation effect is **decision-gate semantics**: harm judged under every live hypothesis, "no intervention warranted" as a sufficient conclusion, and discriminating next evidence. The effect is +2.8 [0.5, 5.7], from a single trial on one tier, so it is unconfirmed.
- **Clarity integration** into diagnosis gave no benefit. **EDF-0** was slightly worse than full EDF. A **long generic protocol** hurt accuracy.

Read `strategy/executive-recommendations.md` first.

## Map

| Area | Files |
|---|---|
| Baseline audit | `baseline/methodology-inventory.md`, `implementation-audit.md`, `diagnostic-taxonomy.md`, `current-limitations.md` |
| Science | `scientific-foundations/literature-review.md`, `literature/*.md`, `competing-methodologies.md`, `methodological-opportunities.md`, `evidence-register.csv`, `longitudinal-assessment.md` |
| Validity | `diagnostic-validity/construct-validity.md`, `reliability-analysis.md`, `false-positive-negative-analysis.md`, `failure-taxonomy.md` |
| Causal | `causal-analysis/causal-methods.md`, `causal-benchmarks.md`, `observed-failures.md`, `causal-benchmark-items.csv` |
| Organizational dynamics | `organizational-dynamics/incentive-analysis.md`, `stakeholder-bias.md`, `organizational-complexity.md` |
| Interventions | `intervention-effectiveness/intervention-research.md`, `evaluation-design.md`, `outcome-measurement.md` |
| Benchmark | `benchmarks/benchmark-specification.md`, `benchmark-catalog.json`, `evaluation-rubric.md`, `held-out-policy.md`, `cases/` (22 case, truth and review files), `frozen-cases-*.sha256` |
| Experiments | `experiments/preregistration-OD1.json`, `amendment-02-after-dev.md`, `experiment-register.csv`, `baseline-results.md`, `variant-results.md`, `ablation-results.md`, `replication-results.md`, `prompts/`, `raw/` (all outputs and journals), `scored/`, `replication/` |
| Improvements | `improvements/candidate-improvements.md`, `accepted-improvements.md`, `rejected-improvements.md`, `regression-analysis.md` |
| Efficiency | `efficiency/diagnostic-cost.md`, `adaptive-diagnostics.md`, `token-analysis.md` |
| Clarity | `integration/clarity-comparison.md`, `combined-workflow-analysis.md` |
| Strategy | `strategy/improvement-priorities.md`, `framework-roadmap.md`, `executive-recommendations.md` |
| Evidence | `evidence/source-register.csv` (224 sources with verification level), `claims-and-evidence.csv`, `contradictions.csv` |
| Continuation | `continuation/research-status.json`, `unresolved-questions.md`, `continuation-manifest.json` |
| Harness | `harness/Od1.fsx` (F#: validate, bundle, ingest, score, stats, ogbundle, gradebundle, ogscore, adapt1, adapt2), `harness/run-analyzers.workflow.js` |

## Reproduce

```bash
dotnet fsi harness/Od1.fsx validate benchmarks/cases
dotnet fsi harness/Od1.fsx score benchmarks/cases experiments/raw/heldout /tmp/scored
dotnet fsi harness/Od1.fsx stats /tmp/scored heldout 20261116 "B_EDF03,A_GENERAL;D_EDFX,B_EDF03"
python3 -I experiments/replication/od1_score.py   # independent re-scoring
```

## Integrity notes

- Every claim is labeled by evidence type. Synthetic results are not evidence of real-world organizational effectiveness.
- Case authors, reviewers, analyzers and graders were all Claude-family models. That is a declared limitation.
- Analyzer isolation: each analyzer read exactly one prompt bundle. All 484 analyzer and grader transcripts were audited for tool calls. Truth files were encrypted on disk during analyzer runs.
- Known defects are documented, not hidden: the request-menu order leak, a calibration-weighting deviation from spec, and the benchmark ceiling.
