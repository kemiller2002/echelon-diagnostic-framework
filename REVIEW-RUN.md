# Re-review revised ST-003

The first Claude cross-review correctly found lexical leakage in ST-003's title. The only scientific-content change since that review is:

- old title: `Notification ingress backlog after an audit-path rollout`
- new title: `Notification ingress backlog and retry surge`

Review the complete revised ST-003 case/truth pair again rather than assuming the title change fixed everything.

Run:

`node scripts/edf-002-review.mjs prompt ST-003 Anthropic`

Give the exact emitted prompt to a fresh isolated Claude Sonnet 5 context with no previous review, branch history, other cases, or analyzer results.

Save strict JSON to:

`research/experiments/EX-EDF-2026-A003/reviews/ST-003.review.json`

Then run:

`node scripts/edf-002-review.mjs validate ST-003`

Commit and push the review. Do not merge this branch.
