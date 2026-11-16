# EDF Implementation Audit (Baseline)

Audit agent: Agent A
Baseline commit: `41fea6ffc764adc69643d5ee2940916592486f84`
Audit date: 2026-11-16. All command results below were produced in this audit session with Node `v22.23.4`.

## 1. Is there a runtime diagnostic engine? No. Verified.

- `git ls-files` lists 368 tracked files. The only executable source files are: `scripts/edf-002-{analyzer,case-author,freeze,review,score,validate}.mjs`, `scripts/edf-benchmark.mjs`, `scripts/repository-integrity.mjs`, `scripts/validate-repository.mjs`, `tools/ros_fs_launcher.mjs`, `tests/*.test.mjs`, and `tests/browser/{Probes,Program}.fs` (website browser conformance).
- None of them performs an EDF diagnosis. A grep for `origin network|originNetwork|controlPoint` across tracked code matches only `scripts/edf-benchmark.mjs`, and there only in the rubric-leak check (`scripts/edf-benchmark.mjs:55`), which makes sure the evaluator rubric does **not** contain EDF vocabulary.
- The only UI that ever represented the EDF flow was `app/DiagnosticFlow.tsx`: a static, presentational tab list of seven stages with no input handling or inference. It was deleted, together with the whole `app/` React stack, in commit `b27f5ff` (2026-09-20).
- The public site (`static-site/*.html`: index, learn, start, examples, evidence) is static teaching content.
- **EDF exists only as prose specification plus Markdown templates** (`docs/edf/templates/edf-{0,1,2}-*.md`). An analysis is something a human or an LLM writes by following those documents. There is no schema for an EDF analysis, no parser, no validator of EDF outputs, no store of analyses, and no API.

## 2. What is implemented

### 2.1 Repository-integrity tooling (governance of claims, not diagnosis)

`scripts/repository-integrity.mjs` (218 lines), called by `scripts/validate-repository.mjs` and `tests/repository-integrity.test.mjs`. It checks:
- registry parsing, unique IDs, allowed states and confidence labels, and cross-references HY→EV and TH→HY (`:86-131`);
- that the current REP `research/packages/RP-EDF-2026-002.md` exists with its required sections (`:136-140`), and that journal `JR-EDF-2026-003.md` exists (`:143`);
- the frozen v0.3 bundle: 9 files, git-blob SHA against `docs/edf/releases/v0.3/manifest.json` (`:147-160`);
- local Markdown/HTML link integrity (`:179`);
- that every hypothesis cited on the public site uses the registry's `publicState` (public claim drift check, `:188-189`);
- the Forma lock, no stale duplicate web stacks, and that `reproducibility-findings.md` contains the required boundary text (`:194-203`).

**Not checked:** whether `docs/edf/validation/evidence-ledger.md` (the "human-readable projection" of the registries) matches the registries. It does not (see §6.1).

### 2.2 EX-EDF-001 harness: `scripts/edf-benchmark.mjs` (177 lines)

- `validate` (`:34-70`): exactly 6 cases and 3 conditions; matched-neutral prompt length ≥ 90% of the EDF prompt (`:52`); the rubric must not leak EDF vocabulary (`:55`); rubric weights sum to 100; the output contract has 6 fields; SHA-256 of the 7 frozen artifacts; executor-matrix readiness.
- `run` / `evaluate` (`:111-148`): refuse to execute unless `executor-matrix.json` is `confirmatory-ready` with provider, model, version, config path and config hash bound (`:106-109`). Executors are external commands in empty temp dirs (`:86-95`). Writes run bundles to `research/experiments/EX-EDF-001/runs/` and `evaluations/`.
- `analyze` (`:156-170`): needs ≥ 18 paired units (three analyzer families) and computes mean difference, seeded xorshift bootstrap and success criteria.
- **Scoring mechanism:** an LLM evaluator gives five dimensions × 0-4 (`evaluation-rubric.json`), × 5 → 0-100 (`:150`); mean of E1/E2, with E3 median if the difference is ≥ 20 (`:162`).

### 2.3 EX-EDF-2026-A003 harness: `scripts/edf-002-*.mjs` ("edf-002" is a legacy name, see `research/journal/JR-EDF-2026-004.md:33`)

| Script | Role |
|---|---|
| `edf-002-validate.mjs` | Spec checks: 12 assignments, 6 constructs × {OpenAI, Anthropic}, prompt-module length ratio ≤ 1.20, condition modules must not name "EDF" (`:10-27`). Pair checks: hard minimums for evidence, candidates, scored items, unknown items, red herrings, conflicts, reasoning hops, harmful actions (`:29-65`). `freeze` mode requires 12 cases (`:72`). |
| `edf-002-case-author.mjs` | Produces the authoring prompt packet for a case |
| `edf-002-review.mjs` | Produces the opposite-provider review packet; validates review JSON |
| `edf-002-freeze.mjs` | Refuses to freeze unless all cases have an opposite-family `pass` review with every check true (`:14-29`); writes the SHA-256 manifest `frozen-files.json` once (`flag:'wx'`) |
| `edf-002-analyzer.mjs` | Manual-run workflow: `prepare`, `bind`, `next` (prints exact prompt), `record` (validates and stores raw output append-only), `finish`. Requires `frozen-files.json` (`:12-15`). Output goes to `manual-runs/<slot>/` |
| `edf-002-score.mjs` | Deterministic mechanical scorer (below) |

**A003 scoring mechanism** (`scripts/edf-002-score.mjs:14-39`; `scoring-spec.json`):
- 70 points weighted exact state accuracy (hypotheses and edges: supported / contradicted / unknown; actions: preferred / acceptable / reject / harmful / unknown);
- 20 points evidence-link F1, with precision against `allowedEvidence` and recall against `requiredEvidence`;
- 10 points "calibration" = `correct ? conf/100 : 1 - conf/100` (`:30-31`);
- −15 per harmful action answered preferred or acceptable; clamped to 0..100;
- primary: construct-critical items only; paired construct minus neutral over 24 units; seeded bootstrap (`:44-66`).

**Scoring-design issues found in this audit (not documented in the repo):**
1. The calibration component is a **linear, improper scoring rule**. For an answer with subjective accuracy p, expected credit is p·c + (1−p)(1−c). That is maximized at c = 100 whenever p > 0.5, so it rewards overconfidence rather than calibration. A Brier or log score would be proper. Its weight is only 10 points.
2. When an analyzer cites no evidence and the item has no required evidence, precision is set to 1 (`:27`). Citing nothing can then earn full evidence credit on such items.
3. With answer-state exact match carrying 70% of the score, the outcome is dominated by agreement with the case author's hidden truth. Truth validity therefore rests entirely on the single opposite-provider reviewer (see §4.2: 4 of 6 cases are still flagged `groundTruthSupported=false`).
4. Prompt/case coupling. The construct modules name the exact trap each case is built around. For example, the protective-controls module says "Do not weaken validation, monitoring, circuit breaking, quarantine..." (`prompt-modules.json`, `construct.protective-controls`), and case ST-011 is a "Billing pipeline quarantine surge" whose harmful action is disabling quarantine paging. The neutral module is a generic "disciplined generic review". The A003 design therefore partly tests "telling the model what kind of trap is present" versus not telling it, rather than EDF semantics per se. Cases were authored knowing the construct.

### 2.4 Tests: actual results (run 2026-11-16)

`node --test` (repo root):

```
ok 1 - EX-EDF-001 benchmark is frozen and fail-closed before executor binding
ok 2 - EX-EDF-001 cannot manufacture a result without complete runs and evaluations
ok 3 - mechanical score rewards evidence-grounded uncertainty and penalizes harmful action promotion
ok 4 - EX-EDF-2026-A003 preregistered specification is internally valid
ok 5..7, 9..16 - Forma/website conformance tests
ok 8 - every class the site uses is defined by Forma or declared locally # SKIP Forma bundle not installed ...
ok 17 - EDF repository integrity
# tests 17  # pass 16  # fail 0  # skipped 1
```

Other commands:

| Command | Result |
|---|---|
| `node scripts/validate-repository.mjs` | `Repository integrity PASSED`; hypotheses 16, evidenceRecords 9, theoryRecords 3, repsChecked 1, frozenV03FilesChecked 9, checkedLinkFiles 123, requiredWebsiteFiles 8, formaVersion 0.13.0; exit 0 |
| `node scripts/edf-benchmark.mjs validate` | `{experimentId: EX-EDF-001, cases: 6, conditions: 3, frozenArtifacts: 7, executorMatrixReady: false, analyzerRuns: 0}`; exit 0 |
| `node scripts/edf-benchmark.mjs analyze` | `{status: "not-evaluable", reason: "No complete run/evaluation set exists.", primaryUnits: 0, requiredPrimaryUnits: 18}` |
| `node scripts/edf-002-validate.mjs validate` | `{ok: true, mode: "validate"}`; exit 0 |
| `node scripts/edf-002-validate.mjs freeze` (validation only; writes nothing) | `freeze requires 12 cases; found 6`; exit 1 |
| `node scripts/edf-002-freeze.mjs verify` | `No frozen-files.json`; exit 1 |
| `node scripts/edf-002-score.mjs` | `{status: "not-evaluable", reason: "No runs directory."}` |
| `node scripts/edf-002-review.mjs validate ST-00{1,3,5,7,9}, ST-011` | all `{ok: true}` (schema validity only; 4 of the 6 verdicts are `revise`) |
| `./praxis registry check` | `registries are current`; exit 0 |
| `./praxis validate` | `validation passed`; exit 0 |

What the tests cover: claim governance, frozen-file hashes, fail-closed behaviour, and one 2-item toy scoring case. **No test exercises an EDF analysis or its quality.** Test 1 asserts `analyzerRuns === 0` (`tests/benchmark-integrity.test.mjs:13`) and test 2 asserts `not-evaluable`. Committing the real EX-EDF-001 run bundles under `runs/` would make the suite fail, so the test suite structurally encodes the absence of the evidence behind the published result.

### 2.5 CI

- `.github/workflows/quality.yml`: `node --test`, `validate-repository`, `edf-benchmark validate`; a .NET/Playwright browser conformance job for the site.
- `.github/workflows/pages.yml`: the same integrity gate before Pages deploy.
- `.github/workflows/praxis-validation.yml`: `./praxis registry check`, `./praxis validate`.
- `.github/workflows/limen-verify.yml`: Limen boundary verification.
- No CI job runs `edf-002-validate.mjs`, so A003 spec validity is covered only via test 4.

### 2.6 Governance (Praxis / ROS)

- Work items, journals and attribution go through ROS/Praxis (`.ros/work/queue.json`, `research/journal/JR-*.md`, `[ros-attributed]` commits). Praxis is a pinned external native binary installed under `~/.echelon` (`praxis` shell wrapper, lines 1-30). Its source is not in this repo.
- Research governance documents: `docs/framework-engineering/claim-and-confidence-policy.md` (claim states, numerical-claim gate, research execution gate), `validation-protocol-v1.2.md`, `framework/REP-SPECIFICATION.md`, and the registries.
- The work item for this improvement program, WI-0013 "EDF-IMPROVEMENT-2026-001", appears in `.ros/work/queue.md:22`.

## 3. Inputs and outputs

| Artifact | Input | Output |
|---|---|---|
| EDF method (docs) | A human/LLM analyst plus case narrative | Free-text Markdown with 15 headings (templates). No schema. |
| EX-EDF-001 | `cases.json`: 6 synthetic cases SYN-001..006 (billing cutover, cold chain, deployment, queue stall, search latency, data import), all software/physical operations | JSON per `output-contract.json` (6 top-level fields); evaluator JSON of 5 dimension scores + flags |
| A003 | `cases/ST-*.case.json`: evidence items with stages S1-S3, candidate hypotheses, edges (relations: contributes-to 30, permits 3, amplifies 2, precedes 2, triggers 1, modifies 1), actions, and scored items | JSON `{caseId, answers:[{itemId, answer, confidence 0-100, evidenceIds}]}` |

## 4. Experiment state

### 4.1 EX-EDF-001: Multi-Model EDF Structural-Value Benchmark (HY-EDF-008)

- Preregistered 2026-09-21 (`research/experiments/EX-EDF-001/preregistration.json`): 3 analyzer families × 6 cases × 3 conditions, +5 margin, bootstrap lower bound > 0, ≥ 2/3 families positive.
- `executor-matrix.json` is still `"status": "unbound"` with all provider and model fields `null`. By the experiment's own rule, "Until then, executions are exploratory and cannot promote HY-EDF-008" (`docs/edf/validation/ex-edf-001-structural-value-benchmark.md:13`).
- Amendment A2 (`amendments/EX-EDF-2026-A002--two-family-availability.md`): no third family "under the current no-API-cost execution constraint" (`:18`). A separate analysis, AN-EDF-001-2F, was registered post-generation and pre-evaluation (`two-family-analysis.json`).
- Result (`two-family-results.json`; `research/analysis/edf-two-family-structural-value-results.md`):
  - A1 OpenAI gpt-5.6-sol 24/24 valid; A2 Anthropic claude-opus-5 24/24; E1 gpt-5.6-terra 36/36; E2 claude-sonnet-5 33/36 (3 Markdown-fence failures).
  - 9/12 complete pairs: **EDF − matched-neutral = +0.56, bootstrap 95% [0.00, +1.39]**; conservative min(E1, E2) +1.11 [0.00, +2.78]; A1 0.00, A2 +1.00. 3 of 4 support criteria not met.
  - Post-hoc all-12: +0.21 [−0.42, +0.83].
  - Ceiling: E1 mean 94.86 with 19/36 scoring 100; E2 mean 99.70 with 31/33 scoring 100.
  - Conclusion: HY-EDF-008 "not-supported-by-this-benchmark".
- **Provenance gap (surprise):** none of the analyzer outputs, evaluator outputs, prompts-as-sent or run manifests are in the repository, in its history, or on any branch. `git log --all` over `EX-EDF-001/runs|evaluations|manual-runs` returns nothing, and `edf-benchmark validate` reports `analyzerRuns: 0`. The published numbers (+0.56, [0, 1.39], etc.) therefore fail the repo's own numerical-claim gate, which requires preserved "raw inputs and outputs ... executable scoring procedure ... calculation provenance" (`claim-and-confidence-policy.md:75-89`) and the research execution gate (`:141-156`). The runs were evidently executed outside the harness (manually, "no-API-cost"), so the aggregation code is not in the repo either. The result is directionally credible and conservative (a null), but it is **not recomputable**, which is the same failure mode FM-001/FM-005 that the repo criticised in its historical R1 cases.

### 4.2 EX-EDF-2026-A003: Construct-Specific Stress Benchmark (HY-EDF-2026-A011..A016)

- Preregistration status: `"preregistered-pending-case-authoring"` (`preregistration.json`). Design: 12 cases (6 constructs × OpenAI/Anthropic author), 2 analyzer families × 2 conditions = 48 runs, deterministic scoring. Support requires mean ≥ +7, CI lower bound > 0, both analyzer families and both author families positive, ≥ 4/6 constructs positive, and no increase in harmful promotion.
- **Cases authored in the repo:** the OpenAI-assigned odd cases only:

| Case | Construct | Domain | Latest Anthropic review (JR-EDF-2026-008, at `48fc7ab`) |
|---|---|---|---|
| ST-001 | negative-knowledge | event-driven billing | **pass** |
| ST-003 | control-points | distributed search | revise (groundTruthSupported) |
| ST-005 | propagation | telemetry ingestion | revise (groundTruthSupported, noLexicalLeakage) |
| ST-007 | evidence-revision | identity platform | **pass** |
| ST-009 | interaction | media processing | revise (groundTruthSupported) |
| ST-011 | protective-controls | billing event pipeline | revise (groundTruthSupported) |

- The first Anthropic review at `fab9973` returned `revise` on all six, with up to six failing checks per case (`research/journal/JR-EDF-2026-006.md:36-45`). Cross-cutting finding: action candidates were "pre-tested" by single evidence items, so "action scoring ... becomes lookup rather than diagnostic reasoning" (`JR-EDF-2026-006.md:60`).
- **Anthropic-assigned cases ST-002, -004, -006, -008, -010, -012 do not exist** in the working tree, in git history, or on any local or remote branch (`git log --all` over `cases/ST-002*` is empty; branches are only `main` and the current work branch). `JR-EDF-2026-004.md:29` records them as "pending Anthropic authoring". However, `JR-EDF-2026-006.md:30` states "The Anthropic authoring phase (ST-002 ... ST-012) was already complete." That statement is **not backed by any artifact in this repository** (inconsistency or lost work). `JR-EDF-2026-005` is also missing; it is said to live on a "parallel OpenAI-review branch" (`JR-EDF-2026-006.md:20`) that is not present.
- **What blocks A003:** (1) the six Anthropic-authored cases are missing; (2) four OpenAI cases still need remediation r2 plus re-review (`JR-EDF-2026-008.md:92`); (3) the freeze gate (`edf-002-freeze.mjs`) needs 12 cases with all checks passing; (4) no analyzer executor bound. No analyzer run has occurred. All six A011-A016 hypotheses are "unsupported / Preregistered ... No analyzer run has occurred".
- Domain coverage: every assignment's `domainHint` is "software/operational system" (`case-authoring-assignments.json`). A003, even when complete, contains **no organizational case**.

### 4.3 Other planned or partial studies

- IFVS-001 independent human study: "Working draft"; zero runs (`docs/edf/validation/independent/IFVS-001.md:3`; `non-human-cumulative-findings.md:11`).
- LRC-001 Chernobyl longitudinal: "Planned. Not yet analyzed." (`LRC-001-chernobyl.md:3`). Evidence packages EP-013A/B/C exist.
- R1 cases (Challenger, Apollo 13, Boeing 737 MAX, Apollo 11, Pixar, Toyota): historical, single research process, raw outputs not preserved; "Very High reproducibility" reclassified as "internal structural agreement only" (`docs/edf/validation/reproducibility-findings.md:24-35`).
- CFCV-001/002/003 (RCA/5-Whys, FMEA, FTA): comparators "narratively reconstructed, not independently executed" (EV-EDF-003; FM-004).
- ABL-002: single-case wording ablation (EV-EDF-002).

## 5. Implementation coverage by EDF component

| Component | Implemented anywhere as code/schema? |
|---|---|
| System Context / Outcome / Manifestations | No |
| Origin Network / Propagation | Only as candidate items in A003 cases (hypotheses and edges with relation labels) |
| Evidence evaluation | A003 answer states supported/contradicted/unknown + evidence citations; A003 evidence stages S1-S3 |
| Control points / ranking | A003 action classes preferred/acceptable/reject/harmful/unknown; no ranking |
| Confidence | A003 integer 0-100 per item (scored with an improper linear rule) |
| Diagnostic Sufficiency / Next Responsible Action | No |
| Complexity levels | No |
| Diagnostic Calibration | No |
| Clarity handoff | No |

## 6. Inconsistencies and integrity defects found

1. **Stale evidence ledger.** `docs/edf/validation/evidence-ledger.md:4` "Updated: 2026-09-21"; `:26` says HY-EDF-008 has Confidence "Low" and "no performance result exists". The registry (canonical, updated 2026-09-22) has HY-EDF-008 at Low-Medium with EV-EDF-009. The ledger also lacks HY-EDF-2026-A011..A016 and EV-EDF-009. `:65-75` still describes EX-EDF-001 as not executed. The integrity script does not check ledger/registry agreement.
2. **Stale repository health.** `research/frontier/repository-health.md:16-26` reports 10 hypotheses, 8 evidence records, 0 controlled benchmarks, and "Integrity tests 3/3". Actual counts are 16, 9, 1 (two-family), and 17 tests.
3. **EV-EDF-008 limitation text** still says "No analyzer run, evaluator score, or EDF performance result exists yet" (evidence registry). This is superseded by EV-EDF-009 but was not annotated.
4. **v0.3 internal contradiction:** the frozen spec requires System Context while the frozen complexity levels require System/System Boundary (see `methodology-inventory.md` §2).
5. **Unbacked journal claim** about completed Anthropic authoring (§4.2).
6. **Non-recomputable headline result** for EX-EDF-001 (§4.1).
7. **Test suite encodes absence of evidence:** `analyzerRuns === 0` is asserted (§2.4).
8. Topology table lists a "Mars Climate Orbiter" validation case (`docs/edf/validation/topology-findings.md:20`) that has no case file in `docs/edf/validation/`. It appears only as deferred insight DI-005. FCRs cite evidence packages that are not in the repo: `FCR-000001.md:13` cites "EP-001" and `FCR-000008.md:13` cites "EP-008". Only EP-013A/B/C exist under `docs/edf/validation/evidence-packages/`. The R1 validation cases therefore have no preserved evidence base.
