# Echelon Diagnostic Framework

EDF is a working diagnostic grammar for making system diagnoses explicit enough to challenge.

## Repository source of truth

- `docs/edf/specification.md` - frozen v0.3 working baseline.
- `docs/edf/releases/v0.3/` - hash-verified byte-for-byte reconstruction bundle for v0.3 supporting artifacts.
- `docs/edf/README.md` - current framework orientation and evidence boundary.
- `research/registries/hypothesis-registry.json` - current claim states.
- `research/registries/evidence-registry.json` - current evidence records.
- `research/registries/theory-registry.json` - current theory boundary.
- `docs/framework-engineering/claim-and-confidence-policy.md` - rules for interpreting and promoting claims.
- `docs/framework-engineering/validation-protocol-v1.2.md` - protocol for future controlled validation.
- `research/experiments/EX-EDF-001/` - preregistered structural-value benchmark for HY-EDF-008.
- `research/2026-comparative-program/REPORT.md` - OD2 comparative evaluation (current headline evidence).
- `static-site/` - canonical public website source.

Historical validation artifacts remain in place for traceability. Their original confidence language does not override the current registries.

## Evidence maturity

The repository currently supports EDF as a coherent working representational grammar inside its own research corpus.

Three preregistered benchmarks (EX-EDF-001, OD1, OD2) found that **EDF does not improve language-model diagnosis** when a model applies it as a reasoning procedure. OD2 (1,080 runs, 24 held-out cases) found EDF practically equivalent to a general-purpose prompt, to Kepner-Tregoe, and to Kepner-Tregoe plus Analysis of Competing Hypotheses. Each EDF component could be removed without measurable loss, and raising the model's reasoning effort helped more at lower cost (HY-EDF-011, contradicted). These results come from Claude-family analyzers on synthetic cases.

EDF's remaining plausible value is as a format that helps **people** state, review, and challenge a diagnosis. That is untested. The repository also does not establish cross-executor reproducibility, evidence-sensitive updating, better intervention quality, or routine-use speed and learnability. Historical R1 runs lack enough preserved execution detail for full computational reconstruction.

See `docs/edf/validation/evidence-ledger.md` for the current claim state and `research/2026-comparative-program/REPORT.md` for the latest evidence.

## Validation

The repository intentionally has no application framework or runtime dependency.

Run the integrity suite with Node 22 or newer:

```bash
node --test
node scripts/validate-repository.mjs
node scripts/edf-benchmark.mjs validate
```

The checks validate Forma pinning and site structure, registry referential integrity, evidence paths, internal links, public-claim synchronization, canonical site structure, removal of stale duplicate web stacks, benchmark preregistration integrity, frozen experiment hashes, and confirmatory executor readiness rules.

GitHub Actions runs the same checks on pushes and pull requests. GitHub Pages deploys only after the integrity suite passes.

## Website

The public website is plain, hand-authored HTML in `static-site/`, presented with the pinned Echelon Foundry design system, Forma (`forma.lock`). It ships no JavaScript.

GitHub Pages publishes that directory at the domain root after CI installs the pinned Forma assets. There is no Next.js, Vite, Cloudflare Worker, JavaScript bundle, framework build step, or repository subfolder in published asset URLs. See `docs/website-architecture.md`; browser conformance is `tests/browser` (F#).
