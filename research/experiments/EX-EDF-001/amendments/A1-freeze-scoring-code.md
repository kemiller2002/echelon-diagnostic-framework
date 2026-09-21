# EX-EDF-001 Amendment A1: Freeze scoring code and executor dependencies

Date: 2026-09-21
Timing: before any analyzer or evaluator execution
Outcome data observed before amendment: none
Analyzer runs before amendment: 0
Evaluator runs before amendment: 0

## Reason

The initial preregistration froze cases, ground truth, prompts, output contract, rubric, preregistration, and randomization with SHA-256 values, but named the scoring harness without pinning its bytes.

That was a reproducibility gap. A scoring implementation that can drift after preregistration is not sufficiently frozen for a confirmatory experiment.

## Amendment

1. `scripts/edf-benchmark.mjs` is now pinned by its Git blob SHA in `frozen-files.json`.
2. Benchmark validation recomputes Git blob hashes and fails on scoring-code drift.
3. Executor configuration files may declare `fileDependenciesGitBlobSha`; the harness verifies those dependencies before a run.
4. The provider adapter is expected to be transitively pinned by each committed executor configuration.
5. Provider-returned usage/model/request metadata is captured when available.

## Scientific impact

This amendment does not change the hypothesis, cases, conditions, rubric, primary margin, success criteria, falsification criteria, randomization seed, or analysis estimand.

It reduces researcher degrees of freedom before execution.

Because no outcome data or model outputs existed when this amendment was made, it does not introduce outcome-informed tuning.

Any later change to the frozen scoring harness after the first confirmatory run requires a new experiment version.
