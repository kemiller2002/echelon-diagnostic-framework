# Improvement priorities

Ranking criteria (mission §20):
- expected improvement in diagnostic accuracy;
- severity of the failure addressed;
- strength of evidence;
- generalization;
- intervention value;
- implementation complexity;
- computational cost;
- human effort;
- regression risk;
- maintainability;
- commercial usefulness.

The two rankings below are kept separate, as the mission requires. Ratings are H (high), M (medium), L (low).

## A. Research priority (what to investigate next)

| Rank | Item | Why | Expected information value | Cost |
|---|---|---|---|---|
| 1 | **OD2: harder benchmark with evidence acquisition in the task** (CI-15, CI-03) | OD1 saturated; the only large effect came from which evidence was read | H | M (≈ 300 runs) |
| 2 | **Cross-provider replication** (OpenAI / Google analyzers and case authors) | Single-family bias is the largest validity threat | H | L–M (needs API credentials) |
| 3 | **Human-analyst study.** Consultants with and without EDF on anonymized real cases; blinded expert panel; time-on-task | The only test of EDF's most plausible value (communication, reviewability, learnability) | H | H |
| 4 | **Sequential evidence acquisition** (read → update → choose) vs batch selection, with optimal stopping | Batch selection missed ~50% of decisive evidence | H | M |
| 5 | **Confirm decision-gate semantics** (CI-01) on OD2 with ≥ 2 trials and 2 model tiers | Only component with a significant ablation | M | M |
| 6 | **Class-focused test of organizational guards** (CI-04): ≥ 10 contradictory-stakeholder and masked-risk cases | Case-level hints (+7.7, +9.0) not yet replicated | M | M |
| 7 | **KT IS / IS-NOT contrast step** (CI-10) targeted at under-rejection (FT-06) | Best causal accuracy; most common causal error | M | L |
| 8 | **Typed causal edges with vs without the four-way scale** (CI-02) | Possibly the most effective single causal intervention, untested | M | L |
| 9 | **Real-organization intervention evaluation** (`intervention-effectiveness/evaluation-design.md`) | Predictive validity is entirely untested | H | Very high |
| 10 | **Longitudinal / continuous assessment** pilot under safeguards | Literature says it is defensible only in narrow form | L | H |

## B. Production adoption priority (what to change in EDF now)

| Rank | Change | Classification | Rationale |
|---|---|---|---|
| 1 | Adopt OD1 evaluation infrastructure, with fixes, as the gate for any future EDF claim | **ADOPT** | Independently replicated; ends unreproducible claims |
| 2 | Correct EDF's public claims: no evidence of superiority over a general-purpose AI prompt or established methods for organizational diagnosis | **ADOPT (documentation correction)** | Required by evidence; no framework change |
| 3 | Update the evidence ledger with EX-EDF-001 and OD1 results; record HY-EDF-008 (organizational) as not supported | **ADOPT (records)** | Governance currency (CON-04) |
| 4 | Keep EDF v0.3 frozen; do **not** promote variant D or v1 draft architecture | **Hold** | H-OD1-5 failed; v1 draft untested |
| 5 | Pilot an "EDF-lite decision gates" annex (CI-01 + CI-02 + CI-03) as an optional, experimental profile | **EXPERIMENTAL** | Promising, not confirmed |
| 6 | Resolve the Clarity role conflict in favour of sequential handoff at the decision-record stage | **Documentation decision** | Integration showed no diagnostic benefit; avoids complexity |

Do **not** prioritize: further elaboration of EDF vocabulary, new origin categories, continuous monitoring features, or Clarity–EDF prompt integration.
