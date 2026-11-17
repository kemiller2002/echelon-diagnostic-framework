---
id: EDF-COMPARATIVE-2026-002-INVENTORY
title: Research inventory — state of EDF evidence before the comparative program
status: current
version: 1.0.0
created: 2026-11-17
work_item: WI-0014
mission: EDF-COMPARATIVE-2026-002
repository_head: 728e393570ada11711b45955fcf666f95963e389
---

# Research inventory — EDF evidence state at program start

This records what the repository knows, claims, and has tested about the Echelon
Diagnostic Framework (EDF). It was assembled before any new experiment in this
program. Every row cites the artifact it rests on.

## 1. What is being evaluated

| Item | State | Source |
|---|---|---|
| Canonical specification | **EDF v0.3**, "working draft", frozen as the baseline at `41fea6f` | `docs/edf/specification.md`; `research/2026-framework-improvement/baseline/BASELINE_COMMIT` |
| Candidate successor | v1.0 draft, explicitly *not* validated or promoted | `docs/edf/specification-v1-draft.md` (research-boundary banner) |
| v0.3 components (for ablation) | 9-step sequence: (1) System Context, (2) Outcome, (3) Manifestations, (4) Origin Network, (5) Propagation, (6) Evidence evaluation, (7) Control Points, (8) Control-Point Ranking, (9) Diagnostic Sufficiency; plus required output elements and optional Diagnostic Calibration | `docs/edf/specification.md` §§ Required Diagnostic Sequence, Required Output Elements |
| Lightweight form | EDF-0 quick card | `docs/edf/complexity-levels.md` |

**Finding I-1 (structural).** Neither v0.3 nor the v1 draft defines EDF as a
method that *selects or coordinates other diagnostic methods*. The v1 draft adds
depth *modes* (EDF-0..3) whose selection it assigns to Clarity, not to EDF.
The program's orchestration and adaptive-selection hypotheses (Tracks C and §9)
therefore test a role EDF does not currently claim. They have to be tested as
experimental EDF variants, not as canonical EDF.

## 2. Completed experiments

| ID | What it tested | Design | Result | Status of the evidence |
|---|---|---|---|---|
| EX-EDF-001 (two-family) | EDF vs matched neutral structure (HY-EDF-008) | 6 synthetic cases; OpenAI GPT-5.6 Sol and Claude Opus 5 analyzers; two LLM evaluators; preregistered | EDF − neutral **+0.56 [0.00, 1.39]** on 9/12 complete pairs; frozen +5 margin not met; severe evaluator ceiling | **Verified negative**, two model families. Narrow: 6 cases, LLM-judge scoring at ceiling. `research/analysis/edf-two-family-structural-value-results.md` |
| EXP-OD1-01..07 | EDF v0.3 vs general AI, KT+NT, matched structure, EDF-0, variant D and its ablations, Clarity; recognition and open generation; two tiers | 22 synthetic organizational cases (10 dev, 12 held-out), deterministic candidate-state scoring, 484 runs, preregistered + amendment 2 | B − A **+1.4 [−0.7, 3.1]** (Sonnet), **−1.6 [−3.9, 0.9]** (Haiku), +1.7 [−1.5, 5.8] (open gen.); C(KT+NT) − B −0.6 [−2.0, 0.7]; D − B +1.5 [−0.3, 3.6]; condition means 87–96 (ceiling) | **Verified negative, Claude-only.** Re-scored in this program from raw outputs with `harness/Od1.fsx`: the per-run scores are byte-identical to the committed `scored/heldout/runs.csv`, and B − A, D − B, B − N and C − B reproduce exactly with the same seed (2026-11-17). |
| EXP-OD1-03 | Ablations of variant D | Single trial, one tier | Removing decision gates: D − NOGATE **+2.8 [0.5, 5.7]**; removing organizational guards +1.5 [−0.2, 3.6] | **Preliminary.** Exploratory, single trial, not replicated (UQ-05). |
| EXP-OD1-07 | Adaptive vs random/fixed evidence selection at k = 6 | Exploratory | Model-selected 84.3 vs random 72.6 (**+11.6 [5.9, 17.9]**); EDF-guided selector = general selector | **Preliminary but large.** The largest lever found; EDF added nothing to it. |
| EXP-OD1-08 | Independent re-scoring | Python reimplementation | All results reproduced; grader κ 0.95; item test–retest 97.5% | **Verified** (scoring reproducibility only; same model family produced the outputs). |
| EXP-OD1-09 (OD1-S) | Sequential evidence acquisition | Preregistered, 96 runs | Free stopping within 1–3 points of full evidence at 60–73% of items; KT+NT most evidence-efficient (**−1.7 [−2.6, −0.7] items**); stopping does not track difficulty; variant D broke a 6-item budget in 10/12 runs | **Verified within scope**; KT efficiency is exploratory (just short of the preregistered 2-item threshold). |

## 3. Older records (pre-2026-11)

| Record | State | Source |
|---|---|---|
| R1 case studies (Challenger, Apollo 13, Boeing 737 MAX, Apollo 11, Pixar, Toyota) | Descriptive demonstrations. No raw outputs, prompts, model identity or failed-run ledger preserved. "Very High reproducibility" withdrawn. | `research/analysis/non-human-cumulative-findings.md`; `docs/edf/validation/evidence-ledger.md` |
| ABL-002 (Propagation wording) | One authored POS example | same |
| CFCV comparisons (EDF vs RCA/FMEA/FTA on Chernobyl) | Definitional scope differences; shared narrative evaluator | same |
| IFVS-001 (multi-analyst validation) | **Zero completed runs.** Simulated multi-analyst exercises are not observations. | `research/frontier/document-frontiers/IFVS-001-frontier.md`; non-human inventory |
| LRC-001 (evidence revision) | Zero completed runs | same |
| EX-EDF-2026-A003 (construct-specific stress benchmark) | Preregistered; **6 of 12 cases authored** (OpenAI half only). Anthropic half missing although a journal claims completion (UQ-14). Never executed. | `research/experiments/EX-EDF-2026-A003/` |

## 4. Claim state

Canonical registry: `research/registries/hypothesis-registry.json`.

| Hypothesis | Registry state | What the evidence now supports |
|---|---|---|
| HY-EDF-001 field structure organizes narratives | internal-demonstration | Unchanged: grammar is applicable to authored cases. |
| HY-EDF-002 System Context improves scope agreement | suggestive | No new test. |
| HY-EDF-004 EDF improves control-point quality | suggestive | OD1 action items show no EDF advantage over general AI (action accuracy inside the null). |
| HY-EDF-005 / -003 cross-executor reproducibility | unsupported | Unchanged. |
| HY-EDF-006 proportional updating | unsupported | Unchanged (class H in OD1 is one case per split). |
| HY-EDF-008 EDF adds value beyond matched structure | unsupported ("no performance result exists") | **Stale wording.** Two completed, preregistered tests (EX-EDF-001 two-family; OD1) found no material effect. |
| HY-EDF-010 EDF-0 fast and learnable | unsupported | OD1: EDF-0 was −2.1 [−3.9, −0.5] vs full EDF (non-inferiority inconclusive). No human time data. |
| HY-EDF-2026-A011..A016 construct semantics | unsupported | Untested (A003 not executed). |

**Finding I-2 (records drift).** `evidence-ledger.md` (updated 2026-09-21) and
the hypothesis registry have not absorbed EX-EDF-001's two-family result or the
OD1 programme. The registry still reads as if HY-EDF-008 had no performance data.
Changing claim states is a governance action; this program **proposes** the
update (see §8) and does not apply it without owner approval.

## 5. Verified / preliminary / claimed / unknown

### Verified (experimentally supported, reproducible from preserved raw outputs)

1. EDF v0.3 shows no measurable diagnostic-accuracy advantage over a general-purpose
   Claude prompt on synthetic organizational cases at two tiers (OD1). Reproduced here.
2. EDF does not beat a matched neutral structure by a material margin across two
   model families (EX-EDF-001) or in OD1 at the preregistered margin.
3. An established method (Kepner-Tregoe + Nadler-Tushman) performs at least as
   well as EDF (OD1, OD1-S) and reads fewer evidence items (OD1-S).
4. Evidence *selection* matters more than reasoning procedure over a fixed file
   (OD1-07), and EDF guidance does not improve selection.
5. Long generic procedure can hurt smaller models (N − A −7.0 on Haiku).
6. Deterministic scoring of the OD1 outputs is reproducible (independent re-scoring).

### Preliminary (single trial, single tier, exploratory, or small n)

1. Decision-gate semantics (harm under every live hypothesis; "no intervention
   warranted" as a valid conclusion; discriminating next evidence): +2.8 [0.5, 5.7].
2. KT's IS / IS-NOT contrast specification as the mechanism of its evidence efficiency (UQ-15).
3. Variant D's discriminating-evidence instruction promotes budget violation (FT-20).
4. Organizational guards help on contradictory-stakeholder and masked-risk cases (UQ-06).

### Claimed without supporting evidence

1. EDF improves control-point identification, reproducibility, or evidence-sensitive updating (historical claims, already downgraded).
2. EDF is more effective as an orchestrator of other methods (no specification basis; Finding I-1).
3. EDF is primarily valuable to *human* analysts for communication and review (plausible, untested; UQ-02).
4. Earlier observations that waterfall-style organization beats iterative agent
   execution: **not located in this repository.** Searches for the observation
   and for "semantic-state generalization" found only Ordo's ownership of
   semantic state (`docs/ordo-observation.md`). These questions belong to
   another Echelon repository and are out of scope here unless the owner points
   to the source.

### Unknown (no valid test yet)

1. Whether any EDF effect appears when tasks are hard enough to avoid ceiling (UQ-03).
2. Which v0.3 *components* matter: component-level ablation was never run (completion criterion 12).
3. Whether EDF improves accuracy or only persuasiveness or traceability of explanations (program Track B).
4. Whether EDF's effect, if any, is an efficiency effect that disappears with more compute (Track A).
5. Whether method selection (by Claude or by an EDF-orchestrator variant) beats fixed methods (Track C).
6. Behaviour on non-organizational domains (technical incidents, cross-domain cases), deceptive evidence, and cases where no conclusion is possible.
7. Cross-provider and human-analyst effects (UQ-01, UQ-02).

## 6. Known defects in the existing evidence base

| Defect | Effect | Source |
|---|---|---|
| Single model family authored, reviewed, analyzed and graded OD1 | Shared blind spots can inflate agreement and ceiling | OD1 integrity notes |
| Recognition format offers the true answer among candidates | Ceiling at 87–96; differences live only in boundary judgements | `baseline-results.md` §Interpretation 4 |
| 12 held-out cases, mostly one trial | Power to detect ≈3-point effects is low | same |
| Request-menu order leak, calibration weighting deviation | Fixed (R-1, R-5) and re-scored | `continuation/research-status.json` |
| Truth files restored to disk mid-run in OD1-S | Transcript audit shows no access; still a protocol deviation | `sequential-results.md` |
| Literature register: 187 of 224 sources at "search-result" verification, 2 from memory | Claims resting on them are unverified | `evidence/source-register.csv` |
| Literature gaps vs this program's list | No coverage of decision-theoretic methods (VOI, RDM, minimax regret, scenario planning, red teaming), ACH/SATs beyond mention, falsificationism/abduction/MDL, Bow-Tie, STPA, HRO/NAT/resilience engineering, OODA/double-loop/sensemaking, and current LLM reasoning methods (ToT, GoT, Reflexion, PRMs, test-time compute) | keyword audit of `scientific-foundations/` and the source register, 2026-11-17 |

## 7. Most important unresolved questions (ranked by information value)

Ranking criterion: probability the answer changes a decision about EDF ×
size of that decision ÷ cost of answering it here.

| Rank | Question | Why it ranks here | Feasible in this environment? |
|---|---|---|---|
| 1 | Does *any* EDF effect (or any method effect) appear off the ceiling? | Every existing null may be a ceiling artifact. Without a discriminating benchmark, no other question can be answered. | Yes: build OD2 (harder; hidden evidence; deceptive, no-conclusion and stop-early cases; technical and cross-domain cases). |
| 2 | Which v0.3 components carry weight (component ablation)? | Determines the minimum effective configuration; never run. | Yes, on OD2. |
| 3 | Is EDF's value in efficiency rather than accuracy (Track A)? | A cheap-to-test reframing that could rescue or bury the remaining claims. | Partly: compute knob = samples (self-consistency) and model tier; thinking budgets are not controllable from subagents. |
| 4 | Do decision gates and a value-of-information stopping rule with an enforced budget replicate (UQ-04, UQ-05)? | The only positive signals so far. | Yes, as condition F on OD2. |
| 5 | Accuracy vs persuasiveness (Track B) | Directly tests the most likely failure mode of a structured grammar. | Partly: deterministic correctness yes; persuasiveness only by Claude rating (declared limitation) unless the owner supplies human raters. |
| 6 | Method selection vs fixed method (Track C) | Tests orchestration, which EDF does not currently claim. | Yes, as experimental arms. |
| 7 | Cross-provider replication (UQ-01) | Largest remaining validity threat | **No.** Only Claude is available in this session. |
| 8 | Human-analyst value (UQ-02) | EDF's most plausible remaining value | **No** without participants; protocol only. |

## 8. Proposed governance actions (not applied; need owner approval)

1. Update HY-EDF-008 in `research/registries/hypothesis-registry.json` and
   `docs/edf/validation/evidence-ledger.md` to cite EX-EDF-001 two-family and
   OD1 as completed negative tests.
2. Record OD1 evidence records (`EV-`) so claims can cite them.
3. Resolve UQ-14 (A003 Anthropic half) or formally close A003 as superseded by OD2's construct ablations.

## 9. Highest-information next experiments (selected)

**EX-EDF-2026-OD2**, a harder benchmark executed under one preregistration that
answers questions 1–6 above:

- **Difficulty by construction.** Evidence is hidden behind requests under an
  enforced budget. Decisive evidence is sometimes absent. Cases include misleading
  and deliberately deceptive evidence, cases where the correct conclusion is
  "insufficient", cases where stopping early is optimal, changing conditions,
  and technical-incident and cross-domain cases alongside organizational ones.
- **Conditions.** A (no framework); B (EDF v0.3 verbatim); C (KT); D (KT + ACH
  combination); E1–E5 (v0.3 component ablations); F (v0.3 + decision gates + VOI
  stopping); compute arms (A with k-sample voting); selection arms (Claude
  selects a method; EDF-orchestrator variant selects).
- **Measures.** Deterministic correctness and calibration as primary; persuasiveness
  measured separately and blind to correctness; tokens, reads and time per run.

Pilot on the dev split first; confirmatory runs only after the preregistration is
committed and hashes are frozen.
