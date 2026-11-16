# Token and computational cost analysis

## Measured quantities

| Quantity | Source | Value |
|---|---|---|
| Prompt size per condition (held-out mean, characters) | `experiments/scored/heldout/runs.csv` (`promptChars`) | A 20,830 · N 23,775 · C 23,946 · D 23,779 · B0 23,471 · B 26,061 · B_CLARITY 27,492 · BI_CLARITY 27,795 |
| Structured output size | `outputChars` | 2,240–2,290 chars for every condition. Method does not change output length under a fixed contract. |
| Subagent tokens per analyzer run, as reported by the workflow host | Workflow usage records | ≈ 77k tokens per run (e.g., 1,230,178 for 16 dev runs). This includes the host's fixed agent overhead (system prompt, tool schemas, injected repository instructions), which dominates. |
| Wall-clock per analyzer run | Workflow usage | ≈ 95–135 s per 16–18-run slice at concurrency 2, i.e. about 12–15 s of agent time per run |

Token counts per condition are **not separately exposed** by the host. Marginal prompt tokens are therefore *estimated* at ≈ 4 characters per token and are labeled as estimates.

## Marginal cost of each method (estimated)

| Condition | Extra prompt vs A (chars) | Est. extra input tokens per diagnosis | Extra as share of a ~77k-token run | Accuracy change vs A (held-out Sonnet) |
|---|---|---|---|---|
| B_EDF03 | +5,231 | ≈ +1,300 | ≈ 1.7% | +1.4 [−0.7, 3.1] |
| D_EDFX | +2,949 | ≈ +740 | ≈ 1.0% | +2.9 [0.8, 5.2] |
| C_KTNT | +3,116 | ≈ +780 | ≈ 1.0% | +0.8 (n.s.) |
| N_NEUTRAL | +2,945 | ≈ +740 | ≈ 1.0% | −1.4 [−3.2, −0.1] |
| B_CLARITY | +6,662 | ≈ +1,670 | ≈ 2.2% | +0.6 (n.s.) |
| BI_CLARITY | +6,965 | ≈ +1,740 | ≈ 2.3% | +1.4 (n.s.) |

## Conclusions

1. **Prompt-level framework cost is negligible** (1–2% of a run) compared with fixed agent overhead and the evidence itself. Token cost is not a reason to choose or reject a method. Accuracy and human effort are.
2. **The evidence file is the cost driver.** It is 18–21k characters per case, about 80% of the prompt. The adaptive experiment shows that reading 6 of ~17 items (≈ 35% of evidence) retains ~88% of full-evidence performance when the analyzer chooses. In AI-assisted diagnosis, evidence volume is where efficiency gains lie. In human diagnosis, evidence *collection* (interviews, data pulls) is where cost lies.
3. EDF v0.3 is the most expensive single-framework prompt and the least accurate per token. D achieves at least the same accuracy with 43% less framework text.

## Programme cost (for planning replications)

| Phase | Analyzer and grader runs |
|---|---|
| Case authoring + review | 44 agent runs (~0.75–1.1M tokens per 8) |
| Dev | 80 |
| Held-out recognition | 144 |
| Haiku replication | 48 |
| Open generation | 60 + 80 grades |
| Adaptive | 24 + 48 |
| **Total analyzer and grader runs** | **484** (≈ 37M host-reported subagent tokens) |
