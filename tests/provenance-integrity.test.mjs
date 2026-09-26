// Provenance is not evidence quality; blinded material carries no provenance
// (RQ-EDF-2026-A001..A006, DF-EDF-2026-A001; Praxis RQ-ROS-2026-A019,
// RQ-ROS-2026-A010, RQ-ROS-2026-A015, DF-ROS-2026-A037).
import test from "node:test";
import assert from "node:assert/strict";
import fs from "node:fs";
import crypto from "node:crypto";
import {
  registryEntryFindings, registryFindings, isBlindedMaterial, blindedMaterialFindings, validateProvenanceIntegrity,
} from "../scripts/provenance-integrity.mjs";
import { classify, appendContribution, addLineage, emptyBlock, preservationViolations } from "../scripts/vendor/praxis/provenance-interchange.mjs";

const fixtureUrl = (name) => new URL(`./fixtures/praxis-provenance/${name}`, import.meta.url);
const fixture = (name) => JSON.parse(fs.readFileSync(fixtureUrl(name), "utf8"));

const agentA = { kind: "agent", id: "openai/codex", provider: "openai", model: "gpt-5-codex", runtime: "codex" };
const agentB = { kind: "agent", id: "anthropic/claude-code", provider: "anthropic", model: "unknown", runtime: "claude-code" };
const human = { kind: "human", id: "kevin" };
const unknownActor = { kind: "unknown", id: "unknown", provider: "unknown", model: "unknown", runtime: "unknown" };
const at = (minute) => `2026-09-26T09:${String(minute).padStart(2, "0")}:00.000Z`;
const created = (key, actor) => appendContribution(emptyBlock(), key, { operations: ["created"], at: at(0), actor }).block;

const evidence = (extra = {}) => ({
  id: "EV-EDF-900", type: "synthetic", title: "t", paths: [], supports: "s", limitations: "l", ...extra,
});
const hypothesis = (extra = {}) => ({
  id: "HY-EDF-2026-A900", statement: "s", state: "suggestive", confidence: "Low", publicState: "open", evidence: [], boundary: "b", ...extra,
});

test("vendored Praxis files match SOURCE.json", () => {
  const source = fixture("SOURCE.json");
  assert.equal(source.repository, "kemiller2002/praxis");
  assert.equal(source.commit, "a42c44e8ae0e6e16fdd513141460b700e5fa6648");
  for (const [relative, expected] of Object.entries(source.files)) {
    const actual = crypto.createHash("sha256").update(fs.readFileSync(fixtureUrl(relative))).digest("hex");
    assert.equal(actual, expected, `${relative} was edited locally`);
  }
});

for (const item of fixture("cases.json").cases) {
  test(`Praxis conformance: ${item.name} is ${item.expect}`, () => {
    const result = classify(item.block);
    assert.equal(result.verdict, item.expect, JSON.stringify(result.problems));
    assert.equal(result.warnings.length, item.warnings);
  });
}

test("legacy registry entries without provenance stay valid", () => {
  assert.deepEqual(registryEntryFindings(evidence(), "evidence-registry.json"), []);
  assert.deepEqual(registryEntryFindings(hypothesis(), "hypothesis-registry.json"), []);
});

test("an inbound Praxis block is accepted as an entry's provenance and survives a JSON round trip", () => {
  const block = created("EXE-20260926T090000000Z-aaaa0001", agentA);
  const entry = JSON.parse(JSON.stringify(evidence({ provenance: block })));
  assert.deepEqual(entry.provenance, block);
  assert.equal(classify(entry.provenance).verdict, "supported");
  assert.deepEqual(registryEntryFindings(entry, "evidence-registry.json"), []);
});

test("multiple contributors, two executions of one agent, human, automation, and unknown are all provenance", () => {
  let block = created("EXE-20260926T090000000Z-aaaa0001", agentA);
  const steps = [
    ["EXE-20260926T091000000Z-aaaa0002", { operations: ["modified"], at: at(10), actor: agentA }],
    ["EXT-dokimos.run-7", { operations: ["measured"], at: at(20), actor: agentB }],
    ["CTB-kevin", { operations: ["reviewed"], at: at(30), actor: human }],
    ["EXT-aegis.op-3", { operations: ["transformed"], at: at(40), actor: { kind: "automation", id: "echelon/aegis", provider: "unknown", model: "unknown", runtime: "unknown" } }],
    ["EXT-op.op-9", { operations: ["validated"], at: at(50), actor: unknownActor }],
  ];
  for (const [key, contribution] of steps) {
    const next = appendContribution(block, key, contribution);
    assert.ok(next.ok, JSON.stringify(next));
    assert.deepEqual(preservationViolations(block, next.block), []);
    block = next.block;
  }
  const derived = addLineage(block, ["EV-EDF-009", "praxis:RQ-ROS-2026-A019"]);
  assert.deepEqual(derived.derivedFrom, ["EV-EDF-009", "praxis:RQ-ROS-2026-A019"]);
  assert.deepEqual(preservationViolations(block, derived), []);
  block = derived;
  assert.equal(classify(block).verdict, "supported");
  assert.deepEqual(registryEntryFindings(hypothesis({ provenance: block }), "hypothesis-registry.json"), []);
});

test("identical claims with different actors produce identical claim findings (identity is not weight)", () => {
  const byA = hypothesis({ provenance: created("EXE-20260926T090000000Z-aaaa0001", agentA) });
  const byB = hypothesis({ provenance: created("EXE-20260926T090000000Z-bbbb0001", agentB) });
  const byHuman = hypothesis({ provenance: created("CTB-kevin", human) });
  const { provenance: _a, ...claimA } = byA;
  const { provenance: _b, ...claimB } = byB;
  assert.deepEqual(claimA, claimB);
  for (const entry of [byA, byB, byHuman]) assert.deepEqual(registryEntryFindings(entry, "hypothesis-registry.json"), []);
});

test("identity fields outside the provenance block are rejected", () => {
  const cases = [
    hypothesis({ confidence: { label: "High", provider: "anthropic" } }),
    hypothesis({ strength: { inputs: [{ model: "gpt-5" }] } }),
    evidence({ authorAgent: "codex" }),
    evidence({ executorFamily: "openai" }),
    hypothesis({ evidenceWeights: [{ actor: agentA, weight: 2 }] }),
  ];
  for (const entry of cases) {
    assert.ok(registryEntryFindings(entry, "registry").some((finding) => finding.includes("identity field")), JSON.stringify(entry));
  }
});

test("provenance nested inside a claim or confidence field is rejected", () => {
  const block = created("EXE-20260926T090000000Z-aaaa0001", agentA);
  const findings = registryEntryFindings(hypothesis({ confidence: "Low", support: { provenance: block } }), "registry");
  assert.ok(findings.some((finding) => finding.includes("top-level 'provenance' block")));
  assert.ok(registryEntryFindings(hypothesis({ confidence: { value: "High" } }), "registry").some((finding) => finding.includes("must be a label")));
});

test("a malformed provenance block is rejected, another major is carried verbatim", () => {
  const malformed = { schema: "praxis.provenance/1", contributions: { "EXE-1": { operations: ["created"], at: "yesterday", actor: agentA } } };
  assert.ok(registryEntryFindings(evidence({ provenance: malformed }), "registry").some((finding) => finding.includes("malformed provenance")));
  const future = { schema: "praxis.provenance/2", anything: { goes: true } };
  assert.equal(classify(future).verdict, "unsupported");
  assert.deepEqual(registryEntryFindings(evidence({ provenance: future }), "registry"), []);
});

test("registryFindings walks the named collection", () => {
  const document = { hypotheses: [hypothesis(), hypothesis({ id: "HY-EDF-2026-A901", provider: "openai" })] };
  const findings = registryFindings(document, "hypothesis-registry.json", "hypotheses");
  assert.equal(findings.length, 1);
  assert.match(findings[0], /HY-EDF-2026-A901/);
});

test("blinded material is recognized by path", () => {
  for (const blinded of [
    "research/experiments/EX-EDF-2026-A003/cases/ST-001.case.json",
    "research/experiments/EX-EDF-001/cases.json",
    "research/experiments/EX-EDF-2026-A003/prompt-modules.json",
    "research/experiments/EX-X/analyzer-packets/run-1.txt",
    "research/experiments/EX-X/runs/ST-001.packet.json",
  ]) assert.ok(isBlindedMaterial(blinded), blinded);
  for (const open of [
    "research/experiments/EX-EDF-2026-A003/cases/ST-001.truth.json",
    "research/experiments/EX-EDF-2026-A003/preregistration.json",
    "research/experiments/EX-EDF-2026-A003/manual-runs/slot/executor.json",
    "research/registries/evidence-registry.json",
  ]) assert.ok(!isBlindedMaterial(open), open);
});

test("provenance or author identity in blinded material is rejected", () => {
  const block = created("EXE-20260926T090000000Z-aaaa0001", agentA);
  const leaks = [
    ["x/ST-900.case.json", { id: "ST-900", provenance: block }],
    ["x/ST-900.case.json", { id: "ST-900", evidence: [{ id: "E1", provenance: {} }] }],
    ["x/ST-900.case.json", { id: "ST-900", authorFamily: "OpenAI" }],
    ["x/ST-900.case.json", { id: "ST-900", meta: { schema: "praxis.provenance/1" } }],
    ["x/run.packet.json", { executor: "anthropic" }],
  ];
  for (const [relative, document] of leaks) {
    assert.notDeepEqual(blindedMaterialFindings(relative, JSON.stringify(document)), [], JSON.stringify(document));
  }
  assert.notDeepEqual(blindedMaterialFindings("x/analyzer-packets/a.txt", "provenance:\n  contributions: {}\n"), []);
  assert.deepEqual(blindedMaterialFindings("x/ST-900.case.json", JSON.stringify({ id: "ST-900", entities: [{ provider: "cloud", model: "db-7" }] })), []);
});

test("the repository passes: frozen registries and blinded cases carry no provenance leak", () => {
  const result = validateProvenanceIntegrity();
  assert.deepEqual(result.errors, [], result.errors.join("\n"));
  assert.ok(result.summary.blindedFilesChecked >= 6);
});
