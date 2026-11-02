# EDF Public Website Architecture

Status: Maintainer guide
Last reviewed: 2026-11-02

## Purpose

The public site is the educational interface to EDF. It must teach the working grammar without presenting research hypotheses as established performance.

## Canonical implementation

`static-site/` is the only website implementation.

- HTML is semantic, hand-authored and progressively readable without JavaScript.
  The site ships no client script, so no Limen boundary exists
  (`limen.config.json` declares it not applicable).
- Presentation comes from **Forma**, the Echelon Foundry design system
  (`kemiller2002/forma`). `forma.lock` pins the release and the sha256 of
  `forma-echelon-marketing.css` (generic marketing layer plus the Echelon
  theme: tokens, colors, typography, spacing). The asset is installed into
  `static-site/assets/forma/` at build time by
  `kemiller2002/forma/actions/install-presentation@v<version>` and is never
  committed. Keep the action tags equal to the version in `forma.lock`.
- Every page uses the Forma MarketingShell (skip link, `ef-site-header` with
  the primary navigation, `ef-site__main`, inverse `ef-site-footer`) and Forma
  components: hero, section heading, card grid, facts, entry index, steps,
  badges, callouts and CTA. The light Echelon theme is selected with
  `data-ef-theme="light"`.
- `static-site/styles.css` is the local stylesheet. Forma's local-CSS policy
  limits it to content-unique presentation and documented
  `forma-exception` rules awaiting an upstream release; the F# SiteCssPolicy
  and TokenValidator from the pinned Forma release check it in CI.
- `public/og.png` is copied into the deployment artifact by the Pages workflow.
- GitHub Pages deploys `static-site/` directly; URLs, anchors, titles,
  descriptions and Open Graph tags are stable and test-protected.

The prior Next.js/vinext/Cloudflare implementation was removed in September 2026 because it was not part of the deployed system and created a second, drifting source of truth.

### Forma adoption (WI-0012, November 2026)

The site was hand-styled until Forma 0.13.0 (a private palette and type scale
in one minified stylesheet, primary navigation hidden below 800px, no skip
link). It now consumes the pinned Forma release instead. Decisions:

- **Static HTML, no generator.** The pages stay hand-authored; Forma's
  reference marketing site is authored the same way, and a build step would
  add a runtime to a repository that intentionally has none. The F# code is
  the browser conformance suite (`tests/browser`).
- **No Bolero, no client runtime.** Nothing on the site needs behavior beyond
  native HTML.
- **Folio not adopted yet.** The site has no printable report or diagnostic
  output; Forma's print rules keep pages printable. Folio applies when a
  printable EDF worksheet or report is introduced.
- **Defects found in Forma are fixed in Forma.** The migration found that
  Forma 0.13.0 marketing containers do not contain list-item text and leave a
  96px content box in a toned CTA at 320px with 200% text (fixed upstream in
  `kemiller2002/forma#177`, WI-0043). `styles.css` carries the upstream fix as two documented
  `forma-exception` rules until `forma.lock` moves to the release that
  contains it; delete them on that upgrade.

### Upgrading Forma

1. Read Forma's `CHANGELOG.md` for every version between the pin and the target.
2. Edit the version in `forma.lock`, run Forma's `install.sh --update`, and
   update the `@v<version>` action tags and `FORMA_VERSION`/`FORMA_PACKAGE_SHA256`
   in `.github/workflows/quality.yml` and `pages.yml`.
3. Delete any `forma-exception` rule the release makes redundant.
4. Run the checks below and review the browser evidence screenshots.

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
- Navigation and headings use semantic HTML; every page starts with a skip link.
- The primary navigation stays visible and wraps at every width down to 320px.
- Layouts collapse to one column on narrow screens.
- Motion respects `prefers-reduced-motion`.
- Color is not the sole carrier of evidence state: claim states are written
  in badge text.
- Research content remains printable.

## Deployment gate

Before Pages deployment, CI installs the pinned Forma assets, checks the
local stylesheet against the Forma policy, and runs:

```bash
node --test
node scripts/validate-repository.mjs
node scripts/edf-benchmark.mjs validate
```

On every pull request, `.github/workflows/quality.yml` also runs the F#
TokenValidator and the F# browser conformance suite (`tests/browser`, see its
README) at 320, 375, 430, 768 and 1280px, and uploads screenshots and the
report as the `browser-evidence` artifact.

The validator checks broken local references (a reference to an asset pinned
in `forma.lock` is valid before installation), `forma.lock` integrity and
public claim drift in addition to registry integrity.

## Change rule

A public evidence claim must not be edited in isolation. Update the hypothesis/evidence registries first, then the human-readable ledger, then the public projection.
