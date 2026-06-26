# Validation Case 002 - Apollo 13

Status: Working draft

Framework Version:
EDF v0.3 frozen

## Primary Finding

Apollo 13 exposed that EDF v0.3 represents static analyses well but struggles with systems whose diagnostic state changes repeatedly over time.

## Key Observation

Apollo 13 was not one diagnosis. It was a sequence of diagnoses.

## Observed Pattern

Cyclic adaptive recovery.

## Evidence

Each diagnostic perspective independently modeled some form of loop:

- Observation
- Diagnosis
- Decision
- Action
- New observation
- Reassessment

## Deferred Insight

EDF may eventually need a way to represent linked EDF analyses or evolving diagnostic state, but this is not accepted yet.

## Status

Deferred / under investigation.
