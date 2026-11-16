# Methodological Opportunities for EDF (OPP-01 … OPP-20)

Mission EDF-IMPROVEMENT-2026-001 · Baseline EDF v0.3 (`41fea6f`) · 2026-11-16
Derived from `literature-review.md`, the eight workstream reviews, `competing-methodologies.md` and the baseline audit (`../baseline/`). Source ids refer to `../evidence/source-register.csv`. Workstream proposal codes (C1, D4, E6, F1, G2, I1, L1, O3 and so on) point to the originating review.

These are **candidates, not recommendations**. Every one is a hypothesis. The baseline audit found that the whole EDF prompt did not beat a matched-neutral structure (`../baseline/current-limitations.md` rank 1), so each candidate must be ablated against a matched-neutral control before adoption. For measured results, see experiments/ and strategy/ (EX-EDF-2026-OD1).

**Evidence strength key.** Strong, Moderate, Weak, Theoretical or Negative, applied to the *underlying mechanism*, not to its transfer to EDF. "[SR]" means the key detail rests on search-result-only sources.

**Cost/complexity key.** Burden on the analyst. L = one field or line. M = a structured sub-step. H = new data collection or a new artefact.

**OD1 references.** Benchmark classes and tags come from `../benchmarks/benchmark-specification.md`. As named by the dev cases, class D is conflicting stakeholder explanations, class E is an incentive pilot with a proposed rollout, class I is green dashboards alongside missed dates, and class K is a non-existent problem or an intentional tradeoff.

## Summary

| ID | Opportunity | Main failure mode | Evidence (mechanism) | Cost |
|---|---|---|---|---|
| OPP-01 | Evidence provenance plus source-independence rule | Over-reliance on interested, single-source testimony | Strong / Moderate | M |
| OPP-02 | Commissioning-stake declaration and premise check | Adopting the client's framing | Moderate [SR] | L |
| OPP-03 | Minimal differential with discriminating evidence | Premature closure | Moderate; Negative for heavy versions | M |
| OPP-04 | Missingness and reporting-climate rule | Absence of reports read as absence of problem | Weak–Moderate | L |
| OPP-05 | Metric-integrity check | Gamed or invalid metrics used as evidence | Theoretical + cases | L–M |
| OPP-06 | Organizational origin prompts, including incentive, informational and intentional-tradeoff | Coverage gaps; tradeoffs misdiagnosed as defects | Moderate by analogy | L–M |
| OPP-07 | Typed causal edges (causal vs association-only) | Causal overclaim | Theoretical (formal) | M |
| OPP-08 | Control-structure and feedback-channel fields | Missed information-flow origins; control points that depend on filtered channels | Moderate (2 studies) | M |
| OPP-09 | Loop and system-response check | Acyclic models; policy resistance | Moderate / Theoretical | M |
| OPP-10 | Ranking reform | Invalid aggregation; weak controls; harm ignored | Strong / Moderate | M |
| OPP-11 | Sufficiency as threshold/VOI with permitted outcomes | No abstention; action bias | Theoretical | M |
| OPP-12 | Evidence requests with expected decision impact and stewardship | Unfocused or excessive data gathering | Moderate / Weak | M |
| OPP-13 | EDF-0 as two-stage triage | Uniform depth regardless of need | Moderate | M |
| OPP-14 | Resolvable predictions with a proper scoring rule | Unscorable, uncalibrated confidence | Moderate | M |
| OPP-15 | Forward chain, monitoring plan, implementation-before-effectiveness | Fidelity failure misread as wrong diagnosis | Moderate (frameworks) | M–H |
| OPP-16 | Reliability and validity measurement programme | No evidence EDF works | Strong (as a protocol) | H |
| OPP-17 | AI-run EDF protocol | LLM premature closure, sycophancy, judge bias | Moderate [SR] | M |
| OPP-18 | Work-as-done sampling plan and workaround/silence probes | Hidden frontline problems | Moderate | M–H |
| OPP-19 | Local-rationality record and temporal baseline | Hindsight blame; normalized deviance missed | Moderate / Weak | L–M |
| OPP-20 | Proportionality gate for continuous assessment | Surveillance harms; concealment | Moderate + regulatory | M |

---

### OPP-01: Evidence provenance and source-independence rule

- **Failure mode.** Evidence from interested parties is treated as observation. A diagnosis built on executive interviews is single-source, single-method and interested. EDF Step 6 only says "separate direct observation from inference" (`../baseline/methodology-inventory.md` §9; `../baseline/diagnostic-taxonomy.md` §3.2).
- **Mechanism.**
  - Each evidence item records: `source_role`, `method`, `stake` (yes / no / unknown), `independence_group` and `time_relative_to_outcome`.
  - A claim (origin, edge or control-point influence) may not exceed *Moderate* confidence unless at least two independence groups that differ in method or source role support it.
  - Post-outcome accounts from interested parties count as one group. Leadership accounts stay admissible as *claims about the system* (C1, C2, E1).
- **Evidence.**
  - Common-method bias: Strong (SRC-CDE-006).
  - Informant disagreement and triangulation guidance: Moderate [SR] (SRC-CDE-007, SRC-CDE-008).
  - Status-skewed perception: Weak–Moderate (SRC-CDE-009 fetched, SRC-CDE-010).
  - Measurement source can flip causal conclusions: Moderate [SR] (SRC-CDE-046, SRC-CDE-047).
- **Applicability.** All organization types. Most valuable where the commissioning party is a likely origin. Lower value where system records are rich and uncontested, as in some regulated industries.
- **Cost.** M. Fields per evidence item. At EDF-0, a single "who said it and do they have a stake" tag suffices.
- **Risks.** Over-skepticism toward correct leadership accounts, which can invent hidden problems. Paperwork burden. Independence groups can be gamed by splitting sources.
- **How to test.** OD1 classes D and I. Primary metric: leadership-narrative adoption. Guardrail: false-diagnosis rate on class K and on items where `leadership-narrative` is expected `supported`. Lit hypotheses H-C1 and H-E1, with H-E8 as the guardrail.

### OPP-02: Commissioning-stake declaration and premise check

- **Failure mode.** The presenting request carries the client's causal theory, for example "the problem is in sales". LLMs are sycophantic and accept false premises. The System Context boundary is set by the analyst and is political (`../baseline/current-limitations.md` rank 21).
- **Mechanism.** Step 1 gains three things (E2, I1):
  - a "Commissioning and stakes" line: who asked, which outcome favours them, which candidate origins implicate them;
  - a list of the causal assumptions embedded in the request;
  - at least one origin hypothesis kept outside the requester's framing.

  For AI use, the request is restated neutrally before analysis.
- **Evidence.** Moderate [SR]: sycophancy and false-premise acceptance (SRC-FGI-034, SRC-FGI-052, SRC-FGI-054, SRC-FGI-055). Self-interest check (SRC-CDE-024, practitioner). Political hijack of RCA (SRC-CDE-036).
- **Applicability.** Universal. Strongest in consulting or internal-audit engagements with a sponsor.
- **Cost.** L.
- **Risks.** Performative compliance: the analyst lists premises and ignores them. Sponsors may object.
- **How to test.** OD1 classes D, I and K, where each presenting request names a locus or a crisis. Metrics: leadership-narrative adoption, false-diagnosis rate, and `nonexistent-problem` tag accuracy. Lit hypotheses H-I2 and H-E4.

### OPP-03: Minimal differential with discriminating evidence

- **Failure mode.** Premature closure in Steps 4 and 9. EDF has no rule to carry rivals (`methodology-inventory.md` §13).
- **Mechanism.**
  - Carry two or more live rival explanations, or rival key edges, until a discriminating item is found for each.
  - Use an IS / IS-NOT specification of the outcome, and positive-deviance comparison units where they exist.
  - Label evidence with process-tracing test types: hoop, smoking-gun, doubly-decisive (C5, D2, O2, O3).
  - Explicitly **not** a full ACH matrix.
- **Evidence.**
  - Premature closure is the leading cognitive error: Moderate (SRC-BH-070, fetched).
  - Generic strategy plus structured search: Moderate (SRC-BH-048).
  - Process-tracing logic: Theoretical (SRC-CDE-029, SRC-CDE-030).
  - **Negative** for heavy matrices (SRC-BH-072, RCT).
- **Applicability.** All levels. IS / IS-NOT needs comparison units (sites, teams, periods), so it is weaker for one-off whole-organization outcomes.
- **Cost.** M.
- **Risks.** Added inconsistency, as with ACH. Mandatory rivals can produce straw alternatives.
- **How to test.** OD1 all classes, especially D. Metrics: false-diagnosis rate and item-level test–retest agreement (consistency guardrail). Lit hypotheses H-C4 and H-D3.

### OPP-04: Missingness and reporting-climate rule

- **Failure mode.** "No complaints" or "low incident count" is read as health. EDF Unknowns is free text (`../baseline/diagnostic-taxonomy.md` §3.2).
- **Mechanism.**
  - Tag each important unknown as `not collected`, `unavailable` or `plausibly suppressed`.
  - Low report counts may be used as evidence of absence only alongside an independent reporting-climate indicator: voice or psychological-safety data, observed workaround prevalence, or anonymous versus attributed channel comparison (C4, E7).
- **Evidence.** Weak–Moderate: better-led units report more (SRC-CDE-041, small n). Missing-not-at-random theory (SRC-CDE-012, formal). Psychological safety associates with voice (SRC-CDE-055, SRC-CDE-056, correlational).
- **Applicability.** High in safety-critical, healthcare, financial-control and hierarchical cultures. Lower where telemetry captures the phenomenon directly.
- **Cost.** L.
- **Risks.** Unfalsifiable suspicion ("they must be hiding something").
- **How to test.** OD1 tag `organizational-silence` (classes I, K and others) and `hidden-problem`. Metrics: missed-problem rate, and the false-diagnosis rate in class K. Lit hypotheses H-C3 and H-E5.

### OPP-05: Metric-integrity check

- **Failure mode.** KPIs used as manifestations or evidence are themselves targets that can be gamed, lagging or mis-specified (`current-limitations.md` rank 12).
- **Mechanism.** For each metric cited, ask:
  - Is it a target, and who is evaluated on it?
  - When was its link to the underlying goal last checked?
  - Which Goodhart variant is suspected: regressional, extremal, causal or adversarial?
  - Is there a non-target corroborating indicator?

  (E6, L5.)
- **Evidence.** Theoretical plus many cases (SRC-CDE-062, SRC-CDE-063, SRC-FGI-073, SRC-CDE-064). The Goodhart taxonomy was fetched at abstract level only.
- **Applicability.** Universal wherever performance management exists. Especially sales, contact centres, software delivery and public-sector targets.
- **Cost.** L–M.
- **Risks.** Blanket distrust of all metrics. Some metrics are robust.
- **How to test.** OD1 classes E and I. Tags `metric-gaming` and `measurement-artifact`. Metric: tag-specific accuracy. Lit hypothesis H-E3.

### OPP-06: Organizational origin prompts

- **Failure mode.**
  - The v0.3 origin types (Technical, Human, Organizational, Environmental, Financial, Regulatory) omit incentives, informal power, goal conflict, intentional tradeoffs, capability, culture, interfaces, market conditions and metric validity.
  - The origin definition presumes a defect (`../baseline/diagnostic-taxonomy.md` §1.3).
  - Investigators find what their model looks for.
- **Mechanism.**
  - Add optional coverage prompts drawn from the content models: strategy, structure, processes, rewards, people and capacity, leadership, culture, formal-versus-informal gaps, external environment.
  - Add a *goal conflict / intentional tradeoff* category, whose remedy is a decision, not a fix.
  - Before closing Step 4, list the categories not examined (O6, D7, E9).
- **Evidence.** Moderate by analogy: domain model plus strategy (SRC-BH-048) and taxonomies recommended for systemic analysis (SRC-BH-065). WYLFIWYF (SRC-CDE-043, [SR]). The specific taxonomies are unvalidated (SRC-BH-003, SRC-BH-007), and taxonomy coding can itself be unreliable (SRC-BH-069).
- **Applicability.** High for organizational EDF. Keep the prompts as coverage aids, not causal claims.
- **Cost.** L–M.
- **Risks.** Checklist bloat. Category-forcing ("mandatory multi-cause storytelling", which `principles.md` names as an anti-pattern).
- **How to test.** OD1 class K and tag `intentional-tradeoff` (an intentional choice must not be diagnosed as dysfunction). Tag `local-optimization`. Missed-problem rate.

### OPP-07: Typed causal edges

- **Failure mode.** EDF uses causal language without an identification standard (`current-limitations.md` rank 4). In A003, 77% of edges used "contributes-to" (`diagnostic-taxonomy.md` §2.1).
- **Mechanism.** Each edge records:
  - mechanism (one sentence);
  - counterfactual;
  - edge type (enabling, triggering, amplifying, failed-defence);
  - evidence ids;
  - test type;
  - plausible confounders;
  - status: causal, association-only, contradicted or unknown (D1).
- **Evidence.** Theoretical (formal): SCM explicitness and equivalence classes (SRC-CDE-027, SRC-CDE-032). Holland's distinction (SRC-CDE-028).
- **Applicability.** EDF-1 and EDF-2. At EDF-0, only the causal versus association-only flag.
- **Cost.** M.
- **Risks.** Formality without accuracy. LLMs may fill fields fluently but wrongly (SRC-FGI-038).
- **How to test.** OD1 edge items. Metrics: causal overclaim rate and causal accuracy. Tags `causal-overclaim-trap` and `confounded`. Lit hypotheses H-D1 and H-D2.

### OPP-08: Control-structure and feedback-channel fields

- **Failure mode.** Origins in information flow are missed: filtered escalation, or dashboards that hide queue age. Control points are proposed that depend on the very channel that is silent.
- **Mechanism.** Each control point gets `controller`, `control_action`, `feedback_channel` (what the controller sees, and with what delay), `process_model` (what the controller believed) and `constraint_enforced`. If the feedback is filtered, pair the control point with a feedback fix or downgrade its controllability (D4, E5, E11, O7).
- **Evidence.** Moderate: STAMP-CAST was more reliable than AcciMap in two studies [SR] (SRC-CDE-050, SRC-CDE-051). Its validity was still moderate (SRC-BH-067). It bridges to the silence literature (SRC-CDE-053, SRC-CDE-057).
- **Applicability.** Governance-heavy settings, multi-division firms and regulated industries.
- **Cost.** M.
- **Risks.** Engineering vocabulary can alienate organizational users.
- **How to test.** OD1 class I (green dashboards). Tags `organizational-silence` and `hidden-problem`. Lit hypothesis H-D5.

### OPP-09: Loop and system-response check

- **Failure mode.** EDF propagation is acyclic, and Step 8 ignores how the system will respond (`diagnostic-taxonomy.md` §2.2).
- **Mechanism.** Ask which loop maintains the outcome. For each top control point, ask what compensating behaviour, displacement or gaming it will trigger. Flag expected worse-before-better (D5, O5).
- **Evidence.** Moderate: experiments on misperception of feedback (SRC-CDE-034, [SR]). Theoretical: policy resistance and drift (SRC-CDE-035, SRC-CDE-048). Field cases plus model: capability trap (SRC-BH-022).
- **Applicability.** Chronic problems such as on-call exhaustion, firefighting and attrition spirals. Less relevant for one-off events.
- **Cost.** M.
- **Risks.** Speculative loops without evidence.
- **How to test.** OD1 tags `feedback-loop`, `iatrogenic-intervention` and `side-effect`. Harmful-action promotion. Lit hypothesis H-D6.

### OPP-10: Ranking reform

- **Failure mode.**
  - Four unanchored criteria with no aggregation rule, and three inconsistent lists.
  - Influence is undirected, with no harm dimension.
  - Controllability conflates authority with implementability.
  - Influence rests on retrospective evidence (`diagnostic-taxonomy.md` §4.2).
- **Mechanism.**
  - (a) Criteria are ordinal. Never multiply or sum them; use dominance or Pareto ordering with stated tradeoffs.
  - (b) Rank control strength: structural or design > feedback or information > incentive > procedure > training. Training-only controls are penalised unless a real knowledge gap is shown.
  - (c) Add a Harm / backfire field.
  - (d) Split Controllability into authority and implementability.
  - (e) Record `influence_basis`: prior intervention in this system, comparable-system evidence, or reasoning only.
  - (f) Require finding-to-fix traceability.

  (O4, D3, D6, D9, F1, F2.)
- **Evidence.**
  - Ordinal aggregation is invalid: Strong (SRC-BH-031).
  - Weak controls dominate RCA and events recur: Moderate (SRC-CDE-037).
  - Action hierarchy: practitioner consensus (SRC-BH-028, SRC-BH-029).
  - Backfire in more than a third of feedback interventions: Strong [SR] (SRC-FGI-011).
  - CFIR determinants: frameworks (SRC-FGI-006).
- **Applicability.** Universal.
- **Cost.** M.
- **Risks.** Over-penalising training where skills really are the gap. Harm estimates are themselves speculative.
- **How to test.** OD1 action items. Metrics: harmful-action promotion and premature-action promotion. Tags `protective-control` and `iatrogenic-intervention`, especially class E (rolling out a gamed bonus). Lit hypotheses H-D4, H-D7, H-D9 and H-F1.

### OPP-11: Diagnostic Sufficiency as a threshold/VOI test with permitted outcomes

- **Failure mode.** Sufficiency is a binary Yes/No with no abstention or evidence-request outcome (`current-limitations.md` rank 6). It relies on self-rated confidence, which degrades on hard cases.
- **Mechanism.** Ask:
  - (1) What would we do now?
  - (2) Which plausible result would change that, and how likely is it?
  - (3) Is finding out worth the cost, including delay, disruption and trust?
  - (4) Is the action robust across the live rivals?
  - (5) Is the action reversible? Reversibility sets the bar.

  Permitted outcomes: intervene at X; run a reversible probe; gather evidence E; monitor with a trigger; no action warranted. Each of the last two must name a re-review trigger (F4, F5, F6, C7, G5, O9).
- **Evidence.** Theoretical (decision analysis): SRC-FGI-019, SRC-FGI-020, SRC-FGI-014, SRC-FGI-013. Moderate [SR]: confidence insensitivity (SRC-CDE-014). Action bias outside organizations (SRC-FGI-017). Probe-sense-respond is theoretical (SRC-BH-049).
- **Applicability.** Universal. Matters most in high-consequence, irreversible decisions such as restructurings and layoffs.
- **Cost.** M.
- **Risks.** Analysis paralysis. "Insufficient" used as a safe default.
- **How to test.** OD1 SUFF item (abstention accuracy), premature-action promotion, and class K (correct answer: no substantive intervention). Guardrail: missed-problem rate on true-problem classes. Lit hypotheses H-F3 and H-C6.

### OPP-12: Evidence requests with expected decision impact and stewardship

- **Failure mode.** Unknowns are not turned into a ranked, costed request plan (`current-limitations.md` rank 7). AI analysts over-request or ask badly.
- **Mechanism.** Before gathering evidence, name the rivals and choose evidence that would shift the top control point. Record the expected decision impact and the cost of each request (time, disruption, trust). Decline low-yield, high-cost requests with a reason (G2, G3).
- **Evidence.** VOI versus information gain: Theoretical (SRC-FGI-020, SRC-FGI-021, SRC-FGI-022). Weak, single preprint: stewardship role in SDBench (SRC-FGI-031). Weak–Moderate [SR]: naive asking hurts (SRC-FGI-033).
- **Applicability.** Universal. Most valuable where data access is costly or politically sensitive.
- **Cost.** M.
- **Risks.** Expected-impact estimates are guesses. Under-collection.
- **How to test.** OD1 evidence-request value score: discriminating versus low-value requests. Lit hypotheses H-G1 and H-G3.

### OPP-13: EDF-0 as a two-stage triage

- **Failure mode.** The same nine steps run at every level, and depth labels do not select content (`methodology-inventory.md` §14).
- **Mechanism.** A fixed short item set whose output chooses which EDF-1/2 modules to run, for example stakeholder module, metric module or dynamics module (G1).
- **Evidence.** Moderate in psychometrics: two-stage branching nearly matches CAT, and CAT is only marginally better than good static forms (SRC-FGI-024, SRC-FGI-025). Only the direction transfers; organizational diagnosis lacks calibrated items.
- **Applicability.** High for routine internal use. Less so for one-off strategic diagnoses.
- **Cost.** M to design, then low per use.
- **Risks.** Triage misroutes. Missing a module is a silent failure.
- **How to test.** OD1 B0 versus B comparison as a non-inferiority probe of short forms. Lit hypothesis H-G2. Open-generation arm for missed problems.

### OPP-14: Resolvable predictions with a proper scoring rule

- **Failure mode.** Confidence is a three-level label, uncalibrated and unscorable. A003 used a linear, improper rule (`methodology-inventory.md` §12).
- **Mechanism.** Each high-ranked origin carries at least one prediction that the next action will check, with a probability. Score with a Brier or log rule and recalibrate from track records (C6). For LLMs, use agreement across independent runs, not verbalized confidence (I3).
- **Evidence.** Moderate: calibration is scorable, trainable and recalibratable (SRC-CDE-016, SRC-CDE-017, SRC-CDE-018). Moderate [SR]: LLM verbalized overconfidence (SRC-FGI-040, SRC-FGI-041).
- **Applicability.** Repeated users and portfolios of diagnoses, where enough predictions resolve to calibrate.
- **Cost.** M.
- **Risks.** False precision, which `principles.md` names as an anti-pattern. Too few resolutions per organization.
- **How to test.** OD1 calibration component and Brier secondary metric. Lit hypothesis H-C5. In real organizations, see `../intervention-effectiveness/evaluation-design.md`.

### OPP-15: Forward chain, monitoring plan, implementation before effectiveness

- **Failure mode.** EDF ends at the next action. The validation loop cannot tell a fidelity failure from a wrong diagnosis, and it ignores regression to the mean.
- **Mechanism.**
  - For each top control point, write a mini theory of change: control point, then propagation change, then manifestation change, then outcome, with assumptions named.
  - Give it a leading indicator, a non-target indicator, a review date and a comparison (trend, comparison unit or threshold).
  - Check adoption and fidelity before judging effectiveness.

  (F3, F7, O10, L1.)
- **Evidence.** Moderate (frameworks with wide use): SRC-FGI-009, SRC-FGI-010, SRC-FGI-005, SRC-FGI-007. Validity threats (SRC-FGI-015). Weak RCA follow-through (SRC-BH-026).
- **Applicability.** Universal where EDF hands off to execution, for example Clarity.
- **Cost.** M–H.
- **Risks.** Bureaucratic overhead for EDF-0.
- **How to test.** Not testable in OD1 (synthetic, no execution). Requires the field protocol in `../intervention-effectiveness/evaluation-design.md`. Lit hypotheses H-F4 and H-F5.

### OPP-16: Reliability and validity measurement programme

- **Failure mode.** There is no human-rater data, no inter-analyst reliability and no criterion validity (`current-limitations.md` ranks 2, 3 and 13).
- **Mechanism.** Use the accident-analysis protocol:
  - independent analysts per case;
  - origin-set overlap and Krippendorff's alpha on edges and top-3 control points;
  - signal-detection scoring against a referent, with phase fragmentation;
  - a baseline method;
  - pre-registration and publication of nulls.

  (O1, C10.)
- **Evidence.** Strong as a measurement method (SRC-BH-066, SRC-BH-067, SRC-CDE-004, SRC-CDE-005). The 38–65% reliability baseline is [SR] and comes from one accident.
- **Applicability.** Programme-level, not per analysis.
- **Cost.** H.
- **Risks.** Synthetic referents encode author bias. LLM analysts are not independent of LLM authors (OD1 spec §7).
- **How to test.** OD1 provides item-level test–retest only. Human inter-analyst studies remain to be run.

### OPP-17: AI-run EDF protocol

- **Failure mode.** LLM premature closure in multi-turn settings, order sensitivity, sycophancy, unfaithful reasoning, and judge biases.
- **Mechanism.**
  - Functional roles (hypothesis, discriminating-evidence, challenger, stewardship) over a shared evidence ledger.
  - A minimum evidence set before committing.
  - Consolidated restatement of the evidence.
  - An order-perturbation re-test.
  - Claim-level citations.
  - Independent runs against a cost-matched baseline.
  - Cross-family, counterbalanced, length-normalized judging.
  - Human check of the top causal claims against raw evidence.

  (I2–I7.)
- **Evidence.** Moderate [SR], mostly from medicine and general reasoning: SRC-FGI-031, SRC-FGI-047, SRC-FGI-045, SRC-FGI-038, SRC-FGI-035, SRC-FGI-036, SRC-FGI-037, SRC-FGI-057, SRC-FGI-058, SRC-FGI-048.
- **Applicability.** Any AI-assisted use.
- **Cost.** M; compute rises N-fold with independent runs.
- **Risks.** Role prompts may add tokens without accuracy (SRC-FGI-039, SRC-FGI-043).
- **How to test.** OD1 test–retest agreement, the evidence-citation F1 component, and the open-generation arm with grader agreement. Lit hypotheses H-I3 to H-I6.

### OPP-18: Work-as-done sampling plan and workaround/silence probes

- **Failure mode.** The diagnosis rests on management interviews. Frontline workarounds and withheld issues never enter the record.
- **Mechanism.**
  - Before Step 4, state which levels and methods will be sampled, and whether frontline sources are protected.
  - Ask the required prompts: "What do people do to get work done despite the system?", "What is fixed locally and never reported?", "What would people not tell their manager?"
  - Record workarounds both as manifestations and as evidence of origins. Optionally use process mining or conformance checking, or ONA, as evidence sources (E3, E4; SRC-BH-034, SRC-BH-036).
- **Evidence.** Moderate: direct observation (SRC-CDE-057, fetched). Silence research [SR] (SRC-CDE-053, SRC-CDE-054, SRC-CDE-060). Workaround theory (SRC-CDE-069, theoretical).
- **Applicability.** High in operations-heavy settings: healthcare, contact centres, retail, logistics. Requires access and confidentiality guarantees.
- **Cost.** M–H (new data collection).
- **Risks.** Burden and privacy. Protected sources may still self-censor (SRC-FGI-074, SRC-FGI-067).
- **How to test.** OD1 can only test whether analyzers *use* frontline and anonymous-survey evidence already in the packet (tags `staff-narrative` and `hidden-problem`, plus the evidence-request value score). Real sampling needs field evaluation. Lit hypothesis H-E2.

### OPP-19: Local-rationality record and temporal baseline

- **Failure mode.** Hindsight-laden blame ("should have known"). Normalized deviance is invisible because it has become normal.
- **Mechanism.** For each human or organizational origin, record what was known, the goals and pressures at the time, and the signals available before the outcome. Ban "should have known" without a pre-outcome citation. Compare current practice with the original standard and log when each deviation became accepted (D8, E8, C9).
- **Evidence.** Hindsight bias as a phenomenon: Moderate, with mixed specific replications (SRC-CDE-025). Second-story analysis: theoretical (SRC-CDE-045, SRC-CDE-040). Normalization of deviance: Weak (case-based) (SRC-CDE-061, SRC-CDE-048).
- **Applicability.** Incident-like outcomes and gradual-drift problems.
- **Cost.** L–M.
- **Risks.** Excusing real accountability failures.
- **How to test.** OD1 class H staged items (tag `revision`) partially. Lit hypotheses H-D8 and H-E6.

### OPP-20: Proportionality gate for continuous assessment

- **Failure mode.** Continuous monitoring is added to "keep diagnosing". This causes stress and concealment, and has legal exposure.
- **Mechanism.** Before any recurring data collection, check:
  - purpose tied to a prior EDF control point;
  - aggregate unit of analysis with minimum group sizes;
  - least-intrusive signal;
  - response owner;
  - at least one non-target indicator;
  - invariance or anchor checks;
  - trigger false-alarm estimate;
  - employee participation and a DPIA where required;
  - sunset date.

  No individual or emotion scoring (L1–L5).
- **Evidence.** Moderate [SR]: EPM meta-analysis (SRC-FGI-060), transparency paradox (SRC-FGI-067), early-warning false alarms (SRC-FGI-069). Regulatory, from secondary texts (SRC-FGI-062 to SRC-FGI-066).
- **Applicability.** EU and UK settings bind most tightly. Elsewhere it is ethically advisable. Unions and works councils may hold co-determination rights; check local law.
- **Cost.** M.
- **Risks.** Legal content may be out of date. Verify against primary texts.
- **How to test.** Not testable in OD1. See `longitudinal-assessment.md` and lit hypotheses H-L1 to H-L5.

---

## Sequencing note (evidence-led, not a recommendation)

- **Strongest and cheapest mechanisms:** OPP-10(a) (no arithmetic aggregation), OPP-02, OPP-04 and OPP-05. Each is one line or field with direct literature support.
- **Highest expected value for organizational use, given the dynamics literature:** OPP-01, OPP-11 and OPP-18.
- **Most exposed to "structure that hurts":** OPP-03 and OPP-17, because of the ACH and CoT evidence. Ablate these individually.
- **Testable only outside synthetic benchmarks:** OPP-15, OPP-16 (human arm), OPP-18 (sampling arm) and OPP-20.
