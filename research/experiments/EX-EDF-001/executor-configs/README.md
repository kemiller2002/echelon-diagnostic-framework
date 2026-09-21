# EX-EDF-001 Executor Binding

The benchmark includes a zero-dependency provider adapter for OpenAI, Anthropic, and Google Gemini text-generation APIs.

The adapter does not contain credentials and does not enable tools, web access, repository access, or retrieval for the model. It receives the benchmark prompt on standard input and returns only the model text on standard output. Provider usage and returned-model metadata are written as a machine-readable `EDF_ADAPTER_META` line on standard error and preserved in the run bundle.

## Credentials

Supply credentials only through the local process environment:

- OpenAI: `OPENAI_API_KEY`
- Anthropic: `ANTHROPIC_API_KEY`
- Google Gemini: `GEMINI_API_KEY`

Never add secret values to executor configuration files or the repository.

## Bind an executor

Use the binding helper with the exact model identifier and a version description sufficient to identify the intended execution condition.

```bash
node scripts/edf-bind-executor.mjs \
  --slot A1 \
  --provider OpenAI \
  --model <exact-model-id> \
  --version <version-or-resolution-description>
```

Analyzer provider families are fixed by the preregistration:

- A1: OpenAI
- A2: Anthropic
- A3: Google

E1 and E2 must use different provider families. E3 remains optional until adjudication is required.

The binding helper:

1. creates `executor-configs/<slot>.json`;
2. pins the provider adapter Git blob SHA inside that config;
3. computes the config SHA-256;
4. updates `executor-matrix.json`;
5. marks the matrix `confirmatory-ready` only after A1-A3 and E1-E2 are complete;
6. freezes the completed executor matrix in `frozen-files.json`.

## Before the first run

Commit all bindings, run the full repository quality workflow, and inspect the diff. Do not run a confirmatory execution from an uncommitted binding.

A credential/connectivity smoke test should use a separate prompt and must not expose any EX-EDF-001 held-out case or ground truth. Do not use benchmark outputs to tune model selection.

## Execute

Once the committed matrix is confirmatory-ready:

```bash
node scripts/edf-benchmark.mjs run --executor A1 --case SYN-001 --condition edf --replicate 1
```

The randomization file defines the required condition order. The operator should generate the full schedule mechanically rather than choosing condition order manually.

## Evidence boundary

A successful API call is infrastructure evidence, not evidence for HY-EDF-008. Performance interpretation begins only after the full preregistered paired set is complete and blindly evaluated.
