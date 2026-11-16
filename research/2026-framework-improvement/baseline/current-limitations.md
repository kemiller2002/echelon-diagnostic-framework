# EDF Current Limitations (Baseline), Ranked by Severity

Audit agent: Agent A
Baseline commit: `41fea6ffc764adc69643d5ee2940916592486f84`

Severity scale, for the stated goal of using EDF for organizational diagnosis:

- **S1 Critical:** blocks any valid claim that EDF works for organizational diagnosis, or risks harmful use.
- **S2 High:** materially limits validity or reliability; must be addressed before organizational deployment.
- **S3 Medium:** reduces quality or usability; addressable incrementally.
- **S4 Low:** documentation or hygiene.

Source column: **[D]** = documented by the repo itself (with citation); **[I]** = independently identified in this audit.

## Ranked list

| Rank | Severity | Limitation | Source | Evidence / citation |
|---:|---|---|---|---|
| 1 | S1 | **No demonstrated incremental value.** The only controlled comparison found that the EDF prompt did not beat a matched neutral structured prompt: +0.56 points, CI [0.00, 1.39], margin +5; one analyzer family exactly 0.00; natural analysis numerically highest. | D | EV-EDF-009; HY-EDF-008 "unsupported"; `research/analysis/edf-two-family-structural-value-results.md:29-46, 98-104`; TH-EDF-002 "unestablished" |
| 2 | S1 | **Zero organizational validation.** Every case is an engineering, mission or operations narrative: R1 Challenger, Apollo 13, Boeing 737 MAX, Apollo 11, Pixar, Toyota; CFCV/LRC Chernobyl; EX-EDF-001 SYN-001..006 are all software/physical ops; A003 assignments are all `domainHint: "software/operational system"`. The only "organizational" cases (Pixar, Toyota) are celebrated success stories built from public management narratives with no cited sources (`case-008R1-pixar.md:22-24`). There is no organizational-dysfunction benchmark (e.g., metric gaming, coordination failure, leadership bottleneck, attrition). | I (partly D: FM-007 case-selection bias) | `docs/edf/validation/`; `research/experiments/EX-EDF-001/cases.json`; `research/experiments/EX-EDF-2026-A003/case-authoring-assignments.json`; `research/analysis/non-human-experimental-failure-modes.md:13` |
| 3 | S1 | **No human-rater or human-user data of any kind.** No independent human analyst study (IFVS-001 zero runs), no inter-rater reliability for any category, no usability or time-on-task data (HY-EDF-010), and no decision-outcome data. All evaluators so far are LLMs or the authoring process. | D | `non-human-cumulative-findings.md:11`; HY-EDF-005, HY-EDF-010 "unsupported"; EV-EDF-005 |
| 4 | S1 | **Causal claims without a causal-identification standard.** EDF speaks in causal terms ("origin ... where a contributing cause began", "made the outcome possible", "propagation") but requires no level of causal evidence (intervention, contrast, counterfactual, mechanism) and does not distinguish association from causation. Organizational data is mostly observational and retrospective, which makes post-hoc causal storytelling the default failure. | I | `docs/edf/model.md:77-105`; `specification.md:92-123` (only "Separate direct observation from inference") |
| 5 | S1 | **No stakeholder-credibility or source-interest handling.** Evidence is treated as "observed, measured, documented, or independently supported" (`model.md:109`) with no field for who supplied it, their stake in the diagnosis, independence, or corroboration. In organizational diagnosis the people diagnosed are usually the main evidence source. | I | `diagnostic-taxonomy.md` §3 |
| 6 | S2 | **No abstention rule.** Diagnostic Sufficiency is binary Yes/No in templates and examples. No outcome says "evidence insufficient: do not act on this diagnosis". Combined with LLM execution, this pushes toward always producing a confident-looking diagnosis. | I | `specification.md:142-148`; `complexity-levels.md:57`; `examples/challenger.md:21` |
| 7 | S2 | **No evidence-request mechanism.** "Unknowns" is a free-text list. There is no structured step to name discriminating evidence, its source, cost and expected effect on the action, even though Diagnostic Sufficiency is implicitly a value-of-information judgment. (EX-EDF-001's natural and neutral prompts ask for this, but the EDF spec does not.) | I | `specification.md:142-148, 167`; `research/experiments/EX-EDF-001/prompts.json` |
| 8 | S2 | **Headline experimental result is not recomputable.** No EX-EDF-001 analyzer outputs, evaluator outputs, run manifests or aggregation code are in the repo, its history, or any branch. `executor-matrix.json` remains "unbound", so by the experiment's own rule the runs were exploratory. This violates the repo's own numerical-claim and research-execution gates. | I (the policy is D) | `claim-and-confidence-policy.md:75-89, 141-156`; `docs/edf/validation/ex-edf-001-structural-value-benchmark.md:13`; `edf-benchmark.mjs validate` → `analyzerRuns: 0` |
| 9 | S2 | **Raw-output erasure and prompt/config loss across all historical experiments** (12/12); shared-evaluator dependence (≥ 9/12); unverifiable precision; baseline simulation (CFCV comparators reconstructed, not executed). | D | FM-001, FM-003, FM-004, FM-005 (`non-human-experimental-failure-modes.md:7-11`); EV-EDF-004 |
| 10 | S2 | **Single-author, single-process corpus.** Cases, analyses, lenses and evaluations were produced by one research process, and named lenses are not independent. A003 tries to fix this with cross-provider authoring and review, but only the OpenAI half exists. | D | `claim-and-confidence-policy.md:91-97`; `diagnostic-calibration.md:21`; CTR-001, CTR-005; FM-003 |
| 11 | S2 | **Taxonomy lacks organizational constructs**: incentives, informal power/politics, goal conflict, intentional tradeoffs, capability/capacity, culture, structure/interfaces, market conditions, metric validity, path dependence. The origin model presumes a defect, so deliberate tradeoffs get misdiagnosed as failures. | I | `diagnostic-taxonomy.md` §1.3; `model.md:79` |
| 12 | S2 | **Measurement/metric validity not addressed.** Outcomes and manifestations are assumed valid if observable. In organizations they are usually KPIs subject to gaming, lag and mis-specification. | I | `specification.md:72-90`; `model.md:57-75` |
| 13 | S2 | **Category schemes undefined and unreliable.** Origin, propagation, evidence, control-point, confidence and topology categories have no definitions, exclusivity rules or agreement data. Two inconsistent origin lists exist (v0.3 vs v1). | I (partly D: HY-EDF-009) | `specification.md:96-117` vs `specification-v1-draft.md:77-87`; `diagnostic-taxonomy.md` §10 |
| 14 | S2 | **Control-point ranking is an unvalidated heuristic** with three inconsistent criteria lists and no aggregation. It omits political feasibility, ownership, incentive side-effects and reversibility (the last appears in the policy only). HY-EDF-004 "suggestive, Low"; A012 pending. | D + I | `specification.md:131-140`; `claim-and-confidence-policy.md:123-127`; `specification-v1-draft.md:147-159` |
| 15 | S2 | **Confidence is uncalibrated and inconsistent** (3-level, 5-level and 0-100 scales); there is no rule for deriving a label, and no per-claim granularity. Where scored (A003), the rule is a linear, improper scoring rule that rewards extreme confidence. | D (uncalibrated) + I (improper rule) | `model.md:117-127`; `README.md:34`; `scripts/edf-002-score.mjs:30-31` |
| 16 | S2 | **A003 construct-specific benchmark stalled.** 6 of 12 cases authored (OpenAI only); 2 pass and 4 `revise` (groundTruthSupported=false) after two review rounds; 6 Anthropic cases missing; no freeze; no runs. A journal claims the Anthropic authoring was "already complete" with no artifact to back it. | I (status D) | `research/journal/JR-EDF-2026-008.md:28-37, 92`; `JR-EDF-2026-004.md:29` vs `JR-EDF-2026-006.md:30`; `node scripts/edf-002-validate.mjs freeze` → "freeze requires 12 cases; found 6" |
| 17 | S2 | **A003 design confound:** construct modules name the trap the case is built around (e.g., "do not weaken ... quarantine" vs a quarantine-surge case), while the neutral module is generic. A positive result would partly measure "being told what to look for". Truth validity rests on a single opposite-family LLM reviewer. | I | `research/experiments/EX-EDF-2026-A003/prompt-modules.json`; `cases/ST-011.case.json` |
| 18 | S2 | **Evaluator ceiling / weak discrimination** in EX-EDF-001 (E2: 31/33 perfect scores). Cases were too easy for frontier models. | D | `edf-two-family-structural-value-results.md:82-104` |
| 19 | S2 | **No longitudinal / evidence-updating evidence.** Evidence Isolation and LRC architecture exist; LRC-001 is "Planned. Not yet analyzed."; HY-EDF-006 unsupported. Historical cases cannot be isolated from model prior knowledge. | D | CTR-003; `LRC-001-chernobyl.md:3`; `constitution.md:112-168` |
| 20 | S3 | **Static, acyclic representation.** No feedback loops, delays or evolving state; Apollo 13 analysis showed compression of repeated state changes. Organizational dynamics are dominated by reinforcing and balancing loops. | D (FM-010, DI-002) + I | `non-human-experimental-failure-modes.md:16`; `deferred-insights.md:13` |
| 21 | S3 | **Context Principle can immunize diagnoses.** "Valid only within its declared System Context" with no rule for who sets or challenges the boundary. Boundary choice is political in organizations. In every R1 case Focus = "Varied by lens", so Focus was never exercised. | I | `specification.md:35-37, 70`; `case-*R1-*.md` (Focus lines) |
| 22 | S3 | **EDF→Clarity relationship undefined and contradictory**: v0.3 has Clarity downstream (decide after diagnosis); v1 has Clarity governing EDF depth and mode selection. There is no data contract, and Clarity repos are absent from this checkout. | I (partly D: frontier) | `specification.md:194-200`; `operating-cycle.md:77-89`; `specification-v1-draft.md:212-223, 237`; `EDF-V1-DRAFT-frontier.md:11` |
| 23 | S3 | **v1 draft removes Diagnostic Sufficiency and Next Responsible Action** as named outputs (folded into "Calibration") without evidence, and adds EDF-3. No v0.3→v1 claim/evidence/change matrix exists. | I (partly D) | `specification-v1-draft.md:32-43, 161-174, 293-313`; `EDF-V1-DRAFT-frontier.md:21` |
| 24 | S3 | **No operational engine, schema or output validator.** EDF is prose plus Markdown templates; analyses cannot be machine-checked, stored, compared or handed off. | I | `implementation-audit.md` §1 |
| 25 | S3 | **Constitutional commitment to simplify on a null result not executed.** After matched-neutral non-inferiority, the program narrowed to construct tests rather than simplifying the framework. | I | `repository-health.md:49`; `ex-edf-001-structural-value-benchmark.md:35` |
| 26 | S3 | **Success-case explanatory claims are post-hoc.** Apollo 11, Pixar and Toyota were selected and interpreted after the framework existed; there are no negative success cases. | D | CTR-006 (`non-human-experimental-contradictions.md:12`) |
| 27 | S3 | **Diagnostic Calibration and lens metrics are untested**; lens outputs from one process are not independent; there are no organizational lenses (HR, finance, customer, frontline). | D + I | `diagnostic-calibration.md:15-21, 93-97`; FM-003 |
| 28 | S3 | **Self-assessed comparative superiority persists in a working draft**: EDF rated "Excellent" for Organizational Analysis and Governance with no evidence. | D (downgraded via CTR-008, FM-011) | `comparative-gap-analysis.md:17-18` |
| 29 | S4 | **Stale projections.** The evidence ledger (2026-09-21) still says no EX-EDF-001 result exists and omits EV-EDF-009 and HY-2026-A011..A016; repository-health counts are outdated; EV-EDF-008's limitation text is superseded. No integrity check compares ledger and registry. | I | `evidence-ledger.md:4, 26, 65-75`; `repository-health.md:16-26` |
| 30 | S4 | **Frozen v0.3 is internally inconsistent** (spec requires System Context; frozen complexity levels require System/System Boundary). Current guidance partly misdescribes the frozen fields. Teaching examples use the old grammar. | I | `releases/v0.3/complexity-levels.md:19-33`; `complexity-levels.md:7, 19-35`; `examples/challenger.md:11-12` |
| 31 | S4 | **Dangling references:** a Mars Climate Orbiter topology entry has no case file; FCRs cite evidence packages EP-001 and EP-008, which do not exist; JR-EDF-2026-005 is missing. | I | `topology-findings.md:20`; `FCR-000001.md:13`; `FCR-000008.md:13`; `JR-EDF-2026-006.md:20` |
| 32 | S4 | **Test suite asserts absence of runs** (`analyzerRuns === 0`), so committing real EX-EDF-001 run bundles would break CI. | I | `tests/benchmark-integrity.test.mjs:13` |

## Documented-limitation cross-reference

### From the contradiction registry (`research/analysis/non-human-experimental-contradictions.md`)

| ID | Status | Mapped rank |
|---|---|---|
| CTR-001 "Very High reproducibility" vs no independent evidence | Unresolved | 3, 9 |
| CTR-002 VP v1.1 "improves" vs no v1.0 control | Unresolved | 9 |
| CTR-003 evidence evolution claimed vs LRC not run | Unresolved | 19 |
| CTR-004 "should not expand" vs many candidate additions | Contextual tension | 23 |
| CTR-005 System Context improves agreement vs no ablation | Unresolved | 21 |
| CTR-006 success explained post-hoc | Unresolved | 26 |
| CTR-007 ON/Propagation distinction evaluated once | Unresolved | 13 |
| CTR-008 CFCV comparators reconstructed | Design defect | 9, 28 |

### From the failure-mode taxonomy (`research/analysis/non-human-experimental-failure-modes.md`)

FM-001 raw-output erasure (Critical) → 8, 9; FM-002 unverifiable precision → 9; FM-003 shared-evaluator dependence (Critical) → 10; FM-004 baseline simulation → 9; FM-005 prompt/config loss (Critical) → 8, 9; FM-006 planned capability promoted to strength → 19, 28; FM-007 case-selection bias → 2, 26; FM-008 negative-run invisibility → 8; FM-009 construct/metric mismatch → 3; FM-010 dynamic-state compression → 20; FM-011 method-scope overclaim → 28; FM-012 build-environment under-specification (mitigated by `scripts/validate-repository.mjs`).

### From the claim policy (`docs/framework-engineering/claim-and-confidence-policy.md:105-113`): must not be described as established

Independent reproducibility (→ 3, 10), causal improvement from System Context (→ 21), improved control-point quality (→ 14), evidence-sensitive updating (→ 19), superiority to natural or matched-neutral (→ 1), quantitative calibration (→ 15), routine-use speed or learnability (→ 3). All seven remain unestablished at baseline.

## What the repository does well (for balance)

- Explicit, machine-checked claim governance; public claims are mechanically tied to registry states (`scripts/repository-integrity.mjs:188-189`).
- Willingness to record and publish a null result against its own framework (EV-EDF-009).
- Frozen, hash-verified v0.3 bundle; fail-closed experiment harnesses; append-only run recording design in A003 (`edf-002-analyzer.mjs` `flag:'wx'`).
- A003's deterministic item-level scoring, staged evidence and negative-knowledge distinctions (observed absence / not observed / unknown) are the most promising operational ideas in the repo. They are not yet part of EDF itself.

## Implications for the organizational-diagnosis program (for downstream agents)

1. Any claim that "EDF diagnoses organizations well" currently has **no supporting evidence**. Treat the baseline as an untested checklist grammar.
2. The first missing artifact is an **organizational-dysfunction benchmark** with hidden truth, interested-party testimony of varying credibility, metric-validity traps, intentional-tradeoff distractors, and cases where the correct output is "abstain" or "request evidence X".
3. The baseline lacks the outputs needed to score those behaviours (abstain, evidence request, source credibility, causal-strength grading). Adding them is a framework change and must be tested against a matched-neutral control, given rank 1.
4. Human-rater data is necessary for any usability or reliability claim; LLM-only evaluation has already shown ceiling and affinity problems.
