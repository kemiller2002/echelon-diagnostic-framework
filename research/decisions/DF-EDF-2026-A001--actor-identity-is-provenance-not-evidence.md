---
id: DF-EDF-2026-A001
title: Actor identity is provenance, not evidence quality
status: accepted
version: 1.0.0
owners:
  - repository-governance
created: 2026-09-26
updated: 2026-09-26
research_area: framework-engineering
decision_type: governance
confidence: high
supports: []
supersedes: []
superseded_by: []
related_documents:
  - docs/framework-engineering/provenance-requirements.md
  - docs/framework-engineering/claim-and-confidence-policy.md
  - scripts/provenance-integrity.mjs
tags: [provenance, claims, blinding, governance]
---

# Decision

EDF adopts the Praxis agent provenance contract (Praxis DF-ROS-2026-A036,
DF-ROS-2026-A037) for its research records without redefining it, and fixes
how that provenance relates to claims:

1. **New records retain provenance.** New research artifacts, diagnostic
   findings, evidence, hypotheses, and conclusions record who created or
   changed them as Praxis provenance (`ros provenance record` for Markdown;
   a top-level `praxis.provenance/1` block for registry entries) where the
   repository's ROS release supports it (RQ-EDF-2026-A001).
2. **Identity is not evidence.** Whether a record came from an OpenAI,
   Anthropic, Google, or other model, a human, or automation never increases
   or decreases evidentiary weight, confidence, claim state, or public claim
   (Praxis RQ-ROS-2026-A019). Recorded identity is self-reported (Praxis
   RQ-ROS-2026-A010) (RQ-EDF-2026-A002).
3. **Executor identity only as a declared variable.** Executor identity
   matters only when a preregistration declares it as the experimental
   variable, as in cross-executor reproducibility, and it is then recorded in
   sealed or evaluator material (RQ-EDF-2026-A003).
4. **Blinded material carries no provenance** (RQ-EDF-2026-A004).
5. **No retroactive change** to frozen, preregistered, released, or
   hash-pinned material (Praxis RQ-ROS-2026-A015 legacy rules;
   RQ-EDF-2026-A005).
6. **Mechanical enforcement** in the repository validator (RQ-EDF-2026-A006).

# Why

- EDF's claim policy already separates structural conformity, agreement,
  correctness, reproducibility, value, and utility. A model family label is
  none of these; letting it act as a credibility signal would let one
  mislabelled or forged actor change a claim.
- EDF experiments already treat executor family as a variable
  (EX-EDF-001, EX-EDF-2026-A003). Keeping identity in sealed or evaluator
  material is what makes those comparisons blind.
- A structural check (identity fields only inside a top-level `provenance`
  block; labels stay labels) is concrete and does not depend on free text.

# Alternatives rejected

- **Record identity as registry fields such as `authorFamily`.** Rejected: it
  places identity next to confidence and invites weighting.
- **Weight evidence by executor diversity automatically.** Rejected:
  independence is a property of the execution design under preregistration,
  judged by the claim policy, not a count of provider labels.
- **Backfill provenance into existing records.** Rejected: fabricates
  history and would edit frozen material.

# Consequences

- `scripts/provenance-integrity.mjs` runs inside
  `scripts/repository-integrity.mjs`, so `node --test` and
  `node scripts/validate-repository.mjs` enforce the rules. It vendors the
  Praxis reference library unchanged (hash recorded in
  `tests/fixtures/praxis-provenance/SOURCE.json`).
- Existing registry entries, cases, and experiments are unchanged and valid.
- **Limitation:** the repository runs ROS 3.1.x, which predates
  `ros provenance` and the `ros.json` provenance policy; Markdown provenance
  recording and policy enforcement wait for the ROS upgrade.
- **Limitation:** the ROS-managed `docs/00-governance/Governance-Decision-Log.md`
  is pinned by `.ros/installation.json`, so this decision is recorded here, in
  the canonical decision root (`docs/decisions/README.md`), rather than as a
  `DF-GOV-` entry in that log.

# Revisit when

- The repository upgrades to a ROS release with provenance support.
- A preregistered design needs identity-aware analysis beyond grouping by the
  declared condition.
