---
id: EDF-COMPARATIVE-2026-002-REPORT
title: What does EDF contribute? Comparative evaluation and deep experimental validation (OD2)
status: complete-for-this-program (cross-provider and human studies outstanding)
version: 1.0.0
created: 2026-11-17
work_item: WI-0014
preregistration: experiments/preregistration-OD2.md (v1.0, frozen before held-out runs)
evidence: experiments/scored/heldout/{verdicts.md,stats.md,tracks.md}; experiments/scored/heldout-sensitivity/verdicts.md
---

# What does EDF contribute?

## Bottom line

**On 24 hard, held-out, preregistered diagnostic cases, EDF v0.3 made no measurable
difference to Claude's diagnoses.** It did not change accuracy, evidence collection,
calibration, repeatability or persuasiveness. Removing any of its components made no
difference either. The cheapest thing that measurably improved results was not a
framework at all: raising the model's reasoning effort one step, from medium to high,
with no framework. That beat EDF by about 3–4 points at *lower* cost.

This is the fourth independent null for EDF's diagnostic value (EX-EDF-001
two-family, OD1, OD1-S, OD2). It is the first to measure **equivalence** with tight
intervals: most EDF contrasts sit within ±2 points. It is also the first to ablate the
canonical specification component by component.

Scope: one model family (Claude) as author, analyzer and rater; synthetic cases;
recognition-format scoring, still near ceiling (accuracy ≈ 0.92); no human analysts.
EDF's most plausible remaining value, helping *people* state, communicate and
challenge diagnoses, is untested.

## Headline results (Sonnet 5.5, effort medium, N = 24 cases)

Primary analysis includes every run. The preregistered sensitivity analysis
excludes four runs in which the analyzer mistyped its own task-file path and
answered without the task (three produced no valid output). Those protocol failures
fell on different conditions and, with only 2–3 trials per case, they dominate the
primary intervals.

| Hypothesis | Contrast | Primary Δ score [95% CI] | Sensitivity Δ [95% CI] | Verdict (sensitivity) |
|---|---|---|---|---|
| H1 EDF vs no framework | B − A | −0.98 [−4.53, 1.36] | **+0.19 [−1.21, 1.45]** | practically equivalent |
| H2 EDF vs Kepner-Tregoe | B − C | +0.11 [−3.05, 2.58] | +1.28 [−0.08, 2.95] | practically equivalent |
| H3 EDF vs KT + ACH | B − D | +0.82 [−3.12, 5.40] | +0.12 [−0.80, 1.09] | practically equivalent |
| H4 gates + VOI collection vs EDF | F − B | +0.95 [−2.11, 5.16] | −0.17 [−2.19, 1.81] | practically equivalent |
| H5a System Context | B − E_NOCTX | +1.08 [−3.27, 6.57] | +0.19 [−1.08, 1.41] | practically equivalent (removable) |
| H5b Origin Network + Propagation | B − E_NOCAUSAL | −1.00 [−4.26, 1.39] | +0.17 [−1.20, 1.61] | practically equivalent (removable) |
| H5c Evidence evaluation | B − E_NOEVID | −0.35 [−4.32, 2.98] | +0.82 [−1.32, 3.35] | inconclusive |
| H5d Control Points + Ranking | B − E_NOCTRL | −0.63 [−3.92, 1.77] | +0.54 [−0.82, 1.97] | practically equivalent (removable) |
| H5e Diagnostic Sufficiency | B − E_NOSUFF | −1.31 [−4.74, 0.85] | −0.13 [−1.26, 0.89] | practically equivalent (removable) |
| H6 Sequence | B − E_NOSEQ | −0.35 [−3.39, 2.01] | +0.82 [−0.64, 2.37] | practically equivalent |
| H7a vote-of-5, no framework (5× cost) | A_VOTE5 − B | +1.91 [−0.51, 5.47] † | +0.73 [−0.62, 2.20] † | practically equivalent |
| H9a Claude-chosen method vs KT | SEL_CLAUDE − C | +0.52 [−1.11, 2.43] | same | practically equivalent |
| H9b EDF orchestrator vs EDF | SEL_EDFO − B | +0.31 [−3.51, 4.78] | −0.86 [−3.88, 1.67] | inconclusive |

† Vote acquisition is the mean over member runs (post-hoc deviation D-3). The
preregistered union version (A_VOTE5U) scores lower, 89.3, purely because pooling five
runs' reads mechanically exceeds the 2-item budget. Its accuracy is the same (0.943).

**No contrast reached the preregistered material-advantage threshold** (Δ ≥ 5,
CI > 0) in either analysis. Condition means for every Sonnet-medium arm lie between
89.7 and 92.7. Item accuracy lies between 0.91 and 0.94.

### Haiku 5.5 replication (H11, effort low)

| Contrast | Δ score [95% CI] | Δ accuracy (points) |
|---|---|---|
| EDF − no framework | −2.08 [−4.72, 0.24] | −1.09 [−3.09, 0.78] |
| EDF − Kepner-Tregoe | **−3.31 [−5.57, −1.42]** | −2.13 [−3.82, −0.52] |
| gates + VOI − EDF | +0.30 [−2.44, 3.04] | −0.75 [−3.03, 1.51] |

At the smaller tier, EDF is nominally worse than no framework and reliably (if
modestly) worse than Kepner-Tregoe. Neither result passes the 5-point margin. This
replicates OD1's direction on Haiku (−1.6). Structure aimed at a large model can cost
a small one.

## Track A — structure vs compute

| Arm (Sonnet) | Score | Accuracy | Cost per run (USD) | Thinking tokens |
|---|---|---|---|---|
| No framework, low | 91.5 | 0.926 | 0.048 | 132 |
| No framework, medium | 91.7 | 0.938 | 0.050 | 247 |
| **No framework, high** | **95.2** | **0.972** | **0.055** | 676 |
| No framework, max | 96.6 | 0.975 | 0.848 | 65,159 |
| EDF, medium | 90.7 | 0.921 | 0.058 | 234 |
| EDF, high | 94.1 | 0.963 | 0.064 | 683 |
| EDF, max | 95.1 | 0.969 | 0.781 | 60,972 |
| Vote of 5, no framework, medium | 92.7 | 0.943 | 0.247 | 1,235 |

- **No framework at high effort vs EDF at medium: +4.42 [0.41, 9.36]** score and
  +5.0 [1.4, 9.7] accuracy points, while costing **$0.0035 less per run**
  [0.0015, 0.0050]. In the sensitivity analysis: +3.25 [0.11, 7.48]. Changing the
  effort flag beats adding the framework on quality and cost together.
- **H7b efficiency gain: not supported.** At no effort level does EDF match no framework
  at a higher effort for less money. **Capability gain: not supported.** EDF@max − A@max
  is −1.46 [−4.50, 0.48]. At high effort EDF is slightly *worse*: −1.08 [−2.37, −0.05].
- **Max effort buys little over high:** about +1.4 points for roughly 15× the cost.
- **EDF's only measurable cost effect is more prompt tokens:** about +$0.008–0.010 per
  run (prompt length) and no benefit.

## Track B — accuracy vs persuasiveness

A blind Opus 5.5 rater saw the case background, the initial evidence and one brief,
with method vocabulary scrubbed. It is a Claude rater, **not independent human
evidence**.

| Contrast | Persuasiveness (1–7) | Perceived correctness | True accuracy | Persuasion gap |
|---|---|---|---|---|
| EDF − no framework | −0.08 [−0.17, −0.02] | −0.73 [−1.35, −0.17] | 0.00 [−0.016, 0.015] | −0.008 [−0.025, 0.011] |
| EDF − KT | −0.10 [−0.25, 0.00] | −1.31 [−3.00, −0.08] | +0.016 [0.001, 0.033] | −0.029 [−0.054, −0.007] |

- **H8 not supported.** EDF did not make diagnoses more persuasive than they were
  accurate. Its plain-language briefs were, if anything, marginally *less* persuasive.
  The differences are tiny: under 0.1 on a 7-point scale.
- **The rater could barely tell right briefs from wrong ones.** Spearman correlation
  between perceived correctness and true accuracy was 0.27–0.54 across conditions. It
  was lowest for EDF (0.27 [−0.04, 0.56]) and highest for the EDF orchestrator (0.54
  [0.30, 0.72]), with overlapping intervals. Persuasiveness is a poor proxy for
  correctness whichever method wrote the brief. Any EDF evaluation that rests on how
  convincing its outputs look, as the historical R1 corpus did, is unreliable by
  construction.
- **Caveat.** Briefs were constrained to plain language with no method vocabulary.
  A structured EDF write-up shown to human reviewers is untested. That is where a
  persuasion effect, good or bad, would most plausibly appear.

## Track C — method selection and orchestration

- Claude, given EDF, KT, ACH and unstructured reasoning in full and asked to choose,
  **never chose EDF (0 of 48 runs)**. It chose ACH (14), ACH plus careful reasoning
  (13), KT plus ACH (7), KT (6), unstructured (3), and 5 runs were uncoded. Results
  were equivalent to fixed KT (+0.52 [−1.11, 2.43]).
- The experimental **EDF orchestrator** (EDF record plus technique switching plus
  gates) mostly ran KT then ACH (19 runs), or KT alone (16). It mentioned EDF
  techniques in 5 of 48. It was not better than fixed EDF (−0.86 [−3.88, 1.67],
  sensitivity).
- Selection errors (chose an arm ≥ 5 below the best fixed arm for that case) were rare:
  4 of 48 for SEL_CLAUDE, 1 for SEL_EDFO. With every fixed method near ceiling there
  was little to gain from selection, so H9 cannot show a selection advantage on this
  benchmark.

## Mechanisms (predictions registered before results; `theory/mechanism-predictions.md`)

| Mechanism | Prediction | Result | Status |
|---|---|---|---|
| M1 search-space reduction | EDF reads fewer items at equal accuracy | Reads 1.79 (EDF) vs 1.82 (none); equal accuracy | Refuted for this benchmark |
| M2 externalized state | Gains from fields, not order | No gain to attribute; B − E_NOSEQ ≈ 0 | Nothing to explain |
| M3 evidence linkage | Higher evidence F1; E_NOEVID lowers it | F1 0.842 vs 0.835; E_NOEVID 0.838 | Refuted |
| M4 reduced premature convergence | Fewer false diagnoses and overclaims | Both rates ≈ 0 in all arms (floor) | Untestable here |
| M5 VOI-like stopping | EDF stops earlier on STOP, reads more on SEQ | Every arm read 0 items on STOP cases and ≈ 2 elsewhere; no difference with or without Sufficiency or the VOI rule | Refuted: stopping is already optimal without EDF |
| M7 calibration vs conviction | Better Brier, or a persuasion gap | Calibration 0.922 vs 0.922; no gap | Neither |
| M8 causal vs narrative | Better on association-only edges | 0.859 (EDF) vs 0.883 (none) | Refuted |
| M9 variance | Higher (or lower) test–retest | 0.963 vs 0.963. **KT + ACH lower: 0.911 [0.877, 0.937]** | EDF: no effect. ACH-style multistep: added noise, as Mandel and Tetlock (2018) predict |
| M10 compute-normalized value | Compute beats structure per dollar | Effort high > EDF at lower cost | **Supported** |
| M11 orchestration | Choices vary by class; orchestrator > EDF | Choices dominated by KT/ACH; no gain | Not supported |

## Answers to the required questions

1. **What does EDF do differently?** It supplies a *representational grammar*:
   declared scope, origin network, propagation, control points distinct from origins,
   and an explicit sufficiency judgment, applied the same way to failures and
   successes across domains. Its procedural content overlaps heavily with KT, systemic
   accident models (STPA/CAST), HRO's reluctance to simplify, and a qualitative form of
   value-of-information stopping.
2. **Closest relatives.** Kepner-Tregoe (problem specification and verification),
   STPA/CAST (control structures, multi-cause), A3 (scoped, one-page reasoning), and
   ACH (keeping rivals live). See `literature/01-literature-and-taxonomy.md`.
3. **Where EDF outperforms, matches or underperforms.** Matches no framework, KT and
   KT + ACH on Sonnet (equivalence within about ±2). Underperforms KT on Haiku
   (−3.3, CI excludes 0). Underperforms "no framework at higher effort" on both score
   and cost. Outperforms nothing tested.
4. **Advantages experimentally demonstrated.** None for diagnostic accuracy,
   acquisition, calibration, repeatability or persuasiveness, in four studies.
5. **Advantages that remain theoretical.** Value to human analysts: shared vocabulary,
   reviewability, challengeability, learnability (HY-EDF-010), and cross-team
   comparison of diagnoses. These are untestable with the methods used here.
6. **Capability, efficiency, or both?** Neither. EDF costs slightly more per run and
   adds nothing at any effort level.
7. **Accuracy or explanation quality?** Neither measurably, in Claude-rated
   plain-language briefs. Persuasiveness was not inflated.
8. **Adaptive selection vs fixed EDF?** No advantage on this benchmark. Claude itself
   never selected EDF.
9. **Value as an orchestration system?** Not demonstrated. The orchestrator variant
   gravitated to KT and ACH, and did not beat fixed EDF.
10. **Benefit from Claude.** Effectively all of it. Condition differences are under
    about 2 points, while effort (+3–5) and model tier (Sonnet vs Haiku, ≈ +4) are the
    levers that move results.
11. **Necessary components.** None demonstrated for model diagnosis. System Context,
    Origin Network + Propagation, Control Points + Ranking, Diagnostic Sufficiency and
    sequence are each practically equivalent to removal. Evidence evaluation is
    inconclusive (+0.82 [−1.32, 3.35]).
12. **Minimum effective configuration.** For automated diagnosis with a strong model:
    **no EDF prompt**, plus an effort setting of high. If a structure is wanted for
    human-facing output, the evidence supports a short output contract (scope, causes
    with evidence grades, unknowns, next action), presented as a *reporting* format
    rather than a reasoning procedure. This is a hypothesis for human studies, not a
    finding.
13. **Claims that survive skeptical review.**
    - "EDF does not improve LLM diagnostic accuracy over a general prompt or an
      established method, on synthetic benchmarks, for Claude models." This now rests
      on four studies, one with two model families.
    - "EDF components are individually removable without measurable loss for Claude
      (OD2)."
    - "Raising reasoning effort is a cheaper lever than adding EDF."
    - "Claude does not prefer EDF when offered alternatives."
    No claim of EDF superiority survives.
14. **Improvements supported by evidence.** For automated use: drop the procedural
    prompt and spend compute on effort or sampling. Treat budgets as hard constraints;
    OD1-S's budget-violation finding did not recur at budget 2. No proposed EDF
    enhancement (gates, VOI rule, orchestration) improved on v0.3. Proposals are in
    `improvements/proposals.md`.
15. **Experiments needed before broader adoption.**
    - A cross-provider replication (UQ-01).
    - **A human-analyst study**: time to an actionable finding, reviewer error
      detection, inter-analyst agreement and communication, with and without EDF
      (UQ-02).
    - An open-generation benchmark without a candidate list, to break the recognition
      ceiling.
    - Real or retrospective incidents with documented outcomes.
16. **What the next program should investigate.**
    - EDF as a **human communication and review format**, not as a reasoning
      procedure for models.
    - Benchmarks where evidence is long, noisy and multi-document, so context
      management (M2, M6) could matter.
    - Whether any structure helps *small* models. Here it hurt on Haiku: why?
    - Evaluation of diagnoses against real downstream outcomes.

## Negative findings, contradictions and limitations

- **Ceiling persisted** despite two preregistered difficulty amendments (read
  budgets 5 → 3 → 2). Item accuracy was 0.92–0.94 on Sonnet-medium. Every analyzer
  read 0 items on STOP cases and found decisive evidence elsewhere. The benchmark can
  establish equivalence within about ±2 points. It cannot rule out small effects on
  harder tasks.
- **Single family.** Claude authored, reviewed, analyzed and rated. Shared blind spots
  could hide method effects that another family would show. EX-EDF-001's two-family
  null reduces, but does not remove, that worry.
- **Protocol failures.** Four runs (0.4%) mistyped the task path. They are disclosed;
  the primary analysis keeps them and the sensitivity analysis drops them.
- **Post-hoc choices** are labelled: vote acquisition (D-3). Analysis code was added
  after the freeze but before any result was viewed (D-1). The runner was changed
  mid-run in a behaviour-neutral way (D-2).
- **Review gate.** 2 of 26 held-out cases were excluded for key ambiguity. The most
  common residual ambiguity was whether several actions can be "preferred". That is a
  limitation of the answer semantics, which OD1 shares.
- **Contradiction with earlier signals.** OD1's exploratory decision-gate effect
  (+2.8 on one trial) did **not** replicate: F − B is −0.17 [−2.19, 1.81]. OD1-S's
  KT evidence-efficiency advantage is not testable at budget 2, where every method
  reads about 2 items.

## Cost and provenance

- Held-out: 1,080 analyzer runs, **$87.93**. Ratings: $14.77 (held-out $12.82 plus
  pilot $1.95). Case authoring: $34.78. Review and revision: at least $18.86. Pilots:
  $10.45. **Total recorded: $166.80.** Round-1 solve and adjudication transcripts for
  the 12 re-reviewed cases were overwritten by round 2, so their cost is not counted.
  Their review records are preserved in `benchmark/reviews/round1/`.
- Every run records the exact model ID (`claude-sonnet-5-5`, `claude-haiku-5-5`)
  confirmed by the API, tokens (including thinking), cost, wall time, and a gzipped
  transcript.
- Reproduce: `harness/README.md`.
