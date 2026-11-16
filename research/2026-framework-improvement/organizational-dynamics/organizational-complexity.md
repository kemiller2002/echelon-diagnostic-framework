# Organizational Complexity: Hidden Dependencies, Informal Decision-Making and Dynamics

Mission EDF-IMPROVEMENT-2026-001 · Baseline EDF v0.3 (`41fea6f`) · 2026-11-16
Inputs: `../scientific-foundations/literature/lit-causal.md`, `lit-dynamics.md`, `lit-org-diagnosis.md`, `../scientific-foundations/competing-methodologies.md`; `../baseline/*.md`; `../benchmarks/benchmark-specification.md`. Candidate mechanisms (OPP-xx) are defined in `../scientific-foundations/methodological-opportunities.md`. "[SR]" marks a detail that rests on search-result-only sources.

No experimental results are reported here. See experiments/ and strategy/ for measured results (EX-EDF-2026-OD1).

Related documents: `stakeholder-bias.md` covers leadership-unrecognized problems, perception vs verification, management interviews and silence. `incentive-analysis.md` covers incentives, local optimization, metric concealment and tradeoffs.

## Core premise

Organizational outcomes are often produced by:

- **dependencies that no one owns**, such as shared services, interfaces and handoffs;
- **decisions made outside the formal structure**, such as informal vetoes, brokers and workarounds;
- **feedback structures** that sustain a problem after its original trigger is gone.

EDF v0.3 represents an organization as an origin network with acyclic propagation by medium (Physical, Informational, Organizational, Financial, Human). It has no feedback, no delays, no interface category and no informal structure (`../baseline/diagnostic-taxonomy.md` §1.3, §2.2; `../baseline/current-limitations.md` rank 20).

## OD1 coverage

The benchmark is specified in `../benchmarks/benchmark-specification.md`. Hidden dependencies and informal decision-making have no dedicated tags. They are tested indirectly through four routes:

- **Class D** (conflicting stakeholder explanations). Each party's story places the cause in another party's domain.
- **Class I** (green dashboards and missed dates). Several divisions share one Platform Engineering team, a classic shared dependency.
- **Class K** (non-existent or intentional). This class guards against inventing hidden structure.
- **Dynamics and harm tags:** `feedback-loop` (dev F), `local-optimization`, `side-effect`, `iatrogenic-intervention` and `confounded`.

**Limitation.** Open-ended discovery of a dependency that no candidate item names can only be probed in the open-generation arm (OD1-G). There, grader mapping is the main validity threat (spec §8).

---

## Q1. Can EDF detect hidden dependencies?

**What EDF v0.3 does.**

- Origin types have no *structure / interfaces / coordination* category. The only trace is the topology label "Interface-centered propagation", which has no case file behind it (`diagnostic-taxonomy.md` §1.3; `current-limitations.md` rank 31).
- Origin links are not typed as jointly necessary (AND) or independently sufficient (OR). This matters for which control points actually prevent recurrence (`../scientific-foundations/competing-methodologies.md`, "Where competitors outperform EDF" item 5).
- Propagation is classified by medium, not by mechanism. There is no type for a blocked or delayed path (`diagnostic-taxonomy.md` §2.2).

**What the literature says.**

- Coordination increasingly runs through informal networks. ONA reveals bottlenecks and fragmentation that the org chart hides (SRC-CDE-066 / SRC-BH-036, practitioner research, [SR]). Network position relates to performance (SRC-BH-037, meta-analysis, [SR]).
- Process mining reconstructs the actual process from event logs. Conformance checking exposes deviations from the designed process (SRC-BH-034, fetched; SRC-BH-035). It is technically validated for process reconstruction, not for organizational diagnosis.
- Fault-tree AND/OR semantics and minimal cut sets make joint dependence explicit (SRC-BH-030). Fault trees map to Bayesian networks, which support backward inference (SRC-CDE-033, [SR]).
- Constrained control-structure representations raised inter-analyst reliability in two studies (SRC-CDE-050, SRC-CDE-051, [SR]). This is suggestive evidence that explicit structure reduces the number of dependencies that go unnoticed.

**Proposed mechanism.**

- **AND/OR origin links and an interface prompt (OPP-06, lit-org-diagnosis O5).** Ask: which handoffs, shared resources or interfaces does the outcome pass through, and who owns each?
- **Control-structure fields for each control point (OPP-08).** These expose controllers who depend on resources they do not control.
- **Relational and event-log evidence as source types (OPP-18).** Examples are ONA and conformance checking. Use them only where the proportionality conditions in OPP-20 hold.
- **Optional formalisation at EDF-2.** A Bayesian-network or fault-tree export where quantitative data exist (lit-causal D10).

**How OD1 tests it.**

- Class I hypotheses and edges that place the cause in a shared dependency versus in the requesting division.
- Tags `confounded` and `causal-overclaim-trap` on edges that cross units.
- Metrics: causal accuracy, causal overclaim rate, and evidence-request value, i.e. whether the analyzer requests the cross-unit queue or allocation records.

---

## Q2. Can EDF detect informal decision-making?

**What EDF v0.3 does.**

- Informal power, politics and authority structure are absent from the taxonomy. Challenger's "engineering authority" is treated as formal governance (`diagnostic-taxonomy.md` §1.3).
- Origin coding has no rule for the person-versus-system boundary, so an individual act inside an informal norm is coded inconsistently (§1.2).
- The control-point ranking omits political feasibility and sponsorship (§4.2).
- Who actually decides is invisible to the method.

**What the literature says.**

- Workarounds are a form of informal decision-making. They are pervasive and arise from goals, obstacles and latitude to deviate (SRC-CDE-069, theoretical). Frontline staff decide locally to "do what it takes" and do not escalate (SRC-CDE-057, fetched).
- Normalization of deviance happened at NASA without rule-breaking. Anomalies were redefined as acceptable through informal, incremental judgments (SRC-CDE-061, case-based).
- Weisbord's Six-Box model explicitly probes the gap between the formal and the informal system (SRC-BH-006, theoretical; face validity only, SRC-BH-007).
- Diagnosis is itself political. Investigations can be hijacked (SRC-CDE-036, [SR]), and fixes follow factors other than the findings (SRC-CDE-044, [SR]).
- Groupthink is not a reliable explanatory label (SRC-CDE-068). Record observable process facts instead: who was present, whether dissent was raised, and how alternatives were handled (SRC-CDE-024).

**Proposed mechanism.**

- **Decision-rights probe (OPP-06).** Ask: who actually made or blocked the decisions that shaped the outcome, and does that match the formal decision rights? Record the gaps as origins of the type *informal authority*.
- **Process model plus local-rationality record (OPP-08, OPP-19).** Record what each controller believed and what pressures they faced.
- **Political feasibility and authority (OPP-10(d)).** Separate *authority* from *implementability* in the control-point ranking.
- **Dissent record (lit-dynamics E10).**

**How OD1 tests it.** Only indirectly:

- items where the formal account conflicts with what staff or records show people actually did (`staff-narrative`, `hidden-problem`);
- action items where a formally sanctioned fix is `harmful` because it would disable an informal protective practice (tag `protective-control`; present in dev A, C, E, G, I and K).

A dedicated informal-authority probe would need a new case class.

---

## Q3. Feedback loops, delays and drift

This is not a mission question in its own right, but Q1 and Q2 depend on it.

**What EDF v0.3 does.** The representation is static and acyclic. The Apollo 13 analysis showed that the schema compresses repeated state changes (FM-010; `current-limitations.md` rank 20). Step 8 ranks control points without asking how the system will respond.

**What the literature says.**

- Misperception of feedback and delays is experimentally supported (SRC-CDE-034, [SR]).
- Policy resistance (SRC-CDE-035) and drift toward the boundary (SRC-CDE-048) are theory with case support.
- The capability trap is supported by field cases and a model (SRC-BH-022): firefighting erodes improvement capacity.
- Organizational outcomes often come from feedback structure, not single causes (lit-org-diagnosis §3.4).

**Proposed mechanism.** OPP-09:

- allow cycles in Propagation;
- ask which loop maintains the outcome;
- for each top control point, ask about compensating behaviour, displacement, gaming and worse-before-better.

OPP-19 adds a temporal baseline for drift.

**How OD1 tests it.** Tag `feedback-loop`, heaviest in dev F (on-call exhaustion and rising incidents), plus `iatrogenic-intervention` and `side-effect`. Class H's staged evidence (`asOfStage`) tests revision as the state evolves.

---

## Q4. Boundary-setting under complexity

**What EDF v0.3 does.**

- The Context Principle ("valid only within its declared System Context") has no rule for who sets the boundary or how it is challenged.
- In every R1 validation case, Focus was "Varied by lens", so the boundary was never fixed (`methodology-inventory.md` §2, §4).

**What the literature says.**

- The investigation model determines what is found (SRC-CDE-043, [SR]).
- Multiframe diagnosis deliberately applies several frames (SRC-BH-055, theoretical).
- Cynefin argues that in complex situations cause and effect are coherent only in retrospect, and recommends probe-sense-respond over further analysis (SRC-BH-049, SRC-BH-050, theoretical and unvalidated).
- This is consistent with the moderate validity ceiling of retrospective systemic analysis (SRC-BH-067).

**Proposed mechanism.**

- **Boundary challenge (OPP-02).** Name the adjacent systems excluded from scope, and who would draw the boundary differently.
- **Rival networks (OPP-03).** Rivals may differ in boundary as well as in cause.
- **Reversible probe (OPP-11).** Where the situation is judged complex, a safe-to-fail probe is a legitimate next responsible action.

**How OD1 tests it.**

- The SUFF item and the `premature` action answers. In complex cases, committing to a substantive restructuring should be scored premature, while an evidence-gathering or probe action is preferred.
- Classes D and I, where the requester's boundary excludes the true origin.

---

## Cautions

- **Complexity tooling has real costs.** ONA, process mining and system-dynamics modelling are moderate-to-high cost and carry privacy risk (`competing-methodologies.md` E1, E3, C6). Reserve them for EDF-2, and only when the triage stage (OPP-13) flags a cross-unit or dynamic pattern.
- **More structure is not more accuracy by default.** The ACH result (SRC-BH-072) and the CoT result (SRC-FGI-039) warn that added representational burden can raise inconsistency. AND/OR typing, loops and control-structure fields should each be ablated separately.
- **Synthetic limits.** OD1 cases are written and analyzed by Claude-family agents (spec §7). Hidden dependencies in real organizations are often unknown to everyone, including whoever would write the benchmark. Only field studies can test discovery in that sense.
