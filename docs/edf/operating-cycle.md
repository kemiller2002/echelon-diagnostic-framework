# EDF Operating Cycle

Status: Current interpretation guidance

The v0.3-era operating-cycle artifact is preserved at [releases/v0.3/operating-cycle.md](releases/v0.3/operating-cycle.md).

EDF is one part of a broader operating loop:

```text
Understand -> Decide -> Commit -> Execute -> Understand
```

## Cycle Map

| Stage | Purpose | Primary Framework |
|-------|---------|-------------------|
| Understand | Make the current diagnostic model and its evidence explicit | EDF, optionally with cross-analysis review |
| Decide | Evaluate options and choose a path | Clarity |
| Commit | Assign resources, ownership, authority, and priority | Operating discipline |
| Execute | Change reality | Delivery / operations |
| Understand again | Validate results and learn | EDF |

## Stage Notes

### Understand

Use EDF to define the system, describe the outcome, identify manifestations, build the origin network, trace propagation, evaluate evidence, and rank control points.

When a case is complex, high-stakes, disputed, cross-disciplinary, or ambiguous, Diagnostic Calibration can be used as an experimental cross-analysis protocol. Its use should not be interpreted as validated confidence improvement.

Primary question:

> What is happening?

### Decide

Use Clarity to assess options, assumptions, tradeoffs, and risks.

Primary question:

> What should we do?

### Commit

A decision is not yet execution.

Commit means turning a decision into something real:

- Resources are allocated
- Ownership is assigned
- Authority is clear
- Priority is explicit

### Execute

Execution changes reality.

This is where teams implement changes, run missions, update policy, ship code, retrain operators, or alter governance.

### Understand Again

Return to EDF after execution to validate whether the action changed the system in the expected way and to capture learning.

## Execution Layer

For each action, track:

| Field | Purpose |
|-------|---------|
| Owner | Who is accountable |
| Plan | What will be done |
| Priority | How important it is relative to other work |
| Verification | How we will know the action worked |

This keeps the handoff between diagnosis and action concrete.

## Practical Handshake Between EDF And Clarity

| EDF Output | Why it matters to Clarity |
|------------|---------------------------|
| System and boundary | Clarifies what decision scope actually exists |
| Outcome and manifestations | Defines what is really happening |
| Origin network and propagation | Clarifies what must be addressed |
| Ranked control points | Identifies candidate intervention priorities and their rationale |
| Confidence and unknowns | Shows remaining epistemic uncertainty relevant to the decision |

If Diagnostic Calibration is used, Clarity should consume the cross-reviewed synthesis while preserving unresolved disagreement rather than treating convergence as proof.

See [diagnostic-calibration.md](diagnostic-calibration.md).

## Validation Loop

The final "Understand" is not repetition for its own sake. It answers:

- Did the intervention change the manifestation?
- Did it alter the intended control point?
- Were the assumed origins correct?
- What did we learn for the next cycle?
