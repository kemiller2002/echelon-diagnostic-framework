# Stakeholder Bias, Leadership Narratives and Organizational Silence

Mission EDF-IMPROVEMENT-2026-001 · Baseline EDF v0.3 (`41fea6f`) · 2026-11-16
Inputs: `../scientific-foundations/literature/lit-dynamics.md`, `lit-validity.md`, `lit-ai-diagnosis.md`; `../baseline/*.md`; `../benchmarks/benchmark-specification.md`. Candidate mechanisms (OPP-xx) are defined in `../scientific-foundations/methodological-opportunities.md`. Source ids refer to `../evidence/source-register.csv`. "[SR]" marks a detail that rests on search-result-only sources.

No experimental results are reported here. See experiments/ and strategy/ for measured results (EX-EDF-2026-OD1).

## Core premise

In organizational diagnosis, the evidence comes from the system under diagnosis. The mechanisms that create the problem are the same ones that shape what the diagnostician is told: filtering, silence, status and stake. Leadership accounts are therefore not neutral observations. They are outputs of the system, and often its most filtered outputs (lit-dynamics §0).

A method that ignores this will reproduce the leadership narrative. A method that over-corrects will invent hidden problems. Both failure directions must be measured.

## How OD1 tests this family

The OD1 benchmark is specified in `../benchmarks/benchmark-specification.md`.

- **Evidence labels.** Every evidence item carries a `sourceType`, such as `leadership-account`, `staff-account`, `anonymous-staff-survey`, `system-record` or `independent-observation`.
- **No authoritative group.** Authoring rule 3 says no stakeholder group is automatically authoritative. Leadership, staff and system records can each be right or wrong.
- **Presenting request.** The presenting request "may be wrong" (rule 7).
- **Relevant classes**, as named by the dev cases:
  - D: conflicting stakeholder explanations (a CEO blames sales; win rate collapses).
  - I: green dashboards while customer dates are missed.
  - K: a reported crisis that may not exist, or may be an intentional tradeoff.
- **Relevant tags.** `leadership-narrative` appears in every dev case. `staff-narrative`, `organizational-silence` and `hidden-problem` also apply.
- **Secondary metrics.** Leadership-narrative adoption (items tagged `leadership-narrative` with expected ≠ supported but answered supported), false-diagnosis rate, missed-problem rate, tag-specific accuracy, and evidence F1 against `requiredEvidence`.

**Two structural limits.**

1. The packets already contain the frontline or record evidence. OD1 can test whether an analyzer *weighs* such evidence correctly. It cannot test whether a method makes a diagnostician *collect* it.
2. Candidate items test recognition. Missed-problem detection is therefore upper-bounded, and the open-generation arm (OD1-G) partly addresses this.

---

## Q1. Can EDF detect problems leadership does not recognize?

**What EDF v0.3 does.**

- Nothing in the nine steps asks whose account the evidence comes from, or whether leadership's view could be part of the problem (`../baseline/methodology-inventory.md` §1, "Organizational fitness").
- Manifestations are "what can be seen, measured, reported" (§6). An interested party's report counts as observable.
- The System Context boundary has no rule for who sets it. Drawing it around "the team" rather than "leadership" changes which origins can be found (§4; `../baseline/current-limitations.md` rank 21).
- The origin taxonomy has no *informational* category in v0.3. It also has no category for informal power or for information that failed to travel (`../baseline/diagnostic-taxonomy.md` §1.3, §2.2).

**Verdict:** EDF can detect such problems only if the analyst happens to seek out non-leadership evidence. Nothing in the method prompts it.

**What the literature says.**

- Leaders get filtered information.
  - The MUM effect: bad news is passed on less readily (SRC-CDE-058, experimental, [SR]).
  - Silence is widespread and rooted in managers' fear of negative feedback (SRC-CDE-053, SRC-CDE-054, [SR]).
  - Managers discount critical upward feedback (SRC-CDE-059, [SR]).
- Frontline staff absorb most process failures through workarounds. In one observational study, 93% of problems were handled this way and only 7% met lenient criteria for second-order problem solving. This creates an "illusory equilibrium" in which managers do not see failures (SRC-CDE-057, fetched; the strongest design in this literature).
- Higher-status members rate their organization more favourably, and perceptions weakly track objective data (SRC-CDE-009, fetched; SRC-CDE-010). The evidence is thin and contested.
- Managers may attribute recurring problems to worker motivation rather than system structure (SRC-BH-022). This detail was not verified from the paper text.

**Proposed mechanism.**

- OPP-02: commissioning-stake declaration and premise check.
- OPP-18: work-as-done sampling plan and workaround/silence probes.
- OPP-08: for each control point, would the controller currently learn the problem exists?
- OPP-06: an *informational* origin prompt.

**How OD1 tests it.**

- Classes D and I: items where the leadership narrative is wrong and the true origin appears in staff, record or observation evidence.
- Metrics: missed-problem rate on `hidden-problem` items, and leadership-narrative adoption.
- Guardrail: class K and any items where the leadership account is correct, measured by the false-diagnosis rate. This is lit hypothesis H-E8.

---

## Q2. Can EDF distinguish perceptions from verified conditions?

**What EDF v0.3 does.**

- Step 6 says "Separate direct observation from inference" (`methodology-inventory.md` §9). That gives two implicit evidence types.
- There is no source type, no credibility or stake field, no corroboration count, and no contemporaneous-versus-retrospective distinction (`diagnostic-taxonomy.md` §3.2).
- Confidence is a three-level label with no rule for deriving it (`methodology-inventory.md` §12).
- The A003 experiment had supported / contradicted / unknown states and a negative-knowledge construct, but these are not in the spec (`diagnostic-taxonomy.md` §3.1).

**What the literature says.**

- Common-method variance inflates relationships, and procedural separation of sources and methods is the first remedy (SRC-CDE-006).
- Competent key informants frequently disagree, and their accuracy varies with construct and informant (SRC-CDE-007, SRC-CDE-008, [SR]).
- Averaged perceptions can look accurate even when most individuals are inaccurate (SRC-CDE-009).
- Measurement source can flip a causal conclusion (SRC-CDE-046 vs SRC-CDE-047, [SR]).
- Social-desirability bias has no single remedy, so detection and prevention should be combined (SRC-CDE-011, [SR]).

**Proposed mechanism.**

- OPP-01: provenance fields plus a triangulation rule. A claim cannot exceed *Moderate* confidence unless two or more independence groups that differ in method or source role support it. Interested post-outcome accounts count as one group.
- OPP-07: label each edge causal or association-only.
- Treat systematic disagreement between levels as a *manifestation*, not noise (lit-validity C3). An example is leaders being more favourable than the frontline.

**How OD1 tests it.**

- The `sourceType` labels let an analyzer separate perception from record.
- Items expected `unknown` or `association-only` (required in every case by authoring rule 4) test whether perception is mistaken for verification.
- Metrics: unsupported certainty, causal overclaim rate, and evidence F1 (whether cited evidence matches `requiredEvidence`).
- Rule 3 (records can be gamed) keeps the test from being solvable by trusting records over people.

---

## Q3. Does EDF over-rely on management interviews?

**What EDF v0.3 does.** The spec does not name evidence sources, so it does not formally rely on any one. In practice, the validation corpus was built from public narratives and accident reports in which the record already exists (`current-limitations.md` rank 2). There is no evidence-sampling plan, so whatever the commissioning party provides becomes the evidence base by default. The audit lists "no stakeholder-credibility or source-interest handling" as an S1 limitation (rank 5).

**What the literature says.**

- A diagnosis built from executive interviews is single-source, single-method and interested. It is the textbook case for common-method and status bias (lit-validity §1.3).
- The self-interest check ("is there any reason to suspect errors driven by the self-interest of the recommending team?") and "where did the numbers come from" are practitioner guidance (SRC-CDE-024, fetched; observational support only).
- RCA investigations are prone to "political hijack" (SRC-CDE-036, [SR]).
- LLM analysts add their own amplifier. They are sycophantic toward the requester's view and accept false premises (SRC-FGI-034, SRC-FGI-054, SRC-FGI-055, [SR]). An LLM-run EDF that is given a leadership request plus leadership interviews has two reasons to agree.

**Proposed mechanism.**

- OPP-18 (sampling plan stated before Step 4, with coverage gaps recorded as Unknowns).
- OPP-01 (independence rule).
- OPP-02 (premise check and stake declaration).
- OPP-12 (evidence requests ranked by expected decision impact, so the analyst asks for the frontline or record evidence that would discriminate).

**How OD1 tests it.**

- Class D packets present a coherent leadership story alongside discriminating staff and record items.
- The evidence-request component rewards choosing hidden discriminating requests (`discriminatingRequests`) and penalises low-value ones.
- Metrics: leadership-narrative adoption, evidence-request value, and `staff-narrative` tag accuracy. Staff narratives can also be wrong, which guards against simple inversion.

---

## Q4. Can EDF detect organizational silence?

**What EDF v0.3 does.**

- Unknowns is a free-text list (`methodology-inventory.md` §2).
- There is no missingness classification and no rule against reading an absence of reports as an absence of problems (`diagnostic-taxonomy.md` §3.2).
- There is no propagation type for blocked, filtered or distorted information (§2.2).
- The only nearby construct is A003's experimental "negative knowledge" module, which is outside the spec.

**What the literature says.**

- Silence is collective and widespread (SRC-CDE-053, SRC-CDE-054, [SR]).
- Implicit voice theories predict silence even for pro-organizational suggestions (SRC-CDE-060, [SR]).
- Better-led units *report more* errors (SRC-CDE-041, small n). Report counts therefore measure reporting climate.
- Evidence missing not at random biases inference (SRC-CDE-012).
- Psychological safety associates with voice, mostly cross-sectionally (SRC-CDE-055, SRC-CDE-056). It is most useful as an evidence-quality moderator.
- Surveillance chills expression (SRC-FGI-074, non-workplace) and induces concealment (SRC-FGI-067, [SR]). Monitoring-heavy units may look quieter for that reason.
- Do not label silence "groupthink". The full model is weakly supported (SRC-CDE-068).

**Proposed mechanism.**

- OPP-04: tag unknowns as not collected, unavailable or plausibly suppressed. Allow "no reports" as evidence only alongside an independent reporting-climate indicator.
- OPP-18: silence probes such as "What would people not say to their manager here?"
- A dissent record that flags "no dissent found" on a complex case as a warning (lit-dynamics E10).
- OPP-08: flag any control point whose effect depends on a channel identified as silent.

**How OD1 tests it.**

- Tag `organizational-silence` (dev cases E, F, G, H, I and K; heaviest in I).
- Tag `hidden-problem`.
- Class K is the guardrail: some signals there are weak or anecdotal, and over-reading silence would produce a false diagnosis.
- Metrics: tag-specific accuracy, missed-problem rate, false-diagnosis rate, and the SUFF item (whether evidence warrants a substantive intervention).

---

## Design cautions

1. **Asymmetric harm.** Wrongly overriding a correct leadership account can damage trust and the sponsor relationship. Wrongly adopting a false one can punish the wrong unit. OD1 scores harmful actions with a −15 penalty and reports leadership-narrative adoption separately, so both directions are visible.
2. **Confidentiality is a precondition, not a courtesy.** Frontline evidence is only obtainable if sources are protected (SRC-CDE-057, SRC-CDE-053). EDF needs a confidentiality and sensitivity field (`diagnostic-taxonomy.md` §3.2), plus a rule that minimum group sizes protect attributed testimony.
3. **LLM analyzers inherit bias through the packet.** They do not experience status or silence themselves (lit-dynamics §5). Benchmark rules therefore test *correction* for biased inputs, not analyzer bias.
4. **Synthetic-case limit.** OD1 cases are authored and analyzed by Claude-family agents (spec §7). Real-organization validity of any stakeholder-bias rule needs field evaluation (`../intervention-effectiveness/evaluation-design.md`).
