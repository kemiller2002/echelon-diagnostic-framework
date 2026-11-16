# EDF methodology audit (Workstream B)

Retrieval/inspection date: 2026-11-16. Repo inspected read-only: `/home/user/echelon-diagnostic-framework` (HEAD 41fea6f, "Merge pull request #33 … edf-forma-migration"; shallow history, 2 commits visible, so authorship history cannot be reconstructed from git).

Labels used: **DOCUMENTED** = written down as part of the framework; **ASPIRATIONAL** = described as intended / hypothesis / candidate / planned; **VALIDATED** = supported by a completed test that meets EDF's own claim policy. Nothing in EDF currently reaches VALIDATED for any performance claim.

---

## 1. What EDF actually is (OBSERVATION)

- EDF describes itself as "a working diagnostic grammar for making system diagnoses explicit enough to challenge" (REPO:echelon-diagnostic-framework/README.md; SRC-B-001) and "a systems-understanding framework with a diagnostic grammar" (REPO:…/docs/edf/README.md; SRC-B-002).
- Status: "Working draft, Specification v0.3" — v0.3 is the frozen baseline; a v1.0 document exists as a "candidate architecture, not a validated successor" (SRC-B-003, SRC-B-008).
- Physically, EDF is ~9 markdown concept documents (≈1,480 lines in `docs/edf/*.md`), three fill-in templates (EDF-0/1/2), three worked historical examples (Apollo 11, Boeing 737 MAX, Challenger), a research corpus (validation case files, registries, two preregistered experiments), and a 5-page static website with no JavaScript (`static-site/`, served at diagnostic.echelonfoundry.com per `CNAME`).
- **There is no EDF software.** No analysis engine, no data model implementation, no runtime. `scripts/edf-benchmark.mjs` and `scripts/edf-002-*.mjs` are research-harness scripts for validating/scoring experiments; `scripts/validate-repository.mjs` checks registry integrity and site structure. Confirmed by the portfolio governance audit: "Diagnostic-grammar research framework plus a static public website … No runtime code. Node integrity scripts" (REPO:echelon-organization-administration/application-governance/evidence/2026-10-05-phase1-discovery/applications.md; SRC-B-025).
- **`praxis` / `ros` / `.ros/` / `.echelon/` / `.sde/` / `tools/ros_fs_launcher.mjs` / top-level `registries/` are repository-governance tooling** (Repository Operating System: work-item attribution, agent provenance, telemetry), not part of EDF. Confirmed: `AGENTS.md` is a "GV-START-001 Agent Startup Guide" with agent provenance records (8 entries `openai/codex`, 1 `anthropic/claude-code`, 2 human); `ros.json` describes ROS version 3.1.1 and work protocol. CONFIRMED that this tooling is not EDF.

## 2. Purpose and core constructs (DOCUMENTED, v0.3)

Purpose: "explain how a system produced an outcome and … identify the highest-leverage opportunities for influencing future outcomes." "EDF is not primarily a decision framework." (SRC-B-003)

Design rule: "If a new concept does not improve one of the five verbs, it does not belong in EDF." Five verbs: Define, Observe, Explain, Influence, Validate. Context principle: "Every EDF analysis is valid only within its declared System Context."

Required 9-step diagnostic sequence (SRC-B-003):
1. Define the System Context (Primary System, Context, Focus)
2. Define the Outcome (factual, not interpretive)
3. Identify Manifestation(s) (observable signals)
4. Build the Origin Network (origin types: Technical, Human, Organizational, Environmental, Financial, Regulatory)
5. Trace Propagation (Physical, Informational, Organizational, Financial, Human paths)
6. Evaluate Evidence (separate observation from inference; gaps, disputes)
7. Identify Control Points ("Control points may differ from origins")
8. Rank Control Points (Influence, Controllability, Cost, Confidence — "does not need false precision")
9. Determine Diagnostic Sufficiency ("Do we understand enough to take the next responsible action?")

Required output elements (15): System Context, Primary System, Context, Focus, Outcome, Manifestation(s), Origin Network, Propagation, Evidence, Control Points, Control Point Ranking, Confidence (qualitative Low/Medium/High, explicitly *not* calibrated probabilities — SRC-B-005), Diagnostic Sufficiency, Unknowns, Next Responsible Action.

### Modes / levels (only as named)
- **v0.3 (current): EDF-0 Rapid, EDF-1 Standard, EDF-2 Complex.** Same required fields at every level; only depth changes. Escalation triggers table (uncertain leading origin, interacting contributors, control ≠ origin, stakeholder dispute, high consequence/irreversibility). Rule: "Use the smallest depth that supports the next responsible action." (REPO:…/docs/edf/complexity-levels.md; SRC-B-004)
- **v1.0 draft (ASPIRATIONAL): EDF-0, EDF-1, EDF-2, EDF-3**, with *progressive* fields (EDF-0 = Manifestation, Likely Origin, Immediate Control Point; EDF-3 adds "Full Calibration"). "Mode selection is governed by Clarity." Explicitly "design hypotheses until controlled evidence supports promotion." (SRC-B-008)
- No "maturity levels" exist in EDF. (EDF uses claim states, not maturity levels: internal-demonstration / suggestive / unsupported / contradicted / falsified / superseded / planned — SRC-B-013.)

### Optional extension: Diagnostic Calibration (ASPIRATIONAL)
Multiple EDF analyses compared via four stages (separate analyses → cross review → revision record → synthesis), with five default "Analytical Lenses" (Systems; Operations/Governance; Independent Investigation; Differential Diagnosis; Legal/Evidentiary). Its own document: "The repository does not yet establish that Diagnostic Calibration improves diagnostic accuracy, confidence calibration, or decision outcomes. Its current justified value is procedural." Lenses produced by one model "are not independent replications." Tracking metrics (Calibration Delta, Consensus Index …) are "descriptive tracking ideas only." (SRC-B-006)

### Operating cycle (DOCUMENTED, positioning only)
Understand (EDF) → Decide (Clarity) → Commit → Execute → Understand again (EDF). Execution layer fields: Owner, Plan, Priority, Verification. Validation loop questions: did the intervention change the manifestation / alter the control point / were origins correct. (SRC-B-007) No tooling or evidence for this loop.

## 3. Diagnosis vs choosing a response

- DOCUMENTED separation: EDF answers "What is happening?"; Clarity (per EDF) answers "What should we do?" (SRC-B-002). Start page: "Diagnosis is not the decision. EDF clarifies reality and identifies leverage. Your decision process still has to evaluate options, tradeoffs, ethics, resources, and risk." (SRC-B-022)
- In practice the boundary is porous: EDF's required outputs include **ranked control points** and a **Next Responsible Action**, and the site's worked examples recommend specific actions ("Disable automatic retry for 10% of traffic"). So EDF does include a lightweight intervention-selection step. Diagnostic Sufficiency is defined as action-relative ("not a certificate that the diagnosis is true or complete").
- CONCLUSION: EDF is diagnosis-plus-candidate-intervention-ranking; full option evaluation is deferred to "Clarity" or an unspecified decision process.

## 4. Relationship to Clarity as EDF documents it (with contradictions)

| Source | Characterisation of Clarity |
|---|---|
| EDF v0.3 spec & docs/edf/README (SRC-B-003, SRC-B-002) | "Clarity answers: What should we do?"; "Clarity evaluates options, tradeoffs, and decisions"; "decision frameworks such as Clarity" |
| EDF operating cycle (SRC-B-007) | Clarity = "Decide" stage, "assess options, assumptions, tradeoffs, and risks" |
| EDF v1.0 draft (SRC-B-008) | "Clarity governs reasoning depth and reconsideration … Clarity can determine which EDF mode is currently justified." "Mode selection is governed by Clarity." |
| Clarity README (REPO:clarity-framework/README.md; SRC-B-026) | "Clarity is not primarily: a decision framework … a consulting methodology"; "Clarity can be used before and during EDF to determine whether EDF-0 is sufficient or whether deeper diagnostic modes are justified. Clarity does not perform diagnosis. EDF performs diagnosis." |

CONTRADICTION (CON-B-004): the frozen EDF v0.3 positions Clarity as a decision framework; Clarity itself rejects that framing; the EDF v1 draft aligns with Clarity's own view (Clarity as commitment/reassessment governor that selects EDF depth). The two frameworks have not been reconciled in the canonical EDF baseline. The handoff table ("EDF Output → Why it matters to Clarity", SRC-B-007) is the only concrete interface; no shared schema or worked joint example was found.

## 5. Theoretical foundations cited

OBSERVATION: EDF cites essentially **no external diagnostic, safety-science, systems or organisational-science literature**. Repository-wide grep (excluding .git) for Leveson/STAMP-as-safety, Reason/Swiss cheese, Meadows/leverage points, Dekker, Weick, Rasmussen, Perrow, Vaughan, Croskerry, Graber, Cynefin/Snowden, Ishikawa, Kepner, Goldratt/Theory of Constraints, Pearl, causal loop, Senge, Hollnagel/FRAM, AcciMap, HFACS, Peerally returned zero substantive hits ("STAMP" hits were `TIMESTAMP` in CLI docs). The only external scholarly citations found in `docs/` and `research/` are the US National Academies 2019 report on reproducibility and NIST TN 1297 terminology — both about research method, not about diagnosis (SRC-B-010 EV-EDF-007; SRC-B-013).
- Named lineage is implicit only: a "Differential Diagnosis Lens"; comparisons to RCA/Five Whys, FMEA and FTA (CFCV-001..003), which are narrative reconstructions by the same research process, not executed comparators (SRC-B-010 EV-EDF-003; SRC-B-020).
- CONCLUSION: EDF's constructs closely resemble established ideas (Reason's latent conditions, Leveson's STAMP control structure/CAST, Meadows' leverage points, differential diagnosis, analysis of competing hypotheses, Bayesian updating, systems "propagation" in FTA/bow-tie) but neither acknowledges nor differentiates from them. Risk: reinvention, and credibility loss with expert buyers (safety, SRE, quality professionals) who will recognise the antecedents. See edf-scientific-foundations.md.

## 6. Target users, inputs, outputs, workflow

- **Target users: not established.** PROJECT-CHARTER.md: "Intended users — Not yet established"; "First bounded outcome — Not yet selected"; owners "Not yet assigned" (SRC-B-021). The charter is a generic template that talks about a "communication problem" and "communication taxonomy" — apparently copied from another Echelon project (CON-B-006).
- Implied users from the site: teams/operators diagnosing operational, software, product and organisational problems ("Bring one real outcome … a recurring support escalation, a delayed handoff, an adoption drop"), with a caution that "High-consequence safety, legal, clinical, or personnel decisions need appropriate experts and governance" (SRC-B-022).
- Inputs: a declared evidence set (observations, logs, documents). Research protocol adds frozen "Evidence Packages" for evidence isolation (e.g., EP-013A/B/C Chernobyl).
- Outputs: a structured text record with the 15 required elements (templates in `docs/edf/templates/`).
- Workflow: the 9-step sequence; the site compresses it to "seven moves" and an EDF-0 worksheet in six passes with a 15-minute timebox (CON-B-003 minor drift).

## 7. Human vs AI involvement

- OBSERVATION: All recorded validation executions are by AI models. Historical R1 analyses were produced "inside a shared research process" with "named analytical lenses [that] were not independent execution systems; exact prompts and model identities were not preserved" (REPO:…/docs/edf/validation/reproducibility-findings.md; SRC-B-018).
- EX-EDF-001 two-family run: analyzers OpenAI "gpt-5.6-sol" and Anthropic "claude-opus-5"; evaluators OpenAI "gpt-5.6-terra" and Anthropic "claude-sonnet-5" (as recorded in two-family-results.json; I cannot independently verify these model identifiers). EX-EDF-2026-A003 cases are authored by OpenAI-family models and reviewed by "claude-opus-5-5".
- Independent human study IFVS-001 is designed (recruitment, IDAR template) but has **zero completed runs** (SRC-B-019, SRC-B-027). No usability, time-on-task, or learnability study exists (HY-EDF-010 unsupported).
- Repository authorship is predominantly agent-generated per provenance metadata (AGENTS.md: `openai/codex` 8 entries). ASSUMPTION: the framework text itself is largely AI-drafted under founder direction; not verifiable from shallow history.

## 8. Validation status (what was tested, by whom, results)

### Registry counts (research/registries/*.json, v1.3.0, updated 2026-09-22)
Hypotheses (16 total): **internal-demonstration 1** (HY-001), **suggestive 4** (HY-002, -004, -007, -009), **unsupported 11** (HY-003, -005, -006, -008, -010, and six construct hypotheses HY-2026-A011..A016). **0 contradicted, 0 falsified, 0 supported.** Evidence records: 9 (2 negative-evidence, 1 internal case family, 1 ablation, 1 narrative method comparison, 1 adversarial synthesis, 1 external methodology, 1 preregistration infrastructure, 1 blinded two-family benchmark). Theories: 3 (TH-001 "EDF as explicit diagnostic representation" working/Medium; TH-002 "EDF-specific incremental diagnostic value" **unestablished/Low**; TH-003 provenance principle working/High). (SRC-B-009, SRC-B-010, SRC-B-011)

### EX-EDF-001 (Multi-Model Structural-Value Benchmark)
- Design: 6 synthetic, repository-authored cases × 3 conditions (natural, matched-neutral structured prompt, EDF) × 3 analyzer families (OpenAI/Anthropic/Google), blinded LLM evaluators, +5-point margin on 0–100 semantic utility, seeded bootstrap. Preregistered 2026-09-21 (SRC-B-014).
- **Why "blocked":** README and ledger say confirmatory execution is blocked "until exact analyzer and evaluator provider/model/version/configuration bindings are frozen"; `executor-matrix.json` remains `"status": "unbound"` with all slots null. Amendment A2 (2026-09-22) records that a third family is "assumed unavailable" under a "no-API-cost execution constraint" (SRC-B-016). So the three-family confirmatory test is blocked by executor availability/cost, not by design.
- **What actually ran:** a separately registered two-family analysis AN-EDF-001-2F. Results (SRC-B-015): 9/12 complete pairs; mean EDF − matched-neutral **+0.56 points** vs frozen +5 margin; bootstrap 95% [0.00, +1.39]; OpenAI-analyzer arm 0.00; post-hoc all-12-pairs +0.21 [−0.42, +0.83]. Severe ceiling: evaluator E2 scored 31/33 valid outputs at 100; mean condition scores natural 97.71, EDF 97.08, neutral 96.88 (natural numerically highest). Registered verdict: "does **not** support HY-EDF-008." 
- Validity caveats (CONCLUSION): LLM-only executors and evaluators; evaluators are same provider families as analyzers (affinity risk); 6 easy synthetic cases authored by the same research process; ceiling effect means the test could not discriminate. The result is weak evidence of *no large advantage* on easy cases, not evidence that EDF is useless.

### EX-EDF-2026-A003 (Construct-Specific Stress Benchmark)
Preregistered 2026-09-22, status "preregistered-pending-case-authoring": 12 cases planned, 6 authored (all OpenAI-family), reviews: 2 pass, 4 "revise" (e.g., ST-003 ground truth not supported as written). No analyzer run (SRC-B-017).

### Historical claims and self-correction
Earlier R1 summaries claimed "High"/"Very High reproducibility" for Challenger, Apollo 13, Boeing 737 MAX, Apollo 11, Pixar, Toyota. These are now reclassified as "historical internal structural agreement" with non-recomputable percentages (SRC-B-018). Positive governance observation: the project actively downgrades its own claims.

## 9. Documented limitations (EDF's own)
From README/ledger/policy: no cross-executor reproducibility; no causal improvement from System Context; no evidence of better control points; no evidence-sensitive updating; no superiority over natural analysis or matched neutral structure; no validated quantitative confidence; no routine-use speed/learnability; historical R1 runs not reconstructable; "Leverage is currently a heuristic judgment, not a validated quantitative metric" (SRC-B-005); Apollo 13 analysis found "static EDF output has difficulty expressing repeated state changes" (SRC-B-019).

## 10. Training materials
- `static-site/learn.html` ("A 12-minute introduction": three distinctions — Outcome ≠ explanation, Origin ≠ control point, Confidence ≠ certainty; seven moves; choosing depth), `start.html` (EDF-0 worksheet + downloadable Markdown template), `examples.html` (Challenger validation case + invented teaching cases: restaurant, API latency, feature adoption), `docs/edf/templates/` (3 templates), `docs/edf/examples/` (3 cases).
- No facilitator guide, course, certification, assessment of learners, or measured learning outcomes. No EDF material found in `/home/user/clarity-framework-training` (grep for "EDF" returned nothing).

## 11. Intended commercial applications
- EDF repo: none stated (charter: "Not yet established").
- Business plan (`business-plan/`): EDF is **not named** in any section markdown. Echelon is positioned as "a problem-class diagnostic organization built around Evidence → Diagnosis → Decision → Action → Learning" with "Paid Diagnostic Engagements" (05.02) and a "Paid Diagnostic Test" (17.01), and "method" listed as part of the "integrated moat" (01.05). The only source-level link: `business-plans/part-13.md` line 443 — "EDF may support this work." and `part-1.md` lists EDF among Echelon assets (SRC-B-023, SRC-B-024).
- The business-plan sections are near-identical templated text (same "Boundary and competing case", "Operating rule" and "Open Evidence" paragraphs repeated across 05.02 and 17.01) and label themselves "Evidence-supported hypothesis" without citing evidence for the diagnostic method's effectiveness.

## 12. Capability table

| Capability | Documented where | Implemented? | Validated? | Evidence |
|---|---|---|---|---|
| 9-step diagnostic sequence & 15 required output fields | docs/edf/specification.md (v0.3) | As templates/text only (docs/edf/templates) | No (structural conformity only) | HY-001 internal-demonstration (EV-001, EV-006) |
| System Context (Primary System/Context/Focus) | specification.md, model.md | Template fields | No — HY-002 suggestive, no matched-heading ablation | EV-001, EV-006 |
| Origin Network vs Propagation distinction | specification.md, model.md | Template fields | Partially — HY-007 suggestive (one author-assessed ablation, single case) | EV-002 |
| Control points + ranking (Influence/Controllability/Cost/Confidence) | specification.md | Template fields; no scoring method | No — HY-004 suggestive; leverage "heuristic" | EV-001, EV-006 |
| Qualitative confidence labels | model.md | Template field | No — explicitly not calibrated | Policy SRC-B-013 |
| Diagnostic Sufficiency / Next Responsible Action | specification.md | Template field | No | none |
| EDF-0/1/2 levels | complexity-levels.md | Templates per level | No — HY-010 unsupported (no time/learnability data) | EV-005 |
| EDF-3 & progressive fields | specification-v1-draft.md | No | No (design hypothesis) | none |
| Diagnostic Calibration (multi-analysis, 5 lenses) | diagnostic-calibration.md | No tooling; protocol text | No — "procedural value" only | EV-001 (lenses not independent) |
| Evidence-sensitive updating (longitudinal) | LRC-001, evidence packages EP-013A-C | Evidence packages exist | No — HY-006 unsupported, zero completed runs | EV-005 |
| Cross-executor reproducibility | IFVS-001, reproducibility-findings.md | Protocol only | No — HY-005 unsupported | EV-004, EV-005 |
| Incremental value over neutral structure | EX-EDF-001 | Benchmark harness (scripts/edf-benchmark.mjs) | Tested; **not supported** (+0.56 vs +5 margin, ceiling) | EV-009 |
| Construct-specific value (negative knowledge, control points, propagation, revision, interaction, protective controls) | EX-EDF-2026-A003 | Harness + 6/12 cases | No — pending; 4/6 cases need revision | none |
| Comparison with RCA/5 Whys, FMEA, FTA | CFCV-001..003 | Narrative reconstruction | No — comparators not executed | EV-003 |
| Operating cycle with Clarity (Understand→Decide→Commit→Execute) | operating-cycle.md | No | No | none |
| Validation governance (claim policy, preregistration, provenance, hashes, CI) | claim-and-confidence-policy.md, validation-protocol-v1.2.md, scripts/, .github/workflows | Yes (Node scripts; CI) | Works as governance (5/5 tests passing per SRC-B-025) | SRC-B-025 |
| Public website | static-site/ | Yes (static HTML) | n/a | — |
| Software for running EDF analyses | — | **No** | — | — |
| Training programme / certification | learn.html (12-min intro) | Minimal | No learning outcomes measured | HY-010 |

## 13. Mismatches between spec, registries, website and business plan

- **CON-B-001 (registry vs README/ledger/experiment files):** `evidence-registry.json` (2026-09-22) and `research/analysis/edf-two-family-structural-value-results.md` record a completed two-family run with a negative result for HY-008, but README.md still says confirmatory execution "is intentionally blocked", `evidence-ledger.md` (2026-09-21) says for HY-008 "no performance result exists", `executor-matrix.json` is still "unbound", and `preregistration.json` is still "preregistered-pending-executors". HY-008's registry state stays "unsupported" rather than "contradicted" (the policy permits "contradicted"; the analysis says the result "weighs against" HY-008). Human-readable projections lag the canonical registry by at least one update.
- **CON-B-002 (website vs registry):** `static-site/evidence.html` lists four claim states and never mentions EX-EDF-001, matched-neutral structure, or the negative result. Not a violation of the "public wording may not be stronger" rule, but a material omission from a page titled "Trust should be earned in public." The homepage presents EDF as "EDF interrupts here" in the Observation→Assumption→Solution pattern without noting that a neutral structured prompt performed equivalently in its only benchmark.
- **CON-B-003 (website vs spec, minor drift):** site "Seven moves" vs spec 9 steps; site ranking criteria add reversibility, speed, timing to spec's four; site origin types include "informational, economic" vs spec's "Financial, Regulatory".
- **CON-B-004 (EDF vs Clarity):** see §4.
- **CON-B-005 (modes):** v0.3 EDF-0/1/2 same fields vs v1-draft EDF-0..3 progressive fields; Clarity README refers to "deeper diagnostic modes" (v1 concept).
- **CON-B-006 (charter):** charter is an unfilled template about a "communication problem"; contradicts the framework docs and the site's implied audience.
- **CON-B-007 (business plan vs EDF evidence):** business plan treats diagnosis as the company's core and method as part of the moat ("Evidence-supported hypothesis"), yet (a) never commits to EDF as that method, and (b) EDF's only comparative test found no material advantage over a generic structured prompt. A method that a neutral structured template matches is unlikely to be a defensible moat by itself (CONCLUSION, medium confidence).
- **CON-B-008 (portfolio registry):** EDF is not in the echelon-registry systems list; its distribution class (application vs domain pack) is undecided (SRC-B-025).
- **CON-B-009 (CFCV docs):** CFCV-001's table rates EDF "Strong" vs RCA on complex origin networks, evidence evolution, control-point breadth — unqualified in the document itself, though the registry correctly bounds EV-003 as narrative.

## 14. Bottom line (CONCLUSION)
EDF is a carefully governed, honestly-bounded **research draft of a diagnostic template** with no software, no identified user, no human-subject evidence, and one completed comparative test that did not show value beyond a generic structured prompt (on easy cases). Its strongest asset is its research governance (claim policy, preregistration, provenance), not demonstrated diagnostic performance. Its weakest areas are absence of engagement with the existing safety/diagnostic science it resembles, unresolved positioning with Clarity, and no link to a defined commercial offer.
