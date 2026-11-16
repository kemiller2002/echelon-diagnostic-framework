# Construct, content, criterion and predictive validity of EDF

## Question

Does EDF measure or produce what it claims: genuine organizational problems and their causes, as opposed to convincing explanations built from incomplete information?

## Evidence available

| Validity type | What would establish it | What exists | Status |
|---|---|---|---|
| Content validity | Expert judgement that EDF's fields cover the domain of organizational dysfunction | The baseline audit (`baseline/diagnostic-taxonomy.md`) finds EDF's origin types (technical/human/organizational/environmental/financial/regulatory) were derived from engineering and mission narratives (Apollo, Challenger, 737 MAX). They lack incentives, informal power, stakeholder credibility, metric validity, intentional tradeoffs and "no problem". | **Weak for organizational use.** Gaps identified. |
| Construct validity | EDF's constructs (origin, propagation, control point, sufficiency) behave as distinct, meaningful constructs | Prior: one ablation suggesting Origin Network ≠ Propagation (HY-EDF-007, suggestive). OD1: removing the descriptive constructs (D omits System Context, propagation step and origin taxonomy) did not reduce accuracy. | **Not supported for the descriptive constructs.** Partial support for decision-gate constructs (sufficiency, harm test; D − D_NOGATE +2.8 [0.5, 5.7]). |
| Criterion validity | Agreement of EDF diagnoses with an independent criterion (verified causes) | OD1 synthetic hidden truth: EDF v0.3 item accuracy ≈ 95–98%, the same as a no-framework prompt (B − A +1.4 [−0.7, 3.1]). | **Equivalent to baseline.** EDF-specific criterion validity is not shown. |
| Predictive validity | EDF-chosen interventions predict better outcomes | None. No real-organization outcome data exist. Protocol in `intervention-effectiveness/evaluation-design.md`. | **Untested** |
| Discriminant validity | EDF distinguishes symptom from cause, real from nonexistent problems, internal from external causes | OD1 tags: external-cause recognition 95% (B) vs 95% (A); tradeoff / nonexistent-problem recognition 100% for all conditions held-out. | Same as baseline |

## Conclusions

1. As an **organizational** diagnostic method, EDF v0.3 has no validity evidence beyond what an unstructured frontier-model prompt achieves on the same evidence. That does not make EDF invalid. It means EDF's specific constructs are not yet shown to carry diagnostic information.
2. The literature review (`scientific-foundations/literature-review.md`) finds the same situation for established organizational diagnosis models. 7S, Burke-Litwin, Nadler-Tushman and Weisbord have practitioner adoption but essentially no evidence of diagnostic accuracy or inter-analyst agreement. EDF is not uniquely weak here, but it cannot claim superiority either.
3. Construct validity should be pursued through **behavioural constructs that change decisions**: sufficiency, harm under every hypothesis, discriminating evidence requests. Descriptive labels have not been shown to change decisions.

## Next tests

- **Human-analyst study.** Consultants or managers with and without EDF on real anonymized cases, with blinded expert criterion panels. This is the only way to test whether the vocabulary aids human reasoning and communication, which is the most plausible remaining value of EDF.
- **Construct separability.** Ask independent raters to classify the same statements into EDF fields (origin vs propagation vs control point) and measure agreement (κ).
