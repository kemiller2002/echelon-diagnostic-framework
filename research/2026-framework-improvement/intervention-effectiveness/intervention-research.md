# Intervention Research: What the Literature Says About Acting on a Diagnosis

Mission EDF-IMPROVEMENT-2026-001 · Baseline EDF v0.3 (`41fea6f`) · 2026-11-16
Primary input: `../scientific-foundations/literature/lit-intervention.md`. Also used: `lit-adaptive.md`, `lit-longitudinal.md`, `lit-org-diagnosis.md` §3.2, `../baseline/*.md`.
Source ids refer to `../evidence/source-register.csv`. "[SR]" means search-result-only support.
Experimental results are not reported here. See experiments/ and strategy/ for measured results (EX-EDF-2026-OD1).

## Why this matters for EDF

EDF v0.3 ends with three steps:

- rank control points on Influence, Controllability, Cost and Confidence;
- decide Diagnostic Sufficiency;
- name a Next Responsible Action.

The operating cycle then hands off to Clarity and later asks, "Did the intervention change the manifestation?" (`../baseline/methodology-inventory.md` §11, §13, §17).

The baseline audit lists the gaps at this handoff:

- no abstention outcome (rank 6);
- no harm or reversibility criterion in the spec (rank 14);
- a ranking heuristic that has never been validated (rank 14);
- an undefined data contract with Clarity (rank 22).

All are in `../baseline/current-limitations.md`.

---

## 1. Base rates: how often organizational interventions work

| Finding | Type / strength | Sources | EDF implication |
|---|---|---|---|
| The "70% of change fails" figure has no valid, reliable empirical basis | Critical review of 5 published sources; moderate (fetched) | SRC-FGI-001 | Do not justify diagnostic depth with generic failure rates. Use reference-class priors (OPP-11; Workstream F8) |
| Meta-analysis of 52 planned-change evaluations: interventions change work-setting variables, then behaviour, then outcomes. Results "largely consistent" in aggregate. A later single-firm longitudinal test did not support it | Meta-analysis; moderate [SR] | SRC-FGI-002 | Each control point should state which link in the chain it acts on |
| Human-process OD interventions changed attitudes more than techno-structural ones | Meta-analysis (1989); moderate [SR] | SRC-FGI-003, via SRC-FGI-004 | The prior for attitude change is stronger than the prior for organizational outcome change |
| Psychologically based interventions: average productivity gain of nearly 0.5 SD | Meta-analysis (1985); moderate [SR] | SRC-FGI-018 | Old, published-only, probably inflated by publication bias |
| Feedback interventions: positive average (d ≈ 0.41), but more than a third *reduced* performance | Meta-analysis of 607 effects; strong [SR] | SRC-FGI-011 | "Influence" must be directional. Add a harm/backfire field (OPP-10c) |
| Survey feedback helps only with action planning, actionable items, supported managers and fast sharing | Systematic review; moderate (fetched) | SRC-FGI-004 | Collecting diagnostic data is itself a commitment |
| Identical AI processes at 8 sites: 4 transformed, 2 incremental, 2 little or no change | Comparative case; weak (fetched) | SRC-BH-052 | Contextual moderators belong in control-point confidence |
| Outcome evidence for the actions that diagnostic methods recommend is weak or mixed: Lean in healthcare, AI, BSC, EA, TOC, RCA | Reviews; weak to moderate | SRC-BH-017, SRC-BH-053, SRC-BH-014, SRC-BH-038, SRC-BH-020, SRC-BH-026, SRC-BH-027 | A correct diagnosis is not sufficient for improvement |

**Synthesis.**

- **Prior on attitude metrics.** The prior that a well-chosen intervention moves an attitude or behaviour metric is moderately positive.
- **Prior on the diagnosed outcome.** The prior that it moves the specific outcome a diagnosis targets is weaker, because the causal chain is longer.
- **Harm.** The prior that it causes harm is not negligible.
- **Quality of the base.** All OD meta-analyses found are 30 or more years old and exposed to publication bias. No recent comprehensive OD meta-analysis was located (lit-intervention §8).

---

## 2. Theory of change and implementation science

- **Programme theory.** Complex-intervention guidance (the MRC/NIHR update) requires an explicit programme theory and attention to how the intervention interacts with its context (SRC-FGI-009, [SR]). Theory of Change links process and effectiveness measures in one causal map (SRC-FGI-010, [SR]). Both are expert consensus, applied in trials. EDF's propagation path is a backward-looking causal map; a ToC is the forward-looking map for the intervention.
  - **Proposal OPP-15.** Each top control point carries a mini forward chain: control point, then propagation change, then manifestation change, then outcome, with the assumption at each link named.
- **Implementation outcomes are distinct from effectiveness.** Proctor's eight implementation outcomes are acceptability, adoption, appropriateness, feasibility, fidelity, cost, reach and sustainability (SRC-FGI-005, [SR]). RE-AIM covers Reach, Effectiveness, Adoption, Implementation and Maintenance (SRC-FGI-007, SRC-FGI-008, [SR]). CFIR 2022 covers determinants of implementability (SRC-FGI-006, [SR]). These frameworks are widely used, mostly in health. Their predictive validity in commercial organizations is not established.
  - **Proposal OPP-10(d).** Split Controllability into *authority* (can someone act?) and *implementability* (can it be done with fidelity here?).
  - **Proposal OPP-15.** In the validation loop, check adoption and fidelity *before* judging effectiveness. Otherwise an execution failure is misread as a wrong diagnosis.
- **Finding-to-fix gap.** Incident-analysis recommendations skew to training and policy, and events recur (SRC-CDE-037, fetched). Fixes are shaped by factors other than findings (SRC-CDE-044, [SR]). The RCA2 Action Hierarchy ranks structural changes above procedural ones and procedural above training (SRC-BH-028, fetched; practitioner consensus).
  - **Proposals.** OPP-10(b) adds a control-strength hierarchy. OPP-10(f) adds traceability from findings to fixes.

---

## 3. Change-failure claims and evidence-based management

Evidence-based management argues for using the best available evidence explicitly rather than authority claims (SRC-FGI-016, conceptual). The 70% myth is the canonical counter-example (SRC-FGI-001). McFillen et al. hypothesize that misdiagnosis drives change failure (SRC-BH-059, theoretical). The failure base rate they lean on is unsupported, and the misdiagnosis-to-failure link is untested.

**Implication.** EDF materials must not argue for themselves with change-failure statistics. The case for better diagnosis must be made by direct evaluation (`evaluation-design.md`).

---

## 4. Value of information, thresholds, real options and reversibility

| Concept | Statement | Type | Sources | EDF use |
|---|---|---|---|---|
| Test/treatment thresholds | Below the testing threshold, neither test nor treat. Above the test-treatment threshold, treat. Between the two, test. Thresholds depend on treatment risk and benefit, and on test accuracy and risk | Decision theory | SRC-FGI-019 [SR] | Formal meaning for Diagnostic Sufficiency (OPP-11) |
| Value of information | Information is worth the expected improvement in decision value. Perfect information is the upper bound | Decision theory | SRC-FGI-020 [SR] | Evidence requests should target what could change the top control point (OPP-12) |
| Decide on expected value; value research separately (EVPI) | Significance testing is largely irrelevant to adoption decisions | Decision theory / HTA practice | SRC-FGI-014 [SR] | Low confidence can still be sufficient if all live rivals imply the same action |
| EIG ≠ VOI | Information gain about the model differs from loss reduction for the decision | Formal | SRC-FGI-021, SRC-FGI-022 | Do not refine the origin network once it no longer changes the action |
| Real options | With irreversibility and resolvable uncertainty, waiting has option value; NPV invests too early | Economic theory | SRC-FGI-013 [SR] | Irreversible actions (restructuring, exits) need a higher sufficiency bar |
| One-way/two-way doors | Large organizations apply heavyweight process to reversible decisions | Practitioner heuristic | SRC-FGI-012 (secondary copies only; quotes unverified) | Reversible probes can proceed at low confidence (OPP-11) |

**Missing inputs.** EDF's sufficiency question ("additional investigation is unlikely to materially change the next responsible action") is a verbal version of the threshold model. It omits the *cost and harm of further diagnosis* (disruption, trust, survey fatigue) and the *cost of delay*. The claim policy mentions reversibility, but the spec does not (`../baseline/methodology-inventory.md` §11, §13).

**Practical limit.** The formal models need probabilities and utilities that organizations rarely have. The usable form is ordinal and qualitative:

- the current best action;
- what would flip it;
- roughly how likely that is;
- whether finding out is worth the cost and delay;
- how reversible the action is.

---

## 5. When to diagnose without recommending

- **Action bias.** Decision-makers prefer action even when inaction is optimal (SRC-FGI-017; football goalkeepers; mechanism evidence only). Consulting and internal-diagnosis settings add strong pressure to recommend something.
- **Backfire.** "Doing something" is not free: more than a third of feedback interventions harmed performance (SRC-FGI-011).
- **Formal permission.** The threshold model formally permits "no test, no treat" (SRC-FGI-019).
- **Complex domains.** Probe-sense-respond is proposed over analysis in complex situations (SRC-BH-049, SRC-BH-050; theoretical and unvalidated).
- **Gap.** No primary study measures how often organizational diagnoses *should* end without an intervention.

**Permitted EDF outcomes (OPP-11).** Each outcome needs a stated reason, and outcomes (d) and (e) need a re-review trigger.

- (a) Intervene at control point X.
- (b) Run a reversible probe.
- (c) Gather specific evidence E, stating its expected decision impact.
- (d) Monitor with a defined trigger (watchful waiting).
- (e) No action warranted.

**Diagnose without recommending when** any of these holds:

- the problem is not established, as in OD1 class K (non-existent problem);
- the "problem" is an accepted tradeoff, so the remedy is a decision for the owner, not a fix;
- all candidate actions are premature under the live rival explanations and no reversible probe exists;
- the expected harm of the best available action exceeds its expected benefit;
- data collection has already created a commitment (survey feedback) that must be honoured *first* (SRC-FGI-004).

---

## 6. Open questions this literature cannot answer

1. Do EDF-selected interventions outperform conventionally selected ones in real organizations? This needs the field protocol in `evaluation-design.md`.
2. How should selection quality be separated from execution quality? See `evaluation-design.md` §4 and implementation-outcome measurement in `outcome-measurement.md`.
3. Over what horizon should success be judged, and how should beta and gamma change be handled? See `outcome-measurement.md`.

## 7. Source cautions

- The OD meta-analyses (SRC-FGI-002, SRC-FGI-003, SRC-FGI-018) and Kluger and DeNisi (SRC-FGI-011) were seen only at abstract or summary level. Verify their effect sizes before any quantitative use.
- SRC-FGI-012 rests on secondary copies of a shareholder letter. Treat it as heuristic framing only.
- Implementation-science frameworks come from health care. Their transfer to commercial organizations is plausible but untested.
