import fs from "node:fs";
import path from "node:path";
import crypto from "node:crypto";

const allowedStates = new Set(["internal-demonstration","suggestive","unsupported","contradicted","falsified","superseded","planned"]);
const allowedPublicStates = new Set(["internal","suggestive","unsupported","open"]);
const allowedConfidence = new Set(["Low","Low-Medium","Medium","Medium-High","High"]);
const allowedTheoryStates = new Set(["working","unestablished","deferred","rejected","superseded"]);

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
  const theoryPath = path.join(root, "research/registries/theory-registry.json");

  let hreg, ereg, treg;
  try { hreg = JSON.parse(fs.readFileSync(hypothesisPath, "utf8")); } catch (e) { errors.push("Cannot parse hypothesis registry: " + e.message); }
  try { ereg = JSON.parse(fs.readFileSync(evidencePath, "utf8")); } catch (e) { errors.push("Cannot parse evidence registry: " + e.message); }
  try { treg = JSON.parse(fs.readFileSync(theoryPath, "utf8")); } catch (e) { errors.push("Cannot parse theory registry: " + e.message); }

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

  const theoryIds = new Set();
  if (treg) {
    for (const th of treg.theories ?? []) {
      if (theoryIds.has(th.id)) errors.push("Duplicate theory ID: " + th.id);
      theoryIds.add(th.id);
      if (!allowedTheoryStates.has(th.state)) errors.push("Invalid theory state: " + th.id + " -> " + th.state);
      if (!allowedConfidence.has(th.confidence)) errors.push("Invalid theory confidence: " + th.id + " -> " + th.confidence);
      for (const id of [...(th.supportedBy ?? []), ...(th.challengedBy ?? [])]) {
        if (!hypothesisIds.has(id)) errors.push("Unknown hypothesis reference: " + th.id + " -> " + id);
      }
    }
  }

  const repPath = path.join(root, "research/packages/RP-EDF-2026-002.md");
  if (!fs.existsSync(repPath)) errors.push("Missing current REP: research/packages/RP-EDF-2026-002.md");
  else {
    const repText = fs.readFileSync(repPath, "utf8");
    const headings = ["Research State Snapshot","Executive Summary","Original Objective","Scope","Repository Context","Current Understanding","Key Discoveries","Evidence Registry","Hypothesis Registry","Failed Assumptions","Open Questions","Recommended Next Research","Research Backlog","Suggested Specialized Research Agents","Parallel Research Opportunities","Risks","Cross-Discipline Opportunities","Knowledge Relationships","Theory Impact Assessment","Research Quality Metrics","Research Debt","Repository Updates","Website Updates","AI Consumption Notes","Handoff Instructions","Research Journal","Appendix","Completion Checklist"];
    for (const heading of headings) if (!repText.includes("# " + heading) && !repText.includes("## " + heading)) errors.push("Current REP missing section: " + heading);
  }

  if (!fs.existsSync(path.join(root, "research/journal/JR-EDF-2026-002.md"))) errors.push("Missing current research journal.");

  const releaseManifestPath = path.join(root, "docs/edf/releases/v0.3/manifest.json");
  let frozenFilesChecked = 0;
  if (!fs.existsSync(releaseManifestPath)) errors.push("Missing v0.3 release manifest.");
  else {
    try {
      const manifest = JSON.parse(fs.readFileSync(releaseManifestPath, "utf8"));
      for (const item of manifest.files ?? []) {
        const full = path.join(root, item.snapshotPath);
        if (!fs.existsSync(full)) { errors.push("Missing frozen v0.3 artifact: " + item.snapshotPath); continue; }
        const bytes = fs.readFileSync(full);
        const prefix = Buffer.from("blob " + bytes.length + "\0");
        const sha = crypto.createHash("sha1").update(prefix).update(bytes).digest("hex");
        if (sha !== item.gitBlobSha) errors.push("Frozen v0.3 artifact hash drift: " + item.snapshotPath);
        frozenFilesChecked++;
      }
    } catch (e) { errors.push("Cannot validate v0.3 release manifest: " + e.message); }
  }

  const linkFiles = [
    ...walk(path.join(root, "docs"), f => f.endsWith(".md") && !f.includes(path.join("docs", "edf", "releases"))),
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
      theoryRecords: theoryIds.size,
      repsChecked: fs.existsSync(repPath) ? 1 : 0,
      frozenV03FilesChecked: frozenFilesChecked,
      checkedLinkFiles: linkFiles.length,
      requiredWebsiteFiles: required.length
    }
  };
}
