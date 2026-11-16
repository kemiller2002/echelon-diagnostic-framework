# Incentives, Metrics, Local Optimization and Intentional Tradeoffs

Mission EDF-IMPROVEMENT-2026-001 · Baseline EDF v0.3 (`41fea6f`) · 2026-11-16
Inputs: `../scientific-foundations/literature/lit-dynamics.md`, `lit-causal.md`, `lit-longitudinal.md`, `lit-intervention.md`; `../baseline/*.md`; `../benchmarks/benchmark-specification.md`. Candidate mechanisms (OPP-xx) are defined in `../scientific-foundations/methodological-opportunities.md`. "[SR]" means search-result-only support.

No experimental results are reported here. See experiments/ and strategy/ for measured results (EX-EDF-2026-OD1).

## Core premise

In organizations, much behaviour that looks like dysfunction is a rational response to what is measured and rewarded. Some of it is not dysfunction at all: it is a deliberate choice whose costs are known.

EDF's origin model defines an origin as "a point where a contributing cause began" (`../baseline/diagnostic-taxonomy.md` §1.3, citing `model.md:79`). That definition presumes a defect. EDF also treats metrics as valid manifestations if they are observable (`../baseline/current-limitations.md` rank 12).

The two gaps interact. An analyst can diagnose a gamed metric as a real improvement, and diagnose a deliberate tradeoff as a failure that needs fixing.

## OD1 coverage

From `../benchmarks/benchmark-specification.md` and the dev case titles:

| Class | What it tests here | Example |
|---|---|---|
| E | An incentive pilot and its proposed rollout | Average-handle-time (AHT) bonus; AHT fell while customer satisfaction was reported flat |
| I | Metrics concealing deterioration | Division at "95% project health" while missing customer dates; head proposes moving 12 shared engineers under their line |
| D | Conflicting explanations with metric artefacts | "Leads up 60%" offered as proof that marketing is fine |
| K | Non-existent problem or intentional tradeoff | — |

Relevant tags: `metric-gaming` (dev D, E, F, I), `measurement-artifact` (all dev cases), `local-optimization` (dev D, G, I), `intentional-tradeoff` (dev G, K), `nonexistent-problem` (K), `side-effect` and `iatrogenic-intervention`.

Metrics: tag-specific accuracy, false-diagnosis rate, harmful-action promotion (rolling out a gamed incentive is a candidate `harmful` action), premature-action promotion, causal overclaim rate, and SUFF abstention accuracy.

---

## Q1. Can EDF identify conflicting incentives?

**What EDF v0.3 does.** Incentives are not an origin type. They appear only as one item in the Operations/Governance lens of the optional Diagnostic Calibration, as a disclaimer in `principles.md`, and as lens prose in the Boeing case (`diagnostic-taxonomy.md` §1.3). The Financial origin type covers budget and compensation in form but not in mechanism, and v1 drops it without saying where incentives go (§1.2). Control-point ranking has no field for incentive side-effects (§4.2).

**What the literature says.**

- Reward systems frequently reward A while hoping for B (SRC-CDE-062).
- Agency costs and information asymmetry are structural (SRC-CDE-067).

Both are well-developed theory with many case illustrations, but systematic evidence on how prevalent the problem is remains mixed (lit-dynamics §1.5). A rejoinder argues that some mismatches are deliberate. That rejoinder is not in the register and is noted as unverified in lit-dynamics. This links Q1 to Q4 below.

**Proposed mechanism.**

- **Incentive map (EDF-1/2).** For each key actor, record what is measured and rewarded, what is hoped for, and where they diverge (lit-dynamics E9).
- **Origin prompt.** Add *incentive* as an origin prompt (OPP-06).
- **Control-point check.** For each control point, ask what it rewards and what behaviour it will displace (OPP-09, OPP-10).
- **Stake declaration.** Applied to the commissioning party (OPP-02). The sponsor's incentives are part of the incentive map.

**How OD1 tests it.**

- Class E: the bonus pays for AHT, while the organization hopes for service quality.
- Class I: the division is measured on dashboard health, not on customer dates.
- Measured by tag accuracy on `metric-gaming` and `local-optimization` items, and by harmful-action promotion on rollout items.

---

## Q2. Can EDF detect local optimization that damages global performance?

**What EDF v0.3 does.**

- The System Context (Primary System, Context, Focus) sets the scope, but nothing checks whether optimizing the declared system harms the wider one.
- "Valid only within its declared System Context" can immunize a diagnosis that has been drawn too narrowly (`current-limitations.md` rank 21; `methodology-inventory.md` §4).
- Structure, interfaces and coordination are absent as origin categories (`diagnostic-taxonomy.md` §1.3).
- Propagation has no feedback loops (§2.2), so displacement of a problem onto another unit cannot be represented.

**What the literature says.**

- People misperceive feedback and delays. They are insensitive to how their own decisions feed back through the wider system (SRC-CDE-034, experimental, [SR]).
- Well-intended interventions are defeated by the system's response (SRC-CDE-035, theory).
- Rasmussen's control-across-levels view treats local efficiency pressure as a driver of system-level drift (SRC-CDE-048).
- Goodhart's causal and adversarial variants describe proxy optimization that breaks the goal (SRC-CDE-064, abstract-level).

**Proposed mechanism.**

- **Boundary challenge in Step 1.** Name at least one adjacent system that could bear displaced cost, and ask who would draw the boundary differently. This extends OPP-02.
- **Displacement question in OPP-09.** "If this unit's metric improves, where does the cost go?"
- **Ranking field in OPP-10.** Add a distributional-impact field: who bears the cost (`diagnostic-taxonomy.md` §4.2 lists it as missing).
- **Requester-proposed fixes (OPP-10 traceability).** Fixes that move shared resources toward the requester must trace to diagnosed origins.

**How OD1 tests it.**

- Tag `local-optimization` in classes D and I. Class I's request to move shared platform engineers is the clearest probe.
- Iatrogenic and side-effect items test whether the analyzer recognizes harm elsewhere.
- Metrics: tag accuracy, and harmful- or premature-action promotion on the reallocation action.

---

## Q3. Can EDF detect metrics that conceal deterioration?

**What EDF v0.3 does.**

- Manifestations are signals that can be "seen, measured, reported" (`methodology-inventory.md` §6).
- The Outcome step gives no guidance for metric-defined outcomes, where the first question is whether the metric is valid (§5).
- Measurement and metric validity is an S2 gap (`current-limitations.md` rank 12; `diagnostic-taxonomy.md` §1.3).

**What the literature says.**

- Indicators used for decisions attract corruption pressure (SRC-CDE-063 / SRC-FGI-073; exact passage unverified).
- The proxy–goal relationship collapses under optimization (SRC-CDE-064).
- Report counts depend on reporting climate (SRC-CDE-041). A falling count can mean deterioration in reporting rather than improvement in the system.
- Normalized deviance hides drift because the deviant has become the baseline (SRC-CDE-061, SRC-CDE-048; case-based).
- Survey scores can shift by recalibration (beta) or redefinition (gamma) rather than by real change (SRC-FGI-075, SRC-FGI-076).
- Generic statistical early-warning signals have high false-alarm rates (SRC-FGI-069).
- The balanced-scorecard literature shows that hypothesized metric causal chains are often asserted, not tested (SRC-BH-013).

**Proposed mechanism.**

- **OPP-05 metric-integrity check**, applied to every metric used as evidence: Is it a target? Who is judged on it? When was its link to the goal last checked? Which Goodhart variant is suspected? Is there a non-target corroborator?
- **OPP-04 missingness rule** for count metrics.
- **OPP-19 temporal baseline** for drift.
- **Forward look (OPP-15, OPP-20).** Any monitoring plan keeps at least one indicator that is not a target.

**How OD1 tests it.**

- Class I ("green dashboards, missed dates").
- Class E (AHT improvement that may be gamed).
- Class D ("leads up 60%").
- Tags `metric-gaming` and `measurement-artifact`.
- An analyzer that cites the gamed metric as support for a `supported` answer loses item accuracy and evidence F1.

---

## Q4. Can EDF distinguish dysfunction from intentional tradeoffs?

**What EDF v0.3 does.**

- There is no category for goal conflict or intentional tradeoff, and the origin definition presumes a defect (`diagnostic-taxonomy.md` §1.3).
- Diagnostic Sufficiency has no "no action warranted" outcome (`current-limitations.md` rank 6; `methodology-inventory.md` §13). So a correctly recognized tradeoff still has to produce a "next responsible action" toward fixing something.
- The sequence assumes a discrete, already-happened outcome. Chronic, contested-goal situations fit poorly (`methodology-inventory.md` §1).

**What the literature says.** The literature here is thin.

- The dialogic critique notes that some organizational "outcomes" are contested meanings, not facts (SRC-BH-062, theoretical).
- Multiframe diagnosis makes the frame explicit (SRC-BH-055, theoretical).
- Action bias pushes toward recommending something (SRC-FGI-017, weak, non-organizational).
- Interventions can harm (SRC-FGI-011).
- The threshold model formally permits "no test, no treat" (SRC-FGI-019).
- No primary study was found on how often organizational diagnoses *should* end in no action (lit-intervention §5, gap).

**Proposed mechanism.**

- **Tradeoff category (OPP-06).** Add a *goal conflict / intentional tradeoff* category with a discriminating test: Was the cost known and accepted by someone with authority to accept it? Is it documented? Is it still within the accepted bound?
- **Remedy type.** For a tradeoff, the remedy is a *decision*: re-affirm, re-price or reverse it. It is not a fix. This is a handoff to Clarity's decision fields.
- **Sufficiency outcomes (OPP-11).** Permit "no action warranted" and "monitor with trigger" as legitimate outcomes.
- **Contested framings (OPP-02).** Record contested framings in System Context rather than silently resolving them (lit-org-diagnosis §1.4).

**How OD1 tests it.**

- Class K: the dev case is a law firm's "morale and retention crisis" built on two resignations and an overheard complaint.
- Tags `intentional-tradeoff` and `nonexistent-problem`.
- The SUFF item, where the correct answer can be `insufficient`.
- Actions such as launching a broad retention programme can be `premature` or `harmful`.
- Metrics: false-diagnosis rate, premature-action promotion, abstention accuracy and `intentional-tradeoff` tag accuracy.
- Guardrail: missed-problem rate on true-problem classes, so that "it's a tradeoff" does not become a blanket excuse.

---

## Cautions

- **Incentive explanations are seductive.** They fit almost any outcome, so OPP-01's triangulation rule applies to them as much as to leadership narratives. An incentive origin needs evidence that behaviour actually changed in the rewarded direction.
- **Suspicion of all metrics is its own failure.** Benchmark rule 3 ("system records may be gamed") cuts both ways. Some records are right, and the analyzer must tell which.
- **The real-world test is consequential.** Whether incentive-aware diagnosis leads to better incentive redesign can only be tested in the field. See `../intervention-effectiveness/evaluation-design.md` and `outcome-measurement.md` (non-target indicators and unintended-consequence monitoring).
