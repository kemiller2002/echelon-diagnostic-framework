# Workstream C — Diagnostic Validity and Measurement

Status: staging draft (research input, not a framework change)
Retrieved: 2026-11-16
Scope: what measurement science, key-informant research, forecasting/calibration research, and medical diagnostic-error research imply for EDF v0.3 as an *organizational* diagnostic method.
Source IDs refer to `sources-CDE.csv`. Verification levels: **fetched** (primary text or abstract read directly), **search-result** (bibliographic details and abstract-level claims confirmed via search snippets only), **from-memory-unverified** (flagged; not relied on for any recommendation).

---

## 0. Why this matters for EDF specifically

The current evidence state (`docs/edf/validation/evidence-ledger.md`, `research/analysis/edf-two-family-structural-value-results.md`) shows:

- No validity claim beyond "internal demonstration" (HY-EDF-001); reproducibility and control-point-quality claims are unsupported or suggestive.
- The two-family benchmark found EDF ≈ matched-neutral structure (+0.56 points, CI [0.00, +1.39]) with a severe evaluator **ceiling effect** (E2: 31/33 outputs scored 100).

In measurement terms, that benchmark had an instrument with almost no discriminating range near the top, no external criterion (no ground truth for the causal model), and synthetic cases authored by the same research process. The literature below explains what a valid diagnostic-quality measurement would need, and which EDF fields would have to exist to be measured at all.

---

## 1. Key findings

### 1.1 Validity is a property of inferences and uses, not of a framework

- **Messick (1995)** [SRC-CDE-002, search-result]: validity is "not a property of the test … but of the meaning of the test scores"; content, criterion and construct evidence are aspects of one unified construct validity, and the **social consequences of use** are part of validity. The two main threats are **construct under-representation** and **construct-irrelevant variance**.
- **Cronbach & Meehl (1955)** [SRC-CDE-001, search-result]: construct validation is required when no fully adequate criterion exists — which is exactly the situation for organizational diagnosis (there is rarely an unambiguous "true cause").
- **Campbell & Fiske (1959)** [SRC-CDE-003, search-result; bibliographic only]: discriminant validation (multitrait–multimethod) is required, not only convergence.

Status: theoretical/methodological consensus (foundational, not an empirical effect).

*EDF reading:* "Is EDF valid?" is ill-posed. The validatable claims are of the form "EDF-produced Origin Networks recover adjudicated causal structure better than X on case class Y" (criterion), and "EDF control-point rankings lead to interventions that work" (consequential/predictive). Construct-irrelevant variance in the current benchmark includes prose fluency and rubric-format compliance (the E2 Markdown-fence failures).

### 1.2 Reliability of causal-analysis methods is empirically low

- **Jun, Waterson & Goncalves Filho (2019)** [SRC-CDE-051, search-result]: four studies on one accident (Sewol ferry); inter-analyst reliability (overlap of causal factors) was **STAMP 65% vs AcciMap 38%**; cross-method validity was as low as **8%**.
- **Hulme et al. (2024, *Ergonomics*)** [SRC-CDE-050, search-result]: 30 expert analysts; AcciMap, STAMP-CAST and AcciNet all showed weak-to-moderate inter-rater agreement; **STAMP-CAST significantly higher** than the others; intra-rater differences not meaningful.
- **Hulme et al. (2021, *Safety Science*)** [SRC-CDE-052, search-result]: state-of-the-art systems-based methods "did not produce satisfactory" **criterion-referenced validity** versus a referent expert analysis.
- **Vincent, Carthey, Macrae & Amalberti (2017)** [SRC-CDE-037, fetched]: "the implementation and quality of incident analysis can be highly variable and is often poor"; research on reliability, validity and outcomes of incident-analysis methods is limited.

Status: **empirically supported** (small numbers of studies, safety domain, but direct measurements).

*EDF reading:* This is the closest external analogue to HY-EDF-005. It sets a realistic baseline: independent analysts using structured systemic methods agree on roughly 40–65% of causal factors. EDF has no comparable number. Methods with an explicit **control-structure** representation (STAMP) were more reliable than free-form causal maps (AcciMap), which is a testable design lever for EDF's Origin Network and Control Points.

- **Koo & Li (2016)** [SRC-CDE-004, search-result] and **Hayes & Krippendorff (2007)** [SRC-CDE-005, search-result]: reliability must be reported with the correct ICC form (model/type/definition) or with Krippendorff's alpha (any number of coders, missing data, any scale). Correlation is not agreement.

Status: methodological standard.

### 1.3 Key informants, self-report, and common-method bias

- **Podsakoff, MacKenzie, Lee & Podsakoff (2003)** [SRC-CDE-006, search-result]: method variance (same source, same method, same time) materially inflates or deflates observed relationships; remedies are procedural first (separate sources, temporal/methodological separation, protecting anonymity) and statistical second.
- **Kumar, Stern & Anderson (1993)** [SRC-CDE-007, search-result]: key-informant research faces a **selection problem** (finding informants competent to report) and a **perceptual agreement problem** (competent informants frequently disagree).
- **Homburg, Klarmann, Reimann & Schilke (2012)** [SRC-CDE-008, search-result]: meta-analysis of 127 studies + 8 multi-informant datasets; informant accuracy depends on construct-, informant-, organization- and industry-level characteristics; they give guidelines on *when* triangulation is needed. (Secondary-source numbers — e.g., performance items more accurate than internal-process items — were **not** verified and are not relied on.)
- **Starbuck & Mezias (1996, *J. Organizational Behavior*)** [SRC-CDE-009, fetched — working-paper version]: few studies ever compare managers' perceptions with objective data; citing Payne & Pugh (1976), members' perceptions of their own firm's structure/climate "correlate weakly with 'objective' measures", members disagree so much that averages were "uninterpretable", and "**higher-status members generally have more favorable views of their organizations**". Averaged perceptions can look accurate even when almost all individuals are inaccurate.
- **Mezias & Starbuck (2003)** [SRC-CDE-010, search-result]: follow-up studies indicate managers' perceptions "are often very inaccurate".
- **Nederhof (1985)** [SRC-CDE-011, search-result]: social desirability splits into self-deception and other-deception; no single remedy excels; combine detection and prevention.

Status: common-method bias and informant disagreement are **empirically well supported**; the magnitude of managerial perceptual error is supported but by a thin and methodologically contested literature (Starbuck & Mezias themselves note research-method artefacts may explain some error).

*EDF reading:* EDF's Step 6 ("separate direct observation from inference") does not distinguish *who* observed, *by what method*, and *with what stake*. A diagnosis built from executive interviews is a single-source, single-method, interested-party measurement — the textbook case for common-method and status-related bias.

### 1.4 Missing data and "absence of reports"

- **Rubin (1976)** [SRC-CDE-012, search-result]: the missing-data mechanism can be ignored only under missing-at-random conditions; otherwise inference is biased.
- **Edmondson (1996)** [SRC-CDE-041, search-result]: better-led nursing units *reported more* drug errors, attributed to willingness to report rather than more errors. Reported-error counts therefore measure reporting climate as much as error.

Status: Rubin is formal theory; Edmondson is a small (8-unit) empirical study, frequently cited, consistent with later voice/silence research.

*EDF reading:* In organizations, evidence is very often **missing not at random** — the problems least reported are the ones people fear reporting. "No evidence of X" from an incident system or management report is not evidence of no X.

### 1.5 Measurement invariance

- **Vandenberg & Lance (2000)** [SRC-CDE-013, search-result]: measurement invariance across groups is a "logical prerequisite" to cross-group comparison and is seldom tested in organizational research.

*EDF reading:* Comparing manifestations across units (e.g., survey scores, incident rates, engagement) assumes the instrument means the same thing in each. Diagnostic Calibration (multiple EDF analyses) also assumes analysts use field definitions invariantly — which the two-family evaluator disagreement on SYN-003 suggests is not guaranteed.

### 1.6 Calibration and confidence

- **Meyer, Payne, Meeks, Rao & Singh (2013)** [SRC-CDE-014, search-result]: 118 physicians, 4 vignettes; accuracy fell sharply on hard cases but confidence barely changed (7.2 vs 6.4/10); on hard cases calibration was worse (overconfidence), and **requests for additional diagnostic tests fell as confidence rose** — i.e., clinicians asked for less information exactly when they needed more.
- **Berner & Graber (2008)** [SRC-CDE-015, search-result]: physicians generally underestimate how often their diagnoses are wrong; lack of feedback is a contributor. (Note: the "overconfidence hypothesis" has critics; treated as plausible, not settled.)
- **Mandel & Barnes (2014, PNAS)** [SRC-CDE-016, fetched]: 1,514 strategic-intelligence forecasts from one organization; AUC 0.94, calibration index 0.016; miscalibration was mostly **underconfidence**, worse for harder/more important forecasts; a recalibration transform cut mean absolute deviation from ~12.7% to ~4.2%, with an overfitting caveat.
- **Tetlock & Mellers (2014, PNAS commentary)** [SRC-CDE-017, fetched]: calibration is context-dependent; question difficulty should be rated *in advance* to avoid hindsight; accountability and short horizons help; neither good nor bad calibration is a general trait of analysts.
- **Mellers et al. (2014, *Psychological Science*)** [SRC-CDE-018, search-result]: randomized experiments in the IARPA tournament showed training, teaming and accountability improved Brier scores.

Status: **empirically supported** that (a) self-rated confidence is a weak signal of diagnostic accuracy on hard cases, (b) calibration is measurable with Brier/calibration indices when predictions resolve, (c) calibration can be trained and recalibrated with track records. Direction of miscalibration is domain-dependent (overconfidence in clinical vignettes; underconfidence in one intelligence unit).

*EDF reading:* EDF's "Confidence" field and Step 9 ("Do we understand enough…?") currently rely on the analyst's own judgment. Meyer et al. show that this is precisely the judgment that degrades on hard cases, and that it suppresses information-seeking. EDF confidence is currently not scorable because it is not attached to resolvable predictions.

### 1.7 Base rates

- **Gigerenzer & Hoffrage (1995)** [SRC-CDE-019, search-result]: Bayesian reasoning improves when information is in **natural frequencies**; up to ~50% of inferences became Bayesian without instruction.

*EDF reading:* EDF does not ask for base rates of origin classes ("in comparable organizations, how often is a delivery-slip outcome primarily a staffing vs. a requirements-volatility problem?"). Without priors, vivid single-case evidence dominates.

### 1.8 Diagnostic error research in medicine

- **Graber, Franklin & Gordon (2005)** [SRC-CDE-020, search-result]: 100 diagnostic-error cases; error was usually **multifactorial (system + cognitive)**; **premature closure** — failing to consider alternatives after an initial diagnosis — was the most common cognitive factor; system factors were dominated by policies/procedures, inefficient processes, teamwork and communication.
- **Croskerry (2003)** [SRC-CDE-021, search-result]: catalog of "cognitive dispositions to respond" (anchoring, premature closure, overconfidence …) and proposed **cognitive debiasing** via metacognition and forcing strategies.
- **Ely, Graber & Croskerry (2011)** [SRC-CDE-022, search-result]: proposed general, differential-diagnosis and pitfall checklists; explicitly **not rigorously evaluated**.
- **Dhami, Belton & Mandel (2019)** [SRC-CDE-023, fetched]: RCT with 50 intelligence analysts; ACH-trained analysts did not follow all steps, evidence on reducing confirmation bias was mixed, and "**ACH may increase judgement inconsistency and error**".
- **Kahneman, Lovallo & Sibony (2011)** [SRC-CDE-024, fetched]: 12-question decision-review checklist (self-interest of the recommending team; absence of dissent "should sound an alarm"; credible alternatives; where the numbers came from). Supporting evidence cited is an observational McKinsey study of >1,000 decisions (process quality associated with up to ~7 points higher ROI) — **practitioner evidence, not a controlled test**.

Status: premature closure and multifactorial causation are **empirically supported** (case-review designs, subject to selection and hindsight). The effectiveness of debiasing checklists and structured alternatives methods is **theoretical/unsupported**, and one RCT (ACH) is negative.

*What transfers:* (1) premature closure as the dominant cognitive failure; (2) the requirement to carry a differential; (3) system + cognitive co-causation; (4) feedback/calibration loops. *What does not transfer cleanly:* medicine has reference standards (autopsy, pathology, follow-up) and base-rate epidemiology; organizational diagnosis usually has neither, so criterion validity must be manufactured (adjudicated cases, seeded ground truth, prospective intervention outcomes).

---

## 2. Empirically supported vs theoretical — summary

| Claim | Status | Key sources |
|---|---|---|
| Independent analysts using systemic causal methods agree only moderately; control-structure methods (STAMP) agree more | Empirical (few studies) | SRC-CDE-050, -051 |
| Systemic methods have unsatisfactory criterion validity vs expert referent | Empirical (one study) | SRC-CDE-052 |
| Single-source/single-method data inflate relationships | Empirical, robust | SRC-CDE-006 |
| Informants disagree; accuracy varies by construct and informant | Empirical | SRC-CDE-007, -008 |
| Higher-status members view their organization more favorably; perceptions weakly track objective measures | Empirical but thin/contested | SRC-CDE-009, -010 |
| Report counts reflect reporting climate | Empirical (small n) | SRC-CDE-041 |
| Confidence is poorly sensitive to case difficulty and suppresses information-seeking | Empirical (vignettes) | SRC-CDE-014 |
| Calibration is scorable, trainable, recalibratable | Empirical | SRC-CDE-016, -017, -018 |
| Premature closure is the most common cognitive error | Empirical (case review) | SRC-CDE-020 |
| Debiasing checklists improve diagnosis | Theoretical / untested | SRC-CDE-021, -022, -024 |
| Structured competing-hypotheses procedures improve accuracy | **Not supported** (one RCT negative) | SRC-CDE-023 |

---

## 3. Concrete implications for EDF

Each item names the step/field affected and is phrased so it can be ablated.

**C1. Evidence provenance fields (Step 6; Required Output "Evidence").**
Every evidence item records: `source_role` (executive / middle manager / frontline / staff function / external / system-of-record), `method` (interview, survey, document, telemetry/log, direct observation, artefact), `stake` (does the source's standing depend on the conclusion? yes/no/unknown), `independence_group` (items sharing a source, method, or upstream document share a group), and `time_relative_to_outcome` (pre-outcome record vs post-outcome account). Rationale: SRC-CDE-006, -007, -009.

**C2. Triangulation rule for confidence (Step 6 → Confidence).**
An origin or propagation edge may not be rated above *Moderate* confidence unless supported by ≥2 independence groups that differ in method **or** source role. Post-outcome accounts from interested parties count as one group regardless of how many people give them. Rationale: common-method bias; key-informant disagreement; Homburg-style triangulation guidelines.

**C3. Informant disagreement is a manifestation, not noise (Steps 3 and 6).**
Record where informants at different levels disagree. Systematic level-ordered disagreement (leaders more favorable than frontline) is itself diagnostic evidence about information flow. Rationale: SRC-CDE-007, -009.

**C4. Missingness classification (Step 6; "Unknowns").**
Each important unknown is tagged: `not collected`, `unavailable`, or `plausibly suppressed` (missing-not-at-random: the people who know have reasons not to say). Low incident/complaint counts may not be used as evidence of absence unless reporting climate is independently assessed. Rationale: SRC-CDE-012, -041.

**C5. Differential requirement with an exit test (Step 4).**
Step 4 must carry ≥2 rival Origin Networks (or rival key edges) until a discriminating piece of evidence is identified for each; record which evidence eliminated which rival. *Caution:* the ACH RCT (SRC-CDE-023) shows heavy matrix procedures can increase inconsistency, so the rule should be minimal (rivals + discriminating test), not a full ACH matrix, and must be benchmarked rather than assumed.

**C6. Resolvable diagnostic predictions (new field under Evidence/Confidence).**
Each high-ranked origin carries ≥1 *prediction that would be checked by the next responsible action* ("if this origin is operative, queue-age data for team B will show …"). Confidence is stated as a probability. This makes EDF confidence Brier-scorable across cases and enables recalibration (SRC-CDE-016, -018).

**C7. Diagnostic Sufficiency cannot rest on self-rated confidence (Step 9).**
Replace the single question with: (a) are the top rival networks still live? (b) would any feasible evidence change the top-ranked control point? (c) is the next action robust across live rivals? Rationale: confidence insensitivity and reduced information requests on hard cases (SRC-CDE-014).

**C8. Base-rate prompt (Step 4).**
Ask for the reference class and approximate frequency of competing origin types, in natural-frequency form where possible (SRC-CDE-019). Optional at EDF-0.

**C9. Outcome-blind reconstruction (Steps 3–5).**
Where feasible, reconstruct pre-outcome signals from pre-outcome records before reading post-outcome accounts (hindsight: SRC-CDE-025 — note mixed replication record).

**C10. Validity programme definition (validation docs, not spec).**
Define EDF validity targets explicitly: criterion (origin-edge recall/precision vs adjudicated or seeded ground truth), reliability (Krippendorff's alpha on edges and on top-3 control points across independent analysts), calibration (Brier on C6 predictions), consequential (intervention outcome where available). Report ICC/alpha with intervals (SRC-CDE-004, -005).

---

## 4. Testable hypotheses for the benchmark

The two-family result recommends harder cases, deterministic measures and **targeted ablations**. Each hypothesis below is a single-construct ablation (EDF with vs without the rule), on cases with seeded ground truth.

| ID | Hypothesis | Case design | Primary measure |
|---|---|---|---|
| H-C1 | The triangulation rule (C2) reduces the rate at which analyses rate a *seeded false* management-favored origin at ≥Moderate confidence | Case packets where executive interviews converge on a wrong origin; frontline logs/telemetry contain the true one | False-origin endorsement rate; true-origin recall |
| H-C2 | Provenance fields (C1) increase evidence-citation precision and reduce claims unsupported by the packet | Same packets | Citation precision; forbidden-claim rate |
| H-C3 | Missingness tagging (C4) reduces "absence of reports ⇒ absence of problem" inferences | Packets where a unit has low incident counts and low psychological-safety indicators | Rate of explicit evidence-of-absence errors |
| H-C4 | The differential requirement (C5) reduces premature closure **without** increasing between-run inconsistency | Packets with a salient early clue pointing to the wrong origin and a later discriminating clue | Premature-closure rate; Krippendorff's alpha across repeat runs |
| H-C5 | Resolvable predictions (C6) yield better-calibrated confidence (lower Brier) than free-text confidence | Multi-stage cases where later evidence resolves predictions | Brier score; calibration slope |
| H-C6 | Revised Sufficiency test (C7) increases appropriate "investigate further" decisions on hard cases and does not increase them on easy cases | Mixed-difficulty set with pre-rated difficulty (per SRC-CDE-017) | Sensitivity/specificity of "sufficient" vs a key |
| H-C7 | EDF inter-analyst agreement on Origin Network edges exceeds the AcciMap-like baseline (~38% overlap) and approaches STAMP-like levels (~65%) | Same case, ≥5 independent analyzer runs per condition | Edge overlap; Krippendorff's alpha |
| H-C8 | Ceiling removal: deterministic measures (H-C1…C5) discriminate between conditions where the 0–4 rubric did not | Re-score existing two-family outputs + new hard cases | Proportion of outputs at ceiling; effect-size CI width |

Note: H-C4 is a direct test of whether EDF should adopt a differential at all, given SRC-CDE-023's negative ACH result.

---

## 5. Limits of this review

- Several key items are verified only at abstract level (search-result). Effect sizes beyond those listed were deliberately not reported.
- The accident-analysis reliability studies use different metrics; the 38%/65% figures are from one accident and should be treated as an order-of-magnitude baseline.
- Medical diagnostic-error findings come from case-review designs subject to selection and hindsight bias.
