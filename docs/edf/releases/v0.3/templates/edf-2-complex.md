# EDF-2 Complex

Use this when the outcome emerged from multiple interacting causes.

## Template

```md
# EDF-2

## System Context

### Primary System

[What system are we primarily analyzing?]

### Context

[What larger system significantly influences the primary system?]

### Focus

[What aspect of the primary system is under examination?]

## Outcome

[What happened in factual terms?]

## Manifestations

- [Observable signal]

## Origin Network

- [Contributing origin]
- [Contributing origin]
- [Contributing origin]

## Propagation

[How did effects travel through the system?]

## Evidence

- [Evidence item]
- [Evidence item]

## Control Points

- [High-leverage intervention point]
- [High-leverage intervention point]

## Control Point Ranking

[Rank by influence, controllability, cost, and confidence]

## Confidence

[Low / Medium / High]

## Diagnostic Sufficiency

[Do we understand enough to take the next responsible action?]

## Unknowns

- [Unknown]
- [Unknown]

## Next Responsible Action

[What should happen next?]
```

## Prompting Questions

| Field | Question |
|-------|----------|
| System Context | Within what declared context is this analysis valid? |
| Primary System | What system are we primarily analyzing? |
| Context | What larger system materially influences it? |
| Focus | What aspect are we examining? |
| Outcome | What happened in factual terms? |
| Manifestations | What observable signals show the outcome? |
| Origin Network | What interacting causes contributed? |
| Propagation | How did the effect move from origins to outcome? |
| Evidence | What supports the current model? |
| Control Points | Where can future outcomes be influenced most effectively? |
| Control Point Ranking | Which control points rank highest and why? |
| Confidence | How certain are we overall? |
| Diagnostic Sufficiency | Do we understand enough to take the next responsible action? |
| Unknowns | What important uncertainty remains? |
| Next Responsible Action | What should happen next? |

## Example Skeleton

```md
# EDF-2

## System Context

### Primary System

737 MAX design, certification, training, and operational safety system

### Context

Commercial aviation safety, certification, and competitive aircraft development

### Focus

Failure pathway and risk control

## Outcome

Two aircraft were lost and the fleet was grounded

## Manifestations

- Repeated loss-of-control events
- Aircraft loss
- Fleet grounding

## Origin Network

- MCAS behavior and sensor dependency
- Derivative certification assumptions
- Pilot training assumptions
- Information flow issues

## Propagation

Design assumptions, certification posture, and training assumptions interacted to produce a failure pathway that crews were not adequately prepared to manage.

## Evidence

- Accident investigation findings
- Design and certification records
- Training and operator documentation

## Control Points

- Certification strategy
- Training requirements
- Risk escalation authority
- Software design

## Control Point Ranking

Software design and escalation authority rank highest because they offer strong influence on recurrence with high confidence

## Confidence

Medium to high

## Diagnostic Sufficiency

Yes

## Unknowns

- Which control changes would have prevented both accidents most reliably
- Where governance signals were first strong enough to force escalation

## Next Responsible Action

Correct hazardous software behavior and align escalation, training, and certification with actual system risk
```
