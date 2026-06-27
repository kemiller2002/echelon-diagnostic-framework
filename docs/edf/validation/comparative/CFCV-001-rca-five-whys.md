# CFCV-001 - RCA / Five Whys

Status: Working draft

Use case:
Chernobyl modern evidence state.

## Summary

RCA / Five Whys is fast, simple, and easy to learn. It is strong for simple retrospective causal explanation.

RCA likely produces a causal chain such as:

```text
Reactor exploded
-> power excursion
-> unsafe test conditions and operator actions
-> procedure and safety system mishandling
-> inadequate training, supervision, and safety culture
```

## Finding

RCA is useful but insufficient for Chernobyl because it compresses multiple interacting origins into a chain.

EDF produced a broader diagnostic model:

- System Context
- Origin Network
- Multiple interacting technical, operational, organizational, governmental, and informational origins
- Broader control points
- Explicit evidence handling and confidence

## Comparison

| Capability | RCA / Five Whys | EDF | Result |
|---|---|---|---|
| Speed | Strong | Weaker | RCA |
| Ease of Learning | Strong | Weaker | RCA |
| Simple cases | Strong | Strong if EDF-0 | Tie |
| Complex origin networks | Weak | Strong | EDF |
| Evidence evolution | Weak | Strong via Framework Engineering | EDF |
| Control-point breadth | Limited | Strong | EDF |
| Risk of oversimplification | High | Lower | EDF |

## Conclusion

RCA is better when the problem is simple and speed matters.

EDF is better when the outcome emerges from multiple interacting systems.

## Architectural Finding

Some sophistication used during EDF validation belongs to Framework Engineering, not EDF itself. EDF should remain smaller and focus on:

- System Context
- Manifestations
- Origin Network
- Propagation
- Control Points
- Calibration
