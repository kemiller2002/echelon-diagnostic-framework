# Reliability analysis

## Test–retest (same condition, same case, independent runs)

| Condition | Item pairs | Agreement (trial 1 vs trial 2) |
|---|---|---|
| B_EDF03 (EDF v0.3), Sonnet, held-out | 198 | 97.5% |
| D_EDFX (variant), Sonnet, held-out | 198 | 97.5% |

The two trials ran in separate workflows. Item-level answers are highly repeatable for both, so the structure does not change reproducibility at this ceiling level. These are LLM test–retest numbers; they say nothing about agreement between human analysts.

## Inter-rater (open-generation grader agreement)

Two independent graders (Sonnet-tier and the session default tier) mapped 20 randomly sampled open-ended diagnoses onto the frozen items.

- Item-reading agreement: 302/316 = **95.6%**; Cohen's κ = **0.95**.
- OG score difference (g2 − g1): mean −0.3, maximum absolute 12.0 (a single output).

Mapping free-text diagnoses onto candidate items is reliable. The open-generation results are therefore not an artefact of grader noise.

## Inter-analyst and cross-method agreement

The benchmark's per-case item answers allow a cross-method agreement measure: how often two methods give the same answer to the same item. With ~95% item accuracy in every condition, cross-method agreement is necessarily high. It cannot distinguish "methods converge on truth" from "the cases are easy". The literature shows cross-method agreement can be very low on hard cases: agreement between different systemic accident-analysis methods applied to the same incident has been reported in the single digits (see `scientific-foundations/competing-methodologies.md`).

## Measurement invariance

Performance by benchmark class (held-out, Sonnet) varies far more than by method:

| Class | Range across A/B/C/N/D |
|---|---|
| A, B, F, K | 96–99 (ceiling) |
| C (incomplete information) | 90.3–97.1 |
| D (contradictory stakeholders) | 73.0–93.0 |
| I (adversarial) | 87.4–94.8 |
| J (out-of-distribution) | 87.2–92.6 |

Each held-out class has only 1–2 cases. No claim of invariance across organization types is possible. The OOD cases (orchestra, grain cooperative) did not collapse performance.

## Historical reliability claims

The repository's historical "very high reproducibility" figures (pre-2026 R1 runs) cannot be reconstructed: their raw outputs are not preserved. The EX-EDF-001 headline (+0.56) also has no raw outputs in the repository (`baseline/implementation-audit.md`). OD1 preserves every raw output (`experiments/raw/`) and journal, and the independent replication (`experiments/replication/`) recomputes all scores from them.

## Gaps

- No human inter-rater reliability for applying EDF fields.
- Single model family; cross-provider reproducibility untested (no credentials available in this session).
