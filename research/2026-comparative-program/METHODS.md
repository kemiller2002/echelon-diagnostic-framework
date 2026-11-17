# Methods (for the final report)

## Design

- **Benchmark.** OD2 has 24 held-out synthetic cases across 13 classes and three
  domains (organizational, technical, cross-domain). Each case has a short initial
  file and a catalogue of 14–16 hidden evidence items, of which the analyzer may
  collect **two**. Candidates (hypotheses, causal links, actions, sufficiency) are
  frozen, and keys are hidden. Scoring is deterministic: 60 accuracy, 10 evidence
  F1, 10 calibration, 20 acquisition, −15 per harmful action promoted.
- **Authoring and review.** Isolated Opus 5.5 sessions authored each case from a
  frozen contract. A second isolated session solved each case blind with the full
  catalogue, and a third adjudicated every disagreement. Cases with keys still
  ruled ambiguous after two revision rounds were excluded (2 of 26).
- **Execution.** Each analyzer run was a separate headless Claude Code session in
  an empty directory, with only the Read tool, a fixed minimal system prompt, no
  settings or MCP servers, and structured output. The exact model ID, tokens
  (including thinking), cost, wall time and the full transcript were recorded for
  every run. Catalogue labels were shuffled per run, and run order was seeded and
  shuffled. Tool calls were audited against the run's own files.
- **Conditions.** No framework (A); EDF v0.3 verbatim (B, byte-identical to OD1);
  Kepner-Tregoe (C); KT + ACH (D); six v0.3 component ablations (E); v0.3 + decision
  gates + value-of-information collection (F); Claude choosing among four techniques
  (SEL_CLAUDE); an experimental EDF orchestrator (SEL_EDFO); a majority vote over five
  no-framework runs (A_VOTE5); effort low, high and max for A and B; and a Haiku 5.5
  replication of A, B, C and F.
- **Preregistration.** Hypotheses, decision rules, conditions, plan and hashes
  were committed before any held-out run (`experiments/preregistration-OD2.md`,
  v1.0). Deviations are in `experiments/deviations-OD2.md`.

## What makes this stronger than the earlier tests

1. **Exact executor binding.** Earlier OD1 runs could not expose model versions,
   and EX-EDF-001 was blocked on it. Every OD2 run records the model ID reported by
   the API.
2. **Hidden evidence under a hard budget.** The task now includes acquisition, the
   lever OD1-07 identified.
3. **Component-level ablation of the canonical spec.** These were generated
   mechanically from `docs/edf/specification.md` and had never been run before.
4. **Compute, persuasiveness and selection** were measured directly.

## Limitations (declared before results)

1. **Single model family.** Authors, reviewers, analyzers and raters are all Claude
   models. Separate sessions are not independent models, and Claude ratings are not
   human judgments.
2. **Synthetic cases.** These are fictional and authored by a model. No real
   organizations, incidents or interventions are involved.
3. **Recognition format.** Analyzers classify frozen candidates; they do not
   generate hypotheses. Dev pilots showed item accuracy at ceiling. Amendments 1–2
   reduced the read budget to raise difficulty, but held-out accuracy may still be
   near ceiling, and differences may then sit inside equivalence bounds by
   construction.
4. **Power.** 24 cases. With case-level SD of differences around 5–8 points, 95% CIs
   are roughly ±2–3 points. That can detect the preregistered 5-point material margin,
   but not smaller effects with confidence.
5. **No human analysts.** EDF's most plausible remaining value, helping people
   communicate, review and challenge diagnoses, is not measured.
6. **Public truth files.** Cases and keys are committed to a public repository, so
   future models could be trained on them. Current analyzers had no web access.
