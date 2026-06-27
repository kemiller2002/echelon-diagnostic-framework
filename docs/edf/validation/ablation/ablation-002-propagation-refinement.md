# Ablation 002 - Propagation Refinement

Status: Working draft

## Purpose

Evaluate whether the refined definition of Propagation improves conceptual separation from Origin Network.

## Variable Changed

Old definition:
Propagation explains how origins interacted.

New definition:
Propagation describes how influence, failure, information, or constraints moved through the system from interacting origins to observable manifestations.

## Hypothesis

The refined definition will improve conceptual separation without adding a new EDF concept.

## Case

Regional Retail POS Outage.

## Baseline Grammar

- System Context
- Manifestations
- Origin Network
- Propagation
- Control Points
- Calibration

## Finding

The refined definition made Propagation more distinct.

Before refinement:
Propagation appeared partially redundant with Origin Network.

After refinement:
Origin Network represented structure.
Propagation represented behavior.

## Updated Ablation Results

| Removed Element | Previous Degradation | Current Degradation |
|---|---|---|
| System Context | High | High |
| Manifestations | High | High |
| Origin Network | Very High | Very High |
| Propagation | Medium | High |
| Control Points | Very High | Very High |
| Calibration | High | High |

## Conclusion

Propagation remains part of EDF core grammar.

## Engineering Finding

### EF-EDF-001 - Structural vs. Behavioral Separation

Observation:
Refining Propagation from "how origins interacted" to "how influence moved through the system" increased its distinctiveness during ablation testing.

Implication:
EDF contains two complementary but non-overlapping concepts:

- Origin Network: structural model of interacting contributors
- Propagation: behavioral model of movement through the system

Status:
Supported by simulated ablation testing.
Needs future independent validation.
