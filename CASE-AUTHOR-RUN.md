# Run Anthropic case authoring for EX-EDF-2026-A003

Author these six assigned cases:

- ST-002
- ST-004
- ST-006
- ST-008
- ST-010
- ST-012

For each case, in order:

1. Run `node scripts/edf-002-case-author.mjs prompt <CASE_ID>`.
2. Give the emitted prompt to a **fresh isolated Claude Opus 5 context** with no access to prior authored cases, OpenAI cases, analyzer results, evaluator results, web search, or prior conversations.
3. Have that context return the contents for the two requested JSON files.
4. Write them to:
   - `research/experiments/EX-EDF-2026-A003/cases/<CASE_ID>.case.json`
   - `research/experiments/EX-EDF-2026-A003/cases/<CASE_ID>.truth.json`
5. Run `node scripts/edf-002-case-author.mjs validate <CASE_ID>`.
6. If validation fails, revise before committing. Revisions are allowed in the case-authoring phase because no analyzer run exists yet. Do not weaken the validator.
7. Commit and push that one case/truth pair before starting the next case.
8. Destroy/close the author context before continuing.

After all six cases pass, run:

`node scripts/edf-002-validate.mjs validate`

Do not merge this branch and do not begin analyzer execution. Opposite-provider review and case freeze happen next.
