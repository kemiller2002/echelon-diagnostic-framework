# OD2 held-out review summary

Review procedure: `harness/Review.fsx`. An isolated Opus 5.5 session (effort high)
solved each case blind with the full catalogue. A second isolated session adjudicated
every item where the blind answer differed from the key, or that the solver flagged
as ambiguous.

| Round | Cases reviewed | Cases needing revision | Records |
|---|---|---|---|
| 1 | 26 | 12 (11 with ambiguous or key-wrong rulings, 1 structural: MISLEAD-1) | `round1/`, pre-revision files in `round1/pre-revision/` |
| 2 | 12 revised | 3 (ADVERS-1, CHANGE-2, INCOMPLETE-1) | `round2/`, pre-revision files in `round2/pre-revision/` |
| 3 (final) | 3 revised twice | 2 still ambiguous (CHANGE-2, INCOMPLETE-1): **excluded** | current `OD2-HO-*.review.json` |

Most common ambiguity: whether more than one action can be "preferred". The shared
semantics define "preferred" as "among the best next actions", which allows several.
Revisions made the alternatives clearly inferior. Two other ambiguities recurred:
stage-S1 answers in CHANGE cases, and "supported vs unknown" when the evidence is
suggestive but a rival is not excluded.

Blind agreement with the final keys was 13/15 to 16/16 items per case.
Disagreements on included cases were adjudicated as key-uniquely-defensible.
