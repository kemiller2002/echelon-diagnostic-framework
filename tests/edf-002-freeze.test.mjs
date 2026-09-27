import test from 'node:test';
import assert from 'node:assert/strict';
import path from 'node:path';
import { criticalFiles, manifestFiles } from '../scripts/edf-002-freeze.mjs';

const rel=p=>path.relative(process.cwd(),p).replaceAll('\\','/');
const AMENDMENTS='research/experiments/EX-EDF-2026-A003/amendments/';

test('EX-EDF-2026-A003 freeze manifest covers every pre-freeze amendment',()=>{
  const files=manifestFiles().map(rel);
  const amendments=files.filter(f=>f.startsWith(AMENDMENTS));
  assert.ok(amendments.includes(AMENDMENTS+'EX-EDF-2026-A003-A001-scoring-compatibility.json'));
  assert.ok(amendments.includes(AMENDMENTS+'EX-EDF-2026-A003-A002-a001-reconciliation.json'));
  assert.ok(criticalFiles().map(rel).every(f=>files.includes(f)));
  assert.equal(new Set(files).size,files.length);
});
