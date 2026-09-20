# Claim and Confidence Policy

Status: Current research governance
Version: 1.0
Effective: 2026-09-20

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
