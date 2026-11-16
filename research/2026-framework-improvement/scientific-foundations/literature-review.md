# Integrated Literature Review: Scientific Foundations for Organizational Diagnosis with EDF

Mission: EDF-IMPROVEMENT-2026-001 · Baseline: EDF v0.3 (`41fea6f`) · Prepared 2026-11-16
Inputs: the eight workstream reviews in `literature/` (lit-org-diagnosis, lit-validity, lit-causal, lit-dynamics, lit-intervention, lit-adaptive, lit-longitudinal, lit-ai-diagnosis), `competing-methodologies.md`, `../evidence/source-register.csv`, `../baseline/*.md`.
Companion files: `methodological-opportunities.md` (candidate changes OPP-01 to OPP-20), `evidence-register.csv` (claims LIT-001 to LIT-060), `longitudinal-assessment.md`.

**Conventions.** Sources are cited by `source_id`. Of the 224 registered sources, 35 were read directly (*fetched*), 187 are *search-result* only (abstract, publisher description or secondary summary), and 2 are *from-memory-unverified* (SRC-CDE-070, SRC-CDE-071). Neither of the last two supports any claim here. A claim is marked **[SR]** when the specific detail cited (a number or a quoted finding) rests only on search-result sources. **Empirical** means data-based. **Theoretical** means argued, formal or practitioner consensus without a test. Experimental results from this mission are not reported here. See experiments/ and strategy/ for measured results (EX-EDF-2026-OD1).

---

## 1. Bottom line

1. **Organizational diagnostic models in general lack validation evidence.** None of the models reviewed has published evidence of *diagnostic accuracy*: inter-diagnostician reliability, agreement with a reference diagnosis, or evidence that model-guided diagnosis picks better interventions than unguided diagnosis. This covers Burke-Litwin, Nadler-Tushman, Weisbord, 7S, Star, Cummings and Worley, culture instruments, Balanced Scorecard and Appreciative Inquiry (SRC-BH-003, SRC-BH-004, SRC-BH-007, SRC-BH-043, SRC-BH-044, SRC-BH-060; `competing-methodologies.md` summary table). EDF v0.3 is no exception. The baseline audit finds no positive, controlled, independently replicated result for any EDF component (`baseline/methodology-inventory.md` §0; `baseline/current-limitations.md` ranks 1–3). The absence was found with non-systematic searches. A database search (PsycINFO, Business Source) is needed to confirm it.
2. **The best available priors come from neighbouring diagnostic sciences.** These are medical diagnostic error, systemic accident analysis, intelligence analysis, troubleshooting research, measurement science and, most recently, LLM sequential-diagnosis benchmarks. They support specific, testable mechanisms. They do not support claims that a whole framework works.
3. **The main threat to organizational diagnosis is the evidence, not the reasoning.** The system being diagnosed produces most of the evidence. That evidence is filtered upward (SRC-CDE-058, SRC-CDE-053, SRC-CDE-059), absorbed at the frontline (SRC-CDE-057), skewed by status (SRC-CDE-009) and corrupted where metrics are targets (SRC-CDE-062, SRC-CDE-064, SRC-FGI-073). EDF v0.3 has no field for source, stake, independence or missingness (`baseline/diagnostic-taxonomy.md` §3).
4. **Diagnosis and intervention are both weakly evidenced, and interventions can do harm.** OD meta-analyses show modest average effects with wide variance (SRC-FGI-002, SRC-FGI-003, SRC-FGI-018). More than a third of feedback interventions reduced performance (SRC-FGI-011, [SR]). The common "70% of change fails" base rate has no valid empirical basis (SRC-FGI-001, fetched). A diagnostic method therefore needs a legitimate "do not act yet" output and a harm dimension. EDF v0.3 has neither (`current-limitations.md` rank 6; `diagnostic-taxonomy.md` §4.2).
5. **Adding structure does not automatically help, and some structure hurts.** Heavy competing-hypothesis matrices did not reduce bias in an RCT and may have increased inconsistency (SRC-BH-072, fetched). Naive question-asking lowered LLM diagnostic accuracy (SRC-FGI-033, [SR]). Chain-of-thought helps mainly on symbolic tasks (SRC-FGI-039, [SR]). Every EDF addition should therefore be ablated against a matched-neutral control. This matches the baseline's rank-1 limitation.

---

## 2. What organizational diagnosis is, and why its evidence base is thin

The field has three lineages:

- **Clinical**: Levinson's history-taking case study (SRC-BH-057).
- **Model-driven OD**: content models that say what to look at and how components should fit (SRC-BH-002, SRC-BH-006, SRC-BH-008, SRC-BH-061).
- **Applied-research / evidence-based**: Harrison's models-methods-processes view, Harrison and Shirom's "sharp-image" and multiframe diagnosis (SRC-BH-055, SRC-BH-056), and McFillen et al.'s call for medical-style evidence-based diagnosis (SRC-BH-059).

The dialogic critique argues that organizations are socially constructed, so the act of inquiry is itself an intervention (SRC-BH-062). All of these are **theoretical** at the level of claims about diagnostic accuracy.

McFillen et al. state that "the diagnostic process itself has received little study". They hypothesize that misdiagnosis contributes to change failure (SRC-BH-059, [SR]). That hypothesis is untested, and the "failure rate" it leans on is itself unsupported (SRC-FGI-001).

Validation evidence that exists is of the wrong kind:

- exploratory utility studies (SRC-BH-003);
- partial hypothesis tests with mixed results (SRC-BH-004, [SR]: 4 of 15 hypotheses, unverified);
- face/content validity of local questionnaires (SRC-BH-007, [SR]);
- construct–performance meta-analyses, such as culture types and effectiveness (SRC-BH-044) or network position and performance (SRC-BH-037, [SR]);
- reviews documenting that most culture instruments lack reliability and validity evidence (SRC-BH-043; authors partly affiliated with a commercial survey vendor).

Construct evidence says nothing about whether a diagnostician using the model reaches a correct, reproducible diagnosis.

The intervention literature that diagnostic models feed into is also weak:

- Lean in healthcare: weak outcome evidence (SRC-BH-017).
- Appreciative Inquiry: minimal (SRC-BH-053, SRC-BH-054).
- Balanced Scorecard: mixed (SRC-BH-014, SRC-BH-015).
- Enterprise Architecture: only about half of value claims are empirically supported (SRC-BH-038).
- Theory of Constraints: likely publication-biased (SRC-BH-020).
- RCA: weak evidence of recurrence prevention (SRC-BH-026, SRC-BH-027, SRC-BH-075).

Three recurring design faults are worth naming because EDF's own validation must avoid them: no controls, self-selected success cases, and outcome attribution that ignores alternative explanations.

**Implication.** EDF cannot claim parity with "validated" competitors, because there are none. It also cannot borrow legitimacy from their popularity. The defensible route is to measure EDF directly with published reliability and validity protocols (section 3) and to treat every component as a hypothesis.

---

## 3. Reliability and validity: what can be measured, and the realistic bar

**Validity attaches to inferences and uses, not to frameworks** (SRC-CDE-002; SRC-CDE-001). Organizational diagnosis rarely has an unambiguous criterion such as a "true cause", so construct validation and manufactured criteria (adjudicated cases, seeded ground truth, prospective outcomes) are required. Discriminant as well as convergent evidence is needed (SRC-CDE-003, bibliographic only). Construct-irrelevant variance matters directly for LLM-scored benchmarks: fluency and format compliance can masquerade as diagnostic quality.

**The nearest measured analogue is systemic accident analysis.** Its methods share EDF's multi-level origin networks and control structures.

- Inter-analyst overlap on causal factors was about 65% for STAMP and 38% for AcciMap. Cross-method overlap was about 8% (SRC-BH-066 / SRC-CDE-051, [SR], one accident).
- A later 30-analyst study found weak-to-moderate inter-rater agreement, with STAMP-CAST significantly higher (SRC-BH-068 / SRC-CDE-050, [SR]).
- Criterion validity against an expert referent was "moderate" and judged unsatisfactory (SRC-BH-067, fetched).
- Different methods applied to the same event produced materially different analyses (SRC-BH-065, [SR]).
- FMEA teams did not reproduce each other's results, and the risk priority number multiplies ordinal scales, which is mathematically invalid (SRC-BH-031, fetched; SRC-BH-032).

Three lessons follow:

- (a) Expect independent EDF analyses to diverge substantially.
- (b) Constrained control-structure representations appear to raise reliability (suggestive, two studies).
- (c) A ready-made measurement protocol exists: factor overlap, Krippendorff's alpha or the correct ICC form (SRC-CDE-004, SRC-CDE-005), and signal-detection hits and false alarms against a referent, with phase-level fragmentation (SRC-BH-067).

**Calibration is measurable when predictions resolve.**

- Clinicians' confidence barely changed between easy and hard vignettes. Their requests for more information *fell* as confidence rose (SRC-CDE-014, [SR]).
- An intelligence unit's 1,514 forecasts were well discriminated but underconfident. They were recalibratable with a simple transform (SRC-CDE-016, fetched).
- Training, teaming and accountability improved Brier scores in randomized tournament experiments (SRC-CDE-018, [SR]).
- Calibration is context-dependent, and difficulty should be rated before outcomes are known (SRC-CDE-017, fetched).

EDF confidence is a three-level label with no rubric. The A003 harness scored it with a linear, improper rule (`baseline/methodology-inventory.md` §12). EDF confidence cannot be scored until it is attached to resolvable predictions and scored with a proper rule.

---

## 4. Diagnostic reasoning: what fails and which remedies have evidence

**Failures are multifactorial, and premature closure is the leading cognitive error.** In 100 internal-medicine error cases there were 5.9 contributing factors per case. System factors contributed in 65% of cases and cognitive factors in 74%. Premature closure was the most common cognitive factor (SRC-BH-070, fetched; SRC-CDE-020). These are case-review data and carry hindsight and selection bias. This is EDF's strongest empirical tailwind: it supports the origin-network default over single root causes (`baseline/methodology-inventory.md` §7). It also locates EDF's main risk in Diagnostic Sufficiency, where closure happens.

**Debiasing remedies are mostly untested, and one is negative.** Of 141 articles on cognitive interventions, few tested them (SRC-BH-071, [SR]). Checklists remain proposals (SRC-CDE-022). The decision-review checklist (self-interest, absence of dissent, provenance of numbers) rests on practitioner and observational evidence (SRC-CDE-024, fetched). The ACH RCT found incomplete adherence, mixed effects on bias, and possibly more inconsistency (SRC-BH-072 / SRC-CDE-023, fetched). Any differential requirement in EDF should therefore be **minimal**: live rivals plus a discriminating test, not a matrix. It should be tested for harm to consistency.

**A generic strategy plus a domain model works better than a generic strategy alone.** Structured troubleshooting, built from cognitive task analysis, combined a domain-independent strategy with context-specific functional models. In a controlled training evaluation it roughly doubled malfunctions solved, in less time (SRC-BH-048, fetched). DMAIC's main weakness, by analytic review, is poor support for efficient diagnosis, partly because generic methods cannot exploit task knowledge (SRC-BH-019, fetched, theoretical). EDF supplies only the generic strategy. For organizations it needs content prompts drawn from the (unvalidated) organizational models. The transfer from equipment to organizations is an analogy and has not been demonstrated.

**Contrast and diagnosticity.** Kepner-Tregoe's IS / IS-NOT specification (SRC-BH-046, [SR], no independent validation) and process tracing's hoop / smoking-gun / doubly-decisive tests (SRC-CDE-029, SRC-CDE-030, SRC-CDE-072, [SR]) give a light grammar for how strongly a piece of evidence discriminates. The logic is well developed. There is little experimental evidence that analysts using these tests reach more accurate conclusions. Bayesian process tracing helps locate disagreement, but probabilities cannot be specified unambiguously in qualitative work (SRC-CDE-031, [SR]). Base rates in natural-frequency form improve Bayesian reasoning (SRC-CDE-019, [SR]). EDF never asks for a reference class.

**Hindsight and local rationality.** Outcome knowledge inflates judged foreseeability (SRC-CDE-025). The general phenomenon is robust, but the specific 1975 effects have a mixed replication record. "Human error" is an attribution, and analysis should recover how actions made sense at the time (SRC-CDE-045, SRC-CDE-040, fetched). EDF writes Steps 4–5 backward from the outcome and has no local-rationality record.

---

## 5. Causal inference: what EDF's causal claims can and cannot rest on

EDF makes two kinds of causal claim without separating them:

- **causes-of-effects**: retrospective origins and propagation (Steps 4–5);
- **effects-of-causes**: control-point influence (Step 8).

Holland's distinction (SRC-CDE-028, [SR]) shows these need different evidence. Step 8 currently reuses single-case retrospective evidence for an interventional claim (`current-limitations.md` rank 4).

Formal results apply by analogy:

- Observational evidence typically supports an **equivalence class** of causal models, not one model, and latent confounding is common (SRC-CDE-027, SRC-CDE-032, formal theory). EDF should report rival networks.
- Even well-identified quasi-experiments can flip sign with the measurement source (SRC-CDE-046 vs SRC-CDE-047, [SR]). Provenance is a causal variable, not bookkeeping.

The safety and healthcare literatures converge on rejecting a single root cause (SRC-CDE-040, fetched; SRC-CDE-037, fetched; SRC-CDE-036, SRC-CDE-038):

- In a review of 302 incident analyses, the commonest recommendations were more training and reinforced policy, and similar events recurred (SRC-CDE-037).
- Investigation manuals determine what is found ("what you look for is what you find"), and fixes are not fully driven by findings (SRC-CDE-043, SRC-CDE-044, [SR]).
- The RCA2 Action Hierarchy ranks structural and forcing-function changes above procedures and training (SRC-BH-028, fetched). This is practitioner consensus grounded in human factors, not tested here.

**Dynamics.** People misperceive feedback and delays in experiments (SRC-CDE-034, [SR], empirical). Policy resistance and drift to the boundary are theory with case support (SRC-CDE-035, SRC-CDE-048). Improvement programmes fail through interaction with existing structures (SRC-BH-022, fetched abstract; field cases plus model). EDF propagation is effectively acyclic (`diagnostic-taxonomy.md` §2.2). For EDF-2 with data, fault trees map to Bayesian networks, which support backward inference (SRC-CDE-033, SRC-BH-030).

---

## 6. Organizational dynamics that corrupt the evidence

This is the most decision-relevant section for organizational use of EDF. Organizational-dynamics findings matter mostly as threats to evidence validity, and secondarily as candidate origins.

- **Upward filtering and silence.**
  - Bad news is passed on less readily than good news (SRC-CDE-058, experimental, [SR]).
  - Silence is a collective phenomenon rooted in managers' fear of negative feedback (SRC-CDE-053, theory with survey support).
  - In one interview study 85% of respondents had felt unable to raise an important issue (SRC-CDE-054, [SR]).
  - Implicit voice theories predict silence with incremental validity (SRC-CDE-060, [SR]).
  - Managers discount critical upward feedback (SRC-CDE-059, [SR]).
- **Frontline absorption.** Direct observation of 26 nurses found 93% of problems handled by first-order workarounds, never reported upward. Only 7% met lenient criteria for second-order problem solving (SRC-CDE-057, fetched). This is the strongest single source here because it does not rely on self-report. It implies that management accounts systematically under-represent process failures.
- **Status-skewed perception.** Higher-status members rate their organization more favourably, and perceptions correlate weakly with objective measures (SRC-CDE-009, fetched; SRC-CDE-010, [SR]). The evidence is thin and methodologically contested.
- **Method and informant effects.** Common-method bias is robust (SRC-CDE-006). Key informants disagree, and their accuracy varies with construct, informant and organization (SRC-CDE-007, SRC-CDE-008, [SR]).
- **Missingness.** Better-led units reported *more* errors (SRC-CDE-041, small n, [SR]). Organizational evidence is often missing not at random (SRC-CDE-012). Absence of reports is not absence of problems.
- **Incentives and metric corruption.** "Rewarding A while hoping for B" (SRC-CDE-062), agency costs (SRC-CDE-067), Campbell's law (SRC-CDE-063 / SRC-FGI-073, exact passage unverified) and Goodhart variants (SRC-CDE-064, fetched abstract) are well-developed theory with many case illustrations. Systematic prevalence evidence is mixed.
- **Normalization of deviance and drift.** Case-based theory (SRC-CDE-061, SRC-CDE-048). It is invisible to insiders because it has become normal.
- **Informal structure.** Coordination runs through informal networks, which ONA can measure (SRC-CDE-066 / SRC-BH-036). Network position relates to performance (SRC-BH-037). This is construct-validated, but its value as an organizational diagnostic rests on cases.
- **Groupthink** as a full model has only partial support (SRC-CDE-068). Record observable dissent facts instead of using the label.
- **Psychological safety** associates robustly but mostly cross-sectionally with learning and voice (SRC-CDE-055, SRC-CDE-056). Use it as an evidence-quality moderator, not a default origin.

A guardrail is needed. A method that distrusts management by default will invent hidden problems. Every source-skepticism rule must be tested on cases where the leadership account is correct (lit-dynamics H-E8; OD1 authoring rule 3 and class K, `benchmarks/benchmark-specification.md` §3).

---

## 7. Intervention effectiveness and the diagnosis-to-action handoff

**Base rates.**

- Meta-analyses of OD interventions are old (1985–1993) and exposed to publication bias. They show attitude effects, an average productivity gain of nearly 0.5 SD for psychologically based interventions (SRC-FGI-018, [SR]), and causal-chain support in aggregate (SRC-FGI-002, [SR]).
- Survey feedback works only with genuine action planning and follow-through (SRC-FGI-004, fetched, systematic review).
- Identical Appreciative Inquiry processes at eight sites produced four transformations and two non-changes (SRC-BH-052, fetched, small comparative case). Context moderates.

**Specifying interventions.** Complex-intervention guidance requires a programme theory and attention to context (SRC-FGI-009, SRC-FGI-010, [SR]). Implementation outcomes are distinct from effectiveness outcomes (SRC-FGI-005, SRC-FGI-007, SRC-FGI-008, SRC-FGI-006; frameworks with broad use, predictive validity outside health unestablished). EDF's "Controllability" conflates authority with implementability. Its validation loop cannot tell a fidelity failure from a wrong diagnosis.

**When to stop diagnosing and when not to act.**

- The test/treatment threshold model (SRC-FGI-019) and value-of-information theory (SRC-FGI-020, SRC-FGI-014) give Diagnostic Sufficiency a formal meaning. Gather evidence only if a plausible result would change the action and the expected improvement exceeds the cost, including delay and disruption.
- Irreversibility creates option value of waiting (SRC-FGI-013, theoretical). Reversibility lowers the bar, as in the one-way/two-way door heuristic (SRC-FGI-012, secondary copies only).
- Action bias is documented outside organizations (SRC-FGI-017, [SR]).
- Backfire is common (SRC-FGI-011).

Together these justify explicit "reversible probe", "watchful waiting with trigger" and "no action warranted" outcomes. No study was found on how often organizational diagnoses *should* end in no action. That is a gap.

Detail: `../intervention-effectiveness/intervention-research.md`.

---

## 8. Efficiency: adaptive and sequential diagnosis

- Sequential tests reach decisions with fewer observations on average (SRC-FGI-026, formal).
- Bayesian sequential diagnosis with test costs dates to 1968 (SRC-FGI-027).
- Computerized adaptive testing used about 12 of 389 items with r = 0.95 against the full bank (SRC-FGI-025, fetched).
- Fully adaptive selection was only marginally better than a good static short form, and a two-stage branching design came close to CAT (SRC-FGI-024, fetched).

The transferable lesson is direction, not magnitude. A short triage followed by a targeted module captures most of the gain. Expected information gain about the model differs from value of information for the decision (SRC-FGI-021, SRC-FGI-022, SRC-FGI-020). EDF's question selection should target evidence that would change the top control point. Risk-based verification can match full verification when critical data are defined in advance, but the evidence is mixed (SRC-FGI-029 trade press; counter-evidence SRC-FGI-030).

Detail: `../efficiency/adaptive-diagnostics.md`.

---

## 9. AI-run diagnosis

**Direct evidence on LLMs diagnosing real organizations: none found.** The nearest study uses case-interview puzzles (SRC-FGI-049, [SR]).

Closest analogues:

- **AI assistance can hurt outside the capability frontier.** On a business-diagnosis task from interviews and spreadsheets, consultants using AI were 19 percentage points less likely to be correct (SRC-FGI-048, [SR], pre-registered field experiment).
- **Human–AI combination is not additive** (SRC-FGI-046, [SR], RCT).
- **Multi-turn, underspecified information gathering degrades LLM reliability.** Performance fell about 39% on average, through early assumptions and premature answers (SRC-FGI-047, fetched). See also SRC-FGI-044 and SRC-FGI-045.
- **Asking more questions is not better.** Naive question-asking hurt, and confidence gating helped (SRC-FGI-033).
- **Functional role structure plus cost awareness helped.** Hypothesis, test-chooser, challenger and stewardship roles improved accuracy per cost on SDBench (SRC-FGI-031, fetched; single-team preprint, hard cases only, no false-positive measurement).
- **Structure versus accuracy.** Structured prompts add interpretability more than accuracy (SRC-FGI-043, SRC-FGI-039).
- **Prompt and reasoning reliability.** Format sensitivity is large (SRC-FGI-042). Written reasoning is unfaithful to what drove the answer (SRC-FGI-038).
- **Confidence.** Verbalized confidence is overconfident (SRC-FGI-040, SRC-FGI-041).
- **Premises.** Models are sycophantic and accept false premises (SRC-FGI-034, SRC-FGI-052, SRC-FGI-054, SRC-FGI-055). This matters because client requests usually embed a causal theory, which is OD1's "presenting request may be wrong" rule.
- **Judges and aggregation.** LLM judges show position, verbosity and self-preference bias (SRC-FGI-035, SRC-FGI-036, SRC-FGI-037). Debate does not reliably beat self-consistency (SRC-FGI-056, SRC-FGI-057, SRC-FGI-058).
- **Contamination.** Benchmark contamination inflates scores (SRC-FGI-050, SRC-FGI-051). Famous cases test recall, not diagnosis.

Almost all of this is search-result-level and comes from medicine or general reasoning, so transfer to organizations is inferred.

---

## 10. Longitudinal assessment

- Continuous electronic monitoring raises stress without reliable performance gains (SRC-FGI-060, [SR], meta-analysis).
- Full observability drove concealment of productive deviations, and privacy improved performance (SRC-FGI-067, [SR], field experiment). Monitoring can blind the diagnostician to workarounds.
- Hawthorne lore is overstated, so reactivity should be measured, not assumed (SRC-FGI-077).
- Scale change can be alpha, beta or gamma change (SRC-FGI-075). Invariance must be tested before comparing over time (SRC-FGI-076).
- Generic early-warning signals give many false alarms (SRC-FGI-069).
- EU and UK rules restrict workplace monitoring. Emotion inference is prohibited, and employment AI is high-risk (SRC-FGI-062 to SRC-FGI-066; secondary legal commentary, not legal advice).

Detail: `longitudinal-assessment.md`.

---

## 11. Findings table

Strength scale: **Strong** = robust empirical or formal result directly on point. **Moderate** = empirical but few studies, an adjacent domain, or correlational. **Weak** = single small study, case-based or contested. **Theoretical** = argued, formal-by-analogy or consensus without a test. **Negative** = tested and not supported. "[SR]" = the key detail rests on search-result-only sources.

| Finding | Evidence strength | Implication for EDF | Source ids |
|---|---|---|---|
| No organizational diagnostic model has published diagnostic-accuracy evidence | Moderate (consistent absence; non-systematic search) | Treat EDF and all competitors as unvalidated; build direct reliability and validity measurement | SRC-BH-003, SRC-BH-004, SRC-BH-007, SRC-BH-043, SRC-BH-060 |
| Systemic causal analyses agree about 38–65% between analysts, about 8% across methods | Moderate [SR] | Expect divergence; adopt overlap and alpha metrics; 65% is the nearest bar | SRC-BH-066, SRC-CDE-051, SRC-BH-068 |
| Control-structure methods (STAMP-CAST) are more reliable than free-form maps | Moderate (2 studies) [SR] | Give control points controller / action / feedback / process-model fields | SRC-CDE-050, SRC-CDE-051, SRC-BH-064 |
| Criterion validity of systemic methods against an expert referent is moderate | Moderate (1 study) | Score EDF against seeded truth with signal detection and phase fragmentation | SRC-BH-067 |
| Diagnostic failures are multifactorial; premature closure is the top cognitive error | Moderate (case review) | Keep the origin network; guard Sufficiency with live rivals | SRC-BH-070, SRC-CDE-020 |
| Heavy competing-hypothesis matrices do not reliably reduce bias and may add inconsistency | Negative (1 RCT) | Use a minimal differential and test it for consistency harm | SRC-BH-072, SRC-CDE-023 |
| Generic strategy plus domain model improves troubleshooting | Moderate (1 controlled evaluation) | Add organizational origin prompts as coverage aids | SRC-BH-048, SRC-BH-019 |
| Multiplying ordinal scores misleads prioritization; FMEA teams disagree | Strong (mathematical) / Moderate (empirical) | Never aggregate ranking criteria arithmetically | SRC-BH-031, SRC-BH-032 |
| RCA recommendations skew to weak controls (training, policy) and events recur | Moderate | Add a control-strength hierarchy; require finding-to-fix traceability | SRC-CDE-037, SRC-CDE-044, SRC-BH-028 |
| Investigation model determines what is found | Moderate [SR] | List unexamined origin categories; add incentive and informational prompts | SRC-CDE-043 |
| Common-method bias inflates relationships; informants disagree | Strong / Moderate [SR] | Provenance fields; triangulation required for high confidence | SRC-CDE-006, SRC-CDE-007, SRC-CDE-008 |
| Higher-status members view the organization more favourably; perceptions weakly track objective data | Weak–Moderate (thin, contested) | Leadership accounts are claims until corroborated | SRC-CDE-009, SRC-CDE-010 |
| Bad news is withheld; organizational silence is widespread | Moderate [SR] | Silence probes; protected frontline sampling | SRC-CDE-058, SRC-CDE-053, SRC-CDE-054, SRC-CDE-060 |
| Frontline workarounds absorb most process failures unseen by managers | Moderate (1 observational study, fetched) | Sample work-as-done; record workarounds as evidence of origins | SRC-CDE-057, SRC-CDE-069 |
| Report counts reflect reporting climate; evidence is often missing not at random | Weak–Moderate | Missingness tags; no absence-of-evidence inference without climate data | SRC-CDE-041, SRC-CDE-012 |
| Metrics used as targets are corrupted | Theoretical + cases | Metric-integrity check; keep non-target indicators | SRC-CDE-062, SRC-CDE-064, SRC-FGI-073 |
| People misperceive feedback and delays; interventions meet policy resistance | Moderate (experiments) / Theoretical | Allow loops in propagation; ask how the system will respond | SRC-CDE-034, SRC-CDE-035, SRC-BH-022 |
| Observational evidence supports a set of causal models | Theoretical (formal) | Report rival networks and discriminating evidence | SRC-CDE-027, SRC-CDE-032 |
| Confidence is insensitive to difficulty and suppresses information-seeking | Moderate [SR] | Sufficiency must not rest on self-rated confidence | SRC-CDE-014 |
| Calibration is scorable, trainable and recalibratable when predictions resolve | Moderate | Attach resolvable predictions; use a proper scoring rule | SRC-CDE-016, SRC-CDE-017, SRC-CDE-018 |
| The "70% of change fails" figure lacks a valid basis | Moderate (critical review, fetched) | Ban unsourced base rates; use reference-class priors | SRC-FGI-001 |
| More than a third of feedback interventions reduce performance | Strong (meta-analysis) [SR] | Add a harm/backfire criterion to ranking | SRC-FGI-011 |
| Survey feedback helps only with follow-through | Moderate (systematic review) | Treat data collection as a commitment | SRC-FGI-004 |
| Identical interventions diverge by context | Weak (comparative case) | Name contextual moderators for control points | SRC-BH-052 |
| Test/treat thresholds and VOI define when to stop | Theoretical (decision theory) | Make Sufficiency a VOI test; allow probe / wait / no-action | SRC-FGI-019, SRC-FGI-020, SRC-FGI-014, SRC-FGI-013 |
| Short triage plus branching captures most adaptive-testing efficiency | Moderate (psychometrics) | EDF-0 as triage that selects modules | SRC-FGI-024, SRC-FGI-025 |
| Naive LLM question-asking hurts; gated, decision-focused asking helps | Weak–Moderate [SR] | Stewardship and expected-decision-impact on evidence requests | SRC-FGI-033, SRC-FGI-031, SRC-FGI-032 |
| AI help reduced correctness on an outside-frontier business-diagnosis task | Moderate (field experiment) [SR] | Human review must check AI causal claims against raw evidence | SRC-FGI-048, SRC-FGI-046 |
| LLMs accept false premises and are sycophantic | Moderate [SR] | Premise check on the presenting request | SRC-FGI-034, SRC-FGI-054, SRC-FGI-055 |
| LLM self-confidence and LLM judges are biased | Moderate [SR] | Agreement-based confidence; cross-family, counterbalanced judging | SRC-FGI-040, SRC-FGI-035, SRC-FGI-036, SRC-FGI-037 |
| Monitoring raises stress and induces concealment | Moderate [SR] | Proportionality gate; aggregate-level only; monitoring as a propagation path | SRC-FGI-060, SRC-FGI-067 |
| Survey change can be beta/gamma, not real change | Moderate (methodological) | Invariance and anchor checks before reading pre/post change | SRC-FGI-075, SRC-FGI-076 |

---

## 12. What remains unknown

- Inter-consultant agreement in **non-accident** organizational diagnosis. No study was located.
- Whether any content model, lens set or structured step improves organizational diagnostic accuracy, for humans or for LLMs.
- How often diagnoses should end in "no action", and the real-world cost of over-diagnosis.
- Whether EDF-selected interventions outperform conventionally selected ones in real organizations. Synthetic benchmarks cannot answer this. A protocol is in `../intervention-effectiveness/evaluation-design.md`.
- Effect sizes that rest only on search-result sources should be checked against full texts before any quantitative use. This applies in particular to SRC-BH-066/SRC-CDE-051, SRC-FGI-011, SRC-FGI-060 and SRC-FGI-048.

## 13. Source-quality notes

- **Duplicates with inconsistent metadata.** Jun et al. appears as 2018 (SRC-BH-066) and as 2019 (SRC-CDE-051). The Hulme et al. reliability study appears as 2023 (SRC-BH-068) and as 2024 with an unconfirmed title (SRC-CDE-050). SRC-BH-067 is fetched but its duplicate SRC-CDE-052 is search-result. Other duplicate pairs: SRC-BH-070/CDE-020, BH-072/CDE-023, BH-022/CDE-065, BH-024/CDE-036, BH-025/CDE-038, BH-063/CDE-048, BH-065/CDE-049, BH-036/CDE-066, CDE-013/FGI-076, CDE-063/FGI-073.
- **Authors not captured.** SRC-BH-004, -005, -007, -014, -026, -033, -075; SRC-FGI-030, -049, -054, -055.
- **Secondary or commercial sources.** Vendor: SRC-BH-047. Trade press: SRC-FGI-029. Secondary copies of a letter: SRC-FGI-012. Law-firm commentary: SRC-FGI-062. Disputed application dates: SRC-FGI-066. Vendor-affiliated authors: SRC-BH-043.
- **Excluded from evidential use.** SRC-CDE-070 and SRC-CDE-071 are from-memory-unverified and are not used.
