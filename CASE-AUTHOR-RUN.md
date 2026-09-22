# Run OpenAI case authoring for EX-EDF-2026-A003

This branch authors the six OpenAI-source stress cases, one per construct.

Use the automated isolated runner:

`bash scripts/run-edf-002-openai-case-author.sh gpt-5.6-sol`

The runner creates a new empty temporary Git repository and a fresh `codex exec --ephemeral` session for each assigned case. The session receives only the exact authoring prompt for that case. It cannot see previously authored cases or the wider EDF repository.

Each generated case/truth pair is copied back, validated mechanically, committed, and pushed before the next case.

If a generated pair fails validation, the runner stops and leaves the pair uncommitted for inspection/revision. Do not weaken the validator to make a case pass.

When all six pairs are valid and pushed, create no analyzer runs. Case review/freeze is a separate stage.
