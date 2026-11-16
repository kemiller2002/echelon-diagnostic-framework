# Regression analysis

Before any candidate could be recommended, I checked whether it made any metric or class *worse* than EDF v0.3.

## D_EDFX vs B_EDF03 (held-out, Sonnet)

| Metric | B | D | Regression? |
|---|---|---|---|
| False diagnosis | 0/102 | 0/102 | No |
| Missed problem | 1/40 | 2/40 | Possible (+1 item; n.s.) |
| Unsupported certainty | 2/44 | 2/44 | No |
| Causal overclaim | 1/64 | 0/64 | No |
| Harmful promotion | 0/44 | 0/44 | No |
| Revision (staged evidence) | 0.96 | 0.93 | Possible (1 item) |
| Feedback-loop items | 1.00 | 1.00 | No |

Per case:
- **Worse for D:** HO-F (96.3 vs 98.6, a complex-system loop case), HO-H (96.6 vs 97.6) and HO-J1 (94.6 vs 96.6).
- **Better for D:** HO-D (+5.9), HO-E (+7.2), HO-J2 (+8.5).

Haiku: D was worse than B on HO-G (−3.8) and HO-I (−4.1), and better on HO-C (+4.6) and HO-D (+8.9).

**Assessment:** no systematic regression, but D's per-case effects are noisy in both directions. D's dropping of a separate propagation step coincides with its weakest case (HO-F, a feedback loop). Any simplification should keep loop and delay annotation (CI-02, CI-06).

## Clarity variants vs B

B_CLARITY: abstention accuracy fell (0.83 vs 0.92) and hidden-problem detection fell (0.91 vs 0.96). BI_CLARITY showed no regression. These are small counts. Neither variant is recommended.

## EDF-0 vs B

B0 was worse on causal accuracy (0.91 vs 0.94), external-cause recognition (0.86 vs 0.95) and metric gaming (0.89 vs 0.92). This is consistent with a thinner prompt omitting cues.

## Neutral protocol

N regressed on Haiku across action accuracy (0.89), abstention (0.75) and false diagnosis (3/51). This is the clearest regression found.
