# EDF Specification v0.3

Status: Working draft

This is the authoritative EDF v0.3 specification for the current draft.

Under EDF's constitutional version integrity rule, this defines a subsequent version rather than changing the meaning of v0.2 retroactively.

EDF evolves under constitutional rules defined in [constitution.md](constitution.md). The constitution governs framework evolution, not how to perform an EDF analysis.

## Purpose

EDF is a systems understanding framework with a diagnostic grammar.

It is used to explain how a system produced an outcome and to identify the highest-leverage opportunities for influencing future outcomes.

EDF is not primarily a decision framework. It establishes confidence in reality before decision frameworks such as Clarity evaluate what to do next.

Diagnostic Calibration is an optional extension for comparing, challenging, refining, and synthesizing multiple EDF analyses of the same system.

Framework changes should remain consistent with the constitutional rules of EDF.

## Design Rule

If a new concept does not improve one of the five verbs, it does not belong in EDF.

The five verbs are:

- Define
- Observe
- Explain
- Influence
- Validate

## Context Principle

Every EDF analysis is valid only within its declared System Context.

## Required Diagnostic Sequence

Every EDF analysis should follow this sequence:

1. Define the System Context
   - Primary System
   - Context
   - Focus
2. Define the Outcome
3. Identify Manifestation(s)
4. Build the Origin Network
5. Trace Propagation
6. Evaluate Evidence
7. Identify Control Points
8. Rank Control Points
9. Determine Diagnostic Sufficiency

## Sequence Notes

### 1. Define the System Context

Every EDF begins by declaring its System Context.

System Context has three required fields:

- Primary System
- Context
- Focus

These define what is being analyzed, what larger system materially influences it, and what aspect of the primary system is under examination.

Different analyses may disagree partly because they declared different System Contexts. That is not automatically a conflict in evidence.

### 2. Define the Outcome

State the outcome in factual terms, not interpretive terms.

Good:

- Two aircraft were lost and the fleet was grounded
- The mission landed on the moon and returned safely

Weak:

- Management failed
- The launch culture was broken

### 3. Identify Manifestation(s)

Manifestations are observable signals.

They are how the outcome appears in the world. A complex outcome may have several manifestations.

### 4. Build the Origin Network

Identify the contributing origins that made the outcome possible.

Origins may be:

- Technical
- Human
- Organizational
- Environmental
- Financial
- Regulatory

Complex outcomes should usually be modeled as an origin network rather than a single root cause.

### 5. Trace Propagation

Describe how effects moved from the origin network to the manifestations and outcome.

Propagation paths may be:

- Physical
- Informational
- Organizational
- Financial
- Human

### 6. Evaluate Evidence

Separate direct observation from inference.

Evaluate the quality of evidence supporting the current model and note meaningful gaps or disputes.

### 7. Identify Control Points

Identify where intervention could most effectively influence future outcomes.

Control points may differ from origins.

### 8. Rank Control Points

Rank candidate control points using:

- Influence
- Controllability
- Cost
- Confidence

The ranking does not need false precision. It does need a defensible ordering.

### 9. Determine Diagnostic Sufficiency

Ask:

> Do we understand enough to take the next responsible action?

Diagnosis is sufficient when additional investigation is unlikely to materially change the next responsible action.

## Required Output Elements

All EDF outputs should include, at minimum:

- System Context
- Primary System
- Context
- Focus
- Outcome
- Manifestation(s)
- Origin Network
- Propagation
- Evidence
- Control Points
- Control Point Ranking
- Confidence
- Diagnostic Sufficiency
- Unknowns
- Next Responsible Action

## Optional Extension: Diagnostic Calibration

Diagnostic Calibration is not required for routine EDF use.

Use it when:

- The situation is complex
- The stakes are high
- Causes are disputed
- Multiple disciplines are involved
- The cost of being wrong is significant

Diagnostic Calibration uses multiple independent EDF analyses to improve confidence in the diagnostic model while preserving valuable disciplinary perspectives.

See [diagnostic-calibration.md](diagnostic-calibration.md).

## Relationship To Complexity Levels

- Use [EDF-0](complexity-levels.md#edf-0-rapid) for rapid, low-complexity situations.
- Use [EDF-1](complexity-levels.md#edf-1-standard) when evidence and alternatives matter.
- Use [EDF-2](complexity-levels.md#edf-2-complex) when multiple interacting origins and layered control points must be modeled.

The sequence remains the same across levels. The depth changes.

## Relationship To Clarity

EDF creates the diagnostic grammar.

Diagnostic Calibration improves confidence in the diagnostic model.

Clarity uses the calibrated understanding to evaluate decisions.
