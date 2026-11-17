# OD2 track analyses

## Track A — structure vs compute

| cell | runs | score | acc | acq | thinking tok | cost USD | time s |
|---|---|---|---|---|---|---|---|
| A_GENERAL@sonnet-low | 2 | 94.21 | 1.000 | 0.783 | 179 | 0.0532 | 17 |
| A_GENERAL@sonnet-medium | 8 | 86.02 | 0.904 | 0.742 | 204 | 0.0529 | 15 |
| A_GENERAL@sonnet-high | 2 | 84.15 | 1.000 | 0.283 | 594 | 0.0630 | 20 |
| A_GENERAL@sonnet-max | 2 | 89.13 | 1.000 | 0.567 | 66484 | 0.8441 | 480 |
| B_EDF03@sonnet-low | 2 | 92.04 | 0.970 | 0.783 | 59 | 0.0609 | 15 |
| B_EDF03@sonnet-medium | 8 | 86.69 | 0.940 | 0.675 | 272 | 0.0631 | 16 |
| B_EDF03@sonnet-high | 2 | 98.94 | 1.000 | 1.000 | 488 | 0.0669 | 16 |
| B_EDF03@sonnet-max | 2 | 90.40 | 1.000 | 0.567 | 62845 | 0.8540 | 464 |

| contrast | measure | cases | mean diff | 95% CI |
|---|---|---|---|---|
| A_VOTE5@sonnet-medium − B_EDF03@sonnet-medium | score | - | - | - |
| A_VOTE5@sonnet-medium − B_EDF03@sonnet-medium | acc | - | - | - |
| A_VOTE5@sonnet-medium − B_EDF03@sonnet-medium | costUsd | - | - | - |
| A_VOTE5@sonnet-medium − A_GENERAL@sonnet-medium | score | - | - | - |
| A_VOTE5@sonnet-medium − A_GENERAL@sonnet-medium | acc | - | - | - |
| A_VOTE5@sonnet-medium − A_GENERAL@sonnet-medium | costUsd | - | - | - |
| B_EDF03@sonnet-low − A_GENERAL@sonnet-low | score | 2 | -2.170 | [-4.376, 0.037] |
| B_EDF03@sonnet-low − A_GENERAL@sonnet-low | acc | 2 | -0.030 | [-0.061, 0.000] |
| B_EDF03@sonnet-low − A_GENERAL@sonnet-low | costUsd | 2 | 0.008 | [0.007, 0.008] |
| B_EDF03@sonnet-high − A_GENERAL@sonnet-high | score | 2 | 14.793 | [9.099, 20.487] |
| B_EDF03@sonnet-high − A_GENERAL@sonnet-high | acc | 2 | 0.000 | [0.000, 0.000] |
| B_EDF03@sonnet-high − A_GENERAL@sonnet-high | costUsd | 2 | 0.004 | [0.001, 0.007] |
| B_EDF03@sonnet-max − A_GENERAL@sonnet-max | score | 2 | 1.264 | [-0.111, 2.640] |
| B_EDF03@sonnet-max − A_GENERAL@sonnet-max | acc | 2 | 0.000 | [0.000, 0.000] |
| B_EDF03@sonnet-max − A_GENERAL@sonnet-max | costUsd | 2 | 0.010 | [-0.037, 0.057] |
| A_GENERAL@sonnet-max − A_GENERAL@sonnet-medium | score | 2 | -4.568 | [-8.284, -0.852] |
| A_GENERAL@sonnet-max − A_GENERAL@sonnet-medium | acc | 2 | 0.015 | [0.000, 0.030] |
| A_GENERAL@sonnet-max − A_GENERAL@sonnet-medium | costUsd | 2 | 0.792 | [0.600, 0.985] |
| B_EDF03@sonnet-max − B_EDF03@sonnet-medium | score | 2 | -0.987 | [-1.966, -0.008] |
| B_EDF03@sonnet-max − B_EDF03@sonnet-medium | acc | 2 | 0.000 | [0.000, 0.000] |
| B_EDF03@sonnet-max − B_EDF03@sonnet-medium | costUsd | 2 | 0.791 | [0.646, 0.937] |

## Track B — accuracy vs persuasiveness (rater: Claude Opus 5.5, blind; not independent human evidence)

Rated briefs: 52.

| cell | briefs | persuasiveness | perceived correctness | actionability | true acc | persuasion gap | Spearman(perceived, acc) [95% CI] |
|---|---|---|---|---|---|---|---|
| A_GENERAL@haiku-low | 4 | 5.75 | 74.0 | 6.25 | 0.905 | -0.165 | NaN [NaN, NaN] |
| A_GENERAL@sonnet-high | 2 | 6.00 | 81.5 | 6.50 | 1.000 | -0.185 | NaN [NaN, NaN] |
| A_GENERAL@sonnet-low | 2 | 6.00 | 80.0 | 6.50 | 1.000 | -0.200 | NaN [NaN, NaN] |
| A_GENERAL@sonnet-max | 2 | 6.00 | 81.0 | 6.50 | 1.000 | -0.190 | NaN [NaN, NaN] |
| A_GENERAL@sonnet-medium | 8 | 5.75 | 76.5 | 6.12 | 0.904 | -0.139 | 0.819 [0.535, 0.979] |
| B_EDF03@haiku-low | 4 | 5.75 | 73.5 | 6.25 | 0.912 | -0.177 | NaN [NaN, NaN] |
| B_EDF03@sonnet-high | 2 | 6.00 | 82.5 | 6.50 | 1.000 | -0.175 | NaN [NaN, NaN] |
| B_EDF03@sonnet-low | 2 | 6.00 | 81.5 | 6.00 | 0.970 | -0.155 | NaN [NaN, NaN] |
| B_EDF03@sonnet-max | 2 | 6.00 | 81.5 | 6.50 | 1.000 | -0.185 | NaN [NaN, NaN] |
| B_EDF03@sonnet-medium | 8 | 6.00 | 77.9 | 6.25 | 0.940 | -0.161 | 0.768 [0.584, 0.974] |
| C_KT@sonnet-medium | 8 | 5.75 | 77.1 | 6.25 | 0.895 | -0.124 | 0.616 [-0.235, 0.981] |
| F_EDFG@sonnet-medium | 8 | 5.88 | 78.1 | 6.12 | 0.912 | -0.130 | 0.710 [0.000, 0.993] |

| contrast | measure | cases | mean diff | 95% CI |
|---|---|---|---|---|
| B_EDF03@sonnet-medium − A_GENERAL@sonnet-medium | persuasiveness | 4 | 0.250 | [0.000, 0.500] |
| B_EDF03@sonnet-medium − A_GENERAL@sonnet-medium | perceivedCorrectness | 4 | 1.375 | [-0.750, 3.500] |
| B_EDF03@sonnet-medium − A_GENERAL@sonnet-medium | actionability | 4 | 0.125 | [-0.375, 0.750] |
| B_EDF03@sonnet-medium − A_GENERAL@sonnet-medium | trueAcc | 4 | 0.036 | [0.000, 0.085] |
| B_EDF03@sonnet-medium − A_GENERAL@sonnet-medium | persuasionGap | 4 | -0.022 | [-0.058, 0.015] |
| B_EDF03@sonnet-medium − C_KT@sonnet-medium | persuasiveness | 4 | 0.250 | [0.000, 0.500] |
| B_EDF03@sonnet-medium − C_KT@sonnet-medium | perceivedCorrectness | 4 | 0.750 | [-1.250, 2.750] |
| B_EDF03@sonnet-medium − C_KT@sonnet-medium | actionability | 4 | 0.000 | [-0.375, 0.375] |
| B_EDF03@sonnet-medium − C_KT@sonnet-medium | trueAcc | 4 | 0.044 | [0.031, 0.068] |
| B_EDF03@sonnet-medium − C_KT@sonnet-medium | persuasionGap | 4 | -0.037 | [-0.047, -0.022] |
| F_EDFG@sonnet-medium − A_GENERAL@sonnet-medium | persuasiveness | 4 | 0.125 | [0.000, 0.375] |
| F_EDFG@sonnet-medium − A_GENERAL@sonnet-medium | perceivedCorrectness | 4 | 1.625 | [0.250, 3.625] |
| F_EDFG@sonnet-medium − A_GENERAL@sonnet-medium | actionability | 4 | 0.000 | [-0.625, 0.500] |
| F_EDFG@sonnet-medium − A_GENERAL@sonnet-medium | trueAcc | 4 | 0.008 | [-0.016, 0.031] |
| F_EDFG@sonnet-medium − A_GENERAL@sonnet-medium | persuasionGap | 4 | 0.008 | [-0.021, 0.052] |

## Track C — method selection

| arm | runs | coded choices | selection errors | execution errors | best-arm choices |
|---|---|---|---|---|---|
| SEL_CLAUDE | 2 | D:1, C:1 | 1 | 0 | 0 |
| SEL_EDFO | 2 | C:1, D:1 | 1 | 0 | 0 |

Selection error: chose a fixed method whose case mean is ≥ 5 below the best fixed arm. Execution error: chose within 5 of the best arm but the run itself scored ≥ 5 below it. Combinations and uncoded choices are counted in the distribution only.
