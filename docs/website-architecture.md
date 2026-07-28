# EDF Public Website Architecture

Status: Maintainer guide  
Last reviewed: 2026-07-28

## Purpose

The public site is an educational interface to EDF, not a marketing funnel or
a substitute for the source research. It helps a new visitor recognize
premature certainty, understand EDF's diagnostic grammar, and inspect the
evidence state behind public claims.

## Information architecture

The current release is a focused, single-route learning path:

1. Establish the problem: organizations often act before they understand.
2. Contrast default problem-solving with EDF.
3. Teach the seven public-facing diagnostic moves interactively.
4. Demonstrate origin-versus-control-point reasoning with Challenger.
5. Separate supported, provisional, and open claims.
6. Show application domains without promising validated outcomes.
7. Give the visitor a small, responsible first action.

This sequence favors five-minute comprehension over a large documentation
tree. Future routes should be added only when their content justifies
independent navigation, beginning with `/framework`, `/examples`, and
`/research`.

## Claim and evidence policy

Public copy must preserve these boundaries:

`Observation → Evidence → Hypothesis → Interpretation → Theory → Public claim`

Claims should link to the frozen specification, validation findings, or
evidence ledger. Provisional findings must remain visibly provisional.
Limitations should appear beside the claims they qualify. A new validation
result does not silently change a frozen EDF specification.

## Component and technical architecture

- `app/page.tsx` owns the narrative order and server-rendered content.
- `app/DiagnosticFlow.tsx` is the only client component. It implements the
  interactive diagnostic grammar with accessible tabs and keyboard navigation.
- `app/globals.css` owns the compact design system, responsive behavior,
  automatic dark mode, reduced-motion handling, and print rules.
- `app/layout.tsx` owns canonical metadata and host-derived social metadata.

Keep JavaScript limited to interactions that materially improve learning.
Prefer semantic HTML and CSS over visual dependencies.

## Design rationale

The visual language uses warm paper, near-black green, evidence orange, and a
high-visibility chartreuse accent. Monospace labels signal method, state, and
traceability; sans-serif text carries explanatory prose; italic serif text
marks reflective questions. Borders and numbered sequences make reasoning
inspectable without implying false certainty.

## Accessibility decisions

- The learning sequence remains meaningful without JavaScript.
- The diagnostic flow supports Arrow keys, Home, End, Tab, and pointer input.
- Focusable controls expose selected state and panel relationships.
- Layouts collapse to one column on narrow screens.
- Motion respects `prefers-reduced-motion`.
- Color is never the only carrier of evidence status.
- Research content has print-specific presentation.

## Search, sharing, and maintenance

Metadata leads with the framework's purpose. Social metadata uses the request
host so preview URLs remain correct across deployments. Future research and
example routes should use descriptive titles and direct links to source
artifacts.

Before changing a public claim, compare it against the frozen specification,
evidence ledger, and reproducibility findings. Test the production build after
content or component changes. Review interactions with keyboard-only input and
at a narrow viewport. Preserve the existing hosting project identifier.

## Roadmap

1. Expand the framework into stage-level reference pages.
2. Publish complete, source-linked worked examples.
3. Build a research index for accepted, rejected, and open hypotheses.
4. Add application guides only as domain evidence matures.
5. Test comprehension and usability with independent human participants.
6. Revisit information architecture after real navigation evidence exists.

The highest-priority unresolved risk is empirical: current evidence is produced
within the research program and has not yet been validated with independent
human analysts.
