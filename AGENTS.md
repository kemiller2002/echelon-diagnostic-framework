# EX-EDF-2026-A003 CLAUDE CROSS-REVIEW BRANCH

This branch contains only the frozen OpenAI-authored case/truth pairs required for Anthropic review.

Review only ST-001, ST-003, ST-005, ST-007, ST-009, and ST-011.

Do not inspect:
- the OpenAI authoring branch or its history;
- Claude-authored cases;
- any EX-EDF-001 analyzer/evaluator result;
- any EX-EDF-2026-A003 analyzer output;
- other branches, pull requests, issues, or repository history.

Each case must be reviewed in a fresh isolated Claude context. The coordinator may read/write review files and run validators, but must not solve the cases itself.

Follow REVIEW-RUN.md exactly.
