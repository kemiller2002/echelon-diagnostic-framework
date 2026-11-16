# Literature Review F: Intervention Effectiveness and the Diagnosis-to-Action Handoff

Workstream: F (with cross-links to G). Prepared 2026-11-16 for the EDF framework-improvement review.
Source IDs refer to `sources-FGI.csv`. Verification labels: **fetched** (primary or repository page read), **search-result** (bibliographic and abstract-level facts confirmed in search results, full text not read), **from-memory-unverified** (none used as load-bearing evidence in this file).

## Scope and the EDF question

EDF v0.3 ends with steps 7 to 9: identify control points, rank them on Influence, Controllability, Cost and Confidence, then decide Diagnostic Sufficiency ("Do we understand enough to take the next responsible action?"). The operating cycle hands ranked control points to Clarity and returns to EDF after execution ("Did the intervention change the manifestation?").

This review asks: what does the evidence say about (a) how often organizational interventions work, (b) how to specify and evaluate them, and (c) when the responsible action is to diagnose without recommending one?

## 1. Base rates of organizational change success

### Finding 1.1: The "70% of change fails" figure has no valid empirical basis
- Hughes (2011) reviewed five published sources of the 70% failure figure (including Hammer and Champy, Beer and Nohria, Kotter, and McKinsey and Bain articles). He concluded that the narrative exists but "there is no valid and reliable empirical evidence to support such a narrative" [SRC-FGI-001, fetched].
- **Type:** critical review of evidence (empirical in method, negative in result).
- **Implication:** EDF documents and any Echelon training material must not use a generic failure base rate to justify diagnostic depth. Where EDF uses base rates, it needs a reference class specific to the intervention type.

### Finding 1.2: Meta-analyses of OD interventions show modest average effects and wide variation
- Robertson, Roberts and Porras (1993) meta-analysed 52 evaluations of planned change. They tested a model in which interventions change work-setting variables (organizing arrangements, social factors, technology, physical setting), which change individual behaviour, which changes organizational outcomes. The aggregated results were "largely consistent" with the model [SRC-FGI-002, search-result]. A later longitudinal test (N = 690, one firm) did not support the hypothesised relationships (reported in the same search results; not separately sourced).
- Neuman, Edwards and Raju (1989) meta-analysed OD interventions on satisfaction and attitudes [SRC-FGI-003, search-result]. As summarised by Huebner and Zacher (2021) [SRC-FGI-004, fetched], human-process interventions changed attitudes more than techno-structural ones, and survey feedback made up only six of 84 human-process studies.
- Guzzo, Jette and Katzell (1985) meta-analysed 11 kinds of psychologically based interventions and found an average productivity gain of nearly half a standard deviation [SRC-FGI-018, search-result].
- **Type:** empirical (meta-analytic). These studies are old (1985 to 1993), mostly published evaluations, and exposed to publication bias.
- **Implication:** The prior for "a well-chosen OD intervention moves an attitude or behaviour metric" is moderately positive. The prior for "it moves the organizational outcome EDF defined in step 2" is weaker, because the causal chain is longer. EDF's control-point ranking should state which link in the chain each control point acts on.

### Finding 1.3: Interventions often do harm, not just nothing
- Kluger and DeNisi (1996) meta-analysed 607 effect sizes from feedback interventions. The average effect was positive (d ≈ 0.41), but more than a third of the interventions *reduced* performance [SRC-FGI-011, search-result].
- **Type:** empirical (meta-analytic).
- **Implication:** EDF's ranking criteria have no downside or harm dimension. "Influence" is undirected. A control point can be high-influence in the wrong direction. **EDF needs an explicit "Risk of harm / backfire" field on each ranked control point.**

### Finding 1.4: Survey-feedback interventions work only with follow-through
- Huebner and Zacher (2021) systematically reviewed survey follow-up. Evidence was "mixed". Positive effects depended on actual action planning, actionable items, trained and supported managers, stakeholder involvement and quick sharing of results. They also noted inconsistent implementation, weak reporting and likely publication bias [SRC-FGI-004, fetched].
- **Type:** empirical (systematic review).
- **Implication:** Diagnosis itself is an intervention. Collecting data and then not acting is likely to cost trust. EDF should treat "we asked people" as a commitment that the Next Responsible Action must address (see also lit-longitudinal.md).

## 2. Specifying interventions: theory of change and implementation science

### Finding 2.1: Complex-intervention guidance requires an explicit programme theory and attention to context
- The updated MRC/NIHR framework (Skivington et al., 2021, BMJ) has six core elements, including what the programme theory is and how the intervention interacts with its context [SRC-FGI-009, search-result].
- De Silva et al. (2014) showed how Theory of Change (ToC) can be built into the MRC framework. Their argument is that ToC links process measures and effectiveness measures in one causal map. They tested it in two RCTs and one non-randomised evaluation [SRC-FGI-010, search-result].
- **Type:** methodological guidance (expert consensus), plus case applications.
- **Implication:** EDF's propagation path (step 5) is a backward-looking causal map. A ToC is the forward-looking map for an intervention. **Proposed change:** each top-ranked control point should carry a short forward chain (control point → expected change in a propagation path → expected change in the manifestation → expected change in the outcome), with the assumptions at each link named. This is what Clarity's "assumptions" and "consequence" fields need to consume.

### Finding 2.2: Implementation outcomes are distinct from effectiveness outcomes
- Proctor et al. (2011) separated implementation outcomes (acceptability, adoption, appropriateness, feasibility, fidelity, implementation cost, reach, sustainability) from service and clinical outcomes [SRC-FGI-005, search-result]. A ten-year follow-up reported that measurement improved but conceptual precision still lags (from the same search results).
- RE-AIM (Glasgow, Vogt and Boles, 1999) evaluates Reach, Effectiveness, Adoption, Implementation and Maintenance [SRC-FGI-007, search-result]. The 20-year review (Glasgow et al., 2019) reports more than 450 applications and a move toward pragmatic use by non-researchers [SRC-FGI-008, search-result].
- CFIR, updated in 2022 (Damschroder et al.), is a determinant framework for contextual barriers and facilitators, revised using 59 user-feedback articles and an author survey [SRC-FGI-006, search-result].
- **Type:** conceptual frameworks with wide empirical use. Their predictive validity for organizational (non-health) settings is not established in what was found here.
- **Implication:** "Controllability" in EDF conflates two things: whether anyone has authority to act, and whether the action can be implemented with fidelity. **Proposed change:** split Controllability into (a) authority/ownership and (b) implementability, where implementability is judged against a few CFIR-style determinants (for example: recipient readiness, inner-setting capacity, fit with existing work). The "Understand again" step should check implementation outcomes (Was it adopted? Done with fidelity? Sustained?) *before* it judges effectiveness. Otherwise a fidelity failure gets misread as a wrong diagnosis.

## 3. Evaluating whether the intervention worked

### Finding 3.1: Most organizational evaluation designs are quasi-experimental and have known threats
- Shadish, Cook and Campbell (2002) is the standard reference on quasi-experimental designs (interrupted time series, non-equivalent comparison groups, regression discontinuity) and on validity threats such as history, maturation, regression to the mean, instrumentation and selection [SRC-FGI-015, search-result].
- **Type:** methodological.
- **Implication:** EDF's validation loop asks "Did the intervention change the manifestation?" but gives no guidance on the counterfactual. Regression to the mean matters most here: organizations usually diagnose *after* a bad outcome, so improvement is expected even if nothing is done. **Proposed change:** the Verification field in the execution layer should state the comparison (baseline trend, comparison unit, or pre-registered threshold) and name the most plausible alternative explanation for improvement.

### Finding 3.2: Evidence-based management argues for using the best available evidence explicitly, not single authorities
- Rousseau (2006) proposed evidence-based management on the model of evidence-based medicine and criticised reliance on claims from famous executives or consultancies [SRC-FGI-016, search-result]. The 70% myth (Finding 1.1) is a clear case of what she criticises.
- **Type:** conceptual/position.

## 4. Decision value of further diagnosis: VOI, thresholds, real options, reversibility

### Finding 4.1: The test/treat threshold model gives "Diagnostic Sufficiency" a formal meaning
- Pauker and Kassirer (1980) derived two thresholds, a *testing* threshold and a *test-treatment* threshold. Below the first, do not test and do not treat. Above the second, treat without testing. Between them, test. The thresholds depend on the risks and benefits of treatment and the accuracy and risk of the test [SRC-FGI-019, search-result].
- **Type:** theoretical (decision analysis), widely applied.
- **Implication:** EDF's sufficiency rule ("additional investigation is unlikely to materially change the next responsible action") is a verbal version of this. It omits two inputs: the *cost and harm of further diagnosis* (interviews, surveys and audits disrupt work and can damage trust) and the *cost of delay*. **Proposed change:** make step 9 explicit with three questions: (1) Which action would we take now? (2) What evidence result would change it, and how likely is that result? (3) Is the expected improvement worth the cost of getting it, including delay and disruption?

### Finding 4.2: Value-of-information theory supplies the stopping logic
- Howard (1966) introduced information value theory: information is worth the expected improvement in decision value it produces, with perfect information as an upper bound [SRC-FGI-020, search-result].
- Claxton (1999) argued that conventional statistical inference (significance) is largely irrelevant to adoption decisions. Decide on expected value, and value further research separately using EVPI [SRC-FGI-014, search-result].
- **Type:** theoretical, with heavy applied use in health technology assessment.
- **Implication:** EDF's "Confidence" field should not be a gate on its own. A low-confidence diagnosis can still be sufficient if every plausible model points to the same next action, or if that action is cheap and reversible. A high-confidence diagnosis can be insufficient if the action is irreversible and one remaining uncertainty would flip it.

### Finding 4.3: Irreversibility creates option value of waiting; reversibility lowers the sufficiency bar
- Dixit and Pindyck (1994) showed that when investment is irreversible and uncertainty can be resolved over time, waiting has option value. The standard net-present-value rule then invests too early [SRC-FGI-013, search-result].
- Bezos's 2015 Amazon shareholder letter distinguished "one-way door" (Type 1, irreversible) from "two-way door" (Type 2, reversible) decisions. It warned that large organizations tend to apply Type 1 process to Type 2 decisions [SRC-FGI-012, search-result; only secondary copies were found, so the exact quotes are not verified against the primary letter].
- **Type:** theoretical (economics), plus practitioner heuristic.
- **Implication:** Reversibility is already a Clarity field, but EDF sets diagnostic depth *before* Clarity sees it. **Proposed change:** let the reversibility of the candidate next action set the sufficiency bar in step 9. A cheap, reversible probe can be the next responsible action even at low confidence. An irreversible restructuring needs a higher bar. Add "Probe / reversible test" as an explicit type of Next Responsible Action.

## 5. Diagnostic restraint: diagnosing without recommending

### Finding 5.1: Decision-makers show action bias
- Bar-Eli et al. (2007) analysed 286 penalty kicks. Goalkeepers almost always dive, although staying in the centre would be optimal given kick distributions. Their explanation (norm theory) is that conceded goals feel worse after inaction than after action [SRC-FGI-017, search-result].
- **Type:** empirical (field data plus survey). The domain is far from organizations, so treat this as a mechanism, not an effect size.
- **Implication:** Consulting and diagnostic settings carry strong pressure to recommend something. Kluger and DeNisi's backfire rate [SRC-FGI-011] shows that "do something" is not free.

### Finding 5.2: The threshold model formally allows "no test, no treat"
- In Pauker and Kassirer's model, when the probability of the condition is below the testing threshold, the right action is neither to test further nor to treat [SRC-FGI-019].
- **Implication:** EDF's required outputs include a "Next Responsible Action" but no legitimate "no intervention" outcome. **Proposed change:** list explicit permitted outcomes for step 9: (a) intervene at control point X; (b) run a reversible probe; (c) gather specific further evidence E (with an expected decision impact); (d) monitor with a defined trigger (watchful waiting); (e) no action warranted, with the reason. Forms (d) and (e) must name a re-review trigger.

### Gap
No primary study was found that measures how often organizational diagnoses *should* have ended in "no action". This is a research gap, not evidence of absence.

## 6. Consolidated proposed changes to EDF (Workstream F)

| # | Change | Step affected | Evidence basis |
|---|--------|---------------|----------------|
| F1 | Add a **Harm/backfire risk** criterion to the control-point ranking (Influence, Controllability, Cost, Confidence, **Harm**). | 8 | SRC-FGI-011, -004 |
| F2 | Split Controllability into **Authority** and **Implementability** (short CFIR-style checklist). | 7–8 | SRC-FGI-006, -005 |
| F3 | Require a **forward causal chain (mini Theory of Change)** with named assumptions for each top-ranked control point, mapped onto Clarity's assumptions and consequence fields. | 7–8, handoff | SRC-FGI-009, -010, -002 |
| F4 | Restate Diagnostic Sufficiency as a **threshold/VOI test**: current best action, what would flip it, probability of that, and cost of finding out (including delay and disruption). | 9 | SRC-FGI-019, -020, -014 |
| F5 | Make **action reversibility** an input to the sufficiency bar, and add "reversible probe" as a type of Next Responsible Action. | 9, handoff | SRC-FGI-013, -012 |
| F6 | Permit and name **"watchful waiting with trigger"** and **"no action warranted"** as valid outcomes. | 9 | SRC-FGI-019, -017, -011 |
| F7 | Validation loop: check **implementation outcomes before effectiveness**, and state a **counterfactual/comparison** plus the leading alternative explanation (especially regression to the mean). | Understand again | SRC-FGI-005, -007, -015 |
| F8 | Ban un-sourced base rates (for example "70% of change fails") from EDF materials. Require reference-class-specific priors. | Evidence (6) | SRC-FGI-001, -016 |

## 7. Testable benchmark hypotheses

- **H-F1 (harm field):** On a benchmark of historical cases with known intervention outcomes, analyses that include an explicit harm/backfire field will rank interventions that later backfired lower than analyses that omit it. Metric: rank of the eventual backfire intervention among proposed control points.
- **H-F2 (sufficiency as VOI):** Given cases where later evidence was released step by step, EDF analysts using the explicit VOI test (F4) will stop with fewer evidence requests than with the current verbal rule, without a significant drop in agreement with the expert-chosen next action. Non-inferiority margin to be pre-registered.
- **H-F3 (permitted no-action outcomes):** On a case set that includes "noise" cases (where expert consensus is that no intervention is warranted), analyses under the F6 rule will recommend interventions in fewer of those cases than analyses under v0.3, with no significant increase in missed interventions on true-problem cases.
- **H-F4 (implementation vs effectiveness):** In retrospective "Understand again" reviews, adding an implementation-outcome check (F7) will lower the rate at which a fidelity failure is misclassified as a wrong diagnosis, as judged by blinded raters.
- **H-F5 (forward chain):** Clarity decisions made from EDF outputs that include F3 forward chains will have more named and later-tested assumptions than decisions from v0.3 outputs.

## 8. Limitations of this review

- The OD meta-analyses (1985 to 1993) are dated. No recent comprehensive OD meta-analysis was located.
- Most implementation-science evidence is from health care. Transfer to commercial organizations is plausible but untested.
- Robertson et al. (1993) effect details, Kluger and DeNisi effect sizes and Pauker and Kassirer formulas were confirmed only through search-result abstracts and summaries, not full texts.
