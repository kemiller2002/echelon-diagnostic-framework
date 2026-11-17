# OD2 harness (F#, `dotnet fsi`)

Run from `research/2026-comparative-program/`. Requires .NET 10 SDK and the
`claude` CLI (headless; tested with 2.1.326). All scripts are pure functions
around a thin IO edge.

| Script | Purpose |
|---|---|
| `Claude.fsx` | One isolated `claude -p` session: Read tool only, fixed system prompt, no settings or MCP, structured output, full stream-json transcript, token/cost/thinking telemetry |
| `Domain.fsx` | Case/truth model, structural validation, deterministic scoring, bootstrap and Wilson intervals |
| `Author.fsx` | Author cases from the frozen contract; `validate <split>` |
| `Review.fsx` | Blind solve → adjudication → revision |
| `Prompts.fsx` | Generate EDF-derived condition modules from `docs/edf/specification.md` |
| `Plan.fsx` | Build the preregistered held-out run plan |
| `Run.fsx` | Execute a plan (resumable; seeded order; per-run shuffled catalogue labels; tool-call audit) |
| `Score.fsx` | Score raw runs (`score`), summary statistics (`stats`); vote arms via `NAME=SOURCE:k` |
| `Rate.fsx` | Track B blind brief rating |
| `Analyze.fsx` | `verdicts` (preregistered decision rules, H10, class/M5 tables) and `tracks` (A/B/C) |

## Reproduce the held-out analysis from committed raw outputs

```bash
dotnet fsi harness/Author.fsx validate heldout
dotnet fsi harness/Score.fsx score experiments/raw/heldout /tmp/od2 A_VOTE5=A_GENERAL:5
dotnet fsi harness/Score.fsx stats /tmp/od2 20261117 "B_EDF03,A_GENERAL"
dotnet fsi harness/Analyze.fsx verdicts /tmp/od2 20261117 /tmp/od2/verdicts.md
dotnet fsi harness/Analyze.fsx tracks /tmp/od2 experiments/raw/heldout experiments/ratings/heldout 20261117 /tmp/od2/tracks.md
```

Transcripts are gzipped under `experiments/raw/heldout/transcripts/`.
