## Condition score distributions (run level)

| phase | condition | runs | mean | SD | min | median | max | share ≥95 | mean item acc |
|---|---|---|---|---|---|---|---|---|---|
| dev | A_GENERAL | 10 | 91.44 | 6.58 | 82.68 | 94.17 | 99.34 | 50% | 0.928 |
| dev | B0_EDF0 | 10 | 93.15 | 6.98 | 78.48 | 95.21 | 99.51 | 50% | 0.948 |
| dev | BI_CLARITY | 10 | 95.07 | 4.97 | 82.82 | 95.57 | 99.22 | 70% | 0.966 |
| dev | B_CLARITY | 10 | 94.02 | 4.92 | 83.18 | 94.70 | 99.02 | 40% | 0.960 |
| dev | B_EDF03 | 10 | 93.54 | 5.84 | 82.48 | 95.44 | 99.21 | 50% | 0.953 |
| dev | CL_ALONE | 10 | 93.51 | 5.89 | 83.24 | 94.69 | 99.57 | 40% | 0.953 |
| dev | C_KTNT | 10 | 92.84 | 5.93 | 82.50 | 93.63 | 99.47 | 40% | 0.944 |
| dev | N_NEUTRAL | 10 | 94.80 | 5.02 | 83.32 | 96.28 | 99.38 | 70% | 0.968 |
| heldout | A_GENERAL | 12 | 93.06 | 6.29 | 81.98 | 94.55 | 98.86 | 42% | 0.940 |
| heldout | B0_EDF0 | 12 | 92.37 | 6.59 | 79.37 | 92.40 | 99.20 | 42% | 0.931 |
| heldout | BI_CLARITY | 12 | 94.43 | 5.48 | 82.64 | 97.38 | 99.13 | 58% | 0.956 |
| heldout | B_CLARITY | 12 | 93.66 | 4.78 | 85.03 | 95.08 | 98.98 | 50% | 0.952 |
| heldout | B_EDF03 | 24 | 94.47 | 5.62 | 81.97 | 96.77 | 99.28 | 62% | 0.956 |
| heldout | C_KTNT | 12 | 93.87 | 6.41 | 82.20 | 96.87 | 99.30 | 58% | 0.948 |
| heldout | D_EDFX | 24 | 95.95 | 3.34 | 87.62 | 96.95 | 99.29 | 67% | 0.977 |
| heldout | D_NOGATE | 12 | 93.14 | 6.82 | 78.97 | 97.09 | 99.21 | 58% | 0.942 |
| heldout | D_NOORG | 12 | 94.42 | 5.80 | 81.71 | 97.09 | 99.55 | 67% | 0.958 |
| heldout | N_NEUTRAL | 12 | 91.69 | 7.73 | 72.96 | 93.03 | 99.21 | 33% | 0.925 |
| haiku | A_GENERAL | 12 | 94.25 | 5.42 | 80.06 | 96.46 | 98.63 | 58% | 0.974 |
| haiku | B_EDF03 | 12 | 92.65 | 7.78 | 71.34 | 95.12 | 98.60 | 50% | 0.950 |
| haiku | D_EDFX | 12 | 92.78 | 5.30 | 80.30 | 93.16 | 98.54 | 33% | 0.952 |
| haiku | N_NEUTRAL | 12 | 87.21 | 7.14 | 75.45 | 87.59 | 97.45 | 17% | 0.913 |

### Primary score: paired case-level contrasts (trials averaged; bootstrap seed 20261116, 10000 resamples; exact sign-flip p over 2^n patterns; Holm over these 15)

| phase | contrast | n | mean A | mean B | diff | 95% boot CI | SD diff | dz | sign-flip p | Holm p | W/L/T |
|---|---|---|---|---|---|---|---|---|---|---|---|
| heldout | B_EDF03 − A_GENERAL | 12 | 94.47 | 93.06 | **+1.40** | [-0.72, +3.13] | 3.59 | +0.39 | 0.2026 | 1.0000 | 9/3/0 |
| heldout | B_EDF03 − N_NEUTRAL | 12 | 94.47 | 91.69 | **+2.77** | [+0.76, +5.33] | 4.23 | +0.66 | 0.0195 | 0.2539 | 10/2/0 |
| heldout | D_EDFX − B_EDF03 | 12 | 95.95 | 94.47 | **+1.48** | [-0.31, +3.59] | 3.63 | +0.41 | 0.2104 | 1.0000 | 6/6/0 |
| heldout | D_EDFX − N_NEUTRAL | 12 | 95.95 | 91.69 | **+4.25** | [+1.53, +7.90] | 5.82 | +0.73 | 0.0063 | 0.0952 | 11/1/0 |
| heldout | C_KTNT − B_EDF03 | 12 | 93.87 | 94.47 | **-0.59** | [-2.02, +0.68] | 2.49 | -0.24 | 0.4639 | 1.0000 | 8/4/0 |
| heldout | B0_EDF0 − B_EDF03 | 12 | 92.37 | 94.47 | **-2.10** | [-3.92, -0.48] | 3.20 | -0.66 | 0.0552 | 0.6621 | 3/9/0 |
| heldout | B_CLARITY − B_EDF03 | 12 | 93.66 | 94.47 | **-0.80** | [-2.25, +0.79] | 2.80 | -0.29 | 0.3350 | 1.0000 | 3/9/0 |
| heldout | BI_CLARITY − B_EDF03 | 12 | 94.43 | 94.47 | **-0.04** | [-1.69, +1.62] | 3.09 | -0.01 | 0.9771 | 1.0000 | 6/6/0 |
| heldout | D_EDFX − D_NOGATE | 12 | 95.95 | 93.14 | **+2.80** | [+0.44, +5.65] | 4.85 | +0.58 | 0.0557 | 0.6621 | 8/4/0 |
| heldout | D_EDFX − D_NOORG | 12 | 95.95 | 94.42 | **+1.53** | [-0.23, +3.62] | 3.55 | +0.43 | 0.1899 | 1.0000 | 7/5/0 |
| haiku | B_EDF03 − A_GENERAL | 12 | 92.65 | 94.25 | **-1.60** | [-3.92, +0.94] | 4.44 | -0.36 | 0.2212 | 1.0000 | 4/8/0 |
| haiku | D_EDFX − B_EDF03 | 12 | 92.78 | 92.65 | **+0.13** | [-1.94, +2.37] | 4.03 | +0.03 | 0.9497 | 1.0000 | 4/8/0 |
| haiku | N_NEUTRAL − A_GENERAL | 12 | 87.21 | 94.25 | **-7.04** | [-11.16, -3.14] | 7.31 | -0.96 | 0.0063 | 0.0952 | 2/10/0 |
| dev | B_EDF03 − A_GENERAL | 10 | 93.54 | 91.44 | **+2.10** | [-0.35, +5.24] | 4.84 | +0.43 | 0.2070 | 1.0000 | 6/4/0 |
| dev | B_EDF03 − N_NEUTRAL | 10 | 93.54 | 94.80 | **-1.25** | [-5.39, +3.08] | 7.14 | -0.18 | 0.5645 | 1.0000 | 3/7/0 |

### Haiku contrasts with lenient request-id parsing ("Q1: title" counted as Q1) — Holm within these 3 only

| phase | contrast | n | mean A | mean B | diff | 95% boot CI | SD diff | dz | sign-flip p | Holm p | W/L/T |
|---|---|---|---|---|---|---|---|---|---|---|---|
| haiku | B_EDF03 − A_GENERAL | 12 | 92.65 | 94.25 | **-1.60** | [-3.92, +0.94] | 4.44 | -0.36 | 0.2212 | 0.4424 | 4/8/0 |
| haiku | D_EDFX − B_EDF03 | 12 | 92.78 | 92.65 | **+0.13** | [-1.94, +2.37] | 4.03 | +0.03 | 0.9497 | 0.9497 | 4/8/0 |
| haiku | N_NEUTRAL − A_GENERAL | 12 | 88.88 | 94.25 | **-5.37** | [-8.69, -2.11] | 6.12 | -0.88 | 0.0127 | 0.0381 | 3/9/0 |

### Sensitivity (b): accuracy-only score (100 × weighted item accuracy)

| phase | contrast | n | mean A | mean B | diff | 95% boot CI | SD diff | dz | sign-flip p | Holm p | W/L/T |
|---|---|---|---|---|---|---|---|---|---|---|---|
| heldout | B_EDF03 − A_GENERAL | 12 | 95.59 | 94.00 | **+1.59** | [-0.91, +3.61] | 4.20 | +0.38 | 0.2188 | 1.0000 | 6/1/5 |
| heldout | B_EDF03 − N_NEUTRAL | 12 | 95.59 | 92.46 | **+3.13** | [+0.81, +6.18] | 4.97 | +0.63 | 0.0312 | 0.4062 | 6/1/5 |
| heldout | D_EDFX − B_EDF03 | 12 | 97.72 | 95.59 | **+2.13** | [+0.02, +4.67] | 4.30 | +0.50 | 0.1562 | 1.0000 | 4/2/6 |
| heldout | D_EDFX − N_NEUTRAL | 12 | 97.72 | 92.46 | **+5.26** | [+2.12, +9.60] | 6.88 | +0.77 | 0.0078 | 0.1172 | 8/0/4 |
| heldout | C_KTNT − B_EDF03 | 12 | 94.83 | 95.59 | **-0.76** | [-2.34, +0.66] | 2.81 | -0.27 | 0.4375 | 1.0000 | 2/3/7 |
| heldout | B0_EDF0 − B_EDF03 | 12 | 93.08 | 95.59 | **-2.51** | [-4.78, -0.49] | 3.98 | -0.63 | 0.0938 | 0.9375 | 1/5/6 |
| heldout | B_CLARITY − B_EDF03 | 12 | 95.25 | 95.59 | **-0.34** | [-1.90, +1.38] | 3.00 | -0.11 | 0.7188 | 1.0000 | 2/4/6 |
| heldout | BI_CLARITY − B_EDF03 | 12 | 95.62 | 95.59 | **+0.04** | [-2.02, +2.07] | 3.79 | +0.01 | 0.9688 | 1.0000 | 4/3/5 |
| heldout | D_EDFX − D_NOGATE | 12 | 97.72 | 94.16 | **+3.56** | [+0.83, +6.85] | 5.62 | +0.63 | 0.0625 | 0.7500 | 5/0/7 |
| heldout | D_EDFX − D_NOORG | 12 | 97.72 | 95.79 | **+1.93** | [+0.01, +4.28] | 3.95 | +0.49 | 0.1250 | 1.0000 | 4/1/7 |
| haiku | B_EDF03 − A_GENERAL | 12 | 94.95 | 97.43 | **-2.47** | [-4.92, -0.31] | 4.23 | -0.58 | 0.0625 | 0.7500 | 1/5/6 |
| haiku | D_EDFX − B_EDF03 | 12 | 95.18 | 94.95 | **+0.23** | [-2.22, +2.90] | 4.78 | +0.05 | 0.9062 | 1.0000 | 3/3/6 |
| haiku | N_NEUTRAL − A_GENERAL | 12 | 91.30 | 97.43 | **-6.13** | [-9.34, -2.94] | 5.93 | -1.03 | 0.0078 | 0.1172 | 1/9/2 |
| dev | B_EDF03 − A_GENERAL | 10 | 95.31 | 92.79 | **+2.52** | [-0.42, +6.45] | 6.06 | +0.42 | 0.3750 | 1.0000 | 3/1/6 |
| dev | B_EDF03 − N_NEUTRAL | 10 | 95.31 | 96.80 | **-1.49** | [-6.67, +4.00] | 8.97 | -0.17 | 0.5625 | 1.0000 | 2/4/4 |

### Sensitivity (a): items every condition got wrong

Dev items wrong in all 8 dev runs (possible truth-key ambiguity): OD-DEV-I/I7 (expected contradicted; given association-only)

Held-out items wrong in all 144 Sonnet held-out runs with the identical wrong answer: OD-HO-J2/I7 (expected contradicted; all answered association-only)

Held-out items wrong in all held-out runs (any wrong answer): OD-HO-J2/I7 (expected contradicted; given association-only)

Held-out items wrong in ≥90% of Sonnet runs: OD-HO-J2/I7 100% wrong (expected contradicted; modal answer association-only)

### Sensitivity (a): held-out contrasts excluding items wrong in every held-out run (Holm within these 10)

| phase | contrast | n | mean A | mean B | diff | 95% boot CI | SD diff | dz | sign-flip p | Holm p | W/L/T |
|---|---|---|---|---|---|---|---|---|---|---|---|
| heldout | B_EDF03 − A_GENERAL | 12 | 94.85 | 93.45 | **+1.40** | [-0.72, +3.13] | 3.59 | +0.39 | 0.2051 | 1.0000 | 8/4/0 |
| heldout | B_EDF03 − N_NEUTRAL | 12 | 94.85 | 92.08 | **+2.77** | [+0.75, +5.32] | 4.23 | +0.65 | 0.0200 | 0.1802 | 10/2/0 |
| heldout | D_EDFX − B_EDF03 | 12 | 96.39 | 94.85 | **+1.53** | [-0.31, +3.72] | 3.74 | +0.41 | 0.2104 | 1.0000 | 6/6/0 |
| heldout | D_EDFX − N_NEUTRAL | 12 | 96.39 | 92.08 | **+4.30** | [+1.55, +7.95] | 5.86 | +0.73 | 0.0063 | 0.0635 | 11/1/0 |
| heldout | C_KTNT − B_EDF03 | 12 | 94.27 | 94.85 | **-0.59** | [-2.02, +0.69] | 2.49 | -0.24 | 0.4648 | 1.0000 | 8/4/0 |
| heldout | B0_EDF0 − B_EDF03 | 12 | 92.76 | 94.85 | **-2.10** | [-3.92, -0.47] | 3.21 | -0.65 | 0.0562 | 0.4375 | 4/8/0 |
| heldout | B_CLARITY − B_EDF03 | 12 | 94.09 | 94.85 | **-0.76** | [-2.25, +0.90] | 2.90 | -0.26 | 0.3696 | 1.0000 | 3/9/0 |
| heldout | BI_CLARITY − B_EDF03 | 12 | 94.85 | 94.85 | **-0.01** | [-1.69, +1.68] | 3.15 | -0.00 | 0.9951 | 1.0000 | 6/6/0 |
| heldout | D_EDFX − D_NOGATE | 12 | 96.39 | 93.56 | **+2.82** | [+0.46, +5.67] | 4.85 | +0.58 | 0.0547 | 0.4375 | 8/4/0 |
| heldout | D_EDFX − D_NOORG | 12 | 96.39 | 94.81 | **+1.58** | [-0.23, +3.75] | 3.66 | +0.43 | 0.1899 | 1.0000 | 7/5/0 |

### Sensitivity (a'): held-out contrasts excluding items wrong in ≥90% of held-out runs (Holm within these 10)

| phase | contrast | n | mean A | mean B | diff | 95% boot CI | SD diff | dz | sign-flip p | Holm p | W/L/T |
|---|---|---|---|---|---|---|---|---|---|---|---|
| heldout | B_EDF03 − A_GENERAL | 12 | 94.85 | 93.45 | **+1.40** | [-0.72, +3.13] | 3.59 | +0.39 | 0.2051 | 1.0000 | 8/4/0 |
| heldout | B_EDF03 − N_NEUTRAL | 12 | 94.85 | 92.08 | **+2.77** | [+0.75, +5.32] | 4.23 | +0.65 | 0.0200 | 0.1802 | 10/2/0 |
| heldout | D_EDFX − B_EDF03 | 12 | 96.39 | 94.85 | **+1.53** | [-0.31, +3.72] | 3.74 | +0.41 | 0.2104 | 1.0000 | 6/6/0 |
| heldout | D_EDFX − N_NEUTRAL | 12 | 96.39 | 92.08 | **+4.30** | [+1.55, +7.95] | 5.86 | +0.73 | 0.0063 | 0.0635 | 11/1/0 |
| heldout | C_KTNT − B_EDF03 | 12 | 94.27 | 94.85 | **-0.59** | [-2.02, +0.69] | 2.49 | -0.24 | 0.4648 | 1.0000 | 8/4/0 |
| heldout | B0_EDF0 − B_EDF03 | 12 | 92.76 | 94.85 | **-2.10** | [-3.92, -0.47] | 3.21 | -0.65 | 0.0562 | 0.4375 | 4/8/0 |
| heldout | B_CLARITY − B_EDF03 | 12 | 94.09 | 94.85 | **-0.76** | [-2.25, +0.90] | 2.90 | -0.26 | 0.3696 | 1.0000 | 3/9/0 |
| heldout | BI_CLARITY − B_EDF03 | 12 | 94.85 | 94.85 | **-0.01** | [-1.69, +1.68] | 3.15 | -0.00 | 0.9951 | 1.0000 | 6/6/0 |
| heldout | D_EDFX − D_NOGATE | 12 | 96.39 | 93.56 | **+2.82** | [+0.46, +5.67] | 4.85 | +0.58 | 0.0547 | 0.4375 | 8/4/0 |
| heldout | D_EDFX − D_NOORG | 12 | 96.39 | 94.81 | **+1.58** | [-0.23, +3.75] | 3.66 | +0.43 | 0.1899 | 1.0000 | 7/5/0 |

### Sensitivity (a): haiku contrasts excluding the same held-out items

| phase | contrast | n | mean A | mean B | diff | 95% boot CI | SD diff | dz | sign-flip p | Holm p | W/L/T |
|---|---|---|---|---|---|---|---|---|---|---|---|
| haiku | B_EDF03 − A_GENERAL | 12 | 92.65 | 94.25 | **-1.60** | [-3.92, +0.94] | 4.44 | -0.36 | 0.2217 | 0.4434 | 4/8/0 |
| haiku | D_EDFX − B_EDF03 | 12 | 92.76 | 92.65 | **+0.11** | [-1.99, +2.36] | 4.06 | +0.03 | 0.9507 | 0.9507 | 4/8/0 |
| haiku | N_NEUTRAL − A_GENERAL | 12 | 87.67 | 94.25 | **-6.58** | [-10.89, -2.61] | 7.61 | -0.86 | 0.0117 | 0.0352 | 3/9/0 |

### Sensitivity (c): per-benchmark-class differences (case-level, trials averaged)

| phase | case | class | D_EDFX − B_EDF03 | B_EDF03 − A_GENERAL | B_EDF03 − N_NEUTRAL |
|---|---|---|---|---|---|
| heldout | OD-HO-A | A | -0.41 | +0.29 | +0.47 |
| heldout | OD-HO-B | B | +0.34 | -0.42 | +0.06 |
| heldout | OD-HO-C | C | +0.01 | +2.96 | +3.55 |
| heldout | OD-HO-D | D | +5.94 | +5.06 | +14.08 |
| heldout | OD-HO-E | E | +7.17 | -7.67 | -1.83 |
| heldout | OD-HO-F | F | -2.35 | -0.27 | +0.10 |
| heldout | OD-HO-G | G | -0.03 | +3.71 | +4.18 |
| heldout | OD-HO-H | H | -1.01 | +2.87 | +2.99 |
| heldout | OD-HO-I | I | +1.75 | +5.63 | +5.12 |
| heldout | OD-HO-J1 | J | -2.03 | +4.22 | +4.69 |
| heldout | OD-HO-J2 | J | +8.41 | +0.01 | -0.19 |
| heldout | OD-HO-K | K | -0.01 | +0.43 | +0.06 |
| haiku | OD-HO-A | A | +3.32 | -4.27 | -3.09 |
| haiku | OD-HO-B | B | -0.12 | -5.20 | +5.55 |
| haiku | OD-HO-C | C | +4.50 | -4.47 | -8.03 |
| haiku | OD-HO-D | D | +8.96 | -8.71 | -5.82 |
| haiku | OD-HO-E | E | -1.01 | -5.53 | +15.98 |
| haiku | OD-HO-F | F | -0.20 | +8.63 | +6.71 |
| haiku | OD-HO-G | G | -3.89 | +1.12 | +7.65 |
| haiku | OD-HO-H | H | +0.37 | -0.13 | +15.08 |
| haiku | OD-HO-I | I | -4.09 | -0.52 | +9.20 |
| haiku | OD-HO-J1 | J | -0.22 | +0.10 | +4.27 |
| haiku | OD-HO-J2 | J | -5.81 | +0.20 | +5.31 |
| haiku | OD-HO-K | K | -0.27 | -0.44 | +12.44 |
| dev | OD-DEV-A | A | – | -3.00 | -3.03 |
| dev | OD-DEV-B | B | – | +0.64 | +0.73 |
| dev | OD-DEV-C | C | – | +13.58 | +13.40 |
| dev | OD-DEV-D | D | – | +6.81 | -0.02 |
| dev | OD-DEV-E | E | – | -0.73 | -0.08 |
| dev | OD-DEV-F | F | – | +3.80 | +2.71 |
| dev | OD-DEV-G | G | – | -0.17 | -0.19 |
| dev | OD-DEV-H | H | – | +0.31 | -5.11 |
| dev | OD-DEV-I | I | – | -0.21 | -6.47 |
| dev | OD-DEV-K | K | – | +0.01 | -14.47 |

### Power for detecting +5 points (paired t, two-sided α=0.05, 80% power)

| contrast | observed SD of case diffs | n needed for +5 | power at n=12 (or observed n) for +5 | n needed for +3 |
|---|---|---|---|---|
| heldout B_EDF03 − A_GENERAL | 3.59 | 6 | 0.99 | 14 |
| heldout B_EDF03 − N_NEUTRAL | 4.23 | 8 | 0.96 | 18 |
| heldout D_EDFX − B_EDF03 | 3.63 | 6 | 0.99 | 14 |
| heldout D_EDFX − N_NEUTRAL | 5.82 | 13 | 0.77 | 32 |
| heldout C_KTNT − B_EDF03 | 2.49 | 3 | 1.00 | 8 |
| heldout B0_EDF0 − B_EDF03 | 3.20 | 6 | 1.00 | 11 |
| heldout B_CLARITY − B_EDF03 | 2.80 | 4 | 1.00 | 9 |
| heldout BI_CLARITY − B_EDF03 | 3.09 | 5 | 1.00 | 11 |
| heldout D_EDFX − D_NOGATE | 4.85 | 10 | 0.90 | 23 |
| heldout D_EDFX − D_NOORG | 3.55 | 6 | 0.99 | 13 |
| haiku B_EDF03 − A_GENERAL | 4.44 | 9 | 0.94 | 20 |
| haiku D_EDFX − B_EDF03 | 4.03 | 7 | 0.97 | 17 |
| haiku N_NEUTRAL − A_GENERAL | 7.31 | 19 | 0.57 | 49 |
| dev B_EDF03 − A_GENERAL | 4.84 | 10 | 0.83 | 23 |
| dev B_EDF03 − N_NEUTRAL | 7.14 | 18 | 0.48 | 47 |

### Trial-to-trial noise (held-out, B_EDF03 and D_EDFX two trials)

| condition | item agreement t1 vs t2 | mean |score t1 − t2| | SD of (t1 − t2) |
|---|---|---|---|
| B_EDF03 | 0.975 (193/198) | 1.89 | 3.33 |
| D_EDFX | 0.975 (193/198) | 2.75 | 4.33 |
