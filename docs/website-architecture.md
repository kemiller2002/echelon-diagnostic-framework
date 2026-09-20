# EDF Public Website Architecture

Status: Maintainer guide
Last reviewed: 2026-09-20

## Purpose

The public site is the educational interface to EDF. It must teach the working grammar without presenting research hypotheses as established performance.

## Canonical implementation

`static-site/` is the only website implementation.

- HTML is semantic and progressively readable without JavaScript.
- `static-site/styles.css` is the single design stylesheet.
- `public/og.png` is copied into the deployment artifact by the Pages workflow.
- GitHub Pages deploys `static-site/` directly.
- No application framework or client runtime is required.

The prior Next.js/vinext/Cloudflare implementation was removed in September 2026 because it was not part of the deployed system and created a second, drifting source of truth.

## Claim architecture

Public performance claims are downstream projections of:

- `research/registries/hypothesis-registry.json`;
- `research/registries/evidence-registry.json`;
- `docs/framework-engineering/claim-and-confidence-policy.md`.

Material claim elements use:

- `data-claim-id`
- `data-claim-state`

The repository validator checks those attributes against the current hypothesis registry.

Public wording may be weaker than the registry. It may not be stronger.

Historical case pages may retain historical wording for traceability, but current summary pages and the website must reflect later contradictory evidence.

## Information architecture

1. Home: problem framing, grammar, examples, bounded evidence claims.
2. Learn: field distinctions and diagnostic sequence.
3. Examples: validation examples clearly separated from invented teaching cases.
4. Evidence: current claim states, limitations, and source links.
5. Start: guided EDF-0 worksheet.

## Accessibility

- Content remains usable without JavaScript.
- Navigation and headings use semantic HTML.
- Layouts collapse to one column on narrow screens.
- Motion respects `prefers-reduced-motion`.
- Color is not the sole carrier of evidence state.
- Research content remains printable.

## Deployment gate

Before Pages deployment, CI must pass:

```bash
node --test
node scripts/validate-repository.mjs
```

The validator checks broken local references and public claim drift in addition to registry integrity.

## Change rule

A public evidence claim must not be edited in isolation. Update the hypothesis/evidence registries first, then the human-readable ledger, then the public projection.
