#!/usr/bin/env node
import fs from 'node:fs';
import path from 'node:path';

const ROOT=process.cwd();
const EXP=path.join(ROOT,'research','experiments','EX-EDF-2026-A003');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8'));
const schema=read(path.join(EXP,'review-schema.json'));

function paths(id){
  return {
    casePath:path.join(EXP,'cases',id+'.case.json'),
    truthPath:path.join(EXP,'cases',id+'.truth.json'),
    reviewPath:path.join(EXP,'reviews',id+'.review.json')
  };
}
function prompt(id,reviewerFamily){
  const p=paths(id);
  if(!fs.existsSync(p.casePath)||!fs.existsSync(p.truthPath)) throw new Error('Missing case/truth pair for '+id);
  const c=read(p.casePath),t=read(p.truthPath);
  if(String(t.authorFamily).toLowerCase()===String(reviewerFamily).toLowerCase()) throw new Error('Reviewer must be opposite provider family.');
  return [
    '# EX-EDF-2026-A003 opposite-provider case review','',
    'Case ID: '+id,
    'Author family: '+t.authorFamily,
    'Reviewer family: '+reviewerFamily,'',
    'Review the case and hidden truth for experimental validity. Do not solve analyzer conditions or simulate benchmark results.',
    'A pass requires every frozen check to be true. If any check fails, verdict must be revise.','',
    'Review schema:',JSON.stringify(schema,null,2),'',
    'Analyzer-visible case:',JSON.stringify(c,null,2),'',
    'Hidden truth:',JSON.stringify(t,null,2),'',
    'Check specifically:',
    '- every expected state is supported by allowed case evidence or a genuine encoded absence;',
    '- candidate wording does not leak correct answers;',
    '- no single evidence item reveals the complete target diagnosis/action;',
    '- wrong candidates are genuinely plausible rather than straw men;',
    '- at least four construct-critical items truly require the assigned construct;',
    '- difficulty is high enough to create plausible errors under generic reasoning;',
    '- weights/evidence keys do not arbitrarily favor the construct condition.','',
    'Return strict JSON only following the review schema. Do not wrap it in Markdown.'
  ].join('\n');
}
function validateReview(id){
  const p=paths(id);
  if(!fs.existsSync(p.reviewPath)) return ['missing review file'];
  const r=read(p.reviewPath),t=read(p.truthPath),errors=[];
  for(const f of schema.required) if(!(f in r)) errors.push('missing '+f);
  if(r.caseId!==id) errors.push('caseId mismatch');
  if(r.authorFamily!==t.authorFamily) errors.push('authorFamily mismatch');
  if(String(r.reviewerFamily).toLowerCase()===String(t.authorFamily).toLowerCase()) errors.push('reviewer must be opposite family');
  for(const k of Object.keys(schema.checks)) if(typeof r.checks?.[k]!=='boolean') errors.push('invalid check '+k);
  if(!schema.verdict.includes(r.verdict)) errors.push('invalid verdict');
  if(r.verdict==='pass'&&Object.keys(schema.checks).some(k=>r.checks?.[k]!==true)) errors.push('pass requires every check true');
  return errors;
}

const [cmd,...args]=process.argv.slice(2);
if(cmd==='prompt') console.log(prompt(args[0],args[1]));
else if(cmd==='validate'){
  const errors=validateReview(args[0]);
  if(errors.length){for(const e of errors)console.error(e);process.exitCode=1;}
  else console.log(JSON.stringify({ok:true,caseId:args[0]},null,2));
}else{
  console.error('Usage: node scripts/edf-002-review.mjs prompt <caseId> <reviewerFamily> | validate <caseId>');
  process.exitCode=2;
}
