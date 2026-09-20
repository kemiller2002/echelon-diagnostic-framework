# EDF Complexity Levels

Status: Working draft

EDF should scale with the problem. Start small and expand only when the situation requires it.

## Selection Guide

| Level | Use When | Typical Output |
|-------|----------|----------------|
| EDF-0 | Simple, local, low-dispute issue | Quick read with explicit boundary and next action |
| EDF-1 | Several plausible causes or moderate consequences | Evidence-backed diagnosis with ranked control points |
| EDF-2 | Multiple interacting systems, stakeholders, or disputed causes | Full origin network and propagation model |

## EDF-0 Rapid

Use EDF-0 when the problem is simple enough that a fast, responsible diagnosis is possible.

Required fields:

- System
- System Boundary
- Outcome
- Manifestation
- Origin Network
- Propagation
- Evidence
- Control Points
- Control Point Ranking
- Confidence
- Diagnostic Sufficiency
- Unknowns
- Next Responsible Action

Example:

```text
System: Conference room lighting
System Boundary: Fixture, bulb, wall switch, and room power supply
Outcome: Room lights did not turn on during use
Manifestation: No light when switch is activated
Origin Network: Burned-out bulb
Propagation: Failed bulb prevented illumination when power was applied
Evidence: Bulb filament visibly failed; replacement restored function
Control Points: Replace bulb; stock spare bulbs
Control Point Ranking: Replace bulb first due to high influence, high controllability, low cost, high confidence
Confidence: High
Diagnostic Sufficiency: Yes
Unknowns: None material
Next Responsible Action: Replace bulb
```

## EDF-1 Standard

Use EDF-1 when several explanations are plausible, when consequences matter, or when you need to show why one explanation is stronger than another.

Required fields:

- System
- System Boundary
- Outcome
- Manifestation
- Origin Network
- Propagation
- Evidence
- Control Points
- Control Point Ranking
- Confidence
- Diagnostic Sufficiency
- Unknowns
- Next Responsible Action

Example:

```text
System: Residential cooling system
System Boundary: Thermostat, condenser, capacitor, compressor, and power supply
Outcome: House temperature remained above target during cooling demand
Manifestation: Thermostat calls for cooling; outdoor unit hums but does not start
Origin Network: Failed capacitor; possible thermostat fault considered and deprioritized
Propagation: Capacitor failure prevented normal motor startup, which blocked cooling delivery
Evidence: Measured capacitor out of range; thermostat calling for cooling; symptoms match startup failure
Control Points: Replace capacitor; verify compressor operation; inspect thermostat signal if issue persists
Control Point Ranking: Replace capacitor first due to highest influence and confidence at low cost
Confidence: Medium
Diagnostic Sufficiency: Yes
Unknowns: Compressor condition until startup is restored
Next Responsible Action: Replace capacitor and verify startup
```

## EDF-2 Complex

Use EDF-2 when outcomes emerge from interacting technical, organizational, and human factors.

Common triggers:

- Multiple teams or institutions are involved
- The propagation path is long
- Causes are disputed
- Control is distributed across layers
- Regulatory, governance, or training assumptions matter

Required fields:

- System
- System Boundary
- Outcome
- Manifestation
- Origin Network
- Propagation
- Evidence
- Control Points
- Control Point Ranking
- Confidence
- Diagnostic Sufficiency
- Unknowns
- Next Responsible Action

## Escalation Triggers

Move up a level when one or more of these are true:

| Trigger | Why it matters | Likely move |
|---------|----------------|-------------|
| Origin is uncertain | Quick diagnosis is not defensible | EDF-0 -> EDF-1 |
| Multiple causes seem active | Single-origin framing is weak | EDF-0/1 -> EDF-2 |
| Control differs from origin | More system modeling is needed | EDF-1 -> EDF-2 |
| Stakeholders dispute the explanation | Evidence and ranking must be explicit | EDF-0 -> EDF-1 or EDF-2 |
| Consequences are high | Higher rigor is justified | EDF-0 -> EDF-1/2 |

## Working Rule

Default to the smallest EDF level that reaches diagnostic sufficiency.
