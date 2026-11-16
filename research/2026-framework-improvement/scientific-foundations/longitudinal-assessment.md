# Longitudinal Assessment: Continuous vs One-Time Organizational Diagnosis

Mission EDF-IMPROVEMENT-2026-001 · Baseline EDF v0.3 (`41fea6f`) · 2026-11-16

Primary input is `literature/lit-longitudinal.md`, supplemented by lit-dynamics, lit-intervention and lit-validity, and by `../baseline/*.md`. Source ids refer to `../evidence/source-register.csv`. A **[SR]** tag marks a detail that rests only on search-result sources. Legal sources are secondary commentary, and nothing here is legal advice; check primary texts and local law before relying on them.

This file reports no experimental results. See experiments/ and strategy/ for measured results (EX-EDF-2026-OD1).

---

## 1. Why the question arises

EDF's operating cycle (Understand → Decide → Commit → Execute → Understand) implies repeated diagnosis (`../baseline/methodology-inventory.md` §17). The obvious extension is continuous assessment: pulse surveys, dashboards and people-analytics signals that trigger an EDF.

EDF v0.3 has no longitudinal machinery. The relevant baseline gaps are:

- Evidence-updating is untested (`../baseline/current-limitations.md` rank 19).
- The representation is static, so state changes get compressed (rank 20).
- Nothing governs monitoring data, aggregation or privacy (`../baseline/diagnostic-taxonomy.md` §3.2, "Confidentiality / sensitivity").

---

## 2. What the evidence says

| Finding | Type / strength | Sources | Consequence for continuous EDF |
|---|---|---|---|
| Survey feedback helps only with action planning and follow-through | Systematic review; moderate (fetched) | SRC-FGI-004 | A pulse with no response process is neutral at best and costs trust |
| There is little rigorous evidence on optimal pulse frequency or survey fatigue; most material found was vendor content | Gap | SRC-FGI-070 (reference only; findings not read) | Any cadence is a hypothesis to test |
| Electronic performance monitoring (EPM) is linked to higher stress, shows no reliable performance gain, and effects vary by context | Meta-analysis, 94 samples; moderate [SR] | SRC-FGI-060, SRC-FGI-059, SRC-FGI-061 | Intrusive monitoring is costly and does not reliably help |
| Full observability led workers to conceal productive deviations; modest group privacy raised performance | Field experiment; moderate [SR] | SRC-FGI-067 | **Monitoring can blind the diagnosis.** It hides the workarounds EDF most needs to see (SRC-CDE-057) |
| Surveillance produces chilling effects | Interrupted time series, non-workplace; mechanism evidence [SR] | SRC-FGI-074 | Expect self-censorship in visible channels and in free text |
| Hawthorne lore is largely unsupported; reactivity should be measured | Re-analysis [SR] | SRC-FGI-077 | Neither assume nor dismiss reactivity |
| Indicators used for decisions become corrupted | Theory plus cases | SRC-FGI-073, SRC-CDE-064 | Continuous KPIs used to steer drift away from reality |
| Survey change can be alpha, beta or gamma; invariance must be tested before comparing over time | Methodological [SR] | SRC-FGI-075, SRC-FGI-076 | A falling score after an awareness intervention may be recalibration, not deterioration |
| Generic early-warning signals have high false-alarm and miss rates | Simulation, ecology [SR] | SRC-FGI-068, SRC-FGI-069 | Alerts should come from a stated causal model, with estimated false-alarm rates |
| The people-analytics literature largely omits ethics; vendor claims are not validity evidence | Scoping review [SR] | SRC-FGI-071, SRC-FGI-072 | Do not import vendor "signals" as diagnostic evidence |
| Report counts reflect reporting climate | Small n [SR] | SRC-CDE-041 | Trend lines in incident or complaint counts need a climate covariate |
| Normalization of deviance and drift are gradual and invisible to insiders | Case-based | SRC-CDE-061, SRC-CDE-048 | **One argument *for* longitudinal assessment:** a temporal baseline can reveal drift a snapshot cannot |

### Regulatory constraints (EU/UK focus; secondary sources)

- **GDPR Art. 88:** suitable and specific measures for employment processing, with particular attention to workplace monitoring (SRC-FGI-064).
- **WP29 Opinion 2/2017:** consent is rarely a valid basis at work. It recommends employee involvement, a DPIA, data minimisation and the least-intrusive means (SRC-FGI-063).
- **UK ICO guidance (2023):** monitoring must be necessary, proportionate and transparent. A DPIA is required for high-risk monitoring, and continuous audio/video is rarely justified (SRC-FGI-062; law-firm commentary only).
- **EU AI Act Art. 5(1)(f):** AI inference of emotions at work has been prohibited since 2 February 2025, except for medical or safety purposes (SRC-FGI-065).
- **EU AI Act Annex III(4):** AI used for monitoring and evaluating workers is high-risk. Sources disagree on application dates, so confirm against the Official Journal (SRC-FGI-066).

---

## 3. One-time vs continuous: when each is defensible

| Situation | One-time (episodic) EDF | Continuous or periodic assessment |
|---|---|---|
| Discrete problem with a clear manifestation | **Default.** Diagnose, act, re-assess at pre-set windows (`../intervention-effectiveness/outcome-measurement.md` §6) | Not justified beyond the monitoring plan for the chosen control point |
| Validating an intervention (the "Understand again" step) | Insufficient: needs a comparison over time | **Defensible.** Time-limited, tied to the control point, with a sunset date |
| Suspected gradual drift or normalized deviance | Snapshot misses drift | **Defensible.** Uses low-intrusion operational records and anchor items, against a stated baseline |
| Chronic, feedback-driven problem (capability trap, attrition spiral) | May mistake a phase for the trend | **Defensible.** Needs time series to see loops and delays (SRC-CDE-034, SRC-BH-022) |
| General "organizational health" surveillance with no prior diagnosis | Not applicable | **Not defensible as EDF.** It has no causal model, generic triggers produce false alarms (SRC-FGI-069), and it risks chilling and concealment |
| Individual-level behavioural or sentiment scoring | Not applicable | **Out of scope for EDF.** It is the highest-risk legal category and hides the evidence EDF needs |

---

## 4. Conditions for defensible continuous assessment

All of the following must hold. This synthesis follows lit-longitudinal §6 and is the basis of OPP-20.

1. **Purpose limitation.** The assessment monitors a specific outcome or control point from a prior EDF, with a stated causal model. It is not open-ended (SRC-FGI-063, SRC-FGI-062, SRC-FGI-069).
2. **System-level unit of analysis.** Data are aggregated with minimum group sizes. There is no individual scoring and no emotion or intention inference (SRC-FGI-065, SRC-FGI-066).
3. **Least-intrusive signal.** Existing operational records or voluntary input come before any passive behavioural capture (SRC-FGI-060, SRC-FGI-062).
4. **Response commitment.** Each signal has an owner, a response rule and a feedback loop to the people who provided the data (SRC-FGI-004).
5. **Non-target indicator.** At least one tracked indicator is not a performance target (SRC-FGI-073).
6. **Drift and invariance checks.** Invariance tests or anchor items are run before change is interpreted (SRC-FGI-075, SRC-FGI-076).
7. **Trigger validation.** Each alert threshold has a false-alarm rate estimated before deployment, and is derived from the causal model, not from generic statistics (SRC-FGI-069).
8. **Participation and transparency.** Employee representatives take part in the design. A DPIA is done where required, and data subjects are told what is collected and why (SRC-FGI-063, SRC-FGI-062, SRC-FGI-064).
9. **Sunset clause.** Monitoring ends, or is re-justified, when the control point is resolved.
10. **Observation-effect accounting.** Step 5 (Propagation) lists how monitoring itself may change what is observed: concealment, chilling and gaming (SRC-FGI-067, SRC-FGI-074; lit-longitudinal L6).

---

## 5. Surveillance and privacy risks specific to diagnosis

- **Epistemic risk.** Monitoring pushes informal workarounds and voice underground (SRC-FGI-067, SRC-FGI-074). The organization then looks healthier on the signals while its diagnosability falls. This is the same "illusory equilibrium" mechanism seen at the frontline (SRC-CDE-057).
- **Silence amplification.** Visible channels attract socially desirable answers (SRC-CDE-011, [SR]). Implicit voice theories predict withholding where speaking up is risky (SRC-CDE-060, [SR]).
- **Function creep.** Data gathered for system diagnosis gets reused for individual evaluation. Legally this moves into high-risk territory (SRC-FGI-066), and practically it destroys candour.
- **Re-identification.** Small units and free text can identify individuals even in "aggregated" data. Set minimum group sizes and suppress small cells.
- **AI-specific risk.** LLM summarisation of employee free text drifts toward sentiment or intention inference, which is prohibited at work in the EU (SRC-FGI-065). Keep analysis at the level of topics and conditions.

---

## 6. Measurement drift: what to watch over time

| Drift type | Mechanism | Safeguard |
|---|---|---|
| Beta / gamma change | Respondents recalibrate or redefine the construct, especially after OD interventions (SRC-FGI-075) | Invariance testing (SRC-FGI-076); behavioural anchor items; retrospective pre-tests |
| Indicator corruption | The monitored metric becomes a target (SRC-FGI-073, SRC-CDE-064) | Non-target corroborator; periodic metric-integrity check (OPP-05) |
| Instrument and definition change | Systems, coding rules or survey items change | Dated change log; parallel running across definition changes |
| Response-rate decay and composition shift | Fatigue, or loss of trust when no action follows (SRC-FGI-004) | Track response rate and composition; reweight or flag; honour response commitments |
| Reporting-climate shift | Better climate raises report counts (SRC-CDE-041) | Climate covariate alongside counts |
| Practice drift | Gradual normalization (SRC-CDE-061, SRC-CDE-048) | Temporal baseline against the original standard (OPP-19) |
| Reactivity | Behaviour changes because it is measured (SRC-FGI-077) | Compare with lightly measured units where feasible |

---

## 7. Recommendation boundaries

These are evidence-based limits on what EDF should claim or do. They are not product decisions.

- **EDF may** define an optional *Monitoring Plan* output tied to ranked control points: indicator, aggregation level, cadence, owner, response rule, trigger with an estimated false-alarm rate, and sunset date (lit-longitudinal L1). It may also require a proportionality gate before any recurring collection (OPP-20).
- **EDF should not** claim that continuous assessment improves diagnosis or outcomes. No evidence was found for that claim in organizations. Pulse cadence evidence is essentially absent (SRC-FGI-070 reference only).
- **EDF should not** use individual-level behavioural, emotion or intention scoring as evidence (SRC-FGI-065, SRC-FGI-060, SRC-FGI-066).
- **EDF should not** interpret pre/post survey change as real change without an invariance or anchor check (SRC-FGI-075, SRC-FGI-076).
- **EDF should treat** a generic dashboard alert as a prompt to run an EDF-0 triage (OPP-13), not as a diagnosis (SRC-FGI-069).
- **Cadence** recommendations must be framed as hypotheses to test, for example H-L2: units whose pulse results receive an owner-led response keep their response rates; units without one decline.

### Testable hypotheses (from lit-longitudinal §8)

These are field hypotheses and cannot be tested in OD1:

- **H-L1:** Triggers derived from the causal model have fewer false alarms than generic triggers at equal sensitivity.
- **H-L2:** A response commitment sustains response rates.
- **H-L3:** Some measured declines after awareness interventions are non-invariant, i.e. reflect beta or gamma change rather than real decline.
- **H-L4:** Aggregate data are non-inferior to individual data for system-level diagnosis.
- **H-L5:** Higher monitoring intensity reduces the number of workarounds surfaced in confidential interviews.
