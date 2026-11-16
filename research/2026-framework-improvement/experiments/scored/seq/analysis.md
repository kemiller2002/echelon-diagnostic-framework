# OD1-S results (held-out, Sonnet)

| Condition | Runs | Mean score | Items read (mean) | Share of catalogue | Required-evidence coverage | Full-evidence score (same module) | Δ vs full [95% CI] |
|---|---|---|---|---|---|---|---|
| A_GENERAL | 24 | 91.8 | 11.8 | 70% | 82% | 93.1 | -1.3 [-3.5, +1.9] |
| B_EDF03 | 12 | 91.9 | 12.3 | 73% | 87% | 94.5 | -2.5 [-6.6, +2.1] |
| C_KTNT | 12 | 90.9 | 10.1 | 60% | 75% | 93.9 | -2.9 [-6.9, +1.2] |
| D_EDFX | 24 | 93.2 | 11.0 | 66% | 80% | 95.9 | -2.7 [-5.2, -0.6] |
| A_GENERAL_K6 | 12 | 85.7 | 6.5 | 39% | 51% | 93.1 | -7.3 [-11.7, -3.2] |
| D_EDFX_K6 | 12 | 89.9 | 7.4 | 44% | 59% | 95.9 | -6.1 [-9.9, -2.8] |

## Sequential vs batch selection at budget 6

- A_GENERAL_K6 − SEL_A_GENERAL (batch mean 84.4): +1.3 [-2.7, +5.7]
- D_EDFX_K6 − SEL_D_EDFX (batch mean 84.2): +5.7 [+0.8, +10.0]

## Between-method contrasts (free stopping)

- D_EDFX − A_GENERAL: score +1.5 [-0.3, +3.4]; items read -0.7 [-1.7, +0.3]
- B_EDF03 − A_GENERAL: score +0.2 [-1.1, +1.2]; items read +0.6 [-0.5, +2.0]
- C_KTNT − A_GENERAL: score -0.8 [-2.6, +0.6]; items read -1.7 [-2.6, -0.7]
- D_EDFX − B_EDF03: score +1.3 [-1.0, +3.8]; items read -1.3 [-2.9, +0.2]
- reads vs full-evidence difficulty (A_GENERAL): r = +0.04
- reads vs full-evidence difficulty (D_EDFX): r = -0.24
- reads vs full-evidence difficulty (B_EDF03): r = +0.08
- reads vs full-evidence difficulty (C_KTNT): r = -0.04
