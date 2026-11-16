# Literature Review G: Adaptive Diagnostics and Diagnostic Efficiency

Workstream: G. Prepared 2026-11-16 for the EDF framework-improvement review.
Source IDs refer to `sources-FGI.csv`. Verification labels: **fetched**, **search-result**, **from-memory-unverified** (none used here).

## Scope and the EDF question

EDF v0.3 runs the same nine-step sequence at every complexity level (EDF-0/1/2), and "The sequence remains the same across levels. The depth changes." It gives no rule for *which* evidence to seek next, or for when to stop seeking evidence beyond the verbal sufficiency question. This review asks: what is the evidence that adaptive, information-driven questioning reaches equal accuracy with less effort, and what stopping rules are defensible?

## 1. Sequential and adaptive diagnosis: theory

### Finding 1.1: Sequential testing reaches decisions with fewer observations on average
- Wald (1945) introduced the sequential probability ratio test (SPRT). It reaches a decision with, on average, substantially fewer observations than a fixed-sample test with the same error rates [SRC-FGI-026, search-result]. The saving depends on the true state. Near the indifference region, expected sample size can exceed that of a fixed test (course notes in the same search results).
- **Type:** theoretical/statistical, with extensive applied validation.
- **Implication:** Fixed checklists (every step at full depth) are inefficient by construction. Stopping rules should be set by error tolerance, not by completing a template.

### Finding 1.2: Sequential Bayesian diagnosis with test costs dates back to 1968
- Gorry and Barnett (1968) built a computer model of sequential diagnosis (congenital heart disease) that used Bayes' theorem to choose the next test, with test cost treated explicitly [SRC-FGI-027, search-result].
- **Type:** early empirical/engineering.

### Finding 1.3: Bayesian experimental design formalises "ask the most informative question next"
- Chaloner and Verdinelli (1995) reviewed Bayesian experimental design in a decision-theoretic frame, in which the design criterion follows from the utility of the decision [SRC-FGI-021, search-result].
- Rainforth et al. (Modern Bayesian Experimental Design, *Statistical Science*, 2024) defined expected information gain (EIG) as the mutual information between the outcome and the parameters. They described *adaptive* design, which updates the design after each observation [SRC-FGI-022, search-result].
- Settles (2009) surveyed active learning: choosing which queries to label to reach accuracy with fewer labels (uncertainty sampling, query-by-committee, expected model change, expected error reduction) [SRC-FGI-023, search-result].
- **Type:** theoretical/methodological.
- **Key distinction for EDF:** EIG (reduce uncertainty about the *model*) is not the same as value of information (reduce the expected loss of the *decision*; Howard 1966 [SRC-FGI-020]). EDF's sufficiency rule is decision-centred, so its question-selection rule should also be decision-centred: seek evidence that discriminates between hypotheses which imply *different control points*. Evidence that only refines the origin network without changing the action has low value.

### Finding 1.4: Threshold model (cross-ref Workstream F)
- Pauker and Kassirer (1980): test only when the probability lies between the testing threshold and the test-treatment threshold [SRC-FGI-019, search-result]. This is the clinical form of a decision-centred stopping rule.

## 2. Empirical evidence that adaptive questioning reaches equivalent accuracy with fewer items

### Finding 2.1: Computerized adaptive testing (CAT) for depression: 12 items replace 389 at r = 0.95
- Gibbons et al. (2012, *Archives of General Psychiatry*), N = 1,614. CAT drew from a 389-item bank and stopped at SE = 0.3. It used a mean of about 12 items (12.31) and correlated r = 0.95 with the full 389-item score. Median administration time was about 2.3 minutes versus about 52 minutes for the full bank. Loosening the stop rule to SE = 0.4 cut the mean to about 6 items, with r = 0.92 [SRC-FGI-025, fetched].
- **Type:** empirical (large calibration and simulation study).

### Finding 2.2: CAT advantage over well-designed static short forms is real but small
- Choi et al. (2010, *Quality of Life Research*), PROMIS 28-item depression bank, n = 768, post hoc simulation. Two CAT procedures reached correlations of about 0.96 with the full bank using 5 items. The best static forms needed 6 items, and expected-information static forms needed 8. A two-stage branching design came close to CAT's efficiency. The authors concluded that CAT was "marginally more efficient" than static short forms [SRC-FGI-024, fetched].
- **Type:** empirical (simulation on real responses).
- **Implication for EDF:** Most of the efficiency gain comes from (a) a good short form and (b) one branching decision. Fully adaptive selection adds a little more. A **two-stage design** (short triage, then branch to a targeted module) is the pragmatic target for EDF: EDF-0 acts as the triage stage and decides whether EDF-1 or EDF-2 modules are needed and which ones.

### Finding 2.3: Adaptive survey design is an established field
- Schouten, Peytchev and Wagner (2017, *Adaptive Survey Design*) cover tailoring fieldwork strategies to subgroups, including stratification, interventions, nonresponse modelling, cost and optimisation [SRC-FGI-028, search-result].
- **Type:** methodological.

### Finding 2.4: Selective (risk-based) verification often matches full verification
- Trade-press reporting of a 2014 *Therapeutic Innovation & Regulatory Science* analysis said only about 1.1% of site-entered trial data were corrected by 100% source-data verification (SDV). It also reported FDA's 2013 position that risk-based approaches focused on critical data are more likely to ensure quality than 100% verification [SRC-FGI-029, search-result; secondary news report, quote not verified against the FDA guidance].
- Counter-evidence: a 2019 prospective comparison found targeted SDV left residual errors (1.47% overall, 0.78% on key data) and generated about four times more queries, possibly raising cost [SRC-FGI-030, search-result].
- **Type:** empirical, mixed.
- **Implication:** Risk-based depth works when "critical data" are defined in advance. EDF's step 6 (Evaluate Evidence) should verify claims in proportion to how much the ranking depends on them. Claims that drive the top-ranked control point get independent corroboration; peripheral claims do not.

## 3. LLM-era sequential diagnosis (cross-ref Workstream I)

### Finding 3.1: Cost-aware orchestration improved accuracy per dollar on a sequential benchmark
- Nori et al. (2025, arXiv 2506.22405) built SDBench from 304 NEJM clinicopathological cases. Information is revealed only on request by a "gatekeeper" model, and agents are scored on accuracy and cumulative test cost [SRC-FGI-031, fetched]. Results:
  - Off-the-shelf o3: 78.6% at about $7,850 per case.
  - MAI-DxO (o3): 81.9% at about $4,735.
  - A lower-cost configuration: 79.9% at about $2,397.
  - Ensemble: 85.5% at about $7,184.
  - Generalist physicians: 19.9% at about $2,963. Physicians were barred from references, colleagues and search, which the authors acknowledge.
- MAI-DxO uses five role prompts: Hypothesis (ranked top-3 differential, Bayesian-updated), Test-Chooser (tests that best discriminate between the leading hypotheses), Challenger (devil's advocate against anchoring), Stewardship (vetoes low-yield costly tests) and Checklist (consistency). It commits to a diagnosis when certainty exceeds a threshold. The threshold value is not published.
- Limitations the authors state: curated hard cases with no benign or healthy cases (so false positives were not measured); cost only, with no invasiveness or delay; possible contamination.
- **Type:** empirical, single-team preprint. Results are not independently replicated in the material found.
- **Implication:** The design maps closely onto EDF. Hypothesis → origin network. Test-Chooser → discriminating evidence. Challenger → Diagnostic Calibration. Stewardship → cost of diagnosis. **Stewardship has no counterpart in EDF today.** The benchmark design (gatekeeper reveals information only when asked; score accuracy *and* information cost) is directly reusable for an EDF benchmark.

### Finding 3.2: Information-gain planning helps LLMs ask better questions
- Hu et al. (2024, "Uncertainty of Thoughts", NeurIPS) used simulated futures and information-gain rewards to choose questions. They report an average 38.1% improvement in task success over direct prompting across medical diagnosis, troubleshooting and 20 Questions, with fewer questions [SRC-FGI-032, search-result].
- MediQ (Li et al., 2024, NeurIPS): simply prompting LLMs to ask questions *reduced* accuracy (about 11% versus not asking, from the same limited information). Confidence estimation and abstention improved results [SRC-FGI-033, search-result].
- **Implication:** "Ask more questions" is not automatically better. Gains come from *principled selection plus a calibrated stopping rule*.

## 4. Consolidated proposed changes to EDF (Workstream G)

| # | Change | Step affected | Evidence basis |
|---|--------|---------------|----------------|
| G1 | Turn EDF-0 into an explicit **triage stage** with a fixed short item set. Its output picks which EDF-1/EDF-2 modules to run (two-stage branching), not just a depth label. | Complexity levels | SRC-FGI-024, -025, -028 |
| G2 | Add a **discriminating-evidence rule** to step 6: before gathering evidence, name the competing hypotheses and pick evidence that would shift the control-point ranking. Record the "expected decision impact" of each evidence request. | 4–6 | SRC-FGI-021, -022, -020, -031 |
| G3 | Add a **Stewardship check**: each evidence request records its cost (time, disruption, trust). Low-yield, high-cost requests are declined with a reason. | 6, 9 | SRC-FGI-031, -027 |
| G4 | **Risk-based verification:** claims that support the top-ranked control point need independent corroboration (two sources or direct observation). Peripheral claims can rest on a single report, marked as such. | 6 | SRC-FGI-029, -030 |
| G5 | Make the **stopping rule explicit**: stop when (a) the top control point is stable under the remaining plausible hypotheses, or (b) the evidence budget is spent, or (c) a reversible probe is cheaper than further diagnosis. Record which applied. | 9 | SRC-FGI-026, -019, -013 |
| G6 | Treat "more questions" as a cost, not a virtue, in any AI-assisted EDF. Add confidence/abstention gating before asking. | AI use | SRC-FGI-033, -032 |

## 5. Testable benchmark hypotheses

Benchmark template (adapted from SDBench): each case is a hidden case file. A gatekeeper reveals evidence only when asked. Every request has a cost. Ground truth is the expert-agreed outcome, origin network and best control point (or "no action").

- **H-G1 (non-inferior with fewer requests):** EDF with G2 + G5 will reach top-control-point agreement non-inferior to full-depth EDF v0.3 (margin pre-registered, for example 5 percentage points) with at least 30% fewer evidence requests. The 30% figure is a hypothesis, motivated by CAT results [SRC-FGI-024, -025], and is not a transferred estimate.
- **H-G2 (two-stage triage):** A two-stage EDF (fixed triage, then branch) will match a fully adaptive selection policy within a small margin. This replicates the Choi et al. pattern [SRC-FGI-024] in the organizational domain.
- **H-G3 (stewardship):** Adding the G3 Stewardship check lowers mean evidence cost per case without a drop in accuracy beyond the margin. Without it, AI-run EDF will over-request.
- **H-G4 (risk-based verification):** Applying G4 cuts verification effort with no increase in the rate at which the top-ranked control point rests on a false claim, measured using planted false claims in benchmark cases.
- **H-G5 (stopping reason calibration):** Cases stopped under rule (a) (ranking stable) will show higher agreement with ground truth than cases stopped under (b) (budget spent). This would validate the stopping reason as a confidence signal.

## 6. Limitations

- The strongest efficiency evidence (CAT) comes from unidimensional psychometric constructs with calibrated item banks. Organizational diagnosis is multi-causal and lacks calibrated "items", so the size of the gain will not transfer. Only the direction is supported.
- SDBench and MAI-DxO are a single-team preprint on curated hard cases.
- The risk-based verification evidence is mixed and comes from clinical-trial data management.
