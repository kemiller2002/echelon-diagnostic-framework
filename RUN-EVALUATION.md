# Run EX-EDF-001 E2 blinded evaluation

E2 should use **Claude Sonnet 5**, intentionally different from the Opus 5 analyzer used for A2. If Sonnet 5 is unavailable, stop before binding or opening the first anonymous item.

The helper creates a 36-item blinded primary evaluation packet from the completed A1 and A2 branches. It does not expose analyzer identity, provider, model, prompt condition, run id, or source branch to the evaluator.

## Start

1. Confirm branch `experiment/edf-e2-claude-evaluator`.
2. Read only this file, `evaluator-setup.json`, and `scripts/edf-manual-evaluator-run.mjs`.
3. Run `node scripts/edf-manual-evaluator-run.mjs verify`.
4. Run `node scripts/edf-manual-evaluator-run.mjs prepare`.
5. Select Claude Sonnet 5 and verify the exact displayed model label.
6. Bind before opening any item:
   `node scripts/edf-manual-evaluator-run.mjs bind "claude-sonnet-5" "Claude Code remote session"`
7. Commit and push the packet and binding.

## Evaluate

Repeat until `next` says `COMPLETE`:

1. `node scripts/edf-manual-evaluator-run.mjs next > /tmp/edf-eval-next.txt`
2. Read only the anonymous label and exact prompt emitted by the helper.
3. Start a **fresh isolated Claude Sonnet 5 context**.
4. Give it exactly that prompt, with no repository access, web/search, memory, prior evaluations, or analyzer information.
5. Save the raw response verbatim.
6. Record it with:
   `node scripts/edf-manual-evaluator-run.mjs record <ANONYMOUS_LABEL> /tmp/edf-eval-output.json`
7. Commit and push the new E2 evaluation files.
8. Destroy that evaluator context before the next item.

A schema-invalid evaluator response is terminal data and must not be repaired or retried.

## Finish

Run `node scripts/edf-manual-evaluator-run.mjs finish`, commit and push the completion record, and report only the model label, 36/36 terminal evaluations, valid/invalid counts, and final commit SHA.

Do not unblind or aggregate scores on this branch.
