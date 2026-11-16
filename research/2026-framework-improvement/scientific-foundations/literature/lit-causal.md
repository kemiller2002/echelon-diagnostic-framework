# Workstream D — Causal Inference

Status: staging draft (research input, not a framework change)
Retrieved: 2026-11-16
Scope: what formal causal inference, case-based causal methods (process tracing), systems-safety accident analysis, system dynamics and RCA critiques imply for EDF's Origin Network (Step 4), Propagation (Step 5), Evidence (Step 6) and Control Points (Steps 7–8).
Source IDs refer to `sources-CDE.csv`. Verification: **fetched**, **search-result**, **from-memory-unverified** (flagged).

---

## 0. The causal claims EDF actually makes

An EDF analysis makes two different kinds of causal claim, and the spec does not distinguish them:

1. **Causes-of-effects (retrospective, single case):** "These origins, through these paths, produced this outcome." (Steps 4–5.)
2. **Effects-of-causes (prospective, interventional):** "Intervening at control point K will change future outcomes by roughly this much." (Steps 7–8: *Influence*, *Controllability*.)

**Holland (1986)** [SRC-CDE-028, search-result] distinguishes measuring the *effects of causes* (what statistics/experiments can address) from identifying the *causes of effects*. EDF's Step 8 "Influence" is an effects-of-causes claim, but it is currently justified with the same retrospective single-case evidence used for Step 4. This is the largest unacknowledged inferential gap in the spec.

---

## 1. Key findings

### 1.1 Structural causal models and counterfactuals

- **Pearl (2009, *Statistics Surveys*)** [SRC-CDE-027, search-result]: the structural causal model (SCM) unifies graphs, potential outcomes and structural equations, and separates three query types — **effects of interventions**, **counterfactuals**, and **direct vs indirect effects**. Associational evidence alone cannot answer interventional or counterfactual queries without causal assumptions encoded in a graph.
- **Spirtes & Zhang (2016)** [SRC-CDE-032, search-result]: automated causal discovery from observational data generally yields a **set** of models consistent with the data (an equivalence class), not a single model; unmeasured confounders and time-varying structure are practical obstacles.

Status: formal theory (mathematically established); applicability to qualitative organizational cases is by analogy.

*EDF reading:* An Origin Network is an informal causal graph. Making edges explicit (cause → effect, mechanism, what the counterfactual would be, what confounders are plausible) is cheap and enables checking. Spirtes & Zhang imply EDF should expect *multiple* networks consistent with the evidence and report them, rather than a single network.

### 1.2 Process tracing and evidentiary tests

- **Collier (2011)** [SRC-CDE-029, search-result], building on **Van Evera (1997)** [SRC-CDE-072, search-result — mentioned in secondary sources only]: four tests by necessity/sufficiency of the evidence — *straw-in-the-wind* (weak), *hoop* (failing eliminates; passing only modestly supports), *smoking-gun* (passing strongly supports; failing only weakens), *doubly decisive*.
- **Bennett & Checkel (eds., 2014/2015)** [SRC-CDE-030, search-result]: process tracing as analysis of "processes, sequences and conjunctures of events within a case"; ten best practices, including considering alternative explanations and being even-handed; technical appendix formalizing the tests in Bayesian terms.
- **Fairfield & Charman (2017)** [SRC-CDE-031, search-result]: explicit Bayesian process tracing is feasible and helps "pinpoint the locus of contention when scholars disagree", but probabilities "cannot be unambiguously specified" in qualitative work.

Status: methodological (well-developed logic; little experimental evidence that analysts using the tests reach more accurate conclusions).

*EDF reading:* EDF Step 6 asks to "evaluate the quality of evidence" but gives no grammar for **diagnosticity** — how much a piece of evidence discriminates between rival origin networks. The hoop/smoking-gun vocabulary is a lightweight, teachable grammar for exactly that, and directly supports the differential requirement proposed in Workstream C (C5).

### 1.3 "No root cause": convergent critique from safety science and healthcare

- **Cook (1998), "How Complex Systems Fail"** [SRC-CDE-040, fetched]: #3 catastrophe requires multiple failures; #5 complex systems run in degraded mode; #7 "Post-accident attribution to a 'root cause' is fundamentally wrong"; #8 hindsight "remains the primary obstacle to accident investigation"; #15 views of cause limit the effectiveness of defenses.
- **Peerally, Carr, Waring & Dixon-Woods (2017)** [SRC-CDE-036, search-result; PSNet summary fetched]: RCA in healthcare is applied without attention to what makes it work; challenges include inappropriate focus on **single-point causation**, poor analysis quality, insufficient feedback/closure, and **political hijack** of investigations.
- **Card (2017), "The problem with '5 whys'"** [SRC-CDE-038, search-result]: 5-whys forces a single causal chain, assumes the most distal link is the best intervention point, and misses upstream causes; not suitable for healthcare RCA.
- **Vincent, Carthey, Macrae & Amalberti (2017)** [SRC-CDE-037, fetched]: "the ideal of a single root cause is thoroughly misleading"; analyse contributory factors at different points in time; a review of 302 analyses found the commonest recommendations were **more training and reinforced policies**, with similar events recurring; recommend fewer, deeper investigations and analysis of what went well.
- **Card, Ward & Clarkson (2012)** [SRC-CDE-039, search-result]: healthcare translates risk analysis into risk-control actions poorly — finding causes does not ensure effective controls.
- **Lundberg, Rollenhagen & Hollnagel (2009)** [SRC-CDE-043, search-result]: in eight investigation manuals, **What-You-Look-For-Is-What-You-Find** — the underlying accident model determines the causes found. Their 2010 follow-up [SRC-CDE-044, search-result]: "what you find is not always what you fix" — recommendations are shaped by factors other than the identified causes.

Status: the single-root-cause critique is a strong expert consensus with supporting empirical evidence on poor RCA quality and weak recommendations (Vincent's 302-analysis review; Lundberg's manual analysis). There is little controlled evidence that multi-cause methods produce better *outcomes*.

*EDF reading:* EDF already rejects single-root-cause modelling ("Complex outcomes should usually be modeled as an origin network"). This is aligned with the literature and is a genuine strength. What EDF lacks: (a) protection against WYLFIWYF — its six origin categories are themselves a search model; (b) a **strength hierarchy for control points** that penalises "training/reminder/policy" controls; (c) a check that the recommended control points actually follow from the identified network rather than from what is politically or administratively easy.

### 1.4 STAMP/CAST: control structures rather than chains

- **Leveson (2011), *Engineering a Safer World*** [SRC-CDE-042, search-result]: STAMP treats accidents as resulting from inadequate enforcement of safety constraints by a hierarchical **control structure**; CAST analyses each controller's responsibilities, control actions, **feedback**, and **process model** (what the controller believed), and treats hindsight bias and blame as investigation traps.
- Reliability evidence: STAMP-based analyses showed higher inter-analyst agreement than AcciMap in **Jun et al. (2019)** [SRC-CDE-051] and **Hulme et al. (2024)** [SRC-CDE-050] (both search-result); **Salmon, Cornelissen & Trotter (2012)** [SRC-CDE-049, search-result] found AcciMap, HFACS and STAMP produce materially different analyses of the same event.

Status: STAMP is a well-specified theory; reliability advantage over AcciMap is **empirically supported in two studies**; criterion validity of all systemic methods is weak (SRC-CDE-052).

*EDF reading:* EDF Control Points are close to STAMP controllers but lack the four fields that make CAST analysable: *who controls*, *what control action*, *what feedback the controller receives*, *what the controller believed (process model)*. Missing or distorted feedback to controllers is precisely the organizational-silence mechanism in Workstream E — the two literatures meet here.

### 1.5 Hindsight and local rationality

- **Fischhoff (1975)** [SRC-CDE-025, search-result]: outcome knowledge raises judged prior probability of the outcome and changes perceived relevance of data, largely without awareness ("creeping determinism"). Replication record is **mixed** (FORRT atlas lists 1 successful, 2 failed replications, including a 2025 preregistered failure) — the general hindsight-bias phenomenon is well documented, but the specific 1975 effect sizes are uncertain.
- **Woods, Dekker, Cook, Johannesen & Sarter (2010), *Behind Human Error*** [SRC-CDE-045, search-result]: "human error" is an attribution produced by a judgment process influenced by hindsight; investigation should recover the "second story" of how actions made sense at the time.

*EDF reading:* EDF's Outcome step instructs factual framing (good), but Steps 4–5 are written from the outcome backwards. Human and organizational origins should carry a **local-rationality record**: what was known, goals and pressures at the time, and what signals were actually available pre-outcome.

### 1.6 Feedback, delays and policy resistance (system dynamics)

- **Sterman (1989)** [SRC-CDE-034, search-result]: in the Beer Distribution Game, subjects' behaviour fitted an anchoring-and-adjustment rule and showed systematic **misperceptions of feedback** — insensitivity to the feedback from their own decisions to the environment, especially with delays.
- **Sterman (2001)** [SRC-CDE-035, search-result]: **policy resistance** — "the tendency for well-intentioned interventions to be defeated by the response of the system to the intervention itself".
- **Repenning & Sterman (2001)** [SRC-CDE-065, fetched — abstract only]: improvement programmes fail mostly because of how the new effort interacts with the surrounding physical, economic, social and psychological structures, not because of the technique.
- **Rasmussen (1997)** [SRC-CDE-048, search-result]: behaviour migrates under efficiency and least-effort gradients toward the boundary of acceptable performance; risk management is a control problem across levels of society.

Status: misperception of feedback is **empirically supported in experiments**; policy resistance and drift are **theory with case support**.

*EDF reading:* EDF Propagation is described as paths from origin to manifestation — implicitly acyclic. Organizational problems frequently involve **reinforcing loops** (firefighting erodes capability, which creates more fires) and **balancing loops** that will counteract a control-point intervention. Step 8 ranks control points by influence without asking how the system will respond.

### 1.7 Fault trees and Bayesian networks

- **Bobbio, Portinale, Minichino & Ciancamerla (2001)** [SRC-CDE-033, search-result]: any fault tree maps to a Bayesian network; BNs relax fault-tree assumptions (e.g., deterministic gates, independence) and support **diagnostic** (backward) inference.

*EDF reading:* For EDF-2 with quantitative data, an Origin Network can be formalised as a BN to compute which unobserved origin is most probable given manifestations. This is an optional extension, not a core requirement; the qualitative probability-specification caveat (SRC-CDE-031) applies.

### 1.8 Quasi-experiments: the data source can flip the conclusion

- **Card & Krueger (1994)** [SRC-CDE-046, search-result]: difference-in-differences natural experiment (NJ vs PA fast food after a minimum-wage rise) found no employment loss (relative +2.5 FTE/store).
- **Neumark & Wascher (NBER w5224)** [SRC-CDE-047, search-result]: payroll records for a different sample gave the opposite sign (≈ −4.6% relative employment); they argued the survey data were much noisier. Card & Krueger's 2000 reply with BLS data again found no effect.
- **Campbell (1979)** [SRC-CDE-063, search-result]: argues for quasi-experimental evaluation of planned change.

Status: empirical illustration that even a well-identified design is sensitive to measurement source.

*EDF reading:* (1) Control-point "Influence" claims should, where possible, be converted into a **staged or comparative rollout** (a DiD-style next responsible action) rather than asserted. (2) Measurement source (survey vs records) is a first-class determinant of causal conclusions — reinforcing provenance fields (Workstream C, C1).

---

## 2. Empirically supported vs theoretical — summary

| Claim | Status | Sources |
|---|---|---|
| Observational evidence yields a set of compatible causal models; latent confounding is common | Formal theory | SRC-CDE-027, -032 |
| Evidence diagnosticity can be graded (hoop/smoking gun; Bayesian) | Methodological logic; little outcome evidence | SRC-CDE-029, -030, -031 |
| Single-root-cause framing is misleading; RCA quality is often poor; recommendations skew to weak controls | Expert consensus + empirical reviews | SRC-CDE-036, -037, -038, -040 |
| Investigation model determines what is found; recommendations not fully driven by findings | Empirical (document analysis) | SRC-CDE-043, -044 |
| Control-structure methods (STAMP) more reliable between analysts than AcciMap | Empirical (2 studies) | SRC-CDE-050, -051 |
| Systemic methods have weak criterion validity | Empirical (1 study) | SRC-CDE-052 |
| People misperceive feedback and delays | Empirical (experiments) | SRC-CDE-034 |
| Policy resistance / drift | Theory + cases | SRC-CDE-035, -048 |
| Hindsight bias distorts post-hoc causal judgment | Empirical phenomenon; specific classic replications mixed | SRC-CDE-025, -045 |

---

## 3. Concrete implications for EDF

**D1. Typed causal edges (Step 4/5; Origin Network + Propagation outputs).**
Each edge records: `from`, `to`, `mechanism` (one sentence), `counterfactual` ("absent X, Y would/would not have…"), `edge_type` (enabling / triggering / amplifying / failed-defence), `evidence_ids`, `test_type` (straw-in-wind / hoop / smoking-gun / doubly-decisive), and `plausible_confounders`. Rationale: SCM explicitness (SRC-CDE-027); process-tracing tests (SRC-CDE-029); makes edge-level reliability and accuracy scorable (H-D1).

**D2. Rival networks as an output (Step 4 + Unknowns).**
Report the set of networks still compatible with the evidence (usually 2–3 for EDF-1/2), with the evidence that would discriminate them. Rationale: equivalence classes (SRC-CDE-032); premature closure (Workstream C).

**D3. Split Step 8 "Influence" into retrospective and interventional evidence.**
`influence_basis` ∈ {prior intervention evidence in this system, comparable-system evidence, model-based reasoning only}. Control points whose influence rests on reasoning only cannot be ranked first without flagging; the Next Responsible Action should preferentially be a **staged/comparative intervention** that generates effects-of-causes evidence. Rationale: SRC-CDE-028, -046, -047.

**D4. Control-structure fields for each control point (Step 7).**
`controller`, `control_action`, `feedback_channel` (what the controller actually sees and with what delay), `process_model` (what the controller believed at the time), `constraint_enforced`. Flag control points where feedback is missing, delayed, or filtered. Rationale: STAMP/CAST (SRC-CDE-042); reliability advantage (SRC-CDE-050, -051); bridges to organizational silence (Workstream E).

**D5. Loop check (Step 5 and Step 8).**
Propagation may contain cycles; analysts must ask "which feedback loop maintains this outcome?" and, for each top control point, "how will the system respond to this intervention (compensating behaviour, displacement, metric gaming)?" Rationale: SRC-CDE-034, -035, -048, -065.

**D6. Control-strength hierarchy (Step 8, as a ranking modifier).**
Classify each control as structural/design change, feedback/information change, incentive change, procedure/policy, training/reminder. Training/policy-only controls are penalised unless the network shows a genuine knowledge gap. Rationale: Vincent et al.'s finding that training/policy dominate and events recur (SRC-CDE-037); RCA→control gap (SRC-CDE-039).

**D7. WYLFIWYF check (Step 4).**
Before closing Step 4, list the origin categories *not* examined and why. Add *informational* (who knew what, when) and *incentive* as explicit origin prompts — they are where organizational problems hide (Workstream E). Rationale: SRC-CDE-043.

**D8. Local-rationality record for human/organizational origins (Step 4).**
For each human/organizational origin: what was known, goals/pressures, signals available pre-outcome. Prohibit "should have known" language without a pre-outcome evidence citation. Rationale: SRC-CDE-025, -040, -045.

**D9. Finding→fix traceability (Step 7).**
Every control point cites the origin(s)/edge(s) it acts on; control points with no upstream link are flagged as "not derived from diagnosis". Rationale: SRC-CDE-044, -036 (political hijack).

**D10. Optional formalisation (EDF-2 only).**
Allow a BN/fault-tree export of the Origin Network when quantitative data exist (SRC-CDE-033), with an explicit note that elicited probabilities are uncertain (SRC-CDE-031).

---

## 4. Testable hypotheses for the benchmark

| ID | Hypothesis | Case design | Primary measure |
|---|---|---|---|
| H-D1 | Typed edges (D1) raise causal-edge accuracy against a seeded ground-truth graph vs EDF without edge typing | Synthetic/adversarial cases with a hidden known causal graph (incl. a confounder and a decoy correlation) | Edge precision/recall; decoy-edge adoption rate |
| H-D2 | Typed edges + test types raise inter-run/inter-analyst agreement on edges | ≥5 runs per condition per case | Krippendorff's alpha on edge set |
| H-D3 | Rival-network output (D2) increases recall of the true network in cases where the most salient explanation is wrong | Cases with a salient wrong explanation and a late discriminating clue | True-network-in-set rate; premature-closure rate |
| H-D4 | Influence-basis split (D3) reduces overconfident control-point rankings and increases selection of staged/comparative next actions | Cases where the intuitive control point has no interventional evidence and a less obvious one does | Rate of "reasoning-only" control points ranked #1 without flag; % next actions that generate interventional evidence |
| H-D5 | Control-structure fields (D4) increase detection of seeded **feedback/information-flow** origins (e.g., a dashboard that hides queue age; a filtered escalation channel) | Cases where the true origin is a missing/filtered feedback channel | Hidden-feedback-origin recall |
| H-D6 | Loop check (D5) increases identification of seeded compensating responses (e.g., metric gaming after a target is imposed) | Multi-stage cases with a prior failed intervention | Policy-resistance identification rate |
| H-D7 | Control-strength hierarchy (D6) lowers the proportion of training/policy-only top-ranked controls without lowering expert-rated control quality | Any case set; blinded expert key of control quality | % weak-control top picks; pairwise expert preference |
| H-D8 | Local-rationality record (D8) reduces hindsight-laden blame statements | Cases with an operator action that was reasonable given pre-outcome information | Rate of unsupported "should have known" claims |
| H-D9 | Finding→fix traceability (D9) reduces control points not derived from the diagnosis | Cases where the packet includes a sponsor-preferred fix unrelated to the evidence | Rate of sponsor-preferred, unlinked controls recommended |

---

## 5. Limits

- Formal causal-inference results are invoked by analogy; EDF cases are typically n=1 with qualitative evidence, where identification theory gives guidance on *what to state* (assumptions, confounders, rivals) rather than estimates.
- The STAMP reliability advantage comes from safety-accident analyses by expert analysts; transfer to organizational performance problems and to LLM analyzers is untested.
- Several sources verified at abstract level only; no effect sizes beyond those reported in fetched or search-confirmed abstracts are used.
