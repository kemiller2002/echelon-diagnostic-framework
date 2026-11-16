# Field Evaluation Protocol: EDF-Selected vs Conventionally Selected Interventions

Mission EDF-IMPROVEMENT-2026-001 · Baseline EDF v0.3 (`41fea6f`) · 2026-11-16 · Status: **draft protocol, not yet run**
Related documents: `intervention-research.md` (literature), `outcome-measurement.md` (measures), `../scientific-foundations/methodological-opportunities.md` (OPP-xx). Source ids refer to `../evidence/source-register.csv`.

Synthetic benchmarks such as EX-EDF-2026-OD1 cannot show that EDF improves real organizational outcomes. The OD1 specification says so itself (`../benchmarks/benchmark-specification.md` §7). For OD1 measured results, see experiments/ and strategy/ (EX-EDF-2026-OD1). This document specifies how to test the consequential claim in real organizations.

**Sample-size notes.** Figures marked *(arithmetic)* are standard normal-approximation power calculations made for this document. They are not sourced from the register, and every input (effect size, ICC, correlation) is an illustrative assumption to be replaced from pilot data.

---

## 1. Questions and estimands

| ID | Question | Estimand | Primary design |
|---|---|---|---|
| Q-SEL | Does EDF choose *better interventions* than the conventional process, with execution held constant? | Difference in outcome between implementing EDF's choice and implementing the conventional choice, among problems where the two disagree | Design A |
| Q-TOT | Does using EDF in a unit improve that unit's outcomes, including all downstream effects (choice, buy-in, execution)? | Average effect of assigning a unit to EDF diagnosis vs conventional diagnosis | Design B (or C/D if randomization is infeasible) |
| Q-ACC | Are EDF diagnoses more *accurate* and better *calibrated*? | Difference in Brier score on pre-registered, resolvable predictions | Design E (embedded in all) |
| Q-HARM | Does EDF change the rate of harmful or backfiring interventions, or of unwarranted interventions? | Rate of pre-defined harm events; rate of "no action warranted" decisions later confirmed or refuted | All designs |
| Q-COST | What does EDF cost in diagnostic time and disruption? | Person-hours of diagnosis; data-collection burden on staff | All designs |

**Comparator ("conventional selection").** One of the following, fixed per study and pre-registered:

- (i) the organization's usual management process for choosing a fix;
- (ii) a typical external-consultant engagement;
- (iii) an established method, such as Kepner-Tregoe problem and decision analysis framed by Nadler-Tushman. This mirrors OD1 condition `C_KTNT`.

Choices (i) and (iii) are preferred. Choice (ii) varies too much to replicate.

**Intervention under test.** A frozen, versioned EDF specification, either v0.3 or a named variant, plus a frozen analyst guide. The version is hashed and cited, following the same discipline as `../benchmarks/held-out-policy.md`.

---

## 2. Unit of analysis and eligibility

- **Problem episode.** A presenting organizational problem with a named owner and a measurable manifestation. A *unit* is the organizational entity in which an intervention is implemented, such as a site, team, branch or division.
- **Eligible organizations:**
  - multi-unit organizations (6 or more comparable units), which make Designs A and B feasible; or
  - single organizations with a stable measured outcome series of at least 12 pre-period points, for Design D.
- **Exclusions:**
  - safety-critical emergencies, where delay for evaluation is unethical;
  - problems whose candidate interventions involve individual discipline or dismissal;
  - settings where the data needed would require individual-level monitoring outside the proportionality gate (`../scientific-foundations/longitudinal-assessment.md`).

---

## 3. Design options

### Design A: Discordance-randomized dual diagnosis (core design for selection quality)

1. **Independent diagnosis.** Two diagnostic teams diagnose the same problem episode independently. One uses EDF and one uses the comparator. They have the same access to evidence, the same time budget and the same sponsor briefing, and they are blind to each other's work.
2. **Recorded outputs.** Each team records:
   - its top-ranked intervention;
   - its rank-2 alternative;
   - its sufficiency decision (intervene / probe / gather / monitor / no action);
   - at least three resolvable predictions with probabilities (Design E).
3. **Independent concordance rating.** An adjudicator who did not take part in either diagnosis rates whether the two top interventions are materially the same.
   - **Concordant:** implement it. The episode contributes to Q-ACC and Q-COST, but not to Q-SEL.
   - **Discordant and in equipoise:** the owner and an independent reviewer judge both options acceptable, with neither known to be harmful. Randomize which recommendation is implemented. If several comparable units share the problem, randomize units to EDF's choice or the comparator's choice instead.
   - **Discordant, not in equipoise:** do not randomize. Implement the owner's choice and record the result as observational.
4. **Hold execution constant.** The same implementation team and the same resources execute whichever option is chosen, under a fidelity protocol (§4).

- **Why this design.** It is the only design that separates *choice* from *execution* and from the method's effect on buy-in. Only discordant cases carry information about selection, so the analysis conditions on discordance.
- **Threats.**
  - The discordance rate may be low. That is itself informative: it means EDF and the comparator mostly agree.
  - Equipoise may rarely hold.
  - Problem episodes are heterogeneous.
  - The two teams may differ in skill. Mitigate by rotating analysts across methods, or by training each analyst in both methods and assigning methods randomly.
- **Rough size *(arithmetic)*.** Suppose a binary "manifestation improved beyond pre-registered threshold" outcome, with an expected success rate of 40% for the comparator. Detecting 65% for EDF's choice (alpha 0.05 two-sided, 80% power) needs about 60 discordant episodes per arm, so about 120 discordant episodes. At an assumed 30% discordance rate, that means about 400 dual-diagnosed episodes. This is a multi-organization consortium study. A pilot of 20–40 episodes should estimate discordance, equipoise and fidelity rates first.

### Design B: Cluster-randomized or stepped-wedge across units (total effect)

- **Cluster RCT.** Randomize units to EDF-led diagnosis or conventional diagnosis for all eligible problem episodes over a fixed window, typically 12 months. Stratify by unit size and baseline outcome.
- **Stepped-wedge.** Use this when every unit must eventually receive EDF, or when roll-out capacity is limited. All units start conventional, and blocks of units cross to EDF at randomized times, for example 12 units in 4 steps of 3 over 5 periods. Analyse with a mixed model that has period fixed effects, because time trends are confounded with exposure.
- **Contamination.** Managers trained in EDF may spread its ideas. Randomize at a level that limits spillover, such as region or division, and measure diffusion with a short exposure questionnaire.
- **Rough size *(arithmetic)*.**
  - *Unit-level outcome* (one value per unit, for example a site's on-time rate). Detecting d = 0.5 needs about 64 units per arm. With a baseline covariate correlated r = 0.6 with the outcome, ANCOVA reduces this to about 41 per arm. Detecting d = 0.8 needs about 26 per arm.
  - *Employee-level outcome nested in units*, with an assumed 30 respondents per unit and ICC = 0.05. The design effect is about 2.45. Detecting d = 0.3 needs about 15 units per arm.
  - Stepped-wedge designs usually need fewer clusters than parallel designs at the same power when ICC is moderate. Compute power by simulation from pilot data.

### Design C: Matched comparison with difference-in-differences (DiD)

Use this when randomization is refused.

- Match EDF units to comparison units on pre-period outcome level and trend, size, industry segment and leadership tenure.
- Estimate the DiD effect on the manifestation and outcome measures.
- Required checks:
  - parallel pre-trends, with an event-study plot of at least 4 pre-period points;
  - a placebo intervention date;
  - sensitivity to the measurement source, since records and surveys can give opposite conclusions (SRC-CDE-046 vs SRC-CDE-047, [SR]).
- **Threats.** Selection into EDF (motivated managers volunteer), history, and regression to the mean. Diagnosis usually follows a bad period (SRC-FGI-015, [SR]).

### Design D: Interrupted time series (single organization or few units)

- Use a segmented regression of the outcome series, with autocorrelation modelled, around the intervention date.
- Strengthen it with:
  - multiple baselines: staggered intervention start across units;
  - a non-equivalent dependent variable that the intervention should *not* affect;
  - a comparison series.
- **Rough size.** A common methodological rule of thumb is at least 8 points per segment, preferably 12 or more. This rule is not sourced in the register; treat it as an assumption.
- **Limitation.** ITS estimates the total effect of the diagnose-and-intervene episode. It cannot isolate selection quality without Design A or E.

### Design E: Prospective prediction scoring (embedded in every design)

- Each diagnosis records resolvable predictions before implementation. For example: "If origin X is operative, queue age in team B will fall by at least 20% within 8 weeks of control Y." Each prediction carries a probability (OPP-14).
- An adjudicator blind to which method made each prediction scores them when they resolve.
- Compare methods on Brier score and calibration slope (SRC-CDE-016, SRC-CDE-018).
- **Why.** This measures diagnostic *accuracy* independently of whether the intervention was executed well, and it accumulates evidence even from concordant cases.
- **Rough size.** A meaningful calibration curve needs on the order of 100 or more resolved predictions per method (assumption). Predictions are not independent within an episode, so use a mixed model or a cluster bootstrap at episode level.

### Design choice guide

| Situation | Use |
|---|---|
| Multi-unit organization, owner accepts randomization | B, with A nested where problems recur across units, plus E |
| Consortium of organizations, problem-level randomization acceptable | A plus E |
| Randomization refused, comparable units exist | C plus E |
| Single organization with long outcome series | D plus E |
| Any setting, minimal disruption | E alone (accuracy only; makes no claim about outcomes) |

---

## 4. Separating selection quality from execution quality

1. **Same executor, same resources.** In Design A the implementation team does not depend on which method chose the intervention. If feasible, the team is not told which method chose it.
2. **Implementation outcomes measured first.** Measure adoption, fidelity, reach, dose and implementation cost with a pre-specified fidelity checklist per intervention (SRC-FGI-005, SRC-FGI-007, [SR]; OPP-15). An effectiveness failure with low fidelity is classed as *not evaluable for selection*, not as a wrong diagnosis.
3. **Analysis populations.**
   - *Intention-to-select* (primary): all randomized discordant episodes.
   - *Fidelity-adjusted* (secondary): a complier-average estimate among episodes that reached the pre-specified fidelity threshold.
4. **Ex-ante decision-quality rating, outcome-blind.** Before any outcome is known, a blinded panel rates each method's recommended intervention against the evidence file on four questions:
   - Does it trace to diagnosed origins?
   - Is its control strength adequate?
   - Is the harm risk acknowledged?
   - Is the reversibility proportionate?

   This guards against outcome and hindsight bias in judging choices (SRC-CDE-025).
5. **Counterfactual record.** Both methods' choices and predictions are recorded in every episode, including concordant and non-randomized ones. The unchosen option's predicted effect can then be compared descriptively with the observed outcome.
6. **Implementation context.** Record CFIR-style determinants (SRC-FGI-006), such as readiness, capacity and fit, as moderators. This addresses the context dependence seen in SRC-BH-052.

---

## 5. Measures (summary; full detail in `outcome-measurement.md`)

- **Primary.** The pre-registered manifestation metric for the problem episode, judged against a pre-registered threshold and comparison (trend, comparison unit or ITS counterfactual), at the primary window. The default window is 6 months, adjustable per problem type and fixed in advance.
- **Secondary.**
  - Outcome measures at 12 and 24 months for sustainability.
  - Implementation outcomes.
  - Brier score.
  - Rate of "no action" or "probe" decisions and their later confirmation.
  - Diagnostic person-hours and staff burden.
- **Harms.** Pre-defined harm events: metric gaming detected in a non-target indicator, displacement of cost onto adjacent units, attrition spike, safety or quality incidents, and survey-trust indicators. Monitored by an independent reviewer (§7).
- **Measurement integrity.** At least one non-target indicator per episode (SRC-FGI-073). Invariance or anchor checks on survey scales before reading change (SRC-FGI-075, SRC-FGI-076).

---

## 6. Analysis plan (to pre-register)

- **Pre-registration** before the first episode: hypotheses, primary outcome, windows, analysis populations, models, stopping rules, and the frozen EDF and comparator versions. Publish null and negative results, following the EDF repository's own claim policy (`../baseline/current-limitations.md`, "What the repository does well").
- **Models.**
  - Design A: a mixed-effects logistic or linear model, with episode-type and organization random effects.
  - Design B: a cluster-level analysis or a mixed model. Stepped-wedge adds period effects.
  - Design C: two-way fixed-effects DiD, with heterogeneity-robust estimators if adoption is staggered.
  - Design D: segmented regression with autoregressive errors.
- **Multiplicity.** One primary outcome per question. Secondary outcomes are reported with intervals and labelled as secondary.
- **Heterogeneity**, all pre-specified and exploratory: organization type (for-profit, non-profit, public), industry, problem class (aligned with OD1 classes, e.g. conflicting accounts, incentive-driven, metric-concealed, non-existent or tradeoff), and complexity level (EDF-0/1/2).

---

## 7. Ethics, consent and governance

- **Ethics review.** Obtain approval from an independent research ethics committee or IRB, even when an organization frames the work as internal improvement, because staff data and randomization are involved.
- **Consent in the employment context.** Employee consent is rarely a valid legal basis for processing in employment, because of the power imbalance (SRC-FGI-063, [SR]; SRC-FGI-062, secondary commentary; not legal advice). Use organizational agreement as the lawful basis for processing. Seek *individual informed consent for research participation* in interviews and surveys, with a real right to decline and no manager visibility of who declined. Involve employee representatives or works councils in the design.
- **Confidentiality.**
  - Frontline testimony is reported only in aggregate, with minimum group sizes. No individual is identifiable to management.
  - Raw interview data stays with the evaluation team.
  - Without this, the silence mechanisms (SRC-CDE-053, SRC-CDE-057) will bias both arms.
- **No individual-level decisions.** Diagnoses may not be used for performance evaluation, discipline or selection of individuals. AI-assisted analysis must not infer emotions, which is prohibited at work in the EU since February 2025 (SRC-FGI-065, [SR]). If any component evaluates workers, treat it as high-risk under the EU AI Act Annex III (SRC-FGI-066; confirm dates against the Official Journal).
- **DPIA** where processing is likely high-risk (SRC-FGI-062, [SR]).
- **Equipoise and harm.** Randomize only between options that the owner and an independent reviewer judge acceptable (Design A).
  - Use a data-monitoring role, independent of Echelon, to review harm events at pre-set intervals.
  - Stop or modify if harm events in one arm exceed a pre-registered threshold. More than a third of feedback interventions harmed performance (SRC-FGI-011), so harm is a realistic outcome, not a formality.
- **Withholding.** Conventional diagnosis is the status quo, so assignment to it withholds nothing standard. A stepped-wedge design offers EDF to all units eventually.
- **Respondent burden and response commitment.** Each data-collection round has an owner and a feedback commitment. Data gathered without action erodes trust and response rates (SRC-FGI-004).
- **Conflict of interest.** Echelon designed EDF. Analysis must be done by an independent evaluator, with the analysis code and frozen versions published, and the comparator delivered competently (no straw comparator). Comparator analysts should be trained to the same standard and paid the same.
- **Organizational confidentiality.** Organizations may be anonymized in publications. Aggregate results are published whatever their direction.

---

## 8. Minimal executable pilot (first step)

1. Recruit 3–5 organizations, each with 6 or more comparable units or a long outcome series.
2. Train 4 or more analysts in both EDF and the comparator. Assign methods to episodes at random.
3. Run 20–40 dual-diagnosed episodes (Design A procedure) with Design E predictions.
4. Pilot endpoints:
   - discordance rate;
   - equipoise rate;
   - fidelity rate;
   - prediction-resolution rate;
   - diagnostic hours per method;
   - staff burden;
   - harm events;
   - inter-analyst agreement on origins and top-3 control points (Krippendorff's alpha; SRC-CDE-005; OPP-16).
5. Decision gate: proceed to the full Design A or B only if discordance is 20% or more, equipoise is 50% or more of discordant episodes, and fidelity is 70% or more. These thresholds are proposals to fix at pre-registration.

---

## 9. Main threats and mitigations

| Threat | Mitigation |
|---|---|
| Regression to the mean (diagnosis follows a bad period) | Comparison units or series; pre-period trend; never pre/post alone (SRC-FGI-015) |
| Analyst skill confounded with method | Cross-train analysts and randomize method per episode; record analyst ID as a random effect |
| Execution differs by arm | Same executor; fidelity measured; fidelity-adjusted secondary analysis |
| Outcome or hindsight bias in judging choices | Outcome-blind ex-ante rating; Brier scoring of pre-registered predictions |
| Beta/gamma change in survey outcomes | Invariance and anchor checks (SRC-FGI-075, SRC-FGI-076) |
| Metric gaming of the primary outcome | Non-target indicators; process records (SRC-FGI-073) |
| Reactivity to being studied | Measure it with unstudied comparison units where possible; do not assume a "Hawthorne effect" (SRC-FGI-077) |
| Sponsor pressure to adopt a preferred fix | Stake declaration (OPP-02); traceability; independent adjudicator |
| Publication bias (the field's chronic problem; SRC-BH-020) | Pre-registration; commitment to publish nulls |
