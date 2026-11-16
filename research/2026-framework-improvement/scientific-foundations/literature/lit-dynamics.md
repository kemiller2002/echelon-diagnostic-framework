# Workstream E — Organizational Dynamics

Status: staging draft (research input, not a framework change)
Retrieved: 2026-11-16
Scope: organizational mechanisms that (a) generate hidden problems and (b) distort the information a diagnostician receives — and what EDF must do to avoid over-relying on management accounts.
Source IDs refer to `sources-CDE.csv`. Verification: **fetched**, **search-result**, **from-memory-unverified** (flagged).

---

## 0. The core problem for an organizational diagnostic method

EDF v0.3 is domain-general: it was largely exercised on failure/success narratives where the evidence record already exists (accident reports, histories). In a live organizational diagnosis, the diagnostician's **evidence is produced by the system being diagnosed**. The same mechanisms that create the problem (incentives, silence, filtering, workarounds) also shape what the analyst is told. Management accounts are therefore not neutral observations but *outputs* of the system, and frequently its most filtered outputs.

The literature below supports treating "how information about this problem reaches (or fails to reach) decision-makers" as both a likely **origin** and a **threat to evidence validity**.

---

## 1. Key findings

### 1.1 Upward information filtering and silence

- **Rosen & Tesser (1970)** [SRC-CDE-058, search-result]: the **MUM effect** — people are reluctant to transmit bad news; good news was passed on more readily than bad. Later work reviewed in secondary sources finds the effect is robust across relationships but moderated by perceived valence.
- **Morrison & Milliken (2000)** [SRC-CDE-053, search-result]: **organizational silence** is a collective-level phenomenon — "powerful forces in many organizations … cause widespread withholding of information about potential problems"; origins include top managers' fear of negative feedback and implicit managerial beliefs; consequences include impaired change and error correction.
- **Milliken, Morrison & Hewlin (2003)** [SRC-CDE-054, search-result]: in an exploratory interview study, **85%** of respondents reported having felt unable to raise an important issue with a supervisor; withheld topics included supervisor/colleague competence and problems with organizational processes and performance. (Sample size reported only by a secondary source; not relied on.)
- **Detert & Edmondson (2011)** [SRC-CDE-060, search-result]: **implicit voice theories** — taken-for-granted beliefs about when speaking up is risky — have discriminant and incremental predictive validity for workplace silence, even for pro-organizational suggestions.
- **Tourish & Robson (2006)** [SRC-CDE-059, search-result]: managers' sensemaking shortcuts lead them to discount critical upward feedback and staff to withhold it; eliminating critical upward communication produces "iatrogenic" organizational problems.

Status: silence and bad-news reluctance are **empirically supported** (experiments, surveys, validated scales), though much of the organizational evidence is self-report and cross-sectional.

### 1.2 Status, perception, and the executive narrative

- **Starbuck & Mezias (1996)** [SRC-CDE-009, fetched]: summarising Payne & Pugh (1976), members' perceptions of their firm correlate weakly with objective measures, and "higher-status members generally have more favorable views of their organizations"; perceptual research is rare and methodologically hard.
- **Mezias & Starbuck (2003)** [SRC-CDE-010, search-result]: managers' perceptions are often very inaccurate.
- **Kahneman, Lovallo & Sibony (2011)** [SRC-CDE-024, fetched]: decision review should first ask whether there is "any reason to suspect motivated errors, or errors driven by the self-interest of the recommending team"; "an absence of dissent in a team addressing a complex problem should sound an alarm"; ask "where the numbers came from". Evidence: observational/practitioner.

Status: status-related optimism is supported but thin; self-interest checks are theoretically motivated practitioner guidance.

*EDF reading:* The "executive narrative" is a predictable artefact: a coherent, favorable, single-cause story from people who (a) receive filtered information, (b) hold more favorable views on average, and (c) have a stake in the diagnosis. EDF currently has no field identifying the commissioning party's stake or the hierarchical distribution of its evidence.

### 1.3 Work-as-done: workarounds and first-order problem solving

- **Tucker & Edmondson (2003)** [SRC-CDE-057, fetched]: 239 hours observing 26 nurses in nine hospitals. 86% of observed failures were *problems* (not errors). Nurses used "do what it takes to continue … no more, no less" for **93%** of problems, and then "neither communicated that it occurred to others nor sought to investigate or change causes". Only **7%** of responses met even lenient criteria for second-order problem solving (bringing it to someone who could fix it). **70%** of interviewed nurses believed their manager expected them to work through disruptions on their own; speaking up risked being seen as incompetent. First-order problem solving creates an "illusory equilibrium" in which managers do not see failures.
- **Alter (2014)** [SRC-CDE-069, search-result]: integrated **theory of workarounds** — workarounds are pervasive, under-theorised, and arise from the interaction of goals, obstacles, and the latitude to deviate; useful for explaining compliance/non-compliance and for systems analysis.
- **Repenning & Sterman (2001)** [SRC-CDE-065, fetched — abstract only]: improvement efforts fail through interaction with the surrounding physical, economic, social and psychological structures. (Secondary sources describe a "capability trap" in which firefighting erodes improvement capacity and managers attribute the result to worker motivation; that specific attribution claim was **not verified** from the paper text and is not relied on.)

Status: **empirically supported by direct observation** (Tucker & Edmondson is the strongest single source here because it does not depend on self-report).

*EDF reading:* The most important fact for EDF: **the frontline absorbs and conceals process failures by design**, so management accounts systematically under-represent them. A diagnosis that does not sample work-as-done (observation, logs, frontline interviews with protected confidentiality) will miss a large share of origins.

### 1.4 Reporting climate and psychological safety as evidence-quality moderators

- **Edmondson (1996)** [SRC-CDE-041, search-result]: better-led units reported *more* errors, interpreted as greater willingness to report.
- **Edmondson (1999)** [SRC-CDE-055, search-result]: in 51 manufacturing teams, team psychological safety was associated with learning behaviour, which mediated its relation to performance.
- **Frazier et al. (2017)** [SRC-CDE-056, search-result]: meta-analysis of 136 samples (>22,000 individuals, ~5,000 groups) mapping antecedents and outcomes; secondary summaries note most included studies were cross-sectional.

Status: association between psychological safety and voice/learning is **empirically robust but mostly correlational**; causal efficacy of interventions to raise it is less established.

*EDF reading:* Use psychological safety primarily as an **evidence-quality moderator** (how much should we trust the absence of reports from this unit?) rather than as a default origin or control point.

### 1.5 Incentives, agency, and metric corruption

- **Jensen & Meckling (1976)** [SRC-CDE-067, search-result]: agency costs arise from separation of principal and agent interests and require monitoring/bonding; information asymmetry is structural.
- **Kerr (1975)** [SRC-CDE-062, search-result]: organizations frequently reward behaviour A while hoping for B; illustrated with manufacturing and insurance reward systems. (A rejoinder, Boettger & Greer 1994, argues some such mismatches are deliberate — not separately verified.)
- **Campbell (1979)** [SRC-CDE-063, search-result]: source of "Campbell's law" (quantitative indicators used for social decision-making become subject to corruption pressures). **Note:** the exact passage was not retrieved (the open reprint returned an error); the attribution is standard but the quote is **unverified**.
- **Manheim & Garrabrant (2018)** [SRC-CDE-064, fetched — abstract]: Goodhart effects are the collapse of the statistical relationship between goal and proxy under optimization; distinguish regressional, extremal, causal and adversarial variants (names confirmed via search; definitions of the last two not verified from text). Risk grows with optimization pressure.
- **Goodhart (1975)** [SRC-CDE-071, **from-memory-unverified**]: original monetary-policy formulation; not used for any recommendation.

Status: incentive misalignment and metric corruption are **well-documented theory with many case illustrations**; systematic quantitative evidence on prevalence is mixed.

*EDF reading:* Every KPI cited as evidence in Step 3 (Manifestations) or Step 6 is also potentially a *target* someone is managed against. Metrics under pressure are both unreliable evidence and possible origins (gaming, displacement).

### 1.6 Drift and normalization of deviance

- **Vaughan (1996)** [SRC-CDE-061, search-result]: at NASA, repeated anomalies were incrementally redefined as acceptable — "normalization of deviance" — without rule-breaking; the standard explanation (managers knowingly overriding engineers) was incomplete.
- **Rasmussen (1997)** [SRC-CDE-048, search-result]: systematic migration toward the boundary of acceptable performance under efficiency and least-effort pressure.

Status: rich case-based and theoretical support; not experimentally testable in the usual sense.

*EDF reading:* Normalized deviance is invisible to insiders *because* it is normal. EDF needs a **temporal baseline** — compare current practice with the original design/standard and log when each accepted anomaly became accepted.

### 1.7 Informal networks

- **Cross, Borgatti & Parker (2002)** [SRC-CDE-066, search-result]: coordination increasingly runs through informal networks; social network analysis makes "invisible work visible" and reveals bottlenecks and fragmentation the org chart hides (based on work with 23 companies).

Status: practitioner-research synthesis; ONA as a measurement method is well established, its diagnostic value in specific cases is case-dependent.

*EDF reading:* EDF Propagation's "informational" and "organizational" paths should be traced through **actual** communication routes, not formal reporting lines.

### 1.8 Groupthink — weaker evidence than its reputation

- **Esser (1998)** [SRC-CDE-068, search-result]: review of 25 years of groupthink research; earlier review (Park 1990) found most studies reported only **partial support**.
- **Janis (1972/1982), *Groupthink*** [SRC-CDE-070, **from-memory-unverified**] — originating work; not used for recommendations.

Status: the full groupthink model is **weakly supported empirically**; individual elements (pressure toward consensus, suppressed dissent) have better support elsewhere.

*EDF reading:* Do not include "groupthink" as a named origin type. Instead, record observable process facts — was dissent present, was it recorded, how were alternatives handled (SRC-CDE-024).

---

## 2. Empirically supported vs theoretical — summary

| Claim | Status | Sources |
|---|---|---|
| Bad news is transmitted less readily than good news | Empirical (experimental) | SRC-CDE-058 |
| Employees widely withhold concerns from supervisors; implicit beliefs predict silence | Empirical (survey/interview; validated scales) | SRC-CDE-053, -054, -060 |
| Frontline workers resolve most process failures locally without upward reporting; managers don't see them | Empirical (direct observation) | SRC-CDE-057 |
| Higher-status members hold more favorable views; managers' perceptions often inaccurate | Empirical, thin/contested | SRC-CDE-009, -010 |
| Report counts depend on reporting climate | Empirical (small n) | SRC-CDE-041 |
| Psychological safety associates with voice/learning | Empirical, mostly correlational | SRC-CDE-055, -056 |
| Reward/target misalignment and metric corruption | Theory + cases | SRC-CDE-062, -063, -064, -067 |
| Normalization of deviance; drift to boundary | Case-based theory | SRC-CDE-061, -048 |
| Groupthink as a full model | Weak / partial support | SRC-CDE-068 |
| Self-interest and dissent checks improve decisions | Practitioner/observational | SRC-CDE-024 |

---

## 3. Concrete implications for EDF

**E1. Source-independence rule (Step 6; binding on Confidence and Ranking).**
No origin, propagation edge, or control-point *influence* claim may be rated High confidence if all supporting evidence comes from (a) one hierarchical level or (b) parties with a stake in the conclusion. Management accounts are always admissible but are recorded as *claims about the system* until corroborated by an independent group (frontline account, record, observation, external data). Rationale: SRC-CDE-009, -053, -057, -058, -059; aligns with C2 in Workstream C.

**E2. Commissioning-stake declaration (Step 1, System Context).**
Add a `Commissioning & stakes` sub-field: who requested the diagnosis, what outcome would be favorable/unfavorable to them, and which candidate origins implicate them. Rationale: SRC-CDE-024 (self-interest check), SRC-CDE-036 (political hijack of RCA).

**E3. Evidence-sampling plan across levels (between Steps 2 and 3, or within Step 6 at EDF-1/2).**
Before building the Origin Network, state which levels (executive, middle, frontline, adjacent teams, customers) and methods (interview, observation, records, telemetry) will be sampled, and whether frontline sources are protected. Record coverage gaps as Unknowns. Rationale: SRC-CDE-057, -007.

**E4. Workaround and silence probes (Step 3 Manifestations; Step 4 origin prompts).**
Required prompts: "What do people do to get work done despite the system?", "What problems are fixed locally and never reported?", "What would people not say to their manager here?" Workarounds are recorded both as *manifestations* and as *evidence of upstream origins*. Rationale: SRC-CDE-057, -069, -054.

**E5. Information-flow origin type and feedback check (Step 4; links to D4).**
Add *Informational* to origin types (alongside Technical, Human, Organizational, etc.): origins where a decision-maker's feedback is missing, filtered, delayed or distorted. For each top control point, ask whether the controller would currently learn that the problem exists. Rationale: SRC-CDE-053, -059, -042.

**E6. Metric-integrity check (Step 3 and Step 6).**
For every metric used as a manifestation or evidence: is it a target? who is evaluated on it? has its relationship to the underlying goal been checked recently? Classify suspected Goodhart variant. Rationale: SRC-CDE-062, -063, -064.

**E7. Reporting-climate adjustment for absence-of-evidence (Step 6; with C4).**
Interpret low report/complaint counts only alongside an indicator of reporting climate (psychological safety, voice behaviour, observed workaround prevalence). Rationale: SRC-CDE-041, -055, -056.

**E8. Temporal baseline / deviance log (Step 5 Propagation).**
For origins involving practice, compare current practice to the original standard or design intent and record when deviations became accepted. Rationale: SRC-CDE-061, -048.

**E9. Incentive map (Step 4, EDF-1/2).**
For each key actor: what is measured/rewarded, what is hoped for, where they diverge. Rationale: SRC-CDE-062, -067.

**E10. Dissent record (Step 6; Diagnostic Calibration extension).**
Record dissenting interpretations encountered and how they were resolved; flag "no dissent found" on a complex case as a warning, not reassurance. Do **not** label origins as "groupthink" (SRC-CDE-068). Rationale: SRC-CDE-024.

**E11. Control points must not depend on the filtered channel (Step 8).**
If a control point's effectiveness requires information to flow through a channel identified as filtered/silent, either pair it with a feedback-channel fix or downgrade its Controllability. Rationale: SRC-CDE-057, -053.

---

## 4. Testable hypotheses for the benchmark

All designed as targeted ablations on seeded cases, per the two-family results recommendation (harder, adversarial, multi-source cases; deterministic measures).

| ID | Hypothesis | Case design | Primary measure |
|---|---|---|---|
| H-E1 | **A source-independence rule (E1) reduces adoption of executive false narratives.** | Packet: executive interviews present a coherent, favorable, wrong narrative (e.g., "underperforming team skill gap"); frontline notes, ticket logs, or observation memos contain disconfirming evidence of the true origin (e.g., approval bottleneck) | Rate at which the false narrative is the top-ranked origin; true-origin recall |
| H-E2 | Workaround/silence probes (E4) increase recall of seeded hidden origins that appear only as workarounds in frontline material | Packet where the true origin appears only as an undocumented workaround | Hidden-origin recall; workaround-as-evidence citation rate |
| H-E3 | The metric-integrity check (E6) reduces the use of a gamed KPI as confirmatory evidence | Packet where an improving KPI is the result of gaming (e.g., tickets closed and reopened) | Rate of citing the gamed KPI as support; detection of gaming |
| H-E4 | The commissioning-stake declaration (E2) reduces recommendations of sponsor-preferred controls that are not linked to the diagnosed origins | Packet where the sponsor requests a specific fix implicating another unit | Sponsor-fix adoption rate absent supporting evidence |
| H-E5 | Reporting-climate adjustment (E7) reduces "low incidents ⇒ healthy unit" inferences | Two units; one with low reports and low safety indicators and high hidden failure | Misclassification of the silent unit |
| H-E6 | The temporal baseline (E8) increases detection of normalized deviance | Longitudinal packet where an anomaly was gradually accepted | Normalized-deviance identification rate |
| H-E7 | The effect of E1/E4 is larger on "management-filtered" cases than on cases with complete records (interaction) — i.e., these rules add value specifically where the two-family synthetic cases did not test | Factorial: filtered vs unfiltered packet × rule on/off | Interaction term on true-origin recall |
| H-E8 (cost/guardrail) | E1–E4 do not increase false-positive "hidden problem" claims on control cases where management accounts are accurate | Control packets where executive narrative is correct and frontline material agrees | False hidden-origin rate; over-skepticism rate |

H-E8 is essential: a framework that distrusts management by default could invent hidden problems. The benchmark must include cases where the management account is correct.

---

## 5. Limits

- Most organizational-dynamics evidence is correlational, self-reported, or case-based; Tucker & Edmondson (observation) and Rosen & Tesser (experiment) are the strongest designs here.
- Several sources verified only at abstract/summary level; specific numbers are reported only where fetched (Tucker & Edmondson) or present in search-confirmed abstracts (Milliken et al. 85%).
- LLM analyzers do not experience silence or status bias themselves, but they inherit it through the evidence packets; the benchmark hypotheses therefore test whether EDF rules make analyzers *correct for* biased inputs, not whether analyzers are biased.
