## Open-generation arm (OD1-G) re-scored from grader journals

Grades: 80 (g1=60, g2=20); orchestrator rows: 80. Max |my score − orchestrator| = 0.0000 (og|B_EDF03|OD-HO-C|t1/g1). Early-stage items present in grader mappings: 0. Scorable items missing from mappings: 0 in 0 gradings. Mapping ids not in case: 0.

| condition | n | mean OG score (g1) | mean acc | mean req | harmful recommended |
|---|---|---|---|---|---|
| A_GENERAL | 12 | 85.75 | 0.869 | 0.750 | 0 |
| B_EDF03 | 12 | 87.44 | 0.891 | 0.722 | 0 |
| C_KTNT | 12 | 90.05 | 0.911 | 0.806 | 0 |
| D_EDFX | 12 | 88.33 | 0.900 | 0.736 | 0 |
| N_NEUTRAL | 12 | 87.60 | 0.885 | 0.792 | 0 |

### OG paired contrasts (grader g1; Holm within these)

| phase | contrast | n | mean A | mean B | diff | 95% boot CI | SD diff | dz | sign-flip p | Holm p | W/L/T |
|---|---|---|---|---|---|---|---|---|---|---|---|
| og | D_EDFX − B_EDF03 | 12 | 88.33 | 87.44 | **+0.89** | [-4.38, +6.51] | 9.94 | +0.09 | 0.7266 | 1.0000 | 5/4/3 |
| og | B_EDF03 − A_GENERAL | 12 | 87.44 | 85.75 | **+1.69** | [-1.47, +5.85] | 6.74 | +0.25 | 0.5000 | 1.0000 | 5/3/4 |
| og | B_EDF03 − N_NEUTRAL | 12 | 87.44 | 87.60 | **-0.16** | [-3.70, +3.65] | 6.79 | -0.02 | 0.9409 | 1.0000 | 6/6/0 |
| og | D_EDFX − N_NEUTRAL | 12 | 88.33 | 87.60 | **+0.73** | [-3.90, +5.25] | 8.40 | +0.09 | 0.7812 | 1.0000 | 5/3/4 |
| og | D_EDFX − A_GENERAL | 12 | 88.33 | 85.75 | **+2.58** | [-3.22, +9.28] | 11.38 | +0.23 | 0.4922 | 1.0000 | 6/3/3 |
| og | C_KTNT − B_EDF03 | 12 | 90.05 | 87.44 | **+2.60** | [-3.46, +7.55] | 10.12 | +0.26 | 0.3887 | 1.0000 | 8/3/1 |

### Inter-grader agreement (g1 vs g2, 20 double-graded runs, 316 item readings)

Overall raw agreement 0.956, Cohen's kappa 0.949.

| item type | n | agreement | kappa |
|---|---|---|---|
| action | 100 | 0.930 | 0.901 |
| edge | 86 | 0.953 | 0.935 |
| hypothesis | 110 | 0.973 | 0.955 |
| sufficiency | 20 | 1.000 | 1.000 |

Run-score agreement: mean g1 88.01 vs g2 87.70; mean |g1−g2| 1.92; max |g1−g2| 12.00; Pearson r 0.904.
Conditions in double-graded subset: {'A_GENERAL': 1, 'B_EDF03': 5, 'C_KTNT': 3, 'D_EDFX': 4, 'N_NEUTRAL': 7}
Most common disagreements (g1→g2): association-only→unknown ×2; acceptable→harmful ×2; unknown→not-addressed ×2; acceptable→preferred ×2; premature→not-addressed ×1; contradicted→unknown ×1; premature→harmful ×1; harmful→premature ×1

Method-name vocabulary visible to the 'blind' grader in OG analyst outputs (runs containing term): C_KTNT:congruence×1
