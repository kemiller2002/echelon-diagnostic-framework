# Longitudinal Reference Cases

Status: Working draft

## Purpose

A Longitudinal Reference Case examines the same historical or synthetic system under multiple frozen Evidence Packages.

Its purpose is to test whether an analysis updates appropriately as the *available evidence* changes.

It does not assume that later evidence is always better, nor that the underlying historical reality changed.

## Core Idea

The same event is analyzed multiple times with different declared evidence states.

Each run receives its own Framework Configuration Record and immutable execution bundle.

Unless intentionally manipulated, keep constant:

- EDF version;
- Validation Protocol version;
- prompt condition;
- model/provider/version;
- evaluator configuration;
- Research Question Set.

The intended independent variable is the Evidence Package. Model prior knowledge is a contamination risk and must be tested rather than assumed away.

## Example: Chernobyl

Chernobyl remains a candidate first longitudinal historical case:

- Run A: FCR-0100 / EP-013A / evidence reasonably available in 1986.
- Run B: FCR-0101 / EP-013B / INSAG-7 evidence state.
- Run C: FCR-0102 / EP-013C / later historical evidence state.

Historical model priors can leak later facts into earlier runs, so the design should include explicit leakage canaries and a synthetic longitudinal case with known ground truth.

## Candidate Construct: Diagnostic Stability

Diagnostic Stability is a proposed construct for describing how much an analysis changes across evidence states.

The desired behavior is a hypothesis:

- facts supported in every package remain stable;
- newly supported information changes the parts of the diagnosis it bears on;
- unsupported later facts do not leak into earlier evidence states;
- confidence changes have explicit evidence reasons;
- control-point changes can be traced to changed evidence.

The repository has not yet validated a quantitative Diagnostic Stability metric.

## Candidate Comparison Dimensions

Future longitudinal work may compare:

- System Context
- Manifestations
- Origin Network
- Propagation
- Control Points
- Confidence rationale
- Diagnostic Sufficiency
- Interpretation changes
- evidence leakage

Do not aggregate these into a single score until construct validity and weighting are established.

## Experimental Design

1. Define the Longitudinal Reference Case.
2. Freeze the Research Question Set.
3. Freeze and hash each Evidence Package.
4. Create one FCR per run.
5. Freeze prompts, models, evaluators, scoring, randomization, and stop rules.
6. Execute each evidence state in isolated sessions.
7. Preserve every attempted run, including failures.
8. Run leakage checks.
9. Compare changes under the preregistered analysis plan.
10. Preserve earlier runs unchanged.

## Research Question Sets

A Research Question Set freezes what each run is trying to answer.

Example RQS-001:

- How did the system produce the observed outcome given only the declared evidence?
- Which origins are supported at this evidence state?
- Which control points are supportable at this evidence state?
- Which conclusions should remain unchanged?
- Which conclusions should change if the next evidence package is introduced?

## Evidence Packages

Changing the allowed evidence requires a new Evidence Package and a new FCR.

Evidence Isolation is necessary but insufficient: a computational run also needs controls against model-prior leakage and complete execution provenance.

## Relationship To Framework Engineering

Longitudinal Reference Cases are experiments on framework behavior, not proof of framework capability by design alone.

The first completed controlled study should update HY-EDF-006.

## Caution

Do not describe evidence-sensitive updating as a demonstrated EDF strength until a completed longitudinal study passes preregistered leakage and stability criteria.
