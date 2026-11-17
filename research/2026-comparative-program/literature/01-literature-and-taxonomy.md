---
id: EDF-COMPARATIVE-2026-002-LIT
title: Comparative literature and framework taxonomy (first tranche)
status: in-progress
version: 0.1.0
created: 2026-11-17
work_item: WI-0014
builds_on: research/2026-framework-improvement/scientific-foundations/
---

# Comparative literature and framework taxonomy — first tranche

This tranche adds what the 2026-11-16 review (224 sources) did not cover:
decision-theoretic and intelligence-analysis methods, safety-engineering methods,
and current LLM reasoning research. It does not repeat the organizational-diagnosis,
operations and RCA coverage in `../2026-framework-improvement/scientific-foundations/`.
Machine-readable entries are in `source-register.csv`.

Verification levels:
- **fetched**: the primary page or abstract was read in this program, and the finding is stated from it.
- **search-confirmed**: bibliographic existence and venue were confirmed by search results only.
- **not-yet-verified**: cited from general knowledge as a pointer. Claims rest on it only after verification.

## 1. Findings that bear directly on EDF

### 1.1 Structured analytic techniques have weak or negative evidence in humans

- **ACH does not improve accuracy and may add noise.** In a randomized study,
  50 intelligence analysts were assigned to use or not use Analysis of
  Competing Hypotheses. The ACH users did not follow all its steps. Evidence
  that ACH reduced confirmation bias was mixed, and ACH may have *increased*
  judgment inconsistency and error (Dhami, Belton and Mandel, 2019; fetched,
  abstract only).
- In a second experiment, training analysts in ACH and requiring its use did
  not improve probability-judgment accuracy: the control group was slightly,
  not significantly, more accurate. ACH did improve ratings of which cues were
  diagnostic. Accuracy improved instead through *statistical* corrections:
  coherentizing probabilities cut mean absolute error by about 35%, and
  combining that with aggregation across 50 analysts cut it by 61% (Mandel,
  Karvetski and Dhami, 2018; fetched).
- Structured analytic techniques spread through endorsement, not testing.
  Multistep techniques can add random noise because analysts make many
  unconstrained implementation choices (Mandel and Tetlock, 2018; fetched;
  perspective piece).

**Relevance.** EDF is a multistep structured technique. The best-studied
relative (ACH) did not raise accuracy in controlled human studies, and it raised
noise. The intervention that did raise accuracy was aggregation. OD2 therefore
includes a self-consistency (aggregation) arm as the direct analogue, and it
measures test–retest agreement, which is the noise prediction.

### 1.2 Reasoning scaffolds for LLMs help narrowly

- A meta-analysis of more than 100 papers, plus the authors' own evaluations on
  20 datasets and 14 models, found that chain-of-thought gains concentrate in math
  and symbolic logic. On knowledge and reasoning benchmarks such as MMLU, direct
  answering is nearly as good (Sprague et al., ICLR 2025; fetched).
- Chain-of-thought explanations can be systematically unfaithful: they can
  rationalize answers driven by biasing features in the prompt without mentioning
  them (Turpin et al., NeurIPS 2023; search-confirmed).
- Without external feedback, LLMs do not reliably improve their reasoning by
  self-correction and can make it worse (Huang et al., ICLR 2024; search-confirmed).
- Sampling several reasoning paths and taking the majority answer
  (self-consistency) improves accuracy over single chain-of-thought (Wang et al.,
  ICLR 2023; search-confirmed).
- Allocating test-time compute adaptively can beat scaling model parameters, and
  the best allocation depends on problem difficulty (Snell et al., 2024;
  search-confirmed).
- Deliberate search over reasoning steps (Tree of Thoughts, Yao et al., NeurIPS
  2023), step-level verification with process reward models (Lightman et al.,
  ICLR 2024), and multi-agent debate (Du et al., ICML 2024) report gains on
  tasks with checkable intermediate steps (all search-confirmed).

**Relevance.** Diagnosis over prose evidence resembles the knowledge-reasoning
tasks where scaffolds help least. The gains that do replicate come from *more
samples or more compute applied well*, not from richer instructions. That makes
Track A, structure versus compute, the critical comparison. Turpin et al. make
Track B, accuracy versus persuasiveness, necessary: a structured explanation can
be more convincing without being more faithful.

### 1.3 Causal reasoning in LLMs

- CLadder tests formal causal inference (association, intervention and
  counterfactual queries) and reports that LLMs struggle with it (Jin et al.,
  NeurIPS 2023; search-confirmed).

**Relevance.** EDF's causal vocabulary (origin network, propagation) is
qualitative. Nothing in EDF supplies the formal identification machinery
(do-calculus, back-door criteria) whose absence CLadder probes. OD2's edge items
test the practical version: distinguishing causal from association-only links.

### 1.4 Safety-engineering methods

- In a controlled student experiment (n = 21, 3 × 3 balanced design), STPA found
  more hazards (recall 0.44 vs FMEA 0.33 vs FTA 0.23) and covered more requirement
  types than FTA or FMEA, but took longer (116 vs 94 vs 88 minutes) (Abdulkhaleq
  and Wagner, EASE 2015; fetched). The sample was small, and participants were
  students.

**Relevance.** The best-evidenced systems method gains coverage at a time cost.
That is the same efficiency trade-off Track A measures for EDF.

### 1.5 Kepner-Tregoe

- The only effectiveness evidence located for Kepner-Tregoe is vendor material
  (case studies and whitepapers on the KT site). No independent controlled
  evaluation was found. **Vendor claim, not evidence.** KT's strength in OD1 and
  OD1-S is therefore the only controlled evidence for it known to this program,
  and it comes from this repository.

### 1.6 Decision theory under uncertainty

- Value of information is the expected improvement in a decision from observing
  something before deciding (Howard, 1966; search-confirmed). It gives a principled
  stopping rule: stop when no available observation's expected value exceeds its
  cost. EDF's Diagnostic Sufficiency ("additional investigation is unlikely to
  materially change the next responsible action") is a qualitative statement of
  the same rule, without the expectation calculation.
- Robust Decision Making seeks strategies that perform acceptably across many
  plausible futures rather than optimizing for one (Lempert, Popper and Bankes,
  2003; search-confirmed).

## 2. Taxonomy

### 2.1 Kinds of thing (do not compare across rows as if equivalent)

| Kind | Members (examples) | What it supplies |
|---|---|---|
| Complete diagnostic procedure | Kepner-Tregoe PA, DMAIC, A3, RCA programs, STPA/CAST, EDF | Ordered steps from problem statement to action |
| Content model | Nadler-Tushman, Burke-Litwin, 7S, Cynefin | What to look at; categories |
| Formal causal model | SCM / causal Bayesian networks, fault trees, system dynamics | Machinery to compute consequences of assumptions |
| Decision-theoretic method | Expected utility, VOI, RDM, minimax regret, real options, MCDA | Rules for choosing actions or what to observe |
| Analytic technique | ACH, Five Whys, fishbone, FMEA, red teaming, premortem | One structured move within a larger process |
| Epistemic principle | Falsification, hypothetico-deductive, abduction, MDL, preregistration | Criteria for good inference, not procedures |
| Organizational theory | HRO, NAT, resilience engineering, double-loop learning, sensemaking | Explanations of how systems fail or learn |
| LLM inference method | CoT, self-consistency, ToT/GoT, ReAct, Reflexion, debate, PRMs | How compute or sampling is spent at inference |

EDF v0.3 is a **complete diagnostic procedure with a qualitative grammar**. Its
closest relatives are procedure-level (KT, CAST/STPA, A3), not formal causal models
or LLM inference methods.

### 2.2 Dimensions (selected frameworks; first pass, judgments from method definitions)

Legend: ● explicit and central; ◐ present but informal; ○ absent. "Evidence" is
the strength of independent controlled evidence for diagnostic accuracy.

| Dimension | EDF v0.3 | KT PA | ACH | RCA/5 Whys | FTA | STPA/CAST | SCM/CBN | VOI/Decision analysis | Self-consistency (LLM) |
|---|---|---|---|---|---|---|---|---|---|
| Purpose | diagnostic | diagnostic | diagnostic | diagnostic | diagnostic/predictive | predictive/diagnostic | all three | prescriptive | (inference) |
| Causal vs correlational | ◐ causal, qualitative | ● causal via distinctions/changes | ◐ | ◐ chain | ● logical | ● control-structure causal | ● formal | n/a | n/a |
| Probabilistic treatment | ○ (confidence words) | ○ | ◐ | ○ | ◐ quantitative FTA | ○ | ● | ● | ◐ vote shares |
| Static vs adaptive | static sequence | static | static | static | static | static | static | adaptive (sequential VOI) | n/a |
| Uncertainty representation | ◐ Unknowns, Confidence | ○ | ◐ consistency matrix | ○ | ◐ | ○ | ● | ● | ◐ |
| Falsifiability built in | ○ | ● IS / IS-NOT test | ● seek disconfirming evidence | ○ | ◐ | ○ | ● testable implications | ○ | ○ |
| Discriminating tests recommended | ◐ (sufficiency) | ● verify probable cause | ● diagnostic evidence | ○ | ◐ | ○ | ● | ● | ○ |
| Recognizes insufficient evidence | ● Diagnostic Sufficiency | ◐ | ◐ | ○ | ○ | ○ | ● identifiability | ● | ○ |
| Stops when further work not worth it | ◐ qualitative | ○ | ○ | ○ | ○ | ○ | ○ | ● | ○ |
| Multi-cause / interaction | ● origin network | ◐ | ◐ | ○ (single chain) | ● AND gates | ● | ● | n/a | n/a |
| Separates mechanism from controls | ● control points ≠ origins | ○ | ○ | ○ | ○ | ● control structure | ◐ | n/a | n/a |
| Transparency / auditability | ● required outputs | ● | ● matrix | ◐ | ● | ● | ● | ● | ◐ |
| Dependence on domain expertise | high | medium | medium | high | high | high | high | medium | (model) |
| Human or LLM evidence of accuracy gain | **none (3 nulls)** | vendor only; OD1 parity | **negative/mixed** | weak (Peerally et al. 2017 critique) | limited | small student RCT (coverage up, time up) | strong for formal inference; LLM weak | strong in theory; limited applied | **positive** (Wang et al.) |

### 2.3 What is distinctive about EDF, and what is shared

| Capability | Distinctive to EDF? | Evidence |
|---|---|---|
| Declared System Context as a validity boundary | Partly distinctive (scoping exists in A3, CAST) | HY-EDF-002 suggestive; untested by controlled ablation until OD2 E_NOCTX |
| Origin *network* rather than single root cause | Shared (FTA, STPA, Burke-Litwin, systemic accident models) | No EDF-specific evidence |
| Control points distinct from origins | Shared with STPA/CAST control structures | HY-EDF-004 suggestive; OD1 showed no action-accuracy gain |
| Diagnostic Sufficiency as a stopping criterion | Qualitative version of VOI stopping | Stopping did not track difficulty in OD1-S under any method |
| Common grammar across domains and success/failure | Distinctive as a representation | Internal demonstration only (HY-EDF-001) |

Uniqueness is not superiority. The only EDF capability with no close established
equivalent is the cross-domain *representational* grammar, and that is the one
capability that diagnostic-accuracy benchmarks cannot measure. Measuring it needs
human communication and review studies (UQ-02).

## 3. Not yet covered (next tranche)

Bow-tie analysis; Cynefin as an operating method; TRIZ; Theory of Constraints
evidence; real options; minimax regret; scenario planning; red teaming and
premortem evidence; HRO, NAT and resilience engineering (Perrow; Weick and
Sutcliffe; Hollnagel); OODA; double-loop learning (Argyris and Schön); sensemaking
(Weick); falsificationism, abduction and MDL as evaluative criteria; Reflexion and
ReAct; Graph of Thoughts; agent-reliability evaluation literature. These are
recorded in the source register as **not-yet-verified** pointers. No claim in this
program rests on them yet.
