# Boeing 737 MAX

Status: Working draft

This is a concise EDF-2 style example, not a full technical or legal account.

## EDF Read

| Element | Draft |
|---------|-------|
| System | 737 MAX design, certification, training, and operational safety system |
| System Boundary | Aircraft software and sensors, manufacturer design decisions, certification approach, pilot training assumptions, and operator-facing information relevant to the failure pathway |
| Outcome | Two aircraft were lost and the fleet was grounded |
| Manifestations | Repeated loss-of-control events; aircraft loss; fleet-wide grounding; operator confusion around failure behavior |
| Origin Network | Technical: MCAS behavior and sensor dependency. Organizational: derivative certification assumptions. Human: pilot training assumptions. Informational: information flow issues. Financial and competitive pressure also shaped system choices |
| Propagation | Design assumptions shaped software behavior; certification posture constrained risk framing; training assumptions reduced pilot preparation; information gaps limited shared understanding across the system |
| Evidence | Accident investigation findings; design and certification records; training materials; operator documentation |
| Control Points | Certification strategy; training requirements; risk escalation authority; software design |
| Control Point Ranking | 1. Software design. 2. Risk escalation authority. 3. Training requirements. 4. Certification strategy. Ranking reflects influence on recurrence, practical controllability, implementation cost, and confidence |
| Confidence | High that the outcome was multi-origin rather than single-cause |
| Diagnostic Sufficiency | Yes for identifying the next responsible system actions |
| Unknowns | Which intervention timing would have reduced risk earliest; precise weighting across organizational and regulatory contributors |
| Next Responsible Action | Correct hazardous software behavior, strengthen escalation authority, and align training and certification with actual system risk |

## Why This Example Matters

This is the type of case where EDF-2 is necessary. A single-cause explanation does not capture the interacting technical, organizational, and regulatory contributors.

The value of EDF here is not just identifying what malfunctioned. It is mapping the origin network and locating control across several layers.

## Concise Takeaway

The visible manifestation was aircraft loss, but the control points extended well beyond software into certification, training, escalation, and system design choices.
