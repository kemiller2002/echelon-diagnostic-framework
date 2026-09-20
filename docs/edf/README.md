# Echelon Diagnostic Framework (EDF)

Status: Working draft, Specification v0.3

EDF is a systems-understanding framework with a diagnostic grammar.

Its job is to make a diagnosis explicit enough to challenge: what was observed, what interacting origins may explain it, how effects propagated, what evidence supports the model, where intervention may have leverage, and whether the model is sufficient for the next responsible action.

EDF answers:

> What is happening?

Clarity answers:

> What should we do?

## Evidence Boundary

The framework is coherent enough to use as a working diagnostic grammar. Its performance claims are much less mature.

The current research record supports:

- internal demonstration that the same field structure can organize varied repository-authored failure and success narratives;
- suggestive evidence that Origin Network and Propagation are usefully distinct;
- a set of explicit hypotheses about System Context, control points, evidence evolution, and reproducibility.

The current research record does **not** establish:

- independent reproducibility;
- causal improvement from System Context;
- better control points than matched alternatives;
- evidence-sensitive updating;
- superiority to natural analysis or matched neutral structure;
- validated quantitative confidence;
- routine-use speed or learnability.

Current claim state is maintained in:

- [validation/evidence-ledger.md](validation/evidence-ledger.md)
- [../../research/registries/hypothesis-registry.json](../../research/registries/hypothesis-registry.json)
- [../../research/registries/evidence-registry.json](../../research/registries/evidence-registry.json)
- [../../research/registries/theory-registry.json](../../research/registries/theory-registry.json)

## Core Promise

Complex outcomes can involve interacting contributors, and the most useful intervention point may differ from the nearest manifestation or earliest origin.

EDF makes that model explicit. Whether EDF improves performance over simpler structured alternatives is an open empirical question, not an established fact.

## v0.3 Workflow

Every EDF analysis follows this sequence:

1. Define the System Context
   - Primary System
   - Context
   - Focus
2. Define the Outcome
3. Identify Manifestation(s)
4. Build the Origin Network
5. Trace Propagation
6. Evaluate Evidence
7. Identify Control Points
8. Rank Control Points
9. Determine Diagnostic Sufficiency

## The Five Verbs

- Define
- Observe
- Explain
- Influence
- Validate

Design rule:

> If a new concept does not improve one of the five verbs, it does not belong in EDF.

## Version Integrity

The v0.3 specification is preserved as the current frozen working baseline.

The v1.0 document is a draft candidate architecture. It is not evidence that proposed changes improve EDF, and it must not be promoted solely because it is more complete or elegant.

## Relationship To Clarity

- EDF expresses the diagnostic model.
- Diagnostic Calibration may compare multiple analyses.
- Clarity evaluates options, tradeoffs, and decisions once the understanding is sufficient for the contemplated action.

Diagnostic Sufficiency is action-relative. It is not a certificate that the diagnosis is true or complete.

## Documents

| Document | Purpose |
|---|---|
| [specification.md](specification.md) | Frozen EDF v0.3 working baseline |
| [specification-v1-draft.md](specification-v1-draft.md) | Candidate v1.0 architecture, not yet promoted |
| [constitution.md](constitution.md) | Governance principles |
| [model.md](model.md) | Core concept definitions |
| [complexity-levels.md](complexity-levels.md) | EDF-0, EDF-1, EDF-2 guidance |
| [operating-cycle.md](operating-cycle.md) | EDF in the broader operating loop |
| [diagnostic-calibration.md](diagnostic-calibration.md) | Optional multi-analysis comparison protocol |
| [../framework-engineering/claim-and-confidence-policy.md](../framework-engineering/claim-and-confidence-policy.md) | Current research claim policy |
| [../framework-engineering/validation-protocol-v1.2.md](../framework-engineering/validation-protocol-v1.2.md) | Future validation protocol |

## Current Research Priority

The highest-information next computational experiment is the Multi-Model EDF Structural-Value Benchmark.

It is designed to distinguish EDF-specific semantic value from:

- generic structure;
- prompt length;
- model choice;
- evaluator affinity.

The required design is in `research/analysis/non-human-next-experiments.md`.

Independent-human reproducibility remains important, but it should not be confused with computational replication and requires its own participant protocol.

## Starting Point

Use the smallest EDF level that reaches action-relative diagnostic sufficiency.

Treat confidence labels as qualitative unless a validated quantitative calibration method is explicitly used.
