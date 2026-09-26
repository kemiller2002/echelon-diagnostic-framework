// Optional provenance sidecars (DF-EDF-2026-A001): they validate, they live
// outside every frozen/hashed set and every analyzer-visible input, nothing
// that scores or analyzes reads them, and no frozen artifact changed.
import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import { actor } from '../scripts/praxis-provenance-record.mjs';
import {
  SIDECAR_PATH, executionKey, makeSidecar, sha256, sidecarPathFor, sidecarProblems
} from '../scripts/edf-provenance-sidecar.mjs';

const ROOT = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '..');
const rel = full => path.relative(ROOT, full).split(path.sep).join('/');
const readFile = relative => fs.readFileSync(path.join(ROOT, relative));
const readJson = relative => JSON.parse(readFile(relative).toString('utf8'));
const exists = relative => fs.existsSync(path.join(ROOT, relative));
const walk = dir => !fs.existsSync(dir) ? [] : fs.readdirSync(dir, { withFileTypes: true }).flatMap(entry =>
  entry.isDirectory() ? (['.git', 'node_modules'].includes(entry.name) ? [] : walk(path.join(dir, entry.name))) : [path.join(dir, entry.name)]);
const schema = readJson('schemas/edf-provenance-sidecar.schema.json');

const EXPERIMENT = 'EX-EDF-2026-A003';
const REVIEW = `research/experiments/${EXPERIMENT}/reviews/ST-001.review.json`;
const reviewSidecar = () => makeSidecar({
  experiment: EXPERIMENT,
  kind: 'review',
  target: REVIEW,
  targetBytes: readFile(REVIEW),
  contributions: [{
    key: executionKey({}, 'review-ST-001-r1'),
    operations: ['created'],
    at: '2026-10-01T12:00:00.000Z',
    actor: actor({ kind: 'agent', provider: 'example-provider', runtime: 'example-runtime' }),
    reason: 'Opposite-family case review'
  }]
});

test('a sidecar built from declared identity validates at its place', () => {
  const sidecar = reviewSidecar();
  const where = sidecarPathFor(EXPERIMENT, 'review', 'ST-001');
  assert.equal(where, `research/experiments/${EXPERIMENT}/provenance/reviews/ST-001.provenance.json`);
  assert.deepEqual(sidecarProblems(schema, where, sidecar, readFile), []);
  assert.deepEqual(Object.values(sidecar.provenance.contributions)[0].actor,
    { kind: 'agent', id: 'example-provider/example-runtime', provider: 'example-provider', model: 'unknown', runtime: 'example-runtime' });
});

test('malformed, misplaced, or evidence-bearing sidecars are rejected, not dropped', () => {
  const base = reviewSidecar();
  const where = sidecarPathFor(EXPERIMENT, 'review', 'ST-001');
  const [key] = Object.keys(base.provenance.contributions);
  const contribution = base.provenance.contributions[key];
  const rejected = {
    'weight smuggled into the sidecar': [where, { ...base, weight: 2 }],
    'confidence smuggled into the sidecar': [where, { ...base, confidence: 'High' }],
    'score smuggled into the sidecar': [where, { ...base, score: 90 }],
    'inside a hashed directory': [`research/experiments/${EXPERIMENT}/reviews/ST-001.provenance.json`, base],
    'wrong kind directory': [sidecarPathFor(EXPERIMENT, 'run', 'ST-001'), base],
    'target bytes changed': [where, { ...base, targetSha256: '0'.repeat(64) }],
    'subject names another artifact': [where, { ...base, provenance: { ...base.provenance, subject: 'edf:elsewhere' } }],
    'agent without a model': [where, { ...base, provenance: { ...base.provenance, contributions: { [key]: { ...contribution, actor: { kind: 'agent', id: 'x', provider: 'p', runtime: 'r' } } } } }],
    'agent outside an execution': [where, { ...base, provenance: { ...base.provenance, contributions: { 'CTB-20261001-aaaa': contribution } } }],
    'unsupported Praxis major': [where, { ...base, provenance: { ...base.provenance, version: '2.0.0' } }]
  };
  for (const [name, [relative, sidecar]] of Object.entries(rejected)) assert.notDeepEqual(sidecarProblems(schema, relative, sidecar, readFile), [], name);
  assert.throws(() => makeSidecar({ ...base, targetBytes: Buffer.from(''), contributions: [
    { key: 'EXE-edf.a', operations: ['created'], at: '2026-10-01T12:00:00.000Z', actor: actor({ kind: 'human', id: 'a' }) },
    { key: 'EXE-edf.b', operations: ['created'], at: '2026-10-01T13:00:00.000Z', actor: actor({ kind: 'human', id: 'b' }) }
  ] }), /malformed/);
});

test('every sidecar in the repository validates, and none is misplaced', () => {
  const files = walk(path.join(ROOT, 'research')).map(rel).filter(relative => relative.endsWith('.json'));
  for (const relative of files.filter(relative => SIDECAR_PATH.test(relative))) {
    assert.deepEqual(sidecarProblems(schema, relative, readJson(relative), readFile), [], relative);
  }
  const misplaced = walk(ROOT).map(rel)
    .filter(relative => relative.endsWith('.json') && !relative.startsWith('tests/fixtures/'))
    .filter(relative => { try { return readJson(relative)?.contract === 'edf.provenance-sidecar'; } catch { return false; } })
    .filter(relative => !SIDECAR_PATH.test(relative));
  assert.deepEqual(misplaced, []);
});

// What each experiment hashes when it freezes (see frozen-files.json and
// scripts/edf-002-freeze.mjs). A sidecar path must never be selected.
const A003_FROZEN_SELECTION = [/^research\/experiments\/EX-EDF-2026-A003\/cases\/[^/]+\.(case|truth)\.json$/, /^research\/experiments\/EX-EDF-2026-A003\/reviews\/[^/]+\.review\.json$/];

test('sidecar locations are outside every frozen/hashed set and every analyzer-visible input', () => {
  const frozenPaths = [
    ...Object.keys(readJson('research/experiments/EX-EDF-001/frozen-files.json').sha256),
    ...(exists('research/experiments/EX-EDF-2026-A003/frozen-files.json') ? Object.keys(readJson('research/experiments/EX-EDF-2026-A003/frozen-files.json').files) : [])
  ];
  const samples = ['case', 'review', 'run', 'evaluation'].flatMap(kind =>
    ['EX-EDF-001', 'EX-EDF-2026-A003'].map(experiment => sidecarPathFor(experiment, kind, 'ST-001')));
  for (const sample of samples) {
    assert.ok(SIDECAR_PATH.test(sample), sample);
    assert.ok(!frozenPaths.includes(sample), `${sample} is hashed`);
    assert.ok(!A003_FROZEN_SELECTION.some(pattern => pattern.test(sample)), `${sample} would be hashed at freeze`);
    assert.doesNotMatch(sample, /\/EX-[^/]+\/(cases|reviews|runs|evaluations|manual-runs)\//, `${sample} is inside an input directory`);
  }
});

const IDENTITY_KEY = /^(actor|actors|provider|model|runtime|provenance|agentId|executor|authorFamily|reviewerFamily|reviewerModel|author|reviewer)$/i;
const identityKeys = (value, where = '') =>
  Array.isArray(value) ? value.flatMap((item, index) => identityKeys(item, `${where}[${index}]`))
    : value && typeof value === 'object'
      ? Object.entries(value).flatMap(([key, item]) => [...(IDENTITY_KEY.test(key) ? [`${where}.${key}`] : []), ...identityKeys(item, `${where}.${key}`)])
      : [];

test('analyzer-visible inputs carry no actor, author, reviewer, or provenance fields', () => {
  const analyzerVisible = [
    ...fs.readdirSync(path.join(ROOT, 'research/experiments/EX-EDF-2026-A003/cases')).filter(name => name.endsWith('.case.json'))
      .map(name => `research/experiments/EX-EDF-2026-A003/cases/${name}`),
    'research/experiments/EX-EDF-2026-A003/prompt-modules.json',
    'research/experiments/EX-EDF-2026-A003/output-contract.json',
    'research/experiments/EX-EDF-001/cases.json',
    'research/experiments/EX-EDF-001/prompts.json',
    'research/experiments/EX-EDF-001/output-contract.json'
  ];
  assert.ok(analyzerVisible.length >= 7);
  for (const relative of analyzerVisible) {
    assert.deepEqual(identityKeys(readJson(relative)), [], relative);
    assert.doesNotMatch(readFile(relative).toString('utf8'), /\bEXE-[A-Za-z0-9]|praxis\.provenance-record|edf\.provenance-sidecar/, relative);
  }
});

test('nothing that scores, analyzes, or prompts reads provenance sidecars', () => {
  for (const script of ['edf-002-score.mjs', 'edf-002-analyzer.mjs', 'edf-002-validate.mjs', 'edf-002-freeze.mjs', 'edf-002-review.mjs', 'edf-002-case-author.mjs', 'edf-benchmark.mjs']) {
    assert.doesNotMatch(readFile(`scripts/${script}`).toString('utf8'), /provenance|praxis/i, script);
  }
});

// EX-EDF-001's frozen-files.json is itself preregistered; pin it so the
// existing per-file hash check cannot be satisfied by editing both sides.
test('frozen-files.json manifests are unchanged and their hashes still match', () => {
  assert.equal(sha256(readFile('research/experiments/EX-EDF-001/frozen-files.json')), '43955c99f0aa5bc13b9f75be94bd100d12b7473e5c2e435b8d6ebcf8854d51c8');
  const manifests = [
    ['research/experiments/EX-EDF-001/frozen-files.json', manifest => manifest.sha256],
    ['research/experiments/EX-EDF-2026-A003/frozen-files.json', manifest => manifest.files]
  ].filter(([relative]) => exists(relative));
  let checked = 0;
  for (const [relative, files] of manifests) {
    for (const [file, digest] of Object.entries(files(readJson(relative)))) {
      assert.equal(sha256(readFile(file)), digest, file);
      checked++;
    }
  }
  assert.ok(checked >= 7);
});
