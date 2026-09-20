# EDF Complexity Levels

Status: Working draft for EDF v0.3

EDF v0.3 keeps the same required diagnostic fields across levels. The level changes depth, evidence burden, and collaboration, not the underlying grammar.

The alternative progressive grammar described in the v1.0 draft remains a candidate design and is not part of v0.3.

## Selection Guide

| Level | Use When | Depth |
|---|---|---|
| EDF-0 | Simple, local, low-consequence, low-dispute issue | Concise pass through every required field |
| EDF-1 | Several plausible origins or moderate consequence | More explicit alternatives, evidence, and ranking rationale |
| EDF-2 | Multiple interacting systems, stakeholders, disputed origins, or higher consequence | Full network/propagation detail and stronger evidence burden |

## Common Required Fields

Every v0.3 level includes:

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

Depth should be proportional to the contemplated action.

## EDF-0 Rapid

Use EDF-0 for a concise diagnosis when the next action is bounded and reversible.

Example:

```text
Primary System: Conference room lighting
Context: Office facility
Focus: Failure to illuminate
Outcome: Room lights did not turn on
Manifestation: No illumination when the switch was activated
Origin Network: Burned-out bulb is the leading explanation
Propagation: Failed bulb prevented illumination when power was applied
Evidence: Replacement bulb restored lighting
Control Points: Replace bulb; keep a spare
Ranking: Replace bulb first; direct, low-cost, reversible
Confidence: High qualitative confidence
Diagnostic Sufficiency: Yes for replacement
Unknowns: None material to this action
Next Responsible Action: Replace bulb
```

## EDF-1 Standard

Use EDF-1 when several explanations remain plausible or the consequence warrants a more explicit evidence record.

Example:

```text
Primary System: Residential cooling system
Context: Home electrical/HVAC environment
Focus: Failure to start cooling
Outcome: Indoor temperature remained above target
Manifestation: Thermostat calls for cooling; outdoor unit hums but does not start
Origin Network: Failed capacitor is leading; thermostat/compressor remain alternatives
Propagation: Capacitor failure could prevent motor startup and cooling delivery
Evidence: Capacitor measured out of range; thermostat is calling for cooling
Control Points: Replace capacitor; verify compressor; inspect signal if issue persists
Ranking: Replace capacitor first as the least costly direct test of the leading path
Confidence: Medium qualitative confidence
Diagnostic Sufficiency: Yes for a bounded replacement-and-verify action
Unknowns: Compressor condition until startup is restored
Next Responsible Action: Replace capacitor and verify startup
```

## EDF-2 Complex

Use EDF-2 when origins interact across technical, human, organizational, environmental, regulatory, or informational systems.

Common triggers:

- multiple teams or institutions;
- long or branching propagation paths;
- disputed explanations;
- distributed control;
- high consequence;
- material evidence gaps;
- need for cross-analysis review.

EDF-2 does not require complexity for its own sake. A complex case may still have a small supported origin network.

## Escalation Triggers

| Trigger | Why it matters | Likely move |
|---|---|---|
| Leading origin is uncertain | More discriminating evidence is needed | EDF-0 -> EDF-1 |
| Multiple supported contributors interact | Network representation becomes useful | EDF-0/1 -> EDF-2 |
| Control differs materially from origin | More propagation/control analysis is useful | EDF-1 -> EDF-2 |
| Stakeholders dispute evidence or scope | Assumptions and evidence need more explicit treatment | EDF-0 -> EDF-1/2 |
| Consequences are high or action is irreversible | Stronger evidence burden is justified | increase depth regardless of label |

## Working Rule

Use the smallest depth that supports the next responsible action.

Do not escalate solely because a case sounds important, and do not remain at EDF-0 when a material unknown could reverse a high-consequence action.
