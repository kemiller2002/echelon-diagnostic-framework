# Deferred Insights

Status: Working draft

This document tracks findings, deferred insights, and research questions that emerged from validation work but are not accepted into EDF.

EDF v0.3 remains frozen. Entries here are evidence-first records for future evaluation, not specification changes.

Validation methodology evolves separately through the [Validation Protocol](validation-protocol.md).

## Deferred Insights

### DI-002 Apollo 13 Evolving Diagnostic State

Source:
[docs/edf/validation/case-002R1-apollo-13.md](../edf/validation/case-002R1-apollo-13.md)

Finding:
Apollo 13 suggests EDF v0.3 models static analyses well but does not yet represent a sequence of linked diagnoses cleanly.

Deferred insight:
EDF may eventually need a way to represent evolving diagnostic state or linked EDF analyses.

Status:
Deferred / under investigation.

### DI-003 Boeing Networked Origin Structure

Source:
[docs/edf/validation/case-003R1-boeing-737-max.md](../edf/validation/case-003R1-boeing-737-max.md)

Finding:
Boeing 737 MAX suggests some cases are better understood as large interacting origin networks than as simple chains or timelines.

Deferred insight:
EDF may eventually need guidance for interpreting large origin networks.

Status:
Deferred / under investigation.

### DI-004 Apollo 11 Layered Resilience / Success Diagnostics

Source:
[docs/edf/validation/case-004-apollo-11.md](../edf/validation/case-004-apollo-11.md)

Finding:
Apollo 11 suggests EDF can explain success and may eventually benefit from better language for recurring success structures such as layered resilience.

Deferred insight:
EDF may eventually need vocabulary for describing success-oriented recurring patterns without turning them into required input fields.

Status:
Deferred / under investigation.

### DI-005 Mars Climate Orbiter Interface-Centered Propagation

Finding:
Mars Climate Orbiter suggests some failures are best understood through interface-centered propagation rather than only local defects or governance narratives.

Deferred insight:
EDF may eventually need stronger guidance for interface-driven propagation patterns.

Status:
Deferred / under investigation.

### DI-006 Pixar Creative Learning System

Observation:
Pixar appears to create sustained excellence through feedback loops, psychological safety, iterative critique, and technical-creative integration.

Potential implication:
EDF may eventually need vocabulary for sustained excellence or adaptive learning systems.

Status:
Deferred / under investigation.

### DI-007 Toyota Operational Learning System

Observation:
Toyota appears to create sustained excellence through visible problems, standardized work, immediate correction, and daily improvement.

Potential implication:
EDF may eventually need vocabulary distinguishing creative learning systems from operational learning systems.

Status:
Deferred / under investigation.

### DI-008 Chernobyl As Longitudinal Reference Case

Observation:
Chernobyl's historical interpretation changed substantially over time, making it suitable for testing evidence evolution.

Potential implication:
Use Chernobyl to test Diagnostic Stability and evidence-sensitive framework behavior.

Status:
Planned.

## Research Questions

### RQ-001 Diagnostic Topology / System Behavior

Source:
[docs/edf/validation/topology-findings.md](../edf/validation/topology-findings.md)

Question:
Do Validation Cases naturally exhibit recurring system behaviors or diagnostic topologies that improve diagnosis, synthesis, or intervention if explicitly identified?

Current caution:
Diagnostic topology is not yet part of EDF and should not be added to the frozen specification without further evidence.

Status:
Deferred / under investigation.

### RQ-002 Minimum Complementary Lens Set

Source:
[analytical-lens-evaluation.md](analytical-lens-evaluation.md)

Observation:
The current five lenses may not be the optimal set. Some lenses may overlap, while other valuable reasoning models may be missing.

Potential implication:
Framework Engineering may eventually need a lens-selection protocol based on problem type.

Status:
Deferred / under investigation.

### RQ-003 Interacting Origin Categories Or Layers

Question:
Do origin networks naturally organize into recurring interacting categories or layers?

Status:
Deferred / under investigation.

### RQ-006 Sustained Excellence And Adaptive System Pattern

Question:
Are sustained excellence and resilience distinct recurring system behaviors, or are they manifestations of a broader adaptive system pattern?

Status:
Deferred / under investigation.

### RQ-008 Diagnostic Stability

Observation:
Longitudinal Reference Cases may reveal whether EDF analyses remain appropriately stable as evidence changes.

Potential implication:
Framework Engineering may need formal Diagnostic Stability metrics before EDF 1.0.

Status:
Deferred / planned.

### RQ-009 Research Question Sets

Observation:
Validation runs may not be comparable unless the research questions are frozen along with the evidence and framework versions.

Potential implication:
Framework Configuration Records should include Research Question Set versions.

Status:
Deferred / planned.
