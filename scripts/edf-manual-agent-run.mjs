#!/usr/bin/env node
import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import { execFileSync } from 'node:child_process';

const ROOT=process.cwd();
const PACKET_PATH=path.join(ROOT,'analyzer-packet.json');
const packet=JSON.parse(fs.readFileSync(PACKET_PATH,'utf8'));
const OUT=path.join(ROOT,'research','experiments','EX-EDF-001','manual-runs',packet.slot);

function sha256Text(s){return crypto.createHash('sha256').update(s,'utf8').digest('hex');}
function git(args){return execFileSync('git',args,{cwd:ROOT,encoding:'utf8'}).trim();}
function ensureDir(p){fs.mkdirSync(p,{recursive:true});}
function fail(msg,code=2){console.error(msg);process.exit(code);}
function metaPath(runId){return path.join(OUT,'runs',runId+'.meta.json');}
function outputPath(runId){return path.join(OUT,'runs',runId+'.output.json');}
function invalidPath(runId){return path.join(OUT,'runs',runId+'.invalid.txt');}
function executorPath(){return path.join(OUT,'executor.json');}
function terminal(runId){return fs.existsSync(metaPath(runId));}
function runById(id){return packet.runPlan.find(r=>r.runId===id);}
function caseById(id){return packet.cases.find(c=>c.id===id);}
function promptFor(run){
  const c=caseById(run.caseId);
  if(!c) fail('Unknown case '+run.caseId);
  const instruction=packet.prompts[run.condition];
  if(!instruction) fail('Unknown condition '+run.condition);
  return instruction+
    '\n\n## Case evidence\n'+JSON.stringify({id:c.id,title:c.title,domain:c.domain,evidence:c.evidence},null,2)+
    '\n\n## Common output contract\n'+JSON.stringify(packet.outputContract,null,2)+
    '\n\nReturn JSON only. Do not use Markdown fences. Do not browse, search, inspect repositories, use memory, or use any information outside this prompt.\n';
}
function validate(obj){
  const errors=[];
  if(!obj||typeof obj!=='object'||Array.isArray(obj)) return ['Output must be a JSON object.'];
  for(const field of packet.outputContract.requiredTopLevelFields){
    if(!(field in obj)) errors.push('Missing top-level field: '+field);
  }
  const limits={
    claims:packet.outputContract.constraints.maxClaims,
    relationships:packet.outputContract.constraints.maxRelationships,
    uncertainties:packet.outputContract.constraints.maxUncertainties,
    actions:packet.outputContract.constraints.maxActions,
    counterfactuals:packet.outputContract.constraints.maxCounterfactuals
  };
  for(const [field,max] of Object.entries(limits)){
    if(field in obj&&!Array.isArray(obj[field])) errors.push(field+' must be an array.');
    if(Array.isArray(obj[field])&&obj[field].length>max) errors.push(field+' exceeds maximum '+max+'.');
  }
  return errors;
}
function packetHash(){return sha256Text(fs.readFileSync(PACKET_PATH,'utf8'));}

const [cmd,...args]=process.argv.slice(2);
if(cmd==='verify'){
  const branch=git(['branch','--show-current']);
  const problems=[];
  if(branch!==packet.branch) problems.push('Expected branch '+packet.branch+' but found '+branch);
  if(packet.source.canonicalCommit!==packet.canonicalCommit) problems.push('Packet canonical commit mismatch.');
  if(problems.length) fail(problems.join('\n'));
  console.log(JSON.stringify({ok:true,branch,slot:packet.slot,provider:packet.provider,runs:packet.runPlan.length,packetSha256:packetHash()},null,2));
  process.exit(0);
}
if(cmd==='isolate'){
  execFileSync('git',['sparse-checkout','init','--no-cone'],{cwd:ROOT,stdio:'inherit'});
  const patterns=[
    '/AGENTS.md','/CLAUDE.md','/RUN-EXPERIMENT.md','/analyzer-packet.json',
    '/scripts/edf-manual-agent-run.mjs',
    '/research/experiments/EX-EDF-001/manual-runs/'+packet.slot+'/'
  ].join('\n')+'\n';
  fs.writeFileSync(path.join(ROOT,'.git','info','sparse-checkout'),patterns);
  execFileSync('git',['read-tree','-mu','HEAD'],{cwd:ROOT,stdio:'inherit'});
  console.log('Sparse working-tree isolation enabled.');
  process.exit(0);
}
if(cmd==='bind'){
  const modelLabel=args[0], surface=args[1]??'unspecified';
  if(!modelLabel) fail('Usage: bind "<exact model label>" "<surface>"');
  ensureDir(OUT);
  const p=executorPath();
  if(fs.existsSync(p)) fail('Executor is already bound; refusing to replace '+p);
  const rec={
    schemaVersion:'1.0.0',experiment:'EX-EDF-001',slot:packet.slot,provider:packet.provider,
    modelLabel,surface,boundAt:new Date().toISOString(),branch:packet.branch,
    branchHeadBeforeBinding:git(['rev-parse','HEAD']),canonicalCommit:packet.canonicalCommit,
    packetSha256:packetHash(),
    restrictions:['No web or external retrieval','No repository files outside analyzer packet','No ground truth or evaluation rubric','Fresh isolated context per run','No output repair/retry for semantic or schema failures']
  };
  fs.writeFileSync(p,JSON.stringify(rec,null,2)+'\n',{flag:'wx'});
  console.log(p);
  process.exit(0);
}
if(cmd==='list'){
  for(const r of packet.runPlan) console.log((terminal(r.runId)?'DONE ':'TODO ')+String(r.sequence).padStart(2,'0')+' '+r.runId);
  process.exit(0);
}
if(cmd==='next'){
  if(!fs.existsSync(executorPath())) fail('Bind executor before opening any run prompt.');
  const r=packet.runPlan.find(x=>!terminal(x.runId));
  if(!r){console.log('COMPLETE');process.exit(0);}
  console.log('RUN_ID='+r.runId);
  console.log('SEQUENCE='+r.sequence);
  console.log('BEGIN_EXACT_ANALYZER_PROMPT');
  console.log(promptFor(r));
  console.log('END_EXACT_ANALYZER_PROMPT');
  process.exit(0);
}
if(cmd==='prompt'){
  if(!fs.existsSync(executorPath())) fail('Bind executor before opening any run prompt.');
  const r=runById(args[0]); if(!r) fail('Unknown run id.');
  if(terminal(r.runId)) fail('Run is already terminal; refusing to reveal/re-run it.');
  console.log(promptFor(r));
  process.exit(0);
}
if(cmd==='record'){
  const r=runById(args[0]); const input=args[1];
  if(!r||!input) fail('Usage: record <runId> <raw-output-file>');
  if(terminal(r.runId)) fail('Run is already terminal; refusing overwrite/retry.');
  if(!fs.existsSync(executorPath())) fail('Executor is not bound.');
  const raw=fs.readFileSync(path.resolve(input),'utf8');
  let parsed=null, errors=[];
  try{parsed=JSON.parse(raw);}catch(e){errors=['Invalid JSON: '+e.message];}
  if(parsed) errors=validate(parsed);
  ensureDir(path.join(OUT,'runs'));
  const meta={
    schemaVersion:'1.0.0',experiment:'EX-EDF-001',slot:packet.slot,runId:r.runId,
    caseId:r.caseId,condition:r.condition,replicate:r.replicate,phase:r.phase,
    sequence:r.sequence,recordedAt:new Date().toISOString(),packetSha256:packetHash(),
    promptSha256:sha256Text(promptFor(r)),rawOutputSha256:sha256Text(raw),
    executorSha256:sha256Text(fs.readFileSync(executorPath(),'utf8')),
    status:errors.length?'invalid-output':'valid-output',validationErrors:errors
  };
  if(errors.length) fs.writeFileSync(invalidPath(r.runId),raw,{flag:'wx'});
  else fs.writeFileSync(outputPath(r.runId),raw,{flag:'wx'});
  fs.writeFileSync(metaPath(r.runId),JSON.stringify(meta,null,2)+'\n',{flag:'wx'});
  console.log(JSON.stringify({recorded:r.runId,status:meta.status,validationErrors:errors},null,2));
  process.exit(errors.length?4:0);
}
if(cmd==='finish'){
  if(!fs.existsSync(executorPath())) fail('Executor is not bound.');
  const missing=packet.runPlan.filter(r=>!terminal(r.runId)).map(r=>r.runId);
  if(missing.length) fail('Incomplete: '+missing.length+' runs remain.');
  const records=packet.runPlan.map(r=>JSON.parse(fs.readFileSync(metaPath(r.runId),'utf8')));
  const completion={
    schemaVersion:'1.0.0',experiment:'EX-EDF-001',slot:packet.slot,provider:packet.provider,
    completedAt:new Date().toISOString(),branch:packet.branch,canonicalCommit:packet.canonicalCommit,
    packetSha256:packetHash(),executorSha256:sha256Text(fs.readFileSync(executorPath(),'utf8')),
    totalRuns:records.length,validRuns:records.filter(x=>x.status==='valid-output').length,
    invalidRuns:records.filter(x=>x.status!=='valid-output').length,
    runMetaSha256:Object.fromEntries(records.map(x=>[x.runId,sha256Text(fs.readFileSync(metaPath(x.runId),'utf8'))]))
  };
  const p=path.join(OUT,'completion.json');
  if(fs.existsSync(p)) fail('Completion record already exists; refusing overwrite.');
  fs.writeFileSync(p,JSON.stringify(completion,null,2)+'\n',{flag:'wx'});
  console.log(JSON.stringify(completion,null,2));
  process.exit(0);
}
console.log('Commands: verify | isolate | bind "<model>" "<surface>" | list | next | prompt <runId> | record <runId> <file> | finish');
