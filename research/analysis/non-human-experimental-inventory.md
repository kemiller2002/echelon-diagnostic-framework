---
id: NHEA-INVENTORY-2026-001
title: Master Non-Human Experimental Inventory
status: complete
analysis_date: 2026-07-30
---

# Master Non-Human Experimental Inventory

## Scope and repository state

This inventory covers all discoverable repository experiments that do not directly test human participants. The pre-analysis branch was `main`, tracking `origin/main`; `research/` was already untracked from the preceding frontier analysis. No tracked files were modified before this analysis.

Repository entry findings:

- `README.md` exists and documents only website build/deployment.
- `BOOTSTRAP.md`, canonical ROS/REP governance, lifecycle standards, generated research registries, and identifier standards do not exist.
- The practical research governance is distributed across `docs/edf/constitution.md`, `docs/framework-engineering/validation-protocol.md`, and `docs/framework-engineering/framework-configuration-record.md`.
- The documented README build passes when both documented environment variables are present.
- `npm run build:pages` without those variables fails because `basePath` becomes `/.`.
- The successful build warns that Next.js selects `/Users/kevinmiller/dev/package-lock.json` as workspace root instead of the repository lockfile.

## Inclusion rules

Included: completed case analyses, comparative validations, and ablation tests whose subject is EDF, a method, or generated analyses. Planned studies are retained as planned records but are not counted as experimental evidence.

Excluded from evidence counts: examples, templates, specifications, evidence packages, FCRs, research-question sets, and study protocols unless an execution record exists.

## Located experimental corpus

| ID | Title | Type | Implemented subject | Runs / units | Raw data | Reconstruction | Evidence status |
|---|---|---|---|---:|---|---|---|
| VC-001R1 | Challenger R1 | Internal multi-lens validation | EDF v0.3 under VP v1.1 | 5 lens outputs summarized | No | Partial | Completed, dependent |
| VC-002 | Apollo 13 initial case | Exploratory case | EDF diagnostic interpretation | Missing | No | Low | Completed, weak record |
| VC-002R1 | Apollo 13 R1 | Internal multi-lens validation | EDF v0.3 under VP v1.1 | 5 lens outputs summarized | No | Partial | Completed, dependent |
| VC-003 | Boeing 737 MAX initial case | Exploratory case | EDF diagnostic interpretation | Missing | No | Low | Completed, weak record |
| VC-003R1 | Boeing 737 MAX R1 | Internal multi-lens validation | EDF v0.3 under VP v1.1 | 5 lens outputs summarized | No | Partial | Completed, dependent |
| VC-004 | Apollo 11 | Exploratory success case | EDF applied to successful outcome | Missing | No | Low | Completed, weak record |
| VC-008R1 | Pixar R1 | Internal multi-lens validation | EDF on sustained creative success | 5 lens outputs summarized | No | Partial | Completed, dependent |
| VC-009R1 | Toyota R1 | Internal multi-lens validation | EDF on sustained operational success | 5 lens outputs summarized | No | Partial | Completed, dependent |
| ABL-002 | Propagation Refinement | Single-case ablation | Old vs refined Propagation definition | 1 case | No | Partial | Completed, narrow |
| CFCV-001 | EDF vs RCA/Five Whys | Qualitative method comparison | Chernobyl modern evidence framing | 1 analyst synthesis | No | Partial | Completed, non-controlled |
| CFCV-002 | EDF vs FMEA | Qualitative method comparison | Chernobyl modern evidence framing | 1 analyst synthesis | No | Partial | Completed, non-controlled |
| CFCV-003 | EDF vs FTA | Qualitative method comparison | Chernobyl modern evidence framing | 1 analyst synthesis | No | Partial | Completed, non-controlled |
| IFVS-001 | Independent Framework Validation Study | Planned independent validation | Human analysts applying EDF | 0 | No | Protocol only | Not conducted; human scope excluded |
| LRC-001 | Chernobyl Longitudinal Reference Case | Planned non-human-compatible longitudinal study | EDF across three evidence states | 0 | Packages only | Protocol ready, packages incomplete | Not conducted |

## Common missing metadata

Unless explicitly stated, every completed experiment is missing:

- execution date;
- executing agent identity;
- model, provider, and model version;
- exact prompts and prompt hashes;
- raw per-lens outputs;
- repository commit;
- random seed and stochastic settings;
- sampling procedure and sample-size rationale;
- independent-run count;
- evaluator identity and blinding;
- scoring rubric implementation;
- uncertainty estimates;
- execution logs and failed-run logs.

Missing values are represented as `null` in the machine-readable matrix, not zero.

## Reconstruction mismatches

| ID | Stated experiment | Implemented evidence | Measured outcome | Claimed conclusion | Supported conclusion |
|---|---|---|---|---|---|
| VC-001R1 | Test VP v1.1 reproducibility | One repository-authored synthesis of five named lenses | Reported field-overlap percentages; calculation unavailable | v1.1 improves reproducibility | The summarized lens outputs appear structurally similar; improvement and reproducibility are not independently demonstrated |
| VC-002R1 | Test reproducibility on a dynamic case | Summarized lenses and one synthesized chain | Qualitative High ratings | v1.1 improves comparability; EDF strains on evolving state | Suggestive evidence of representational strain; comparative improvement unmeasured |
| VC-003R1 | Re-test Boeing and evaluate reproducibility | Summaries plus percentages without source outputs | Structural agreement percentages | Very High reproducibility and layered origins | Internal summaries converge; layered organization is a plausible hypothesis |
| VC-008R1 | Test sustained excellence | Prediction lock plus summarized analyses | Agreement percentages without computation | EDF explains sustained success | EDF can be used to describe this selected success case; generality untested |
| VC-009R1 | Test sustained operational excellence | Summarized analyses | Qualitative agreement | Broad adaptive-learning hypothesis | Toyota description is compatible with the hypothesis; mechanism not isolated |
| ABL-002 | Test refined definition | One POS example, before/after conceptual assessment | Ordinal degradation table | Propagation distinctiveness improved | Refinement appears clearer in this authored example; causal improvement and generalization unmeasured |
| CFCV-001/2/3 | Compare EDF to established methods | Narrative reconstructions, not executions by independent practitioners | Capability judgments | Complementarity and differentiated strengths | Reasonable theory comparison; no performance effect is measured |

## Experimental quality matrix

Scale: High/Medium/Low/None. “Independence” refers to independence from the EDF research/evaluation process.

| Experiment | Hypothesis clarity | Controls | Sample adequacy | Metric validity | Raw data | Reproducibility | Independence | Overall strength |
|---|---|---|---|---|---|---|---|---|
| VC-001R1 | High | Low | Low | Low | None | Low | Low | Suggestive |
| VC-002 | Low | None | Low | Low | None | None | Low | Weak |
| VC-002R1 | Medium | Low | Low | Low | None | Low | Low | Suggestive |
| VC-003 | Low | None | Low | Low | None | None | Low | Weak |
| VC-003R1 | Medium | Low | Low | Low | None | Low | Low | Suggestive |
| VC-004 | Low | None | Low | Low | None | None | Low | Weak |
| VC-008R1 | High | Low | Low | Low | None | Low | Low | Suggestive |
| VC-009R1 | Medium | Low | Low | Low | None | Low | Low | Suggestive |
| ABL-002 | High | Medium | Low | Medium-low | None | Low | Low | Suggestive within one example |
| CFCV-001 | Medium | None | Low | Low | None | Low | Low | Theory-level |
| CFCV-002 | Medium | None | Low | Low | None | Low | Low | Theory-level |
| CFCV-003 | High | None | Low | Low | None | Low | Low | Theory-level |

## Reconstruction status

The corpus is sufficient for qualitative cumulative analysis but not formal meta-analysis. No completed experiment exposes the raw unit-level data needed to verify reported percentages, calculate variance, or reproduce an effect.

