import test from "node:test";
import assert from "node:assert/strict";
import { validateRepository } from "../scripts/repository-integrity.mjs";

test("EDF repository integrity", () => {
  const result = validateRepository();
  assert.deepEqual(result.errors, [], result.errors.join("\n"));
  assert.ok(result.summary.hypotheses >= 10);
  assert.ok(result.summary.evidenceRecords >= 6);
  assert.ok(result.summary.checkedLinkFiles > 20);
});
