# EDF Methodology Inventory (Baseline Audit)

Audit agent: Agent A (methodology and baseline audit)
Baseline commit: `41fea6ffc764adc69643d5ee2940916592486f84` (see `BASELINE_COMMIT`; frozen hashes in `baseline-freeze.sha256`, verified unchanged with `sha256sum -c` during this audit)
Audit date: 2026-11-16
Scope: read-only audit of `docs/edf/*`, `docs/edf/validation/*`, `docs/framework-engineering/*`, `research/*`, `scripts/*`, `tests/*`, and git history (113 commits).

Conventions:

- `file:line` citations refer to the baseline commit.
- "Value evidence" uses the repository's own claim states from `research/registries/hypothesis-registry.json` (HY-*) and `research/registries/evidence-registry.json` (EV-*). The claim states are defined in `docs/framework-engineering/claim-and-confidence-policy.md:47-59`.
- "Generic vs organization-specific": **Generic** = domain-neutral diagnostic concept usable by anyone. **Echelon-specific** = depends on the Echelon tool/framework ecosystem (Clarity, Framework Engineering, Praxis/ROS). Separately, each entry notes **fitness for organizational diagnosis** (diagnosing an organization's own dysfunction), which is the intended future use and is not what EDF was validated on.

## 0. Summary table

| # | Component | Defined at | Value evidence (repo's own state) | Generic / Echelon-specific |
|---|---|---|---|---|
| 1 | 9-step Required Diagnostic Sequence | `docs/edf/specification.md:39-54` | HY-EDF-001 internal-demonstration only; HY-EDF-008 unsupported (EV-EDF-009: whole EDF prompt +0.56 vs matched-neutral, below +5 margin) | Generic |
| 2 | Required Output Elements (15) | `docs/edf/specification.md:150-168` | None beyond structural conformity (HY-EDF-001) | Generic |
| 3 | Five verbs / design rule | `docs/edf/specification.md:23-33`; `docs/edf/principles.md:51-61` | None; self-declared "design filter, not an empirical proof rule" (`principles.md:61`) | Generic (framework-governance device) |
| 4 | Context Principle / System Context | `docs/edf/specification.md:35-37, 58-70`; `docs/edf/model.md:11-55` | HY-EDF-002 suggestive, Low-Medium; no matched-heading ablation | Generic |
| 5 | Outcome | `specification.md:72-84`; `model.md:57-65` | None isolated | Generic |
| 6 | Manifestations | `specification.md:86-90`; `model.md:67-75` | None isolated | Generic |
| 7 | Origin Network + origin types | `specification.md:92-105`; `model.md:77-95` | HY-EDF-007 suggestive, Medium (with Propagation); HY-EDF-009 (layers/topology) suggestive, Low | Generic |
| 8 | Propagation + propagation types | `specification.md:107-117`; `model.md:97-105` | HY-EDF-007 suggestive, Medium (EV-EDF-002: one ablation); HY-EDF-2026-A013 pending | Generic |
| 9 | Evidence evaluation | `specification.md:119-123`; `model.md:107-115` | None; HY-EDF-2026-A011/A014 pending (no runs) | Generic |
| 10 | Control Points | `specification.md:125-129`; `model.md:129-137` | HY-EDF-004 suggestive, Low; HY-EDF-2026-A012 pending | Generic |
| 11 | Control-point ranking criteria | `specification.md:131-140` | None; policy says "heuristics, not a validated scoring system" (`claim-and-confidence-policy.md:123-127`) | Generic |
| 12 | Confidence handling | `model.md:117-127`; `claim-and-confidence-policy.md:61-73` | None; labels explicitly not calibrated | Generic |
| 13 | Diagnostic Sufficiency | `specification.md:142-148`; `model.md:139-145`; `claim-and-confidence-policy.md:115-121` | None; no hypothesis registered for it | Generic |
| 14 | Complexity levels EDF-0/1/2 | `docs/edf/complexity-levels.md:9-115` | HY-EDF-010 unsupported | Generic |
| 15 | Diagnostic Calibration (optional) | `docs/edf/diagnostic-calibration.md:1-107`; `specification.md:170-184` | None; HY-EDF-003/HY-EDF-005 unsupported; doc's own evidence boundary at `diagnostic-calibration.md:15-21` | Generic protocol, Echelon-specific lens set |
| 16 | Constitution (12 articles) | `docs/edf/constitution.md:1-168` | Not an empirical claim; governance | Echelon-specific (governance) |
| 17 | Clarity relationship | `specification.md:194-200`; `operating-cycle.md:13-21, 77-89`; `specification-v1-draft.md:212-223, 237` | None; Clarity not present in repo | Echelon-specific |
| 18 | v1.0 draft deltas | `docs/edf/specification-v1-draft.md:1-395` | None; frontier says no change-by-change evidence (`research/frontier/document-frontiers/EDF-V1-DRAFT-frontier.md:11`) | Mixed |
| 19 | Diagnostic topology (research only) | `docs/edf/validation/topology-findings.md:1-73` | HY-EDF-009 suggestive, Low; explicitly "not yet part of EDF" (`:8-10`) | Generic (not accepted) |
| 20 | Evidence Isolation (Constitution Art. XII) | `constitution.md:112-168` | HY-EDF-006 unsupported (LRC-001 "Planned. Not yet analyzed.") | Generic research method |

Bottom line: **no EDF component has a positive, controlled, independently replicated result.** The only completed controlled comparison (EV-EDF-009) found the whole EDF prompt did not beat a matched neutral structured prompt. The strongest positive state anywhere is "internal-demonstration" (HY-EDF-001) and "suggestive, Medium" (HY-EDF-007).

---

## 1. The 9-step Required Diagnostic Sequence

**Definition.** `docs/edf/specification.md:39-54`: "Every EDF analysis should follow this sequence: 1. Define the System Context (Primary System, Context, Focus) 2. Define the Outcome 3. Identify Manifestation(s) 4. Build the Origin Network 5. Trace Propagation 6. Evaluate Evidence 7. Identify Control Points 8. Rank Control Points 9. Determine Diagnostic Sufficiency". Repeated at `docs/edf/README.md:50-65`. `docs/edf/specification.md` is byte-identical to the frozen `docs/edf/releases/v0.3/specification.md` (verified with `diff`).

**Stated purpose.** "explain how a system produced an outcome and to identify the highest-leverage opportunities for influencing future outcomes" (`specification.md:15`). "The sequence remains the same across levels. The depth changes." (`specification.md:192`).

**Value evidence.**
- HY-EDF-001 "internal-demonstration", Medium-High (EV-EDF-001, EV-EDF-006): repository-authored narratives can be populated with these fields. Boundary: "does not establish explanatory validity or external generalization" (hypothesis registry).
- HY-EDF-008 "unsupported", Low-Medium (EV-EDF-009): EX-EDF-001 two-family run, complete-pair mean EDF minus matched-neutral +0.56 points, seeded bootstrap 95% [0.00, +1.39], frozen margin +5; analyzer A1 (OpenAI) 0.00, A2 (Anthropic) +1.00 (`research/analysis/edf-two-family-structural-value-results.md:29-46`; `research/experiments/EX-EDF-001/two-family-results.json`). Post-hoc fence-strip sensitivity: +0.21, [-0.42, +0.83] (`:57-62`). Natural analysis scored numerically highest (97.71 vs EDF 97.08 vs neutral 96.88, `:98-104`), with a severe ceiling effect.
- Caveat on that evidence: the "EDF" condition prompt in EX-EDF-001 (`research/experiments/EX-EDF-001/prompts.json`, condition `edf`) is a 7-point paraphrase that does **not** contain Outcome as a separate step, Manifestations, origin categories, the ranking criteria (influence/controllability/cost/confidence), Diagnostic Sufficiency or Next Responsible Action, and it **adds** "negative knowledge" and "counterfactuals", which are not v0.3 concepts. The matched-neutral prompt asks almost the same seven questions. So EX-EDF-001 tested an EDF-inspired reasoning prompt against a near-equivalent unlabeled prompt, not the v0.3 sequence as specified. The result says "EDF wording adds nothing over the same content without labels"; it does not test whether the 9-step content beats a weaker baseline (natural analysis was not better or worse either).

**Generic vs specific.** Generic. Domain-neutral; the vocabulary is engineering-incident-flavoured (manifestation, propagation, control point).

**Organizational fitness.** The sequence assumes a discrete outcome that already happened (`specification.md:72-84` demands factual outcome statements). Chronic organizational dysfunction (slow decay, persistent underperformance, contested goals) often has no single "outcome event". Nothing in the sequence asks whose account the evidence comes from, whether actors benefit from a particular diagnosis, or whether the outcome was an intended tradeoff.

## 2. Required Output Elements

**Definition.** `specification.md:150-168`: System Context, Primary System, Context, Focus, Outcome, Manifestation(s), Origin Network, Propagation, Evidence, Control Points, Control Point Ranking, Confidence, Diagnostic Sufficiency, Unknowns, Next Responsible Action. Templates implement them as headings (`docs/edf/templates/edf-0-quick-card.md:10-64`, `edf-1-standard.md:10-67`, `edf-2-complex.md:10-69`).

**Stated purpose.** Minimum inspectable record of a diagnosis.

**Value evidence.** Structural conformity only (HY-EDF-001). Claim policy says structural conformity does not establish semantic agreement, correctness, reproducibility, incremental value or utility (`claim-and-confidence-policy.md:13-24`).

**Internal inconsistencies found.**
- "Confidence", "Unknowns" and "Next Responsible Action" are required outputs but have no corresponding step in the 9-step sequence (`specification.md:39-54` vs `:150-168`).
- The frozen v0.3 `releases/v0.3/complexity-levels.md:19-33, 57-71, 103-117` requires **System** and **System Boundary**, not System Context/Primary System/Context/Focus. So frozen v0.3 contradicts itself (spec vs complexity levels). The current `complexity-levels.md:7` says "The frozen v0.3 baseline used the same required diagnostic fields across levels" and then lists System Context (`:19-35`). The first half of that sentence is true; the field list shown is not the frozen one.
- The teaching examples still use the pre-v0.3 grammar: `docs/edf/examples/challenger.md:11-12` uses "System" and "System Boundary", and `:20` uses a non-standard confidence label ("High on technical origin and strong on governance significance").
- In every R1 validation case, Focus is "Varied by lens" (`case-001R1-challenger.md:40`, `case-002R1-apollo-13.md:32`, `case-003R1-boeing-737-max.md:32`, `case-008R1-pixar.md:40`, `case-009R1-toyota-production-system.md:32`). Focus was never actually fixed in the validation corpus.

**Generic vs specific.** Generic.

## 3. Five verbs (design rule)

**Definition.** "If a new concept does not improve one of the five verbs, it does not belong in EDF. The five verbs are: Define, Observe, Explain, Influence, Validate" (`specification.md:23-33`). Principles: "This is a design filter, not an empirical proof rule. Concepts that pass it still carry Concept Burden." (`principles.md:51-61`).

**Stated purpose.** Guard against concept bloat. Pairs with Constitution Article VI, Concept Burden (`constitution.md:55-64`).

**Value evidence.** None, and none claimed. No mapping from step to verb exists in the current spec. The only mapping found is the removed website component `app/DiagnosticFlow.tsx` (deleted in commit `b27f5ff`), which mapped seven stages to verbs (two stages map to "Explain" and two to "Validate").

**Assessment.** The test is unfalsifiable as written: nearly any diagnostic concept "improves Explain". It has not filtered anything in practice. Concepts were deferred through the topology acceptance criteria (`topology-findings.md:66-73`) and Concept Burden, not through the verb test.

**Generic vs specific.** Generic governance heuristic, Echelon-authored.

## 4. Context Principle and System Context

**Definition.** "Every EDF analysis is valid only within its declared System Context." (`specification.md:35-37`; `model.md:21`). Three fields: Primary System, Context, Focus (`specification.md:58-70`; `model.md:23-49`). System Boundary is "subordinate" (`model.md:51-55`).

**Stated purpose.** Make scope inspectable. "Different analyses may disagree partly because they declared different System Contexts. That is not automatically a conflict in evidence." (`specification.md:70`).

**Value evidence.** HY-EDF-002 "System Context improves semantic scope agreement beyond the effect of adding matched headings": suggestive, Low-Medium; "No randomized System Context ablation or matched-heading control has been completed." Contradiction CTR-005: "Prompt-induced formatting could explain agreement" (`research/analysis/non-human-experimental-contradictions.md:11`). The historical "High confidence" claim was downgraded (`docs/edf/validation/evidence-ledger.md:44`). `principles.md:41` restates it as "an empirical hypothesis".

**Assessment.** The principle is reasonable but carries a hidden risk: "valid only within declared context" can make every diagnosis immune to criticism, because scope can be redefined after the fact. No rule says who sets the context or how a context choice is challenged. In organizations, choosing the boundary is itself political: drawing the system around "the team" rather than "leadership" changes which origins can be found.

**Generic vs specific.** Generic.

## 5. Outcome

**Definition.** "State the outcome in factual terms, not interpretive terms." Good: "Two aircraft were lost and the fleet was grounded". Weak: "Management failed" (`specification.md:72-84`; `model.md:57-65`).

**Value evidence.** None isolated.

**Organizational fitness.** Helpful as anti-blame discipline. It offers no guidance for outcomes defined by a metric (for example "attrition rose 40%"), where metric validity is the first question. See `diagnostic-taxonomy.md`.

## 6. Manifestations

**Definition.** "Manifestations are observable signals." (`specification.md:86-90`); "what can be seen, measured, reported, or directly observed" (`model.md:71`).

**Value evidence.** None isolated. In ABL-002 the author rated removing Manifestations as "High" degradation (`docs/edf/validation/ablation/ablation-002-propagation-refinement.md`, Updated Ablation Results table). That rating is ordinal, author-assessed and single-case (EV-EDF-002 limitations).

**Assessment.** "Reported" is treated as observable. There is no distinction between a self-report by an interested party and an instrumented measurement, which is a major gap for organizational use.

## 7. Origin Network and origin types

**Definition.** "Identify the contributing origins that made the outcome possible. Origins may be: Technical, Human, Organizational, Environmental, Financial, Regulatory. Complex outcomes should usually be modeled as an origin network rather than a single root cause." (`specification.md:92-105`). The model adds: "It should not invent multiple causes merely because a case is complex." (`model.md:91`). Anti-pattern pair: "Forced single-cause storytelling" / "Mandatory multi-cause storytelling" (`principles.md:90-91`).

**Stated purpose.** Avoid premature single root cause (`principles.md:17-21`).

**Value evidence.**
- HY-EDF-007 (Origin Network vs Propagation distinct): suggestive, Medium. EV-EDF-002 is a single POS-outage ablation with a shared evaluator and no raw outputs. CTR-007: "distinction evaluated once by the authoring system".
- HY-EDF-009 (topologies/layers generalize): suggestive, Low.
- HY-EDF-2026-A015 (interaction semantics): unsupported, pending A003; no runs.
- CTR-006: success cases "were selected and interpreted after framework creation"; "descriptive flexibility may be mistaken for explanatory power" (`contradictions.md:12`).

**Generic vs specific.** Generic. The category list is not tailored to organizations (see taxonomy file).

## 8. Propagation and propagation types

**Definition.** "Describe how effects moved from the origin network to the manifestations and outcome. Propagation paths may be: Physical, Informational, Organizational, Financial, Human" (`specification.md:107-117`). The v1 draft refines it: "Origin Network is structural. Propagation is behavioral." (`specification-v1-draft.md:89-105`), adopted from ABL-002.

**Value evidence.** HY-EDF-007 (above). HY-EDF-2026-A013 pending. FM-010 "Dynamic-state compression" (Apollo 13): the static output schema struggles with evolving state (`research/analysis/non-human-experimental-failure-modes.md:16`).

**Generic vs specific.** Generic.

## 9. Evidence evaluation

**Definition.** "Separate direct observation from inference. Evaluate the quality of evidence supporting the current model and note meaningful gaps or disputes." (`specification.md:119-123`). "Evidence is the observed, measured, documented, or independently supported basis" (`model.md:107-115`). Working constraints: "Separate evidence from inference. Record material competing explanations." (`principles.md:78-79`).

**Value evidence.** None. HY-EDF-006 (proportional updating) unsupported. HY-EDF-2026-A011 (negative knowledge) and A014 (evidence revision) pending with no analyzer runs.

**Assessment.** There is no evidence-grading scheme, no source-reliability or credibility rating, no rule for conflicting testimony, no evidence-request step, and no criterion for when evidence is too weak to diagnose. The A003 benchmark operationalises evidence as supported / contradicted / unknown per candidate (`research/experiments/EX-EDF-2026-A003/cases/*.case.json`, `allowedAnswers`). That is more precise than the spec, but it exists only in the experiment, not in EDF.

## 10. Control Points

**Definition.** "Identify where intervention could most effectively influence future outcomes. Control points may differ from origins." (`specification.md:125-129`). "'Leverage' is currently a heuristic judgment, not a validated quantitative metric." (`model.md:133`). v1 draft adds required attributes: Intervention, Responsible actor, System level, Expected influence, Confidence (`specification-v1-draft.md:147-159`).

**Stated purpose.** "Leverage Is A More Useful Diagnostic Target Than Blame" (`principles.md:23-29`).

**Value evidence.** HY-EDF-004 suggestive, Low: "narrative and not scored against a blinded baseline". The ledger downgraded the earlier "Medium-High" claim (`evidence-ledger.md:45`). HY-EDF-2026-A012 pending.

**Generic vs specific.** Generic. This is arguably EDF's most distinctive idea (origin ≠ best intervention point), and also the least tested.

## 11. Control-point ranking criteria

**Definition.** "Rank candidate control points using: Influence, Controllability, Cost, Confidence. The ranking does not need false precision. It does need a defensible ordering." (`specification.md:131-140`). The policy widens the list: "Influence, controllability, cost, reversibility, timing, and confidence may be considered, but no fixed weights or numerical precision are implied until validated." (`claim-and-confidence-policy.md:123-127`). Anti-pattern: "False-precision ranking" (`principles.md:93`).

**Value evidence.** None. RQ-011 (EDF vs FMEA prioritization) is deferred (`docs/framework-engineering/deferred-insights.md:200`). "Formal Control Point prioritization" is an open research area (`specification-v1-draft.md:391`).

**Assessment.** The spec has four criteria, the policy six, and the v1 draft uses "expected leverage". There is no aggregation rule and no tie-break, and none of the criteria cover political feasibility, who owns the control point, side effects, or second-order incentive effects. All of these are central for organizational interventions.

## 12. Confidence handling

**Definition.** "Confidence is a qualitative statement about how strongly the current evidence supports the diagnostic model. In EDF v0.3 these labels are not calibrated probabilities." Labels: Low, Medium, High (`model.md:117-127`). The research policy uses a five-level scale (Low, Low-Medium, Medium, Medium-High, High) (`claim-and-confidence-policy.md:61-73`). Anti-pattern: "Confidence as probability" (`principles.md:94`).

**Value evidence.** None. "validated quantitative confidence" is explicitly not established (`docs/edf/README.md:34`).

**Assessment.** Analyses use three labels and research claims use five. There is no rubric mapping evidence features to a label, no per-claim versus whole-model distinction, and no link from confidence to sufficiency other than prose (`model.md:158`). In A003, confidence is 0-100 per item and is scored with a **linear** rule (`scripts/edf-002-score.mjs:30-31`: `correct ? conf : 1-conf`). A linear rule is not a proper scoring rule: when accuracy is above 50% it rewards stating extreme confidence. See `implementation-audit.md`.

## 13. Diagnostic Sufficiency

**Definition.** "Ask: Do we understand enough to take the next responsible action? Diagnosis is sufficient when additional investigation is unlikely to materially change the next responsible action." (`specification.md:142-148`). "action-relative threshold ... given its consequence, reversibility, and material unknowns" (`model.md:139-145`). Policy: "not a declaration that the diagnosis is true or complete. Higher-consequence or irreversible actions require stronger evidence than low-cost reversible tests." (`claim-and-confidence-policy.md:115-121`).

**Stated purpose.** A stopping rule ("Diagnosis Stops At Action-Relative Sufficiency", `principles.md:43-49`).

**Value evidence.** None. No hypothesis in the registry tests sufficiency judgments.

**Assessment.** This is a sound concept, close to value-of-information reasoning, but it has no operational procedure. Nothing says how to estimate whether more investigation "would materially change" the action. The output is binary (Yes/No in templates and examples, e.g. `complexity-levels.md:57`). There is **no abstention outcome** (for example "insufficient evidence to diagnose; do not act on this diagnosis") and **no evidence-request outcome** (for example "the next responsible action is to collect X"). The removed website flow showed "Act · Investigate · Reframe" as outputs (`app/DiagnosticFlow.tsx`, deleted in `b27f5ff`), but that three-way outcome was never written into the spec.

## 14. Complexity levels EDF-0 / EDF-1 / EDF-2

**Definition (current guidance).** `complexity-levels.md:9-15`: EDF-0 "Simple, local, low-consequence, low-dispute issue"; EDF-1 "Several plausible origins or moderate consequence"; EDF-2 "Multiple interacting systems, stakeholders, disputed origins, or higher consequence". All levels share the same required fields (`:17-37`). Escalation triggers are at `:101-109`. Working rule: "Use the smallest depth that supports the next responsible action." (`:113`).

**Stated purpose.** Proportionality: "The sequence remains the same across levels. The depth changes." (`specification.md:192`).

**Value evidence.** HY-EDF-010 "EDF-0 is fast and learnable enough for routine use": unsupported, Low. "Teaching examples show intended use only". The comparative gap analysis self-rated EDF "Weak-Moderate" on speed and ease of learning (`docs/edf/validation/comparative/comparative-gap-analysis.md:13-14`).

**v1 delta.** The v1 draft replaces "same fields, different depth" with progressive grammar: EDF-0 = Manifestation, Likely Origin, Immediate Control Point; EDF-1 adds System Context, competing origins and confidence; EDF-2 adds Origin Network, Propagation and Ranked Control Points; a new EDF-3 adds full Calibration (`specification-v1-draft.md:233-313`).

**Generic vs specific.** Generic.

## 15. Diagnostic Calibration (optional extension)

**Definition.** "a proposed collaborative protocol for comparing, challenging, refining, and synthesizing multiple EDF analyses of the same system" (`diagnostic-calibration.md:7`). Stages: separate analyses, cross review, revision record, synthesis (`:49-91`). Default lenses: Systems, Operations/Governance, Independent Investigation, Differential Diagnosis, Legal/Evidentiary (`:37-43`).

**Evidence boundary (self-declared).** "The repository does not yet establish that Diagnostic Calibration improves diagnostic accuracy, confidence calibration, or decision outcomes." (`:17`). "When one model, agent, or research process produces several lenses, those outputs are not independent replications." (`:21`). The metrics Calibration Delta, Confidence Shift, Consensus Index and Perspective Contribution are "descriptive tracking ideas only" (`:93-97`).

**Value evidence.** None. FM-003 shared-evaluator dependence: "lenses are not independent evaluators" (`failure-modes.md:9`). HY-EDF-003 and HY-EDF-005 are unsupported.

**Generic vs specific.** The protocol idea (structured multi-analyst comparison) is generic. The specific lens set is Echelon-authored and untested. Of the five lenses, only Operations/Governance mentions incentives (`docs/framework-engineering/validation-protocol.md:83-94`).

## 16. Constitution

**Definition.** Twelve articles (`constitution.md:13-168`): Reality First, Understanding Before Action, Simplicity, Evidence, No Sacred Concepts, Concept Burden, Version Integrity, Evidence-Based Evolution, Diverse Perspectives, Framework Serves Reality, Humility, Evidence Isolation.

**Purpose.** "The constitution governs framework evolution, not how to perform an EDF analysis." (`specification.md:9`).

**Value evidence.** Not an empirical claim. One compliance tension: `research/frontier/repository-health.md:49` and `docs/edf/validation/ex-edf-001-structural-value-benchmark.md:35` commit that if matched-neutral is non-inferior, "the theory should be simplified rather than the goalposts moved". After EV-EDF-009 met that condition, the program did not simplify the framework. It moved to construct-specific tests (EX-EDF-2026-A003). This is defensible as narrowing the question, but the "simplify" commitment remains unexecuted.

**Generic vs specific.** Echelon-specific governance.

## 17. What is the actual relationship between EDF and Clarity?

**Short answer: in this repository, Clarity is a name and an intended handoff, not an implemented or specified integration. The two EDF documents that describe it give incompatible roles.** No Clarity source, schema, API, test or example exists in this checkout. The `clarity-framework` and `clarity-service` repositories are **not present** (`/home/user` contains only `echelon-diagnostic-framework` and `echelon-organization-administration`), and no file in this repo references either name (repo-wide grep for `clarity-framework|clarity-service` returned nothing).

Evidence in the repo:

1. **v0.3 (frozen), Clarity is downstream: the decision framework.**
   - "EDF is not primarily a decision framework. It establishes confidence in reality before decision frameworks such as Clarity evaluate what to do next." (`specification.md:17`)
   - "EDF creates the diagnostic grammar. Diagnostic Calibration improves confidence in the diagnostic model. Clarity uses the calibrated understanding to evaluate decisions." (`specification.md:194-200`)
   - `README.md:9-15`: EDF answers "What is happening?"; Clarity answers "What should we do?". Also `README.md:89-95`.
   - Operating cycle: `Understand -> Decide -> Commit -> Execute -> Understand`, where Decide = Clarity: "Evaluate options and choose a path" (`operating-cycle.md:9-21`; `:35-41` "Use Clarity to assess options, assumptions, tradeoffs, and risks.").
   - Handshake table: EDF hands Clarity system and boundary, outcome and manifestations, origin network and propagation, ranked control points, confidence and unknowns (`operating-cycle.md:77-89`).

2. **v1.0 draft, Clarity is upstream and lateral: a governor of reasoning depth.**
   - "Clarity governs reasoning depth and reconsideration. Clarity helps determine: Current justified commitment; Reconsideration criteria; Escalation; De-escalation. Clarity can determine which EDF mode is currently justified." (`specification-v1-draft.md:212-223`)
   - "Mode selection is governed by Clarity." (`:237`)
   - The comparative gap analysis recommends: "Address speed through Clarity-driven mode selection" (`comparative-gap-analysis.md:35`).
   - The frontier record flags this: "Mode selection is attributed to Clarity without validation here." (`EDF-V1-DRAFT-frontier.md:11`)

3. **Contradiction.** In v0.3, Clarity consumes EDF output after diagnosis is sufficient. In v1, Clarity decides before and during diagnosis how deep EDF goes, and owns "reconsideration". That inverts the dependency and creates a circular loop: EDF's own Diagnostic Sufficiency (`specification.md:142-148`) and escalation triggers (`complexity-levels.md:101-109`) already decide depth, so v1 assigns the same function to two components. Neither document defines Clarity's inputs, outputs or failure modes.

4. **No data contract.** EDF outputs are free-text Markdown headings (templates). The only machine-readable EDF-like contract is the experiment output contract (`research/experiments/EX-EDF-001/output-contract.json`, six top-level fields) and A003's item-answer contract. Neither is described as a Clarity interface.

5. **No evidence.** No hypothesis, evidence record or experiment concerns the EDF→Clarity handoff or decision quality. `principles.md:31-35` says diagnosis "does not prove decision quality".

Conclusion for downstream agents: treat "EDF → Clarity" as an unvalidated, under-specified design intent with two conflicting versions (v0.3 "Clarity decides after EDF" vs v1 "Clarity governs EDF depth"). Any integration work must first pick one, define a schema, and be checked against the actual Clarity repos, which are outside this checkout.

## 18. v1.0 draft deltas (vs v0.3)

Source: `docs/edf/specification-v1-draft.md`. Status: "candidate architecture, not a validated successor to v0.3" (`:5`).

| Area | v0.3 | v1.0 draft | Evidence for change |
|---|---|---|---|
| Core outputs | 9 steps, 15 output elements (`spec:39-54, 150-168`) | 6-element "core grammar": System Context, Manifestations, Origin Network, Propagation, Control Points, Calibration (`v1:32-43`) | Only ABL-002, a single-case ablation run on this 6-element grammar (`ablation-002...md`, "Baseline Grammar") |
| Outcome | Separate required step | Removed as separate element (folded into Manifestations) | None |
| Evidence / Confidence / Unknowns / Sufficiency / Next Action | Separate required elements | Merged into "Calibration": confidence, uncertainty, competing hypotheses, unresolved evidence, diagnostic limits, reconsideration triggers (`v1:161-174`) | None. **Diagnostic Sufficiency and Next Responsible Action disappear as named outputs.** |
| Origin categories | Tech, Human, Org, Environmental, **Financial**, Regulatory (`spec:96-103`) | Tech, Human, **Operational**, Org, **Informational**, Regulatory, Environmental; "helpful but not mandatory" (`v1:77-87`) | None (RQ-003 open) |
| Propagation | "how effects moved" | "structural vs behavioral" split (`v1:89-105`) | EV-EDF-002 (single ablation) |
| Control points | location + 4 ranking criteria | adds Intervention, Responsible actor, System level, Expected influence, Confidence; prioritized "by expected leverage" (`v1:147-159`) | None |
| Levels | EDF-0/1/2, same fields | Progressive EDF-0..3; EDF-3 new (`v1:233-313`) | None (RFR-006 "test progressive modes") |
| Clarity | Downstream decision framework | Governs depth/mode selection (`v1:212-223, 237`) | None |
| Scope boundaries | n/a | "What EDF Is Not" list (RCA, 5 Whys, FTA, FMEA, Bayesian, Systems Thinking, ...) (`v1:176-192`); when-not-to-use (`v1:345-360`) | CFCV-001..003 (EV-EDF-003), narratively reconstructed, FM-004 "Baseline simulation" |
| Open areas | n/a | Topology, layered origins, formal CP prioritization, visualization, quantitative calibration, evolving state, FMEA/FTA integration (`v1:385-395`) | n/a |

Key risk: the v1 draft removes the two elements most relevant to responsible action under uncertainty (Diagnostic Sufficiency, Next Responsible Action) without evidence. It also makes Clarity, which is absent here, a dependency for choosing modes.

## 19. Diagnostic topology (research, not EDF)

Covered in detail in `diagnostic-taxonomy.md`. "Diagnostic topology is not yet part of EDF. It is a research hypothesis." (`topology-findings.md:8-10`). HY-EDF-009 suggestive, Low.

## 20. Evidence Isolation (Constitution Article XII)

**Definition.** "Every Validation Run shall be conducted using only the evidence contained within its declared Evidence Package." (`constitution.md:124`). Purpose: prevent hindsight bias in longitudinal cases.

**Value evidence.** HY-EDF-006 unsupported. LRC-001 status "Planned. Not yet analyzed." (`docs/edf/validation/longitudinal/LRC-001-chernobyl.md:3`). Historical cases (Challenger, Boeing, Apollo) are well known, so frontier models cannot be isolated from prior knowledge of them. This is why EX-EDF-001 and A003 moved to synthetic cases (`ex-edf-001-structural-value-benchmark.md:17`).

## 21. Design-decision timeline (from git history)

| Commit | Date | Decision |
|---|---|---|
| `2c54fc5`..`6784251` | 2026-06-10..06-27 | Initial framework docs |
| `7bab6fc`..`f5c8e5c` | 2026-07-28..30 | Public website, examples; `f5c8e5c` is the v0.3 snapshot source commit (`docs/edf/releases/v0.3/manifest.json`) |
| `b27f5ff` | 2026-09-20 | "Harden EDF proof integrity": registries, claim policy, Validation Protocol v1.2, v0.3 freeze, removed `app/` React stack (incl. `DiagnosticFlow.tsx`), claims downgraded |
| `5d5f977` | 2026-09-21 | Preregister EX-EDF-001 |
| `d514360`..`f522094` | 2026-09-22 | Amendment A2 (two-family), record two-family result, HY-EDF-008 updated |
| `554a03c`..`b86bcb5` | 2026-09-22 | EX-EDF-2026-A003 spec, hypotheses A011-A016, scoring/validator |
| `2202272`..`0ef0abd` | 2026-09-23..24 | OpenAI case authoring; Anthropic review (all revise); remediation r1; re-review (2 pass, 4 revise) |
| `a1954b7`..`41fea6f` | 2026-09-30..11-02 | License, Limen/Praxis/Ordo tooling upgrades, Forma site migration; no methodology changes |

No commit after 2026-09-26 changes EDF methodology or experiment state.
