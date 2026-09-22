#!/usr/bin/env node
import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';

const ROOT=process.cwd();
const EXP=path.join(ROOT,'research','experiments','EX-EDF-2026-A003');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8'));
const shaText=s=>crypto.createHash('sha256').update(s).digest('hex');
const fail=(m,c=2)=>{console.error(m);process.exit(c);};

function assertFrozen(){
  const p=path.join(EXP,'frozen-files.json');
  if(!fs.existsSync(p)) fail('Experiment cases are not frozen.');
}
function plan(slot){
  assertFrozen();
  const random=read(path.join(EXP,'randomization.json'));
  const dir=path.join(EXP,'cases');
  const cases=fs.readdirSync(dir).filter(x=>x.endsWith('.case.json')).map(x=>read(path.join(dir,x))).sort((a,b)=>a.id.localeCompare(b.id));
  const rows=[];
  for(const c of cases) for(const condition of ['neutral','construct']){
    const runId=`${c.id}-${slot}-${condition}-r1`;
    rows.push({runId,caseId:c.id,construct:c.construct,analyzerSlot:slot,condition,replicate:1,orderKey:shaText(random.seed+'|'+slot+'|'+c.id+'|'+condition+'|order')});
  }
  rows.sort((a,b)=>a.orderKey.localeCompare(b.orderKey));
  return rows.map((x,i)=>({...x,sequence:i+1}));
}
function paths(slot){
  const out=path.join(EXP,'manual-runs',slot);
  return {out,executor:path.join(out,'executor.json'),plan:path.join(out,'run-plan.json'),runs:path.join(out,'runs'),completion:path.join(out,'completion.json')};
}
function promptFor(run){
  const c=read(path.join(EXP,'cases',run.caseId+'.case.json'));
  const modules=read(path.join(EXP,'prompt-modules.json'));
  const contract=read(path.join(EXP,'output-contract.json'));
  return [
    modules.shared,'',
    '## Condition instruction',modules[run.condition][c.construct],'',
    '## Case',JSON.stringify(c,null,2),'',
    '## Output contract',JSON.stringify(contract,null,2),'',
    'Return strict JSON only. Do not use Markdown fences.'
  ].join('\n');
}
function validateOutput(run,obj){
  const c=read(path.join(EXP,'cases',run.caseId+'.case.json'));
  const errors=[];
  if(!obj||typeof obj!=='object'||Array.isArray(obj)) return ['output must be object'];
  if(obj.caseId!==run.caseId) errors.push('caseId mismatch');
  if(!Array.isArray(obj.answers)) errors.push('answers must be array');
  else {
    const expected=new Map(c.scoredItems.map(x=>[x.id,x]));
    if(obj.answers.length!==expected.size) errors.push('answer count mismatch');
    const seen=new Set();
    for(const a of obj.answers){
      if(seen.has(a.itemId)) errors.push('duplicate itemId '+a.itemId); seen.add(a.itemId);
      const item=expected.get(a.itemId);
      if(!item){errors.push('unknown itemId '+a.itemId);continue;}
      if(!item.allowedAnswers.includes(a.answer)) errors.push(a.itemId+': invalid answer');
      if(!Number.isInteger(a.confidence)||a.confidence<0||a.confidence>100) errors.push(a.itemId+': confidence must be integer 0..100');
      if(!Array.isArray(a.evidenceIds)) errors.push(a.itemId+': evidenceIds must be array');
      else {
        const ids=new Set(c.evidence.map(e=>e.id));
        for(const e of a.evidenceIds) if(!ids.has(e)) errors.push(a.itemId+': unknown evidence '+e);
      }
    }
    for(const id of expected.keys()) if(!seen.has(id)) errors.push('missing itemId '+id);
  }
  return errors;
}

const [cmd,...args]=process.argv.slice(2);
const opt=n=>{const i=args.indexOf(n);return i>=0?args[i+1]:null;};

if(cmd==='prepare'){
  const slot=opt('--slot'); if(!slot) fail('prepare requires --slot');
  const p=paths(slot); fs.mkdirSync(p.out,{recursive:true});
  if(fs.existsSync(p.plan)) fail('run plan already exists');
  fs.writeFileSync(p.plan,JSON.stringify({experiment:'EX-EDF-2026-A003',slot,runs:plan(slot)},null,2)+'\n',{flag:'wx'});
  console.log(p.plan);
}else if(cmd==='bind'){
  const slot=opt('--slot'),provider=opt('--provider'),model=opt('--model'),surface=opt('--surface')??'unspecified';
  if(!slot||!provider||!model) fail('bind requires --slot --provider --model [--surface]');
  const p=paths(slot); if(!fs.existsSync(p.plan)) fail('prepare slot first'); if(fs.existsSync(p.executor)) fail('executor already bound');
  fs.writeFileSync(p.executor,JSON.stringify({version:'1.0.0',experiment:'EX-EDF-2026-A003',slot,provider,model,surface,boundAt:new Date().toISOString(),planSha256:shaText(fs.readFileSync(p.plan,'utf8'))},null,2)+'\n',{flag:'wx'});
  console.log(p.executor);
}else if(cmd==='next'){
  const slot=opt('--slot'); if(!slot) fail('next requires --slot'); const p=paths(slot);
  if(!fs.existsSync(p.plan)||!fs.existsSync(p.executor)) fail('prepare and bind first');
  const runs=read(p.plan).runs; fs.mkdirSync(p.runs,{recursive:true});
  const next=runs.find(r=>!fs.existsSync(path.join(p.runs,r.runId+'.meta.json')));
  if(!next){console.log('COMPLETE');process.exit(0);}
  console.log('RUN_ID='+next.runId); console.log('BEGIN_EXACT_ANALYZER_PROMPT'); console.log(promptFor(next)); console.log('END_EXACT_ANALYZER_PROMPT');
}else if(cmd==='record'){
  const slot=opt('--slot'),runId=opt('--run'),file=opt('--file'); if(!slot||!runId||!file) fail('record requires --slot --run --file');
  const p=paths(slot),run=read(p.plan).runs.find(x=>x.runId===runId); if(!run) fail('unknown runId');
  fs.mkdirSync(p.runs,{recursive:true}); const metaPath=path.join(p.runs,runId+'.meta.json'); if(fs.existsSync(metaPath)) fail('run already terminal');
  const raw=fs.readFileSync(path.resolve(file),'utf8'); let obj=null,errors=[];
  try{obj=JSON.parse(raw);}catch(e){errors=['invalid JSON: '+e.message];}
  if(obj) errors=validateOutput(run,obj);
  const meta={version:'1.0.0',...run,recordedAt:new Date().toISOString(),promptSha256:shaText(promptFor(run)),rawOutputSha256:shaText(raw),status:errors.length?'invalid-output':'valid-output',validationErrors:errors};
  if(errors.length) fs.writeFileSync(path.join(p.runs,runId+'.invalid.txt'),raw,{flag:'wx'});
  else fs.writeFileSync(path.join(p.runs,runId+'.output.json'),raw,{flag:'wx'});
  fs.writeFileSync(metaPath,JSON.stringify(meta,null,2)+'\n',{flag:'wx'});
  console.log(JSON.stringify({runId,status:meta.status,errors},null,2)); process.exit(errors.length?4:0);
}else if(cmd==='finish'){
  const slot=opt('--slot'); if(!slot) fail('finish requires --slot'); const p=paths(slot);
  const runs=read(p.plan).runs,missing=runs.filter(r=>!fs.existsSync(path.join(p.runs,r.runId+'.meta.json')));
  if(missing.length) fail('incomplete: '+missing.length+' runs remain');
  const metas=runs.map(r=>read(path.join(p.runs,r.runId+'.meta.json')));
  if(fs.existsSync(p.completion)) fail('completion already exists');
  const c={version:'1.0.0',experiment:'EX-EDF-2026-A003',slot,completedAt:new Date().toISOString(),totalRuns:metas.length,validRuns:metas.filter(x=>x.status==='valid-output').length,invalidRuns:metas.filter(x=>x.status!=='valid-output').length};
  fs.writeFileSync(p.completion,JSON.stringify(c,null,2)+'\n',{flag:'wx'}); console.log(JSON.stringify(c,null,2));
}else{console.error('Usage: prepare|bind|next|record|finish');process.exitCode=2;}
