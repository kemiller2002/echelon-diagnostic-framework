# Repository Health Assessment

Analysis date: 2026-07-28

## Scope and counting rules

The corpus contains 53 Markdown documents under `docs/`. Of these, 42 are treated as substantive research or governance artifacts; 11 navigation pages, templates, or short public examples are supporting artifacts. No artifact declares `status: accepted`; most say `Working draft`. “Validated finding” below therefore means supported inside the repository, not externally validated.

## Metrics

| Metric | Result | Interpretation |
|---|---:|---|
| Markdown artifacts reviewed | 53 | Complete `docs/` corpus |
| Substantive research/governance artifacts | 42 | Excludes navigation-only and user templates |
| Repository-supported claims | 10 | Supported or provisionally supported in the evidence ledger |
| Externally validated findings | 0 | No completed independent-human study |
| Open frontier records | 10 | All records are Open |
| Average confidence | 0.61 | Qualitative labels normalized; lowered for non-independent evidence |
| Validation cases with R1 analyses | 6 | Challenger, Apollo 13, Boeing, Apollo 11, Pixar, Toyota |
| Independent validation studies completed | 0 | IFVS-001 is a design only |
| Longitudinal studies completed | 0 | LRC-001 is planned |
| Comparative method studies | 3 | RCA/Five Whys, FMEA, FTA; single principal use case |
| Ablation studies | 1 completed artifact | Propagation refinement |
| Critical/high contradictions | 3 | Reproducibility, version semantics, evidence evolution |
| Semantic duplicate opportunity rate | 58% | 24 raw themes consolidated into 10 RFRs |
| Average research depth | 2.6 / 5 | Strong concepts/cases; weak controlled/external evidence |
| Knowledge graph connectivity | 0.42 | 19 dependency/evidence edges across 20 principal nodes |
| Repository maturity | Emerging / internally coherent | Not ready for empirical performance claims |

## Research by discipline

| Discipline | Coverage | Gap |
|---|---|---|
| Systems engineering | High | External replication |
| Operations/governance | High | Field outcomes and economic measures |
| Evidence/causal reasoning | Medium-high | Blinding and calibration metrics |
| Reliability engineering | Medium | Formal FTA/FMEA integration trials |
| Human factors | Low-medium | Dedicated stress/interface cases |
| Medicine | Low | No clinical case validation |
| Cybersecurity | Low | No completed security case |
| Statistics/measurement | Low | No power analysis, reliability coefficients, or uncertainty intervals |
| Accessibility | Very low | No study |
| Economics | Very low | No cost-effectiveness analysis |

## Coverage

- Internal case coverage: moderate.
- Failure/success diversity: good.
- Independent-participant coverage: absent.
- Longitudinal evidence coverage: absent.
- Simple-case/EDF-0 coverage: minimal.
- Experimental coverage: low.
- Quantitative measurement coverage: low.

## Largest evidence gaps

1. No real independent analysts.
2. No randomized or counterbalanced method comparison.
3. No completed evidence-evolution experiment.
4. No predictive validation of control points.
5. No validated scoring rubric for agreement, confidence, or diagnostic sufficiency.

## Health recommendation

Freeze new core concepts. Invest the next research cycle in independent analysts, longitudinal execution, quantitative measurement, and simple-case usability. Update this health report when RFR-001 or RFR-002 changes status.

