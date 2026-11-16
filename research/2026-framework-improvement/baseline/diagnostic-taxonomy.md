# EDF Diagnostic Taxonomy and Critique (Baseline)

Audit agent: Agent A
Baseline commit: `41fea6ffc764adc69643d5ee2940916592486f84`

This file lists every categorical scheme EDF uses, as written, and then critiques each one for **organizational diagnosis**: diagnosing dysfunction in a company or team, as opposed to engineering incidents. All schemes are prose lists. **None has definitions, decision rules, worked coding examples, or inter-rater reliability data.** That alone means none can currently be applied reliably in the measurement sense.

---

## 1. Origin types

### 1.1 As written

| Source | Categories |
|---|---|
| v0.3 spec `docs/edf/specification.md:96-103`; `docs/edf/model.md:81` | Technical, Human, Organizational, Environmental, Financial, Regulatory |
| v1.0 draft `docs/edf/specification-v1-draft.md:77-87` | Technical, Human, **Operational**, Organizational, **Informational**, Regulatory, Environmental (Financial dropped). "These categories are helpful but not mandatory." |
| RQ-003 `docs/edf/validation/topology-findings.md:28-30` | "technical, organizational, informational, operational, environmental, and human" layers. Research question only |
| Examples in practice | `docs/edf/examples/apollo-11.md:15` adds "Systems"; Challenger uses "Technical / Organizational / Human" (`examples/challenger.md:15`); Pixar R1 groups "Organizational: Leadership philosophy, Psychological safety" (`validation/case-008R1-pixar.md:55-60`) |

The two versions disagree, the examples invent extra categories, and nothing says whether a category is a label for the **locus** (where the cause sits), the **mechanism** (how it works), or the **domain of expertise** needed to see it.

### 1.2 Overlaps and ambiguities

| Pair | Problem |
|---|---|
| Human vs Organizational | A launch manager accepting a risk (Challenger "Human: acceptance of launch despite unresolved concerns", `examples/challenger.md:15`) is an individual act inside an organizational role, norm and incentive. No rule says where to code it. This is the classic person-vs-system attribution problem, and how it is coded drives blame vs systemic remedies. |
| Organizational vs Regulatory | A regulator is an organization, and delegated certification (Boeing ODA) is both. Coding depends on where the System Context boundary was drawn, which is analyst-chosen. |
| Financial vs Organizational | Budget pressure, cost targets and compensation are financial in form but organizational in mechanism (incentives). v1 drops Financial without saying where these go. |
| Environmental | Undefined. Could mean physical environment (cold weather, Challenger), market/competitive environment, or the "Context" of System Context. External market conditions have no clear home. |
| Operational (v1) vs Human / Organizational | Undefined. Operations are performed by humans under organizational procedures. |
| Informational (v1) vs Informational propagation | The same word is both an origin type (v1) and a propagation type (v0.3), with no distinction between "information was wrong at its source" and "information failed to travel". |
| Organizational origin vs Organizational propagation | Same issue: "organizational" is both a cause category and a transmission path (`specification.md:100` and `:115`). |

### 1.3 Missing categories needed for organizational diagnosis

None of the following has a category, and several are absent from the repo entirely (repo-wide searches: "incentive" appears in 3 files, all incidental; "abstain/abstention", "Goodhart", "informal", "interview", "survey" in 0 files under `docs/edf`, `docs/framework-engineering`, `research/analysis`, `research/hypotheses`, `research/experiments`).

| Missing category | Why it matters for organizations | Current repo coverage |
|---|---|---|
| **Incentives and reward structures** | Behaviour is often a rational response to what is measured and rewarded; remedies that ignore incentives regress. | Only as an item inside the Operations/Governance lens (`docs/framework-engineering/validation-protocol.md:87-88`) and as a disclaimer in `principles.md:33`. Boeing R1 lens line "Market pressures, schedule pressure, incentives" (`case-003R1-boeing-737-max.md:21`). Not an origin type. |
| **Informal power, politics and authority structure** | Who can veto, who is listened to, and whose bad news is filtered. Distinct from formal governance. | Absent. Challenger "engineering authority" is treated as formal governance (`examples/challenger.md:18-19`). |
| **Goals and strategy conflict** | Multiple legitimate but incompatible objectives (speed vs quality vs cost) produce "dysfunction" that is actually unresolved priority. | Absent. |
| **Intentional tradeoffs / accepted risk** | Many organizational "problems" are deliberate choices with known costs. Diagnosing them as failures is a category error, and the remedy is a decision rather than a fix. | Absent. The model defines an origin as "a point where a contributing cause began" (`model.md:79`), which presumes a defect. |
| **Capability, skills and capacity** | Under-staffing, skill gaps, overload. | Only implicitly under Human. |
| **Culture and norms** | Normalization of deviance, psychological safety and blame culture recur in the cases. | Used ad hoc ("risk normalization", "testing culture", "Psychological safety") with no category or definition. |
| **Structure and interfaces / coordination** | Handoffs between teams; Conway effects. | The only trace is the topology label "Interface-centered propagation" (Mars Climate Orbiter, `topology-findings.md:20`), which has no case file. |
| **External market and competitive conditions** | Competitor moves, demand shocks, labour market. | Ambiguously "Environmental"; Boeing "Market pressures" only as lens prose. |
| **Measurement and metric validity** | Outcomes and manifestations in organizations are usually metrics (attrition, NPS, velocity). Metrics can be gamed, mis-specified or lagging. A diagnosis built on an invalid metric diagnoses the metric. | Absent. Manifestations are assumed valid if "seen, measured, reported" (`model.md:71`). |
| **Stakeholder credibility and interest** | Evidence in organizations is mostly testimony from interested parties, each with a stake in which diagnosis wins. | Absent (see §3). |
| **History and path dependence** | Prior reorganizations, legacy commitments, sunk costs. | Absent; FM-010 notes static-schema compression of evolving state (`research/analysis/non-human-experimental-failure-modes.md:16`). |
| **Legitimacy and trust** | Whether interventions will be accepted. | Absent. |

### 1.4 Reliability

- No definitions, no exclusivity rule (can an origin carry two types?), no coding manual, and no agreement study. HY-EDF-009 (layers/topologies generalize) is "suggestive, Low": "Patterns were derived from selected cases and have not been coded on a held-out set" (hypothesis registry).
- v1 calls the categories "helpful but not mandatory" (`specification-v1-draft.md:87`), which concedes they are not a measurement instrument.
- **Verdict:** usable as a brainstorming checklist; **not reliably applicable** as a classification.

---

## 2. Propagation types

### 2.1 As written

- v0.3: "Propagation paths may be: Physical, Informational, Organizational, Financial, Human" (`specification.md:111-117`; `model.md:101`).
- v1: definition only, with no type list: "how influence, failure, information, or constraints moved through the system ... Origin Network is structural. Propagation is behavioral." (`specification-v1-draft.md:89-105`).
- Experiment-level relation labels in A003 cases: `contributes-to` (30 of 39 candidate edges), `permits` (3), `amplifies` (2), `precedes` (2), `triggers` (1), `modifies` (1) (tally over `research/experiments/EX-EDF-2026-A003/cases/*.case.json`). These are an implicit edge taxonomy that does not appear in the spec. 77% of edges use the uninformative "contributes-to".

### 2.2 Critique

- **Medium-based, not mechanism-based.** The list says what carries the effect (physical, informational...) but not how: amplification, delay, feedback, accumulation, threshold or cascade, inhibition or masking, or enabling vs triggering. The "potential system behaviors" in `topology-findings.md:36-44` (Resilient, Adaptive, Degrading, Cascading, Networked, Fragile, Recovering) partly fill this gap but are explicitly not EDF.
- **Absence-propagation is missing.** In organizations, the key path is often a signal that failed to propagate (warnings filtered upward; Challenger "known warnings were normalized", `examples/challenger.md:16`). There is no type for blocked, filtered or distorted information.
- **No temporal dimension.** No ordering, lag or "evolving diagnostic state" (DI-002, `docs/framework-engineering/deferred-insights.md:13`). The Apollo 13 analysis found that the static schema compresses repeated state changes (FM-010).
- **Overlaps with origin types** (Organizational, Financial, Human appear in both lists).
- **Feedback loops** (reinforcing/balancing) are central to organizational dynamics but cannot be represented: the grammar is origins → manifestations, which is acyclic in practice.
- **Reliability:** no definitions and no agreement data. HY-EDF-007 (ON vs Propagation distinct) is "suggestive, Medium" from one author-assessed ablation (EV-EDF-002); CTR-007 says it was "evaluated once by the authoring system". **Not reliably applicable** as a classification.

---

## 3. Evidence types

### 3.1 As written

- v0.3: "Separate direct observation from inference. Evaluate the quality of evidence ... note meaningful gaps or disputes." (`specification.md:119-123`). This gives two implicit types: observation and inference.
- Model: "observed, measured, documented, or independently supported" (`model.md:109`); "distinguish evidence from interpretation" (`:111`).
- Experiment-level (A003): evidence items carry a **stage** (S1/S2/S3); truth files carry `requiredEvidence`, `allowedEvidence`, `redHerringEvidenceIds`, `conflictingEvidencePairs`, `missingInformation` (`research/experiments/EX-EDF-2026-A003/case-schema.json`). Answer states: supported / contradicted / unknown. The negative-knowledge construct distinguishes "observed absence, not-observed, and genuinely unknown" (`prompt-modules.json`, `construct.negative-knowledge`). This is the most developed evidence vocabulary in the repo, and **none of it is in the EDF spec**.
- Research-governance: claim states and confidence labels (`docs/framework-engineering/claim-and-confidence-policy.md:47-73`). These apply to claims about EDF, not to evidence inside an analysis.

### 3.2 Missing for organizational diagnosis

| Missing | Notes |
|---|---|
| **Source type** (document, system log, metric, interview, survey, observation, expert opinion) | No distinction between a log line and a manager's account. |
| **Source credibility, interest and independence** | No field for "who said this, what do they gain from this diagnosis, and is it corroborated by an independent source?". In organizations, the subject of diagnosis is usually also the main evidence source. |
| **Corroboration count / triangulation** | Not represented. |
| **Contemporaneous vs retrospective** | Hindsight and recall bias. Evidence Isolation (`constitution.md:112-168`) handles this only for research runs, not inside analyses. |
| **Causal strength of evidence** (intervention or experiment vs natural contrast vs correlation vs anecdote) | Absent. Organizational data is overwhelmingly observational. EDF uses causal language ("origin", "propagation", "made the outcome possible") without requiring causal-identification evidence; the only safeguard is the general "Separate evidence from inference". |
| **Evidence-request / missing-evidence plan** | Unknowns are listed (`specification.md:167`) but there is no structured "what evidence would discriminate, who can provide it, what it costs". EX-EDF-001's natural and neutral prompts ask for this ("state what evidence would resolve it"), but the EDF spec does not. |
| **Confidentiality / sensitivity** | Organizational evidence (HR, performance) has access constraints that limit what can be shown. Absent. |

**Reliability:** "observation vs inference" is a generic and teachable distinction, but there is no rubric and no data. **Partially applicable.**

---

## 4. Control-point criteria

### 4.1 As written

- v0.3 ranking criteria: Influence, Controllability, Cost, Confidence (`specification.md:131-140`).
- Policy: plus reversibility and timing; "no fixed weights" (`claim-and-confidence-policy.md:123-127`).
- v1 control-point attributes: Intervention, Responsible actor, System level, Expected influence, Confidence; "prioritized by expected leverage" (`specification-v1-draft.md:147-159`).
- Experiment-level action classes (A003): preferred, acceptable, reject, harmful, unknown (case `allowedAnswers`). The protective-controls construct distinguishes causal mechanism from detection/containment controls (`prompt-modules.json`, `construct.protective-controls`).

### 4.2 Critique

- **Overlap:** "Influence" and "Expected influence/leverage" are the same as the target quantity being ranked. "Confidence" appears in the ranking criteria and also as a separate output. "Controllability" overlaps with "Responsible actor" (v1).
- **Missing for organizations:**
  - **Political feasibility / sponsorship.** Can the owner actually act, and who will resist?
  - **Incentive side-effects.** Will the intervention shift behaviour elsewhere (Goodhart, gaming, displacement)? A003's "move the failure elsewhere" (`construct.control-points`) is the nearest idea, and it exists only in the experiment.
  - **Time to effect and observability of effect.** How the effect will be known (the operating cycle's "Verification" field, `operating-cycle.md:66-73`, is outside EDF).
  - **Reversibility.** In the policy but not the spec.
  - **Harm / protective-control removal risk.** Only in the A003 hypothesis A016.
  - **Distributional impact.** Who bears the cost.
- **Reliability:** no scale, no anchors, no aggregation, no agreement data. HY-EDF-004 is "suggestive, Low". **Not reliably applicable** beyond a qualitative checklist.

---

## 5. Confidence labels

### 5.1 As written

| Context | Labels | Source |
|---|---|---|
| EDF analysis | Low, Medium, High (qualitative; "not calibrated probabilities") | `model.md:117-127` |
| Research claims | Low, Low-Medium, Medium, Medium-High, High | `claim-and-confidence-policy.md:65-71` |
| Research claim states | internal-demonstration, suggestive, unsupported, contradicted, falsified, superseded, planned | `claim-and-confidence-policy.md:51-57` |
| Examples | free text, e.g. "High on technical origin and strong on governance significance" | `examples/challenger.md:20` |
| A003 experiment | integer 0-100 per item | `scripts/edf-002-analyzer.mjs:60` |

### 5.2 Critique

- Two incompatible ordinal scales (3-level vs 5-level) and one numeric scale. No mapping between them.
- There is no rule for what drives a label (e.g., number of independent corroborating sources, presence of discriminating evidence, live alternatives). `model.md:121` lists factors (evidence quality, consistency, competing explanations, unresolved uncertainty) with no combination rule.
- It is unclear whether confidence applies to each origin, each edge, each control point or the whole model. Templates have one Confidence section.
- No **abstention** label (e.g., "insufficient to diagnose").
- No calibration study exists. Where confidence is scored (A003), the rule is improper (see `implementation-audit.md` §2.3).
- **Reliability:** the labels are applicable as communication but are **not reliable or comparable** across analysts.

---

## 6. Diagnostic Sufficiency (as a binary category)

- Templates and examples use Yes / No ("Yes for replacement", `complexity-levels.md:57`; "Yes for the next responsible action", `examples/challenger.md:21`).
- Missing categories: **Insufficient: gather evidence X** (an evidence request), **Insufficient: abstain** (do not act on this diagnosis), **Sufficient only for a reversible probe**, and **Reframe** (wrong question or boundary). The removed site component offered "Act · Investigate · Reframe" (deleted `app/DiagnosticFlow.tsx`, commit `b27f5ff`); the spec never adopted it.

---

## 7. Complexity levels as a category

- EDF-0/1/2 selection criteria (`complexity-levels.md:9-15, 101-109`) mix complexity (interacting systems), consequence, and dispute. A low-complexity, high-consequence case has no clean level: "increase depth regardless of label" (`:109`) admits this.
- v1 introduces EDF-3 and progressive fields (`specification-v1-draft.md:233-313`), and assigns level selection to Clarity (`:237`).
- **Reliability:** no agreement data (HY-EDF-010 unsupported). Applicable as guidance.

---

## 8. Diagnostic topologies (research-only taxonomy)

### 8.1 As written (`docs/edf/validation/topology-findings.md:14-22, 36-44`)

| Case | Outcome type | Observed structure |
|---|---|---|
| Challenger | Failure | Integrated sociotechnical / cross-context breakdown |
| Apollo 13 | Failure and recovery | Cyclic adaptive recovery / evolving diagnostic state |
| Boeing 737 MAX | Organizational/technical failure | Networked and layered origin structure |
| Apollo 11 | Success | Layered resilience |
| Mars Climate Orbiter | Failure | Interface-centered propagation |
| Pixar | Sustained success | Creative learning system |
| Toyota Production System | Sustained success | Operational learning system |

Potential system behaviors: Resilient, Adaptive, Degrading, Cascading, Networked, Fragile, Recovering.
Status: "Diagnostic Topology is still not accepted into EDF. Layered Origin Networks are not accepted into EDF." (`:50-52`). Acceptance criteria at `:66-73`.

### 8.2 Critique

- **One label per case, derived from the case it labels.** Seven cases produce seven nearly unique topologies, so there is no recurrence to speak of. Pixar and Toyota "learning system" differ only by adjective.
- **Mixed axes.** Labels combine outcome valence (success/failure), structure (networked, layered), dynamics (cyclic, cascading) and domain (creative, operational). They are not mutually exclusive: Boeing is plausibly "cascading" and "fragile" and "networked".
- **Unverifiable entry:** Mars Climate Orbiter has no validation case file in the repo (it appears only in `deferred-insights.md` DI-005).
- **Selection bias:** famous, already-interpreted, extreme cases (CTR-006, FM-007).
- **Organizational relevance:** two of seven entries are organizational success stories (Pixar, Toyota) drawn from management folklore. The Pixar "Historical Baseline" is what Pixar's process is "publicly associated with" (`case-008R1-pixar.md:22-24`), with no cited sources. There are no organizational-dysfunction topologies (e.g., siloed coordination failure, metric gaming, leadership bottleneck, change fatigue, priority thrash).
- **Reliability:** HY-EDF-009 suggestive, Low; no held-out coding. **Cannot be applied reliably.**

---

## 9. Analytical lenses (Diagnostic Calibration)

Systems, Operations/Governance, Independent Investigation, Differential Diagnosis, Legal/Evidentiary (`docs/edf/diagnostic-calibration.md:37-43`; lens content at `docs/framework-engineering/validation-protocol.md:68-133`).

- These are perspectives, not categories. They overlap heavily (Independent Investigation, Differential Diagnosis and Legal/Evidentiary all cover evidence and competing hypotheses).
- Organizational perspectives are missing: people/HR, finance, customer/market, and frontline-worker perspectives. Operations/Governance is the only lens mentioning incentives (`validation-protocol.md:88`).
- "A lens should be retained only if controlled work shows that it contributes distinctive, evidence-supported information" (`diagnostic-calibration.md:47`). No such work exists.

---

## 10. Summary: applicability matrix

| Scheme | Defined? | Mutually exclusive? | Reliability data? | Org-diagnosis coverage | Verdict |
|---|---|---|---|---|---|
| Origin types | Names only; two inconsistent versions | No | None | Poor: lacks incentives, power, goals/tradeoffs, culture, metrics, market | Checklist only |
| Propagation types | Names only | No; overlaps origins | None | Poor: no feedback, blocked signals, delays | Checklist only |
| Evidence types | Observation vs inference only | n/a | None | Very poor: no source credibility, interest, or causal strength | Partially applicable |
| Control-point criteria | Names only; 3 inconsistent lists | No | None | Poor: no feasibility, incentive side-effects, ownership politics | Checklist only |
| Confidence labels | Qualitative; 3 scales | Ordinal | None | Neutral; no abstain | Communication only |
| Diagnostic Sufficiency | Concept defined; outcome binary | Yes/No | None | Missing evidence-request and abstain outcomes | Needs extension |
| Complexity levels | Defined by triggers | Mixed axes | None | Neutral | Guidance only |
| Topologies | Single-case labels | No | None | Very poor | Not applicable |
| Lenses | Bullet lists | Overlapping | None | Partial (Governance lens) | Untested |
