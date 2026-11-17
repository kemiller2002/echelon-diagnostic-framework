---
id: EX-EDF-2026-OD2-AMENDMENT-01
title: OD2 amendment 1 — difficulty and key-ambiguity fixes after dev pilot 1
status: frozen-before-heldout-authoring
created: 2026-11-17
work_item: WI-0014
amends: benchmark/od2-benchmark-specification.md v1.0.0
evidence: experiments/scored/pilot1/stats.md; benchmark/reviews/OD2-DEV-*.review.json
---

# Amendment 1: after dev pilot 1

Made after dev pilot 1 (7 dev cases, 70 analyzer runs) and the dev review. Made
before any held-out case was authored and before any confirmatory run. Only dev
data informed it.

## What the pilot showed

1. **Item accuracy was still at ceiling.** Mean accuracy was 0.90–0.94 for every
   condition, at both Sonnet-medium and Haiku-low. Only the 20-point acquisition
   component kept total scores near 83.
2. **The 5-item budget did not bind.** Analyzers read 3.9–4.5 items, and with
   3–5 discriminating items among 12–14 catalogue entries they usually collected
   the decisive evidence.
3. **Candidate wording leaked answers.** Some true hypotheses carried mechanism
   detail that only a catalogue item could justify. That made them recognizable
   from the initial file. (Example: the dev MISLEAD case's H2 names the job, the
   credential and the rate limit.)
4. **The remaining errors fell on ambiguous key boundaries.** The commonest
   errors were premature vs harmful actions (four cases) and association-only vs
   contradicted edges. The independent adjudicator ruled several of these keys
   **ambiguous** (dev ADVERS Q7, CONFOUND Q5 and Q13, INCOMPLETE Q11, MISLEAD Q11).
   Method differences on such items would measure key noise, not reasoning.

## Changes (apply to cases authored from now on)

| # | Change | Reason |
|---|---|---|
| 1 | **Read budget 3** (was 5). Each case records `"readBudget": 3`. | Make acquisition choices consequential (finding 2). |
| 2 | **Catalogue 14–16 items** (was 10–14); `discriminatingReads` **exactly 2–3** items; `optimalReadPath` ≤ 3. | Fewer decisive items among more plausible distractors. |
| 3 | **Hypothesis parity**: all hypotheses are written at the same level of specificity. No hypothesis may contain a mechanism detail (name, number, timing, component) that only a catalogue item establishes. | Finding 3. |
| 4 | **Action-key rules**: an action is keyed *harmful* only if it causes damage under the hypothesis the evidence supports **and** under every live rival. An action is keyed *premature* only if it would be clearly beneficial when its hypothesis is confirmed **and** causes no lasting damage if it is wrong. Actions that fit neither definition cleanly must be rewritten. | Finding 4. |
| 5 | **SUFF wording**: the SUFF item names the decision it refers to: "Is the evidence sufficient to commit to the action you rank highest?" | Dev CONFOUND Q13 ruling. |
| 6 | **Edges**: an edge keyed *contradicted* must have evidence against the link itself, not merely evidence for a rival. | Dev CONFOUND Q5 ruling. |
| 7 | **Review gate**: a held-out case enters the benchmark only after the blind-solve and adjudication review returns no *ambiguous* or *key-wrong* ruling, after at most two revision rounds. Cases that fail are excluded and the exclusion is reported. | Findings 3 and 4. |

Scoring is unchanged except that the acquisition budget is the case's `readBudget`
(default 5 for cases without the field). Dev-1 cases keep budget 5 and are kept as
`dev` pilot material. A second dev batch (`dev2`, four cases) is authored under
this amendment to check difficulty before held-out authoring.

## Difficulty target for pilot 2

The general-AI condition at Sonnet-medium should have mean item accuracy between
0.60 and 0.85 on dev2. If it is still above 0.85, the read budget drops to 2.
That would be recorded as amendment 2 before held-out authoring.
