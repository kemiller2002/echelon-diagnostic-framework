# Validation Protocol v1.1

Status: Working draft

## Purpose

Validation Protocol defines how EDF Validation Cases are conducted.

It is separate from the EDF specification.

Changes to the Validation Protocol improve the quality of framework evaluation without changing the framework itself.

EDF and the Validation Protocol evolve independently.

Validation Cases should be conducted under a frozen [Framework Configuration Record](framework-configuration-record.md).

Every Validation Case records EDF version, Validation Protocol version, Framework Engineering version, Analytical Lens Set version, Evidence Package, Prediction Set, and FCR.

## Summary Of Changes

Version 1.1 replaces discipline-specific analysis with analytical-lens analysis.

This corrects a methodological weakness identified during the first Validation Cases.

## Problem Identified

Validation Protocol v1.0 unintentionally encouraged Diagnostic Perspectives to analyze only the portions of a system traditionally associated with their professions.

Examples:

- Systems Engineer -> Technical subsystem
- Operations Executive -> Organization
- Physician -> Human diagnosis
- Attorney -> Legal responsibility

This fragmented the analysis.

The perspectives often examined different systems instead of examining the same system through different reasoning models.

## New Principle

### Whole-System Analysis

Every Analytical Lens analyzes the entire declared System Context.

Lenses do not own portions of the system.

Instead, they apply different reasoning models to the same complete system.

## Analytical Lens

Diagnostic Perspectives are now Analytical Lenses.

An Analytical Lens is defined by how it reasons, not what it studies.

Each lens evaluates every relevant aspect of the system.

The analytical lens influences:

- Observations
- Evidence interpretation
- Confidence
- Prioritization
- Control-point selection

It does not limit the scope of analysis.

## Current Default Lenses

### Systems Lens

Analytical Lens:

- System architecture
- Interfaces
- Propagation
- Feedback
- Control mechanisms
- Resilience

The lens evaluates technical, organizational, human, and environmental elements as interacting systems.

### Operations / Governance Lens

Analytical Lens:

- Governance
- Incentives
- Resource allocation
- Organizational design
- Strategic control
- Operational resilience

The lens evaluates the entire system through organizational effectiveness.

### Independent Investigation Lens

Analytical Lens:

- Observation
- Evidence quality
- Competing hypotheses
- Confidence
- Uncertainty
- Causal reasoning

The lens evaluates the complete evidence chain regardless of domain.

### Differential Diagnosis Lens

Analytical Lens:

- Manifestations
- Differential diagnosis
- Progression
- Protective factors
- Stabilization
- Uncertainty

The lens treats the entire system as a diagnostic subject rather than limiting analysis to medical analogies.

### Legal / Evidentiary Lens

Analytical Lens:

- Evidence
- Causation
- Standards
- Burden of proof
- Accountability
- Documentation

The lens evaluates the entire system using evidentiary reasoning rather than focusing solely on legal liability.

## Baseline Comparison

Natural Analysis replaces RCA as the baseline.

Each lens first performs a Natural Analysis without EDF.

Then the same lens performs an EDF Analysis using the same evidence.

This isolates the effect of EDF.

## Expected Outcomes

Validation Protocol v1.1 predicts:

- Greater overlap in identified origins
- Stronger disagreement about importance rather than existence
- Improved Diagnostic Calibration
- Richer cross-disciplinary synthesis
- More complete Control Point identification

## Prediction Lock

### Prediction P-006

Validation Protocol v1.1 will produce more complete and comparable analyses because every Diagnostic Perspective examines the same complete system through a different analytical lens.

This prediction is frozen before additional Validation Cases are analyzed.

## Validation Flow

1. Freeze FCR.
2. Prediction Lock.
3. Natural Analysis by each Analytical Lens.
4. EDF Analysis by each Analytical Lens.
5. Compare Natural Analysis vs EDF Analysis.
6. Diagnostic Calibration.
7. Synthesist Report.
8. RC-7 Reproducibility Evaluation.
9. Framework Evaluation.
10. Deferred Insights and ECR candidates.

## Configuration Requirement

No Validation Case begins without a frozen Framework Configuration Record.

If the framework version, validation protocol, analytical lens set, evidence package, prediction set, or any other versioned component changes, a new FCR must be created.

## Research Question

### RQ-003

How does EDF change the reasoning produced by each Analytical Lens compared with that lens's Natural Analysis?

## Important Distinction

Validation Protocol v1.1 does not modify EDF.

EDF v0.3 remains unchanged.

This update improves the methodology used to evaluate EDF rather than the framework itself.

## Research Principle

Every Diagnostic Perspective studies the same complete system.

The value of multiple perspectives comes from differences in reasoning, not differences in scope.

The objective is not to divide the system among experts.

The objective is to understand how different analytical lenses interpret the same evidence.

## Version History

### Validation Protocol v1.0

Diagnostic Perspectives naturally analyzed the domains associated with their professional expertise.

### Validation Protocol v1.1

Analytical Lenses analyze the entire declared System Context using distinct reasoning models.

This change is considered a methodological improvement pending validation through re-analysis of the Reference Suite.
