# EDF Model

Status: Current interpretation guidance

The byte-for-byte v0.3 version is preserved at [releases/v0.3/model.md](releases/v0.3/model.md).

This page contains post-v0.3 interpretive clarifications identified by later research. It does not retroactively change the frozen v0.3 artifact.

## Core Concepts

### System Context

System Context is the opening frame for every EDF analysis.

It declares what is being analyzed, what larger systems significantly shape it, and what aspect is under examination.

Guiding question:

> Within what declared context is this diagnosis valid?

Every EDF analysis is valid only within its declared System Context.

### Primary System

The Primary System is the system this EDF analysis is primarily analyzing.

Guiding question:

> What system are we primarily trying to understand?

### Context

Context is the larger system or systems that significantly influence the Primary System.

It helps explain why two sound analyses may differ: one may model a wider context than another.

Guiding question:

> What larger system materially shapes the primary system?

### Focus

Focus is the aspect of the Primary System under examination.

It narrows the analysis to the relevant dimension without pretending the whole system is being explained at once.

Guiding question:

> What aspect of the primary system are we examining?

### System Boundary

System Boundary is an older or narrower way of thinking about scope.

It can still be useful when an analysis needs a sharp inside-versus-outside cut, but in v0.3 it is subordinate to System Context rather than the primary opening concept.

### Outcome

The outcome is the factual result to be explained.

It should be stated in observable terms, not interpretive terms.

Guiding question:

> What happened?

### Manifestation

Manifestations are observable signals of the outcome.

A complex outcome may have several manifestations. They are what can be seen, measured, reported, or directly observed.

Guiding question:

> What signals show that this outcome occurred?

### Origin

An origin is a point where a contributing cause began.

Origins may be technical, human, organizational, environmental, financial, or regulatory.

Guiding question:

> Where did this contributing cause begin?

### Origin Network

The origin network is the set of contributing origins currently supported by the diagnostic model.

EDF uses a network representation so interacting contributors can be expressed when the evidence supports them. It should not invent multiple causes merely because a case is complex.

Guiding question:

> What set of interacting causes made this outcome possible?

### Propagation

Propagation describes how effects traveled from the origin network to the manifestations and outcome.

Propagation paths may be physical, informational, organizational, financial, or human.

Guiding question:

> How did the effect move through the system?

### Evidence

Evidence is the observed, measured, documented, or independently supported basis for the current diagnostic model.

EDF should distinguish evidence from interpretation.

Guiding question:

> What supports this explanation?

### Confidence

Confidence is a qualitative statement about how strongly the current evidence supports the diagnostic model.

In EDF v0.3 these labels are not calibrated probabilities. Confidence should reflect evidence quality, consistency, competing explanations, and unresolved uncertainty.

Suggested labels:

- Low
- Medium
- High

### Control Point

A control point is a place where intervention can meaningfully influence future outcomes.

Control points may differ from origins. EDF therefore asks analysts to compare candidate intervention points rather than assuming the earliest origin or nearest manifestation is the best place to act. "Leverage" is currently a heuristic judgment, not a validated quantitative metric.

Guiding question:

> Where can we most effectively influence what happens next?

### Diagnostic Sufficiency

Diagnostic sufficiency is the action-relative threshold at which the current understanding is good enough to support the next responsible action, given its consequence, reversibility, and material unknowns.

Guiding question:

> Do we understand enough to take the next responsible action?

## Concept Relationships

| Concept | Relationship |
|---------|--------------|
| System Context -> Primary System | The analysis declares its main unit |
| System Context -> Context | The analysis declares meaningful external influence |
| System Context -> Focus | The analysis declares what aspect is under examination |
| Outcome -> Manifestations | The outcome appears through observable signals |
| Origin Network -> Propagation | Interacting causes move through the system |
| Evidence -> Confidence | Evidence quality constrains certainty |
| Control Points -> Next Responsible Action | Ranked control points shape what should be acted on |
| Confidence + consequence + reversibility + unknowns -> Diagnostic Sufficiency | Sufficiency is relative to the next action, not a claim of complete truth |

## Minimal Diagnostic Grammar

```text
System Context
-> Primary System
-> Context
-> Focus
-> Outcome
-> Manifestations
-> Origin Network
-> Propagation
-> Evidence
-> Control Points
-> Ranked Control Points
-> Diagnostic Sufficiency
```
