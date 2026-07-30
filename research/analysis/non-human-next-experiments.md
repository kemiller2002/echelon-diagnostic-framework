# Non-Human Experimental Research Backlog

All proposed subjects are AI models, prompts, generated artifacts, automated evaluators, or historical evidence packages. No human participation, observation, profiling, or behavioral-data collection is permitted.

## Priority matrix

| Rank | Proposed experiment | Information gain | Decision impact | Cost | Obsolescence risk | Priority |
|---:|---|---|---|---|---|---|
| 1 | Multi-Model EDF Structural-Value Benchmark | Very high | Critical | Medium | Medium | Immediate |
| 2 | System Context Factorial Ablation | High | High | Low-medium | Low | Immediate |
| 3 | Computational LRC-001 Evidence-Update Study | Very high | High | Medium | Low | Next |
| 4 | Propagation Definition Replication | Medium-high | Medium | Low | Low | Next |
| 5 | Automated Control-Point Utility Benchmark | High | High | High | Medium | Later |

## Experiment 1 — Multi-Model EDF Structural-Value Benchmark

Originating uncertainty: whether EDF adds semantic diagnostic value beyond headings and shared prompting.

Linked prior experiments: all R1 cases, especially VC-001R1, VC-003R1, and VC-008R1.

Hypothesis: across multiple model families and unseen historical evidence packages, EDF prompts improve blinded semantic completeness and supported control-point coverage relative to both natural analysis and a matched-heading control.

Competing hypothesis: apparent improvement is explained by structure, prompt length, evaluator affinity, or shared model family.

Independent variables:

- prompt condition: natural, matched neutral structure, EDF;
- model family/provider and pinned version;
- case/evidence package;
- seed/repeated stochastic run.

Dependent variables:

- evidence-supported claim precision/recall against a case key;
- unsupported-claim rate;
- semantic origin/control-point coverage;
- structural validity;
- token count, latency, retries, and cost;
- score stability across evaluators.

Controls: identical evidence, equivalent token budgets, matched output length instruction, randomized condition order, frozen prompts, no cross-run memory.

Dataset: at least six versioned non-sensitive historical/synthetic evidence packages, including failure, success, dynamic, simple, and technical cases. Hold out all cases used to tune prompts.

Model selection: at least three materially distinct model families from two or more providers when available; pin exact identifiers.

Sample rationale: minimum 30 independent generations per condition/model across the suite, with sequential power simulation before execution; final N must be justified from the smallest practically meaningful improvement and observed pilot variance.

Randomization: deterministic preregistered assignment generated from a recorded seed.

Evaluator design: two automated evaluator families plus deterministic checks; judges blinded to condition, prompt, and model. Resolve evaluator disagreement through a frozen adjudication rule, not post hoc selection.

Success: EDF exceeds both controls on preregistered semantic value without a practically unacceptable increase in unsupported claims or cost, and the effect appears in at least two model families.

Falsification: matched neutral structure performs equivalently within the non-inferiority margin, or EDF improvement disappears under evaluator/model substitution.

Expected information gain: very high. Estimated cost: medium. Dependencies: create run-bundle schema and case keys. Risks: evaluator bias, benchmark leakage, provider drift. Stop: planned runs complete or futility boundary reached. Required artifacts: REP, preregistration, cases, prompt hashes, run manifest, raw outputs, evaluator outputs, analysis script, negative-run registry, final report.

## Experiment 2 — System Context Factorial Ablation

Hypothesis: System Context improves semantic scope agreement beyond adding a common heading.

Competing hypothesis: agreement increases only because identical labels constrain formatting.

Design: cross natural/neutral-context/EDF-System-Context prompts with at least three models and four unseen cases. Measure scope correctness, omitted material systems, semantic overlap, and unsupported scope assumptions.

Success: EDF System Context beats neutral headings on semantic measures. Falsification: only structural-format agreement changes.

Cost: low-medium. Stop: preregistered sample complete.

## Experiment 3 — Computational LRC-001 Evidence-Update Study

Hypothesis: with frozen prompts and models, EDF updates origins, confidence, and control points proportionally across EP-013A/B/C.

Competing hypothesis: later outputs are dominated by model prior knowledge, package phrasing, or indiscriminate narrative replacement.

Controls: isolated sessions, explicit evidence-only instruction, adversarial leakage checks, package-order randomization across independent runs, and a synthetic longitudinal case whose ground-truth changes are known.

Success: stable facts remain stable; newly supported changes localize correctly; prohibited later facts do not appear. Falsification: substantial leakage or wholesale unstable rewrites.

Cost: medium. Dependency: source-level evidence-package audit.

## Experiment 4 — Propagation Definition Replication

Hypothesis: refined wording improves independent automated classification of structure versus behavior.

Design: old/new definitions on unseen synthetic and historical cases, multiple models, matched length, blind judges. Include deliberately ambiguous negative controls.

Success: increased semantic separation and lower category-confusion rate across model families. Falsification: improvement is model-specific or limited to the original POS-like pattern.

## Experiment 5 — Automated Control-Point Utility Benchmark

Hypothesis: EDF yields interventions with better causal coverage and feasibility than controls.

Competing hypothesis: EDF produces broader but not more useful recommendation lists.

Design: simulations and historical cases with pre-authored outcome/intervention keys; compare EDF, natural, matched structure, FMEA, and FTA where appropriate. Measure rank utility, redundancy, unsupported intervention assumptions, and sensitivity to missing evidence.

Dependency: Experiment 1 establishes a reliable automated evaluation stack.

## Research-system prerequisites

Before any new experiment:

1. Create an append-only run manifest.
2. Define canonical IDs and status semantics.
3. Freeze exact prompts, evidence packages, model identifiers, budgets, and scoring code.
4. Preserve every attempted run and error.
5. Record repository commit and environment.
6. Separate exploration from confirmation.

