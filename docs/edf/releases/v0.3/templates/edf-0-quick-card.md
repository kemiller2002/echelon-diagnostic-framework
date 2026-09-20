# EDF-0 Quick Card

Use this when a fast diagnosis is enough.

## Template

```md
# EDF-0

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

## Propagation

[How did the effect move through the system?]

## Evidence

- [Evidence item]

## Control Points

- [Candidate control point]

## Control Point Ranking

[Which control point should be acted on first and why?]

## Confidence

[Low / Medium / High]

## Diagnostic Sufficiency

[Do we understand enough to take the next responsible action?]

## Unknowns

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
| Origin Network | What caused it, even in a simple form? |
| Propagation | How did the effect move from cause to outcome? |
| Evidence | What supports this read? |
| Control Points | Where could we intervene? |
| Control Point Ranking | Which control should go first based on influence, controllability, cost, and confidence? |
| Confidence | How certain are we? |
| Diagnostic Sufficiency | Do we know enough to take the next responsible action? |
| Unknowns | What remains uncertain but not blocking? |
| Next Responsible Action | What should happen next? |

## Example

```md
# EDF-0

## System Context

### Primary System

Conference room lighting

### Context

Office facility operations

### Focus

Basic lighting reliability

## Outcome

Room lights did not turn on during use

## Manifestations

- No illumination when switch is activated

## Origin Network

- Burned-out bulb

## Propagation

Failed bulb prevented illumination when power was applied

## Evidence

- Replacement bulb restored lighting

## Control Points

- Replace bulb
- Keep spare bulbs in stock

## Control Point Ranking

Replace bulb first due to high influence, high controllability, low cost, and high confidence

## Confidence

High

## Diagnostic Sufficiency

Yes

## Unknowns

- None material

## Next Responsible Action

Replace bulb
```
