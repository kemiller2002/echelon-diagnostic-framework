# Claim and Confidence Policy

Status: Current research governance
Version: 1.1
Effective: 2026-09-26

## Purpose

This policy prevents the EDF research corpus from presenting a stronger claim than its evidence can support.

It governs mutable research summaries, public website claims, future Validation Protocols, Research Execution Packages, and registries. Frozen historical artifacts remain unchanged and are interpreted through the current registries.

## Evidence language

EDF research must keep these concepts separate:

1. **Structural conformity** - an output contains the requested EDF fields.
2. **Semantic agreement** - independent outputs identify materially similar meanings.
3. **Factual correctness** - claims are supported by the declared evidence package.
4. **Reproducibility** - an independent execution using a frozen configuration produces materially comparable results.
5. **Incremental value** - EDF performs better than a credible control under a preregistered measure.
6. **Utility** - EDF improves a practical downstream outcome such as intervention quality, speed, cost, or decision quality.

Evidence for an earlier item does not establish a later item.

## Terminology Across Disciplines

"Reproducibility" and "replicability" do not have one universal scientific definition.

- The U.S. National Academies' 2019 report uses **reproducibility** for recomputing results with the same data, code, methods, and analysis conditions, and **replicability** for a new study addressing the same question with new data.
- NIST metrology terminology distinguishes **repeatability** under the same measurement conditions from **reproducibility** under changed conditions.

Sources:

- [National Academies, Reproducibility and Replicability in Science](https://nap.nationalacademies.org/catalog/25303/reproducibility-and-replicability-in-science)
- [NIST TN 1297 terminology](https://www.nist.gov/pml/nist-technical-note-1297/nist-tn-1297-appendix-d1-terminology)

EDF therefore uses qualified operational terms rather than assuming its vocabulary is universal:

- **execution reconstructability** - the preserved artifacts are sufficient to rerun the declared computational procedure;
- **within-condition repeatability** - materially comparable results under explicitly same conditions;
- **cross-executor reproducibility** - materially comparable results under declared changed executor conditions, such as another model family or analyst;
- **cross-case replication/generalization** - a result persists on newly sampled cases or evidence packages relevant to the same research question.

Every quantitative claim must state which construct and changed/held-constant conditions it measures.

## Claim states

Current claims use one of these states:

- **internal-demonstration** - observed inside the repository's own authored or generated artifacts, without independent replication.
- **suggestive** - evidence points in a direction, but major alternative explanations remain.
- **unsupported** - the claim is plausible or planned but has no completed evidence capable of testing it.
- **contradicted** - available evidence materially conflicts with the claim.
- **falsified** - a preregistered test met its falsification rule.
- **superseded** - replaced by a newer claim definition or measurement.
- **planned** - a research design exists but execution has not occurred.

"Supported" without a boundary is prohibited for current performance claims.

## Confidence

Confidence is a qualitative judgment about the claim given the available evidence. It is not a probability unless a calibrated probabilistic method has been defined and validated.

Allowed labels:

- Low
- Low-Medium
- Medium
- Medium-High
- High

A high confidence label can apply to a narrow negative or methodological claim, such as "raw run bundles are absent," while a positive performance claim may remain unsupported.

## Numerical claims

Exact percentages, effect sizes, confidence intervals, p-values, or probabilities may be published only when all of the following are preserved:

- raw inputs and outputs;
- denominator and scoring unit;
- executable scoring procedure;
- evaluator identity/version;
- frozen rubric;
- model/provider/version;
- repository commit;
- excluded and failed runs;
- calculation provenance.

Historical percentages that fail this gate may be retained as historical observations but must be labeled non-recomputable.

## Independence

Named analytical lenses produced by one model, agent, or research process are not independent replications.

Different cases authored by the same research process are not independent evidence families merely because their case IDs differ.

Independent replication requires a materially separate execution source under a frozen configuration. The source may be a human analyst or a separate model family, depending on the research question.

### Actor identity is provenance, not evidence (v1.1)

Each case, review, analyzer run, evaluation, finding, hypothesis update, or conclusion may carry the identity of its author, reviewer, executor, or evaluator. That identity is self-reported provenance, in the shape of the Praxis actor and provenance record. It is not authentication. It is not evidence either. The identity may be an OpenAI, Anthropic, or Google model, a human, or unknown.

Identity MAY be used only for:

- **Independence and separation checks.** Examples: requiring an opposite-family reviewer, or refusing to count two runs from one execution source as independent replications.
- **Stratified reporting.** Results may be reported per author family, analyzer slot, or executor, as descriptive strata.

Identity MUST NOT:

- change evidence weight, scores, claim state, or confidence labels;
- act as a tie-breaker, a prior, or a quality proxy;
- promote or demote a claim because of who produced it.

A stratified result is still judged by its own evidence and preregistered rules. Rules for recording identity:

- Record identity only as declared. When it is not known, record the literal `unknown`. Never infer it from Git authorship, prose, `author_agent`-style fields, or telemetry.
- Recording identity does not relax the requirements of the numerical-claims gate or the research-execution gate. Model/provider/version and evaluator identity are preserved there so that results can be recomputed and audited, not so that they can be weighted.
- Historical artifacts are not backfilled. The *Historical artifacts* rules below still apply, and identity absent from a past artifact stays absent.
- Future artifacts may carry identity in an optional provenance sidecar. See [provenance-sidecars.md](provenance-sidecars.md) and `DF-EDF-2026-A001`. A sidecar lives outside every frozen or hashed set, and outside every analyzer-, reviewer-, and scorer-visible input.

This follows the Praxis provenance contract: `RQ-ROS-2026-A010` (provenance is not attestation or evidence weight) and `RQ-ROS-2026-A013`, `RQ-ROS-2026-A014`, and `RQ-ROS-2026-A015` (interchange record, execution propagation, no silent stripping). Tests enforce it: scoring is identity-invariant (`tests/identity-neutral-scoring.test.mjs`), and sidecars are validated and isolated (`tests/provenance-sidecar.test.mjs`).

## Public claim gate

Every material public claim about EDF performance must map to a current hypothesis or evidence record.

Public wording may be weaker than the registry state. It may not be stronger.

The public site must not describe these as established until controlled evidence exists:

- independent reproducibility;
- causal improvement from System Context;
- improved control-point quality;
- evidence-sensitive updating;
- superiority to natural analysis or matched neutral structure;
- quantitative calibration;
- routine-use speed or learnability.

## Diagnostic sufficiency

Diagnostic Sufficiency means the current model is sufficient for a specified next action under a specified consequence and reversibility profile.

It is not a declaration that the diagnosis is true or complete.

Higher-consequence or irreversible actions require stronger evidence than low-cost reversible tests.

## Control-point ranking

Current control-point dimensions are heuristics, not a validated scoring system.

Influence, controllability, cost, reversibility, timing, and confidence may be considered, but no fixed weights or numerical precision are implied until validated.

## Historical artifacts

Historical Validation Cases, FCRs, and reports are not rewritten to make past work look stronger or cleaner.

When later research changes interpretation:

1. preserve the historical artifact;
2. add the contradiction or supersession to the current registry;
3. update mutable summary documents;
4. update public claims;
5. create a new REP when the theory impact is material.

## Research execution gate

A new computational experiment is not confirmatory unless it preserves:

- immutable run manifest;
- every attempted run, including failures;
- exact prompt and evidence hashes;
- exact model/provider/version;
- randomization seed when used;
- token, latency, retry, and cost data when available;
- raw model output;
- raw evaluator output;
- executable scoring code;
- preregistered success, falsification, and stop rules.

Exploratory runs may omit some of these controls, but they must be labeled exploratory and cannot promote a performance claim.

## Canonical current state

The machine-readable registries under `research/registries/` are the current source of truth for claim state and evidence interpretation.

The Evidence Ledger is a human-readable projection of those registries.

## Version history

- **1.1 (2026-09-26):** Added *Actor identity is provenance, not evidence* under Independence (`DF-EDF-2026-A001`). No claim state, confidence label, or historical artifact changed.
- **1.0 (2026-09-20):** Initial policy.
