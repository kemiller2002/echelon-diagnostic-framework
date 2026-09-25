#!/usr/bin/env node
import fs from 'node:fs';
import path from 'node:path';

const ROOT=process.cwd();
const EXP=path.join(ROOT,'research','experiments','EX-EDF-2026-A003');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8'));
const norm=s=>String(s).replace(/\s+/g,' ').trim();

export function validateSpec(){
  const errors=[];
  const a=read(path.join(EXP,'case-authoring-assignments.json'));
  const p=read(path.join(EXP,'prompt-modules.json'));
  if(a.assignments.length!==12) errors.push('assignments must contain exactly 12 cases');
  const constructs=[...new Set(a.assignments.map(x=>x.construct))];
  if(constructs.length!==6) errors.push('assignments must cover exactly six constructs');
  for(const c of constructs){
    const xs=a.assignments.filter(x=>x.construct===c);
    if(xs.length!==2) errors.push(c+': expected two assignments');
    if(new Set(xs.map(x=>x.authorFamily)).size!==2) errors.push(c+': expected OpenAI and Anthropic authors');
    const n=norm(p.neutral[c]??'').length, k=norm(p.construct[c]??'').length;
    if(!n||!k) errors.push(c+': missing prompt module');
    else if(Math.max(n,k)/Math.min(n,k)>1.20) errors.push(c+': prompt module lengths differ by more than 20%');
    if(/\bEDF\b/i.test(p.neutral[c]??'')||/\bEDF\b/i.test(p.construct[c]??'')) errors.push(c+': condition module names EDF');
  }
  return errors;
}

export function validatePair(caseDoc,truth){
  const errors=[];
  const a=read(path.join(EXP,'case-authoring-assignments.json'));
  const spec=a.hardRequirements;
  if(caseDoc.id!==truth.caseId) errors.push('caseId mismatch');
  const assignment=a.assignments.find(x=>x.caseId===caseDoc.id);
  if(!assignment) errors.push('unknown assignment '+caseDoc.id);
  else {
    if(caseDoc.construct!==assignment.construct) errors.push('construct mismatch');
    if(truth.authorFamily!==assignment.authorFamily) errors.push('authorFamily mismatch');
  }
  for(const f of ['authorFamily','groundTruth','expected','weight','evidenceRole']) if(Object.hasOwn(caseDoc,f)) errors.push('case leaks forbidden field '+f);
  if((caseDoc.evidence??[]).length<spec.minimumEvidenceItems) errors.push('too few evidence items');
  if((caseDoc.candidates?.hypotheses??[]).length<spec.minimumCandidateHypotheses) errors.push('too few candidate hypotheses');
  if((caseDoc.candidates?.edges??[]).length<spec.minimumCandidateEdges) errors.push('too few candidate edges');
  if((caseDoc.candidates?.actions??[]).length<spec.minimumCandidateActions) errors.push('too few candidate actions');
  if((caseDoc.scoredItems??[]).length<spec.minimumScoredItems) errors.push('too few scored items');
  if((caseDoc.scoredItems??[]).filter(x=>x.constructCritical).length<spec.minimumConstructCriticalItems) errors.push('too few construct-critical items');
  const evidenceIds=new Set((caseDoc.evidence??[]).map(x=>x.id));
  const itemIds=new Set((caseDoc.scoredItems??[]).map(x=>x.id));
  if(evidenceIds.size!==(caseDoc.evidence??[]).length) errors.push('duplicate evidence IDs');
  if(itemIds.size!==(caseDoc.scoredItems??[]).length) errors.push('duplicate scored-item IDs');
  const truthAnswers=truth.answers??[];
  const truthIds=truthAnswers.map(x=>x.itemId);
  if(new Set(truthIds).size!==truthIds.length) errors.push('duplicate truth item IDs');
  const truthBy=new Map(truthAnswers.map(x=>[x.itemId,x]));
  for(const item of caseDoc.scoredItems??[]){
    const gt=truthBy.get(item.id);
    if(!gt){errors.push('missing truth for '+item.id);continue;}
    if(!(item.allowedAnswers??[]).includes(gt.expected)) errors.push('truth answer not allowed for '+item.id);
    for(const e of [...(gt.requiredEvidence??[]),...(gt.allowedEvidence??[])]) if(!evidenceIds.has(e)) errors.push(item.id+': unknown evidence '+e);
    const allowed=new Set(gt.allowedEvidence??[]);
    for(const e of gt.requiredEvidence??[]) if(!allowed.has(e)) errors.push(item.id+': required evidence must also be allowed '+e);
  }
  if(truthBy.size!==(caseDoc.scoredItems??[]).length) errors.push('truth/scored-item cardinality mismatch');
  if((truth.answers??[]).filter(x=>x.expected==='unknown').length<spec.minimumExpectedUnknownItems) errors.push('too few expected-unknown items');
  const redHerrings=truth.hardness?.redHerringEvidenceIds??[];
  if(redHerrings.length<spec.minimumRedHerringEvidenceItems) errors.push('too few red herrings');
  for(const e of redHerrings) if(!evidenceIds.has(e)) errors.push('unknown red-herring evidence '+e);
  const conflicts=truth.hardness?.conflictingEvidencePairs??[];
  if(conflicts.length<spec.minimumConflictingEvidencePairs) errors.push('too few conflicting evidence pairs');
  for(const pair of conflicts) for(const e of pair??[]) if(!evidenceIds.has(e)) errors.push('unknown conflicting evidence '+e);
  if(Number(truth.hardness?.reasoningHops??0)<spec.minimumReasoningHops) errors.push('reasoning chain too short');
  const actionIds=new Set((caseDoc.candidates?.actions??[]).map(x=>x.id));
  const harmful=truth.hardness?.harmfulActionCandidateIds??[];
  if(harmful.length<spec.minimumHarmfulCandidateActions) errors.push('no harmful candidate action');
  for(const id of harmful) if(!actionIds.has(id)) errors.push('unknown harmful action '+id);
  return errors;
}

export function validateCases(requireComplete=false){
  const dir=path.join(EXP,'cases');
  if(!fs.existsSync(dir)) return requireComplete?['case directory missing']:[];
  const files=fs.readdirSync(dir).filter(x=>x.endsWith('.case.json')).sort();
  const errors=[];
  if(requireComplete&&files.length!==12) errors.push('freeze requires 12 cases; found '+files.length);
  for(const f of files){
    const tp=path.join(dir,f.replace(/\.case\.json$/,'.truth.json'));
    if(!fs.existsSync(tp)){errors.push(f+': missing truth file');continue;}
    const c=read(path.join(dir,f)),t=read(tp);
    for(const e of validatePair(c,t)) errors.push(f+': '+e);
  }
  return errors;
}

if(import.meta.url===`file://${process.argv[1]}`){
  const mode=process.argv[2]??'validate';
  const errors=[...validateSpec(),...validateCases(mode==='freeze')];
  if(errors.length){for(const e of errors)console.error(e);process.exitCode=1;}
  else console.log(JSON.stringify({ok:true,mode},null,2));
}
