# Unresolved questions

| ID | Question | Why unresolved | What would resolve it | Priority |
|---|---|---|---|---|
| UQ-01 | Do results hold with non-Claude analyzers, case authors and graders? | No cross-provider credentials in this session | Re-run OD1/OD2 with OpenAI and Google analyzers (harness supports any executor that returns the output contract) | High |
| UQ-02 | Does EDF help *human* analysts (comprehension, communication, reviewability, time to actionable finding)? | No human participants | Human-analyst study (`strategy/framework-roadmap.md` item 7) | High |
| UQ-03 | Do framework differences appear when tasks are harder: decisive evidence absent, noisy files, evidence acquisition required? | OD1 saturated (condition means 87–96) | OD2 benchmark | High |
| UQ-04 | Can analyzers stop collecting evidence adaptively (in line with difficulty) and within budget? | OD1-S: sequential acquisition nearly closes the accuracy gap but over-collects (60–73%); stopping does not track difficulty; budget violated (FT-20) | Explicit value-of-information stopping rule with an enforced budget; test on OD2 | High |
| UQ-15 | Is Kepner-Tregoe's contrast specification the mechanism behind its evidence efficiency? | Observed in OD1-S only | Ablate the IS/IS-NOT step | Medium |
| UQ-05 | Do decision-gate semantics (CI-01) replicate across tiers and trials? | Single-trial, single-tier ablation | Confirmatory ablation on OD2 | Medium |
| UQ-06 | Do organizational guards help specifically on contradictory-stakeholder and masked-risk cases? | One case per class; not seen on Haiku | Class-focused benchmark (≥ 10 cases per class) | Medium |
| UQ-07 | Does the four-way causal edge scale itself reduce overclaiming? | Held constant across conditions | With-vs-without-scale experiment | Medium |
| UQ-08 | Do EDF-selected interventions produce better real outcomes? | No real organizations | `intervention-effectiveness/evaluation-design.md` | High (long-term) |
| UQ-09 | Is continuous assessment beneficial net of surveillance costs? | Not testable here | Safeguarded pilot with a defined purpose | Low |
| UQ-10 | Truth-key granularity: are premature-vs-harmful and contradicted-vs-association-only reliably distinguishable by experts? | Most shared errors fall on these boundaries | Human expert panel agreement on truth keys | Medium |
| UQ-11 | Duplicate or conflicting source metadata (Jun et al. 2018 vs 2019; Hulme 2023 vs 2024; unconfirmed titles) | Found after the literature merge | Primary-source verification; deduplicate `evidence/source-register.csv` | Low |
| UQ-12 | Why does a generic long protocol hurt the smaller model so much (−7.0)? | Mechanism untested | Prompt-length and step-count dose-response experiment | Low |
| UQ-13 | Clarity's value at the decision-record stage (ownership, reassessment, learning) | Not captured by item scoring | Longitudinal decision-quality study | Medium |
| UQ-14 | A003 benchmark: Anthropic-authored cases missing, though the journal claims completion | Pre-existing repository inconsistency | Investigate and correct the A003 records | Low |
