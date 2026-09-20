# Evidence Ledger

Status: Current human-readable projection
Updated: 2026-09-20

Purpose:
Track the current evidence state of important EDF hypotheses without allowing historical summaries to outrank later contradictory evidence.

Canonical machine-readable sources:

- `research/registries/hypothesis-registry.json`
- `research/registries/evidence-registry.json`
- `docs/framework-engineering/claim-and-confidence-policy.md`

## Current Claim State

| Hypothesis | Current state | Confidence | What the evidence actually supports | Key limitation |
|---|---|---|---|---|
| HY-EDF-001 - EDF can organize failure and success narratives through a common field structure | Internal demonstration | Medium-High | Repository-authored cases repeatedly populate the same diagnostic grammar | Shared research process; descriptive applicability is not explanatory validity |
| HY-EDF-002 - System Context improves semantic scope agreement beyond matched headings | Suggestive | Low-Medium | R1 cases are more explicitly scoped | No randomized ablation or matched-heading control |
| HY-EDF-003 - VP v1.1 improves independent reproducibility | Unsupported | Low | v1.1 changed the protocol and produced internally similar structures | No controlled v1.0 comparison and no independent replication |
| HY-EDF-004 - EDF improves control-point quality versus natural analysis | Suggestive | Low | Current cases often surface system-level controls | No blinded baseline scoring or intervention utility key |
| HY-EDF-005 - EDF is independently reproducible | Unsupported | Low | No completed evidence | Independent-human and controlled multi-model studies are absent |
| HY-EDF-006 - EDF updates proportionally as evidence changes | Unsupported | Low | Architecture and evidence packages exist | LRC-001 has not produced controlled completed runs |
| HY-EDF-007 - Origin Network and Propagation are usefully distinct | Suggestive | Medium | One ablation plus repeated authored usage | Single direct ablation; no independent replication |
| HY-EDF-008 - EDF adds semantic value beyond matched neutral structure | Open / unsupported | Low | No completed test | This is the primary next benchmark |
| HY-EDF-009 - Diagnostic topologies generalize | Suggestive | Low | Recurring patterns appear in selected cases | No held-out coding or external replication |
| HY-EDF-010 - EDF-0 is fast and learnable for routine use | Unsupported | Low | Teaching examples show intended use only | No time-on-task or usability evidence |

## Interpretation Rules

- "Internal demonstration" means demonstrated inside this repository's authored/generated corpus only.
- "Suggestive" means evidence is directionally supportive but credible alternatives remain.
- "Unsupported" does not mean false. It means the current repository does not contain completed evidence capable of establishing the claim.
- High confidence may be appropriate for a narrow methodological negative claim, such as the absence of raw run bundles, while positive performance claims remain unsupported.
- Historical percentages are not current evidence of reproducibility because their raw outputs, denominators, prompts, evaluator versions, and scoring calculations are not preserved.

## Historical Claim Corrections

The following older formulations are superseded in current summaries:

- "Very High reproducibility" -> historical internal structural agreement, non-recomputable.
- "VP v1.1 improves reproducibility" -> unsupported causal claim pending a controlled protocol comparison.
- "System Context improves cross-lens agreement" at High confidence -> suggestive pending a matched-heading ablation.
- "EDF improves control-point identification" at Medium-High confidence -> suggestive pending blinded baseline scoring.
- "Evidence evolution is a differentiation/strength" -> architectural capability only, empirical performance unsupported.

Historical source documents remain preserved for traceability.

## Promotion Gate

A claim may move to a stronger state only when the new REP identifies:

1. the exact hypothesis ID;
2. the evidence IDs;
3. the frozen run configuration;
4. raw outputs;
5. executable scoring;
6. failed/invalid-run accounting;
7. competing explanations;
8. the preregistered promotion and falsification rules.

## Next Evidence Priority

The highest-information computational test is the Multi-Model EDF Structural-Value Benchmark in `research/analysis/non-human-next-experiments.md`.

It must compare:

- natural analysis;
- matched neutral structure;
- EDF;

with blind evaluation, multiple model families, immutable run bundles, negative-run retention, and cost instrumentation.
