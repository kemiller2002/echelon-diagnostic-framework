# Multi-Model EDF Structural-Value Benchmark

## Mission

Determine whether EDF produces semantic diagnostic value beyond natural analysis and a matched structured template when applied by AI models to unseen, non-human evidence packages.

This mission must not involve human participants, human-subject observation, private-person profiling, medical testing, or collection of personal behavioral data.

## Prior evidence

Read:

- `research/analysis/non-human-experimental-comparative-analysis.md`
- `research/analysis/non-human-experiment-matrix.json`
- `research/analysis/non-human-experimental-lineage.md`
- `research/analysis/non-human-experimental-contradictions.md`
- `docs/edf/validation/case-001R1-challenger.md`
- `docs/edf/validation/case-003R1-boeing-737-max.md`
- `docs/edf/validation/case-008R1-pixar.md`
- `docs/framework-engineering/validation-protocol.md`

The current evidence is dependent, lacks raw outputs, and cannot distinguish EDF value from shared prompt structure or evaluator bias.

## Hypotheses

Primary hypothesis: EDF improves blinded semantic completeness and evidence-supported control-point coverage relative to both natural analysis and a matched neutral structure.

Competing hypothesis: the observed effect is explained by headings, prompt length, output length, evaluator affinity, model choice, or case selection.

## Design

Use three conditions:

1. Natural analysis with no prescribed structure.
2. Matched neutral structure containing the same number and approximate length of fields without EDF terminology.
3. EDF using the frozen tested grammar.

Use at least three materially distinct pinned model families from at least two providers when available. Use at least six versioned, non-sensitive historical or synthetic evidence packages spanning failure, success, dynamic evidence, simple troubleshooting, sociotechnical systems, and technical systems. Do not use cases employed to tune prompts.

Run at least 30 independent generations per condition/model across the suite, subject to a preregistered sample-size simulation. Match token budgets and tool access. Prevent cross-run memory.

## Randomization and blinding

- Preregister a deterministic randomization seed.
- Randomize case, condition, and model execution order.
- Strip condition and model identifiers from judge inputs.
- Use two distinct automated evaluator families plus deterministic schema/provenance checks.
- Freeze disagreement and adjudication rules before results.

## Metrics

- Evidence-supported claim precision and recall against versioned case keys.
- Semantic coverage of manifestations, origins, propagation, and control points.
- Unsupported-claim rate.
- Structural validity.
- Control-point relevance and nonredundancy.
- Tokens, latency, retries, errors, and cost per valid result.
- Between-run and between-evaluator stability.

Do not combine incompatible metrics into one universal score. Preserve original values and report uncertainty.

## Success and falsification

Success requires EDF to exceed both controls by the preregistered practically meaningful margin on semantic measures, without unacceptable unsupported-claim or cost increases, in at least two model families.

The primary hypothesis is falsified if:

- the matched neutral template is equivalent within the preregistered margin;
- improvement disappears under model or evaluator substitution;
- EDF only improves formatting;
- unsupported claims or operational cost erase practical value.

## Evidence preservation

Preserve every attempted run, including timeouts, invalid outputs, refusals, parsing failures, and null results. Store:

- exact prompts and hashes;
- exact evidence-package versions and hashes;
- model/provider/version and inference parameters;
- random seeds and assignment table;
- repository commit and environment;
- raw model outputs;
- evaluator prompts and outputs;
- deterministic-check results;
- scoring code and analysis outputs;
- exclusion decisions with reasons.

Do not silently repair outputs or discard failures.

## Required outputs

1. Canonical REP and preregistration.
2. Machine-readable run manifest.
3. Versioned task/evidence suite.
4. Frozen prompt registry.
5. Raw output bundle.
6. Evaluator and scoring bundle.
7. Negative/null-result registry.
8. Comparative analysis with sensitivity checks.
9. Updated experiment matrix, lineage, contradictions, findings, and backlog.
10. Reproduction instructions verified from a clean environment.

## Stop conditions

Stop when all preregistered runs complete, a preregistered futility/safety boundary is reached, or required model access becomes unavailable. Do not expand the task set after observing results. Record early stopping and all incomplete work.

