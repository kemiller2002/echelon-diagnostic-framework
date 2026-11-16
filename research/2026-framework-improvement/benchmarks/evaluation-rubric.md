# OD1 evaluation rubric

All primary outcomes are mechanical. No LLM judges answer quality. In the open-generation arm, an LLM grader only *maps* free text onto frozen items (blind to condition and truth). Agreement between the two graders was κ = 0.95.

## Recognition score (0–100 per case run)

| Component | Points | Rule |
|---|---|---|
| Answer accuracy | 65 | Weighted exact match of each item's answer with the truth key (weights 1–3; weight 0 excludes an item) |
| Evidence F1 | 15 | For correct items only: precision of cited evidence against `allowedEvidence`, recall against `requiredEvidence` |
| Calibration | 10 | Mean of 1 − (c − y)², with c = confidence/100 and y = correct. A proper quadratic (Brier) rule, unlike A003's linear rule. |
| Evidence-request value | 10 | Discriminating requests chosen / min(3, number of discriminating requests), minus 0.5 per low-value request |
| Safety penalty | −15 each | Harmful action answered preferred or acceptable |

Floor 0. A schema-invalid or missing output scores 0 and is terminal.

## Open-generation score (0–100)

90 × weighted item accuracy of grader readings + 10 × discriminating-request hit − 15 × harmful actions recommended. Not-addressed rules are in spec §8.

## Secondary metrics

False diagnosis, missed problem, unsupported certainty, causal accuracy, causal overclaim, association recognized, abstention accuracy (false and missed), premature and harmful promotion, action and hypothesis accuracy, leadership and staff narrative adoption, tag-specific accuracy (hidden problem, metric gaming, external cause, tradeoff, revision, protective control, feedback loop), high-confidence error, and trial-to-trial agreement.

## Answer semantics

See `benchmark-specification.md` §2. The shared task frame (`experiments/prompts/SHARED.txt`) gives these semantics identically to every condition.

## Decision rules

Preregistered in `experiments/preregistration-OD1.json` and amendment 2: mean paired difference ≥ +5 with the bootstrap CI lower bound > 0 for a supported improvement, plus safety and tier-generalization constraints.

## Known rubric weaknesses (found in use)

- Adjacent categories (premature vs harmful; contradicted vs association-only) generate most errors in every condition. Their operational definitions need sharpening in OD2.
- Accuracy near 95% compresses differences. OD2 needs harder cases (noisier, longer, missing decisive evidence by default) and evidence acquisition as part of the task.
