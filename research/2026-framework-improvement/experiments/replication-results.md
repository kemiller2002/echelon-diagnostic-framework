# Independent replication and statistical review

Full report: `experiments/replication/replication-report.md`. Code: `experiments/replication/*.py`, written in Python, standard library only. It was implemented from the specification before the replicator read `harness/Od1.fsx`.

## Verdict

**All headline results replicate.** The independent scorer matches the F# harness on all 320 recognition runs apart from one convention (below). It reproduces every contrast to within about 0.05 points and the open-generation scores exactly. Every preregistered verdict is unchanged.

| Contrast (held-out unless noted) | Harness | Replication |
|---|---|---|
| B − A | +1.40 [−0.73, 3.13] | +1.40 [−0.72, 3.13] |
| B − N | +2.76 [0.78, 5.29] | +2.77 [0.76, 5.33] |
| D − B | +1.49 [−0.33, 3.58] | +1.48 [−0.31, 3.59] |
| D − N | +4.25 [1.57, 7.81] | +4.25 [1.53, 7.90] |
| D − B (Haiku) | +0.14 | +0.13 |
| OG D − B | +0.89 [−4.33, 6.57] | +0.89 [−4.4, 6.5] |

- With multiplicity control (Holm, sign-flip permutation tests), **no contrast is significant**. The smallest adjusted p is 0.095.
- Excluding the one item every held-out run answered differently from the key (HO-J2 I7) changes nothing.
- Accuracy-only scoring (no evidence F1, calibration or request terms) gives D − N +5.26. That is not the preregistered outcome and is reported for transparency only.

## Discrepancies found

| # | Finding | Effect | Disposition |
|---|---|---|---|
| R-1 | The harness weights the calibration term by item weight; spec §5 says a plain mean | ≤ 0.39 points per run, ≤ 0.02 per contrast; flips one tally (D − B 7/5 → 6/6) | **Known deviation**, recorded here. Results are reported as computed. Both conventions give the same conclusions. |
| R-2 | The harness treats a run as valid if any answers exist; it defaults missing confidence to 50 | No run triggers it | Recorded |
| R-3 | Two Haiku N runs wrote request IDs as "Q1: title"; both scorers give 0 | Haiku N − A moves from −7.04 to −5.37 if parsed leniently | Conclusion unchanged (N still worst) |
| R-4 | `SHARED_OG.txt` was not in a hash manifest | Written before the first OG run and never edited; committed in `e5b54d6` (10:01 UTC) shortly after the first OG slice launched | Hash added retroactively to `frozen-prompts-heldout.sha256`, labeled as retroactive |

## Threats identified by the replication

| Threat | Assessment | Handling |
|---|---|---|
| **Ceiling.** B averages 94.5, so D − B is capped at about +5.5. The +5 thresholds were nearly unattainable. | Confirmed. Twelve cases give adequate power for +5 (≈ 9 needed at observed SD); the ceiling, not n, limits discrimination. | Central limitation. OD2 must be harder. |
| **Request-menu order leaks the answer.** Discriminating requests are the first k menu items in 18 of 22 cases; always choosing Q1–Q3 earns 0.795 of the request credit, and 119 of 144 held-out runs did. | **Confirmed** (orchestrator re-check: 18/22). | The request component (10 points) is invalid as a measure of choosing evidence by diagnostic value. Contrast conclusions are unchanged because the leak applies to all conditions equally. Evidence-selection value is measured only by the adaptive experiment (content-blind catalogue). OD2 must randomize menu order. |
| **Run-to-run noise.** Trial-to-trial SD is 3.3–4.3 points per case, as large as the effects. Only B and D had two trials. | Confirmed | Single-trial conditions are noisy. Conclusions are stated as "no detectable difference", not "equal". |
| **Single model family.** Case authors, reviewers, analyzers and graders are all Claude-family models. | Confirmed design limitation (no cross-provider credentials) | Reported everywhere. Cross-provider replication is the first item in the continuation manifest. |
| **D's vocabulary overlaps held-out truth rationales** more than dev rationales | Plausible. D's language ("presenting problem as hypothesis", "silence is not health") resembles how Claude-family authors write rationales. | Biases toward D. Since D still did not meet its threshold, the null for H-OD1-5 is conservative. |
| **Batch confound.** Held-out B and D ran in a separate bundle phase. | **Refuted.** Workflow journals show `heldout` (B, D) and `heldout1` (other conditions) runs interleaved inside the same workflow slices (e.g. `wf_f6deb5c8-f04`, `wf_06fdf446-c22`). | No action |
