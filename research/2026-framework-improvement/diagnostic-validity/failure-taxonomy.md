# Diagnostic failure taxonomy

Frequencies are from OD1 held-out recognition runs (Sonnet, 144 runs × 15–23 items) unless stated. Severity scale: S1 cosmetic, S2 degrades quality, S3 could mislead a decision, S4 could cause harm.

| ID | Failure | Conditions under which it occurs | Severity | Detection method | Frequency observed | Consequence | Proposed mitigation | Experimental evidence | Remaining uncertainty |
|---|---|---|---|---|---|---|---|---|---|
| FT-01 | Incorrect problem identification (false diagnosis) | Presenting narrative plausible; countering evidence indirect | S3 | Item `falseDiagnosis` | 0–4% of non-supported hypotheses | Wrong problem pursued | Treat presenting problem as hypothesis; source ledger | D 0/102 vs A 2/51 (n.s.) | Rare at this difficulty |
| FT-02 | Missed real problem | Problem not named by client (hidden, silenced) | S3 | `missedProblem`; OG missed-problem rate | 0–5% recognition; 0–1/18 OG | Root problem persists | Open-generation diagnosis plus discriminating evidence requests | No method difference detected | Real-world rate unknown; evidence curated here |
| FT-03 | False root cause | Plausible symptom-level cause | S3 | Edge and hypothesis items tagged symptom-not-cause | Rare | Intervention on symptom | Causal grading rule | No method difference | — |
| FT-04 | Misleading confidence (high-confidence error) | Ambiguous evidence | S3 | Confidence ≥ 80 on wrong item | 0–2.3% of high-confidence answers | Overcommitment | Confidence tied to evidence independence | D 1/259, B 2/262 | Self-reported confidence only |
| FT-05 | Unsupported causal claim | Co-occurrence, confounded timing | S3 | `causalOverclaim` | 0–3% | Wrong lever | Causal grading (temporal order, mechanism, rival excluded) | D 0/64, C 0/32, others 1/32–1/64 | Small counts |
| FT-06 | Under-rejection of refuted link | Link refuted by contrast evidence, labeled association-only | S2 | edge contradicted → association-only | 15 runs (most common edge error) | Dead hypotheses kept alive | KT-style IS/IS-NOT contrast test | C (KT) best causal accuracy 0.96 | Truth-key granularity |
| FT-07 | Stakeholder bias / leadership-narrative adoption | Executive account without corroboration | S3 | `leadershipNarrativeAdoption` | 0–2% | Blames wrong party | Source-independence ledger | ≤ 1/50 every condition | Benchmark too easy on this axis |
| FT-08 | Metric misinterpretation (Goodhart / Campbell) | Rewarded metric, definition change | S3 | tag metric-gaming / measurement-artifact | 4–10% of tagged items | Celebrate deterioration | Metric-provenance check | D 0.96, B 0.92, A 0.95 | — |
| FT-09 | Organizational silence missed | Low reporting read as health | S3 | tag organizational-silence | 13% of tagged items (highest tag error rate) | Hidden problem persists | "Silence is not health" rule | No consistent method difference | Few items |
| FT-10 | Over-abstention (false insufficiency) | Problem does not exist; tradeoff case; evidence adequate | S2 | `falseAbstention` | 14 runs; N 4/10 | Delay; needless data collection | Sufficiency includes "no intervention warranted" | D 0/20 vs B 2/20 vs N 4/10 | Small n |
| FT-11 | Action-severity miscalibration (premature labeled harmful) | Plausible but unconfirmed intervention | S2 | action premature → harmful | 19 runs (most common error) | Over-rejects reasonable options | Explicit definitions of harmful vs premature | No method fully fixes it | Partly truth-key granularity |
| FT-12 | Unsupported certainty on genuinely open questions | Measurement-artifact or hidden-problem items with missing data | S3 | `unsupportedCertainty` | 5–18% Sonnet; 18–23% Haiku | Premature closure | Missing evidence → unknown rule; name live alternatives | B and D 2/44 vs A 4/22 | — |
| FT-13 | Failure to update when evidence changes | Staged evidence (class H) | S3 | `revisionAccuracy` | 79–100%; N worst (0.79) | Stale diagnosis | Stage-bounded reasoning | B 0.96, D 0.93 | 2 dynamic cases |
| FT-14 | Inability to generalize | OOD organizations | S2 | Class J scores | 82–97 (J2 hardest) | — | — | No collapse | 2 cases |
| FT-15 | Excessive procedural complexity | Long generic protocols | S2 | N − A | Haiku −7.0 [−11.2, −3.2]; Sonnet −1.4 | Displaces attention from evidence | Short, decision-relevant rules only | N worst held-out on both tiers | Cause unconfirmed |
| FT-16 | Evidence acquisition failure | Budgeted evidence; decisive items not collected | S4 | Adaptive experiment | −12 (model-selected) to −26 (fixed order) points | Diagnosis on wrong evidence | Discriminating-evidence planning (VOI) | Selected vs random +11.6 [5.9, 17.9] | Catalogue-only selection |
| FT-17 | Inconsistent results across runs | — | S2 | Test–retest | 2.5% item disagreement | Low | — | B = D 97.5% | Human analysts untested |
| FT-18 | Poor user comprehension | Human users | S2 | Not measurable here | — | — | — | None | **Untested** |
| FT-19 | Incorrect intervention selection | — | S3 | Action accuracy | 92–97% | — | Harm-under-every-hypothesis gate | D − D_NOGATE +2.8 [0.5, 5.7] | Real outcomes untested |

| FT-20 | Evidence-budget non-compliance | Instruction to seek discriminating evidence under an explicit collection limit | S3 (scope / consent breach in practice) | Transcript read count vs budget | D 10/12, general 4/12 runs exceeded k = 6 (OD1-S) | Exceeds agreed data-request scope; confounds efficiency | Make budget and consent limits hard constraints in any acquisition rule | OD1-S | Single tier |

## Failure modes found independently (not in the mission list)

- **FT-15, procedural displacement.** A long, generic procedure lowered accuracy, especially for the smaller model.
- **FT-16, evidence acquisition.** The largest measured effect in the programme came from which evidence was read, not from how it was analyzed.
- **FT-20, budget non-compliance.** Found in OD1-S: discriminating-evidence instructions overrode an explicit 6-item limit.
- **Truth-key granularity.** Errors shared by every condition cluster on adjacent categories. Benchmark answer categories need sharper operational definitions. This is a measurement failure, not a diagnostic failure.
