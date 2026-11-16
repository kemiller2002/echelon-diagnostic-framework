# EDF Organizational Diagnosis Benchmark (EX-EDF-2026-OD1) — Specification

Mission: EDF-IMPROVEMENT-2026-001 · Work item: WI-0013 · Baseline: `41fea6f` (EDF v0.3)
Status: specification frozen before case authoring (2026-11-16)

## 1. Why this benchmark exists

EX-EDF-001 found that full EDF prompting gave no material benefit over a strong
matched-neutral structured prompt (+0.56 / 100, 95% CI [0.00, 1.39]) and that LLM-judge
scoring saturated near 100. EX-EDF-2026-A003 responded with deterministic
candidate-state scoring on software/operational incidents but remains half-authored.
Neither experiment tests **organizational** diagnosis: stakeholder narratives,
incentives, metric gaming, organizational silence, external causes, intentional
tradeoffs, abstention, or evidence requests.

OD1 reuses A003's anti-ceiling design (frozen candidate items, hidden truth,
mechanical scoring, no LLM judge for primary outcomes) and extends it to the ten
mission benchmark classes (A–J) plus a "non-existent / intentional" class (K).

## 2. Case file (`<CASE>.case.json`, analyzer-visible)

```json
{
  "id": "OD-DEV-A",
  "split": "dev | heldout",
  "benchmarkClass": "A",
  "title": "...",
  "organization": {"type": "...", "industry": "...", "size": "..."},
  "presentingRequest": "The client's own framing of the problem, verbatim, as an engagement request.",
  "evidence": [
    {"id": "E1", "stage": "S1", "source": "COO (interview)", "sourceType": "leadership-account", "text": "..."}
  ],
  "candidates": {
    "hypotheses": [{"id": "H1", "text": "..."}],
    "edges": [{"id": "R1", "from": "...", "to": "...", "relation": "causes"}],
    "actions": [{"id": "A1", "text": "..."}],
    "evidenceRequests": [{"id": "Q1", "text": "Title of evidence that could be collected next (contents hidden)"}]
  },
  "scoredItems": [
    {"itemId": "I1", "refId": "H1", "type": "hypothesis", "asOfStage": null, "allowedAnswers": ["supported", "contradicted", "unknown"]},
    {"itemId": "I9", "refId": "R1", "type": "edge", "asOfStage": null, "allowedAnswers": ["causal", "association-only", "contradicted", "unknown"]},
    {"itemId": "I12", "refId": "A1", "type": "action", "asOfStage": null, "allowedAnswers": ["preferred", "acceptable", "premature", "harmful"]},
    {"itemId": "I16", "refId": "SUFF", "type": "sufficiency", "asOfStage": null, "allowedAnswers": ["sufficient", "insufficient"]}
  ]
}
```

`sourceType` ∈ `leadership-account, manager-account, staff-account, anonymous-staff-survey,
customer-account, system-record, financial-record, external-data, independent-observation,
document, prior-report`.

Answer semantics (given to every analyzer identically):

| Type | Answer | Meaning |
|---|---|---|
| hypothesis | supported | The evidence supplied, taken together, supports the claim more than its rivals. |
| hypothesis | contradicted | The evidence supplied counts against the claim. |
| hypothesis | unknown | The evidence does not discriminate; the claim is live but unestablished. |
| edge | causal | Evidence supports a causal link (e.g., temporal order + mechanism + covariation + a rival ruled out, a natural experiment, or an intervention result). |
| edge | association-only | The two co-occur/correlate in the evidence, but causation is not established (confounding, reverse causation or common cause remain live). |
| edge | contradicted | Evidence counts against the link. |
| edge | unknown | Evidence is insufficient to say even that they are associated. |
| action | preferred | Among the best next actions given current evidence. |
| action | acceptable | Reasonable, but not among the best. |
| action | premature | Could be right if a still-unconfirmed hypothesis were confirmed; committing now is not justified. |
| action | harmful | Likely to worsen outcomes, suppress information, punish the wrong party, or disable a protective control. |
| sufficiency | sufficient / insufficient | Is current evidence sufficient to commit to a substantive (not merely evidence-gathering) intervention? |

`asOfStage` (class H) means: answer using only evidence whose stage is ≤ that stage.

Evidence requests: the analyzer returns up to three `Q` ids, ranked by expected
diagnostic value. Contents are hidden.

## 3. Truth file (`<CASE>.truth.json`, hidden from analyzers)

```json
{
  "caseId": "OD-DEV-A",
  "groundTruthNarrative": "The true causal story, 80-200 words.",
  "answers": [
    {"itemId": "I1", "expected": "contradicted", "weight": 2,
     "requiredEvidence": ["E2", "E5"], "allowedEvidence": ["E2", "E5", "E9"],
     "tags": ["leadership-narrative"], "rationale": "Why this is the only defensible answer from the evidence."}
  ],
  "discriminatingRequests": ["Q2", "Q5"],
  "lowValueRequests": ["Q1"]
}
```

Tags (for secondary metrics): `leadership-narrative, staff-narrative, nonexistent-problem,
external-cause, symptom-not-cause, root-cause, contributing, intentional-tradeoff,
metric-gaming, hidden-problem, organizational-silence, feedback-loop, causal-overclaim-trap,
confounded, protective-control, iatrogenic-intervention, side-effect, local-optimization,
measurement-artifact, revision`.

Authoring rules:

1. Every expected answer must be **uniquely defensible from the supplied evidence
   alone**, by a careful reader, without domain trivia.
2. Traps must be fair: a misleading surface signal must be countered by specific
   evidence in the case.
3. No stakeholder group is automatically authoritative. Leadership, staff and
   system records may each be right or wrong; system records may be gamed.
4. `unknown` and `association-only` must be used where the evidence genuinely
   does not discriminate. Each case should contain at least one item whose correct
   answer is `unknown` or `association-only`.
5. Cases must not mention EDF, Clarity, or any named diagnostic framework.
6. 10–18 evidence items, 4–6 hypotheses, 3–5 edges, 4–5 actions, exactly one
   `SUFF` item, 5–6 evidence requests (2–3 discriminating).
7. The presenting request must be plausible and may be wrong.

## 4. Analyzer output contract

```json
{"caseId": "...",
 "answers": [{"itemId": "I1", "answer": "supported", "confidence": 80, "evidenceIds": ["E1"]}],
 "evidenceRequests": ["Q2", "Q5", "Q1"],
 "summary": "≤120 words; ignored by primary scorer"}
```

## 5. Scoring (deterministic, `harness/Scoring.fsx`)

Per case, per run (0–100):

- 65 pts weighted exact-answer accuracy;
- 15 pts weighted evidence F1 (precision against `allowedEvidence`, recall against `requiredEvidence`), correct items only receive F1 credit, incorrect items count 0;
- 10 pts calibration: mean over items of `1 − (c − y)²`, where `c` = confidence/100 and `y` = 1 if correct;
- 10 pts evidence-request value: `|chosen ∩ discriminating| / min(3, |discriminating|)` minus 0.5 per low-value request chosen (floor 0);
- −15 per `harmful` action answered `preferred` or `acceptable` (floor 0).

Secondary metrics: false-diagnosis rate (hypotheses with expected ≠ supported
answered supported), missed-problem rate (expected supported answered otherwise),
unsupported certainty (expected `unknown` answered otherwise), causal accuracy and
causal overclaim rate (edges with expected ≠ causal answered causal), abstention
accuracy (SUFF), premature-action promotion (premature answered preferred),
harmful-action promotion, leadership-narrative adoption (tag `leadership-narrative`
with expected ≠ supported answered supported), tag-specific accuracy, Brier score,
item-level test–retest agreement between trials, prompt/output size.

Schema-invalid outputs are terminal data and score 0 (no repair in primary analysis).

## 6. Splits and held-out policy

See `held-out-policy.md`. Dev cases may be inspected and used to design variants;
held-out cases are frozen (SHA-256) before any variant is designed and may not be
inspected for analyzer behaviour until every condition's held-out runs are complete.

## 7. Known limitations (declared in advance)

- Synthetic cases authored by Claude-family agents and analyzed by Claude-family
  agents: author–analyzer affinity and single-provider bias are possible.
- Candidate-item format tests recognition among offered options, not open-ended
  generation of hypotheses; missed-problem detection is therefore *upper-bounded*.
- Synthetic cases are not evidence of real-world organizational effectiveness.
- Human-rater reliability, intervention outcomes in real organizations, and
  time-on-task by human consultants cannot be measured here.
