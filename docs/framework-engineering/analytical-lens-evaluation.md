# Analytical Lens Evaluation

Status: Working draft

## Purpose

Analytical Lens Evaluation defines how the validation process measures whether each lens contributes enough value to remain part of Diagnostic Calibration.

The goal is not to include more lenses.

The goal is to identify the smallest complementary set of lenses that consistently improves understanding while minimizing redundancy.

## Core Principle

Analytical lenses are not sacred.

Each lens must earn its place through evidence.

A lens should remain in the validation protocol only if it improves at least one of:

- Diagnostic completeness
- Origin-network quality
- Control-point selection
- Confidence calibration
- Synthesis quality
- Blind-spot reduction

## Analytical Lens

An Analytical Lens is a reasoning model applied to the entire declared System Context.

It is defined by how it reasons, not by what domain it is allowed to analyze.

Each lens studies the complete system.

Examples:

- Systems Lens
- Operations / Governance Lens
- Independent Investigation Lens
- Differential Diagnosis Lens
- Legal / Evidentiary Lens

## Current Default Lenses

The current default lenses are:

### 1. Systems Lens

Formerly represented by the Systems Engineer perspective.

Focus:
Architecture, interfaces, feedback, propagation, controls, resilience.

### 2. Operations / Governance Lens

Formerly represented by the Operations Executive perspective.

Focus:
Incentives, governance, authority, priority, resource allocation, organizational design.

### 3. Independent Investigation Lens

Formerly represented by the Independent Investigator perspective.

Focus:
Evidence quality, competing hypotheses, confidence, missing evidence, causal reasoning.

### 4. Differential Diagnosis Lens

Formerly represented by the Physician perspective.

Focus:
Symptoms, progression, differential diagnosis, stabilization, uncertainty, reassessment.

### 5. Legal / Evidentiary Lens

Formerly represented by the Attorney perspective.

Focus:
Burden of proof, causation, standards, documentation, accountability, defensibility.

## Evaluation Metrics

For each Validation Case, evaluate each lens using the following metrics:

### Novelty Score

How many useful observations did only this lens identify?

### Adoption Score

How many of this lens's observations or control points survived synthesis?

### Calibration Impact

How much did this lens improve other lenses during Diagnostic Calibration?

### Calibration Receptiveness

How much did this lens improve after reviewing others?

### Control Point Contribution

How many accepted high-leverage control points originated from this lens?

### Blind Spot Index

How often did this lens miss findings that most other lenses identified?

### Redundancy Score

How much does this lens overlap with another lens?

High overlap is not automatically bad, but persistent high overlap may suggest consolidation.

## Lens Retention Criteria

A lens should remain in the default validation set if it:

- Contributes unique useful observations across multiple Validation Cases
- Improves synthesis
- Improves control-point selection
- Reduces blind spots
- Does not create excessive redundancy
- Justifies the added complexity of including it

## Lens Modification Or Removal

A lens may be modified, merged, replaced, or removed if evidence shows that it:

- Contributes little unique value
- Repeatedly duplicates another lens
- Introduces noise without improving synthesis
- Systematically misses important findings
- Weakens Diagnostic Calibration

## Research Question

### RQ-002

What is the minimum set of analytical lenses that consistently produces the most complete understanding of a system?

## Caution

Do not add or remove lenses based on intuition alone.

Lens changes require evidence from multiple Validation Cases and must satisfy Concept Burden.

## Relationship To EDF

Analytical Lens Evaluation does not modify EDF.

EDF provides the diagnostic grammar.

Analytical lenses are part of the validation and Diagnostic Calibration process used to evaluate and improve EDF.
