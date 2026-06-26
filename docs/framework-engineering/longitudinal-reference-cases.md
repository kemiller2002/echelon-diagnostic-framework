# Longitudinal Reference Cases

Status: Working draft

## Purpose

A Longitudinal Reference Case examines the same system or event under multiple frozen Evidence Packages.

Its purpose is to test whether a framework evolves appropriately as evidence changes over time.

Traditional Validation Cases ask:

> Can the framework explain this system?

Longitudinal Reference Cases ask:

> Does the framework update its understanding proportionally and responsibly as better evidence becomes available?

## Core Idea

The same event may be analyzed multiple times using different Evidence Packages.

Each run receives its own Framework Configuration Record.

The framework version, Validation Protocol version, Analytical Lens Set version, and Research Question Set should remain constant unless the experiment intentionally changes them.

The independent variable is the Evidence Package.

## Example: Chernobyl

Chernobyl should become the first Longitudinal Reference Case.

Proposed structure:

### LRC-001 - Chernobyl

Run A:

- FCR-0100
- Evidence Package: EP-013A
- Evidence State: Evidence reasonably available in 1986

Run B:

- FCR-0101
- Evidence Package: EP-013B
- Evidence State: INSAG-7 revised understanding

Run C:

- FCR-0102
- Evidence Package: EP-013C
- Evidence State: Modern historical understanding

## Why Chernobyl Fits

Chernobyl is especially valuable because the official understanding evolved over time.

Early explanations emphasized operator error.

Later investigations and analyses gave greater weight to reactor design flaws, safety culture, regulatory weakness, procedural issues, and organizational/governmental context.

This makes it a strong test of whether EDF and Framework Engineering can handle evidence evolution without defending the first diagnosis.

## Diagnostic Stability

Diagnostic Stability is the degree to which an analysis remains appropriately stable while incorporating newly available evidence.

A good framework should be neither rigid nor unstable.

Bad behavior:

- Conclusions do not change when important new evidence appears.
- Conclusions change completely when new evidence only modestly shifts understanding.

Good behavior:

- Stable elements remain stable.
- New evidence changes the parts of the analysis it actually affects.
- Confidence improves or shifts in proportion to the evidence.
- Control points become more accurate as evidence improves.

## Diagnostic Stability Measures

Compare each run across evidence packages using:

- System Context Stability
- Manifestation Stability
- Origin Network Stability
- Control Point Stability
- Confidence Stability
- Diagnostic Sufficiency Stability
- Interpretation Shift

## Experimental Design

For a Longitudinal Reference Case:

1. Define the Longitudinal Reference Case.
2. Define the Research Question Set.
3. Define each Evidence Package.
4. Create and freeze one Framework Configuration Record per run.
5. Run the analysis separately for each Evidence Package.
6. Compare results across runs.
7. Measure Diagnostic Stability.
8. Log Deferred Insights and ECR candidates.
9. Do not retroactively alter earlier runs.

## Research Question Sets

A Research Question Set defines the questions being answered during a validation run.

Research questions must be frozen as part of the Framework Configuration Record.

This prevents accidental comparison of analyses that are answering different questions.

Example:

### RQS-001

- How did the system produce the observed outcome?
- What origins were visible given the evidence available at the time?
- What control points would have been identifiable at the time?
- How does the analysis change as new evidence packages become available?

## Evidence Packages

An Evidence Package defines the evidence available to a validation run.

Evidence Packages must be versioned.

Changing the available evidence requires a new Evidence Package and a new FCR.

## Configuration Requirements

Each Longitudinal Reference Case run must record:

- Framework Configuration Record ID
- EDF version
- Validation Protocol version
- Framework Engineering version
- Analytical Lens Set version
- Research Question Set version
- Evidence Package ID and version
- Validation Case ID and version

## Relationship To Framework Engineering

Longitudinal Reference Cases test Framework Engineering itself.

They help determine whether frameworks:

- Update responsibly as evidence improves
- Preserve stable conclusions when appropriate
- Avoid defending outdated conclusions
- Maintain traceability across evidence changes

## Relationship To EDF

Longitudinal Reference Cases do not modify EDF directly.

They provide evidence that may later support ECRs or Deferred Insights.

## Caution

Do not treat later evidence as if it were available during earlier runs.

Each run must respect the evidence state defined by its Evidence Package.

This is essential for avoiding hindsight bias.
