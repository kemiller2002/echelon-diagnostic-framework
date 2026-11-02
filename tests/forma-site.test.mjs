// Forma MarketingShell conformance for the EDF public site (static-site/).
// Mirrors the structure checks Forma requires of every marketing site
// (Forma docs/MARKETING-SITES.md, question 8; tests/marketing-structure.test.mjs).
import test from "node:test";
import assert from "node:assert/strict";
import fs from "node:fs";
import path from "node:path";
import { parseFormaLock, formaLockErrors } from "../scripts/repository-integrity.mjs";

const site = "static-site";
const pages = ["index.html", "learn.html", "examples.html", "evidence.html", "start.html"];
const html = file => fs.readFileSync(path.join(site, file), "utf8");
const lock = parseFormaLock(fs.readFileSync("forma.lock", "utf8"));
const bundlePath = path.join(lock.destination ?? "", "forma-echelon-marketing.css");
const bundleInstalled = fs.existsSync(bundlePath);

const classesIn = source => new Set([...source.matchAll(/class="([^"]+)"/g)].flatMap(m => m[1].split(/\s+/)).filter(Boolean));
const definedIn = css => new Set([...css.replace(/\/\*[\s\S]*?\*\//g, "").matchAll(/\.([a-zA-Z][a-zA-Z0-9_-]*)/g)].map(m => m[1]));
// Local rules, minus those a "forma-exception" comment documents as a pending
// upstream Forma fix (Forma docs/MARKETING-SITES.md, question 5).
const rulesOf = css => [...css.matchAll(/\s*((?:\/\*[\s\S]*?\*\/\s*)*)([^{}]+)\{[^{}]*\}/g)]
  .map(m => ({ documented: m[1].includes("forma-exception:"), selector: m[2].trim() }));
const localDefinitions = css => definedIn(rulesOf(css).filter(rule => !rule.documented).map(rule => rule.selector + "{}").join("\n"));
const stylesheetsIn = source => [...source.matchAll(/<link[^>]+rel="stylesheet"[^>]+href="([^"]+)"/g)].map(m => m[1]);
const between = (source, start, end) => source.slice(source.indexOf(start), source.indexOf(end, source.indexOf(start)));

// Public titles and descriptions are SEO metadata; the migration must not change them.
const seo = {
  "index.html": ["EDF — Understand the system before changing it", "The Echelon Diagnostic Framework is a working diagnostic grammar for making system explanations explicit, evidence-bound, and open to challenge."],
  "learn.html": ["Learn EDF — A guided introduction", "Learn the Echelon Diagnostic Framework step by step, from system context to diagnostic sufficiency."],
  "examples.html": ["EDF Examples — From everyday problems to complex systems", "Worked Echelon Diagnostic Framework examples across operations, software, product, organizations, and successful systems."],
  "evidence.html": ["EDF Evidence — What is supported, provisional, and open", "A transparent guide to the evidence, validation status, limitations, and research questions behind EDF."],
  "start.html": ["Start an EDF Diagnosis — A guided worksheet", "Use this guided worksheet to create your first Echelon Diagnostic Framework diagnosis."]
};

// In-page anchors that external links and the previous site exposed.
const anchors = {
  "index.html": ["framework", "how-to", "examples", "research", "start"],
  "learn.html": ["lesson-1", "lesson-2", "lesson-3", "lesson-4"],
  "start.html": ["worksheet"]
};

test("forma.lock pins an exact, hashed Forma release from the authoritative repository", () => {
  assert.deepEqual(formaLockErrors(lock), []);
  assert.deepEqual(lock.assets.map(asset => asset.name), ["forma-echelon-marketing.css"]);
});

test("forma.lock parsing rejects floating or unhashed pins", () => {
  const errors = formaLockErrors(parseFormaLock("forma main\nrepository someone/fork\ndestination static-site/assets/forma\nasset forma-echelon-marketing.css sha256:recorded-at-install\n"));
  assert.ok(errors.some(e => e.includes("exact Forma version")));
  assert.ok(errors.some(e => e.includes("authoritative Forma repository")));
  assert.ok(errors.some(e => e.includes("lacks a recorded sha256")));
});

test("every page renders only from the pinned Forma bundle plus the local stylesheet", () => {
  pages.forEach(file => {
    const source = html(file);
    const local = stylesheetsIn(source).filter(href => !href.startsWith("https://fonts.googleapis.com/"));
    assert.deepEqual(local, ["/assets/forma/forma-echelon-marketing.css", "/styles.css"], `${file} loads unexpected CSS`);
    assert.doesNotMatch(source, /<style\b|\sstyle="/i, `${file} carries inline presentation`);
    assert.doesNotMatch(source, /<script\b/i, `${file} adds runtime behavior`);
    assert.match(source, /<html lang="en" data-ef-theme="light">/, `${file} must select the light Echelon theme`);
  });
});

test("every class the site uses is defined by Forma or declared locally", { skip: !bundleInstalled && !process.env.CI && `Forma bundle not installed at ${bundlePath}; run the forma.lock installer` }, () => {
  assert.ok(bundleInstalled, `Forma bundle missing at ${bundlePath}`);
  const forma = definedIn(fs.readFileSync(bundlePath, "utf8"));
  const local = localDefinitions(fs.readFileSync(path.join(site, "styles.css"), "utf8"));
  pages.forEach(file => [...classesIn(html(file))].forEach(name => {
    assert.ok(forma.has(name) || local.has(name), `${file} uses .${name}, which neither Forma nor styles.css defines`);
    assert.ok(!(forma.has(name) && local.has(name)), `styles.css redefines Forma's .${name}`);
  }));
});

test("every local stylesheet rule that touches Forma classes is a documented forma-exception", () => {
  const rules = rulesOf(fs.readFileSync(path.join(site, "styles.css"), "utf8"));
  rules.filter(rule => /\.ef-/.test(rule.selector) && !rule.documented)
    .forEach(rule => assert.fail(`styles.css restyles Forma without a forma-exception: ${rule.selector}`));
  assert.ok(rules.length <= 4, `styles.css should stay small; it has ${rules.length} rules`);
});

test("every page has the MarketingShell landmarks in semantic order", () => {
  pages.forEach(file => {
    const source = html(file);
    const body = source.slice(source.indexOf("<body"));
    assert.match(body.slice(body.indexOf(">") + 1).trim(), /^<a class="ef-skip-link" href="#main-content">/, `${file} skip link must come first`);
    assert.match(source, /<main class="ef-site__main" id="main-content" tabindex="-1">/);
    const order = ["ef-skip-link", '<header class="ef-site-header"', '<nav class="ef-site-nav" aria-label="Primary"', "<main", '<footer class="ef-site-footer"']
      .map(marker => source.indexOf(marker));
    assert.ok(order.every(i => i >= 0), `${file} is missing a shell region`);
    assert.deepEqual([...order].sort((a, b) => a - b), order, `${file} shell regions are out of order`);
    const primaryNav = between(source, 'aria-label="Primary"', "</nav>");
    assert.equal((primaryNav.match(/aria-current="page"/g) ?? []).length, 1, `${file} must mark exactly one current page`);
    assert.ok((primaryNav.match(/data-ef-variant="action"/g) ?? []).length <= 1, `${file} has more than one navigation action`);
  });
});

test("the shared header is identical on every page apart from the current-page marker", () => {
  const chrome = file => between(html(file), '<header class="ef-site-header">', "</header>").replace(/ aria-current="page"/g, "");
  pages.forEach(file => assert.equal(chrome(file), chrome("index.html"), `${file} header drifted from index.html`));
});

test("pages keep one h1 and do not skip heading levels", () => {
  pages.forEach(file => {
    const levels = [...html(file).matchAll(/<h([1-6])\b/g)].map(m => Number(m[1]));
    assert.equal(levels.filter(level => level === 1).length, 1, `${file} must have exactly one h1`);
    levels.reduce((previous, level) => {
      assert.ok(level <= previous + 1, `${file} skips from h${previous} to h${level}`);
      return level;
    }, 1);
  });
});

test("each hero, section and call to action offers at most one primary action", () => {
  pages.forEach(file => html(file).split(/<section\b/).slice(1).forEach((region, index) => {
    const primaries = (region.match(/data-ef-variant="primary"/g) ?? []).length;
    assert.ok(primaries <= 1, `${file} region ${index + 1} has ${primaries} primary actions`);
  }));
});

test("page titles, descriptions and Open Graph metadata are preserved", () => {
  Object.entries(seo).forEach(([file, [title, description]]) => {
    const source = html(file);
    assert.ok(source.includes(`<title>${title}</title>`), `${file} title changed`);
    assert.ok(source.includes(`<meta name="description" content="${description}">`), `${file} description changed`);
  });
  const home = html("index.html");
  ["og:title", "og:description", "og:image"].forEach(property => assert.match(home, new RegExp(`<meta property="${property}" content="[^"]+">`)));
});

test("previously published in-page anchors still resolve", () => {
  Object.entries(anchors).forEach(([file, ids]) => ids.forEach(id => assert.match(html(file), new RegExp(`id="${id}"`), `${file}#${id} is gone`)));
});

test("labelled regions point at elements that exist", () => {
  pages.forEach(file => {
    const source = html(file);
    const ids = new Set([...source.matchAll(/\sid="([^"]+)"/g)].map(m => m[1]));
    [...source.matchAll(/aria-labelledby="([^"]+)"/g)].flatMap(m => m[1].split(/\s+/))
      .forEach(id => assert.ok(ids.has(id), `${file} aria-labelledby references missing #${id}`));
    const duplicates = [...source.matchAll(/\sid="([^"]+)"/g)].map(m => m[1]).filter((id, i, all) => all.indexOf(id) !== i);
    assert.deepEqual(duplicates, [], `${file} has duplicate ids`);
  });
});
