---
id: EX-EDF-2026-OD2-SPEC
title: OD2 — hard diagnostic benchmark with evidence acquisition (specification)
status: frozen-before-case-authoring
version: 1.0.0
created: 2026-11-17
work_item: WI-0014
mission: EDF-COMPARATIVE-2026-002
derived_from: research/2026-framework-improvement/benchmarks/benchmark-specification.md
---

# OD2 benchmark specification

## 1. Why OD2 exists

Every completed EDF comparison (EX-EDF-001, OD1, OD1-S) found no material
difference between methods. Those benchmarks were near ceiling: condition means
were 87–96, and the analyzer saw curated evidence that contained the answer.
A null on a saturated benchmark cannot distinguish "methods do not matter" from
"the benchmark cannot see the difference".

OD2 is built to be hard enough that methods *could* differ:

1. **Most evidence is hidden.** The analyzer sees a short initial file and a
   catalogue of titles, and may collect at most **5** catalogue items.
2. **The initial file is often wrong.** Presenting narratives, dashboards and
   first reports point to the wrong diagnosis in several classes.
3. **Correct answers include "unknown" and "insufficient"** where the full
   catalogue does not decide the question.
4. **Stopping is part of the task.** In some cases the initial file already
   suffices and further collection only costs.
5. **Three domains.** Organizational, technical-incident (software, operations,
   engineering), and cross-domain cases, so results are not specific to one
   kind of problem.

OD2 keeps OD1's anti-ceiling machinery: frozen candidate items, hidden truth,
deterministic scoring, and no LLM judge for primary outcomes.

## 2. Case classes

Each held-out class has two cases in different domains. The dev split has one
case for each of seven classes.

| Code | Class | What makes it hard | Characteristic correct answers |
|---|---|---|---|
| KNOWN | Single root cause, findable | Decisive evidence sits in 1–2 catalogue items among plausible distractors | One hypothesis supported after the right request |
| INTERACT | Interacting causes | Neither cause alone is sufficient; each alone looks contradicted by a contrast case | Two hypotheses supported; joint edge causal; single-cause fixes premature |
| MISLEAD | Misleading evidence | Initial file contains a salient but misleading signal, countered only by a catalogue item | Salient hypothesis contradicted |
| INCOMPLETE | Decisive evidence absent | No catalogue item discriminates the two leading hypotheses | Both leading hypotheses unknown; SUFF insufficient; an evidence-gathering action preferred |
| CONTRA | Contradictory sources | Sources disagree; independence and provenance resolve which to trust | Interested-party account contradicted by independent record |
| NOCONC | No reliable conclusion | Noise or base rates explain the signal as well as any cause | Most hypotheses unknown; edges association-only or unknown; SUFF insufficient or "no intervention" preferred |
| SEQ | Sequential investigation | The discriminating second request is only identifiable after reading the first | Correct answers require a specific two-step path |
| OBVWRONG | Obvious diagnosis wrong | The obvious cause fits the initial file perfectly; a contrast case refutes it | Obvious cause contradicted; a less salient cause supported |
| CHANGE | Changing conditions | Stage-2 evidence reverses a stage-1 conclusion | `asOfStage` items differ between stages |
| STOP | Stop early | Initial file is sufficient; catalogue items are low value | SUFF sufficient; zero or one read is optimal |
| CROSS | Cross-domain | Mechanism spans two domains (e.g., clinical operations and IT, finance and supply chain) | Cause outside the domain that shows the symptom |
| ADVERS | Strategic or deceptive actor | A record or account is gamed or fabricated; a provenance clue exposes it | Fabricated record not trusted; gaming hypothesis supported |
| CONFOUND | Hidden confounder | A strong correlation is driven by a third factor revealed in the catalogue | Edge association-only; confounder supported |

## 3. Files

### 3.1 Case file `<ID>.case.json` (analyzer-visible parts only)

```json
{
  "id": "OD2-HO-MISLEAD-1",
  "split": "dev | heldout",
  "benchmarkClass": "MISLEAD",
  "domain": "organizational | technical | cross-domain",
  "title": "...",
  "setting": "One paragraph: what the system is, who asked for the diagnosis, and why.",
  "presentingRequest": "The requester's framing, verbatim. It may be wrong.",
  "initialEvidence": [
    {"id": "E1", "stage": "S1", "source": "...", "sourceType": "...", "text": "..."}
  ],
  "catalogue": [
    {"id": "C1", "stage": "S1", "title": "Title of collectable evidence", "source": "...", "sourceType": "...", "text": "hidden from the analyzer until read"}
  ],
  "candidates": {
    "hypotheses": [{"id": "H1", "text": "..."}],
    "edges": [{"id": "R1", "from": "...", "to": "...", "relation": "causes"}],
    "actions": [{"id": "A1", "text": "..."}]
  },
  "scoredItems": [
    {"itemId": "I1", "refId": "H1", "type": "hypothesis", "asOfStage": null, "allowedAnswers": ["supported", "contradicted", "unknown"]}
  ]
}
```

- 4–7 initial evidence items; 10–14 catalogue items; 4–6 hypotheses;
  3–5 edges; 4–5 actions; exactly one `SUFF` item.
- Catalogue `text` is never shown in the task file. Each catalogue item is
  written to its own file under a per-run label (`X1..Xn`, shuffled per run), so
  neither order nor label leaks value.
- `sourceType` values follow OD1, extended with `log`, `telemetry`,
  `change-record`, `vendor-account`, `regulator-record`, `expert-account`.

### 3.2 Truth file `<ID>.truth.json` (hidden)

```json
{
  "caseId": "OD2-HO-MISLEAD-1",
  "groundTruthNarrative": "80-250 words",
  "answers": [
    {"itemId": "I1", "expected": "contradicted", "weight": 2,
     "requiredEvidence": ["E2", "C5"], "allowedEvidence": ["E2", "C5", "C7"],
     "tags": ["misleading-signal"], "rationale": "..."}
  ],
  "discriminatingReads": ["C5", "C9"],
  "lowValueReads": ["C1", "C2"],
  "optimalReadPath": ["C5", "C9"],
  "authoringNotes": "How the trap works and what fair counter-evidence exists."
}
```

**Truth convention.** Every expected answer is the conclusion a careful analyst
would reach **after reading the optimal path**. That path never exceeds 3 items,
so it fits the 5-item budget. An analyzer that does not collect decisive
evidence will often answer "unknown" where the key says otherwise and lose
credit. That is intended: acquisition is part of diagnosis. For INCOMPLETE and
NOCONC cases, the key is what the *entire* catalogue supports, which is
"unknown" or "insufficient" for the leading question.

### 3.3 Authoring rules

1. Every expected answer is uniquely defensible from initial evidence plus the
   optimal path, without domain trivia beyond what a competent generalist knows.
2. Traps are fair: every misleading signal is countered by specific evidence that
   is present either in the initial file or in the catalogue.
3. No source type is automatically authoritative. Leaders, staff, logs, dashboards
   and vendors may each be wrong, interested, or gamed.
4. Each case has at least one item whose correct answer is `unknown` or
   `association-only`.
5. Cases do not mention EDF, Clarity, Kepner-Tregoe, or any named diagnostic
   framework, and do not use framework vocabulary such as "origin network",
   "control point", "IS / IS NOT", or "root cause".
6. Catalogue titles describe *what* would be collected, never what it shows.
   A title must not reveal its own diagnostic value.
7. Distractor catalogue items are plausible things an analyst might ask for.
8. Cases are fictional. No real organization, product or incident is named or
   closely reconstructed (contamination control).
9. Stop-early cases (STOP) have `discriminatingReads: []`, and their initial file
   suffices for every key.

## 4. Analyzer task and output

The analyzer receives one task file containing a shared frame, a method module
(the experimental condition), and the case (setting, request, initial evidence,
catalogue titles with file paths, candidates, scored items). It may use only the
Read tool, only on the listed catalogue files, and at most 5 times. It returns
structured output:

```json
{"caseId": "...",
 "answers": [{"itemId": "I1", "answer": "...", "confidence": 0, "evidenceIds": ["E1", "C5"]}],
 "brief": "At most 200 words to the requester: what is going on, how sure you are, what to do next. Plain language; no method vocabulary."}
```

Evidence ids cited may be initial (`E*`) or catalogue (`C*`) ids. The harness
maps run labels `X*` back to catalogue ids before scoring.

## 5. Scoring (deterministic; `harness/Od2.fsx`)

Per run, 0–100:

| Component | Points | Definition |
|---|---|---|
| Accuracy | 60 | Weighted exact-answer accuracy |
| Evidence | 10 | Weighted evidence F1 on correct items (OD1 rule) |
| Calibration | 10 | Mean over items of `1 − (c − y)²` |
| Acquisition | 20 | `hits/|disc|` (1 if `disc` is empty) minus 0.1 per non-discriminating read, clamped to [0, 1]; **0 if more than 5 items were read** |
| Harm penalty | −15 each | Harmful action answered preferred or acceptable; floor 0 |

Acquisition weighs more than in OD1 (20 vs 10) because collection is the
mechanism OD1-07 identified as the largest lever.

Secondary outcomes: items read; budget breaches; discriminating-read recall;
unsupported certainty; causal overclaim; missed abstention; false abstention;
harmful and premature promotion; high-confidence error rate; Brier score;
expected calibration error; class-specific accuracy; input, output and thinking
tokens; cost in USD; wall time; test–retest agreement.

Invalid outputs (no structured output, or schema violation) score 0 and are
reported; they are never repaired in the primary analysis.

## 6. Separation of persuasiveness from correctness (Track B)

The `brief` is rated in a separate, blind step. A rater sees the case's setting,
request and initial evidence plus one brief (method vocabulary scrubbed by a
fixed substitution list), with no truth and no condition label. It rates
persuasiveness (1–7), perceived correctness (0–100), and actionability (1–7).
Primary scoring never reads the brief. Track B asks whether conditions differ
in rated persuasiveness or perceived correctness **at equal true accuracy**, and
whether perceived correctness tracks true accuracy equally well across conditions.
The rater is a Claude model: this is a declared limitation, not independent evidence.

## 7. Contamination and authorship controls

- Cases are authored after this specification is committed, by isolated
  headless Claude sessions that receive only the authoring contract and their
  class assignment. They never see method prompts.
- Each case is reviewed by a separate isolated session that first solves it blind
  with full evidence and then compares its answers with the key. Disagreements
  are resolved by revising the case (never by keeping an indefensible key), and the
  review record is preserved.
- Held-out files are hash-frozen before any held-out run. Truth files are moved
  outside the analyzer's working tree during runs. Each analyzer runs in an
  empty directory containing only its task file and evidence files.
- All authors, reviewers, analyzers and raters are Claude-family models. This
  shared-family limitation is declared on every result.
