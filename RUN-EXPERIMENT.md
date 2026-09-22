# Run EX-EDF-001 A1 (OpenAI)

This branch executes one preregistered OpenAI analyzer slot without API billing.

## Preferred execution path: Codex CLI

OpenAI does **not** need to spawn subagents. The automated runner starts a separate `codex exec --ephemeral` process for every experiment unit. Each invocation runs in a newly initialized empty temporary Git repository and receives only the exact frozen analyzer prompt.

The analyzer cannot see this repository, Git history, Claude results, hidden ground truth, evaluation rubric, prior outputs, or prior Codex sessions.

There are **24 terminal runs**: 18 primary units plus six preregistered repeatability units. A schema-invalid response is terminal data and is not repaired or retried.

### One-time prerequisite

Use Codex CLI authenticated with the same ChatGPT account you want to use for the OpenAI arm. If needed:

`codex login --device-auth`

Do not configure an OpenAI API key for this experiment. ChatGPT-authenticated Codex uses the Codex access included with the ChatGPT plan and its applicable usage limits.

### Run the complete A1 arm

From this branch:

`bash scripts/run-edf-a1-with-codex.sh gpt-5.6-sol`

The script will:

1. verify the A1 branch and packet;
2. bind A1 permanently to `gpt-5.6-sol` before revealing any case;
3. commit and push the executor binding;
4. request the next frozen run from the existing helper;
5. create an empty temporary Git repository;
6. invoke `codex exec --ephemeral` with no user config/rules and a read-only sandbox;
7. provide exactly one analyzer prompt through stdin;
8. record the raw final response append-only;
9. commit and push that run immediately;
10. discard the temporary repository and Codex session;
11. repeat through all 24 units;
12. create, commit, and push the completion manifest.

If Codex itself fails before returning a response, the script stops and records nothing for that unit. Rerunning the script resumes at the same unrecorded unit. If the model returns invalid JSON/schema, that response is preserved as terminal experimental data and the script proceeds.

## Isolation rules

Do not manually inspect the canonical case files, ground truth, evaluation rubric, other branches, Claude output branch, repository history, prior conversations, or external information while executing A1.

Do not edit analyzer outputs.

Do not merge this branch into `main`.

## Manual fallback

The existing `scripts/edf-manual-agent-run.mjs` workflow remains available if Codex CLI itself is unavailable. Each manual run must still use a brand-new top-level OpenAI conversation/session, not another turn in an existing conversation.

## Finish

When the runner reports `A1 COMPLETE`, report only:

- branch
- bound model label
- 24/24 terminal runs
- valid/invalid counts
- final commit SHA

Do not interpret whether EDF won or lost. Evaluation is a later blinded stage.
