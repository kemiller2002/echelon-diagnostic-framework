---
id: RQ-EDF-2026-PROVENANCE
title: Provenance of EDF research records
status: accepted
created: 2026-09-26
updated: 2026-09-26
requirements: [RQ-EDF-2026-A001, RQ-EDF-2026-A002, RQ-EDF-2026-A003, RQ-EDF-2026-A004, RQ-EDF-2026-A005, RQ-EDF-2026-A006]
related_documents:
  - DF-EDF-2026-A001
  - docs/framework-engineering/claim-and-confidence-policy.md
  - "praxis:DF-ROS-2026-A036"
  - "praxis:DF-ROS-2026-A037"
  - "praxis:RQ-ROS-2026-A004"
  - "praxis:RQ-ROS-2026-A010"
  - "praxis:RQ-ROS-2026-A015"
  - "praxis:RQ-ROS-2026-A019"
---

# Provenance of EDF research records

These requirements apply the Praxis agent provenance contract
(DF-ROS-2026-A036, DF-ROS-2026-A037) to EDF research. They do not redefine
what an actor, execution, contribution, or `unknown` means; Praxis is
authoritative. The decision behind them is
[DF-EDF-2026-A001](../../research/decisions/DF-EDF-2026-A001--actor-identity-is-provenance-not-evidence.md).

## RQ-EDF-2026-A001 New research records retain Praxis provenance

New Markdown research artifacts (hypotheses, evidence, decisions, journals,
packages, analyses, findings, conclusions) SHOULD record who created or changed
them as Praxis front-matter provenance with
`ros provenance record --path <file> --operation <op> --execution <EXE>`
(Praxis RQ-ROS-2026-A004) once the repository's ROS release supports it.
New claim-registry entries (`research/registries/*.json`) MAY carry a
top-level `provenance` field holding a `praxis.provenance/1` block
(Praxis RQ-ROS-2026-A015). Actor values come only from explicit declarations;
unknown values are recorded as `unknown`, never guessed.

## RQ-EDF-2026-A002 Actor identity is provenance, not evidence quality

Whether a record was produced by an OpenAI, Anthropic, Google, or other model,
a human, or automation SHALL NOT increase or decrease its evidentiary weight,
confidence label, claim state, or public claim (Praxis RQ-ROS-2026-A019,
RQ-ROS-2026-A010). Recorded identity is self-reported and unverified.

In the claim registries, identity fields (`actor`, `author`, `authorAgent`,
`authorFamily`, `executor`, `executorFamily`, `provider`, `model`, `runtime`,
and similar) SHALL appear only inside an entry's top-level `provenance` block.
`confidence`, `state`, and `publicState` SHALL remain labels, not structures
that could carry inputs.

## RQ-EDF-2026-A003 Executor identity as a declared experimental variable

Executor identity is relevant to a claim only when a preregistration declares
it as the experimental variable, as in cross-executor reproducibility. Then it
is an experimental condition, recorded in sealed or evaluator material (for
example an executor binding file), and results are grouped by the declared
condition. It is still not a quality signal for any single output.

## RQ-EDF-2026-A004 Blinded material never carries provenance

Analyzer-facing blinded material (`*.case.json`, analyzer packets, and an
experiment's analyzer inputs such as `cases.json`, `prompts.json`,
`prompt-modules.json`, and `output-contract.json`) SHALL NOT contain a
`provenance` block, a `praxis.provenance/...` tag, author/agent/executor/
execution identity fields, legacy author fields (`author_agent`,
`authorAgent`, `created_by_agent`, `owner_agent`, `source_author`), Praxis
execution or contributor keys (`EXE-`, `EXT-`, `CTB-` in any form), actor
environment variable names, or AI provider/model/runtime names, whether as
JSON keys or anywhere in the text. `provider`, `model`, and `runtime` are
allowed as keys only because incident cases describe systems with those
attributes; their values are still checked. This mirrors Percepta's
experiment leakage check (PCT-038). Case authorship and executor bindings
live in evaluator material (truth files, assignments, executor bindings).

## RQ-EDF-2026-A005 No retroactive change

Frozen, preregistered, released, or hash-pinned material SHALL NOT be edited
to add, correct, or remove provenance. This includes `docs/edf/releases/v0.3/**`,
`research/experiments/EX-EDF-001/**` (whose executor matrix stays unbound until
the gated preregistered binding), and `research/experiments/EX-EDF-2026-A003/**`.
Existing registry entries without provenance stay valid and read as
unattributed; no author is inferred or backfilled.

## RQ-EDF-2026-A006 Mechanical enforcement

`node scripts/validate-repository.mjs` and `node --test` SHALL fail when
RQ-EDF-2026-A002 or RQ-EDF-2026-A004 is violated or when a registry entry's
`provenance` block is malformed. The check is read-only and passes on the
current frozen material.

## Repository policy

`ros.json` declares `rosVersion` 3.1.1 and the installed toolchain
(`.echelon/ros.json`) runs ROS 3.1.4. Both predate the Praxis provenance
policy and `ros provenance`, and `ros.json` is a ROS-managed file pinned by
`.ros/installation.json`. The `ros.json` `provenance` policy block is
therefore not added; it is a follow-up for the ROS upgrade, with a
`requiredFrom` date after that upgrade so all existing records stay legacy.

## Traceability

| Requirement | Praxis source | Implementation | Verification |
|---|---|---|---|
| RQ-EDF-2026-A001 | RQ-ROS-2026-A004, A015, A016 | registry `provenance` field accepted and classified by `scripts/provenance-integrity.mjs` using the vendored `scripts/vendor/praxis/provenance-interchange.mjs` | `tests/provenance-integrity.test.mjs` (inbound block, round trip, contributors, lineage, versions) |
| RQ-EDF-2026-A002 | RQ-ROS-2026-A019, A010 | `registryEntryFindings`; claim-and-confidence policy "Provenance is not evidence" | `tests/provenance-integrity.test.mjs` ("identity fields outside the provenance block are rejected", "identity is not weight") |
| RQ-EDF-2026-A003 | RQ-ROS-2026-A019 | claim-and-confidence policy | protocol review |
| RQ-EDF-2026-A004 | DF-ROS-2026-A037 | `blindedMaterialFindings`, `isBlindedMaterial` | `tests/provenance-integrity.test.mjs` (blinded leaks); repository run |
| RQ-EDF-2026-A005 | DF-ROS-2026-A036 | no edits to frozen paths | `git diff --stat` on frozen paths; v0.3 blob-hash check in `scripts/repository-integrity.mjs` |
| RQ-EDF-2026-A006 | RQ-ROS-2026-A018 | `scripts/repository-integrity.mjs` calls `validateProvenanceIntegrity` | `node --test`; `node scripts/validate-repository.mjs`; vendored-file SHA-256 and 40 Praxis conformance cases |
