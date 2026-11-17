# OD2 case-authoring contract (v1.2, amendments 1-2)

You are authoring one case for a benchmark that tests how well analysts diagnose
problems in systems: organizations, technical systems, and systems that span both.
Analysts will see a short initial evidence file and a catalogue of evidence they may
collect, **at most two items**. They then classify frozen candidate items. A hidden
answer key scores them mechanically.

Your case must be **hard but fair**. A careful analyst who reads only the initial
file should be likely to get the central question wrong or leave it unresolved.
A careful analyst who collects the right two catalogue items should be able
to answer every item correctly and defend each answer from the evidence alone.

## Your assignment

- Case id: `{{ID}}`
- Split: `{{SPLIT}}`
- Class: `{{CLASS}}` — {{CLASS_DEFINITION}}
- Domain: `{{DOMAIN}}`
- Setting seed: {{SETTING}} (a seed only: invent every name, number and detail; keep it fictional)

## Answer semantics (analysts receive exactly these definitions)

- hypothesis: "supported" = the evidence, taken together, supports the claim more than its rivals; "contradicted" = the evidence counts against it; "unknown" = the evidence does not discriminate, so the claim is live but unestablished.
- edge (a claimed link from one condition to another): "causal" = the evidence supports a causal link (for example temporal order plus mechanism plus covariation with the main rival ruled out, a natural experiment, or an intervention result); "association-only" = they co-occur or correlate but causation is not established; "contradicted" = evidence counts against the link; "unknown" = evidence is insufficient even to show association.
- action: "preferred" = among the best next actions given the evidence; "acceptable" = reasonable but not among the best; "premature" = could be right if a still-unconfirmed hypothesis were confirmed, but committing now is not justified; "harmful" = likely to worsen outcomes, suppress information, punish the wrong party, or disable a protective control.
- sufficiency (item SUFF): is the evidence sufficient to commit to the action you rank highest, where that action is substantive rather than merely evidence-gathering? "sufficient" or "insufficient". Concluding that no intervention is warranted counts as a substantive decision.
- Items with `asOfStage` set must be answered using only evidence whose stage is at or before that stage.

## Truth convention

Each expected answer is what a careful analyst would conclude after reading the
initial file **plus the optimal read path** (exactly two catalogue items, which you
name in `optimalReadPath`). For classes INCOMPLETE and NOCONC, the expected answer is
what the **whole catalogue** supports (their optimal path is the two items that best show
the question cannot be settled). For the leading question that is "unknown",
and SUFF is "insufficient" unless no intervention is warranted.

## Rules

1. Every expected answer must be uniquely defensible from the evidence, with no
   domain trivia beyond what a competent generalist knows. If two answers are
   defensible, change the case until only one is.
2. Traps must be fair: every misleading signal is countered by specific evidence in
   the initial file or the catalogue.
3. No source type is automatically authoritative. Leaders, staff, logs, dashboards,
   vendors and experts may each be right, wrong, interested, or gamed.
4. Include at least one item whose correct answer is "unknown" or "association-only",
   and at least one action that is "premature".
5. Do not mention EDF, Clarity, Kepner-Tregoe, or any named diagnostic framework or
   method. Do not use the phrases "root cause", "origin network", "control point",
   "propagation", "IS / IS NOT", "five whys", or "fishbone".
6. Catalogue titles say what would be collected, never what it shows. Bad: "Logs
   showing the cache bug". Good: "Application error logs, 1–14 March".
7. Distractor catalogue items must be things a reasonable analyst might request.
8. Everything is fictional. Do not name or closely reconstruct a real organization,
   product, person or incident.
9. Sizes: 4–7 initial evidence items (`E1..`); 14–16 catalogue items (`C1..`); 4–6
   hypotheses; 3–5 edges; 4–5 actions; one SUFF item. Scored items cover every
   hypothesis, edge and action plus SUFF. For class CHANGE, also add stage-specific
   items (`asOfStage: "S1"`) for at least two hypotheses whose answer changes by S2.
10. Evidence texts are 30–140 words each and read like real artifacts: interview
    notes, log excerpts, tables in prose, emails, audit findings.
11. Weights: 3 for the central diagnostic items, 2 for important traps, 1 otherwise.
12. `discriminatingReads` lists the catalogue items that change at least one expected
    answer relative to the initial file alone: **exactly two or three items** (empty for
    STOP; for INCOMPLETE and NOCONC, the items that show the leading question cannot be
    settled, possibly empty). Discriminating items must be **non-redundant**: each settles
    at least one expected answer that no other catalogue item settles, so an analyst who
    reads the wrong pair loses accuracy. `optimalReadPath` is exactly the two items that
    together settle every key (for STOP it is empty). `lowValueReads` lists
    items that change nothing and are clearly peripheral.
13. Evidence citations: `requiredEvidence` lists the items needed to justify the
    answer; `allowedEvidence` is a superset that also lists items that legitimately
    bear on it.

14. **Hypothesis parity.** Write every hypothesis at the same level of specificity. No
    hypothesis may contain a mechanism detail (a name, number, timing or component) that
    only a catalogue item establishes. A reader of the initial file alone must not be able
    to pick the true hypothesis by its wording.
15. **Action keys.** Key an action *harmful* only if it causes damage both under the
    hypothesis the evidence supports and under every live rival. Key an action
    *premature* only if it would be clearly beneficial once its hypothesis is confirmed
    and would cause no lasting damage if that hypothesis is wrong. Rewrite any action
    that fits neither definition cleanly.
16. **Contradicted edges.** Key an edge *contradicted* only when evidence counts against
    the link itself, not merely because a rival explanation is supported.
17. The case JSON includes `"readBudget": 2`.
18. No quantifier claims ("mainly", "most", "largely", "primarily") in hypotheses or
    edges unless the evidence states a magnitude that decides them.
19. At most one action is keyed *preferred*, unless two actions are explicitly
    complementary and the case says so.

## Output

Return one JSON object with two members, `case` and `truth`, through the structured
output tool.

`case`:
```
{ "id", "split", "benchmarkClass", "domain", "readBudget", "title", "setting", "presentingRequest",
  "initialEvidence": [{"id","stage","source","sourceType","text"}],
  "catalogue": [{"id","stage","title","source","sourceType","text"}],
  "candidates": {"hypotheses":[{"id","text"}], "edges":[{"id","from","to","relation"}], "actions":[{"id","text"}]},
  "scoredItems": [{"itemId","refId","type","asOfStage","allowedAnswers"}] }
```
- `type` is one of hypothesis, edge, action, sufficiency; `refId` is the H/R/A id, or
  "SUFF".
- allowedAnswers: hypothesis ["supported","contradicted","unknown"]; edge
  ["causal","association-only","contradicted","unknown"]; action
  ["preferred","acceptable","premature","harmful"]; sufficiency
  ["sufficient","insufficient"].
- `stage` is "S1" unless the class is CHANGE (then S1 or S2). `asOfStage` is null or
  "S1".
- sourceType is one of: leadership-account, manager-account, staff-account,
  anonymous-staff-survey, customer-account, system-record, financial-record,
  external-data, independent-observation, document, prior-report, log, telemetry,
  change-record, vendor-account, regulator-record, expert-account.

`truth`:
```
{ "caseId", "groundTruthNarrative" (80-250 words),
  "answers": [{"itemId","expected","weight","requiredEvidence","allowedEvidence","tags","rationale"}],
  "discriminatingReads", "lowValueReads", "optimalReadPath", "authoringNotes" }
```
Tags are free lower-case labels describing the trap or role, such as
misleading-signal, leadership-narrative, confounded, protective-control, revision,
gamed-record, joint-cause, external-cause, nonexistent-problem, stop-early.

Before you finish, check your own case as a skeptical reviewer. For every item,
could a careful reader defend a different answer? If so, fix the evidence.
