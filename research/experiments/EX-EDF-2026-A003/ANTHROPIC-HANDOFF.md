# Anthropic Handoff for EX-EDF-2026-A003

This handoff preserves the preregistered provider-independence boundary. The Anthropic case author must not inspect the OpenAI-authored case/truth files before completing the six Anthropic-assigned cases.

## Authoring

Use a clean Anthropic session on this branch. Generate the exact authoring prompt for each assigned case with:

```bash
node scripts/edf-002-case-author.mjs prompt ST-002
node scripts/edf-002-case-author.mjs prompt ST-004
node scripts/edf-002-case-author.mjs prompt ST-006
node scripts/edf-002-case-author.mjs prompt ST-008
node scripts/edf-002-case-author.mjs prompt ST-010
node scripts/edf-002-case-author.mjs prompt ST-012
```

For each prompt, have Anthropic write only the requested `.case.json` and `.truth.json` files. Validate each pair immediately:

```bash
node scripts/edf-002-case-author.mjs validate ST-002
node scripts/edf-002-case-author.mjs validate ST-004
node scripts/edf-002-case-author.mjs validate ST-006
node scripts/edf-002-case-author.mjs validate ST-008
node scripts/edf-002-case-author.mjs validate ST-010
node scripts/edf-002-case-author.mjs validate ST-012
```

Do not freeze and do not run analyzers yet.

## Opposite-provider review of OpenAI cases

After Anthropic authoring is complete, use a separate clean Anthropic review session. Review the six OpenAI-authored cases:

```bash
node scripts/edf-002-review.mjs prompt ST-001 Anthropic
node scripts/edf-002-review.mjs prompt ST-003 Anthropic
node scripts/edf-002-review.mjs prompt ST-005 Anthropic
node scripts/edf-002-review.mjs prompt ST-007 Anthropic
node scripts/edf-002-review.mjs prompt ST-009 Anthropic
node scripts/edf-002-review.mjs prompt ST-011 Anthropic
```

Save strict JSON results under `research/experiments/EX-EDF-2026-A003/reviews/<CASE>.review.json` and validate each with `node scripts/edf-002-review.mjs validate <CASE>`.

If any review returns `revise`, revise the case before analyzer execution and repeat the opposite-provider review. Do not repair or tune cases after the first analyzer output exists.
