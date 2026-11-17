# OD2 preregistered verdicts

Decision rule (preregistration §4): material advantage = Δ ≥ +5 and CI > 0; practically equivalent = CI within [−3, +3]; material disadvantage = Δ ≤ −5 and CI < 0; otherwise inconclusive. Accuracy and acquisition are on a 0–100 scale for comparability.

| id | comparison | contrast | cases | Δ score [95% CI] | verdict | Δ accuracy [95% CI] | Δ acquisition [95% CI] |
|---|---|---|---|---|---|---|---|
| H1 | EDF vs no framework | B_EDF03@sonnet-medium − A_GENERAL@sonnet-medium | 24 | +0.19 [-1.21, 1.45] | **practically equivalent** | -0.58 [-1.91, 0.52] | +2.39 [-1.00, 6.11] |
| H2 | EDF vs Kepner-Tregoe | B_EDF03@sonnet-medium − C_KT@sonnet-medium | 24 | +1.28 [-0.08, 2.95] | **practically equivalent** | +0.95 [-0.42, 2.54] | +2.85 [0.35, 6.60] |
| H3 | EDF vs KT+ACH | B_EDF03@sonnet-medium − D_KTACH@sonnet-medium | 24 | +0.12 [-0.80, 1.09] | **practically equivalent** | +0.57 [-0.72, 1.92] | -0.90 [-3.40, 1.04] |
| H4 | gates+VOI vs EDF | F_EDFG@sonnet-medium − B_EDF03@sonnet-medium | 24 | -0.17 [-2.19, 1.81] | **practically equivalent** | +0.66 [-1.06, 2.60] | -3.19 [-9.17, 1.94] |
| H5a | System Context | B_EDF03@sonnet-medium − E_NOCTX@sonnet-medium | 24 | +0.19 [-1.08, 1.41] | **practically equivalent** | +0.13 [-0.70, 1.02] | +0.35 [-4.03, 4.79] |
| H5b | Origin Network + Propagation | B_EDF03@sonnet-medium − E_NOCAUSAL@sonnet-medium | 24 | +0.17 [-1.20, 1.61] | **practically equivalent** | +0.27 [-0.94, 1.60] | +0.14 [-3.33, 4.24] |
| H5c | Evidence evaluation | B_EDF03@sonnet-medium − E_NOEVID@sonnet-medium | 24 | +0.82 [-1.32, 3.35] | **inconclusive** | +0.26 [-1.60, 2.40] | +3.06 [-2.50, 9.17] |
| H5d | Control Points + Ranking | B_EDF03@sonnet-medium − E_NOCTRL@sonnet-medium | 24 | +0.54 [-0.82, 1.97] | **practically equivalent** | +0.93 [-0.65, 2.78] | -0.90 [-4.44, 2.71] |
| H5e | Diagnostic Sufficiency | B_EDF03@sonnet-medium − E_NOSUFF@sonnet-medium | 24 | -0.13 [-1.26, 0.89] | **practically equivalent** | -0.30 [-1.61, 0.90] | +0.56 [-1.67, 2.64] |
| H6 | Sequence | B_EDF03@sonnet-medium − E_NOSEQ@sonnet-medium | 24 | +0.82 [-0.64, 2.37] | **practically equivalent** | +0.78 [-0.61, 2.19] | +1.60 [-1.67, 6.04] |
| H7a | vote-of-5 (no framework) vs EDF | A_VOTE5@sonnet-medium − B_EDF03@sonnet-medium | 24 | +0.73 [-0.62, 2.20] | **practically equivalent** | +1.09 [-0.33, 2.62] | -2.39 [-6.11, 1.00] |
| H9a | Claude-selected method vs KT | SEL_CLAUDE@sonnet-medium − C_KT@sonnet-medium | 24 | +0.52 [-1.11, 2.43] | **practically equivalent** | +0.69 [-1.15, 2.85] | +0.42 [-2.92, 3.96] |
| H9b | EDF orchestrator vs EDF | SEL_EDFO@sonnet-medium − B_EDF03@sonnet-medium | 24 | -0.86 [-3.88, 1.67] | **inconclusive** | +0.04 [-1.97, 2.10] | -4.10 [-11.53, 1.81] |
| H11-H1 | Haiku: EDF vs no framework | B_EDF03@haiku-low − A_GENERAL@haiku-low | 24 | -2.08 [-4.72, 0.24] | **inconclusive** | -1.09 [-3.09, 0.78] | -4.79 [-13.12, 2.71] |
| H11-H2 | Haiku: EDF vs KT | B_EDF03@haiku-low − C_KT@haiku-low | 24 | -3.31 [-5.57, -1.42] | **inconclusive** | -2.13 [-3.82, -0.52] | -7.08 [-14.58, -1.04] |
| H11-H4 | Haiku: gates+VOI vs EDF | F_EDFG@haiku-low − B_EDF03@haiku-low | 24 | +0.30 [-2.44, 3.04] | **inconclusive** | -0.75 [-3.03, 1.51] | +2.29 [-4.58, 9.58] |

## H10 — item test–retest agreement (trials 1 vs 2, Sonnet-medium)

| condition | item pairs | agreement [95% Wilson] |
|---|---|---|
| A_GENERAL | 349 | 0.963 [0.937, 0.978] |
| B_EDF03 | 349 | 0.963 [0.937, 0.978] |
| C_KT | 349 | 0.966 [0.941, 0.980] |
| D_KTACH | 336 | 0.946 [0.917, 0.966] |
| F_EDFG | 333 | 0.958 [0.931, 0.975] |
| SEL_CLAUDE | 349 | 0.948 [0.920, 0.967] |
| SEL_EDFO | 349 | 0.960 [0.934, 0.976] |

## Class and domain breakdown (Sonnet-medium; case means, trials averaged)

| class | A_GENERAL score / acc | B_EDF03 score / acc | C_KT score / acc | D_KTACH score / acc | F_EDFG score / acc |
|---|---|---|---|---|---|
| ADVERS | 92.1 / 0.95 | 93.0 / 0.96 | 91.5 / 0.96 | 91.9 / 0.96 | 97.1 / 0.98 |
| CHANGE | 93.5 / 0.93 | 93.4 / 0.93 | 92.5 / 0.92 | 94.8 / 0.95 | 93.0 / 0.92 |
| CONFOUND | 94.9 / 0.95 | 86.6 / 0.87 | 78.4 / 0.80 | 86.1 / 0.82 | 86.7 / 0.88 |
| CONTRA | 76.4 / 0.79 | 78.0 / 0.81 | 73.9 / 0.75 | 75.2 / 0.77 | 82.0 / 0.83 |
| CROSS | 96.1 / 0.97 | 94.8 / 0.95 | 96.0 / 0.97 | 96.2 / 0.97 | 96.1 / 0.97 |
| INCOMPLETE | 84.0 / 0.99 | 91.1 / 0.98 | 86.7 / 0.95 | 91.7 / 1.00 | 81.4 / 1.00 |
| INTERACT | 95.8 / 0.95 | 96.1 / 0.96 | 95.8 / 0.95 | 96.9 / 0.97 | 96.9 / 0.97 |
| KNOWN | 95.6 / 0.96 | 95.5 / 0.96 | 97.6 / 0.98 | 95.8 / 0.96 | 95.3 / 0.95 |
| MISLEAD | 98.2 / 1.00 | 98.2 / 0.99 | 98.5 / 1.00 | 98.6 / 1.00 | 98.3 / 1.00 |
| NOCONC | 76.2 / 0.78 | 76.7 / 0.78 | 75.3 / 0.76 | 77.7 / 0.80 | 74.5 / 0.77 |
| OBVWRONG | 92.1 / 0.98 | 96.5 / 0.99 | 96.1 / 1.00 | 98.0 / 0.99 | 92.2 / 0.99 |
| SEQ | 96.0 / 0.98 | 97.1 / 0.97 | 97.9 / 0.98 | 95.8 / 0.95 | 94.8 / 0.97 |
| STOP | 98.5 / 1.00 | 98.6 / 1.00 | 97.2 / 0.98 | 95.2 / 0.95 | 97.8 / 0.99 |
| organizational | 87.2 / 0.90 | 87.0 / 0.89 | 83.9 / 0.87 | 86.1 / 0.88 | 85.6 / 0.89 |
| technical | 92.1 / 0.94 | 93.1 / 0.94 | 92.3 / 0.93 | 93.0 / 0.93 | 93.9 / 0.96 |
| cross-domain | 97.2 / 0.98 | 97.3 / 0.98 | 97.4 / 0.98 | 97.4 / 0.98 | 97.6 / 0.98 |

## M5 — evidence collected by class (mean items read, Sonnet-medium)

| class | A_GENERAL | B_EDF03 | E_NOSUFF | F_EDFG | C_KT |
|---|---|---|---|---|---|
| ADVERS | 2.00 | 2.00 | 2.00 | 2.00 | 2.00 |
| CHANGE | 2.00 | 2.00 | 2.00 | 2.00 | 2.00 |
| CONFOUND | 2.00 | 2.00 | 2.00 | 1.67 | 2.00 |
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
