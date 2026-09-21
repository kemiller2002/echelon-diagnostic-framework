# EX-EDF-001: Multi-Model EDF Structural-Value Benchmark

This is the first confirmatory experiment package designed under Validation Protocol v1.2.

## Primary question

Does EDF add semantic diagnostic value beyond natural analysis and a matched neutral structure?

The competing explanation is intentionally strong: generic structure, extra prompting, or serialization discipline may explain the apparent benefit.

## Current state

The design is preregistered and fail-closed. Exact analyzer and evaluator provider/model/version bindings are intentionally not invented. `executor-matrix.json` must be completed and committed before the first confirmatory run. Until then, executions are exploratory and cannot promote HY-EDF-008.

## Design

Six new synthetic cases cover a single primary cause, causal interaction, success/resilience, underdetermination, misleading correlation, and upstream control-point selection. Synthetic cases reduce prior-knowledge leakage and provide hidden ground truth; they do not replace later real-world replication.

The three conditions are natural analysis, matched neutral structure, and EDF. The matched-neutral condition is deliberately strong and approximately matches the EDF reasoning budget without using EDF-specific labels. All conditions serialize into the same output contract.

Evaluator scoring is condition-blinded and uses generic dimensions only. The primary comparison is EDF minus matched-neutral across 18 paired case x analyzer-executor units.

## Commands

```bash
node scripts/edf-benchmark.mjs validate
node scripts/edf-benchmark.mjs analyze
node --test
```

After the executor matrix is frozen, run adapters can be invoked with `run` and evaluation adapters with `evaluate`. Every attempt is retained.

## Claim boundary

No result exists yet. The existence of this harness is not evidence that EDF adds value. If matched-neutral is non-inferior within the preregistered margin, EDF-specific incremental value is not demonstrated and the theory should be simplified rather than the goalposts moved.
