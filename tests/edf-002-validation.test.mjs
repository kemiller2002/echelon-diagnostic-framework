import test from 'node:test';
import assert from 'node:assert/strict';
import { validateSpec, validateCases } from '../scripts/edf-002-validate.mjs';

test('EX-EDF-2026-A003 preregistered specification is internally valid',()=>{
  assert.deepEqual(validateSpec(),[]);
  assert.deepEqual(validateCases(false),[]);
});
