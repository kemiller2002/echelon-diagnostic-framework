// Guard against identity-weighted evidence (claim-and-confidence-policy.md,
// Independence, v1.1; DF-EDF-2026-A001). Author, reviewer, executor, and
// evaluator identity (OpenAI, Anthropic, Google, a human, or unknown) is
// provenance, not evidence quality. The frozen scorer
// (scripts/edf-002-score.mjs) is imported and invoked unmodified. Its scores
// must not move when identity fields in its inputs are added, permuted,
// replaced, or removed. Author-family relabelling may only relabel the
// stratified byAuthor report; it must not change any number.
import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import crypto from 'node:crypto';
import { spawnSync } from 'node:child_process';
import { fileURLToPath, pathToFileURL } from 'node:url';
import { scoreOutput } from '../scripts/edf-002-score.mjs';

const ROOT = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '..');
const EXP = path.join(ROOT, 'research', 'experiments', 'EX-EDF-2026-A003');
const SCORER = path.join(ROOT, 'scripts', 'edf-002-score.mjs');
const readJson = file => JSON.parse(fs.readFileSync(file, 'utf8'));
const unit = seed => parseInt(crypto.createHash('sha256').update(seed).digest('hex').slice(0, 8), 16) / 0xffffffff;

const IDENTITIES = {
  openai: { kind: 'agent', id: 'openai/codex', provider: 'openai', model: 'unknown', runtime: 'codex' },
  anthropic: { kind: 'agent', id: 'anthropic/claude-code', provider: 'anthropic', model: 'unknown', runtime: 'claude-code' },
  google: { kind: 'agent', id: 'google/gemini-cli', provider: 'google', model: 'unknown', runtime: 'gemini-cli' },
  human: { kind: 'human', id: 'analyst-1' },
  unknown: { kind: 'unknown', id: 'unknown', provider: 'unknown', model: 'unknown', runtime: 'unknown' }
};
const FAMILIES = Object.keys(IDENTITIES);

// Every identity-shaped field an input could plausibly carry, top-level and nested.
const decorate = (value, family) => {
  const actor = IDENTITIES[family];
  return {
    ...value,
    actor,
    provider: actor.provider ?? 'not-applicable',
    model: actor.model ?? 'not-applicable',
    executor: { provider: actor.provider ?? 'human', model: actor.model ?? 'n/a', slot: 'hidden' },
    reviewerFamily: family,
    authorFamily: family,
    provenance: {
      contract: 'praxis.provenance-record', version: '1.0.0',
      contributions: { 'EXE-edf.synthetic': { operations: ['created'], at: '2026-10-01T00:00:00.000Z', actor } }
    }
  };
};
const strip = ({ actor, provider, model, executor, reviewerFamily, provenance, ...rest }) => rest;

const realPairs = () => fs.readdirSync(path.join(EXP, 'cases')).filter(name => name.endsWith('.case.json')).sort()
  .map(name => {
    const id = name.replace('.case.json', '');
    return { id, caseDoc: readJson(path.join(EXP, 'cases', name)), truth: readJson(path.join(EXP, 'cases', `${id}.truth.json`)) };
  });

// A deterministic, imperfect analyzer output: some answers right, some wrong,
// varied confidence and evidence, so every scoring term is exercised.
const syntheticOutput = (caseDoc, truth, seed) => {
  const truthBy = new Map(truth.answers.map(item => [item.itemId, item]));
  return {
    caseId: caseDoc.id,
    answers: caseDoc.scoredItems.map((item, index) => {
      const expected = truthBy.get(item.id);
      const correct = unit(`${seed}|${item.id}|correct`) > 0.35;
      const wrong = item.allowedAnswers.find(answer => answer !== expected.expected) ?? expected.expected;
      return {
        itemId: item.id,
        answer: correct ? expected.expected : wrong,
        confidence: Math.round(unit(`${seed}|${item.id}|confidence`) * 100),
        evidenceIds: correct ? [...(expected.requiredEvidence ?? [])] : [...(expected.allowedEvidence ?? []).slice(0, 1), caseDoc.evidence[index % caseDoc.evidence.length].id]
      };
    })
  };
};

test('scoreOutput is invariant to identity fields in case, truth, and output', () => {
  const pairs = realPairs();
  assert.ok(pairs.length >= 6, 'real EX-EDF-2026-A003 case/truth pairs are available');
  for (const { id, caseDoc, truth } of pairs) {
    const output = syntheticOutput(caseDoc, truth, id);
    for (const criticalOnly of [true, false]) {
      const baseline = scoreOutput(caseDoc, strip(truth), output, criticalOnly);
      assert.ok(baseline.itemCount > 0);
      for (const family of FAMILIES) {
        const decoratedOutput = { ...decorate(output, family), answers: output.answers.map(answer => decorate(answer, family)) };
        assert.deepEqual(scoreOutput(caseDoc, decorate(truth, family), decoratedOutput, criticalOnly), baseline, `${id} ${family}`);
        assert.deepEqual(scoreOutput(decorate(caseDoc, family), truth, output, criticalOnly), baseline, `${id} case ${family}`);
      }
    }
  }
});

// ---- analyzeRuns end-to-end, in an isolated copy of the experiment layout ------

const SLOTS = ['A1', 'A2'];
const CONDITIONS = ['neutral', 'construct'];

// Twelve cases: the real pairs plus relabelled copies, as the preregistered
// design needs 24 primary pairs.
const twelveCases = () => {
  const real = realPairs();
  return Array.from({ length: 12 }, (_, index) => {
    const source = real[index % real.length];
    const id = index < real.length ? source.id : `${source.id}-copy${index}`;
    return { id, caseDoc: { ...source.caseDoc, id }, truth: { ...source.truth, caseId: id } };
  });
};

const writeExperiment = (directory, { runIdentity, authorFamily }) => {
  const exp = path.join(directory, 'research', 'experiments', 'EX-EDF-2026-A003');
  fs.mkdirSync(path.join(exp, 'cases'), { recursive: true });
  fs.mkdirSync(path.join(exp, 'runs'), { recursive: true });
  fs.copyFileSync(path.join(EXP, 'scoring-spec.json'), path.join(exp, 'scoring-spec.json'));
  for (const { id, caseDoc, truth } of twelveCases()) {
    fs.writeFileSync(path.join(exp, 'cases', `${id}.case.json`), JSON.stringify(caseDoc));
    fs.writeFileSync(path.join(exp, 'cases', `${id}.truth.json`), JSON.stringify({ ...truth, authorFamily: authorFamily(truth.authorFamily) }));
    for (const [slotIndex, slot] of SLOTS.entries()) for (const condition of CONDITIONS) {
      const runId = `${id}-${slot}-${condition}-r1`;
      const run = { version: '1.0.0', runId, caseId: id, analyzerSlot: slot, condition, replicate: 1, status: 'valid-output' };
      fs.writeFileSync(path.join(exp, 'runs', `${runId}.json`), JSON.stringify(runIdentity(run, slotIndex)));
      fs.writeFileSync(path.join(exp, 'runs', `${runId}.output.json`), JSON.stringify(syntheticOutput(caseDoc, truth, runId)));
    }
  }
};

const analyze = variant => {
  const directory = fs.mkdtempSync(path.join(os.tmpdir(), 'edf-identity-neutral-'));
  try {
    writeExperiment(directory, variant);
    const script = `import { analyzeRuns } from ${JSON.stringify(pathToFileURL(SCORER).href)}; process.stdout.write(JSON.stringify(analyzeRuns()));`;
    const result = spawnSync(process.execPath, ['--input-type=module', '-e', script], { cwd: directory, encoding: 'utf8' });
    assert.equal(result.status, 0, result.stderr);
    return JSON.parse(result.stdout);
  } finally {
    fs.rmSync(directory, { recursive: true, force: true });
  }
};

const same = family => family;
const withIdentity = pick => (run, slotIndex) => decorate(run, pick(slotIndex));

test('analyzeRuns is invariant when executor identity is permuted, replaced, or removed', () => {
  const baseline = analyze({ runIdentity: withIdentity(slot => ['openai', 'anthropic'][slot]), authorFamily: same });
  assert.equal(baseline.status, 'evaluable', JSON.stringify(baseline));
  assert.equal(baseline.pairs, 24);
  const variants = {
    'slots swapped': withIdentity(slot => ['anthropic', 'openai'][slot]),
    'google and human': withIdentity(slot => ['google', 'human'][slot]),
    'all unknown': withIdentity(() => 'unknown'),
    'identity absent': run => run
  };
  for (const [name, runIdentity] of Object.entries(variants)) {
    assert.deepEqual(analyze({ runIdentity, authorFamily: same }), baseline, name);
  }
});

test('relabelling author families only relabels the stratified report', () => {
  const identity = withIdentity(() => 'unknown');
  const baseline = analyze({ runIdentity: identity, authorFamily: same });
  const swap = { OpenAI: 'Anthropic', Anthropic: 'OpenAI' };
  const relabelled = analyze({ runIdentity: identity, authorFamily: family => swap[family] ?? family });
  const { byAuthor, ...numbers } = baseline;
  const { byAuthor: relabelledByAuthor, ...relabelledNumbers } = relabelled;
  assert.deepEqual(relabelledNumbers, numbers, 'no score, difference, interval, or count moves');
  assert.deepEqual(
    Object.fromEntries(Object.entries(relabelledByAuthor).map(([family, value]) => [swap[family] ?? family, value])),
    byAuthor,
    'the same values appear under the swapped labels'
  );
});
