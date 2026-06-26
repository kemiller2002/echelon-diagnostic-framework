# Framework Configuration Record (FCR)

Status: Working draft

## Purpose

The Framework Configuration Record (FCR) defines the complete, immutable configuration under which a Validation Case is conducted.

Its purpose is to ensure reproducibility, traceability, and configuration integrity.

Every Validation Case begins by creating and freezing an FCR.

No analysis may begin until the FCR has been frozen.

## Core Principle

The FCR is the single source of truth for every experiment.

Every conclusion, Engineering Change Request, Deferred Insight, and framework evolution must be traceable to one or more Framework Configuration Records.

## Configuration Integrity

Framework Configuration Integrity requires that every Validation Case records the exact versions of all framework components that influenced the analysis.

No component may change once an FCR has been frozen.

If any component changes, a new FCR must be created.

## FCR Structure

```yaml
Framework Configuration Record:
  id: FCR-000001

  status: Frozen

  created:
    date:
    author:

  frameworks:
    edf:
      version:

    clarity:
      version:
      participation:
        Active | Observation Only | Not Included

    execution:
      version:
      participation:

  framework_engineering:
    version:

  validation_protocol:
    version:

  analytical_lens_set:
    version:
    lenses:
      - Systems
      - Governance
      - Investigation
      - Differential Diagnosis
      - Legal / Evidentiary

  reference_suite:
    version:

  validation_case:
    id:
    version:

  evidence_package:
    id:
    version:

  prediction_set:
    version:

  constitution:
    version:

  release_criteria:
    version:

  deferred_insight_snapshot:
    version:

  notes:
```

## Lifecycle

```text
Draft
↓
Review
↓
Freeze
↓
Prediction Lock
↓
Validation
↓
Diagnostic Calibration
↓
Synthesis
↓
Engineering Change Requests
↓
Archive
```

## Rules

### Rule 1

No Validation Case begins without a frozen FCR.

### Rule 2

Every experiment references exactly one FCR.

### Rule 3

Changing any versioned component requires a new FCR.

### Rule 4

Engineering Change Requests reference the FCR that generated them.

### Rule 5

Deferred Insights reference the originating FCR.

### Rule 6

Published framework versions record the FCRs that provided supporting evidence.

## Traceability

Example:

```text
FCR-000021
↓
Validation Case 003
↓
Deferred Insight DI-005
↓
Engineering Change Request ECR-004
↓
EDF v0.4
↓
FCR-000022
```

Every framework evolution can therefore be traced to the evidence that justified it.

## Relationship To RC-7 (Reproducibility)

Framework reproducibility depends on configuration reproducibility.

Two analyses cannot be meaningfully compared unless they identify the complete Framework Configuration Record used to produce them.

Therefore, the FCR is a prerequisite for RC-7.

## Constitutional Principle

### Configuration Integrity

Every conclusion produced by Framework Engineering shall be traceable to a frozen Framework Configuration Record.

Configuration changes invalidate direct comparison with previous experiments unless explicitly documented through a new FCR.

## Future Extensions

The FCR is intentionally extensible.

Future framework components may be added without altering the role of the FCR as the authoritative configuration record.

Examples include:

- Framework Quality Rubric version
- Lens Evaluation version
- Statistical Evaluation version
- Automation Tooling version
- AI Assistant Configuration version
- Prompt Library version
- External Reviewer Panel version

These additions should be versioned independently and included only when they materially influence validation results.
