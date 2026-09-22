# Recover and complete Claude EX-EDF-2026-A003 case authoring

GitHub currently shows this branch at its original setup commit and contains no authored case/truth pairs. The coding session may have completed work locally without pushing it.

Do **not** immediately regenerate the six cases.

Assigned case IDs are:

- ST-002
- ST-004
- ST-006
- ST-008
- ST-010
- ST-012

## 1. Preserve any local work first

Run:

```bash
git status
git branch --show-current
git remote -v
git fetch origin
```

Check the working tree for the twelve assigned files under:

`research/experiments/EX-EDF-2026-A003/cases/`

If assigned case/truth files exist locally, preserve them. Do not overwrite them.

If the files are not in the working tree, locate commits containing only these exact assigned paths:

```bash
git log --all --oneline -- \
  research/experiments/EX-EDF-2026-A003/cases/ST-002.case.json \
  research/experiments/EX-EDF-2026-A003/cases/ST-002.truth.json \
  research/experiments/EX-EDF-2026-A003/cases/ST-004.case.json \
  research/experiments/EX-EDF-2026-A003/cases/ST-004.truth.json \
  research/experiments/EX-EDF-2026-A003/cases/ST-006.case.json \
  research/experiments/EX-EDF-2026-A003/cases/ST-006.truth.json \
  research/experiments/EX-EDF-2026-A003/cases/ST-008.case.json \
  research/experiments/EX-EDF-2026-A003/cases/ST-008.truth.json \
  research/experiments/EX-EDF-2026-A003/cases/ST-010.case.json \
  research/experiments/EX-EDF-2026-A003/cases/ST-010.truth.json \
  research/experiments/EX-EDF-2026-A003/cases/ST-012.case.json \
  research/experiments/EX-EDF-2026-A003/cases/ST-012.truth.json
```

This history search is allowed only for those twelve assigned paths. Do not inspect OpenAI-authored case paths or unrelated experiment history.

If prior local commits contain the assigned files, move those exact commits/files onto `experiment/edf-002-case-author-claude` without changing their scientific content.

## 2. Validate recovered work

Checkout the required branch and make sure it is current:

```bash
git checkout experiment/edf-002-case-author-claude
git pull --ff-only origin experiment/edf-002-case-author-claude
```

Restore/cherry-pick the recovered assigned case files if needed.

For every assigned case run:

```bash
node scripts/edf-002-case-author.mjs validate ST-002
node scripts/edf-002-case-author.mjs validate ST-004
node scripts/edf-002-case-author.mjs validate ST-006
node scripts/edf-002-case-author.mjs validate ST-008
node scripts/edf-002-case-author.mjs validate ST-010
node scripts/edf-002-case-author.mjs validate ST-012
```

Then run:

```bash
node scripts/edf-002-validate.mjs validate
```

Do not weaken the validator.

## 3. Only regenerate missing cases

If, after checking the working tree and exact-path local history, an assigned case/truth pair truly does not exist, follow `CASE-AUTHOR-RUN.md` for that missing case only.

Use a fresh isolated Claude Opus 5 context for each genuinely missing case.

Do not inspect the OpenAI case-authoring branch or its case files.

## 4. Commit and push

Commit each recovered or newly generated valid pair without altering its content merely to make it look different.

Push all six pairs to:

`origin/experiment/edf-002-case-author-claude`

Confirm GitHub contains exactly:

- ST-002.case.json / ST-002.truth.json
- ST-004.case.json / ST-004.truth.json
- ST-006.case.json / ST-006.truth.json
- ST-008.case.json / ST-008.truth.json
- ST-010.case.json / ST-010.truth.json
- ST-012.case.json / ST-012.truth.json

Do not merge the branch.

Finish by reporting the final pushed commit SHA and that all six validations pass.
