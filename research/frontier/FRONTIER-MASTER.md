---
id: RFA-EDF-2026-001
title: EDF Repository Research Frontier
version: 1.0.0
status: active
analysis_date: 2026-07-28
---

# EDF Repository Research Frontier

## Executive finding

EDF has a coherent v0.3 diagnostic grammar and encouraging internal validation across failure, recovery, and success cases. The repository's knowledge boundary is not another conceptual expansion. It is credible external validation: independent people must use frozen configurations, and the planned Chernobyl longitudinal study must determine whether EDF changes its conclusions proportionally as evidence changes.

All confidence labels in this analysis distinguish repository evidence from real-world validation. Case analyses produced by the repository support hypotheses; they do not substitute for independent analysts, blinded comparisons, or field outcomes.

## What is currently known

- EDF v0.3 consistently requires System Context, Outcome, Manifestations, Origin Network, Propagation, Evidence, Control Points, ranking, and Diagnostic Sufficiency.
- Internal case analyses indicate System Context improves structural comparability.
- Ablation 002 supports separating Origin Network (structure) from Propagation (behavior).
- The same grammar can describe failures, recovery, and sustained success.
- RCA/Five Whys, FMEA, and FTA are complementary; EDF is broader for sociotechnical diagnosis but weaker in speed, mature risk scoring, and formal technical logic.
- Validation Protocol v1.1 is methodologically stronger than v1.0 because every lens analyzes the same declared system.

## What remains unknown

1. Whether independent human analysts can learn and reproduce EDF structures.
2. Whether EDF outperforms natural analysis or established methods under controlled conditions.
3. Whether EDF updates proportionally under changing evidence rather than remaining rigid or becoming unstable.
4. Which analytical lenses add unique value and which are redundant.
5. Whether control-point ranking predicts useful interventions.
6. Whether EDF-0 is genuinely fast and usable for ordinary problems.
7. Whether topology and layered-origin patterns generalize beyond the cases that generated them.
8. How to visualize Origin Networks and Propagation without implying unsupported causal precision.
9. Whether EDF works in neglected domains, especially cybersecurity, medicine, human factors, accessibility, and economics.

## Ranked accepted opportunities

| Rank | Record | Opportunity | Frontier score | Why now |
|---:|---|---|---:|---|
| 1 | [RFR-001](records/RFR-001.md) | Run independent human reproducibility study | 84 | Converts the central reproducibility claim from simulated to empirical |
| 2 | [RFR-002](records/RFR-002.md) | Execute Chernobyl longitudinal reference case | 80 | Tests evidence sensitivity and unlocks stability metrics |
| 3 | [RFR-003](records/RFR-003.md) | Controlled comparative effectiveness trial | 74 | Establishes incremental value over natural analysis/RCA |
| 4 | [RFR-004](records/RFR-004.md) | Validate control-point prioritization | 69 | Tests EDF's promised link from diagnosis to leverage |
| 5 | [RFR-005](records/RFR-005.md) | Determine the minimum complementary lens set | 65 | Reduces validation cost and lens redundancy |
| 6 | [RFR-006](records/RFR-006.md) | EDF-0 usability and time-to-sufficiency study | 64 | Directly addresses speed and learning-curve gaps |
| 7 | [RFR-007](records/RFR-007.md) | Cross-domain reference suite | 61 | Tests generalization and neglected disciplines |
| 8 | [RFR-008](records/RFR-008.md) | Test diagnostic topology generalization | 54 | Prevents overfitting before vocabulary enters EDF |
| 9 | [RFR-009](records/RFR-009.md) | Evidence-preserving visual notation benchmark | 52 | Improves communication without premature formalism |
| 10 | [RFR-010](records/RFR-010.md) | Confidence calibration measurement study | 50 | Replaces qualitative confidence labels with measured calibration |

The score is a normalized comparative score:

`knowledge gain × impact × reuse × scientific importance − dependency cost − difficulty`, scaled to 0–100. Component scores and uncertainty are recorded in `frontier-index.json`. Rankings were stress-tested by reducing feasibility and increasing dependency penalties; RFR-001 and RFR-002 remain first-tier priorities.

## Dependency path

```text
RFR-001 Independent humans ─┬─> RFR-005 Lens minimization
                            ├─> RFR-010 Confidence calibration
                            └─> RFR-003 Comparative trial

RFR-002 Longitudinal case ──┬─> RFR-010 Confidence calibration
                            └─> RFR-008 Topology generalization

RFR-006 EDF-0 usability ───────> RFR-003 Comparative trial
RFR-009 Visual notation ───────> later reproducibility replication
RFR-007 Cross-domain suite ────> RFR-008 Topology generalization
```

The machine-readable graph is in [frontier-graph.json](frontier-graph.json).

## Contradictions and tensions

| Priority | Tension | Resolution needed |
|---:|---|---|
| Critical | Reproducibility is reported as High/Very High, while independent-human reproducibility is explicitly untested | Reserve “internal structural agreement” for current results; validate RC-7 with RFR-001 |
| High | EDF v0.3 is authoritative, while the v1.0 draft changes the core outputs and mode semantics | Create a traceable change map only after validation evidence supports each change |
| High | Evidence evolution is described as an EDF strength, but the longitudinal case is only planned | Treat this as a Framework Engineering hypothesis until RFR-002 completes |
| Medium | EDF should remain lean, while topology, layers, evolving state, visualization, and scoring are proposed | Apply Concept Burden and require multi-case evidence before adoption |
| Medium | Control points are ranked by influence, controllability, cost, and confidence, but comparisons acknowledge no mature evidence-backed prioritization method | Validate ranking reliability and predictive value through RFR-004 |

## Confidence decay

- Claims based only on repository-authored lenses decay one level until independent replication.
- Claims about evidence evolution remain “planned,” regardless of conceptual plausibility.
- Comparative claims based on a single Chernobyl evidence state remain provisional pending multi-case comparisons.
- v0.3 design claims remain current, but implementation guidance should be revalidated when v1.0 is proposed for release.

## Repository-wide frontier

Largest gap: independent external evidence.

Over-researched relative to evidence: conceptual terminology and candidate topology.

Neglected: real users, time-on-task, inter-rater measurement, outcome prediction, accessibility, cybersecurity, clinical use, and economic cost/benefit.

Most influential current result: structural/behavioral separation of Origin Network and Propagation.

Most uncertain prominent claim: EDF reproducibility.

## Executive recommendations

- If only one REP is funded: RFR-001. It tests the claim on which adoption, validation credibility, and later optimization depend.
- Greatest uncertainty reduction: RFR-001.
- Largest unlock: RFR-002, because it enables Diagnostic Stability, confidence-change, and evidence-isolation evaluation.
- Highest ROI: RFR-006; a small usability study can quickly improve adoption and expose unnecessary concept burden.
- Highest risk/highest reward: RFR-003; a controlled comparison could establish unique value or show that EDF adds little beyond structured prompting.
- Begin immediately: RFR-001 recruitment and preregistration, while completing RFR-002 evidence-package integrity review.

## Self-critique

This analysis is limited by repository metadata: most artifacts are working drafts without standardized identifiers, dates, explicit confidence, or acceptance decisions. Artifact counts therefore use documented inclusion rules in `repository-health.md`. Scores are decision aids, not measured scientific quantities. Five opportunities per source are mapped in `document-frontiers/`, but semantic duplicates resolve to the ten canonical records above rather than creating unsupported duplicate RFRs.

