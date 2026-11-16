# Causal methods for EDF

Full literature: `scientific-foundations/literature/lit-causal.md`. This page summarizes what applies to EDF and what OD1 showed.

| Method | What it contributes | Fit for organizational diagnosis | Recommendation for EDF |
|---|---|---|---|
| Structural causal models / DAGs | Explicit nodes and directed edges, confounders, adjustment logic | Useful as a drawing discipline. Formal identification rarely possible with organizational data. | Adopt **typed edges** (causal / association-only / contradicted / unknown) with explicit confounder notes. Do not require formal DAG identification. |
| Counterfactual reasoning | "Would the outcome differ without X?" | Central to intervention choice | Require one counterfactual sentence per top causal claim |
| Process tracing (hoop, smoking-gun, doubly decisive tests) | Grades how diagnostic each piece of evidence is | High; works on qualitative case evidence | Tag key evidence by test type; prefer smoking-gun or doubly decisive evidence for "causal" |
| Kepner-Tregoe IS / IS-NOT | Contrast cases that refute candidate causes | High; best causal accuracy in OD1 alongside D | Adopt a contrast step: for each candidate cause, where is it present without the effect and vice versa? |
| Bayesian networks | Quantified belief propagation | Requires priors and likelihoods rarely available | Do not adopt as default; false precision risk |
| Fault tree analysis | AND/OR logic of contributing conditions | Useful for joint-sufficiency cases | Allow AND/OR notation in the origin network |
| System dynamics (stocks, flows, loops, delays) | Reinforcing and balancing loops, delays | High for chronic dysfunction (firefighting, staffing spirals) | Allow loop and delay annotation; no simulation required |
| STAMP / CAST | Control structure: controller, feedback, process model | Higher inter-analyst agreement than AcciMap in published comparisons | Control points should name controller, feedback channel and what the controller believed |
| Difference-in-differences / natural experiments | Credible causal estimates from comparisons | When rollouts are staggered or comparison units exist | Next-evidence requests should look for natural comparison groups (e.g., branches with and without a change) |
| Root cause analysis (5 Whys, fishbone) | Simple and widely adopted | Literature documents single-cause bias and weak actions | Do not adopt single-root-cause framing; EDF's origin network already avoids it |

## Causal discovery limitations

Automated causal discovery from observational organizational data is unreliable at realistic sample sizes and with unmeasured confounding. EDF should never present discovered structure as causal without an intervention, a natural experiment or a strong contrast case. OD1's four-way edge scale operationalizes that rule.
