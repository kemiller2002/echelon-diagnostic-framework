# Amendment A2: Two-family availability boundary

**Date:** 2026-09-22  
**Experiment:** EX-EDF-001  
**Timing:** After A1 and A2 analyzer generation, before blinded evaluator scoring or semantic inspection of analyzer outputs.

## Reason

The original preregistration requires three independent analyzer provider families. Under the current no-API-cost execution constraint, a third family is assumed unavailable.

This amendment does **not** rewrite EX-EDF-001 as a successful two-family confirmatory experiment. EX-EDF-001 remains incomplete against its original three-family design.

Instead, a separate analysis registration, `AN-EDF-001-2F`, is frozen before evaluator scoring begins.

## Preserved elements

The following remain unchanged:

- six primary cases;
- natural, matched-neutral, and EDF conditions;
- EDF vs matched-neutral as the primary comparison;
- five-point margin;
- frozen case evidence and ground truth;
- frozen semantic rubric;
- analyzer outputs already recorded on A1 and A2;
- seeded bootstrap approach;
- outside-evidence and forbidden-claim safety checks;
- natural analysis as a secondary comparator.

## Two-family analysis

The available primary dataset contains 12 paired case × analyzer units:

- 6 cases × OpenAI A1;
- 6 cases × Anthropic A2.

A1 and A2 each produced 18 primary condition outputs and six additional repeatability outputs.

The two-family analysis requires two blinded evaluator families, E1 and E2.

## No E3 adjudicator

Because a third independent family is assumed unavailable, evaluator disagreement cannot use the original E3 adjudication rule.

Before any evaluator scoring, the replacement rule is frozen:

1. Every unit with two valid evaluator scores remains in the analysis.
2. Primary evaluator aggregation is the arithmetic mean of E1 and E2.
3. An absolute E1/E2 difference of 20 points or more is explicitly flagged as high disagreement.
4. No high-disagreement unit is silently removed or retried.
5. A conservative sensitivity analysis uses the lower of E1 and E2 for every output.
6. High-disagreement count and rate are reported alongside the primary result.

## Claim boundary

Passing the two-family criteria may support HY-EDF-008 only as **two-family supportive evidence**. It cannot satisfy the original EX-EDF-001 confirmatory claim gate requiring three analyzer families.

No analyzer output was semantically inspected and no blinded evaluator score existed when this amendment was written.
