#!/usr/bin/env node
import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import { validateCases } from './edf-002-validate.mjs';

const ROOT=process.cwd();
const EXP=path.join(ROOT,'research','experiments','EX-EDF-2026-A003');
const OUT=path.join(EXP,'frozen-files.json');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8'));
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(p)).digest('hex');
const rel=p=>path.relative(ROOT,p).replaceAll('\\','/');

function reviewErrors(){
  const errors=[];
  const dir=path.join(EXP,'cases');
  for(const file of fs.readdirSync(dir).filter(x=>x.endsWith('.case.json')).sort()){
    const id=file.replace('.case.json','');
    const truth=read(path.join(dir,id+'.truth.json'));
    const rp=path.join(EXP,'reviews',id+'.review.json');
    if(!fs.existsSync(rp)){errors.push(id+': missing opposite-provider review');continue;}
    const r=read(rp);
    if(r.caseId!==id) errors.push(id+': review caseId mismatch');
    if(String(r.reviewerFamily).toLowerCase()===String(truth.authorFamily).toLowerCase()) errors.push(id+': reviewer is same provider family');
    if(r.verdict!=='pass') errors.push(id+': review verdict is not pass');
    for(const [k,v] of Object.entries(r.checks??{})) if(v!==true) errors.push(id+': review check '+k+' is not true');
  }
  return errors;
}

function criticalFiles(){
  return [
    path.join(EXP,'preregistration.json'),
    path.join(EXP,'output-contract.json'),
    path.join(EXP,'case-authoring-assignments.json'),
    path.join(EXP,'prompt-modules.json'),
    path.join(EXP,'case-schema.json'),
    path.join(EXP,'scoring-spec.json'),
    path.join(EXP,'review-schema.json'),
    path.join(EXP,'randomization.json'),
    path.join(ROOT,'scripts','edf-002-score.mjs'),
    path.join(ROOT,'scripts','edf-002-validate.mjs'),
    path.join(ROOT,'scripts','edf-002-analyzer.mjs'),
    path.join(ROOT,'scripts','edf-002-freeze.mjs')
  ];
}
function expected(){
  const errors=[...validateCases(true),...reviewErrors()];
  if(errors.length) return {errors};
  const files=[...criticalFiles()];
  for(const f of fs.readdirSync(path.join(EXP,'cases')).filter(x=>/\.(case|truth)\.json$/.test(x)).sort()) files.push(path.join(EXP,'cases',f));
  for(const f of fs.readdirSync(path.join(EXP,'reviews')).filter(x=>x.endsWith('.review.json')).sort()) files.push(path.join(EXP,'reviews',f));
  return {errors:[],manifest:{
    version:'1.0.0',
    experiment:'EX-EDF-2026-A003',
    frozenAt:new Date().toISOString(),
    files:Object.fromEntries(files.map(p=>[rel(p),sha(p)]))
  }};
}

const cmd=process.argv[2]??'verify';
if(cmd==='freeze'){
  if(fs.existsSync(OUT)){console.error('frozen-files.json already exists; refusing replacement');process.exitCode=2;}
  else {
    const x=expected();
    if(x.errors.length){for(const e of x.errors)console.error(e);process.exitCode=1;}
    else {fs.writeFileSync(OUT,JSON.stringify(x.manifest,null,2)+'\n',{flag:'wx'});console.log(JSON.stringify({ok:true,files:Object.keys(x.manifest.files).length},null,2));}
  }
}else if(cmd==='verify'){
  if(!fs.existsSync(OUT)){console.error('No frozen-files.json');process.exitCode=1;}
  else {
    const m=read(OUT),errors=[];
    for(const [p,h] of Object.entries(m.files)){
      const full=path.join(ROOT,p);
      if(!fs.existsSync(full)) errors.push('missing '+p);
      else if(sha(full)!==h) errors.push('hash mismatch '+p);
    }
    if(errors.length){for(const e of errors)console.error(e);process.exitCode=1;}
    else console.log(JSON.stringify({ok:true,files:Object.keys(m.files).length,frozenAt:m.frozenAt},null,2));
  }
}else {console.error('Usage: node scripts/edf-002-freeze.mjs freeze|verify');process.exitCode=2;}
