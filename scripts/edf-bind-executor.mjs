import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';

const ROOT=process.cwd();
const EXP=path.join(ROOT,'research/experiments/EX-EDF-001');
const MATRIX=path.join(EXP,'executor-matrix.json');
const FROZEN=path.join(EXP,'frozen-files.json');
const ADAPTER_REL='scripts/edf-provider-adapter.mjs';
const ADAPTER=path.join(ROOT,ADAPTER_REL);

function arg(name){ const i=process.argv.indexOf(name); return i>=0?process.argv[i+1]:null; }
function has(name){ return process.argv.includes(name); }
function readJson(p){ return JSON.parse(fs.readFileSync(p,'utf8')); }
function sha256File(p){ return crypto.createHash('sha256').update(fs.readFileSync(p)).digest('hex'); }
function gitBlobShaFile(p){ const b=fs.readFileSync(p); const prefix=Buffer.from(`blob ${b.length}\0`); return crypto.createHash('sha1').update(prefix).update(b).digest('hex'); }
function ready(x){ return x && ['provider','model','version','executorConfigPath','executorConfigSha256'].every(k=>typeof x[k]==='string'&&x[k].length>0); }

function providerSlug(provider){
  const p=provider.toLowerCase();
  if(p==='openai') return 'openai';
  if(p==='anthropic') return 'anthropic';
  if(p==='google') return 'google';
  throw new Error('Provider must be OpenAI, Anthropic, or Google for the built-in adapter.');
}

function expectedProvider(matrix,slot){
  const a=matrix.analyzers.find(x=>x.slot===slot);
  return a?.requiredProviderFamily ?? null;
}

const slot=arg('--slot'), provider=arg('--provider'), model=arg('--model'), version=arg('--version');
if(!slot||!provider||!model||!version){
  console.error('Usage: node scripts/edf-bind-executor.mjs --slot A1|A2|A3|E1|E2|E3 --provider OpenAI|Anthropic|Google --model <exact-model-id> --version <frozen-version-description> [--replace]');
  process.exit(2);
}

const matrix=readJson(MATRIX);
const collection=slot.startsWith('A')?matrix.analyzers:matrix.evaluators;
const binding=collection.find(x=>x.slot===slot);
if(!binding) throw new Error('Unknown executor slot: '+slot);
const expected=expectedProvider(matrix,slot);
if(expected && expected!==provider) throw new Error(`${slot} requires provider family ${expected}, not ${provider}.`);
if(ready(binding) && !has('--replace')) throw new Error(`${slot} is already bound. Use --replace only before confirmatory execution and record the reason.`);

const slug=providerSlug(provider);
const e1=matrix.evaluators.find(x=>x.slot==='E1'), e2=matrix.evaluators.find(x=>x.slot==='E2');
const otherEvaluator=slot==='E1'?e2:slot==='E2'?e1:null;
if(otherEvaluator&&ready(otherEvaluator)&&otherEvaluator.provider===provider) throw new Error(`${slot} must use a different provider family from ${otherEvaluator.slot}.`);
const configDir=path.join(EXP,'executor-configs');
fs.mkdirSync(configDir,{recursive:true});
const configRel=`research/experiments/EX-EDF-001/executor-configs/${slot}.json`;
const configPath=path.join(ROOT,configRel);
if(fs.existsSync(configPath) && !has('--replace')) throw new Error(configRel+' already exists. Use --replace only before confirmatory execution.');

const command=['node','{REPO_ROOT}/scripts/edf-provider-adapter.mjs','--provider',slug,'--model',model,'--max-output-tokens','2200'];
if(slug==='anthropic') command.push('--anthropic-version','2023-06-01');
const config={
  configSchemaVersion:'1.0.0',
  slot,
  provider,
  model,
  version,
  command,
  timeoutMs:180000,
  fileDependenciesGitBlobSha:{[ADAPTER_REL]:gitBlobShaFile(ADAPTER)},
  secretEnvironmentVariable:slug==='openai'?'OPENAI_API_KEY':slug==='anthropic'?'ANTHROPIC_API_KEY':'GEMINI_API_KEY',
  note:'Secret value is read only from the process environment and must never be committed.'
};
fs.writeFileSync(configPath,JSON.stringify(config,null,2)+'\n');
const configHash=sha256File(configPath);

binding.provider=provider;
binding.model=model;
binding.version=version;
binding.executorConfigPath=configRel;
binding.executorConfigSha256=configHash;

if(ready(e1)&&ready(e2)&&e1.provider===e2.provider) throw new Error('E1 and E2 must use different provider families.');

const analyzersReady=matrix.analyzers.length===3&&matrix.analyzers.every(ready);
const evaluatorsReady=[e1,e2].every(ready);
matrix.status=analyzersReady&&evaluatorsReady?'confirmatory-ready':'unbound';
fs.writeFileSync(MATRIX,JSON.stringify(matrix,null,2)+'\n');

if(matrix.status==='confirmatory-ready'){
  const frozen=readJson(FROZEN);
  frozen.sha256??={};
  frozen.sha256['research/experiments/EX-EDF-001/executor-matrix.json']=sha256File(MATRIX);
  fs.writeFileSync(FROZEN,JSON.stringify(frozen,null,2)+'\n');
}

console.log(JSON.stringify({
  slot,provider,model,version,executorConfigPath:configRel,executorConfigSha256:configHash,
  adapterGitBlobSha:config.fileDependenciesGitBlobSha[ADAPTER_REL],
  matrixStatus:matrix.status,
  matrixFrozen:matrix.status==='confirmatory-ready'
},null,2));
