# Framework Engineering

Status: Working draft

Framework Engineering tracks how EDF is evaluated, challenged, and improved without changing the frozen meaning of historical EDF versions.

EDF and framework-engineering methods evolve independently.

## Current Governance

Current research interpretation is governed by:

- [Claim and Confidence Policy](claim-and-confidence-policy.md)
- [Validation Protocol v1.2](validation-protocol-v1.2.md) for future runs
- [Hypothesis Registry](../../research/registries/hypothesis-registry.json)
- [Evidence Registry](../../research/registries/evidence-registry.json)

Historical v1.1 validation artifacts remain preserved. They do not automatically carry forward their original confidence labels.

## Lifecycle

| Document | Purpose |
|---|---|
| [../framework-configurations/README.md](../framework-configurations/README.md) | Index of frozen Framework Configuration Records |
| [framework-configuration-record.md](framework-configuration-record.md) | Defines frozen experiment configuration |
| [validation-protocol.md](validation-protocol.md) | Historical/current v1.1 protocol used by prior cases |
| [validation-protocol-v1.2.md](validation-protocol-v1.2.md) | Protocol for future controlled runs |
| [claim-and-confidence-policy.md](claim-and-confidence-policy.md) | Defines evidence language, claim states, independence, and promotion gates |
| [longitudinal-reference-cases.md](longitudinal-reference-cases.md) | Defines evidence-state evolution studies |
| [analytical-lens-evaluation.md](analytical-lens-evaluation.md) | Evaluates whether analytical lenses earn their place |
| [deferred-insights.md](deferred-insights.md) | Records findings not accepted into EDF |
| [../edf/validation/evidence-ledger.md](../edf/validation/evidence-ledger.md) | Human-readable current claim state |

## Working Rules

- Historical artifacts are preserved.
- Current summaries must reflect the strongest contradictory or limiting evidence.
- Structural conformity is not reproducibility.
- Planned architecture is not demonstrated capability.
- A method-theory comparison is not a performance comparison.
- Exact quantitative claims require recomputable provenance.
- Every future confirmatory computational run must preserve immutable execution bundles, including failures.
- Public claims may not be stronger than the current hypothesis registry.

## Longitudinal Reference Cases

Longitudinal Reference Cases examine the same system under multiple frozen Evidence Packages.

They are intended to test whether a framework changes appropriately when evidence changes. The repository currently contains the architecture for this work, not a completed demonstration of evidence-sensitive updating.

See [longitudinal-reference-cases.md](longitudinal-reference-cases.md).
