import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';
import { spawnSync } from 'node:child_process';

const ROOT=process.cwd();
const EXP=path.join(ROOT,'research/experiments/EX-EDF-001');
const OUT=path.join(EXP,'exploratory','openai-only-2026-09-21');
const ADAPTER=path.join(ROOT,'scripts','edf-provider-adapter.mjs');
const ANALYZER='gpt-5.6-sol';
const E1='gpt-5.6-terra';
const E2='gpt-5.6-luna';
const E3='gpt-5.6-sol';

function readJson(p){return JSON.parse(fs.readFileSync(p,'utf8'));}
function writeJson(p,v){fs.mkdirSync(path.dirname(p),{recursive:true});fs.writeFileSync(p,JSON.stringify(v,null,2)+'\n');}
function sha256(s){return crypto.createHash('sha256').update(s).digest('hex');}
function totalScore(result,rubric){return rubric.dimensions.reduce((sum,d)=>sum+(result.dimensionScores[d.id]*5),0);}
function mean(xs){return xs.reduce((a,b)=>a+b,0)/xs.length;}
function median(xs){const a=[...xs].sort((x,y)=>x-y);return a.length%2?a[(a.length-1)/2]:(a[a.length/2-1]+a[a.length/2])/2;}
function rng(seed){let x=parseInt(sha256(seed).slice(0,8),16)>>>0;return()=>{x^=x<<13;x^=x>>>17;x^=x<<5;return(x>>>0)/4294967296;};}
function bootstrap(values,seed,n=10000){const r=rng(seed),draws=[];for(let i=0;i<n;i++){let s=0;for(let j=0;j<values.length;j++)s+=values[Math.floor(r()*values.length)];draws.push(s/values.length);}draws.sort((a,b)=>a-b);return{lower:draws[Math.floor(n*.025)],upper:draws[Math.floor(n*.975)]};}
function order(seed,caseId){return ['natural','matched-neutral','edf'].map(c=>[sha256(`${seed}|A1|${caseId}|${c}`),c]).sort((a,b)=>a[0].localeCompare(b[0])).map(x=>x[1]);}
function anon(seed,runId){return sha256(`${seed}|${runId}|blind`).slice(0,12);}

function callOpenAI(model,prompt,effort='medium'){
  const started=Date.now();
  const r=spawnSync('node',[ADAPTER,'--provider','openai','--model',model,'--max-output-tokens','2600','--reasoning-effort',effort],{
    cwd:ROOT,input:prompt,encoding:'utf8',env:process.env,maxBuffer:20*1024*1024,timeout:240000
  });
  const ended=Date.now();
  if(r.status!==0) throw new Error(`OpenAI adapter failed for ${model}: ${(r.stderr??'').slice(0,2000)}`);
  const metaLine=(r.stderr??'').split(/\r?\n/).find(x=>x.startsWith('EDF_ADAPTER_META '));
  let metadata=null;if(metaLine){try{metadata=JSON.parse(metaLine.slice('EDF_ADAPTER_META '.length));}catch{}}
  return {text:(r.stdout??'').trim(),metadata,latencyMs:ended-started};
}

function parseJson(text,label){
  try{return JSON.parse(text);}catch(e){throw new Error(`${label} returned invalid JSON: ${e.message}\n${text.slice(0,1000)}`);}
}

const cases=readJson(path.join(EXP,'cases.json')).cases;
const prompts=readJson(path.join(EXP,'prompts.json')).prompts;
const contract=readJson(path.join(EXP,'output-contract.json'));
const truth=readJson(path.join(EXP,'ground-truth.json')).cases;
const rubric=readJson(path.join(EXP,'evaluation-rubric.json'));
const randomization=readJson(path.join(EXP,'randomization.json'));
const seed=randomization.seed;

function analyzerPrompt(condition,c){
  return `${prompts[condition]}\n\n## Case evidence\n${JSON.stringify({id:c.id,title:c.title,domain:c.domain,evidence:c.evidence},null,2)}\n\n## Common output contract\n${JSON.stringify(contract,null,2)}\n\nUse arrays for claims, relationships, uncertainties, actions, and counterfactuals. Include evidence IDs where useful. Return JSON only; no Markdown fences.\n`;
}
function evaluationPrompt(run,output){
  const c=cases.find(x=>x.id===run.caseId), gt=truth.find(x=>x.id===run.caseId);
  return `Evaluate the anonymous analysis against the supplied evidence and hidden ground truth. Do not infer the prompt condition. Use only the rubric. Return JSON only with: dimensionScores (object keyed by rubric dimension id, integer 0..4), flags (array chosen from rubric secondaryFlags), rationale (short string).\n\nCASE:\n${JSON.stringify(c,null,2)}\n\nHIDDEN GROUND TRUTH:\n${JSON.stringify(gt,null,2)}\n\nRUBRIC:\n${JSON.stringify(rubric,null,2)}\n\nANONYMOUS OUTPUT ${run.anonymousLabel}:\n${JSON.stringify(output,null,2)}\n`;
}

if(!process.env.OPENAI_API_KEY){
  console.error('OPENAI_API_KEY is not available to this workflow.');
  process.exit(3);
}

fs.mkdirSync(OUT,{recursive:true});
const manifest={
  id:'EX-EDF-001-OPENAI-EXPLORATORY-001',
  status:'single-provider-non-confirmatory',
  date:'2026-09-21',
  sourceExperiment:'EX-EDF-001',
  repositoryCommit:process.env.GITHUB_SHA??null,
  analyzer:{provider:'OpenAI',model:ANALYZER},
  evaluators:[{slot:'OE1',provider:'OpenAI',model:E1},{slot:'OE2',provider:'OpenAI',model:E2},{slot:'OE3',provider:'OpenAI',model:E3,conditional:true}],
  limitations:[
    'Single provider family only.',
    'Evaluator models share provider lineage with analyzer.',
    'Cannot promote HY-EDF-008 or satisfy EX-EDF-001 confirmatory criteria.',
    'Purpose is exploratory signal plus end-to-end harness validation.'
  ]
};
writeJson(path.join(OUT,'manifest.json'),manifest);

const primaryRuns=[];
const repeatRuns=[];
for(const c of cases){
  for(const condition of order(seed,c.id)){
    const runId=`${c.id}-OA1-${condition}-r1`;
    const prompt=analyzerPrompt(condition,c);
    const call=callOpenAI(ANALYZER,prompt,'high');
    const output=parseJson(call.text,runId);
    const run={runId,caseId:c.id,condition,replicate:1,anonymousLabel:anon(seed,runId),model:ANALYZER,promptSha256:sha256(prompt),latencyMs:call.latencyMs,providerMetadata:call.metadata,output};
    primaryRuns.push(run);writeJson(path.join(OUT,'runs',runId+'.json'),run);
  }
  if(['SYN-001','SYN-004'].includes(c.id)){
    for(const condition of order(seed,c.id)){
      const runId=`${c.id}-OA1-${condition}-r2`;
      const prompt=analyzerPrompt(condition,c);
      const call=callOpenAI(ANALYZER,prompt,'high');
      const output=parseJson(call.text,runId);
      const run={runId,caseId:c.id,condition,replicate:2,anonymousLabel:anon(seed,runId),model:ANALYZER,promptSha256:sha256(prompt),latencyMs:call.latencyMs,providerMetadata:call.metadata,output};
      repeatRuns.push(run);writeJson(path.join(OUT,'runs',runId+'.json'),run);
    }
  }
}

const evals=[];
for(const run of [...primaryRuns].sort((a,b)=>sha256(`${seed}|OE1|${a.anonymousLabel}|order`).localeCompare(sha256(`${seed}|OE1|${b.anonymousLabel}|order`)))){
  const prompt=evaluationPrompt(run,run.output);
  const a=callOpenAI(E1,prompt,'medium'), b=callOpenAI(E2,prompt,'medium');
  const e1=parseJson(a.text,run.runId+' OE1'), e2=parseJson(b.text,run.runId+' OE2');
  const s1=totalScore(e1,rubric), s2=totalScore(e2,rubric);
  let e3=null,s3=null,finalScore;
  if(Math.abs(s1-s2)>=20){
    const cc=callOpenAI(E3,prompt,'medium'); e3=parseJson(cc.text,run.runId+' OE3'); s3=totalScore(e3,rubric); finalScore=median([s1,s2,s3]);
  }else finalScore=mean([s1,s2]);
  const item={runId:run.runId,anonymousLabel:run.anonymousLabel,caseId:run.caseId,condition:run.condition,scores:{OE1:s1,OE2:s2,OE3:s3,final:finalScore},evaluations:{OE1:e1,OE2:e2,OE3:e3}};
  evals.push(item);writeJson(path.join(OUT,'evaluations',run.runId+'.json'),item);
}

const paired=[];
for(const c of cases){
  const edf=evals.find(x=>x.caseId===c.id&&x.condition==='edf');
  const neutral=evals.find(x=>x.caseId===c.id&&x.condition==='matched-neutral');
  paired.push({caseId:c.id,edf:edf.scores.final,matchedNeutral:neutral.scores.final,difference:edf.scores.final-neutral.scores.final});
}
const diffs=paired.map(x=>x.difference);
const naturalMean=mean(evals.filter(x=>x.condition==='natural').map(x=>x.scores.final));
const neutralMean=mean(evals.filter(x=>x.condition==='matched-neutral').map(x=>x.scores.final));
const edfMean=mean(evals.filter(x=>x.condition==='edf').map(x=>x.scores.final));
const ci=bootstrap(diffs,seed+'|openai-exploratory-bootstrap');
const repeatability=[];
for(const caseId of ['SYN-001','SYN-004']){
  for(const condition of ['natural','matched-neutral','edf']){
    const a=primaryRuns.find(x=>x.caseId===caseId&&x.condition===condition);
    const b=repeatRuns.find(x=>x.caseId===caseId&&x.condition===condition);
    repeatability.push({caseId,condition,primarySha256:sha256(JSON.stringify(a.output)),repeatSha256:sha256(JSON.stringify(b.output)),exactMatch:JSON.stringify(a.output)===JSON.stringify(b.output)});
  }
}
const summary={
  status:'complete-single-provider-exploratory',
  primaryRuns:primaryRuns.length,
  repeatabilityRuns:repeatRuns.length,
  evaluatorCalls:evals.reduce((n,x)=>n+2+(x.scores.OE3==null?0:1),0),
  means:{natural:naturalMean,matchedNeutral:neutralMean,edf:edfMean},
  pairedEdfMinusMatchedNeutral:paired,
  meanDifference:mean(diffs),
  bootstrap95:ci,
  preregisteredMarginReference:5,
  crossesFivePointMargin:mean(diffs)>=5,
  lowerBoundAboveZero:ci.lower>0,
  interpretation:'Exploratory only. This single-provider result cannot promote or falsify HY-EDF-008 under EX-EDF-001.'
};
writeJson(path.join(OUT,'summary.json'),summary);
writeJson(path.join(OUT,'repeatability.json'),repeatability);
console.log('EDF_OPENAI_EXPLORATORY_SUMMARY '+JSON.stringify(summary));
