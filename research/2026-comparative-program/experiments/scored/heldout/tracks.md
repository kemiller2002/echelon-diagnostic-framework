# OD2 track analyses

## Track A — structure vs compute

| cell | runs | score | acc | acq | thinking tok | cost USD | time s |
|---|---|---|---|---|---|---|---|
| A_GENERAL@sonnet-low | 24 | 91.52 | 0.926 | 0.925 | 132 | 0.0481 | 12 |
| A_GENERAL@sonnet-medium | 120 | 91.73 | 0.938 | 0.894 | 247 | 0.0495 | 13 |
| A_GENERAL@sonnet-high | 24 | 95.17 | 0.972 | 0.929 | 676 | 0.0545 | 16 |
| A_GENERAL@sonnet-max | 24 | 96.59 | 0.975 | 0.979 | 65159 | 0.8484 | 470 |
| B_EDF03@sonnet-low | 24 | 91.00 | 0.925 | 0.900 | 139 | 0.0576 | 12 |
| B_EDF03@sonnet-medium | 72 | 90.75 | 0.921 | 0.904 | 234 | 0.0581 | 13 |
| B_EDF03@sonnet-high | 24 | 94.09 | 0.963 | 0.908 | 683 | 0.0644 | 16 |
| B_EDF03@sonnet-max | 24 | 95.13 | 0.969 | 0.933 | 60972 | 0.7805 | 437 |
| A_VOTE5@sonnet-medium | 24 | 92.65 | 0.943 | 0.894 | 1235 | 0.2473 | 64 |

| contrast | measure | cases | mean diff | 95% CI |
|---|---|---|---|---|
| A_VOTE5@sonnet-medium − B_EDF03@sonnet-medium | score | 24 | 1.905 | [-0.514, 5.473] |
| A_VOTE5@sonnet-medium − B_EDF03@sonnet-medium | acc | 24 | 0.022 | [-0.003, 0.058] |
| A_VOTE5@sonnet-medium − B_EDF03@sonnet-medium | costUsd | 24 | 0.189 | [0.184, 0.194] |
| A_VOTE5@sonnet-medium − A_GENERAL@sonnet-medium | score | 24 | 0.926 | [0.182, 1.818] |
| A_VOTE5@sonnet-medium − A_GENERAL@sonnet-medium | acc | 24 | 0.005 | [-0.003, 0.015] |
| A_VOTE5@sonnet-medium − A_GENERAL@sonnet-medium | costUsd | 24 | 0.198 | [0.193, 0.203] |
| B_EDF03@sonnet-low − A_GENERAL@sonnet-low | score | 24 | -0.520 | [-3.115, 1.781] |
| B_EDF03@sonnet-low − A_GENERAL@sonnet-low | acc | 24 | -0.001 | [-0.027, 0.027] |
| B_EDF03@sonnet-low − A_GENERAL@sonnet-low | costUsd | 24 | 0.010 | [0.009, 0.010] |
| B_EDF03@sonnet-high − A_GENERAL@sonnet-high | score | 24 | -1.082 | [-2.365, -0.049] |
| B_EDF03@sonnet-high − A_GENERAL@sonnet-high | acc | 24 | -0.008 | [-0.020, 0.002] |
| B_EDF03@sonnet-high − A_GENERAL@sonnet-high | costUsd | 24 | 0.010 | [0.009, 0.011] |
| B_EDF03@sonnet-max − A_GENERAL@sonnet-max | score | 24 | -1.462 | [-4.500, 0.482] |
| B_EDF03@sonnet-max − A_GENERAL@sonnet-max | acc | 24 | -0.005 | [-0.027, 0.009] |
| B_EDF03@sonnet-max − A_GENERAL@sonnet-max | costUsd | 24 | -0.068 | [-0.170, 0.038] |
| A_GENERAL@sonnet-max − A_GENERAL@sonnet-medium | score | 24 | 4.864 | [1.868, 9.062] |
| A_GENERAL@sonnet-max − A_GENERAL@sonnet-medium | acc | 24 | 0.037 | [0.008, 0.077] |
| A_GENERAL@sonnet-max − A_GENERAL@sonnet-medium | costUsd | 24 | 0.799 | [0.665, 0.945] |
| B_EDF03@sonnet-max − B_EDF03@sonnet-medium | score | 24 | 4.381 | [1.619, 8.347] |
| B_EDF03@sonnet-max − B_EDF03@sonnet-medium | acc | 24 | 0.048 | [0.018, 0.093] |
| B_EDF03@sonnet-max − B_EDF03@sonnet-medium | costUsd | 24 | 0.722 | [0.608, 0.852] |

## Track B — accuracy vs persuasiveness (rater: Claude Opus 5.5, blind; not independent human evidence)

Rated briefs: 336.

| cell | briefs | persuasiveness | perceived correctness | actionability | true acc | persuasion gap | Spearman(perceived, acc) [95% CI] |
|---|---|---|---|---|---|---|---|
| A_GENERAL@sonnet-medium | 48 | 5.98 | 79.0 | 6.35 | 0.938 | -0.148 | 0.392 [0.102, 0.632] |
| B_EDF03@sonnet-medium | 48 | 5.90 | 78.3 | 6.38 | 0.939 | -0.156 | 0.274 [-0.041, 0.555] |
| C_KT@sonnet-medium | 48 | 6.00 | 79.6 | 6.52 | 0.923 | -0.127 | 0.353 [0.071, 0.600] |
| D_KTACH@sonnet-medium | 48 | 5.90 | 78.4 | 6.35 | 0.908 | -0.124 | 0.308 [-0.007, 0.581] |
| F_EDFG@sonnet-medium | 48 | 5.94 | 78.7 | 6.46 | 0.934 | -0.147 | 0.288 [-0.010, 0.572] |
| SEL_CLAUDE@sonnet-medium | 48 | 5.94 | 78.9 | 6.35 | 0.930 | -0.140 | 0.285 [-0.017, 0.567] |
| SEL_EDFO@sonnet-medium | 48 | 5.92 | 78.2 | 6.42 | 0.933 | -0.151 | 0.540 [0.295, 0.723] |

| contrast | measure | cases | mean diff | 95% CI |
|---|---|---|---|---|
| B_EDF03@sonnet-medium − A_GENERAL@sonnet-medium | persuasiveness | 24 | -0.083 | [-0.167, -0.021] |
| B_EDF03@sonnet-medium − A_GENERAL@sonnet-medium | perceivedCorrectness | 24 | -0.729 | [-1.354, -0.167] |
| B_EDF03@sonnet-medium − A_GENERAL@sonnet-medium | actionability | 24 | 0.021 | [-0.104, 0.146] |
| B_EDF03@sonnet-medium − A_GENERAL@sonnet-medium | trueAcc | 24 | 0.000 | [-0.016, 0.015] |
| B_EDF03@sonnet-medium − A_GENERAL@sonnet-medium | persuasionGap | 24 | -0.008 | [-0.025, 0.011] |
| B_EDF03@sonnet-medium − C_KT@sonnet-medium | persuasiveness | 24 | -0.104 | [-0.250, 0.000] |
| B_EDF03@sonnet-medium − C_KT@sonnet-medium | perceivedCorrectness | 24 | -1.312 | [-3.000, -0.083] |
| B_EDF03@sonnet-medium − C_KT@sonnet-medium | actionability | 24 | -0.146 | [-0.271, 0.000] |
| B_EDF03@sonnet-medium − C_KT@sonnet-medium | trueAcc | 24 | 0.016 | [0.001, 0.033] |
| B_EDF03@sonnet-medium − C_KT@sonnet-medium | persuasionGap | 24 | -0.029 | [-0.054, -0.007] |
| F_EDFG@sonnet-medium − A_GENERAL@sonnet-medium | persuasiveness | 24 | -0.042 | [-0.125, 0.042] |
| F_EDFG@sonnet-medium − A_GENERAL@sonnet-medium | perceivedCorrectness | 24 | -0.354 | [-1.229, 0.354] |
| F_EDFG@sonnet-medium − A_GENERAL@sonnet-medium | actionability | 24 | 0.104 | [0.000, 0.208] |
| F_EDFG@sonnet-medium − A_GENERAL@sonnet-medium | trueAcc | 24 | -0.004 | [-0.027, 0.019] |
| F_EDFG@sonnet-medium − A_GENERAL@sonnet-medium | persuasionGap | 24 | 0.001 | [-0.024, 0.026] |

## Track C — method selection

| arm | runs | coded choices | selection errors | execution errors | best-arm choices |
|---|---|---|---|---|---|
| SEL_CLAUDE | 48 | D:14, D+A:13, C+D:7, C:6, uncoded:5, A:3 | 4 | 3 | 7 |
| SEL_EDFO | 48 | C+D:19, C:16, uncoded:7, B+C+D:3, B+D:1, B:1, D:1 | 1 | 3 | 6 |

Selection error: chose a fixed method whose case mean is ≥ 5 below the best fixed arm. Execution error: chose within 5 of the best arm but the run itself scored ≥ 5 below it. Combinations and uncoded choices are counted in the distribution only.
