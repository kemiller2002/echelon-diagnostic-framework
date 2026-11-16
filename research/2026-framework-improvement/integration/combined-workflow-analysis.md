# Combined EDF + Clarity workflow analysis

## Workflows evaluated

1. **EDF alone** (B_EDF03): 94.5 held-out.
2. **Clarity alone** (CL_ALONE, dev only): 93.5 dev, against 91.4 for A_GENERAL and 93.5 for B_EDF03.
3. **EDF followed by Clarity** (B_CLARITY): 93.7 held-out.
4. **Clarity applied during EDF evidence evaluation** (BI_CLARITY): 94.5 held-out.
5. **Integrated workflow with a variant EDF**: not run, because neither integration improved on its parent condition.

## Costs

| Workflow | Prompt overhead vs no framework | Output size | Analyst steps (human use, estimated) |
|---|---|---|---|
| EDF v0.3 | +5.2k chars | ≈ same | 9 steps + 15 output fields |
| Clarity pass | +1.4k chars | ≈ same | 9 elements |
| EDF + Clarity | +6.7k chars | ≈ same | 18 steps |

Output size did not change: every condition used the same structured output contract.

## Recommended combined workflow

The evidence supports a **lean sequential handoff**:

1. Diagnosis with a short evidence-disciplined method. D-like rules only where they changed decisions: sufficiency including "no intervention", harm under every live hypothesis, discriminating next evidence.
2. Clarity at the **decision record** stage only, for decisions with material consequence or low reversibility: owner, consequence if wrong, reversibility, reassessment trigger.

Do not duplicate Clarity's evidence discipline inside EDF. It added nothing measurable.

## Unmeasured but plausible value

- Decision documentation and accountability: ownership and reassessment dates.
- Organizational learning across repeated decisions.

Both need a longitudinal, real-organization study (see `intervention-effectiveness/evaluation-design.md`).
