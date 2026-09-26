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
//    executor identity: the identity of whoever analysed or produced it, as
//    provenance or telemetry artifacts. Prose about the diagnosed system's
//    own actors, sessions, providers, or logs is not refused. Member names
//    are compared in normalised form (case-insensitive, `_`/`-` removed) at
//    the top level of a case record or inside a provenance, contributions, or
//    actor object; identity signals run on the raw text and on every decoded
//    JSON string; and JSON that repeats a member name within one object is
//    rejected (Praxis contract 1.2 rule 1).
//
// Pure functions take documents or text; `validateProvenanceIntegrity` does the
// file-system reads and never writes.
import fs from "node:fs";
import path from "node:path";
import { classify, IDENTITY_ENVIRONMENT_VARIABLES } from "./vendor/praxis/provenance-interchange.mjs";

/**
 * A key in comparable form: lowercase, with `_`, `-`, and spaces removed, so
 * `agent_id`, `agentId`, `Agent-Id`, and `AGENTID` are the same key.
 */
export const normaliseKey = (key) => String(key).toLowerCase().replace(/[_\- ]/g, "");

/**
 * Fields that name who or what produced something, or which execution,
 * session, or run did, in normalised form (see `normaliseKey`).
 */
export const IDENTITY_KEYS = Object.freeze(new Set([
  "actor", "actors", "actorkind", "actorid", "actorname", "actortype",
  "agent", "agents", "agentid", "agentname", "agentkind", "agenttype", "agentfamily",
  "author", "authors", "authorid", "authorname", "authoragent", "authorfamily", "authoredby",
  "createdby", "createdbyagent", "owneragent", "sourceauthor",
  "generatedby", "producedby", "writtenby", "draftedby", "executedby",
  "executor", "executors", "executorid", "executorname", "executorfamily", "executoridentity",
  "execution", "executions", "executionid", "executionkey",
  "contributor", "contributors", "contributorid", "contributionkey",
  "sessionid", "conversationid", "threadid", "runid",
  "provider", "model", "modelid", "modelname", "modelversion", "runtime", "runtimename", "runtimeversion",
]));

/**
 * Keys that describe the system under diagnosis at least as often as the
 * analyst. An incident case can legitimately give a component a `provider`
 * (cloud or DNS provider), a `model` (hardware or ML model), or a `runtime`.
 * These keys are therefore allowed as keys in blinded material, but their
 * values are still scanned: a provider/model/runtime name of an AI executor
 * (IDENTITY_TEXT_SIGNALS) is rejected wherever it appears.
 */
export const SYSTEM_DESCRIPTION_KEYS = Object.freeze(new Set([
  "provider", "model", "modelid", "modelname", "modelversion", "runtime", "runtimename", "runtimeversion",
]));

/** Identity fields forbidden as keys in blinded material (normalised). */
export const BLINDED_IDENTITY_KEYS = Object.freeze(new Set(
  [...IDENTITY_KEYS].filter((key) => !SYSTEM_DESCRIPTION_KEYS.has(key)),
));

export const isIdentityKey = (key) => IDENTITY_KEYS.has(normaliseKey(key));
export const isBlindedIdentityKey = (key) => BLINDED_IDENTITY_KEYS.has(normaliseKey(key));

/**
 * Word matching where `_` is a separator: a match may not touch an ASCII
 * letter or digit on either side, so `claude_code` and `MY_CODEX_THREAD_ID`
 * match but `Claudette` does not.
 */
const word = (body, flags = "") => new RegExp(`(?<![A-Za-z0-9])(?:${body})(?![A-Za-z0-9])`, flags);
const escapeRegExp = (text) => text.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
/** A normalised key as it may be written: optional `_` or `-` between letters, any case. */
const spelledKey = (key) => [...key].map(escapeRegExp).join("[_-]?");

/**
 * AI provider, product, runtime, and model names that do not occur as
 * ordinary words (a model version, a runtime, or a provider brand). They are
 * refused wherever they appear.
 */
const AI_IDENTIFIERS = [
  "anthropic", "openai", "chatgpt", "codex", "deepmind", "copilot", "ollama", "mixtral",
  "gpt-?[0-9][A-Za-z0-9.]*(?:-[A-Za-z0-9.]+)*",
  "llama-?[0-9][A-Za-z0-9.]*(?:-[A-Za-z0-9.]+)*",
  "claude(?:[-_]?[0-9][A-Za-z0-9.]*|[-_](?:code|opus|sonnet|haiku|instant|cli))(?:-[A-Za-z0-9.]+)*",
  "gemini(?:[-_]?[0-9][A-Za-z0-9.]*|[-_](?:cli|pro|ultra|flash|nano|code))(?:-[A-Za-z0-9.]+)*",
  "mistral(?:[-_]?[0-9][A-Za-z0-9.]*|[-_](?:large|medium|small|tiny|nemo))(?:-[A-Za-z0-9.]+)*",
];

/**
 * Model and runtime names that are also ordinary words or names (a person
 * called Claude, a payment provider called Gemini, the mistral wind). They
 * are refused only as a producer: "generated by X", "model: X", "X wrote".
 */
const AI_WORDS = ["claude", "gemini", "mistral", "llama", "grok"];

const AI_NAME = `(?:${[...AI_IDENTIFIERS, ...AI_WORDS].join("|")})`;
const PRODUCER_CONTEXTS = [
  `(?:generated|written|produced|authored|drafted|created|composed|analy[sz]ed|reviewed|summari[sz]ed|annotated)\\s+(?:by|with|using|via)\\s+(?:the\\s+)?${AI_NAME}`,
  `(?:model|runtime)(?:[_-]?(?:name|id))?["']?\\s*[:=]\\s*["']?${AI_NAME}`,
  `${AI_NAME}\\s+(?:wrote|generated|produced|authored|drafted|analy[sz]ed|summari[sz]ed)`,
];

/**
 * Identity fields that are provenance-shaped even in prose: the analyst's
 * producer and execution keys. Generic words such as actor, execution,
 * session_id, or provider describe the diagnosed system as often as the
 * analyst, so in prose they are not identity markers; in blinded JSON the
 * key position decides (`blindedMembers`).
 */
const PROSE_IDENTITY_KEYS = [
  "generatedby", "producedby", "writtenby", "draftedby", "authoredby",
  "executionkey", "contributionkey", "executoridentity",
  "authorfamily", "agentfamily", "executorfamily",
];

/**
 * Signals of provenance or identity: the identity of whoever analysed or
 * produced the blinded material, as it appears in provenance and telemetry
 * artifacts. They run on the full text of every blinded file (JSON or not)
 * and on every decoded JSON string, keys and values, so JSON escapes such as
 * `\u0063laude` cannot hide a name. Mirrors Percepta's ExperimentBlinding
 * (PCT-038).
 */
export const IDENTITY_TEXT_SIGNALS = Object.freeze([
  ["provenance block", /^\s*provenance\s*:|"provenance"\s*:/m],
  ["schema tag", /praxis\.provenance\/|echelon\.execution-envelope\//i],
  ["legacy author field", word("author[_-]?agent|created[_-]?by[_-]?agent|owner[_-]?agent|source[_-]?author", "i")],
  // Case-sensitive, Praxis key grammar: EXE-/CTB- keys, and EXT-<system>.<run>
  // with a lowercase system, so `EXT-4` (a file system) is not a key.
  ["execution key", /(?<![A-Za-z0-9])(?:(?:EXE|CTB)-[A-Za-z0-9][A-Za-z0-9._:-]*|EXT-[a-z][a-z0-9-]*\.[A-Za-z0-9][A-Za-z0-9._-]*)/],
  ["identity environment variable", word(
    [...IDENTITY_ENVIRONMENT_VARIABLES.map(escapeRegExp), "ROS_(?:ACTOR|EXECUTION|TELEMETRY)[A-Z0-9_]*"].join("|"), "i")],
  ["provider/model/runtime name", word([...AI_IDENTIFIERS, ...PRODUCER_CONTEXTS].join("|"), "i")],
  ["identity field", new RegExp(`(?:^|[\\s{,"'])(?:${PROSE_IDENTITY_KEYS.map(spelledKey).join("|")})["']?\\s*:`, "im")],
]);

/** Signals for raw JSON text that parsed: keys are judged by position instead. */
const JSON_TEXT_SIGNALS = Object.freeze(IDENTITY_TEXT_SIGNALS.filter(([name]) => name !== "identity field"));

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

const JSON_TOKEN = /"(?:[^"\\]|\\.)*"|[{}[\]]/g;
const MEMBER_COLON = /[ \t\n\r]*:/y;

/**
 * Member names repeated within one object of valid JSON text (contract 1.2
 * rule 1). JSON.parse silently keeps the last duplicate, so a repeated key
 * could hide a value from every check that reads the parsed document.
 * Names are compared decoded, so `"author"` and `"\u0061uthor"` collide.
 */
export const duplicateMemberNames = (text) => {
  const scopes = [];
  const found = [];
  for (const match of text.matchAll(JSON_TOKEN)) {
    const token = match[0];
    if (token === "{") scopes.push(new Set());
    else if (token === "[") scopes.push(undefined);
    else if (token === "}" || token === "]") scopes.pop();
    else {
      MEMBER_COLON.lastIndex = match.index + token.length;
      const scope = scopes.at(-1);
      if (scope && MEMBER_COLON.test(text)) {
        const name = JSON.parse(token);
        if (scope.has(name)) found.push(name);
        scope.add(name);
      }
    }
  }
  return found;
};

/** Parses JSON text, refusing repeated member names: `{ ok, value }` or `{ ok: false, error }`. */
export const parseJsonText = (text) => {
  try {
    const value = JSON.parse(text);
    const duplicates = duplicateMemberNames(text);
    return duplicates.length === 0
      ? { ok: true, value }
      : { ok: false, error: `member name repeated within one object (${[...new Set(duplicates)].join(", ")})` };
  } catch (error) {
    return { ok: false, error: error.message };
  }
};

/** Findings for one claim-registry entry (hypothesis, evidence, or theory). */
export const registryEntryFindings = (entry, label) => {
  const id = entry?.id ?? "(no id)";
  const where = `${label} ${id}`;
  const { provenance, ...claim } = entry ?? {};

  const identity = entries(claim)
    .filter((item) => isIdentityKey(item.key))
    .map((item) => `${where}: identity field '${item.path}' is provenance, not evidence; record it only in the entry's Praxis 'provenance' block`);

  const nested = entries(claim)
    .filter((item) => normaliseKey(item.key) === "provenance" || (typeof item.value === "string" && PROVENANCE_TAG.test(item.value)))
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

/** Every decoded string in a JSON value, member names and values, with its path. */
const strings = (value, at = "") => {
  if (typeof value === "string") return [{ path: at || "(root)", text: value }];
  if (Array.isArray(value)) return value.flatMap((item, index) => strings(item, `${at}[${index}]`));
  if (value && typeof value === "object") {
    return Object.entries(value).flatMap(([key, item]) => {
      const here = at ? `${at}.${key}` : key;
      return [{ path: here, text: key }, ...strings(item, here)];
    });
  }
  return [];
};

/** Text with `\uXXXX` escapes decoded, so an escaped name in a text packet is still seen. */
const decodeUnicodeEscapes = (text) =>
  text.replace(/\\u([0-9A-Fa-f]{4})/g, (_, hex) => String.fromCharCode(Number.parseInt(hex, 16)));

/** The identity signals found in one piece of text: `[name, matched text]`. */
export const identitySignals = (text, signals = IDENTITY_TEXT_SIGNALS) => signals
  .map(([name, pattern]) => [name, pattern.exec(text) ?? pattern.exec(decodeUnicodeEscapes(text))])
  .filter(([, match]) => match)
  .map(([name, match]) => [name, match[0].trim()]);

/** Objects whose members are the analyst's identity wherever they sit. */
const PROVENANCE_SCOPES = Object.freeze(new Set(["provenance", "contributions", "actor"]));

/**
 * Every member of blinded JSON with its position: `record` when it sits at
 * the top level of a case record (the document root, an element of a root
 * array, or an element of the root `cases` collection), `scoped` when it sits
 * inside a `provenance`, `contributions`, or `actor` object. Members nested in
 * domain data, such as an audit-log excerpt, are neither.
 */
const blindedMembers = (value, at = "", record = true, scoped = false) => {
  if (Array.isArray(value)) return value.flatMap((item, index) => blindedMembers(item, `${at}[${index}]`, record, scoped));
  if (value && typeof value === "object") {
    return Object.entries(value).flatMap(([key, item]) => {
      const here = at ? `${at}.${key}` : key;
      const name = normaliseKey(key);
      return [
        { path: here, key, record, scoped },
        ...blindedMembers(item, here, at === "" && name === "cases", scoped || PROVENANCE_SCOPES.has(name)),
      ];
    });
  }
  return [];
};

/** True when a member names the analyst: an identity key at record level, or any identity key inside a provenance scope. */
const isBlindedIdentityMember = ({ key, record, scoped }) =>
  (record && isBlindedIdentityKey(key)) || (scoped && isIdentityKey(key));

const jsonFindings = (relativePath, document) => [
  ...blindedMembers(document).flatMap((item) => [
    ...(normaliseKey(item.key) === "provenance" ? [`${relativePath}: blinded material carries a provenance block at '${item.path}'`] : []),
    ...(isBlindedIdentityMember(item) ? [`${relativePath}: blinded material carries identity field '${item.path}'`] : []),
  ]),
  ...strings(document).flatMap(({ path: at, text }) =>
    identitySignals(text).map(([name, found]) => `${relativePath}: blinded material carries ${name} at '${at}' (${found})`)),
];

const textFindings = (relativePath, text, signals) => identitySignals(text, signals)
  .map(([name, found]) => `${relativePath}: blinded material carries ${name} (${found})`);

/**
 * Findings for one blinded file, given its text. The identity signals run on
 * the raw text; for JSON they also run on every decoded member name and
 * string value, and member names are compared in normalised form by
 * position: at the top level of a case record or inside a provenance scope,
 * never in nested domain data.
 */
export const blindedMaterialFindings = (relativePath, text) => {
  if (!relativePath.endsWith(".json")) return textFindings(relativePath, text, IDENTITY_TEXT_SIGNALS);
  const parsed = parseJsonText(text);
  return parsed.ok
    ? [...textFindings(relativePath, text, JSON_TEXT_SIGNALS), ...jsonFindings(relativePath, parsed.value)]
    : [...textFindings(relativePath, text, IDENTITY_TEXT_SIGNALS), `${relativePath}: blinded material is not valid JSON (${parsed.error})`];
};

/** Findings for a claim registry given its JSON text. */
export const registryTextFindings = (text, label, collection) => {
  const parsed = parseJsonText(text);
  return parsed.ok ? registryFindings(parsed.value, label, collection) : [`Cannot parse ${label}: ${parsed.error}`];
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
    return registryTextFindings(fs.readFileSync(full, "utf8"), path.basename(relative), collection);
  });
  const blinded = walk(root).filter(isBlindedMaterial);
  const blindedErrors = blinded.flatMap((relative) => blindedMaterialFindings(relative, fs.readFileSync(path.join(root, relative), "utf8")));
  return {
    errors: [...registryErrors, ...blindedErrors],
    summary: { blindedFilesChecked: blinded.length },
  };
}
