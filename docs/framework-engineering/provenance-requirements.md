---
id: RQ-EDF-2026-PROVENANCE
title: Provenance of EDF research records
status: accepted
version: 1.2.0
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
Keys are compared normalised: lowercased, with `_`, `-`, and spaces removed,
so `agent_id`, `Author`, and `created-by-agent` are identity fields too.
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
JSON keys or anywhere in the text. JSON keys are compared normalised
(lowercased, `_`, `-`, and spaces removed) against the identity key set,
which includes `actor`, `author`, `agentId`, `sessionId`, `runId`,
`threadId`, `conversationId`, `executionId`, `generatedBy`, `producedBy`, and
`writtenBy`. The identity text signals run on the raw text and on every
decoded JSON member name and string value, so JSON escapes cannot hide them.
Word matching treats `_` as a separator (`claude_code` is caught). The
environment variable names are every name in Praxis
`identity-environment.json` plus any `ROS_ACTOR*`, `ROS_EXECUTION*`, or
`ROS_TELEMETRY*` name. Schema tags include `praxis.provenance/` and
`echelon.execution-envelope/`, and model names include forms such as
`gpt4o`, `gpt-4o`, and `claude-*`. Blinded JSON that repeats a member name
within one object is rejected, because the shadowed value would never be
checked. `provider`, `model`, and `runtime` (and their `-name`/`-version`
forms) are
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
`provenance` block is malformed. Registry files are read as text: a
registry that repeats a member name within one object, or whose `provenance`
is `null` or holds an unpaired surrogate, fails (Praxis contract 1.2 rule 1
and rule 6). The check is read-only and passes on the
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
| RQ-EDF-2026-A004 | DF-ROS-2026-A037 | `blindedMaterialFindings`, `isBlindedMaterial`, `normaliseKey`, `identitySignals`, `duplicateMemberNames` | `tests/provenance-integrity.test.mjs` (blinded leaks, round-3 bypasses, ordinary text that stays clean); repository run |
| RQ-EDF-2026-A005 | DF-ROS-2026-A036 | no edits to frozen paths | `git diff --stat` on frozen paths; v0.3 blob-hash check in `scripts/repository-integrity.mjs` |
| RQ-EDF-2026-A006 | RQ-ROS-2026-A018 | `scripts/repository-integrity.mjs` calls `validateProvenanceIntegrity` | `node --test`; `node scripts/validate-repository.mjs`; vendored-file SHA-256; the 70 Praxis conformance cases, 14 text cases (also through `registryTextFindings`), 8 lineage cases, and 12 envelope-key cases (vendored library) |

## Revision notes

- **1.2.0** (2026-09-26, FEAT-ECHELON-PROVENANCE-R3; Praxis contract
  revision 1.2 at `b003718`, second adversarial review finding 9).
  RQ-EDF-2026-A002, A004, and A006 are tightened: identity keys are
  normalised; identity signals run on every decoded JSON string; all Praxis
  identity environment variable names are matched; `_` is a word separator;
  schema tags and model-name forms are widened; and JSON that repeats a member
  name is rejected in blinded material and registries. Praxis files are
  re-vendored at `b003718`, including the new text, lineage, and envelope-key
  fixtures, and the `addLineage` result shape (`{ ok, block }`) is adopted. No
  frozen or preregistered material changed, and the repository still passes.
- **1.1.0** (FEAT-ECHELON-PROVENANCE-R2; Praxis contract revision 1.1). The
  full identity key set and a text scan of every blinded file.
- **1.0.0** (FEAT-ECHELON-PROVENANCE). Initial requirements.
