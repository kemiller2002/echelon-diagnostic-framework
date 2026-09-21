# Run EX-EDF-001 A2 (Anthropic)

This branch executes one preregistered analyzer slot without API calls.

## Scientific boundary

This is analyzer execution only. Do not score results and do not inspect hidden truth. The branch packet removes each case's hidden-purpose label. Canonical source blob SHAs and the source commit are recorded for provenance.

There are **24 terminal runs**: 18 primary units plus six repeatability units. Each run must use a **fresh isolated context**. A schema-invalid response is data and is terminal. Do not repair or retry it.

## Start

1. Confirm branch: `experiment/edf-a2-claude-manual`.
2. Read only this file, `analyzer-packet.json`, and `scripts/edf-manual-agent-run.mjs`.
3. Run `node scripts/edf-manual-agent-run.mjs verify`.
4. Run `node scripts/edf-manual-agent-run.mjs isolate`.
5. Identify the **exact model label selected in the Claude host UI/runtime**. Keep it fixed for all 24 runs.
6. Bind it before opening a case:
   `node scripts/edf-manual-agent-run.mjs bind "<EXACT MODEL LABEL>" "<HOST SURFACE>"`
7. Commit and push the binding before any run:
   `git add research/experiments/EX-EDF-001/manual-runs/A2/executor.json && git commit -m "research: bind A2 Anthropic executor" && git push`

## Execute

Repeat until `next` says `COMPLETE`:

1. `node scripts/edf-manual-agent-run.mjs next > /tmp/edf-next.txt`
2. Read only `RUN_ID` and the exact analyzer prompt from that file.
3. Launch a **new isolated analyzer context/subagent** with the bound model.
4. Give it exactly the emitted prompt and nothing else. No repo, web, search, connected tools, memory, prior outputs, or prior runs.
5. Save its raw response verbatim to `/tmp/edf-output.json`. Do not edit it.
6. `node scripts/edf-manual-agent-run.mjs record <RUN_ID> /tmp/edf-output.json`
7. Commit and push the new run files immediately:
   `git add research/experiments/EX-EDF-001/manual-runs/A2/runs && git commit -m "research: record A2 <RUN_ID>" && git push`
8. Destroy/close that analyzer context before the next run.

Never rerun a terminal run.

## Finish

Run `node scripts/edf-manual-agent-run.mjs finish`.

Then:
`git add research/experiments/EX-EDF-001/manual-runs/A2/completion.json && git commit -m "research: complete A2 analyzer execution" && git push`

Report only branch, bound model label, terminal run count, valid/invalid count, and final commit SHA. Do not interpret EDF performance.
