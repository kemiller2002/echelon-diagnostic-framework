---
id: EX-EDF-2026-OD2-AMENDMENT-02
title: OD2 amendment 2 — read budget 2 and non-redundant decisive evidence
status: frozen-before-heldout-authoring
created: 2026-11-17
work_item: WI-0014
amends: amendment-01-after-dev-pilot.md
evidence: experiments/scored/pilot2/stats.md; benchmark/reviews/OD2-DEV2-*.review.json
---

# Amendment 2: after dev2 pilot

## What dev2 showed (exploratory, 4 cases, 70 runs)

1. Item accuracy was still **0.90–0.94** at Sonnet-medium and **0.91** at Haiku-low
   under a 3-item budget. That is above the 0.85 threshold that amendment 1 set,
   so amendment 1's pre-committed rule applies: **the read budget drops to 2**.
2. **Every run, in both pilots, read at least one discriminating item** (zero runs
   with discriminating-read recall 0). Runs that missed some decisive evidence still
   scored 0.92 accuracy, against 0.98 for runs that read all of it. The
   discriminating items are partly **redundant**: one is enough for most keys.
3. Maximum effort cost about 14 times medium (≈66k thinking tokens, $0.84 per
   run) with no accuracy gain on two cases. This is a pilot observation for Track A,
   not a result.
4. Review: CONTRA and INTERACT passed. NOCONC had three ambiguous rulings, all
   from quantifier wording in candidates ("mainly", "most"). SEQ had one, where
   several actions could be "preferred".

## Changes (apply to held-out authoring)

| # | Change | Reason |
|---|---|---|
| 1 | `readBudget: 2` | Pre-committed in amendment 1 |
| 2 | Exactly **2–3 discriminating items**, and they must be **non-redundant**: each settles at least one expected answer that no other catalogue item settles. `optimalReadPath` is exactly the 2 items that together settle every key. A third discriminating item, if present, must be an alternative route to one of them, not a duplicate of both. | Finding 2. Reading the wrong pair must cost accuracy. |
| 3 | No quantifier claims ("mainly", "most", "largely", "primarily") in hypotheses or edges unless the evidence states a magnitude that decides them. | NOCONC rulings |
| 4 | At most **one** action is keyed *preferred*, unless two actions are explicitly complementary and the case says so. | SEQ ruling |
| 5 | The scored item count stays 13–17. | Unchanged |

`Domain.validate` enforces: for budget ≤ 2, catalogue 14–16, discriminating 2–3 for
diagnosable classes, optimal path exactly 2 for those classes.

## What is not changed

Conditions, prompts other than the budget number, scoring weights, and the review gate.

## Note for interpretation

If held-out accuracy is still near ceiling, OD2 cannot detect accuracy differences
between methods for this analyzer. The primary analysis then rests on
**equivalence** (CIs within ±3) rather than difference. Acquisition, cost and the
Track B outcomes stay informative. This is stated before any held-out data exists.
