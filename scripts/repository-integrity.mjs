import fs from "node:fs";
import path from "node:path";

const allowedStates = new Set(["internal-demonstration","suggestive","unsupported","contradicted","falsified","superseded","planned"]);
const allowedPublicStates = new Set(["internal","suggestive","unsupported","open"]);
const allowedConfidence = new Set(["Low","Low-Medium","Medium","Medium-High","High"]);

function walk(root, predicate) {
  const out = [];
  for (const entry of fs.readdirSync(root, { withFileTypes: true })) {
    const full = path.join(root, entry.name);
    if (entry.isDirectory()) out.push(...walk(full, predicate));
    else if (predicate(full)) out.push(full);
  }
  return out;
}

function stripTarget(target) {
  return target.trim().replace(/^<|>$/g, "").split("#")[0].split("?")[0];
}

function localTargetExists(root, sourceFile, target) {
  const t = stripTarget(target);
  if (!t || /^(https?:|mailto:|tel:|javascript:)/i.test(t)) return true;
  if (t === "/") return fs.existsSync(path.join(root, "static-site", "index.html"));
  if (t.startsWith("/")) {
    if (t === "/og.png") return fs.existsSync(path.join(root, "public", "og.png"));
    return fs.existsSync(path.join(root, "static-site", t.slice(1)));
  }
  const resolved = path.resolve(path.dirname(sourceFile), t);
  return fs.existsSync(resolved);
}

export function validateRepository(root = process.cwd()) {
  const errors = [];
  const hypothesisPath = path.join(root, "research/registries/hypothesis-registry.json");
  const evidencePath = path.join(root, "research/registries/evidence-registry.json");

  let hreg, ereg;
  try { hreg = JSON.parse(fs.readFileSync(hypothesisPath, "utf8")); } catch (e) { errors.push("Cannot parse hypothesis registry: " + e.message); }
  try { ereg = JSON.parse(fs.readFileSync(evidencePath, "utf8")); } catch (e) { errors.push("Cannot parse evidence registry: " + e.message); }

  const evidenceIds = new Set();
  if (ereg) {
    for (const ev of ereg.evidence ?? []) {
      if (evidenceIds.has(ev.id)) errors.push("Duplicate evidence ID: " + ev.id);
      evidenceIds.add(ev.id);
      for (const rel of ev.paths ?? []) {
        if (!fs.existsSync(path.join(root, rel))) errors.push("Evidence path does not exist: " + ev.id + " -> " + rel);
      }
    }
  }

  const hypothesisIds = new Set();
  const publicStates = new Map();
  if (hreg) {
    for (const hy of hreg.hypotheses ?? []) {
      if (hypothesisIds.has(hy.id)) errors.push("Duplicate hypothesis ID: " + hy.id);
      hypothesisIds.add(hy.id);
      if (!allowedStates.has(hy.state)) errors.push("Invalid hypothesis state: " + hy.id + " -> " + hy.state);
      if (!allowedConfidence.has(hy.confidence)) errors.push("Invalid confidence: " + hy.id + " -> " + hy.confidence);
      if (!allowedPublicStates.has(hy.publicState)) errors.push("Invalid publicState: " + hy.id + " -> " + hy.publicState);
      publicStates.set(hy.id, hy.publicState);
      for (const id of hy.evidence ?? []) if (!evidenceIds.has(id)) errors.push("Unknown evidence reference: " + hy.id + " -> " + id);
    }
  }

  const linkFiles = [
    ...walk(path.join(root, "docs"), f => f.endsWith(".md")),
    ...walk(path.join(root, "research"), f => f.endsWith(".md")),
    ...walk(path.join(root, "static-site"), f => f.endsWith(".html"))
  ];
  for (const file of linkFiles) {
    const text = fs.readFileSync(file, "utf8");
    const targets = [];
    if (file.endsWith(".md")) {
      for (const m of text.matchAll(/\[[^\]]*\]\(([^)]+)\)/g)) targets.push(m[1]);
    } else {
      for (const m of text.matchAll(/(?:href|src)="([^"]+)"/g)) targets.push(m[1]);
    }
    for (const target of targets) {
      if (!localTargetExists(root, file, target)) {
        errors.push("Broken local reference: " + path.relative(root, file) + " -> " + target);
      }
    }
  }

  for (const rel of ["static-site/index.html","static-site/evidence.html"]) {
    const text = fs.readFileSync(path.join(root, rel), "utf8");
    for (const m of text.matchAll(/data-claim-id="([^"]+)"\s+data-claim-state="([^"]+)"/g)) {
      const id = m[1], state = m[2];
      if (!publicStates.has(id)) errors.push("Public page references unknown hypothesis: " + rel + " -> " + id);
      else if (publicStates.get(id) !== state) errors.push("Public claim state drift: " + rel + " -> " + id + " expected " + publicStates.get(id) + " got " + state);
    }
  }

  const required = ["static-site/index.html","static-site/learn.html","static-site/examples.html","static-site/evidence.html","static-site/start.html","static-site/styles.css","public/og.png"];
  for (const rel of required) if (!fs.existsSync(path.join(root, rel))) errors.push("Missing canonical website file: " + rel);

  const legacy = ["app","build","worker",".openai/hosting.json","next.config.ts","vite.config.ts","next-env.d.ts","tsconfig.json","package.json","package-lock.json"];
  for (const rel of legacy) if (fs.existsSync(path.join(root, rel))) errors.push("Stale duplicate web stack remains: " + rel);

  const repro = fs.readFileSync(path.join(root, "docs/edf/validation/reproducibility-findings.md"), "utf8");
  if (!repro.includes("No completed independent reproducibility result exists")) errors.push("Current reproducibility boundary is missing.");

  return {
    errors,
    summary: {
      hypotheses: hypothesisIds.size,
      evidenceRecords: evidenceIds.size,
      checkedLinkFiles: linkFiles.length,
      requiredWebsiteFiles: required.length
    }
  };
}
