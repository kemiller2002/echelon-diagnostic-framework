#!/usr/bin/env node
import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import { execFileSync } from 'node:child_process';

const ROOT=process.cwd();
const SETUP=JSON.parse(fs.readFileSync(path.join(ROOT,'evaluator-setup.json'),'utf8'));
const EXP=path.join(ROOT,'research','experiments','EX-EDF-001');
const OUT=path.join(EXP,'manual-evaluations',SETUP.slot);
const PACKET=path.join(OUT,'evaluator-packet.json');
const PROVENANCE=path.join(OUT,'source-provenance.json');
const EVALUATOR=path.join(OUT,'evaluator.json');
const EVALS=path.join(OUT,'evaluations');
const COMPLETION=path.join(OUT,'completion.json');

function sha256Text(s){return crypto.createHash('sha256').update(s,'utf8').digest('hex');}
function git(args,opts={}){return execFileSync('git',args,{cwd:ROOT,encoding:'utf8',maxBuffer:32*1024*1024,...opts}).trim();}
function fail(msg,code=2){console.error(msg);process.exit(code);}
function readJson(p){return JSON.parse(fs.readFileSync(p,'utf8'));}
function ensureDir(p){fs.mkdirSync(p,{recursive:true});}
function show(ref,p){return git(['show',ref+':'+p]);}
function remoteRef(branch){return 'refs/remotes/origin/'+branch;}
function evalMeta(label){return path.join(EVALS,label+'.meta.json');}
function evalOutput(label){return path.join(EVALS,label+'.output.json');}
function evalInvalid(label){return path.join(EVALS,label+'.invalid.txt');}
function terminal(label){return fs.existsSync(evalMeta(label));}
function packetHash(){return sha256Text(fs.readFileSync(PACKET,'utf8'));}

function sourceHead(branch){return git(['rev-parse',remoteRef(branch)]);}
function fetchSource(branch){
  execFileSync('git',['fetch','origin','+refs/heads/'+branch+':'+remoteRef(branch)],{cwd:ROOT,stdio:'ignore'});
}
function listPrimaryOutputs(src){
  const ref=remoteRef(src.branch);
  const names=git(['ls-tree','-r','--name-only',ref,src.dir+'/runs']).split(/\r?\n/).filter(Boolean);
  return names.filter(x=>/-r1\.output\.json$/.test(x));
}
function sanitizedCase(c){return {id:c.id,title:c.title,domain:c.domain,evidence:c.evidence};}
function expectedDims(rubric){return rubric.dimensions.map(x=>x.id);}

function prepare(){
  if(fs.existsSync(PACKET)||fs.existsSync(PROVENANCE)) fail('Evaluator packet/provenance already exists; refusing regeneration.');
  const random=readJson(path.join(EXP,'randomization.json'));
  const cases=readJson(path.join(EXP,'cases.json')).cases.map(sanitizedCase);
  const truth=readJson(path.join(EXP,'ground-truth.json')).cases;
  const rubric=readJson(path.join(EXP,'evaluation-rubric.json'));
  const seed=random.seed;
  const items=[];
  const provSources=[];
  const labels=new Set();

  for(const src of SETUP.sourceBranches){
    fetchSource(src.branch);
    const ref=remoteRef(src.branch);
    const completionRaw=show(ref,src.dir+'/completion.json');
    const completion=JSON.parse(completionRaw);
    if(completion.totalRuns!==24||completion.validRuns!==24||completion.invalidRuns!==0){
      fail('Source '+src.slot+' is not a clean 24/24 completion.');
    }
    const files=listPrimaryOutputs(src);
    if(files.length!==SETUP.expectedPrimaryPerAnalyzer) fail('Source '+src.slot+' primary output count '+files.length+' != '+SETUP.expectedPrimaryPerAnalyzer);
    const sourceHashes=[];
    for(const file of files){
      const runId=path.basename(file).replace(/\.output\.json$/,'');
      const m=runId.match(/^(SYN-\d{3})-A\d+-.+-r1$/);
      if(!m) fail('Unexpected primary run id '+runId);
      const caseId=m[1];
      const raw=show(ref,file);
      let output; try{output=JSON.parse(raw);}catch(e){fail('Source output is not JSON: '+file);}
      const anonymousLabel=sha256Text(seed+'|'+runId+'|blind').slice(0,12);
      if(labels.has(anonymousLabel)) fail('Anonymous label collision: '+anonymousLabel);
      labels.add(anonymousLabel);
      const outputSha256=sha256Text(raw);
      sourceHashes.push(outputSha256);
      items.push({anonymousLabel,caseId,outputSha256,output});
    }
    provSources.push({
      sourceSlot:src.slot,
      provider:src.provider,
      sourceBranch:src.branch,
      sourceHead:sourceHead(src.branch),
      completionSha256:sha256Text(completionRaw),
      primaryOutputCount:files.length,
      primaryOutputSetSha256:sha256Text(sourceHashes.sort().join('\n'))
    });
  }

  if(items.length!==SETUP.expectedTotalItems) fail('Anonymous item count mismatch: '+items.length);
  items.sort((a,b)=>sha256Text(seed+'|'+SETUP.slot+'|'+a.anonymousLabel+'|order').localeCompare(sha256Text(seed+'|'+SETUP.slot+'|'+b.anonymousLabel+'|order')));
  const ordered=items.map((x,i)=>({sequence:i+1,...x}));
  const packet={
    schemaVersion:'1.0.0',
    experiment:SETUP.experiment,
    role:'blinded-evaluator',
    evaluatorSlot:SETUP.slot,
    evaluatorProvider:SETUP.provider,
    itemCount:ordered.length,
    blinding:'No analyzer identity, condition label, run id, source branch, or source model is included in evaluator items.',
    outputContract:{
      dimensionScores:Object.fromEntries(expectedDims(rubric).map(x=>[x,'integer 0..4'])),
      flags:rubric.secondaryFlags,
      rationale:'short string'
    },
    cases,
    groundTruth:truth,
    rubric,
    items:ordered
  };
  const provenance={
    schemaVersion:'1.0.0',
    experiment:SETUP.experiment,
    evaluatorSlot:SETUP.slot,
    generatedAt:new Date().toISOString(),
    randomizationSeed:seed,
    sources:provSources,
    note:'Coordinator-only provenance. Never include it in evaluator prompts.'
  };
  ensureDir(OUT);
  fs.writeFileSync(PACKET,JSON.stringify(packet,null,2)+'\n',{flag:'wx'});
  fs.writeFileSync(PROVENANCE,JSON.stringify(provenance,null,2)+'\n',{flag:'wx'});
  console.log(JSON.stringify({prepared:true,slot:SETUP.slot,items:ordered.length,packetSha256:packetHash()},null,2));
}

function validateEval(obj,packet){
  const errors=[];
  if(!obj||typeof obj!=='object'||Array.isArray(obj)) return ['Evaluation must be a JSON object.'];
  const dims=packet.rubric.dimensions.map(x=>x.id);
  if(!obj.dimensionScores||typeof obj.dimensionScores!=='object'||Array.isArray(obj.dimensionScores)) errors.push('dimensionScores must be an object.');
  else for(const d of dims){
    const v=obj.dimensionScores[d];
    if(!Number.isInteger(v)||v<0||v>4) errors.push('dimensionScores.'+d+' must be an integer 0..4.');
  }
  if(!Array.isArray(obj.flags)) errors.push('flags must be an array.');
  else for(const f of obj.flags) if(!packet.rubric.secondaryFlags.includes(f)) errors.push('Unknown flag: '+f);
  if(typeof obj.rationale!=='string'||obj.rationale.length===0) errors.push('rationale must be a non-empty string.');
  return errors;
}

function promptFor(item,packet){
  const c=packet.cases.find(x=>x.id===item.caseId);
  const gt=packet.groundTruth.find(x=>x.id===item.caseId);
  if(!c||!gt) fail('Missing case/truth for '+item.caseId);
  return [
    '# Blinded diagnostic evaluation','',
    'You are evaluating one anonymous analysis. You are not told which analyzer, provider, model, or prompt condition produced it.',
    'Do not infer or guess the condition or analyzer identity. Score only the semantic quality of the anonymous analysis against the supplied evidence, hidden ground truth, and frozen rubric.',
    'Do not use web search, external facts, repository information, prior conversations, memory, or other evaluations.','',
    'Return JSON only with exactly these top-level fields:',
    '- dimensionScores: object keyed by each rubric dimension id, integer 0..4',
    '- flags: array containing only allowed secondary flags',
    '- rationale: concise explanation of the scores and flags','',
    '## Anonymous label',item.anonymousLabel,'',
    '## Case evidence',JSON.stringify(c,null,2),'',
    '## Hidden ground truth',JSON.stringify(gt,null,2),'',
    '## Frozen rubric',JSON.stringify(packet.rubric,null,2),'',
    '## Anonymous analysis',JSON.stringify(item.output,null,2),'',
    'Evaluate the anonymous analysis now. Return JSON only; no Markdown fences.'
  ].join('\n');
}

const [cmd,...args]=process.argv.slice(2);
if(cmd==='verify'){
  const branch=git(['branch','--show-current']);
  if(branch!==SETUP.branch) fail('Expected branch '+SETUP.branch+' but found '+branch);
  console.log(JSON.stringify({ok:true,branch,slot:SETUP.slot,provider:SETUP.provider},null,2));
  process.exit(0);
}
if(cmd==='prepare'){prepare();process.exit(0);}
if(cmd==='bind'){
  if(!fs.existsSync(PACKET)) fail('Prepare evaluator packet first.');
  const modelLabel=args[0],surface=args[1]??'unspecified';
  if(!modelLabel) fail('Usage: bind "<exact model label>" "<surface>"');
  if(fs.existsSync(EVALUATOR)) fail('Evaluator already bound; refusing replacement.');
  const rec={schemaVersion:'1.0.0',experiment:SETUP.experiment,slot:SETUP.slot,provider:SETUP.provider,modelLabel,surface,boundAt:new Date().toISOString(),branch:SETUP.branch,branchHeadBeforeBinding:git(['rev-parse','HEAD']),packetSha256:packetHash(),restrictions:['No web/external retrieval','Fresh isolated context per evaluation','No analyzer identity or condition mapping','No repair/retry for semantic or schema-invalid evaluator output']};
  fs.writeFileSync(EVALUATOR,JSON.stringify(rec,null,2)+'\n',{flag:'wx'});
  console.log(EVALUATOR); process.exit(0);
}
if(cmd==='list'){
  if(!fs.existsSync(PACKET)) fail('Prepare evaluator packet first.');
  const packet=readJson(PACKET);
  for(const item of packet.items) console.log((terminal(item.anonymousLabel)?'DONE ':'TODO ')+String(item.sequence).padStart(2,'0')+' '+item.anonymousLabel+' '+item.caseId);
  process.exit(0);
}
if(cmd==='next'){
  if(!fs.existsSync(PACKET)||!fs.existsSync(EVALUATOR)) fail('Prepare and bind evaluator before opening an item.');
  const packet=readJson(PACKET);
  const item=packet.items.find(x=>!terminal(x.anonymousLabel));
  if(!item){console.log('COMPLETE');process.exit(0);}
  console.log('ANONYMOUS_LABEL='+item.anonymousLabel);
  console.log('SEQUENCE='+item.sequence);
  console.log('BEGIN_EXACT_EVALUATOR_PROMPT');
  console.log(promptFor(item,packet));
  console.log('END_EXACT_EVALUATOR_PROMPT');
  process.exit(0);
}
if(cmd==='record'){
  if(!fs.existsSync(PACKET)||!fs.existsSync(EVALUATOR)) fail('Prepare and bind evaluator first.');
  const label=args[0],input=args[1];
  if(!label||!input) fail('Usage: record <anonymousLabel> <raw-output-file>');
  const packet=readJson(PACKET);
  const item=packet.items.find(x=>x.anonymousLabel===label);
  if(!item) fail('Unknown anonymous label.');
  if(terminal(label)) fail('Evaluation already terminal; refusing overwrite/retry.');
  const raw=fs.readFileSync(path.resolve(input),'utf8');
  let parsed=null,errors=[];
  try{parsed=JSON.parse(raw);}catch(e){errors=['Invalid JSON: '+e.message];}
  if(parsed) errors=validateEval(parsed,packet);
  ensureDir(EVALS);
  const meta={schemaVersion:'1.0.0',experiment:SETUP.experiment,evaluatorSlot:SETUP.slot,anonymousLabel:label,caseId:item.caseId,sequence:item.sequence,recordedAt:new Date().toISOString(),packetSha256:packetHash(),promptSha256:sha256Text(promptFor(item,packet)),rawOutputSha256:sha256Text(raw),evaluatorSha256:sha256Text(fs.readFileSync(EVALUATOR,'utf8')),status:errors.length?'invalid-output':'valid-output',validationErrors:errors};
  if(!errors.length){
    meta.totalScore=packet.rubric.dimensions.reduce((s,d)=>s+(parsed.dimensionScores[d.id]*5),0);
    fs.writeFileSync(evalOutput(label),raw,{flag:'wx'});
  }else fs.writeFileSync(evalInvalid(label),raw,{flag:'wx'});
  fs.writeFileSync(evalMeta(label),JSON.stringify(meta,null,2)+'\n',{flag:'wx'});
  console.log(JSON.stringify({recorded:label,status:meta.status,totalScore:meta.totalScore??null,validationErrors:errors},null,2));
  process.exit(errors.length?4:0);
}
if(cmd==='finish'){
  if(!fs.existsSync(PACKET)||!fs.existsSync(EVALUATOR)) fail('Prepare and bind evaluator first.');
  const packet=readJson(PACKET);
  const missing=packet.items.filter(x=>!terminal(x.anonymousLabel));
  if(missing.length) fail('Incomplete: '+missing.length+' evaluations remain.');
  const metas=packet.items.map(x=>readJson(evalMeta(x.anonymousLabel)));
  const completion={schemaVersion:'1.0.0',experiment:SETUP.experiment,slot:SETUP.slot,provider:SETUP.provider,completedAt:new Date().toISOString(),branch:SETUP.branch,packetSha256:packetHash(),evaluatorSha256:sha256Text(fs.readFileSync(EVALUATOR,'utf8')),totalEvaluations:metas.length,validEvaluations:metas.filter(x=>x.status==='valid-output').length,invalidEvaluations:metas.filter(x=>x.status!=='valid-output').length,evaluationMetaSha256:Object.fromEntries(metas.map(x=>[x.anonymousLabel,sha256Text(fs.readFileSync(evalMeta(x.anonymousLabel),'utf8'))]))};
  if(fs.existsSync(COMPLETION)) fail('Completion already exists; refusing overwrite.');
  fs.writeFileSync(COMPLETION,JSON.stringify(completion,null,2)+'\n',{flag:'wx'});
  console.log(JSON.stringify(completion,null,2)); process.exit(0);
}
console.log('Commands: verify | prepare | bind "<model>" "<surface>" | list | next | record <label> <file> | finish');
