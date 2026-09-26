// Provenance integrity checks (RQ-EDF-2026-A001..A006, DF-EDF-2026-A001).
//
// 1. Actor identity is provenance, never evidence quality (Praxis
//    RQ-ROS-2026-A019, RQ-ROS-2026-A010): in the claim registries an identity
//    field may appear only inside an entry's top-level Praxis `provenance`
//    block, never as a field that could feed claim state, confidence, or
//    strength.
// 2. A registry entry's `provenance` block must be a well-formed
//    praxis.provenance/1 block (or another major, carried verbatim)
//    (RQ-ROS-2026-A015). Malformed blocks are rejected.
// 3. Blinded analyzer-facing material (*.case.json, analyzer packets, and the
//    analyzer inputs of an experiment) never carries provenance or author /
//    executor identity.
//
// Pure functions take parsed documents; `validateProvenanceIntegrity` does the
// file-system reads and never writes.
import fs from "node:fs";
import path from "node:path";
import { classify } from "./vendor/praxis/provenance-interchange.mjs";

/** Fields that name who or what produced something, or which execution did. */
export const IDENTITY_KEYS = Object.freeze(new Set([
  "actor", "actors", "actorKind", "actorId",
  "agent", "agents", "agentId", "agentFamily",
  "author", "authors", "authorAgent", "author_agent", "authorFamily",
  "createdBy", "created_by", "created_by_agent", "owner_agent", "source_author",
  "executor", "executors", "executorFamily", "executorIdentity",
  "execution", "executions", "executionId", "execution_id",
  "provider", "model", "modelVersion", "runtime",
]));

/**
 * Keys that describe the system under diagnosis at least as often as the
 * analyst. An incident case can legitimately give a component a `provider`
 * (cloud or DNS provider), a `model` (hardware or ML model), or a `runtime`.
 * These keys are therefore allowed as keys in blinded material, but their
 * values are still scanned: a provider/model/runtime name of an AI executor
 * (IDENTITY_TEXT_SIGNALS) is rejected wherever it appears.
 */
const SYSTEM_DESCRIPTION_KEYS = new Set(["provider", "model", "modelVersion", "runtime"]);

/** Identity fields forbidden as keys in blinded material. */
export const BLINDED_IDENTITY_KEYS = Object.freeze(new Set(
  [...IDENTITY_KEYS].filter((key) => !SYSTEM_DESCRIPTION_KEYS.has(key)),
));

/**
 * Text signals of provenance or identity, applied to the full text of every
 * blinded file (JSON or not). Mirrors Percepta's ExperimentBlinding
 * (PCT-038), with execution keys matched in any form, not only full EXE ids.
 */
export const IDENTITY_TEXT_SIGNALS = Object.freeze([
  ["provenance block", /^\s*provenance\s*:|"provenance"\s*:|praxis\.provenance\//m],
  ["legacy author field", /\b(author_agent|authorAgent|created_by_agent|owner_agent|source_author)\b/],
  ["execution key", /\b(?:EXE|EXT|CTB)-[A-Za-z0-9][A-Za-z0-9._:-]*/],
  ["actor variable", /\bROS_(?:ACTOR_KIND|ACTOR|TELEMETRY_PROVIDER|TELEMETRY_MODEL|TELEMETRY_RUNTIME|EXECUTION_ID)\b/],
  ["provider/model/runtime name", /\b(?:anthropic|openai|claude|chatgpt|codex|gemini|deepmind|copilot|gpt-\d[\w.-]*)\b/i],
  ["identity field", new RegExp(`(?:^|[\\s{,"'])(?:${[...BLINDED_IDENTITY_KEYS].join("|")})["']?\\s*:`, "m")],
]);

export const REGISTRY_FILES = Object.freeze({
  "research/registries/hypothesis-registry.json": "hypotheses",
  "research/registries/evidence-registry.json": "evidence",
  "research/registries/theory-registry.json": "theories",
});

const PROVENANCE_TAG = /^praxis\.provenance\//;

/** Every (path, key, value) in a JSON value, depth first. */
const entries = (value, at = "") => {
  if (Array.isArray(value)) return value.flatMap((item, index) => entries(item, `${at}[${index}]`));
  if (value && typeof value === "object") {
    return Object.entries(value).flatMap(([key, item]) => {
      const here = at ? `${at}.${key}` : key;
      return [{ path: here, key, value: item }, ...entries(item, here)];
    });
  }
  return [];
};

/** Findings for one claim-registry entry (hypothesis, evidence, or theory). */
export const registryEntryFindings = (entry, label) => {
  const id = entry?.id ?? "(no id)";
  const where = `${label} ${id}`;
  const { provenance, ...claim } = entry ?? {};

  const identity = entries(claim)
    .filter((item) => IDENTITY_KEYS.has(item.key))
    .map((item) => `${where}: identity field '${item.path}' is provenance, not evidence; record it only in the entry's Praxis 'provenance' block`);

  const nested = entries(claim)
    .filter((item) => item.key === "provenance" || (typeof item.value === "string" && PROVENANCE_TAG.test(item.value)))
    .map((item) => `${where}: provenance at '${item.path}' must be the entry's top-level 'provenance' block, never part of a claim, confidence, or strength field`);

  const structured = ["confidence", "state", "publicState"]
    .filter((key) => claim[key] !== undefined && typeof claim[key] !== "string")
    .map((key) => `${where}: '${key}' must be a label, not a structure that could carry inputs`);

  const block = provenance === undefined
    ? []
    : ((result) => result.verdict === "malformed"
      ? [`${where}: malformed provenance block (${result.problems.join("; ")})`]
      : [])(classify(provenance));

  return [...identity, ...nested, ...structured, ...block];
};

/** Findings for a whole registry document. */
export const registryFindings = (document, label, collection) =>
  (document?.[collection] ?? []).flatMap((entry) => registryEntryFindings(entry, label));

/** True for analyzer-facing blinded material (repository-relative path, '/' separators). */
export const isBlindedMaterial = (relativePath) =>
  /\.case\.json$/.test(relativePath)
  || /\.packet\.(json|md|txt)$/.test(relativePath)
  || /(^|\/)analyzer-packets\//.test(relativePath)
  || /^research\/experiments\/[^/]+\/(cases|prompts|prompt-modules|output-contract)\.json$/.test(relativePath);

/** Findings for one blinded file, given its text. */
export const blindedMaterialFindings = (relativePath, text) => {
  const textFindings = IDENTITY_TEXT_SIGNALS
    .map(([name, pattern]) => [name, pattern.exec(text)])
    .filter(([, match]) => match)
    .map(([name, match]) => `${relativePath}: blinded material carries ${name} (${match[0].trim()})`);
  if (!relativePath.endsWith(".json")) return textFindings;
  let document;
  try {
    document = JSON.parse(text);
  } catch (error) {
    return [...textFindings, `${relativePath}: blinded material is not valid JSON (${error.message})`];
  }
  const keyFindings = entries(document).flatMap((item) => [
    ...(item.key === "provenance" ? [`${relativePath}: blinded material carries a provenance block at '${item.path}'`] : []),
    ...(BLINDED_IDENTITY_KEYS.has(item.key) ? [`${relativePath}: blinded material carries identity field '${item.path}'`] : []),
    ...(typeof item.value === "string" && PROVENANCE_TAG.test(item.value) ? [`${relativePath}: blinded material carries a provenance schema tag at '${item.path}'`] : []),
  ]);
  return [...textFindings, ...keyFindings];
};

const SKIPPED_DIRECTORIES = new Set([".git", "node_modules", ".ros", ".echelon"]);

const walk = (root, directory = root) =>
  fs.readdirSync(directory, { withFileTypes: true }).flatMap((entry) => {
    const full = path.join(directory, entry.name);
    if (entry.isDirectory()) return SKIPPED_DIRECTORIES.has(entry.name) ? [] : walk(root, full);
    return [path.relative(root, full).split(path.sep).join("/")];
  });

/** Read-only repository check. */
export function validateProvenanceIntegrity(root = process.cwd()) {
  const registryErrors = Object.entries(REGISTRY_FILES).flatMap(([relative, collection]) => {
    const full = path.join(root, relative);
    if (!fs.existsSync(full)) return [];
    try {
      return registryFindings(JSON.parse(fs.readFileSync(full, "utf8")), path.basename(relative), collection);
    } catch (error) {
      return [`Cannot parse ${relative}: ${error.message}`];
    }
  });
  const blinded = walk(root).filter(isBlindedMaterial);
  const blindedErrors = blinded.flatMap((relative) => blindedMaterialFindings(relative, fs.readFileSync(path.join(root, relative), "utf8")));
  return {
    errors: [...registryErrors, ...blindedErrors],
    summary: { blindedFilesChecked: blinded.length },
  };
}
