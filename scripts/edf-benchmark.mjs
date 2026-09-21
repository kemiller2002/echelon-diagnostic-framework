import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import os from 'node:os';
import { spawnSync } from 'node:child_process';

const ROOT = process.cwd();
const EXP = path.join(ROOT, 'research/experiments/EX-EDF-001');
const RUNS = path.join(EXP, 'runs');
const EVALS = path.join(EXP, 'evaluations');

function readJson(p){ return JSON.parse(fs.readFileSync(p,'utf8')); }
function sha256Bytes(b){ return crypto.createHash('sha256').update(b).digest('hex'); }
function sha256File(p){ return sha256Bytes(fs.readFileSync(p)); }
function sha256Text(s){ return sha256Bytes(Buffer.from(s,'utf8')); }
function gitBlobShaFile(p){ const b=fs.readFileSync(p); const prefix=Buffer.from(`blob ${b.length}\\0`); return crypto.createHash('sha1').update(prefix).update(b).digest('hex'); }
function ensureDir(p){ fs.mkdirSync(p,{recursive:true}); }
function now(){ return new Date().toISOString(); }
function safeId(s){ return s.replace(/[^A-Za-z0-9._-]+/g,'-'); }
function option(args,name){ const i=args.indexOf(name); return i>=0 ? args[i+1] : null; }
function has(args,name){ return args.includes(name); }

export function benchmarkPaths(root=process.cwd()){
  const exp=path.join(root,'research/experiments/EX-EDF-001');
  return {
    exp,
    prereg:path.join(exp,'preregistration.json'), cases:path.join(exp,'cases.json'), truth:path.join(exp,'ground-truth.json'), prompts:path.join(exp,'prompts.json'), contract:path.join(exp,'output-contract.json'), rubric:path.join(exp,'evaluation-rubric.json'), matrix:path.join(exp,'executor-matrix.json'), frozen:path.join(exp,'frozen-files.json'), randomization:path.join(exp,'randomization.json')
  };
}

function executorReady(x){
  return x && ['provider','model','version','executorConfigPath','executorConfigSha256'].every(k => typeof x[k]==='string' && x[k].length>0);
}

export function validateBenchmark(root=process.cwd()){
  const p=benchmarkPaths(root); const errors=[];
  const required=Object.entries(p).filter(([k])=>k!=='exp');
  for(const [k,v] of required) if(!fs.existsSync(v)) errors.push(`Missing benchmark artifact ${k}: ${path.relative(root,v)}`);
  if(errors.length) return {errors,summary:{experimentId:'EX-EDF-001',cases:0,conditions:0,frozenArtifacts:0,executorMatrixReady:false,analyzerRuns:0}};
  let prereg,cases,prompts,contract,rubric,matrix,frozen,randomization;
  try{prereg=readJson(p.prereg);cases=readJson(p.cases);prompts=readJson(p.prompts);contract=readJson(p.contract);rubric=readJson(p.rubric);matrix=readJson(p.matrix);frozen=readJson(p.frozen);randomization=readJson(p.randomization);}catch(e){errors.push('Cannot parse benchmark JSON: '+e.message);}
  if(prereg){
    if(prereg.id!=='EX-EDF-001') errors.push('Experiment ID drift.');
    if(prereg.primaryHypothesis!=='HY-EDF-008') errors.push('Primary hypothesis must remain HY-EDF-008.');
    if(prereg.design?.primaryCases!==6) errors.push('Primary case count must remain 6.');
    if(prereg.primaryOutcome?.margin!==5) errors.push('Primary margin must remain 5 points.');
  }
  const ids=(cases?.cases??[]).map(x=>x.id); if(new Set(ids).size!==ids.length) errors.push('Duplicate case IDs.');
  if(ids.length!==6) errors.push(`Expected 6 cases, found ${ids.length}.`);
  const conditions=Object.keys(prompts?.prompts??{}); for(const c of ['natural','matched-neutral','edf']) if(!conditions.includes(c)) errors.push('Missing prompt condition: '+c);
  if(prompts){
    const neutral=prompts.prompts['matched-neutral']??''; const edf=prompts.prompts.edf??'';
    if(neutral.length < edf.length*0.9) errors.push('Matched-neutral prompt is materially shorter than EDF prompt; control is too weak.');
  }
  const rubricText=JSON.stringify(rubric??{}).toLowerCase();
  for(const forbidden of ['origin network','propagation','control point','negative knowledge']) if(rubricText.includes(forbidden)) errors.push('Evaluator rubric leaks EDF-specific vocabulary: '+forbidden);
  const weights=(rubric?.dimensions??[]).reduce((a,d)=>a+(d.weight??0),0); if(weights!==100) errors.push('Rubric weights must sum to 100.');
  if((contract?.requiredTopLevelFields??[]).length!==6) errors.push('Output contract field count drift.');
  for(const [rel,expected] of Object.entries(frozen?.sha256??{})){
    const full=path.join(root,rel); if(!fs.existsSync(full)) errors.push('Missing frozen artifact: '+rel); else if(sha256File(full)!==expected) errors.push('Frozen benchmark artifact hash drift: '+rel);
  }
  for(const [rel,expected] of Object.entries(frozen?.gitBlobSha??{})){
    const full=path.join(root,rel); if(!fs.existsSync(full)) errors.push('Missing frozen code artifact: '+rel); else if(gitBlobShaFile(full)!==expected) errors.push('Frozen benchmark code blob drift: '+rel);
  }
  const analyzers=matrix?.analyzers??[], evaluators=matrix?.evaluators??[];
  const analyzerReady=analyzers.length===3 && analyzers.every(executorReady);
  const evalReady=evaluators.filter(x=>x.slot==='E1'||x.slot==='E2').length===2 && evaluators.filter(x=>x.slot==='E1'||x.slot==='E2').every(executorReady);
  const declaredReady=matrix?.status==='confirmatory-ready';
  if(declaredReady && !(analyzerReady&&evalReady)) errors.push('Executor matrix claims confirmatory-ready but required bindings are incomplete.');
  if(!declaredReady && analyzerReady&&evalReady) errors.push('Executor bindings are complete but matrix status is not confirmatory-ready.');
  const seed=randomization?.seed; if(typeof seed!=='string'||seed.length<10) errors.push('Randomization seed missing.');
  let analyzerRuns=0; if(fs.existsSync(path.join(root,'research/experiments/EX-EDF-001/runs'))) analyzerRuns=fs.readdirSync(path.join(root,'research/experiments/EX-EDF-001/runs')).filter(x=>x.endsWith('.json')).length;
  return {errors,summary:{experimentId:'EX-EDF-001',cases:ids.length,conditions:conditions.length,frozenArtifacts:Object.keys(frozen?.sha256??{}).length+Object.keys(frozen?.gitBlobSha??{}).length,executorMatrixReady:declaredReady&&analyzerReady&&evalReady,analyzerRuns}};
}

function composeAnalyzerPrompt(condition,caseObj){
  const prompts=readJson(path.join(EXP,'prompts.json')).prompts;
  const contract=readJson(path.join(EXP,'output-contract.json'));
  return `${prompts[condition]}\n\n## Case evidence\n${JSON.stringify({id:caseObj.id,title:caseObj.title,domain:caseObj.domain,evidence:caseObj.evidence},null,2)}\n\n## Common output contract\n${JSON.stringify(contract,null,2)}\n`;
}

function loadExecutorConfig(rel,binding){
  const full=path.resolve(ROOT,rel); const cfg=readJson(full);
  if(!Array.isArray(cfg.command) || cfg.command.length<1) throw new Error('Executor config command must be a non-empty string array.');
  for(const key of ['provider','model','version']) if(cfg[key]!==binding[key]) throw new Error(`Executor config ${key} does not match frozen binding for ${binding.slot}.`);
  for(const [depRel,expected] of Object.entries(cfg.fileDependenciesGitBlobSha??{})){
    const dep=path.resolve(ROOT,depRel);
    if(!fs.existsSync(dep)) throw new Error('Executor dependency missing: '+depRel);
    if(gitBlobShaFile(dep)!==expected) throw new Error('Executor dependency blob drift: '+depRel);
  }
  return {full,cfg};
}

function findSlot(matrix,slot,kind){ return (kind==='analyzer'?matrix.analyzers:matrix.evaluators).find(x=>x.slot===slot); }

function parseAdapterMetadata(stderr){
  const line=(stderr??'').split(/\r?\n/).find(x=>x.startsWith('EDF_ADAPTER_META '));
  if(!line) return null;
  try{return JSON.parse(line.slice('EDF_ADAPTER_META '.length));}catch{return {parseError:true,raw:line};}
}

function executeCommand(binding,input,attemptId){
  const {full,cfg}=loadExecutorConfig(binding.executorConfigPath,binding);
  const actualHash=sha256File(full); if(actualHash!==binding.executorConfigSha256) throw new Error(`Executor config hash mismatch for ${binding.slot}`);
  const cwd=fs.mkdtempSync(path.join(os.tmpdir(),'edf-exec-'));
  const env={...process.env,...(cfg.env??{})};
  const command=cfg.command.map(x=>x.replaceAll('{REPO_ROOT}',ROOT));
  const started=Date.now();
  const r=spawnSync(command[0],command.slice(1),{cwd,input,encoding:'utf8',env,maxBuffer:10*1024*1024,timeout:cfg.timeoutMs??180000});
  const ended=Date.now(); const stderr=r.stderr??''; const adapterMetadata=parseAdapterMetadata(stderr);
  return {attemptId,startedAt:new Date(started).toISOString(),endedAt:new Date(ended).toISOString(),latencyMs:ended-started,exitCode:r.status,signal:r.signal,stdout:r.stdout??'',stderr,error:r.error?String(r.error):null,adapterMetadata,executorConfigPath:binding.executorConfigPath,executorConfigSha256:actualHash,provider:binding.provider,model:binding.model,version:binding.version};
}

function validateAnalyzerOutput(text){
  const contract=readJson(path.join(EXP,'output-contract.json')); let obj;
  try{obj=JSON.parse(text);}catch(e){return {valid:false,errors:['invalid-json: '+e.message],object:null};}
  const errors=[]; for(const f of contract.requiredTopLevelFields) if(!(f in obj)) errors.push('missing-field:'+f);
  const c=contract.constraints;
  for(const [f,max] of [['claims',c.maxClaims],['relationships',c.maxRelationships],['uncertainties',c.maxUncertainties],['actions',c.maxActions],['counterfactuals',c.maxCounterfactuals]]) if(Array.isArray(obj[f])&&obj[f].length>max) errors.push(`too-many-${f}`);
  return {valid:errors.length===0,errors,object:obj};
}

function matrixReadyOrThrow(){
  const v=validateBenchmark(ROOT); if(v.errors.length) throw new Error(v.errors.join('\n'));
  if(!v.summary.executorMatrixReady) throw new Error('Confirmatory execution blocked: executor-matrix.json is not confirmatory-ready.');
}

function runAnalyzer(args){
  matrixReadyOrThrow();
  const slot=option(args,'--executor'), caseId=option(args,'--case'), condition=option(args,'--condition'), replicate=Number(option(args,'--replicate')??1);
  if(!slot||!caseId||!condition) throw new Error('run requires --executor, --case, and --condition.');
  const cases=readJson(path.join(EXP,'cases.json')).cases; const caseObj=cases.find(x=>x.id===caseId); if(!caseObj) throw new Error('Unknown case '+caseId);
  if(!['natural','matched-neutral','edf'].includes(condition)) throw new Error('Unknown condition '+condition);
  const matrix=readJson(path.join(EXP,'executor-matrix.json')); const binding=findSlot(matrix,slot,'analyzer'); if(!binding) throw new Error('Unknown analyzer slot '+slot);
  const prompt=composeAnalyzerPrompt(condition,caseObj);
  const runId=safeId(`${caseId}-${slot}-${condition}-r${replicate}`); const attemptId=`${runId}-${Date.now()}`;
  const execution=executeCommand(binding,prompt,attemptId); const checked=validateAnalyzerOutput(execution.stdout.trim());
  const seed=readJson(path.join(EXP,'randomization.json')).seed; const anonymousLabel=sha256Text(`${seed}|${runId}|blind`).slice(0,12);
  const bundle={experimentId:'EX-EDF-001',runId,anonymousLabel,caseId,executorSlot:slot,condition,replicate,repositoryCommit:process.env.GITHUB_SHA??null,promptSha256:sha256Text(prompt),caseSha256:sha256Text(JSON.stringify(caseObj)),attempt:execution,validation:{valid:checked.valid,errors:checked.errors},included:checked.valid,exclusionReason:checked.valid?null:'invalid analyzer output'};
  ensureDir(RUNS); fs.writeFileSync(path.join(RUNS,runId+'.json'),JSON.stringify(bundle,null,2)+'\n');
  if(checked.valid) fs.writeFileSync(path.join(RUNS,runId+'.output.json'),JSON.stringify(checked.object,null,2)+'\n');
  console.log(JSON.stringify({runId,anonymousLabel,valid:checked.valid,errors:checked.errors},null,2));
}

function composeEvaluationPrompt(run,output){
  const cases=readJson(path.join(EXP,'cases.json')).cases; const caseObj=cases.find(x=>x.id===run.caseId);
  const truth=readJson(path.join(EXP,'ground-truth.json')).cases.find(x=>x.id===run.caseId); const rubric=readJson(path.join(EXP,'evaluation-rubric.json'));
  return `Evaluate the anonymous analysis against the supplied case evidence and hidden ground truth. Do not infer or guess the analysis condition. Return JSON only with fields: dimensionScores (object keyed by rubric dimension id, integer 0..4), flags (array of rubric secondary flag strings), rationale (short string).\n\nCASE:\n${JSON.stringify(caseObj,null,2)}\n\nHIDDEN GROUND TRUTH:\n${JSON.stringify(truth,null,2)}\n\nRUBRIC:\n${JSON.stringify(rubric,null,2)}\n\nANONYMOUS OUTPUT ${run.anonymousLabel}:\n${JSON.stringify(output,null,2)}\n`;
}

function validateEvaluation(obj,rubric){
  const errors=[]; const ids=rubric.dimensions.map(x=>x.id); if(!obj||typeof obj!=='object') return ['evaluation-not-object'];
  for(const id of ids){ const v=obj.dimensionScores?.[id]; if(!Number.isInteger(v)||v<0||v>4) errors.push('invalid-score:'+id); }
  if(!Array.isArray(obj.flags)) errors.push('flags-not-array'); return errors;
}

function runEvaluation(args){
  matrixReadyOrThrow(); const slot=option(args,'--executor'), runId=option(args,'--run'); if(!slot||!runId) throw new Error('evaluate requires --executor and --run.');
  const matrix=readJson(path.join(EXP,'executor-matrix.json')); const binding=findSlot(matrix,slot,'evaluator'); if(!binding||!executorReady(binding)) throw new Error('Evaluator slot not bound: '+slot);
  const run=readJson(path.join(RUNS,runId+'.json')); if(!run.included) throw new Error('Cannot evaluate excluded analyzer run.'); const output=readJson(path.join(RUNS,runId+'.output.json'));
  const prompt=composeEvaluationPrompt(run,output); const attemptId=`eval-${slot}-${runId}-${Date.now()}`; const execution=executeCommand(binding,prompt,attemptId);
  let parsed=null,errors=[]; try{parsed=JSON.parse(execution.stdout.trim());}catch(e){errors.push('invalid-json:'+e.message);} const rubric=readJson(path.join(EXP,'evaluation-rubric.json')); if(parsed) errors.push(...validateEvaluation(parsed,rubric));
  const bundle={experimentId:'EX-EDF-001',runId,evaluatorSlot:slot,anonymousLabel:run.anonymousLabel,attempt:execution,validation:{valid:errors.length===0,errors},result:errors.length===0?parsed:null};
  ensureDir(EVALS); fs.writeFileSync(path.join(EVALS,`${runId}.${slot}.json`),JSON.stringify(bundle,null,2)+'\n'); console.log(JSON.stringify({runId,evaluatorSlot:slot,valid:errors.length===0,errors},null,2));
}

function totalScore(evalObj,rubric){ return rubric.dimensions.reduce((sum,d)=>sum+(evalObj.dimensionScores[d.id]*5),0); }
function mean(xs){ return xs.length?xs.reduce((a,b)=>a+b,0)/xs.length:null; }
function median(xs){ const a=[...xs].sort((x,y)=>x-y); return a.length%2?a[(a.length-1)/2]:(a[a.length/2-1]+a[a.length/2])/2; }
function seeded(seed){ let x=parseInt(sha256Text(seed).slice(0,8),16)>>>0; return ()=>{ x^=x<<13; x^=x>>>17; x^=x<<5; return (x>>>0)/4294967296; }; }
function bootstrapCI(values,seed,n=10000){ if(!values.length) return null; const rnd=seeded(seed); const draws=[]; for(let i=0;i<n;i++){let s=0;for(let j=0;j<values.length;j++)s+=values[Math.floor(rnd()*values.length)];draws.push(s/values.length);}draws.sort((a,b)=>a-b);return {lower:draws[Math.floor(n*0.025)],upper:draws[Math.floor(n*0.975)]}; }

export function analyzeBenchmark(root=process.cwd()){
  const exp=path.join(root,'research/experiments/EX-EDF-001'), runsDir=path.join(exp,'runs'), evalDir=path.join(exp,'evaluations'); const prereg=readJson(path.join(exp,'preregistration.json')), rubric=readJson(path.join(exp,'evaluation-rubric.json')), random=readJson(path.join(exp,'randomization.json'));
  if(!fs.existsSync(runsDir)||!fs.existsSync(evalDir)) return {status:'not-evaluable',reason:'No complete run/evaluation set exists.',primaryUnits:0,requiredPrimaryUnits:18};
  const runs=fs.readdirSync(runsDir).filter(f=>f.endsWith('.json')&&!f.endsWith('.output.json')).map(f=>readJson(path.join(runsDir,f))).filter(r=>r.included&&r.replicate===1);
  const evaluations=fs.readdirSync(evalDir).filter(f=>f.endsWith('.json')).map(f=>readJson(path.join(evalDir,f))).filter(e=>e.validation?.valid);
  const runScores=new Map();
  for(const run of runs){ const es=evaluations.filter(e=>e.runId===run.runId); const bySlot=Object.fromEntries(es.map(e=>[e.evaluatorSlot,totalScore(e.result,rubric)])); if(bySlot.E1==null||bySlot.E2==null) continue; let score;if(Math.abs(bySlot.E1-bySlot.E2)>=20){if(bySlot.E3==null)continue;score=median([bySlot.E1,bySlot.E2,bySlot.E3]);}else score=mean([bySlot.E1,bySlot.E2]); runScores.set(run.runId,{run,score,flags:es.flatMap(e=>e.result.flags??[])}); }
  const diffs=[], byExecutor={};
  for(const caseId of readJson(path.join(exp,'cases.json')).cases.map(x=>x.id)) for(const slot of ['A1','A2','A3']){ const edf=[...runScores.values()].find(x=>x.run.caseId===caseId&&x.run.executorSlot===slot&&x.run.condition==='edf'); const neutral=[...runScores.values()].find(x=>x.run.caseId===caseId&&x.run.executorSlot===slot&&x.run.condition==='matched-neutral'); if(edf&&neutral){const d=edf.score-neutral.score;diffs.push(d);(byExecutor[slot]??=[]).push(d);} }
  if(diffs.length<18) return {status:'not-evaluable',reason:'Primary paired unit set incomplete.',primaryUnits:diffs.length,requiredPrimaryUnits:18};
  const ci=bootstrapCI(diffs,random.seed+'|bootstrap'); const executorMeans=Object.fromEntries(Object.entries(byExecutor).map(([k,v])=>[k,mean(v)])); const positiveExecutors=Object.values(executorMeans).filter(x=>x>0).length;
  const materialFlags=new Set(['outside-evidence-use','forbidden-claim']); let edfFlagged=0,neutralFlagged=0; for(const x of runScores.values()){if(x.run.condition==='edf'&&x.flags.some(f=>materialFlags.has(f)))edfFlagged++; if(x.run.condition==='matched-neutral'&&x.flags.some(f=>materialFlags.has(f)))neutralFlagged++;}
  const meanDiff=mean(diffs); const success=meanDiff>=prereg.primaryOutcome.margin && ci.lower>0 && positiveExecutors>=2 && edfFlagged<=neutralFlagged+1;
  return {status:'evaluable',primaryUnits:18,meanDifference:meanDiff,bootstrap95:ci,executorMeanDifferences:executorMeans,positiveExecutors,materialFlaggedUnits:{edf:edfFlagged,matchedNeutral:neutralFlagged},success,claimInterpretation:success?'HY-EDF-008 may be considered for bounded promotion after human review of protocol conformance.':'HY-EDF-008 is not promoted by this result; apply preregistered falsification interpretation.'};
}

function validateCmd(){ const r=validateBenchmark(ROOT); if(r.errors.length){console.error(r.errors.join('\n'));process.exitCode=1;} console.log(JSON.stringify(r.summary,null,2)); }
function analyzeCmd(){ console.log(JSON.stringify(analyzeBenchmark(ROOT),null,2)); }

if(import.meta.url===`file://${process.argv[1]}`){
  const [cmd,...args]=process.argv.slice(2); try{ if(cmd==='validate') validateCmd(); else if(cmd==='run') runAnalyzer(args); else if(cmd==='evaluate') runEvaluation(args); else if(cmd==='analyze') analyzeCmd(); else {console.error('Usage: node scripts/edf-benchmark.mjs validate|run|evaluate|analyze');process.exitCode=2;} }catch(e){console.error(e.stack??String(e));process.exitCode=1;}
}
