# Repository Health Assessment

Analysis date: 2026-09-21
Status: Current

## Executive assessment

EDF is healthier as a research repository than it was at the July review because its current claim state is now explicit, machine-readable, and enforced against public copy.

The framework remains empirically immature. The upgrade improves research integrity; it does not manufacture new evidence of diagnostic performance.

## Current measurable state

| Metric | Result | Interpretation |
|---|---:|---|
| Current hypotheses | 10 | Canonical registry |
| Current evidence records | 8 | Canonical registry; EV-EDF-008 is procedural benchmark evidence, not performance evidence |
| Current theory records | 3 | Working theory boundary |
| Cross-executor performance studies completed | 0 | Largest evidence gap |
| Controlled matched-structure benchmarks completed | 0 | EDF-specific incremental value unresolved |
| Preregistered matched-structure benchmarks | 1 | EX-EDF-001 is frozen but not yet executed |
| EX-EDF-001 synthetic held-out cases | 6 | Hidden-ground-truth design |
| EX-EDF-001 frozen artifacts | 7 | SHA-256 checked in CI |
| EX-EDF-001 analyzer runs | 0 | Confirmatory execution correctly blocked pending executor binding |
| Current confirmatory protocol | v1.2 | Requires raw run bundles and matched controls |
| Integrity tests | 3/3 pass | Repository + benchmark checks, not framework-performance evidence |
| Markdown/HTML files checked for local links | 101 | Automated repository validation |
| Required website files checked | 7 | Canonical static site |
| Duplicate deployed web stacks | 0 | Removed |
| Public claim cards registry-linked | Yes | Drift is mechanically checked |
| Frozen v0.3 artifacts hash-verified | 9 | Historical baseline cannot silently drift |

## Highest-confidence findings

- Historical R1 outputs do not constitute independent reproducibility.
- Historical exact agreement percentages cannot be recomputed from preserved artifacts.
- An FCR is necessary but insufficient for reconstructing computational execution.
- The public site must not outrank later contradictory research.
- Generic structured prompting is a credible alternative explanation for much of the apparent EDF benefit.

## Largest remaining unknown

Whether EDF-specific semantics add measurable value beyond a matched neutral structured prompt.

## Research health recommendation

Conceptual expansion remains frozen. EX-EDF-001 is now preregistered and should be executed under Validation Protocol v1.2 once real independent analyzer/evaluator bindings are frozen.

If matched neutral structure is non-inferior, simplify the theory rather than redefining success after the fact.

If EDF outperforms both natural and matched-structure controls across independent model families and blinded evaluators, promote only the specific hypotheses measured by that experiment.

## Engineering health recommendation

Keep the repository static-site-first and dependency-light.

Do not reintroduce an application framework unless a tested product requirement cannot be satisfied by semantic HTML/CSS and the cost is justified.
