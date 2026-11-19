---
id: EX-EDF-2026-A003
title: "EDF Construct-Specific Stress Benchmark"
status: cancelled
closed: 2026-11-19
closed_by: WI-0015
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

> **Closed without execution (2026-11-19, WI-0015).** Six of twelve cases were authored (the OpenAI half). The Anthropic half was never committed, although a journal entry claims it was (UQ-14). No analyzer run took place. The experiment is closed rather than completed for four reasons:
>
> 1. Three later preregistered benchmarks (EX-EDF-001, OD1, OD2) found no material EDF advantage for language-model analyzers. OD2 ablated each v0.3 component (context, causal structure, evidence evaluation, control points, sufficiency, sequence) and found each removal practically equivalent to the full specification.
> 2. A003 measures the same kind of outcome (language-model performance on synthetic cases) with the same ceiling risk, so its expected information value is now low.
> 3. The remaining high-value questions concern human analysts and cross-provider replication, which A003 does not address.
> 4. Completing it would mean authoring the missing half and resolving UQ-14's record inconsistency first.
>
> HY-EDF-2026-A011 to A016 stay **unsupported**: OD2's step-level ablations are related to them but do not test their explicit semantics, so no evidence is assigned to them. The authored cases, reviews and preregistration are preserved unchanged for traceability. Reopening requires a new work item with a stated replication or information objective.


EX-EDF-001 found no material semantic advantage for the complete EDF prompt over a strong matched-neutral structured prompt and showed severe evaluator ceiling effects. This experiment isolates EDF-specific semantic constraints under deliberately harder evidence conditions.

Primary scoring is deterministic. Analyzer outputs classify frozen candidate hypotheses, causal edges, and actions, cite evidence IDs, and report confidence. Hidden truth defines the expected state for every scored item. LLM evaluators are not used for the primary outcome.

Twelve cases are independently authored: one OpenAI-authored and one Anthropic-authored case per construct. Both analyzer families execute both primary conditions on every case.
