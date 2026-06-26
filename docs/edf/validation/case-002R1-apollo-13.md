# Validation Case 002R1 - Apollo 13

Status: Working draft

- FCR: FCR-000002
- EDF: v0.3 frozen
- Validation Protocol: v1.1
- Analytical Lens Set: v1.0

## Purpose

Test whether v1.1 improves reproducibility on a dynamic case.

## Historical Anchor

Apollo 13 began as a lunar landing mission but became a mission recovery after an oxygen tank explosion disabled major service-module systems. The crew returned safely.

## Natural Analysis Summary

- Systems Lens: Design, testing, redundancy, and resource-flow failure/recovery
- Governance Lens: Mission Control, authority, preparation, disciplined prioritization
- Investigation Lens: Evidence interpretation under uncertainty
- Differential Diagnosis Lens: Repeated stabilization after new symptoms
- Legal / Evidentiary Lens: Reasonable decisions under changing evidence and known constraints

## EDF Analysis Summary

Common System Context:

- Primary System: Apollo 13 mission recovery system
- Context: Apollo spacecraft, Mission Control, Apollo Program
- Focus: Varied by lens

Shared manifestations:

- Oxygen tank explosion
- Loss of service-module capability
- Power, oxygen, water, navigation, and CO2 constraints
- Aborted lunar landing
- Safe return

Shared origin network:

- Oxygen tank design/change history
- Ground-test damage not recognized
- Electrical/insulation vulnerability
- Redundancy limits
- Mission Control preparedness
- Simulation culture
- Crew-ground communication
- Improvised life-support adaptation

Shared propagation:

```text
Tank vulnerability
-> fan activation / ignition
-> oxygen tank failure
-> service-module degradation
-> mission objective changes
-> lunar module lifeboat strategy
-> repeated diagnosis / intervention cycles
-> safe return
```

## RC-7 Reproducibility

- System Context: High
- Manifestations: Very High
- Major Origins: High
- Propagation: Medium-High
- Control Points: High
- Overall Reproducibility: High

## Key Finding

Validation Protocol v1.1 improved comparability, but Apollo 13 still exposes that EDF v0.3 strains to represent repeated diagnostic cycles over time.

## Status

Evolving diagnostic state remains a deferred insight, not an accepted EDF concept.
