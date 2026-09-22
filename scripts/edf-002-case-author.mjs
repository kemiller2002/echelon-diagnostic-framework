#!/usr/bin/env node
import fs from 'node:fs';
import path from 'node:path';
import { validatePair } from './edf-002-validate.mjs';

const ROOT=process.cwd();
const EXP=path.join(ROOT,'research','experiments','EX-EDF-2026-A003');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8'));
const assignments=read(path.join(EXP,'case-authoring-assignments.json'));
const schema=read(path.join(EXP,'case-schema.json'));
const prompts=read(path.join(EXP,'prompt-modules.json'));
const prereg=read(path.join(EXP,'preregistration.json'));

function assignment(id){
  const a=assignments.assignments.find(x=>x.caseId===id);
  if(!a) throw new Error('Unknown case ID '+id);
  return a;
}
function providerAssignments(provider){
  return assignments.assignments.filter(x=>x.authorFamily.toLowerCase()===provider.toLowerCase());
}
function authorPrompt(id){
  const a=assignment(id);
  const critical=prompts.construct[a.construct];
  return [
    '# EX-EDF-2026-A003 independent case authoring',
    '',
    'You are authoring a hidden-ground-truth diagnostic stress case. You are NOT executing the benchmark and must not simulate analyzer results.',
    '',
    'Case ID: '+a.caseId,
    'Assigned construct: '+a.construct,
    'Hypothesis: '+a.hypothesis,
    'Author family: '+a.authorFamily,
    '',
    'The analyzer-visible case must not contain author identity, expected answers, weights, evidence roles, scoring thresholds, or ground truth.',
    '',
    'Target semantic pressure for this construct:',
    critical,
    '',
    'Hard requirements:',
    JSON.stringify(assignments.hardRequirements,null,2),
    '',
    'Case/truth schema:',
    JSON.stringify(schema,null,2),
    '',
    'Primary experiment context (for design only; do not optimize against thresholds):',
    'The benchmark compares a generic neutral reasoning module with the construct-specific module. The case must make the construct genuinely necessary rather than rewarding verbosity or keyword matching.',
    '',
    'Write exactly two files:',
    '1. research/experiments/EX-EDF-2026-A003/cases/'+id+'.case.json',
    '2. research/experiments/EX-EDF-2026-A003/cases/'+id+'.truth.json',
    '',
    'Design constraints:',
    '- Use a realistic software/operational system, not trivia.',
    '- Include plausible red herrings and conflicting evidence.',
    '- Include at least one attractive but harmful or mechanism-worsening action.',
    '- Include at least two items whose correct state is unknown because discriminating evidence is genuinely absent.',
    '- Candidate wording must be neutral and parallel; no answer should be signaled lexically.',
    '- Every hidden expected answer must be justified by evidence actually present or by an explicit absence/unknown encoded in the case.',
    '- At least four scored items must specifically stress the assigned construct.',
    '- Prefer evidence chains that require at least three reasoning hops.',
    '- Do not mention EDF in the case.',
    '',
    'After writing both files run:',
    'node scripts/edf-002-case-author.mjs validate '+id,
    '',
    'Revise until validation passes. Do not inspect or author cases assigned to the other provider family.'
  ].join('\n');
}

const [cmd,...args]=process.argv.slice(2);
if(cmd==='list'){
  const provider=args[0];
  const xs=provider?providerAssignments(provider):assignments.assignments;
  console.log(JSON.stringify(xs,null,2));
}else if(cmd==='prompt'){
  console.log(authorPrompt(args[0]));
}else if(cmd==='validate'){
  const id=args[0]; assignment(id);
  const cp=path.join(EXP,'cases',id+'.case.json');
  const tp=path.join(EXP,'cases',id+'.truth.json');
  if(!fs.existsSync(cp)||!fs.existsSync(tp)){console.error('Missing case/truth pair for '+id);process.exitCode=1;}
  else {
    const errors=validatePair(read(cp),read(tp));
    if(errors.length){for(const e of errors)console.error(e);process.exitCode=1;}
    else console.log(JSON.stringify({ok:true,caseId:id},null,2));
  }
}else{
  console.error('Usage: node scripts/edf-002-case-author.mjs list [OpenAI|Anthropic] | prompt <caseId> | validate <caseId>');
  process.exitCode=2;
}
