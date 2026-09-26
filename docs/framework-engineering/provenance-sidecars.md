# Provenance sidecars

Status: Current research governance (optional convention for future artifacts)
Decision: `DF-EDF-2026-A001`
Policy: [Claim and Confidence Policy](claim-and-confidence-policy.md), Independence, v1.1

A provenance sidecar records who authored, reviewed, executed, or evaluated one EDF research artifact, and in which run. It adds provenance without touching the artifact, any frozen or hashed set, or anything an analyzer, reviewer, or scorer reads.

Sidecars are **optional** and apply to **future** artifacts only. Historical cases, reviews, runs, and reports are not backfilled, and nothing is inferred for them.

## Location

```
research/experiments/<EX-ID>/provenance/cases/<case-id>.provenance.json
research/experiments/<EX-ID>/provenance/reviews/<case-id>.provenance.json
research/experiments/<EX-ID>/provenance/runs/<run-id>.provenance.json
research/experiments/<EX-ID>/provenance/evaluations/<id>.provenance.json
```

These paths live in a separate `provenance/` directory rather than beside the artifact, for three reasons:

- `scripts/edf-002-freeze.mjs` hashes `cases/*.case.json`, `cases/*.truth.json`, and `reviews/*.review.json`.
- EX-EDF-001's `frozen-files.json` lists its files explicitly.
- `scripts/edf-002-score.mjs` reads every `runs/*.json` except `*.output.json`.

Sidecars in `provenance/` are never hashed and never read as runs.

## Shape

`schemas/edf-provenance-sidecar.schema.json`:

```json
{
  "contract": "edf.provenance-sidecar",
  "version": "1.0.0",
  "experiment": "EX-EDF-2026-A003",
  "kind": "review",
  "target": "research/experiments/EX-EDF-2026-A003/reviews/ST-001.review.json",
  "targetSha256": "<sha256 of the target when the sidecar was written>",
  "provenance": {
    "contract": "praxis.provenance-record",
    "version": "1.0.0",
    "subject": "edf:research/experiments/EX-EDF-2026-A003/reviews/ST-001.review.json",
    "contributions": {
      "EXE-edf.review-ST-001-r1": {
        "operations": ["created"],
        "at": "2026-10-01T12:00:00.000Z",
        "actor": { "kind": "agent", "id": "unknown", "provider": "unknown", "model": "unknown", "runtime": "unknown" }
      }
    }
  }
}
```

`provenance` is the Praxis provenance interchange record, unchanged.

- **Execution keys.** A contribution made inside a run is keyed by `ROS_EXECUTION_ID` when Praxis propagated one. Otherwise it is keyed `EXE-edf.<run>`. A human acting outside any run uses a `CTB-...` key.
- **Actor.** The actor comes only from declared, non-secret identity (`ROS_ACTOR_KIND`, `ROS_ACTOR`, `ROS_TELEMETRY_PROVIDER`, `ROS_TELEMETRY_MODEL`, `ROS_TELEMETRY_RUNTIME`). Anything undeclared is `unknown`. Secrets are never recorded.
- **History.** The history is append-only. A case's author records `created`, and a later reviewer appends `reviewed`. Nobody rewrites another contributor's entry.

Use `makeSidecar` and `executionKey` in `scripts/edf-provenance-sidecar.mjs` to write sidecars. It refuses to write malformed provenance.

## Rules

1. **Identity is provenance, not evidence.**
   - No scoring, analysis, validation, freezing, or prompting script reads sidecars.
   - Identity never changes evidence weight, scores, claim state, or confidence.
   - Identity may be used only for independence checks, such as requiring an opposite-family reviewer, and for stratified reporting.
2. **Analyzer-visible inputs stay identity-free.** Case files, prompt modules, and output contracts carry no actor, author, reviewer, or provenance fields.
3. **Sidecars are bound to bytes.** `targetSha256` must match the target. If the target changes, write a new sidecar; do not edit the old one in place.
4. **Malformed provenance is rejected, not dropped.** `node --test` fails on an invalid or misplaced sidecar.

## Verification

- `tests/provenance-sidecar.test.mjs` checks that:
  - sidecars validate;
  - their placement is outside every hashed set;
  - analyzer-visible inputs are free of identity;
  - scorers do not read sidecars;
  - `frozen-files.json` is unchanged.
- `tests/identity-neutral-scoring.test.mjs` checks that the unmodified scorer's output is identical when identity fields are added, permuted, replaced, or removed.
- `tests/praxis-provenance-conformance.test.mjs` runs the vendored Praxis conformance fixtures through the local codec.
