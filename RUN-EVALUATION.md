# Run EX-EDF-001 E1 blinded evaluation

E1 uses OpenAI **GPT-5.6 Terra** through Codex CLI. Terra is intentionally different from the Sol model used for A1.

The helper mechanically fetches the completed A1/A2 branches, verifies 24/24 clean completion for each, takes only the 36 primary r1 outputs, anonymizes them with the frozen EX-EDF-001 label rule, and sorts them using the preregistered E1 evaluator order.

The evaluator never receives analyzer identity, provider, model, condition, run id, source branch, or prior evaluations.

## Run

Authenticate Codex with your ChatGPT account if necessary, then execute:

`bash scripts/run-edf-e1-with-codex.sh gpt-5.6-terra`

Each anonymous item is evaluated in a new empty temporary Git repository using a fresh `codex exec --ephemeral` session. The runner commits each evaluation append-only.

Schema-invalid evaluator output is preserved as terminal data and is not repaired or retried. A Codex transport/session failure before a response is returned records nothing and stops safely.

When complete, report only the evaluator model, 36/36 terminal evaluations, valid/invalid counts, and final commit SHA. Do not unblind or aggregate results on this branch.
