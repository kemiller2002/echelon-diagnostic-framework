# EDF Two-Family Structural-Value Result

## Executive conclusion

The available two-family benchmark does **not support HY-EDF-008**, the hypothesis that EDF adds material semantic value beyond a matched neutral structured prompt.

This is a bounded result, not a universal rejection of EDF. Under the six synthetic cases in this experiment, EDF and the strong neutral structure performed nearly identically. The benchmark also exhibited a severe evaluator ceiling effect, which limits how much discrimination the current cases and rubric can provide.

## Execution integrity

Analyzer execution completed cleanly:

- A1: OpenAI GPT-5.6 Sol, 24/24 valid runs.
- A2: Anthropic Claude Opus 5, 24/24 valid runs.

Blinded evaluation completed with:

- E1: OpenAI GPT-5.6 Terra, 36/36 valid.
- E2: Anthropic Claude Sonnet 5, 33/36 valid.

The three E2 invalid outputs were formatting failures: each wrapped JSON in Markdown code fences. Per the frozen protocol they remain terminal invalid outputs and were not repaired for the registered primary analysis.

## Registered two-family analysis

The registered design expected 12 paired case × analyzer units.

Only 9/12 units had valid scores from both evaluators for both EDF and matched-neutral.

Across those 9 complete pairs:

- Mean EDF minus matched-neutral: **+0.56 points**
- Frozen margin: **+5 points**
- Seeded paired-bootstrap 95% interval: **[0.00, +1.39]**
- Conservative min(E1,E2) mean difference: **+1.11 points**
- Conservative bootstrap interval: **[0.00, +2.78]**

By analyzer family on complete pairs:

- A1 / OpenAI analyzer: **0.00**
- A2 / Claude analyzer: **+1.00**

The frozen support criteria therefore do not pass:

1. +5-point margin: not met on observed complete pairs.
2. Bootstrap lower bound above zero: not met.
3. Both analyzer families positive: not met because A1 is exactly zero.
4. Material safety-flag constraint: met.

EDF had one materially flagged primary unit for outside-evidence/forbidden-claim categories versus two for matched-neutral.

## Post-hoc formatting sensitivity

For diagnosis only, the embedded JSON inside the three Markdown-fenced E2 responses was mechanically extracted without changing its contents.

This is **not** part of the registered result.

With those three payloads included, all 12 paired units yield:

- Mean EDF minus matched-neutral: **+0.21 points**
- Bootstrap 95% interval: **[-0.42, +0.83]**
- Conservative min(E1,E2): **+0.42 points**
- Conservative bootstrap: **[-0.83, +1.67]**

Analyzer-family means under this post-hoc sensitivity are:

- A1: **-0.42**
- A2: **+0.83**

Evaluator-specific mean differences are:

- E1: **+0.83**
- E2: **-0.42**

The formatting failures therefore do not conceal a material EDF advantage.

## Evaluator disagreement

Two of 33 outputs with valid scores from both evaluators differed by at least 20 points, a rate of about 6.1%.

Both occurred on SYN-003 in the A2 arm, one EDF and one matched-neutral. The disagreement is therefore not selectively concentrated in the EDF condition.

## Ceiling effect

The most important design limitation is score saturation.

E1:

- mean score: 94.86
- 19/36 outputs scored 100
- range: 75 to 100

E2:

- mean score among valid outputs: 99.70
- 31/33 valid outputs scored 100
- range: 95 to 100

Under the post-hoc complete sensitivity set, mean condition scores were:

- Natural: 97.71
- EDF: 97.08
- Matched neutral: 96.88

Natural analysis being numerically highest is another indication that these cases are too easy for current frontier models and the rubric lacks enough resolution near the top.

## Repeatability

All 12 additional repeatability runs were valid.

None was byte-identical to its corresponding primary run. That is expected for generative systems and is not, by itself, evidence of semantic instability.

The repeatability outputs were not independently semantically scored, so no stronger repeatability conclusion is justified.

## Interpretation

The strongest defensible conclusion is:

> In this benchmark, EDF-specific prompting did not produce a material semantic improvement over a strong matched-neutral structured reasoning prompt.

This weighs against the current form of HY-EDF-008.

It does **not** show that:

- EDF has no value as an engineering or teaching representation;
- EDF has no value on harder or real-world cases;
- individual EDF concepts such as negative knowledge, propagation, or control-point reasoning provide no incremental value;
- EDF provides no workflow, consistency, traceability, or human-comprehension benefits.

Those are different hypotheses and require different experiments.

## What the next experiment should change

A direct repeat of this benchmark is not recommended. The next experiment should increase discriminative pressure.

Use:

- harder cases with incomplete, conflicting, misleading, and temporally separated evidence;
- cases where symptom-level reasoning produces a plausible but wrong action;
- cases with multiple live hypotheses and evidence that changes over time;
- externally or adversarially authored cases rather than only repository-authored synthetic cases;
- real incident material where feasible;
- pairwise evaluator preference or finer-grained scoring instead of a saturated 0–4 rubric;
- deterministic measures such as required-finding recall, forbidden-claim rate, evidence citation precision, causal-edge accuracy, uncertainty preservation, and control-point quality;
- targeted ablations of EDF-specific constructs rather than comparing the entire EDF prompt against an already sophisticated neutral prompt.

The next key question should no longer be merely “Does EDF beat structure?”

It should be:

> Which EDF-specific semantic constraints produce measurable value, under what kinds of diagnostic difficulty, and at what cost?

That question is better aligned with the evidence produced here.
