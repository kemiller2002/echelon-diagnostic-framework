# Causal benchmarks

OD1 contains **97 scored causal-edge items** across 22 cases (`causal-benchmark-items.csv`). Expected answers: 40 causal, 29 association-only, 23 contradicted, 5 unknown. Every analyzer had to grade each claimed link on that four-way scale. The scale separates causation from association, which EDF v0.3 itself does not do.

## Required causal structures (mission §5) and where they are tested

| Required structure | Cases | Tag(s) |
|---|---|---|
| Multiple causes produce the same symptom | DEV-B, HO-B (two live causes, confounded timing) | confounded, contributing |
| Apparent root cause is incorrect | DEV-A (engineers' "discipline"), HO-A (ED staffing), HO-H (absenteeism) | leadership-narrative, causal-overclaim-trap |
| Common cause produces unrelated-looking problems | DEV-I (green-shifting), HO-I (bonus-linked deviation suppression) | hidden-problem, organizational-silence |
| Intervention improves one metric while damaging another | DEV-G (lock-up cuts theft and sales), DEV-E / HO-E (bonus improves metric, worsens service) | side-effect, metric-gaming |
| Feedback loops sustain dysfunction | DEV-F (firefighting loop), HO-F (agency-nurse loop) | feedback-loop |
| Successful team masks systemic failure | HO-I (top line borrowing technicians) | hidden-problem |
| Management intervention causes the problem it was meant to solve | HO-G (closure quota → backlog) | iatrogenic-intervention |
| Performing well despite structural risk | HO-J2 (commodity spike) | external-cause |
| Performing poorly for reasons unrelated to management | HO-J1 (venue renovation, sector-wide decline) | external-cause |

## Results (held-out, Sonnet, recognition)

| Metric | A | B (EDF v0.3) | C (KT + NT) | N | D |
|---|---|---|---|---|---|
| Causal-edge accuracy | 0.94 | 0.94 | 0.96 | 0.89 | 0.96 |
| Causal overclaim (non-causal → causal) | 1/32 | 1/64 | 0/32 | 1/32 | 0/64 |
| Association recognized | 0.88 | 0.91 | 0.94 | 0.82 | 0.97 |
| Feedback-loop items | 1.00 | 1.00 | 1.00 | 1.00 | 1.00 |
| Revision (staged evidence) | 0.93 | 0.96 | 0.93 | 0.79 | 0.93 |

On Haiku, causal accuracy fell for all conditions; see `experiments/scored/haiku/stats.md`.

## Interpretation

- No method produced material causal overclaiming once the shared task frame defined the four-way edge scale. **The single most effective causal intervention in this programme was the answer scale itself.** It forces "association-only" to be a named option, which EDF v0.3's free-text Origin Network and Propagation fields do not. This is a design finding: EDF should adopt typed causal edges with an explicit association/causation distinction.
- The residual causal error is **under-rejection**: links that contrast evidence refutes are kept alive as "association-only" (15 runs). Kepner-Tregoe's IS / IS-NOT testing (condition C) had the best causal accuracy, alongside D.
- Feedback loops were recognized by every condition. The loops in these cases are spelled out in the evidence, so detection of *implicit* loops is untested.

## Limits

Truth for "causal" in synthetic cases is stipulated by the author. Real organizational causal identification usually lacks interventions or natural experiments. The correct real-world answer is then more often `association-only` or `unknown` than here.
