# EDF Specification v1.0 Draft

Status: Working draft

## Defining Statement

EDF is a diagnostic grammar for understanding complex systems.

It provides a structured language for expressing, comparing, calibrating, and improving diagnoses.

EDF does not tell analysts what to think.

It helps analysts state clearly:

- What system is being diagnosed
- What was observed
- What origins may have contributed
- How those origins interacted
- Where intervention may create leverage
- How confident the diagnosis should be

## Purpose

EDF is designed for diagnostic reasoning.

It answers:

> What system produced this outcome, how did it propagate, and where can intervention produce the greatest improvement?

## Primary Outputs

Every EDF investigation should produce:

1. System Context
2. Manifestations
3. Origin Network
4. Propagation
5. Control Points
6. Calibration

These six elements are the EDF core grammar.

## Core Grammar

### 1. System Context

Defines the system being diagnosed.

Questions:

- What system is this analysis about?
- What is the Primary System?
- What larger Context influences it?
- What is the Focus of this diagnosis?

Output:
A clear declaration of the diagnostic scope.

### 2. Manifestations

Describes observable outcomes, symptoms, signals, or effects.

Rules:

- Manifestations should be observable.
- Manifestations should avoid causal assumptions.
- Manifestations are not necessarily origins.

### 3. Origin Network

Identifies interacting origins capable of producing the manifestations.

EDF does not assume one root cause.

Typical descriptive categories may include:

- Technical
- Human
- Operational
- Organizational
- Informational
- Regulatory
- Environmental

These categories are helpful but not mandatory.

### 4. Propagation

Propagation describes how influence, failure, information, or constraints moved through the system from interacting origins to observable manifestations.

Origin Network is structural.

Propagation is behavioral.

Origin Network asks:

> What interacting contributors exist?

Propagation asks:

> How did influence move through the system over time to produce the manifestations?

It is not primarily about blame.

Examples:

Retail POS outage:

Origin Network:

- Database
- API
- Payment processor
- Store operations

Propagation:

```text
database latency
-> API retries
-> timeout errors
-> payment failures
-> abandoned purchases
```

Chernobyl:

Origin Network:

- Reactor design
- Operations
- Training
- Governance
- Information

Propagation:

```text
unsafe reactor state
-> power excursion
-> explosion
-> radioactive release
```

### 5. Control Points

Identifies places where intervention may alter future outcomes.

A Control Point should describe:

- Intervention
- Responsible actor
- System level
- Expected influence
- Confidence

Control Points are prioritized by expected leverage, not only by likelihood or severity.

### 6. Calibration

Describes the quality and limits of the diagnosis.

Calibration includes:

- Confidence
- Uncertainty
- Competing hypotheses
- Unresolved evidence
- Diagnostic limits
- Reconsideration triggers when appropriate

Calibration prevents EDF from overstating what the evidence supports.

## What EDF Is Not

EDF is not:

- Root Cause Analysis
- Five Whys
- Fault Tree Analysis
- FMEA
- Bayesian reasoning
- Systems Thinking
- Project management
- Execution planning
- Framework governance

EDF may work alongside these methods.

It should not claim to replace them.

## Relationship To Other Frameworks

### Framework Engineering

Framework Engineering governs how frameworks are built, validated, evolved, and released.

Framework Engineering owns:

- Constitutions
- Validation protocols
- Evidence packages
- Framework Configuration Records
- Longitudinal validation
- Evidence ledgers
- Constitutional reviews

EDF may be validated through Framework Engineering, but EDF does not own those concepts.

### Clarity

Clarity governs reasoning depth and reconsideration.

Clarity helps determine:

- Current justified commitment
- Reconsideration criteria
- Escalation
- De-escalation

Clarity can determine which EDF mode is currently justified.

### Execution

Execution governs implementation.

EDF identifies Control Points.

Execution determines how interventions are planned, resourced, owned, and carried out.

## Modes

EDF supports progressive diagnostic depth.

Mode selection is governed by Clarity.

EDF defines what diagnostic capability is present at each level.

### EDF-0

Purpose:
Simple troubleshooting.

Outputs:

- Manifestation
- Likely Origin
- Immediate Control Point

Use when:

- The issue is simple
- Consequences are low
- A direct repair or test is obvious
- Deeper diagnosis is not yet justified

### EDF-1

Purpose:
Single-system diagnosis.

Adds:

- System Context
- Basic competing origins
- Confidence statement

Use when:

- The system matters
- More than one origin is plausible
- Confidence should be stated explicitly

### EDF-2

Purpose:
Multi-origin diagnosis.

Adds:

- Origin Network
- Propagation
- Ranked Control Points

Use when:

- Origins interact
- Multiple subsystems are involved
- Intervention choice matters

### EDF-3

Purpose:
Complex sociotechnical diagnosis.

Adds:

- Full Calibration
- Multi-system analysis
- Formal Diagnostic Calibration when appropriate
- Optional Framework Engineering artifacts for formal investigations

Use when:

- Consequences are high
- Multiple stakeholders exist
- Governance or organizational behavior matters
- Evidence is disputed or evolving
- Reproducibility is important

Framework Engineering artifacts may accompany EDF-3 investigations, but they remain external to EDF.

## Relationship To Existing Methods

### RCA / Five Whys

RCA and Five Whys are faster and easier for simple causal explanations.

EDF is more useful when outcomes emerge from multiple interacting origins.

### FMEA

FMEA is stronger for prospective failure prevention and risk prioritization.

EDF is stronger for retrospective diagnosis and high-leverage system understanding.

They are complementary.

### Fault Tree Analysis

FTA is stronger for formal technical failure logic.

EDF is broader and better suited for sociotechnical diagnosis.

For highly technical reliability analysis, FTA may be used inside the technical branch of an EDF Origin Network.

## When To Use EDF

Use EDF when:

- Multiple origins may be interacting
- The system is complex
- Uncertainty matters
- Organizational or governance factors matter
- Control points must be identified
- A diagnosis must be compared or calibrated
- Simplistic root-cause language may mislead

## When Not To Use EDF

Do not use full EDF when:

- A direct repair is obvious
- Consequences are low
- Speed matters more than completeness
- Detailed technical reliability modeling is required
- Prospective design risk analysis is required

Use:

- EDF-0 for simple troubleshooting
- FTA for detailed technical fault logic
- FMEA for prospective risk analysis
- RCA / Five Whys for quick simple causal chains

## Success Criteria

A successful EDF analysis should clearly answer:

1. What system was diagnosed?
2. What was observed?
3. What origins may have contributed?
4. How did those origins interact?
5. Where should intervention occur?
6. How confident should we be?

## Design Principles

EDF should:

- Minimize unnecessary complexity
- Preserve uncertainty
- Support multiple interacting origins
- Avoid premature conclusions
- Identify high-leverage interventions
- Scale with evidence rather than assumptions
- Work with other methods rather than replace them

## Open Research Areas

The following remain under investigation and should not yet be treated as accepted EDF concepts:

- Diagnostic Topology
- Layered Origin Network categories
- Formal Control Point prioritization
- EDF visualization notation
- Quantitative calibration metrics
- Evolving diagnostic state
- Integration patterns with FMEA and FTA
