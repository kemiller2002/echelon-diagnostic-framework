# Non-Human Experimental Failure-Mode Taxonomy

Frequency is record prevalence, not event frequency; poor reporting may hide occurrences.

| ID | Failure mode | Affected experiments | Recorded prevalence | Severity | Detectability | Likely cause | Causal confidence | Existing mitigation | Effectiveness | Next test |
|---|---|---|---:|---|---|---|---|---|---|---|
| FM-001 | Raw-output erasure by synthesis | All 12 | 12/12 | Critical | High | Reports preserve conclusions, not executions | High | Evidence Ledger | Low | Require immutable run bundles |
| FM-002 | Unverifiable precision | VC-001R1, VC-003R1, VC-008R1 | 3/12 | High | High | Percentages lack rubric/calculation provenance | High | FCR metadata | Low | Recompute from blinded raw outputs |
| FM-003 | Shared-evaluator dependence | R1 and CFCV families | ≥9/12 | Critical | Medium | One research system authors and judges outputs | Medium-high | Multiple named lenses | Low; lenses are not independent evaluators | Multi-model blind evaluator design |
| FM-004 | Baseline simulation | CFCV-001/2/3 | 3/12 | High | High | Comparator outputs are reconstructed narratively | High | Capability matrices | Low | Execute comparator methods |
| FM-005 | Prompt/configuration loss | All 12 | 12/12 | Critical | High | No execution bundle standard | High | FCR records version names | Low | Hash prompts/configs and store manifests |
| FM-006 | Planned capability promoted to strength | Comparative and ledger claims | Multiple documents | High | Medium | Architecture and performance conflated | High | Status labels | Medium | Automated claim-evidence status checks |
| FM-007 | Case-selection bias | All cases | 12/12 | High | Low | No sampling frame or negative controls | Medium | Diverse narratives | Low | Preregistered task suite |
| FM-008 | Negative-run invisibility | All 12 | 12/12 have no run ledger | High | Low | Result-centered documentation | Medium-high | Some deferred insights | Low | Append-only run manifest |
| FM-009 | Construct/metric mismatch | R1 family | ≥5/12 | High | Medium | Structural overlap labeled reproducibility | High | RC-7 definition | Medium | Separate format, semantic, and correctness metrics |
| FM-010 | Dynamic-state compression | VC-002, VC-002R1 | 2/12 | Medium | High | Static output schema | Medium | Deferred insight | Not tested | Multi-step evidence-release benchmark |
| FM-011 | Method-scope overclaim | CFCV family | 3/12 | Medium | High | Taxonomic comparison read as effectiveness | High | “Complementary” framing | Medium | Controlled performance comparison |
| FM-012 | Build-environment under-specification | Repository validation | 1 observed failure | Medium | High | Required env variables live only in README example | High | README command | Medium | Add validation script with explicit checks |

## Highest-risk interaction

FM-001, FM-003, FM-005, and FM-009 compound: without raw outputs and configurations, a shared evaluator can assign structural-agreement percentages that cannot be reproduced, while the resulting score is called reproducibility. The next experiment must break this chain simultaneously.

## Stop/mitigation rule

No future non-human experiment should be accepted as confirmatory unless it preserves every attempted run, exact inputs, configuration, model identifier, raw outputs, evaluator outputs, and an executable scoring procedure.

