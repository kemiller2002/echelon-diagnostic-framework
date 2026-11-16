# Executive recommendations — EDF improvement research (EDF-IMPROVEMENT-2026-001)

## Bottom line

**EDF v0.3 does not measurably improve organizational diagnosis over a general-purpose AI prompt or an established diagnostic method.** No rewritten version tested here improved on it by a meaningful margin either. The largest measurable lever was **evidence acquisition**: which evidence gets read. How a method tells the analyst to reason over a fixed evidence file mattered far less.

## What was done

- Audited the EDF repository and froze the v0.3 baseline (commit `41fea6f`).
- Reviewed 224 sources across organizational diagnosis, validity, causal inference, organizational dynamics, interventions, adaptive diagnosis, AI diagnosis and monitoring.
- Built **OD1**, a 22-case organizational diagnosis benchmark covering classes A–K. Cases were independently authored, adversarially reviewed, hash-frozen, and their truth sealed during runs.
- Ran **484 analyzer and grader runs** across:
  - general AI, EDF v0.3, EDF-0, Kepner-Tregoe + Nadler-Tushman and matched structure;
  - an experimental EDF variant and two ablations;
  - three Clarity integrations, two model tiers, an open-generation arm and an adaptive-evidence experiment.
- An independent agent reimplemented the scoring from scratch and reproduced every result.

## Key results (held-out, paired, 95% CI)

| Question | Answer |
|---|---|
| Does EDF beat general-purpose AI? | No: +1.4 [−0.7, 3.1] (Sonnet), −1.6 [−3.9, 0.9] (Haiku), +1.7 [−1.5, 5.8] (open generation) |
| Does EDF beat an established method? | No: KT + NT − EDF −0.6 [−2.0, 0.7]; in open generation KT was nominally best |
| Does EDF add value beyond generic structure? | Not by the preregistered margin; inconsistent across phases. Generic long structure itself can hurt (−7.0 on Haiku). |
| Does the improved variant beat EDF? | No: +1.5 [−0.3, 3.6]; +0.1 on Haiku |
| Does Clarity integration help diagnosis? | No: −0.8 and −0.0 |
| Is EDF-0 enough? | Slightly worse than full EDF: −2.1 [−3.9, −0.5] |
| What helps? | Choosing evidence adaptively: +11.7 [5.9, 17.9] over random collection at the same budget. Explicit decision gates: +2.8 [0.5, 5.7] in ablation, unconfirmed. |
| Are results reproducible? | Yes, under independent re-scoring. 97.5% item test–retest. Grader κ 0.95. |

## Recommendations

1. **Stop claiming diagnostic superiority** for EDF in organizational settings. Present it as a representational grammar whose diagnostic value is unproven.
2. **Do not elaborate EDF further.** Evidence favours fewer, decision-relevant rules over more vocabulary.
3. **Adopt the OD1 evaluation infrastructure** (with the menu-order and calibration fixes) as the mandatory gate for any future EDF claim.
4. **Redirect EDF's design toward evidence acquisition and decision gates:** sequential evidence requests chosen for discriminating power; "no intervention warranted" as a valid outcome; harm judged under every live hypothesis. Test these on a harder OD2 benchmark before adoption.
5. **Run the two studies that could change this conclusion:**
   - a **cross-provider replication**;
   - a **human-analyst study**, since EDF's plausible remaining value is helping people communicate and review diagnoses, which this programme could not measure.
6. **Keep Clarity separate**, at the decision-record stage.
7. **Do not add continuous monitoring** to EDF except for a defined purpose with aggregate data and safeguards.

## Confidence and limits

The negative findings are robust within scope: three arms, two model tiers and independent replication. The scope itself is narrow:
- synthetic cases;
- a single model family for authoring, analysis and grading;
- a benchmark near ceiling;
- no human analysts;
- no real interventions.

The evidence shows only that EDF's advantage, if one exists, did not appear in any of these tests; it does not show that EDF is worthless. Status: **PARTIALLY COMPLETE**. Real-world studies could not be performed; executable protocols are preserved.
