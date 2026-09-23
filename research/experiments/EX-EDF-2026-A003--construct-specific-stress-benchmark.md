---
id: EX-EDF-2026-A003
title: "EDF Construct-Specific Stress Benchmark"
status: proposed
created: 2026-09-22
research_area: diagnostic-frameworks
author_agent: "ChatGPT GPT-5.6 Sol"
tests_hypotheses:
  - HY-EDF-2026-A011
  - HY-EDF-2026-A012
  - HY-EDF-2026-A013
  - HY-EDF-2026-A014
  - HY-EDF-2026-A015
  - HY-EDF-2026-A016
tags:
  - edf
  - ablation
  - stress-test
  - mechanical-scoring
  - two-family
---

# EDF Construct-Specific Stress Benchmark

EX-EDF-001 found no material semantic advantage for the complete EDF prompt over a strong matched-neutral structured prompt and showed severe evaluator ceiling effects. This experiment isolates EDF-specific semantic constraints under deliberately harder evidence conditions.

Primary scoring is deterministic. Analyzer outputs classify frozen candidate hypotheses, causal edges, and actions, cite evidence IDs, and report confidence. Hidden truth defines the expected state for every scored item. LLM evaluators are not used for the primary outcome.

Twelve cases are independently authored: one OpenAI-authored and one Anthropic-authored case per construct. Both analyzer families execute both primary conditions on every case.

## Current execution state

As of 2026-09-23, the six OpenAI-assigned cases (ST-001, ST-003, ST-005, ST-007, ST-009, ST-011) have been authored with hidden truth files and are pending opposite-provider review. The six Anthropic-assigned cases remain un-authored. No case set has been frozen and no analyzer run has begun.

The canonical experiment identifier is `EX-EDF-2026-A003`. Legacy harness filenames beginning with `edf-002-` are compatibility names from pre-merge harness work and do not identify a separate experiment.
