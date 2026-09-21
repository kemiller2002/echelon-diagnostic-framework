import test from "node:test";
import assert from "node:assert/strict";
import { validateBenchmark, analyzeBenchmark } from "../scripts/edf-benchmark.mjs";

test("EX-EDF-001 benchmark is frozen and fail-closed before executor binding", () => {
  const result = validateBenchmark();
  assert.deepEqual(result.errors, [], result.errors.join("\n"));
  assert.equal(result.summary.experimentId, "EX-EDF-001");
  assert.equal(result.summary.cases, 6);
  assert.equal(result.summary.conditions, 3);
  assert.ok(result.summary.frozenArtifacts >= 8);
  assert.equal(result.summary.executorMatrixReady, false);
  assert.equal(result.summary.analyzerRuns, 0);
});

test("EX-EDF-001 cannot manufacture a result without complete runs and evaluations", () => {
  const result = analyzeBenchmark();
  assert.equal(result.status, "not-evaluable");
  assert.equal(result.primaryUnits, 0);
  assert.equal(result.requiredPrimaryUnits, 18);
});
