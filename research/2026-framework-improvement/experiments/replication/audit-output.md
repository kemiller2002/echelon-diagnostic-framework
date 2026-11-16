## Audit

**Truth-text quotation.** 8-word shingles present in an analyzer output and in the truth narrative/rationales but absent from the analyzer-visible case file: 24 runs affected out of 332 checked (dev, held-out, haiku, OG).
- dev A_GENERAL OD-DEV-C: 1 shingles, e.g. "sustainer profiles were imported without payment methods and"
- dev BI_CLARITY OD-DEV-A: 1 shingles, e.g. "time sits entirely between qa pass and sign-off"
- dev BI_CLARITY OD-DEV-C: 1 shingles, e.g. "sustainer profiles were imported without payment methods and"
- heldout A_GENERAL OD-HO-J2: 1 shingles, e.g. "and peers with no new program had similar"
- heldout B0_EDF0 OD-HO-E: 3 shingles, e.g. "complaints rose while damage and billing complaints stayed"
- heldout B0_EDF0 OD-HO-K: 1 shingles, e.g. "group operations deferred the gm transfer requests and"
- heldout BI_CLARITY OD-HO-F: 5 shingles, e.g. "bonus funded by cutting the charge and preceptor"
- heldout B_CLARITY OD-HO-E: 1 shingles, e.g. "show on time against original dates year earlier"
- heldout D_NOGATE OD-HO-J2: 1 shingles, e.g. "were flat handling revenue was flat and the"
- heldout D_NOORG OD-HO-C: 6 shingles, e.g. "field is stale for the seven branches that"

**Identical outputs.** Cross-condition run pairs on the same case: 1120; byte-identical outputs: 0 []; identical answer vectors (all items same label): 602 (54%). Byte-identical trial pairs within a condition: 0.

**Evidence-request position leak.** Discriminating requests by menu position across all 22 cases: {'Q1': 20, 'Q2': 21, 'Q3': 12, 'Q5': 1, 'Q6': 3}; low-value: {'Q3': 6, 'Q4': 21, 'Q5': 14, 'Q6': 5}. In 18/22 cases the discriminating set is exactly the first k menu items. A content-blind strategy 'always request Q1,Q2,Q3' earns mean request value 0.795 (max 1). Share of runs requesting exactly {Q1,Q2,Q3}: {'dev': '32/80', 'heldout': '119/144', 'haiku': '32/48'}. The 10-point request component therefore measures little beyond menu order.

**Expected-label base rates (weight-weighted).**
- dev action: {'harmful': 37, 'premature': 25, 'preferred': 24, 'acceptable': 4}
- dev edge: {'causal': 34, 'association-only': 21, 'contradicted': 20, 'unknown': 1}
- dev hypothesis: {'contradicted': 50, 'supported': 29, 'unknown': 23}
- dev sufficiency: {'sufficient': 16, 'insufficient': 5}
- heldout action: {'harmful': 49, 'preferred': 43, 'premature': 21, 'acceptable': 5}
- heldout edge: {'causal': 38, 'association-only': 30, 'contradicted': 18, 'unknown': 6}
- heldout hypothesis: {'contradicted': 55, 'supported': 37, 'unknown': 31}
- heldout sufficiency: {'sufficient': 19, 'insufficient': 4}
- Content-blind baselines (learned on dev keys, applied to held-out): weighted accuracy {'type-majority': 0.454, 'type+position-majority': 0.535}. Analyzer accuracy is ~0.93–0.98, so the format is not trivially guessable, but the room between a careful reader and the ceiling is small.

**Prompt-vocabulary affinity with truth rationales.** For each condition module: distinct content words (≥4 letters, not in SHARED.txt); `rate` = share of truth-rationale tokens (held-out keys) that are module words *and not present anywhere in held-out case text* (i.e. reasoning vocabulary the key uses that the case does not), per 1,000 tokens; `cover` = share of module words found in held-out truth rationales; same for dev keys.

| module | module words | held-out rate /1k | held-out cover | dev rate /1k | dev cover | top shared rationale-only words (held-out) |
|---|---|---|---|---|---|---|
| A_GENERAL | 5 | 0.0 | 0.00 | 0.2 | 0.40 |  |
| B0_EDF0 | 104 | 2.9 | 0.46 | 6.8 | 0.49 | questions, points, outcome, applied, responsible, certain, contributing, signals |
| BI_CLARITY | 301 | 8.6 | 0.41 | 18.5 | 0.46 | explanation, points, harm, direct, establishes, describe, test, irreversible |
| B_CLARITY | 298 | 8.6 | 0.41 | 18.0 | 0.46 | explanation, points, harm, direct, establishes, describe, test, irreversible |
| B_EDF03 | 227 | 5.1 | 0.41 | 10.4 | 0.42 | points, direct, establishes, describe, outcome, disputed, begins, defensible |
| CL_ALONE | 85 | 3.5 | 0.45 | 8.2 | 0.56 | explanation, harm, test, irreversible, depends, observed, relies, path |
| C_KTNT | 159 | 5.1 | 0.53 | 6.8 | 0.48 | explains, test, fits, strategy, observed, communication, true, verified |
| D_EDFX | 147 | 9.1 | 0.60 | 7.5 | 0.62 | explains, makes, direct, controls, strategy, describe, missing, silence |
| D_NOGATE | 130 | 8.1 | 0.60 | 6.7 | 0.62 | explains, points, makes, direct, controls, describe, silence, missing |
| D_NOORG | 81 | 4.1 | 0.58 | 5.9 | 0.62 | controls, co-occurrence, outcome, strategy, punishing, depends, gamed, true |
| N_NEUTRAL | 142 | 5.3 | 0.58 | 13.1 | 0.59 | explanation, plausible, unconfirmed, fits, absent, depends, observed, address |

D_EDFX words that occur in held-out case text but in no dev case and no non-D module (possible held-out tuning signal): ['allow', 'damage', 'deterioration', 'easy', 'grade', 'intentional', 'miss', 'supposed']

Mean prompt chars (held-out): A_GENERAL 20830, B0_EDF0 23471, BI_CLARITY 27795, B_CLARITY 27492, B_EDF03 26061, C_KTNT 23946, D_EDFX 23779, D_NOGATE 23094, D_NOORG 22645, N_NEUTRAL 23775
