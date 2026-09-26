// Runs the local Praxis provenance codec (scripts/praxis-provenance-record.mjs)
// against the vendored Praxis conformance fixtures. SOURCE.json pins the
// Praxis commit and a SHA-256 per file; the first test proves the copy has
// not drifted. Every case, successor pair, and end-to-end step is exercised.
import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import { fileURLToPath } from 'node:url';
import * as praxis from '../scripts/praxis-provenance-record.mjs';

const FIXTURES = path.join(path.dirname(fileURLToPath(import.meta.url)), 'fixtures', 'praxis-provenance-record');
const read = file => fs.readFileSync(path.join(FIXTURES, file));
const load = file => JSON.parse(read(file).toString('utf8'));
const walk = dir => fs.readdirSync(dir, { withFileTypes: true }).flatMap(entry =>
  entry.isDirectory() ? walk(path.join(dir, entry.name)) : [path.relative(FIXTURES, path.join(dir, entry.name)).split(path.sep).join('/')]);
const manifest = load('manifest.json');
const outcome = reading =>
  ({ current: 'valid', unversioned: 'valid-unversioned', unsupported: 'unsupported-version', invalid: 'invalid' })[reading.kind];

test('vendored Praxis fixtures match SOURCE.json digests exactly', () => {
  const source = load('SOURCE.json');
  assert.equal(source.repository, 'kemiller2002/praxis');
  assert.equal(source.path, 'schemas/conformance/provenance-record');
  assert.match(source.commit, /^[0-9a-f]{40}$/);
  const present = walk(FIXTURES).filter(file => file !== 'SOURCE.json').sort();
  assert.deepEqual(present, Object.keys(source.files).sort(), 'no fixture added or removed');
  for (const file of present) assert.equal(crypto.createHash('sha256').update(read(file)).digest('hex'), source.files[file], file);
});

test('codec classifies every conformance case as the manifest expects', () => {
  assert.ok(manifest.cases.length >= 28);
  for (const item of manifest.cases) {
    const reading = praxis.validate(load(item.file));
    assert.equal(outcome(reading), item.expect, `${item.file}: ${item.why} ${JSON.stringify(reading.problems ?? [])}`);
  }
});

test('codec judges every successor pair as the manifest expects', () => {
  assert.ok(manifest.successors.length >= 15);
  for (const pair of manifest.successors) {
    const problems = praxis.successorProblems(load(pair.before), load(pair.after));
    assert.equal(problems.length ? 'destructive' : 'preserved', pair.expect, `${pair.before}: ${pair.why} ${JSON.stringify(problems)}`);
  }
});

test('end-to-end chain keeps every actor, execution, and originator', () => {
  for (const step of manifest.e2e.steps) {
    assert.equal(praxis.validate(load(step.file)).kind, 'current', step.file);
    if (step.successorOf) assert.deepEqual(praxis.successorProblems(load(step.successorOf), load(step.file)), [], step.file);
  }
  const final = praxis.validate(load(manifest.e2e.expected.final)).record;
  const links = praxis.chain(final).map(link => ({
    subject: link.subject, key: link.contribution.key, actor: link.contribution.actor.id, operations: [...link.contribution.operations]
  }));
  assert.deepEqual(links, manifest.e2e.expected.chain);
  const origin = praxis.originator(final);
  assert.equal(origin.key, manifest.e2e.expected.originatorOfFinal.key);
  assert.equal(origin.actor.id, manifest.e2e.expected.originatorOfFinal.actor);
});

test('identity comes only from declared environment; unknown stays unknown; runs are namespaced', () => {
  assert.deepEqual(praxis.actorFromEnvironment({}), { kind: 'unknown', id: 'unknown', provider: 'unknown', model: 'unknown', runtime: 'unknown' });
  assert.deepEqual(praxis.actorFromEnvironment({ ROS_ACTOR_KIND: 'human', ROS_ACTOR: 'reviewer-1' }), { kind: 'human', id: 'reviewer-1' });
  assert.deepEqual(praxis.actorFromEnvironment({ GITHUB_ACTIONS: 'true' }),
    { kind: 'automation', id: 'github/github-actions', provider: 'github', model: 'unknown', runtime: 'github-actions' });
  assert.equal(praxis.executionKeyFrom({}, 'edf', 'ST-001-A1-construct-r1'), 'EXE-edf.ST-001-A1-construct-r1');
  assert.equal(praxis.executionKeyFrom({ ROS_EXECUTION_ID: 'EXE-20261001T120000000Z-0a1b2c3d' }, 'edf', 'x'), 'EXE-20261001T120000000Z-0a1b2c3d');
  assert.throws(() => praxis.executionKeyFrom({}, 'EDF', 'x'));
});
