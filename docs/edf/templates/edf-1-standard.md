# EDF-1 Standard

Use this when evidence and alternative explanations need to be explicit.

## Template

```md
# EDF-1

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

[How did effects move through the system?]

## Evidence

- [Evidence item]
- [Evidence item]

## Control Points

- [Candidate control point]
- [Candidate control point]

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
| Origin Network | What interacting causes are currently supported? |
| Propagation | How did the effect move from origins to outcome? |
| Evidence | What facts support this diagnosis? |
| Control Points | Where are the meaningful intervention points? |
| Control Point Ranking | Which control points rank highest and why? |
| Confidence | How certain are we given the evidence? |
| Diagnostic Sufficiency | Do we know enough to take the next responsible action? |
| Unknowns | What important uncertainty remains? |
| Next Responsible Action | What should happen next? |

## Example

```md
# EDF-1

## System Context

### Primary System

Residential cooling system

### Context

Home electrical and HVAC operating environment

### Focus

Cooling startup failure

## Outcome

House temperature remained above target during cooling demand

## Manifestations

- Thermostat calls for cooling
- Outdoor unit hums but does not start

## Origin Network

- Failed capacitor

## Propagation

Capacitor failure prevented normal startup, which blocked cooling delivery

## Evidence

- Outdoor fan hums but does not start
- Measured capacitor is out of range
- Thermostat is calling for cooling

## Control Points

- Replace capacitor
- Verify compressor operation after startup is restored
- Inspect thermostat signal only if symptoms persist

## Control Point Ranking

Replace capacitor first due to highest influence and confidence at low cost

## Confidence

Medium

## Diagnostic Sufficiency

Yes

## Unknowns

- Compressor condition until startup is restored

## Next Responsible Action

Replace capacitor and verify startup
```
