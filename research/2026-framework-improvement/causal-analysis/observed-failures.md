# Observed causal-reasoning failures (OD1)

| Pattern (expected → given) | Held-out runs | Example case | Diagnosis |
|---|---|---|---|
| contradicted → association-only | 15 | HO-J2 I7, DEV-I I7 (all 8 dev conditions) | A contrast case refutes the link, but analyzers keep it as a weak association. Under-rejection. |
| association-only → unknown | 12 | Several | Analyzers decline to assert even correlation. Over-caution. |
| association-only → causal | 8 | Confounded-timing edges (HO-B) | Genuine overclaim; most frequent in A and N |
| unknown → association-only | 6 | Thin-evidence edges | Mild overreach |
| causal → association-only | dev: 4 runs on DEV-F I10 | External budget freeze → attrition | Under-claiming when the mechanism is stated but no rival is excluded |

Item wrong in every held-out run: `OD-HO-J2 I7`. All conditions disagreed with the truth key there. This is either a truth-key error or a genuinely ambiguous item, and it is reported in the replication sensitivity analysis.

## Lessons

1. The dominant causal failure is **not** the one EDF's design anticipated (overclaiming a single root cause). It is **failing to reject** refuted links, and boundary indecision between adjacent categories.
2. The causal grading rule in D did not eliminate under-rejection. A contrast test (KT IS / IS-NOT) is the most promising targeted fix and should be tested directly.
3. No method recognized "association-only" perfectly. D (0.97) and C (0.94) were best.
