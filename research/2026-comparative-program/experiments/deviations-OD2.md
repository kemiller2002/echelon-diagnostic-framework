# OD2 deviation and change log (after preregistration freeze)

| # | When (relative to held-out results) | Change | Effect on confirmatory analysis |
|---|---|---|---|
| D-1 | After freeze, held-out runs in progress, **no held-out result scored or viewed** | Added the `verdicts` command to `harness/Analyze.fsx`. It implements the preregistered §4 decision rule, H10 test–retest, and class, domain and M5 breakdowns. `frozen-prompts-OD2.sha256` covers the earlier `Analyze.fsx`, whose hash no longer matches. Prompts, plan, cases, truth and scoring code are unchanged. | None to the analyses themselves. They were preregistered; the command only computes them. |
