# Review OpenAI-authored EX-EDF-2026-A003 cases with Claude

Use **Claude Sonnet 5** as the reviewer unless the branch has already been bound to another explicit Anthropic review model before the first case.

Assigned cases:

- ST-001
- ST-003
- ST-005
- ST-007
- ST-009
- ST-011

For each case, in order:

1. Run:
   `node scripts/edf-002-review.mjs prompt <CASE_ID> Anthropic`
2. Give the exact emitted prompt to a **fresh isolated Claude context**.
3. Do not give that context repository history, other cases, previous reviews, web access, prior conversations, analyzer outputs, or OpenAI authoring information beyond the prompt.
4. Save the raw strict-JSON response to:
   `research/experiments/EX-EDF-2026-A003/reviews/<CASE_ID>.review.json`
5. Run:
   `node scripts/edf-002-review.mjs validate <CASE_ID>`
6. If the review verdict is `revise`, commit and push that review as-is. Do not edit the original case yourself and do not convert the verdict to pass.
7. If the review passes, commit and push the review.
8. Close/destroy that Claude review context before starting the next case.

When all six reviews are present, report:
- number reviewed;
- number pass;
- number revise;
- final branch SHA.

Do not merge this branch. Do not begin analyzer execution.
