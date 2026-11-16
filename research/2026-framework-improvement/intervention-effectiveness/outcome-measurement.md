# Outcome Measurement for EDF-Guided Interventions

Mission EDF-IMPROVEMENT-2026-001 · Baseline EDF v0.3 (`41fea6f`) · 2026-11-16
Used by `evaluation-design.md`. Related documents: `intervention-research.md`, `../scientific-foundations/longitudinal-assessment.md`. Source ids refer to `../evidence/source-register.csv`; "[SR]" means search-result-only support.
No experimental results appear here. See experiments/ and strategy/ for measured results (EX-EDF-2026-OD1).

## 1. The measurement problem EDF currently leaves open

The operating cycle asks one question after execution: "Did the intervention change the manifestation?" (`../baseline/methodology-inventory.md` §17). Four things are missing:

- **No comparison.** Without one, improvement after a bad period is expected even if nothing was done (regression to the mean; SRC-FGI-015).
- **No implementation check.** A fidelity failure is indistinguishable from a wrong diagnosis (SRC-FGI-005).
- **No metric-integrity check.** Manifestations are often targets subject to gaming (SRC-CDE-064, SRC-FGI-073; `../baseline/current-limitations.md` rank 12).
- **No invariance check.** A survey change may be recalibration (beta change) or redefinition (gamma change), not real change (SRC-FGI-075).

The measurement set below closes these gaps. Its levels mirror EDF's own causal grammar: control point → propagation → manifestation → outcome.

---

## 2. Outcome hierarchy

| Level | What is measured | Examples | Timing | Why it is needed |
|---|---|---|---|---|
| L0 Implementation | Adoption, fidelity, reach, dose, implementation cost, acceptability | % of teams using the new triage rule; fidelity checklist score | Weeks 2–8 | Establishes that the intervention actually happened before effectiveness is judged (SRC-FGI-005, SRC-FGI-007, [SR]) |
| L1 Mechanism (proximal) | The propagation path the control point was meant to change, per the forward chain (OPP-15) | Queue age at the platform team; escalation latency; approval cycle time | Weeks 4–12 | Tests the *diagnosis*: if L1 does not move, the theory of change is wrong or was not implemented |
| L2 Manifestation | The observable signal that triggered the diagnosis | Missed customer dates; win rate; repeat contacts | Months 3–6 (primary window, pre-registered per problem type) | The primary outcome in `evaluation-design.md` |
| L3 Outcome | The organizational outcome defined in EDF Step 2 | Net revenue retention; incident cost; retention of a cohort | Months 6–24 | Longer causal chain, weaker priors (SRC-FGI-002, [SR]) |
| L4 Unintended | Harms, displacement, gaming, side-effects | See §5 | Throughout | More than a third of feedback interventions harmed performance (SRC-FGI-011, [SR]) |
| L5 Sustainability | Persistence of L1–L3 after active support ends | Same metrics after support or hypercare is withdrawn | 12 and 24 months | RE-AIM "Maintenance" (SRC-FGI-007); Proctor "sustainability" (SRC-FGI-005) |

**Interpretation rules:**

- L0 low → *not evaluable* for diagnosis quality. Report it as an execution finding.
- L0 adequate, L1 unchanged → the forward-chain assumption failed. This is evidence against the diagnosis or the control point.
- L1 moved, L2 unchanged → the propagation-to-manifestation link is wrong, or there is a delay. Check the expected lag stated in advance.
- L2 moved, L4 adverse → net benefit is uncertain. Report both, and do not net them silently.

---

## 3. Leading and lagging indicators

- **Leading** (L0, L1): sensitive, fast and close to the control point. They are also more gameable, because they sit close to the people being asked to change.
- **Lagging** (L2, L3): closer to what matters, but slower and confounded by external events.

Rules:

1. **Pair them.** Each top control point gets one leading indicator and one lagging indicator, plus a review date (lit-org-diagnosis O10, from DMAIC Control and BSC practice: SRC-BH-019, SRC-BH-012).
2. **Treat assumed links as hypotheses.** The causal link between a leading and a lagging indicator is a hypothesis, not a given. The Balanced Scorecard's assumed cause-effect chains were criticized for exactly this (SRC-BH-013, [SR]).
3. **Pre-register the expected lag.** State the lag between L1 and L2 in advance. People systematically misjudge delays (SRC-CDE-034, [SR]), and worse-before-better trajectories are expected in feedback systems (SRC-BH-022).
4. **Tie triggers to the causal model.** Alert triggers must come from the diagnosed causal model, not from generic variance or trend-break statistics. Generic early-warning signals give high false-alarm and miss rates (SRC-FGI-069, [SR]).

---

## 4. Non-target indicators and metric integrity

Indicators used for decisions attract corruption pressure (SRC-FGI-073 / SRC-CDE-063; Goodhart variants SRC-CDE-064). For every monitored control point:

- **Keep at least one non-target indicator.** It measures the same underlying construct by a different route and is not used to evaluate anyone. Example: if handle time is the target, track repeat-contact rate from system records, plus a customer-effort sample.
- **Run a metric-integrity check (OPP-05)** before using any metric as an outcome. Is it a target? Who is evaluated on it? Has its definition or collection changed? Is its relationship to the goal still intact?
- **Prefer independent records to self-report** for L2, while noting that records can also be gamed (OD1 authoring rule 3). The measurement source can reverse a conclusion (SRC-CDE-046, SRC-CDE-047, [SR]).
- **Interpret count metrics with care.** Read incident, complaint and error counts alongside a reporting-climate indicator. More reports can mean more willingness to report (SRC-CDE-041, [SR]).

---

## 5. Unintended-consequence monitoring

Write a pre-specified list per intervention, informed by the loop check (OPP-09) and the harm field (OPP-10c):

| Category | What to watch | Typical signal |
|---|---|---|
| Gaming | Target improves while the non-target corroborator does not | Divergence between AHT and repeat contacts; tickets closed and reopened |
| Displacement | Cost moves to adjacent units or later periods | Adjacent-unit backlog; downstream defect rates |
| Protective-control removal | A buffer or check removed as "waste" turns out to have prevented failure | Rise in rare-but-severe events |
| Concealment and chilling | Monitoring pushes deviations or voice underground (SRC-FGI-067, SRC-FGI-074) | Fewer workarounds surfaced in confidential interviews; falling free-text volume in surveys |
| Wellbeing | Stress and attrition from monitoring or pressure (SRC-FGI-060, [SR]) | Voluntary attrition, absence, stress items |
| Trust and response | Data collected without action (SRC-FGI-004) | Falling survey response rate across cycles |
| Distributional | Who bears the cost | Outcome breakdown by unit, role or shift (aggregate only) |

Every intervention has a named reviewer and a pre-registered harm threshold that triggers review or stopping (see `evaluation-design.md` §7).

---

## 6. Sustainability windows

There is no organizational evidence base for "the right" follow-up interval. These windows are pragmatic proposals to pre-register:

- **Short (0–3 months):** L0 and L1. Detects implementation failure early.
- **Primary (3–6 months):** L2 versus a comparison.
- **Medium (12 months):** L2 and L3 after active support ends. This is the first sustainability check.
- **Long (24 months):** L3 and L5 where feasible. Detects decay and late harms.

Rules:

- Record **when active support ended**, separately from when the intervention started. Sustainability is measured from the end of support.
- Record **co-interventions and external shocks** (reorganizations, market events) in a dated log. These are history threats (SRC-FGI-015).
- Watch for **drift back**. Practice can migrate back toward the boundary under efficiency pressure (SRC-CDE-048). A temporal baseline (OPP-19) shows when reversion began.

---

## 7. Measurement invariance and alpha / beta / gamma change

The three types of change (Golembiewski et al., SRC-FGI-075, [SR]):

- **Alpha:** real change on a stable instrument.
- **Beta:** respondents recalibrate the scale. For example, after training on psychological safety, staff rate the same climate more harshly.
- **Gamma:** respondents redefine the construct itself.

OD interventions are especially likely to induce beta and gamma change. An intervention that raises awareness can *lower* scores while the underlying reality improves.

Measurement invariance is the logical prerequisite for comparing groups or times. It is rarely tested, and it is tested through an ordered sequence of configural, metric and scalar steps (Vandenberg and Lance, SRC-FGI-076 / SRC-CDE-013, [SR]).

Procedure for any survey-based L1–L3 measure:

1. **Use established multi-item scales where possible**, not single items, so that invariance can be tested.
2. **Test invariance across time and across comparison groups** before interpreting mean change. If scalar invariance fails, report the change as *uninterpretable as alpha change*, not as improvement or deterioration (lit-longitudinal L4).
3. **Where samples are too small for formal testing**, use:
   - anchor items with behavioural referents ("in the last two weeks, how many times did you…"), which resist recalibration;
   - a retrospective pre-test ("then" rating collected at post) to estimate recalibration;
   - short qualitative probes about what respondents meant.
4. **Triangulate with non-survey indicators** (records, observation) before concluding that change was real.
5. **Check comparison groups too.** Invariance also matters between units with different cultures or languages (SRC-CDE-013).

---

## 8. Reactivity

Being diagnosed and measured can change behaviour. The classic Hawthorne findings were largely not supported on re-analysis, although subtler observation effects may exist (SRC-FGI-077, [SR]). Do not assume or dismiss reactivity. Estimate it where possible by comparing measured units with lightly measured comparison units, or by staggering measurement intensity.

---

## 9. Minimum measurement record per episode (checklist)

- [ ] Forward chain with named assumptions and expected lags (OPP-15)
- [ ] L0 fidelity checklist and adoption metric
- [ ] L1 mechanism indicator (leading)
- [ ] L2 manifestation indicator (primary), with comparison and threshold
- [ ] L3 outcome indicator (lagging), with windows
- [ ] At least one non-target indicator, plus a metric-integrity note
- [ ] Unintended-consequence list with harm thresholds and reviewer
- [ ] Invariance plan for any survey measure (formal test or anchor items)
- [ ] Dated log of support end, co-interventions and external shocks
- [ ] Aggregation level and minimum group size for any people data (`../scientific-foundations/longitudinal-assessment.md`)
