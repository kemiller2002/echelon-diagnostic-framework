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
- `static-site/` - canonical public website source.

Historical validation artifacts remain in place for traceability. Their original confidence language does not override the current registries.

## Evidence maturity

The repository currently supports EDF as a coherent working representational grammar inside its own research corpus.

It does not yet establish cross-executor reproducibility, incremental value over matched neutral structure, evidence-sensitive updating, better intervention quality, or routine-use speed/learnability. Historical R1 runs also lack enough preserved execution detail for full computational reconstruction.

`EX-EDF-001` is the next confirmatory test of EDF-specific incremental value. Its cases, hidden ground truth, prompts, scoring rules, success criteria, and falsification criteria are preregistered. Confirmatory execution is intentionally blocked until exact analyzer and evaluator provider/model/version/configuration bindings are frozen. The repository does not treat unbound or simulated executors as independent evidence.

See `docs/edf/validation/evidence-ledger.md` for the current claim state and `docs/edf/validation/ex-edf-001-structural-value-benchmark.md` for the benchmark.

## Validation

The repository intentionally has no application framework or runtime dependency.

Run the integrity suite with Node 22 or newer:

```bash
node --test
node scripts/validate-repository.mjs
node scripts/edf-benchmark.mjs validate
```

The checks validate registry referential integrity, evidence paths, internal links, public-claim synchronization, canonical site structure, removal of stale duplicate web stacks, benchmark preregistration integrity, frozen experiment hashes, and confirmatory executor readiness rules.

GitHub Actions runs the same checks on pushes and pull requests. GitHub Pages deploys only after the integrity suite passes.

## Website

The public website is plain HTML and CSS in `static-site/`.

GitHub Pages publishes that directory at the domain root. There is no Next.js, Vite, Cloudflare Worker, JavaScript bundle, framework build step, or repository subfolder in published asset URLs.
