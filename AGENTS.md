# EX-EDF-2026-A003 ANTHROPIC CASE-AUTHORING BRANCH

This branch is for independent Anthropic case authoring only.

Read only the EX-EDF-2026-A003 specification, case-authoring helper, and assigned case files needed for this task. Do not inspect the OpenAI case-authoring branch or its cases. Do not inspect EX-EDF-001 analyzer/evaluator outputs or any future EX-EDF-2026-A003 analyzer results.

Author only ST-002, ST-004, ST-006, ST-008, ST-010, and ST-012.

Each case must be authored in a fresh isolated model context that receives only the exact case prompt emitted by:
`node scripts/edf-002-case-author.mjs prompt <CASE_ID>`

The coordinator may write/commit files but must not solve the case itself or reuse reasoning between cases.
