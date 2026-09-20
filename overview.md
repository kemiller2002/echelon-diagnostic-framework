---
title: Echelon Diagnostic Framework (EDF) - Legacy Overview
status: Superseded
---

# Legacy Overview

This file was the repository's early conceptual overview. It is no longer normative because later EDF versions, validation work, and the 2026 adversarial evidence review changed both terminology and the justified strength of several claims.

Use these sources instead:

- [EDF v0.3 specification](docs/edf/specification.md)
- [Current EDF orientation](docs/edf/README.md)
- [Current evidence ledger](docs/edf/validation/evidence-ledger.md)
- [Claim and Confidence Policy](docs/framework-engineering/claim-and-confidence-policy.md)
- [Current hypothesis registry](research/registries/hypothesis-registry.json)

## Important correction to the former evidence hierarchy

The previous version of this file presented evidence as a single ordinal ladder from opinion through "physical proof."

That model is withdrawn.

Evidence quality is multidimensional. Relevant dimensions can include directness, validity, reliability, independence, measurement error, provenance, relevance to the claim, representativeness, susceptibility to bias, and reproducibility. A physical observation is not automatically stronger than a well-designed independent measurement, and documentation is not automatically stronger than observation.

EDF should therefore evaluate evidence against the claim being made rather than assign a universal evidence rank.
