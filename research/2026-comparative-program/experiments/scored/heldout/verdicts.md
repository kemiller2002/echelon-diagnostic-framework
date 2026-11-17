# OD2 preregistered verdicts

Decision rule (preregistration §4): material advantage = Δ ≥ +5 and CI > 0; practically equivalent = CI within [−3, +3]; material disadvantage = Δ ≤ −5 and CI < 0; otherwise inconclusive. Accuracy and acquisition are on a 0–100 scale for comparability.

| id | comparison | contrast | cases | Δ score [95% CI] | verdict | Δ accuracy [95% CI] | Δ acquisition [95% CI] |
|---|---|---|---|---|---|---|---|
| H1 | EDF vs no framework | B_EDF03@sonnet-medium − A_GENERAL@sonnet-medium | 24 | -0.98 [-4.53, 1.36] | **inconclusive** | -1.68 [-5.18, 0.51] | +1.00 [-3.64, 5.47] |
| H2 | EDF vs Kepner-Tregoe | B_EDF03@sonnet-medium − C_KT@sonnet-medium | 24 | +0.11 [-3.05, 2.58] | **inconclusive** | -0.15 [-3.08, 2.16] | +1.46 [-2.99, 5.97] |
| H3 | EDF vs KT+ACH | B_EDF03@sonnet-medium − D_KTACH@sonnet-medium | 24 | +0.82 [-3.12, 5.40] | **inconclusive** | +1.28 [-2.83, 6.26] | -0.21 [-4.31, 3.68] |
| H4 | gates+VOI vs EDF | F_EDFG@sonnet-medium − B_EDF03@sonnet-medium | 24 | +0.95 [-2.11, 5.16] | **inconclusive** | +1.69 [-1.14, 5.92] | -1.81 [-8.33, 4.17] |
| H5a | System Context | B_EDF03@sonnet-medium − E_NOCTX@sonnet-medium | 24 | +1.08 [-3.27, 6.57] | **inconclusive** | +1.11 [-3.06, 6.52] | -1.04 [-6.11, 4.03] |
| H5b | Origin Network + Propagation | B_EDF03@sonnet-medium − E_NOCAUSAL@sonnet-medium | 24 | -1.00 [-4.26, 1.39] | **inconclusive** | -0.84 [-4.09, 1.45] | -1.25 [-5.83, 3.33] |
| H5c | Evidence evaluation | B_EDF03@sonnet-medium − E_NOEVID@sonnet-medium | 24 | -0.35 [-4.32, 2.98] | **inconclusive** | -0.84 [-4.79, 2.19] | +1.67 [-4.44, 8.19] |
| H5d | Control Points + Ranking | B_EDF03@sonnet-medium − E_NOCTRL@sonnet-medium | 24 | -0.63 [-3.92, 1.77] | **inconclusive** | -0.17 [-3.60, 2.55] | -2.29 [-6.94, 2.01] |
| H5e | Diagnostic Sufficiency | B_EDF03@sonnet-medium − E_NOSUFF@sonnet-medium | 24 | -1.31 [-4.74, 0.85] | **inconclusive** | -1.40 [-4.95, 0.86] | -0.83 [-4.72, 2.22] |
| H6 | Sequence | B_EDF03@sonnet-medium − E_NOSEQ@sonnet-medium | 24 | -0.35 [-3.39, 2.01] | **inconclusive** | -0.33 [-3.23, 1.89] | +0.21 [-4.44, 5.21] |
| H7a | vote-of-5 (no framework) vs EDF | A_VOTE5@sonnet-medium − B_EDF03@sonnet-medium | 24 | -1.48 [-5.45, 3.07] | **inconclusive** | +2.19 [-0.26, 5.78] | -17.92 [-33.75, -3.61] |
| H9a | Claude-selected method vs KT | SEL_CLAUDE@sonnet-medium − C_KT@sonnet-medium | 24 | +0.52 [-1.11, 2.43] | **practically equivalent** | +0.69 [-1.15, 2.85] | +0.42 [-2.92, 3.96] |
| H9b | EDF orchestrator vs EDF | SEL_EDFO@sonnet-medium − B_EDF03@sonnet-medium | 24 | +0.31 [-3.51, 4.78] | **inconclusive** | +1.14 [-1.86, 5.43] | -2.71 [-10.69, 4.24] |
| H11-H1 | Haiku: EDF vs no framework | B_EDF03@haiku-low − A_GENERAL@haiku-low | 24 | -2.08 [-4.72, 0.24] | **inconclusive** | -1.09 [-3.09, 0.78] | -4.79 [-13.12, 2.71] |
| H11-H2 | Haiku: EDF vs KT | B_EDF03@haiku-low − C_KT@haiku-low | 24 | -3.31 [-5.57, -1.42] | **inconclusive** | -2.13 [-3.82, -0.52] | -7.08 [-14.58, -1.04] |
| H11-H4 | Haiku: gates+VOI vs EDF | F_EDFG@haiku-low − B_EDF03@haiku-low | 24 | +0.30 [-2.44, 3.04] | **inconclusive** | -0.75 [-3.03, 1.51] | +2.29 [-4.58, 9.58] |

## H10 — item test–retest agreement (trials 1 vs 2, Sonnet-medium)

| condition | item pairs | agreement [95% Wilson] |
|---|---|---|
| A_GENERAL | 349 | 0.963 [0.937, 0.978] |
| B_EDF03 | 349 | 0.963 [0.937, 0.978] |
| C_KT | 349 | 0.966 [0.941, 0.980] |
| D_KTACH | 349 | 0.911 [0.877, 0.937] |
| F_EDFG | 349 | 0.954 [0.927, 0.972] |
| SEL_CLAUDE | 349 | 0.948 [0.920, 0.967] |
| SEL_EDFO | 349 | 0.960 [0.934, 0.976] |

## Class and domain breakdown (Sonnet-medium; case means, trials averaged)

| class | A_GENERAL score / acc | B_EDF03 score / acc | C_KT score / acc | D_KTACH score / acc | F_EDFG score / acc |
|---|---|---|---|---|---|
| ADVERS | 92.1 / 0.95 | 93.0 / 0.96 | 91.5 / 0.96 | 91.9 / 0.96 | 97.1 / 0.98 |
| CHANGE | 93.5 / 0.93 | 93.4 / 0.93 | 92.5 / 0.92 | 94.8 / 0.95 | 93.0 / 0.92 |
| CONFOUND | 94.9 / 0.95 | 72.2 / 0.72 | 78.4 / 0.80 | 64.6 / 0.61 | 86.7 / 0.88 |
| CONTRA | 76.4 / 0.79 | 78.0 / 0.81 | 73.9 / 0.75 | 75.2 / 0.77 | 82.0 / 0.83 |
| CROSS | 96.1 / 0.97 | 94.8 / 0.95 | 96.0 / 0.97 | 96.2 / 0.97 | 96.1 / 0.97 |
| INCOMPLETE | 84.0 / 0.99 | 91.1 / 0.98 | 86.7 / 0.95 | 91.7 / 1.00 | 81.4 / 1.00 |
| INTERACT | 95.8 / 0.95 | 96.1 / 0.96 | 95.8 / 0.95 | 96.9 / 0.97 | 96.9 / 0.97 |
| KNOWN | 95.6 / 0.96 | 95.5 / 0.96 | 97.6 / 0.98 | 95.8 / 0.96 | 95.3 / 0.95 |
| MISLEAD | 98.2 / 1.00 | 98.2 / 0.99 | 98.5 / 1.00 | 98.6 / 1.00 | 98.3 / 1.00 |
| NOCONC | 76.2 / 0.78 | 76.7 / 0.78 | 75.3 / 0.76 | 77.7 / 0.80 | 75.9 / 0.77 |
| OBVWRONG | 92.1 / 0.98 | 96.5 / 0.99 | 96.1 / 1.00 | 98.0 / 0.99 | 92.2 / 0.99 |
| SEQ | 96.0 / 0.98 | 97.1 / 0.97 | 97.9 / 0.98 | 95.8 / 0.95 | 94.8 / 0.97 |
| STOP | 98.5 / 1.00 | 98.6 / 1.00 | 97.2 / 0.98 | 95.2 / 0.95 | 97.8 / 0.99 |
| organizational | 87.2 / 0.90 | 87.0 / 0.89 | 83.9 / 0.87 | 81.3 / 0.83 | 85.6 / 0.89 |
| technical | 92.1 / 0.94 | 89.2 / 0.90 | 92.3 / 0.93 | 93.0 / 0.93 | 93.5 / 0.96 |
| cross-domain | 97.2 / 0.98 | 97.3 / 0.98 | 97.4 / 0.98 | 97.4 / 0.98 | 97.6 / 0.98 |

## M5 — evidence collected by class (mean items read, Sonnet-medium)

| class | A_GENERAL | B_EDF03 | E_NOSUFF | F_EDFG | C_KT |
|---|---|---|---|---|---|
| ADVERS | 2.00 | 2.00 | 2.00 | 2.00 | 2.00 |
| CHANGE | 2.00 | 2.00 | 2.00 | 2.00 | 2.00 |
| CONFOUND | 2.00 | 1.67 | 2.00 | 1.67 | 2.00 |
| CONTRA | 2.00 | 2.00 | 2.00 | 2.00 | 2.00 |
| CROSS | 2.00 | 2.00 | 2.00 | 2.00 | 2.00 |
| INCOMPLETE | 2.00 | 2.00 | 2.00 | 2.00 | 2.00 |
| INTERACT | 2.00 | 2.00 | 2.00 | 2.00 | 2.00 |
| KNOWN | 2.00 | 2.00 | 2.00 | 2.00 | 2.00 |
| MISLEAD | 2.00 | 2.00 | 2.00 | 2.00 | 2.00 |
| NOCONC | 2.00 | 2.00 | 2.00 | 2.00 | 2.00 |
| OBVWRONG | 1.80 | 1.83 | 2.00 | 1.67 | 1.75 |
| SEQ | 2.00 | 2.00 | 2.00 | 1.83 | 2.00 |
| STOP | 0.00 | 0.00 | 0.00 | 0.00 | 0.00 |
