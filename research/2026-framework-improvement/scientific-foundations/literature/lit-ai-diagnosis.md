# Literature Review I: AI-Based Diagnosis and LLM Reliability for EDF

Workstream: I. Prepared 2026-11-16 for the EDF framework-improvement review.
Source IDs refer to `sources-FGI.csv`. Verification labels: **fetched**, **search-result**, **from-memory-unverified** (none used here).

## Scope and the EDF question

EDF is likely to be run, at least in part, by LLMs: as analyst, as one of several independent analyses in Diagnostic Calibration, or as reviewer/judge. This review asks: (1) Is there evidence that LLMs can diagnose organizations? (2) Does structured prompting (a framework like EDF) help? (3) What failure modes matter: calibration, sycophancy toward the premise, judge biases, contamination? (4) What do sequential medical-diagnosis benchmarks teach about information-seeking?

## 1. Direct evidence on LLMs for organizational or business diagnosis

### Finding 1.1: There is almost no direct evidence
- A targeted search found no peer-reviewed study that scores LLM diagnoses of real organizations against expert or outcome ground truth. The nearest is an ECIS 2025 paper that evaluated ten LLMs as "management consultants" on interview-style case studies [SRC-FGI-049, search-result]. Case interviews are stylised puzzles, not diagnoses of real organizations.
- **Type:** gap finding.
- **Implication:** Any claim that "EDF + LLM produces valid organizational diagnoses" is currently untested. Echelon would need to build its own benchmark (see section 6).

### Finding 1.2: AI help on business problem-solving can reduce correctness outside the "frontier"
- Dell'Acqua et al. (2023, HBS working paper; 758 BCG consultants, about 7% of BCG's individual contributors). On tasks inside the AI capability frontier, consultants with GPT-4 completed 12.2% more tasks, 25.1% faster. On a business-problem task designed to sit outside the frontier, they were **19 percentage points less likely** to reach the correct answer than consultants without AI [SRC-FGI-048, search-result].
- **Type:** empirical (pre-registered field experiment).
- **Implication:** The outside-frontier task asked consultants to diagnose a business problem from interviews and spreadsheet data. That is the closest available analogue to EDF, and AI help *hurt*. Human reviewers of AI-assisted EDF output need an explicit step that checks the AI's causal claims against the raw evidence.

### Finding 1.3: In clinical diagnosis, AI access did not improve physicians, even when the AI alone did better
- Goh et al. (2024, *JAMA Network Open*, RCT, 50 physicians). GPT-4 access did not significantly improve diagnostic reasoning scores (76% vs 74%; difference 2 points, 95% CI −4 to 8). GPT-4 alone scored higher than both physician groups [SRC-FGI-046, search-result].
- **Type:** empirical (RCT).
- **Implication:** Human–AI combination is not automatically additive. EDF's workflow must specify how AI output enters human judgment (for example, independent analysis first, then comparison), not just "use AI".

## 2. Does structured prompting or a framework help?

### Finding 2.1: Chain-of-thought helps mainly on math and symbolic tasks
- Sprague et al. (2024/ICLR 2025) meta-analysed more than 100 papers and ran 20 datasets × 14 models. Chain-of-thought (CoT) gave large gains mainly on math and logic. On MMLU, direct answers were nearly as accurate unless the item involved an equals sign [SRC-FGI-039, search-result].
- **Type:** empirical (meta-analysis plus experiments).
- **Implication:** A step-by-step framework does not by itself improve reasoning on the narrative and causal tasks typical of organizational diagnosis. EDF's value for an LLM is more likely to come from *coverage and auditability* (it forces the model to separate observation from inference, list alternatives and state unknowns) than from raw accuracy. That has to be tested, not assumed.

### Finding 2.2: Domain-structured reasoning prompts preserve accuracy and add interpretability, without clear accuracy gains
- Savage et al. (2024, *npj Digital Medicine*) compared standard CoT with "diagnostic reasoning" prompts (differential, intuitive, analytical, Bayesian) on GPT-4. Accuracy was similar: about 76% for CoT versus 72% to 78% for the structured prompts, per a secondary summary. The authors framed the benefit as interpretability without loss of accuracy [SRC-FGI-043, search-result].
- **Type:** empirical.

### Finding 2.3: Results depend heavily on incidental prompt formatting
- Sclar et al. (2024, ICLR) found accuracy spreads of up to 76 points (LLaMA-2-13B) and up to 56 points (GPT-3.5) across meaning-preserving format changes. Format rankings correlated only weakly across models [SRC-FGI-042, search-result].
- **Type:** empirical.
- **Implication:** Any EDF-for-LLM benchmark must test several equivalent templates and report the range, not one prompt.

### Finding 2.4: Written reasoning is not a faithful account of what drove the answer
- Turpin et al. (2023, NeurIPS). Biasing features (for example, always placing the correct answer at "(A)") shifted answers, and the CoT explanations did not mention them. Accuracy fell by up to 36% on BIG-Bench Hard [SRC-FGI-038, search-result].
- **Type:** empirical.
- **Implication:** An EDF analysis written by an LLM can look well-reasoned while being driven by framing in the prompt. EDF's "Evidence" step should require **traceable citations to source material** for each claim, not just a narrative.

## 3. Calibration and overconfidence

### Finding 3.1: Verbalised confidence is overconfident
- Xiong et al. (2024, ICLR): verbalised confidence clustered at 80% to 100%, often in multiples of 5. Vanilla verbalised confidence had average ECE above 0.377 for several models. Sampling plus aggregation helped somewhat [SRC-FGI-040, search-result].
- Kadavath et al. (2022): larger models are fairly well calibrated on multiple-choice questions *in the right format*. Calibration degrades out of distribution [SRC-FGI-041, search-result].
- **Type:** empirical.
- **Implication:** EDF's "Confidence" field should not take an LLM's stated confidence at face value. Prefer **consistency-based confidence**: run independent analyses and measure agreement on the top control point. This supports Diagnostic Calibration, but only if the runs are actually independent (see 5.2).

## 4. Sycophancy and premise acceptance

### Finding 4.1: Assistants systematically agree with the user's stated view
- Sharma et al. (2023/2025, Anthropic): five AI assistants showed sycophancy across four tasks. Human preference data favour responses that match the user's views, and optimising against preference models sometimes trades truthfulness for agreement [SRC-FGI-034, search-result].
- Chen et al. (2025, *npj Digital Medicine*): with illogical drug-related requests, GPT models complied 100% of the time. Prompts that invited refusal or recall of facts first reduced this [SRC-FGI-052, search-result].
- **Type:** empirical.

### Finding 4.2: LLMs often accept false premises in the question
- Hu et al. (2023, ACL, FalseQA): 2,365 false-premise questions. Models can learn to detect them with modest fine-tuning [SRC-FGI-053, search-result].
- PCBench (2025): across 15 LLMs, models often uncritically accept flawed premises [SRC-FGI-054, search-result].
- Cancer-Myth (2025): no frontier model tested corrected false presuppositions more than 43% of the time [SRC-FGI-055, search-result].
- **Type:** empirical.
- **Implication (high priority):** In organizational work, the request usually carries the client's theory ("Why is the sales team underperforming?" presumes the sales team is the locus). EDF steps 1 and 2 (System Context, factual Outcome) are the natural defence. **Proposed change:** add a required **"Premise check"** sub-step to step 1. List the causal assumptions embedded in the request, and keep at least one origin hypothesis outside the requester's framing. For AI-run EDF, state the request in neutral form before analysis.

## 5. LLM-as-judge and multi-analysis aggregation

### Finding 5.1: LLM judges have position, verbosity and self-preference biases
- Zheng et al. (2023, NeurIPS; MT-Bench): position, verbosity and self-enhancement biases. GPT-4 matched human preferences more than 80% of the time, on a par with human–human agreement [SRC-FGI-035, search-result].
- Wang et al. (2023), "LLMs are not fair evaluators": swapping the order of answers can flip verdicts. Mitigations include evidence-first scoring, averaging over both orders, and human review of disputed items [SRC-FGI-037, search-result].
- Panickssery et al. (2024, NeurIPS): models recognise their own outputs, and self-recognition correlates linearly with self-preference [SRC-FGI-036, search-result].
- **Type:** empirical.
- **Implication:** If an LLM synthesises or adjudicates Diagnostic Calibration outputs, (a) present analyses in randomised and counterbalanced order, (b) normalise length, (c) do not use the same model family as both analyst and judge, and (d) send disagreements to a human.

### Finding 5.2: Self-correction and debate are weaker than they look
- Huang et al. (2024, ICLR): without external feedback, LLMs struggle to self-correct reasoning, and performance sometimes falls after self-correction [SRC-FGI-056, search-result].
- Smit et al. (2024, ICML): multi-agent debate did not reliably beat self-consistency or ensembling unless tuned. Agreement-intensity settings mattered a lot [SRC-FGI-057, search-result].
- Self-consistency (Wang et al., 2023, ICLR): sampling several reasoning paths and taking a majority vote improved accuracy, for example +17.9% on GSM8K [SRC-FGI-058, search-result].
- **Type:** empirical.
- **Implication:** Diagnostic Calibration (multiple independent EDF analyses) is closer to self-consistency/ensembling, which has support, than to debate. Its benefit should be tested against a **cost-matched baseline**: the same compute spent on one analysis vs N independent analyses. Convergence must not be read as proof. The operating-cycle doc already warns about this, and the evidence backs that warning.

## 6. Sequential medical-diagnosis benchmarks: lessons for asking for information

### Finding 6.1: Performance drops sharply when information must be gathered through conversation
- Johri et al. (2025, *Nature Medicine*, CRAFT-MD): four LLMs did well on exam-style vignettes but worse in multi-turn conversational history-taking, and worse in back-and-forth than with summarised conversations. Failures included not asking the right questions, missing critical history and failing to synthesise scattered information [SRC-FGI-044, search-result; press-release level, exact percentages not confirmed].
- Laban et al. (2025, ICLR 2026): across more than 200,000 simulated conversations, LLMs dropped on average 39% from single-turn to multi-turn underspecified settings. The drop was mainly *unreliability*, not loss of aptitude. Causes: early assumptions, premature final answers, and over-reliance on those early answers [SRC-FGI-047, fetched].
- Hager et al. (2024, *Nature Medicine*): on 2,400 real MIMIC abdominal cases, open LLMs underperformed clinicians (for example, cholecystitis 13% to 68% vs 84%). They did not follow guidelines, misread labs, and were *sensitive to the order and quantity of information* [SRC-FGI-045, search-result].
- **Type:** empirical.
- **Implication:** Organizational diagnosis is inherently multi-turn and underspecified, so these failure modes apply directly. **Proposed change:** an AI-run EDF should (a) delay committing to an origin network until a minimum evidence set is collected (no premature closure), (b) periodically re-state all collected evidence in one consolidated summary (CRAFT-MD found summaries help), and (c) re-test the leading hypothesis with evidence presented in a different order.

### Finding 6.2: Structured roles plus cost awareness improve sequential diagnosis
- Nori et al. (2025) MAI-DxO/SDBench, details in lit-adaptive.md. Role-structured orchestration (Hypothesis, Test-Chooser, Challenger, Stewardship, Checklist) improved accuracy and cut cost against an off-the-shelf model across several model families [SRC-FGI-031, fetched].
- MediQ (Li et al., 2024): naive question-asking *hurt* accuracy. Confidence-gated asking and abstention helped [SRC-FGI-033, search-result].
- Uncertainty of Thoughts (Hu et al., 2024): information-gain question selection improved success with fewer questions [SRC-FGI-032, search-result].
- **Type:** empirical (benchmarks, mostly preprints or conference papers).
- **Implication:** The evidence favours structure that is *functional* (separate roles for hypothesising, choosing discriminating evidence, challenging and cost control) over structure that is merely *sequential* (fill in nine headings in order). For AI use, EDF should be run as roles over a shared evidence ledger, not as one long template pass.

## 7. Benchmark contamination

- Sainz et al. (2023, EMNLP Findings): contamination inflates performance, and they call for per-benchmark contamination measurement [SRC-FGI-051, search-result]. Xu et al. (2024) survey: definitions, detection and mitigation, including regenerating benchmark items and benchmark-free evaluation [SRC-FGI-050, search-result].
- **Implication:** Famous organizational case studies (Challenger, Boeing 737 MAX, Apollo, and the like; the EDF spec itself uses aircraft-loss and moon-landing examples) are certainly in training data. An EDF benchmark built on them measures recall, not diagnosis. **Use post-cutoff, private or synthetic-but-validated cases, and include perturbed variants (same surface, different true cause) to detect memorisation.**

## 8. Consolidated proposed changes to EDF (Workstream I)

| # | Change | Where | Evidence basis |
|---|--------|-------|----------------|
| I1 | Add a **Premise check** to step 1: list the causal assumptions embedded in the request, and keep at least one origin hypothesis outside the requester's framing. | Step 1 | SRC-FGI-034, -052, -053, -054, -055 |
| I2 | **Claim-level traceability:** each origin and propagation claim cites specific evidence items. Uncited claims are labelled inference. | Step 6 | SRC-FGI-038 |
| I3 | Do not use LLM self-reported confidence as EDF Confidence. Use **agreement across independent runs** and evidence quality. | Confidence field | SRC-FGI-040, -041, -058 |
| I4 | Diagnostic Calibration with LLMs: independent runs, **cost-matched baseline**, counterbalanced order, length normalisation, cross-family judging, human resolution of disagreements. | Calibration extension | SRC-FGI-035, -036, -037, -056, -057 |
| I5 | AI-run EDF as **functional roles** (Hypothesis, Discriminating-Evidence, Challenger, Stewardship) over a shared evidence ledger, with an explicit commit threshold. | Operating guidance | SRC-FGI-031, -033, -032 |
| I6 | **Anti-premature-closure protocol:** minimum evidence set before committing to an origin network, consolidated restatement of evidence, order-perturbation re-test. | Steps 4–6 | SRC-FGI-047, -044, -045 |
| I7 | Human review of AI-assisted EDF must **independently check top-ranked causal claims against raw evidence**. Do not just accept or reject the AI's narrative. | Review | SRC-FGI-048, -046 |
| I8 | Benchmarks: avoid famous cases; use private, post-cutoff or perturbed cases; report results across several equivalent prompt formats. | Benchmark design | SRC-FGI-050, -051, -042 |

## 9. Testable benchmark hypotheses

- **H-I1 (framework vs plain prompt):** On a contamination-controlled set of organizational cases, an LLM following EDF will *not* beat a plain "diagnose this" prompt on top-cause accuracy by more than a small margin (prediction based on SRC-FGI-039, -043). It *will* do better on (a) separating observation from inference, (b) number of alternative hypotheses considered, and (c) rate of explicit unknowns. If the accuracy gain is large, that would be new evidence.
- **H-I2 (premise check):** On cases where the request embeds a false locus (for example, it names team A, but the true origin is upstream process B), I1 will raise the rate of correctly identifying the off-premise origin by a pre-registered margin against v0.3.
- **H-I3 (functional roles):** A role-structured EDF (I5) will beat a single-pass EDF template on accuracy per evidence request in a gatekeeper (SDBench-style) setup.
- **H-I4 (calibration vs compute):** N independent EDF runs with majority synthesis will beat one run using N× tokens. Debate-style synthesis will not beat independent-run voting without tuning (replicating SRC-FGI-057 in this domain).
- **H-I5 (order sensitivity):** Final control-point rankings from AI-run EDF will change under reordering of the same evidence in a measurable share of cases. I6 will reduce that share.
- **H-I6 (judge bias):** An LLM judge grading EDF analyses will prefer longer analyses and its own model family's analyses. Counterbalancing and cross-family judging will reduce agreement gaps with blinded human experts.
- **H-I7 (contamination probe):** Accuracy on famous cases will exceed accuracy on surface-matched perturbed variants with different true causes. The gap estimates memorisation.

## 10. Limitations

- Most findings come from medical or general-reasoning domains. Transfer to organizational diagnosis is an inference.
- Several key results (MAI-DxO, Laban, Cancer-Myth, PCBench) are preprints or recent conference papers, and models change fast. Re-test on current models.
- CRAFT-MD and Chen et al. details were confirmed only at press-summary level.
