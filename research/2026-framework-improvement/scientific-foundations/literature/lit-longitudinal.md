# Literature Review (Longitudinal): Continuous Organizational Assessment

Workstream: Longitudinal. Prepared 2026-11-16 for the EDF framework-improvement review.
Source IDs refer to `sources-FGI.csv`. Verification labels: **fetched**, **search-result**, **from-memory-unverified** (none used here). Legal sources were found as commentary and secondary text, not as Official Journal copies. **This is not legal advice.**

## Scope and the EDF question

The operating cycle (Understand → Decide → Commit → Execute → Understand) implies repeated diagnosis. A natural extension is continuous assessment: pulse surveys, dashboards and people-analytics signals that trigger EDF. This review asks: what does continuous monitoring achieve, what harms does it cause, how do measures drift, and when is it defensible?

## 1. Surveys and pulse measurement

### Finding 1.1: Survey feedback improves outcomes only with action and follow-through
- Huebner and Zacher (2021, systematic review): evidence on survey follow-up is mixed. Benefits (morale, satisfaction, climate, sometimes turnover and absenteeism) depended on actual action planning, actionable items, trained and resourced managers, stakeholder involvement and fast feedback. Reporting quality was poor and publication bias is likely [SRC-FGI-004, fetched].
- **Type:** empirical (systematic review).
- **Implication:** A continuous survey without a committed response process is likely to be neutral at best. Each pulse cycle needs a defined owner and a response commitment. This maps onto the execution-layer fields Owner/Plan/Priority/Verification.

### Finding 1.2: Little rigorous evidence was found on optimal pulse frequency
- Rogelberg and Stanton (2007, *Organizational Research Methods*) is the standard reference on organizational survey nonresponse [SRC-FGI-070, search-result; findings not read]. Searches on pulse frequency and fatigue returned mostly vendor content with conflicting claims, which are not cited here as evidence.
- **Type:** gap.
- **Implication:** Any EDF cadence recommendation should be stated as a hypothesis to test, not as established practice.

## 2. Electronic monitoring and surveillance: effects

### Finding 2.1: Electronic performance monitoring raises stress and does not reliably improve performance
- Ravid et al. (2020, *Journal of Management*, "EPM 20/20"): a review and typology of electronic performance monitoring (EPM) characteristics (purpose, invasiveness, synchronicity and so on) [SRC-FGI-059, search-result].
- Ravid et al. (2023, *Personnel Psychology*; 94 samples, reported >23,000 participants): no evidence that EPM improved performance, a consistent link to higher stress, and highly variable effects by context. Secondary summaries report that more intense monitoring is associated with worse performance [SRC-FGI-060, search-result; effect sizes not read].
- Kalischko and Riedl (2021, *Frontiers in Psychology*): a review of EPM effects and moderators [SRC-FGI-061, search-result].
- **Type:** empirical (meta-analysis plus reviews).

### Finding 2.2: Observation can push behaviour into hiding and reduce performance
- Bernstein (2012, *Administrative Science Quarterly*): a field experiment at a large mobile-phone factory. Full observability led workers to conceal productive deviations. A modest increase in group-level privacy produced sustained, significant performance gains [SRC-FGI-067, search-result].
- **Type:** empirical (field experiment plus ethnography).
- **Implication:** For EDF, the core danger of continuous monitoring is *epistemic*, not only ethical. It hides exactly the informal workarounds and deviations that EDF's origin network and propagation steps need to see. Monitoring can make the diagnosis *worse*.

### Finding 2.3: Surveillance produces chilling effects
- Penney (2016, *Berkeley Technology Law Journal*): after the 2013 NSA revelations, views of privacy-sensitive Wikipedia articles fell immediately (about 30% per a Citizen Lab summary), with a lasting change in trend [SRC-FGI-074, search-result].
- **Type:** empirical (interrupted time series). The context is public online behaviour, not workplaces, so treat it as mechanism evidence.
- **Implication:** Expect monitored employees to self-censor in survey free text and in channels the organization can see. This biases continuous signals toward the "safe" answer.

### Finding 2.4: Measurement reactivity is real but often overstated; Hawthorne lore is partly fictional
- Levitt and List (2011, *AEJ: Applied*) re-analysed the original Hawthorne illumination data. The famous patterns were "entirely fictional", though there were subtler signs of possible observation effects [SRC-FGI-077, search-result].
- **Type:** empirical (re-analysis).
- **Implication:** Do not cite "the Hawthorne effect" as settled. Reactivity should be measured, for example by comparing monitored and unmonitored units.

## 3. Indicators under pressure and measurement drift

### Finding 3.1: Indicators used for decisions get corrupted (Campbell's law)
- Campbell (1979, *Evaluation and Program Planning*): the more a quantitative social indicator is used for decisions, the more it is subject to corruption pressure, and the more it distorts the process it is meant to monitor [SRC-FGI-073, search-result; exact wording is from a secondary source].
- **Type:** theoretical/observational, with broad later support (not reviewed here).
- **Implication:** If EDF control points become KPI targets, the indicators that confirm the diagnosis will drift away from the reality they track. The validation loop needs at least one indicator that is *not* a target.

### Finding 3.2: Longitudinal comparisons need measurement invariance
- Golembiewski, Billingsley and Yeager (1976): change on a survey scale can be alpha (true change), beta (respondents recalibrate the scale) or gamma (respondents redefine the construct). OD interventions in particular can cause beta and gamma change [SRC-FGI-075, search-result].
- Vandenberg and Lance (2000, *Organizational Research Methods*): measurement invariance is a prerequisite for comparing across groups or time, is rarely tested, and is tested through an ordered sequence of steps [SRC-FGI-076, search-result].
- **Type:** methodological, with empirical demonstrations.
- **Implication:** An intervention that raises awareness (for example, of psychological safety) can *lower* survey scores through beta change while the underlying reality improves. EDF's "Understand again" step must not read pre/post scale differences as alpha change without checking. This directly affects the "Did the intervention change the manifestation?" question.

### Finding 3.3: Generic early-warning signals give false alarms
- Scheffer et al. (2009, *Nature*) proposed generic early-warning signals of critical transitions (critical slowing down, rising variance and autocorrelation) [SRC-FGI-068, search-result].
- Boettiger and Hastings (2012, *J. R. Soc. Interface*) showed that common tests of these indicators are "inappropriate and insufficient", with substantial false-alarm and miss rates. They argued against relying on "generic" signals over system-specific models [SRC-FGI-069, search-result].
- **Type:** theoretical plus simulation, from ecology.
- **Implication:** Dashboards that alert on generic statistical shifts (variance, trend breaks) in people metrics will produce false alarms. Early-warning triggers for EDF should be tied to a stated causal model (the origin network from a prior EDF), with false-alarm rates estimated before deployment.

## 4. People analytics: claims and ethics

- Tursunbayeva, Di Lauro and Pagliari (2018, *IJIM*): a scoping review of people-analytics definitions and vendor value propositions. Ethical issues were largely absent from the literature reviewed [SRC-FGI-071, search-result]. The same team later published on the ethics of people analytics in *Personnel Review* (2021) [SRC-FGI-072, search-result].
- **Type:** scoping review (descriptive). Neither source establishes effectiveness.
- **Implication:** Vendor claims are not evidence of diagnostic validity.

## 5. Legal and regulatory constraints (EU/UK focus)

- **GDPR Art. 88:** Member States may set more specific rules for employment processing, with "suitable and specific measures", paying particular attention to transparency and to *monitoring systems at the work place* [SRC-FGI-064, search-result].
- **WP29 Opinion 2/2017 (WP249) on data processing at work:** consent is rarely a valid basis because of the power imbalance. Employers should involve a representative sample of employees in designing monitoring rules. Commentary also notes recommendations for DPIA, minimisation and least-intrusive means [SRC-FGI-063, search-result].
- **UK ICO "Monitoring workers" guidance (October 2023):** per law-firm commentary, monitoring must be necessary, proportionate and transparent. A DPIA is needed where monitoring is likely high-risk (for example covert, continuous or extensive). Continuous audio/video is rarely justified. Consent is usually unsuitable [SRC-FGI-062, search-result; secondary commentary only].
- **EU AI Act Art. 5(1)(f):** since 2 February 2025, AI systems that infer emotions of natural persons in the workplace are prohibited, except for medical or safety purposes. Commission guidance reportedly reads this to cover intention inference too [SRC-FGI-065, search-result].
- **EU AI Act Annex III point 4 (employment and workers management):** AI used for decisions affecting work relationships, task allocation, and monitoring and evaluating performance and behaviour is high-risk. Sources disagree on sub-point labelling and on application dates (August 2026 vs December 2027 are both cited), so **check the Official Journal text and current Commission guidance** [SRC-FGI-066, search-result].
- **Implication:** An AI-assisted continuous EDF that scores individuals' behaviour or sentiment is likely to fall into the high-risk category, or into prohibited emotion inference. A continuous EDF that works at **aggregate/system level**, uses data people provide knowingly, and feeds human diagnosis rather than individual decisions is much easier to defend.

## 6. When is continuous monitoring defensible? (synthesis)

Combining the empirical and regulatory findings, continuous organizational assessment is most defensible when **all** of these hold:

1. **Purpose limitation:** it monitors a specific outcome or control point from a prior EDF, with a stated causal model. It is not open-ended surveillance (SRC-FGI-069, -063, -062).
2. **System-level unit of analysis:** aggregated with minimum group sizes, with no individual scoring or emotion inference (SRC-FGI-065, -066).
3. **Least-intrusive signal:** existing operational data or voluntary input, used before passive behavioural capture (SRC-FGI-062, -063, -060).
4. **Response commitment:** each signal has an owner and a response process. Data collected without action is not neutral (SRC-FGI-004).
5. **Non-target indicators:** at least one tracked indicator is not a performance target (SRC-FGI-073).
6. **Drift checks:** measurement invariance or anchor items are checked before change is interpreted (SRC-FGI-075, -076).
7. **Trigger validation:** alert thresholds have estimated false-alarm rates (SRC-FGI-069).
8. **Participation and transparency:** employee representatives help design it, and a DPIA is done where required (SRC-FGI-063, -062, -064).
9. **Sunset clause:** monitoring ends or is re-justified when the control point is resolved.

## 7. Consolidated proposed changes to EDF (Longitudinal)

| # | Change | Where | Evidence basis |
|---|--------|-------|----------------|
| L1 | Add an optional **"Monitoring Plan"** output, tied to the ranked control points: indicator, level of aggregation, cadence, owner, response rule, trigger with estimated false-alarm rate, sunset date. | Required outputs (optional extension) | SRC-FGI-004, -069 |
| L2 | Add a **Proportionality gate** before any continuous data collection: purpose, least-intrusive alternative, aggregation, legal check (DPIA, Art. 88 national rules, AI Act classification), employee participation. | Operating cycle | SRC-FGI-062, -063, -064, -065, -066 |
| L3 | Prohibit individual-level behavioural or emotion scoring as EDF evidence. | Step 6 | SRC-FGI-065, -060, -066 |
| L4 | "Understand again" must check for **beta/gamma change** (anchor items, invariance testing, or qualitative probes) before reading scale changes as real. | Validation loop | SRC-FGI-075, -076 |
| L5 | Keep at least one **non-target indicator** per monitored control point, to detect Campbell's-law distortion. | Validation loop | SRC-FGI-073 |
| L6 | Treat **observation effects as a propagation path**: list in step 5 how the act of monitoring may change what is observed (concealment, chilling). | Step 5 | SRC-FGI-067, -074, -077 |

## 8. Testable benchmark hypotheses

- **H-L1 (model-tied triggers):** In historical organizational time series with known adverse events, triggers derived from a prior EDF causal model will have a lower false-alarm rate, at equal sensitivity, than generic statistical-change triggers.
- **H-L2 (response commitment):** Units where pulse results come with an owner-led response (L1) will show stable or rising response rates over 4+ cycles. Units without a committed response will show falling response rates. (Prediction derived from SRC-FGI-004; untested at cadence level.)
- **H-L3 (beta change):** After awareness-raising interventions, a share of measured score declines will show non-invariance (beta/gamma change) under formal testing. L4 checks will reclassify these as "uninterpretable" rather than "deterioration".
- **H-L4 (aggregate vs individual):** For system-level diagnosis questions, aggregate-level data will produce top-control-point agreement non-inferior to individual-level data, with fewer privacy risks. This tests whether the extra intrusion buys diagnostic value.
- **H-L5 (concealment):** Where monitoring intensity rises, the number of informal workarounds surfaced in confidential EDF interviews will fall relative to comparable less-monitored units (testing the Bernstein mechanism for EDF evidence quality).

## 9. Limitations

- Legal content comes from commentary and secondary texts. Verify against primary legal texts and national law before relying on it.
- Pulse-survey cadence evidence is weak. Most material found was vendor-produced and was excluded.
- The early-warning literature is from ecology. Transfer to organizations is by analogy.
- Ravid et al. (2023) effect sizes were not read directly.
