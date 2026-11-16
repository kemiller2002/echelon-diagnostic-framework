# EX-EDF-2026-OD1 Amendment 2 — after dev phase, before any held-out analyzer run

Date: 2026-11-16. Held-out cases were frozen (`benchmarks/frozen-cases-heldout.sha256`, commit a53fd0e) and their
truth keys sealed before the dev scores below were computed.

## Dev observation that motivates the amendment

All eight dev conditions scored 91–95/100 (item accuracy ≈95%). Most errors were shared by every condition and sit on
category boundaries (premature vs harmful; contradicted vs association-only), i.e. they reflect truth-key granularity
rather than method differences. The candidate-recognition format therefore has limited discriminating power for
Sonnet-tier analyzers: the true problem and the traps are offered as candidates. Detection of problems nobody names,
resistance to framing without a menu, and action generation are not tested by it.

## Changes

1. **Experimental variant D (`D_EDFX`)** designed from dev failures (over-abstention when the presenting problem does
   not exist, commitment on unverified measurement-artifact hypotheses, action-severity calibration) and from the
   literature (source independence and missingness, premise check, metric validity, causal grading, VOI-driven
   sufficiency, harm under every live hypothesis). It is shorter than EDF v0.3 (3,034 vs 5,316 characters).
   Ablations: `D_NOORG` (steps 1–3 organizational guards removed), `D_NOGATE` (action/sufficiency/request rules
   replaced by EDF v0.3 wording).
2. **Trials.** Cost constraints (≈77k subagent tokens per run, ~2 concurrent agents per workflow): two trials for
   `B_EDF03` and `D_EDFX` (test–retest measured on these), one trial for all other held-out conditions.
   `CL_ALONE` is dev-only.
3. **New open-generation arm (OD1-G).** The same held-out cases with candidates and scored items removed. Analyzers
   receive only the organization, presenting request and evidence and must state problems, causal claims, actions,
   sufficiency and next evidence in their own words. A separate grader, blind to condition and to expected answers,
   maps each output onto the frozen candidate items (`supported / contradicted / unknown / not-addressed`, etc.).
   The deterministic OG score is defined in `benchmark-specification.md` §8 before any OG run. Conditions: A_GENERAL,
   B_EDF03, N_NEUTRAL, D_EDFX (C_KTNT if budget allows). Grader agreement is estimated by double-grading a subset.
4. **Hypotheses added.** H-OD1-8: in open generation, D detects more real problems than B without more false
   diagnoses (support: OG score D−B ≥ +5 with CI lower bound > 0). H-OD1-9: the recognition format saturates
   (descriptive: held-out condition means within 5 points of one another).

Dev results are exploratory and are not used for any confirmatory claim.
