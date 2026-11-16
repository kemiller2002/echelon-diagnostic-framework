# Adaptive Diagnostics and Diagnostic Efficiency

Mission EDF-IMPROVEMENT-2026-001 · Baseline EDF v0.3 (`41fea6f`) · 2026-11-16
Primary input: `../scientific-foundations/literature/lit-adaptive.md`. Also draws on `lit-intervention.md` §4, `lit-ai-diagnosis.md` §6 and `../baseline/*.md`. Source ids refer to `../evidence/source-register.csv`. "[SR]" marks a claim whose key detail rests only on search-result sources.

This file covers the literature only. The section "Experimental results" at the end is reserved for the orchestrator. See experiments/ and strategy/ for measured results (EX-EDF-2026-OD1).

## 1. The efficiency question for EDF

EDF v0.3 runs the same nine steps at every complexity level: "The sequence remains the same across levels. The depth changes." (`../baseline/methodology-inventory.md` §1, §14). This leaves three gaps.

- **No question-selection rule.** EDF says nothing about which evidence to seek next.
- **No operational stopping rule.** The verbal sufficiency question is the only stopping guidance (`methodology-inventory.md` §13).
- **No structured evidence request.** The baseline audit ranks this an S2 limitation (`../baseline/current-limitations.md` rank 7).

The repository has a hypothesis that EDF-0 is fast and learnable enough for routine use (HY-EDF-010). It is marked unsupported.

The questions for this review:

1. Can adaptive, information-driven questioning reach equal diagnostic quality with less effort?
2. What stopping rules are defensible?

## 2. Theory: sequential diagnosis and stopping

| Result | Type | Sources | Relevance |
|---|---|---|---|
| The sequential probability ratio test reaches a decision with fewer observations *on average* than a fixed-sample test at equal error rates. Near the indifference region, expected sample size can be larger. | Statistical theory (formal) | SRC-FGI-026 [SR] | Stop when the error tolerance is met, not when a template is complete. Hard, ambiguous cases may need more evidence than a fixed protocol provides. |
| A 1968 Bayesian sequential-diagnosis model chose the next test by expected information, with test cost included. | Early engineering / empirical | SRC-FGI-027 [SR] | The idea of costed, sequential evidence selection is nearly 60 years old in medicine. |
| Bayesian experimental design derives the design criterion from the utility of the decision. Expected information gain (EIG) is the mutual information between outcome and parameters. Adaptive design updates after each observation. | Methodological | SRC-FGI-021, SRC-FGI-022 [SR] | This supplies a formal rule for choosing the next evidence request. |
| Active learning selects queries by uncertainty sampling, query-by-committee, expected model change or expected error reduction. | Survey | SRC-FGI-023 [SR] | "Query-by-committee" corresponds to asking where independent EDF analyses disagree. |
| Value of information is the expected improvement in the *decision*. | Decision theory | SRC-FGI-020 [SR] | See the next paragraph. |
| Test/treatment thresholds: test only when the probability lies between the testing and test-treatment thresholds. Below that band, do neither. Above it, act. | Decision theory | SRC-FGI-019 [SR] | Gives Diagnostic Sufficiency a formal meaning. |
| Irreversibility gives waiting an option value. | Economic theory | SRC-FGI-013 [SR] | The sufficiency bar should rise with irreversibility. |

**EIG is not VOI.** EIG reduces uncertainty about the *model*. VOI reduces the expected loss of the *decision*. EDF's sufficiency rule is decision-centred, so its question-selection rule should be decision-centred too. It should seek evidence that discriminates between hypotheses implying *different control points*. Evidence that refines the origin network without changing the action has low value, however much it reduces uncertainty.

## 3. Empirical evidence: equal accuracy with fewer items

| Finding | Type / strength | Sources |
|---|---|---|
| A depression computerized adaptive test (CAT) used about 12 of 389 items and correlated r = 0.95 with the full bank. Median time was about 2.3 minutes versus about 52. Loosening the stopping rule to standard error 0.4 cut it to about 6 items, with r = 0.92. | Large calibration study, N = 1,614. Strong within psychometrics (fetched). | SRC-FGI-025 |
| CAT was only "marginally more efficient" than the best static short forms: 5 items versus 6–8 items for r ≈ 0.96. A two-stage branching design came close to CAT. | Simulation on real responses, n = 768. Moderate (fetched). | SRC-FGI-024 |
| Adaptive survey design (tailoring fieldwork to subgroups, cost optimisation) is an established methodological field. | Methodological | SRC-FGI-028 [SR] |
| Risk-based source-data verification: reportedly only about 1.1% of trial data were corrected by 100% verification, and the regulator favoured risk-based approaches. | Trade-press secondary report. Weak. | SRC-FGI-029 [SR; quote not verified] |
| Counter-evidence: targeted verification left residual errors and generated about four times more queries. | Prospective comparison. Moderate. | SRC-FGI-030 [SR; authors not retrieved] |

**What transfers.** The *direction* transfers. A good short form plus one branching decision captures most of the efficiency gain, and full adaptivity adds a little more. The *magnitude* does not transfer. CAT works on unidimensional constructs with calibrated item banks. Organizational diagnosis is multi-causal and has no calibrated "items".

The risk-based verification evidence is mixed. Selective verification works only when the "critical data" are defined in advance. For EDF, these are the claims that the top-ranked control point depends on.

## 4. LLM-era sequential diagnosis (MediQ, SDBench and related work)

| Finding | Type / strength | Sources |
|---|---|---|
| **SDBench / MAI-DxO.** 304 NEJM cases. A gatekeeper reveals information only when asked, and agents are scored on accuracy and cumulative test cost. A role-structured orchestrator (Hypothesis, Test-Chooser, Challenger, Stewardship, Checklist) improved accuracy at lower cost than an off-the-shelf model, and the result held across model families. The commit threshold is not published. Limitations: hard cases only, no benign cases (so false positives were not measured), cost without invasiveness or delay, possible contamination. | Single-team preprint. Weak to moderate (fetched). | SRC-FGI-031 |
| **MediQ.** Simply prompting LLMs to ask questions *reduced* accuracy, by about 11% versus not asking. Confidence estimation and abstention improved results. | Conference benchmark | SRC-FGI-033 [SR] |
| **Uncertainty of Thoughts.** Information-gain question selection with simulated futures improved success by an average of 38.1% over direct prompting, with fewer questions. | Conference benchmark | SRC-FGI-032 [SR] |
| Multi-turn, underspecified settings cut LLM performance by about 39% on average. The drop came mainly from unreliability: early assumptions and premature final answers. | Large simulation study (fetched) | SRC-FGI-047 |
| Conversational history-taking was worse than vignettes. Summarised conversations helped. | Benchmark, press-release level | SRC-FGI-044 [SR] |
| LLMs were sensitive to the order and quantity of information on real clinical cases. | Empirical | SRC-FGI-045 [SR] |

**Synthesis.** "Ask more" is not better. Gains come from three things together: principled selection of discriminating evidence, a calibrated stopping or commit rule, and a cost-stewardship function. The SDBench structure maps closely onto EDF:

- Hypothesis role → Origin Network
- Test-Chooser → discriminating evidence
- Challenger → Diagnostic Calibration
- Stewardship → no current EDF counterpart

OD1 already adopts the gatekeeper idea in a reduced form. Evidence requests are titles with hidden contents, and the analyzer is scored on choosing discriminating requests over low-value ones (`../benchmarks/benchmark-specification.md` §2, §5).

## 5. Human analogues: confidence and information-seeking

Physicians' requests for more diagnostic tests fell as their confidence rose, even on hard cases where accuracy was poor (SRC-CDE-014, [SR]). Self-rated confidence is therefore a poor stopping signal. The stopping rule should depend on whether the action is stable across the live alternatives, not on felt certainty (OPP-11).

## 6. Candidate mechanisms (literature-derived; see `../scientific-foundations/methodological-opportunities.md`)

| ID | Mechanism | Basis |
|---|---|---|
| OPP-13 (G1) | EDF-0 as a **fixed triage short form** whose output selects EDF-1/2 modules (two-stage branching) | SRC-FGI-024, SRC-FGI-025 |
| OPP-12 (G2, G3) | **Discriminating-evidence rule** with the expected decision impact of each request, plus a **Stewardship** check that records cost (time, disruption, trust) and declines low-yield requests | SRC-FGI-020, SRC-FGI-021, SRC-FGI-031, SRC-FGI-027 |
| OPP-01 / G4 | **Risk-based verification**: claims that support the top control point need independent corroboration; peripheral claims may rest on a single source, marked as such | SRC-FGI-029, SRC-FGI-030 (mixed) |
| OPP-11 (G5) | **Explicit stopping rule**, with the reason recorded: (a) the top control point is stable under the remaining plausible hypotheses; (b) the evidence budget is spent; (c) a reversible probe is cheaper than more diagnosis | SRC-FGI-026, SRC-FGI-019, SRC-FGI-013 |
| OPP-17 (G6, I5, I6) | For AI use: **confidence or abstention gating before asking**; functional roles; a minimum evidence set before committing; consolidated restatement of the evidence | SRC-FGI-033, SRC-FGI-031, SRC-FGI-047 |

## 7. Testable hypotheses (from lit-adaptive §5)

- **H-G1.** Decision-centred selection plus an explicit stopping rule is non-inferior to full-depth EDF v0.3 on top-control-point agreement, with at least 30% fewer evidence requests. The 30% figure is a hypothesis motivated by the CAT results, not an estimate transferred from them.
- **H-G2.** A two-stage EDF comes within a small margin of a fully adaptive policy.
- **H-G3.** A Stewardship check lowers evidence cost without an accuracy loss beyond the margin.
- **H-G4.** Risk-based verification does not increase the rate at which the top control point rests on a planted false claim.
- **H-G5.** Cases stopped because the ranking was stable agree with ground truth more often than cases stopped because the budget ran out.

## 8. Limits

- The strongest efficiency evidence (CAT) is from unidimensional psychometrics. Only the direction of the effect transfers.
- SDBench is a single-team preprint on curated hard cases. MediQ, Uncertainty of Thoughts and CRAFT-MD details were seen only at search-result or press level.
- No study was found of adaptive or sequential diagnosis *of organizations*.
- In real organizations, evidence "cost" includes trust and disruption, which no benchmark prices (`../intervention-effectiveness/outcome-measurement.md` §5).

## Experimental results

EX-EDF-2026-OD1, adaptive-evidence experiment (EXP-OD1-07). 12 held-out cases, Sonnet analyzers. Raw data: `experiments/raw/adapt-selection/`, `experiments/raw/adapt2/`; scores: `experiments/scored/adapt2/`.

**Design.** Stage 1: the analyzer sees the presenting request, the candidate items and a catalogue of evidence (ID, stage, source, source type; content hidden) and chooses k = 6 of about 17 items. Stage 2: a fresh analyzer diagnoses using only the chosen items, with the D_EDFX module. Comparators are the first 6 items in file order ("fixed sequence"), a seeded random 6, and full evidence (D_EDFX held-out runs).

| Strategy | Score | Truth-required evidence covered | Δ vs full evidence (96.0) |
|---|---|---|---|
| Model-selected, D guidance | 84.3 | 48% | −11.6 [−14.4, −8.9] |
| Model-selected, general prompt | 84.5 | 48% | −11.5 [−14.8, −8.3] |
| Fixed sequence | 69.9 | 35% | −26.1 [−31.2, −21.1] |
| Random | 72.7 | 35% | −23.3 [−29.9, −17.0] |

Model-selected vs random: +11.7 [5.9, 17.9]. Model-selected vs fixed: +14.4 [10.1, 18.8]. D-guided vs general selector: −0.2 [−3.8, 3.4].

**Interpretation**

- Adaptive, value-of-information-style selection is the largest efficiency lever measured: about 35% of the evidence retains about 88% of full-evidence performance.
- Fixed-sequence collection is the worst strategy.
- EDF-specific guidance does not improve selection.
- Selection from a content-blind catalogue misses about half of the decisive evidence. That argues for *sequential* acquisition (read, update, choose again) over a single up-front batch. Sequential acquisition is untested here and is the next experiment.
- Optimal stopping was not tested; a fixed budget k = 6 was imposed.
