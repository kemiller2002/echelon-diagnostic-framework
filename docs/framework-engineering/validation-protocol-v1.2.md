# Validation Protocol v1.2

Status: Working draft for future runs

Validation Protocol v1.2 supersedes v1.1 for new validation work. Historical v1.1 runs remain valid historical artifacts and are not reinterpreted as v1.2 executions.

## Purpose

v1.2 changes the validation target from "does the output look structurally similar?" to a layered question:

> Under frozen, independently executable conditions, what does EDF add beyond natural analysis and matched structure, at what cost, and with what failure modes?

## Core distinctions

The protocol measures these separately:

1. Structural validity
2. Semantic completeness
3. Evidence-supported claim accuracy
4. Unsupported-claim rate
5. Control-point coverage and utility
6. Stability across models/evaluators
7. Cost, latency, retries, and invalid-run rate

No single score may be labeled "reproducibility" unless it is an independent replication measure.

## Preconditions

No confirmatory run begins until all are frozen:

- FCR
- hypothesis IDs
- evidence package IDs and hashes
- prompt condition definitions and hashes
- model/provider/version identifiers
- evaluator rubric and evaluator versions
- randomization seed and assignment procedure
- scoring code version
- success criteria
- falsification criteria
- stop rules
- analysis plan

## Required run bundle

Every attempted run receives a unique run ID and stores:

- repository commit;
- FCR ID;
- hypothesis IDs;
- evidence package ID and hash;
- prompt ID and hash;
- condition;
- model/provider/version;
- generation settings;
- start/end timestamps;
- token counts when available;
- latency;
- retries;
- cost when available;
- raw output;
- validation errors;
- evaluator outputs;
- final included/excluded status with reason.

Failed and invalid runs are preserved. They are not silently retried out of the record.

## Comparison conditions

For EDF incremental-value studies, use at minimum:

- Natural analysis
- Matched neutral structure
- EDF

The matched neutral condition should match prompt length, output budget, and field count as closely as practical without using EDF-specific causal concepts.

Specialized methods such as RCA, FMEA, or FTA may be added only when the task is appropriate to that method. They are not universal controls.

## Independence

Analytical Lenses remain useful for generating distinct reasoning perspectives, but multiple lenses from one model or agent are not independent replications.

For computational replication, use at least three materially distinct model families and at least two providers when available.

For human reproducibility research, use a separately approved human-participant protocol.

## Blinding and evaluation

Automated evaluators must be blinded to treatment condition and randomized output order.

Use at least two evaluator families for semantic judgments when feasible.

Deterministic checks should handle structure, provenance, missing fields, and forbidden evidence leakage wherever possible.

Evaluator disagreement is resolved by a frozen adjudication rule. Do not select the more favorable evaluator after seeing results.

## Evidence isolation

Evidence Package boundaries are executable constraints, not just documentation.

Longitudinal studies must include leakage checks for facts outside the allowed evidence state.

A run that uses prohibited later evidence is invalid and remains in the run ledger.

## Claim gates

A result may advance a claim only when the measured construct matches the claim.

Examples:

- structural field completion cannot advance independent reproducibility;
- origin overlap cannot by itself advance factual correctness;
- a capability matrix cannot advance comparative performance;
- a planned longitudinal architecture cannot advance evidence-sensitivity performance;
- a broader intervention list cannot advance intervention utility without a utility criterion.

## v1.2 primary benchmark

The first confirmatory use of v1.2 should be the Multi-Model EDF Structural-Value Benchmark.

Primary question:

> Does EDF add semantic diagnostic value beyond natural analysis and a matched neutral structure?

Primary falsification condition:

> If matched neutral structure is equivalent within the preregistered non-inferiority margin, EDF-specific incremental value is not demonstrated.

## Protocol-improvement claims

v1.2 does not claim to be better than v1.1 merely because it contains more controls.

A future protocol comparison must test that claim directly.

The justified claim today is narrower: v1.2 closes known validity and provenance gaps identified in the 2026-07-30 adversarial review.

## Relationship to v1.1

v1.1 introduced whole-system analytical lenses and replaced discipline silos.

v1.2 retains that useful design idea but removes unsupported causal language around "improved reproducibility" and adds:

- raw execution preservation;
- matched controls;
- model/evaluator independence;
- blinding;
- negative-run retention;
- construct-aligned metrics;
- operational cost measurement;
- explicit falsification and stop rules.

## Acceptance rule

A future Validation Case may be called confirmatory only when its REP demonstrates conformance to this protocol.

Otherwise it is exploratory.
