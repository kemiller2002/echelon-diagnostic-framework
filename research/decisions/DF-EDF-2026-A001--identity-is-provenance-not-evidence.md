---
id: DF-EDF-2026-A001
title: "Actor identity is provenance for independence and stratification, never evidence weight"
status: accepted
created: 2026-09-26
research_area: diagnostic-frameworks
decision_type: research-governance
related_documents:
  - docs/framework-engineering/claim-and-confidence-policy.md
  - docs/framework-engineering/provenance-sidecars.md
  - schemas/edf-provenance-sidecar.schema.json
tags:
  - provenance
  - independence
  - claim-policy
confidence: high
---

# Decision

EDF research artifacts may keep provenance: cases, reviews, analyzer runs, evaluations, diagnostic findings, evidence, hypotheses, and conclusions. Provenance records who authored, reviewed, executed, or evaluated an artifact, and in which run.

That identity is **provenance, not evidence quality**. The Claim and Confidence Policy is therefore versioned to 1.1, with a new Independence subsection. Actor identity is self-reported provenance. It is used only for:

- independence and separation checks;
- stratified reporting.

It MUST NOT change evidence weight, scores, claim state, or confidence. This holds whether the actor is an OpenAI, Anthropic, or Google model, a human, or unknown.

For future artifacts, identity may be recorded in an optional **provenance sidecar** (`docs/framework-engineering/provenance-sidecars.md`, `schemas/edf-provenance-sidecar.schema.json`). The sidecar sits at `research/experiments/<EX-ID>/provenance/<kind>s/<id>.provenance.json`, outside every frozen/hashed set and every analyzer-, reviewer-, and scorer-visible input. It embeds the Praxis provenance interchange record unchanged.

EDF takes no dependency on Praxis code. A local, dependency-free codec (`scripts/praxis-provenance-record.mjs`) is conformance-tested against vendored Praxis fixtures.

Upstream contract (not restated here): Praxis `RQ-ROS-2026-A010` (provenance is not attestation, authorization, evidence, or evidence weight), `RQ-ROS-2026-A013` (interchange record), `RQ-ROS-2026-A014` (execution propagation), `RQ-ROS-2026-A015` (no silent stripping), and `DF-ROS-2026-A037`.

## Context

- **Existing separation checks.** EX-EDF-2026-A003 already uses author and reviewer *family* for separation: `scripts/edf-002-freeze.mjs` refuses a same-family review. It also stratifies results by author family (`byAuthor` in `scripts/edf-002-score.mjs`). Both are legitimate uses of identity.
- **Unwritten boundary.** Nothing stated that identity must never become a weight, a prior, or a tie-breaker.
- **A tempting mistake.** An identity-weighted reading would be easy to introduce and hard to notice. An example is treating a finding as stronger because a particular provider or a human produced it.
- **Frozen inputs.** Reviews and the scorer are hashed inputs of preregistered experiments. Any provenance mechanism must therefore not touch them.

## Rules

1. **Scope of use.** Identity is used only for independence/separation checks and stratified reporting.
2. **Not evidence.** Identity never changes evidence weight, scores, claim state, or confidence labels.
3. **Unknown stays unknown.** Identity is recorded only as declared. Unknown is recorded as `unknown`, never inferred from Git authorship, prose, `author_agent`-style fields, or telemetry.
4. **No backfill.** Historical artifacts keep whatever identity fields they already have, such as `authorFamily`, `reviewerFamily`, and `reviewerModel` in hashed review files. Nothing is added to or removed from them.
5. **Sidecar placement.** Sidecars are optional and live outside every hashed set and every analyzer-visible input. No scoring, analysis, validation, freeze, or prompting script reads them.
6. **Reject, do not drop.** Malformed or misplaced sidecars fail the test suite; they are never silently dropped.

## Alternatives considered

- **Add actor fields to review/run/case files.** Rejected. Those files are hashed at freeze and are analyzer- or reviewer-visible, so this would change frozen inputs and could leak identity into analysis.
- **Put run sidecars beside run records in `runs/`.** Rejected. `analyzeRuns` reads every `runs/*.json`, which would couple the scorer's input set to provenance files.
- **Put review sidecars beside reviews in `reviews/`.** Rejected. It would work only because of the current `*.review.json` filter in the freeze script, a coupling that a future freeze change could silently break.
- **Record this decision in `docs/00-governance/Governance-Decision-Log.md`.** Not done. That file is ROS tool-owned, so a local edit would fail `./ros verify` and be overwritten on upgrade. The navigation entry is in `docs/decisions/README.md` (shared) instead.

## Consequences

- **Tested invariance.** `tests/identity-neutral-scoring.test.mjs` shows that the unmodified scorer's per-output scores and the aggregate analysis are identical when identity fields are added, permuted, replaced, or removed. Relabelling author families only relabels the `byAuthor` stratum.
- **Isolation guards.** `tests/provenance-sidecar.test.mjs` enforces sidecar validity and placement, identity-free analyzer inputs, and unchanged `frozen-files.json` hashes.
- **Reversible.** Removing the convention deletes only files it added. No frozen artifact depends on it.

## Follow-up validation

- The first experiment that writes sidecars should confirm, before unblinding, that no sidecar path appears in its prompts or review bundles. The existing tests enforce this structurally.
