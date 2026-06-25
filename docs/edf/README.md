# Echelon Diagnostic Framework (EDF)

Status: Working draft, Specification v0.3

EDF is a systems understanding framework with a diagnostic grammar.

Its job is to explain how a system produced an outcome and to identify the highest-leverage opportunities for changing future outcomes.

EDF answers:

> What is happening?

Clarity answers:

> What should we do?

EDF establishes confidence in reality before Clarity evaluates decisions.

## Core Promise

Complex outcomes rarely have a single root cause. They emerge from an origin network.

EDF keeps that idea practical by requiring a structured diagnostic sequence and a small set of concepts that improve understanding and action.

EDF also evolves under constitutional rules that define what the framework refuses to compromise on. See [constitution.md](constitution.md).

## v0.3 Update

EDF v0.3 introduces System Context as the opening frame for every analysis.

Under the constitution's version integrity rule, this is a subsequent version rather than a retroactive redefinition of v0.2.

System Context has three fields:

- Primary System
- Context
- Focus

These fields make different professional analyses more comparable without forcing agreement.

## v0.3 Workflow

Every EDF analysis should follow this sequence:

1. Define the System Context
   - Primary System
   - Context
   - Focus
2. Define the Outcome
3. Identify Manifestation(s)
4. Build the Origin Network
5. Trace Propagation
6. Evaluate Evidence
7. Identify Control Points
8. Rank Control Points
9. Determine Diagnostic Sufficiency

## The Five Verbs

EDF uses five verbs as its design test:

- Define
- Observe
- Explain
- Influence
- Validate

Design rule:

> If a new concept does not improve one of the five verbs, it does not belong in EDF.

## Relationship To Clarity

EDF and Clarity are adjacent but different.

- EDF builds understanding of the system, the outcome, the origin network, and control points.
- Diagnostic Calibration optionally improves confidence in that diagnostic model when multiple analyses need to be compared and synthesized.
- Clarity evaluates options, tradeoffs, and decisions once the understanding is good enough.

See [operating-cycle.md](operating-cycle.md) for the full handoff.

## In This Section

| Document | Purpose |
|----------|---------|
| [specification.md](specification.md) | Authoritative EDF v0.3 specification |
| [constitution.md](constitution.md) | Governance rules for how EDF evolves and what it refuses to compromise on |
| [diagnostic-calibration.md](diagnostic-calibration.md) | Optional collaborative protocol for calibrating multiple EDF analyses |
| [principles.md](principles.md) | Guiding principles and design constraints |
| [model.md](model.md) | Definitions of the core EDF concepts |
| [complexity-levels.md](complexity-levels.md) | EDF-0, EDF-1, and EDF-2 usage guidance |
| [operating-cycle.md](operating-cycle.md) | How EDF fits into the broader operating loop |
| [examples/challenger.md](examples/challenger.md) | Concise failure example |
| [examples/apollo-11.md](examples/apollo-11.md) | Concise success example |
| [examples/boeing-737-max.md](examples/boeing-737-max.md) | Concise complex systems example |
| [templates/edf-0-quick-card.md](templates/edf-0-quick-card.md) | Rapid template |
| [templates/edf-1-standard.md](templates/edf-1-standard.md) | Standard template |
| [templates/edf-2-complex.md](templates/edf-2-complex.md) | Complex template |

## Starting Point

Use the smallest EDF level that reaches diagnostic sufficiency:

- [EDF-0](complexity-levels.md#edf-0-rapid)
- [EDF-1](complexity-levels.md#edf-1-standard)
- [EDF-2](complexity-levels.md#edf-2-complex)

Use [diagnostic-calibration.md](diagnostic-calibration.md) only when the situation benefits from comparing and synthesizing multiple EDF analyses.
