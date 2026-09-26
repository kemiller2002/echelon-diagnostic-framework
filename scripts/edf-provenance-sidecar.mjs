// Optional provenance sidecars for future EDF research artifacts
// (claim-and-confidence-policy.md, Independence, v1.1; DF-EDF-2026-A001).
//
// A sidecar records who authored, reviewed, executed, or evaluated one
// artifact, and in which execution. It sits beside the experiment at
// research/experiments/<EX-ID>/provenance/<kind>s/<id>.provenance.json. That
// location is outside every frozen/hashed set and outside every analyzer-,
// reviewer-, and scorer-visible input. No scoring or analysis script reads
// it: identity is provenance for independence checks and stratified
// reporting, never evidence weight. Pure functions only; callers do the I/O.

import crypto from 'node:crypto';
import { executionKeyFrom, validate as validateProvenance, CONTRACT_NAME, CURRENT_VERSION } from './praxis-provenance-record.mjs';
import { validateAgainstSchema } from './json-schema-subset.mjs';

export const SIDECAR_CONTRACT = 'edf.provenance-sidecar';
export const SIDECAR_VERSION = '1.0.0';
export const SYSTEM = 'edf';
export const KIND_DIRECTORIES = Object.freeze({ case: 'cases', review: 'reviews', run: 'runs', evaluation: 'evaluations' });
export const SIDECAR_PATH = /^research\/experiments\/(EX-EDF-[^/]+)\/provenance\/(cases|reviews|runs|evaluations)\/([^/]+)\.provenance\.json$/;

export const sha256 = bytes => crypto.createHash('sha256').update(bytes).digest('hex');
export const subjectFor = target => `${SYSTEM}:${target}`;
export const sidecarPathFor = (experiment, kind, id) => `research/experiments/${experiment}/provenance/${KIND_DIRECTORIES[kind]}/${id}.provenance.json`;

// The key a contribution made inside a run is recorded under: the propagated
// Praxis execution when ROS_EXECUTION_ID is set, else EXE-edf.<run>.
export const executionKey = (env, run) => executionKeyFrom(env, SYSTEM, run);

// Builds a sidecar. `contributions` is a list of
// { key, operations, at, actor, reason?, evidence? }, recorded as given
// (append-only; the codec rejects a second `created`, a late `created`, an
// agent outside an execution, and credentials). Throws rather than writing
// malformed provenance.
export const makeSidecar = ({ experiment, kind, target, targetBytes, contributions }) => {
  const provenance = {
    contract: CONTRACT_NAME,
    version: CURRENT_VERSION,
    subject: subjectFor(target),
    contributions: Object.fromEntries(contributions.map(({ key, operations, at, actor, reason, evidence }) => [key, {
      operations: [...operations],
      at,
      actor,
      ...(reason === undefined ? {} : { reason }),
      ...(evidence === undefined || evidence.length === 0 ? {} : { evidence: [...evidence] })
    }]))
  };
  const reading = validateProvenance(provenance);
  if (reading.kind !== 'current') {
    throw new Error(`refusing to write malformed provenance: ${(reading.problems ?? []).map(item => `${item.field}: ${item.message}`).join('; ')}`);
  }
  return { contract: SIDECAR_CONTRACT, version: SIDECAR_VERSION, experiment, kind, target, targetSha256: sha256(targetBytes), provenance };
};

// Problems with a sidecar at `relativePath`. `readFile(path)` returns a
// Buffer or throws. Empty means valid.
export const sidecarProblems = (schema, relativePath, sidecar, readFile) => {
  const location = SIDECAR_PATH.exec(relativePath);
  if (!location) return [`${relativePath}: sidecars live only at research/experiments/<EX-ID>/provenance/<kind>s/<id>.provenance.json`];
  const schemaProblems = validateAgainstSchema(schema, sidecar);
  if (schemaProblems.length) return schemaProblems.map(item => `${relativePath}: ${item}`);
  const [, experiment, directory] = location;
  const reading = validateProvenance(sidecar.provenance);
  const targetBytes = (() => { try { return readFile(sidecar.target); } catch { return undefined; } })();
  return [
    ...(reading.kind === 'invalid' ? reading.problems.map(item => `provenance.${item.field}: ${item.message}`) : []),
    ...(reading.kind === 'unsupported' || reading.kind === 'unversioned' ? [`provenance: must be a current praxis.provenance-record, got ${reading.kind}`] : []),
    ...(sidecar.experiment !== experiment ? [`experiment ${sidecar.experiment} does not match directory ${experiment}`] : []),
    ...(KIND_DIRECTORIES[sidecar.kind] !== directory ? [`kind '${sidecar.kind}' does not belong in provenance/${directory}/`] : []),
    ...(!sidecar.target.startsWith(`research/experiments/${experiment}/`) || sidecar.target.includes('/provenance/')
      ? ['target must be an artifact of the same experiment, outside provenance/']
      : []),
    ...(sidecar.provenance.subject !== subjectFor(sidecar.target) ? [`provenance.subject must be '${subjectFor(sidecar.target)}'`] : []),
    ...(targetBytes === undefined
      ? [`target ${sidecar.target} does not exist`]
      : sha256(targetBytes) !== sidecar.targetSha256 ? [`target ${sidecar.target} no longer matches targetSha256`] : [])
  ].map(item => `${relativePath}: ${item}`);
};
