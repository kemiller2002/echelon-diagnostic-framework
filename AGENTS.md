# EX-EDF-001 ANALYZER EXECUTION BRANCH

This branch is an isolated analyzer workspace for **A2 / Anthropic**.

## Highest-priority execution rule

Do not inspect, search, summarize, or read repository content except:

- `RUN-EXPERIMENT.md`
- `analyzer-packet.json`
- `scripts/edf-manual-agent-run.mjs`
- `research/experiments/EX-EDF-001/manual-runs/A2/` as it is created

Do not inspect Git history, other branches, tags, pull requests, issues, commits, repository search, web search, connected apps, memory, prior conversations, or canonical experiment files outside the analyzer packet.

You are an **execution coordinator**, not the analyzer. Every run must be performed in a fresh isolated model context that receives only the exact prompt emitted by the helper script. Do not analyze a case in the coordinator context.

If fresh isolated contexts are unavailable, stop before producing any analyzer output. Do not substitute repeated turns in one context.

Follow `RUN-EXPERIMENT.md` exactly.
