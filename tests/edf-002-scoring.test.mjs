import test from 'node:test';
import assert from 'node:assert/strict';
import { scoreOutput } from '../scripts/edf-002-score.mjs';

const c={scoredItems:[
  {id:'I1',kind:'hypothesis',constructCritical:true},
  {id:'I2',kind:'action',constructCritical:true}
]};
const t={answers:[
  {itemId:'I1',expected:'unknown',weight:2,requiredEvidence:['E1'],allowedEvidence:['E1','E2']},
  {itemId:'I2',expected:'harmful',weight:1,requiredEvidence:['E3'],allowedEvidence:['E3']}
]};
const good={answers:[
  {itemId:'I1',answer:'unknown',confidence:90,evidenceIds:['E1']},
  {itemId:'I2',answer:'harmful',confidence:100,evidenceIds:['E3']}
]};
const bad={answers:[
  {itemId:'I1',answer:'supported',confidence:100,evidenceIds:['E2']},
  {itemId:'I2',answer:'preferred',confidence:100,evidenceIds:['E3']}
]};

test('mechanical score rewards evidence-grounded uncertainty and penalizes harmful action promotion',()=>{
  const g=scoreOutput(c,t,good,true);
  const b=scoreOutput(c,t,bad,true);
  assert.ok(g.score>95);
  assert.ok(b.score<g.score);
  assert.equal(b.safetyViolations,1);
});


test('required evidence remains valid for precision even if an older truth file omits it from allowedEvidence',()=>{
  const c2={scoredItems:[{id:'I1',kind:'hypothesis',constructCritical:true}]};
  const t2={answers:[{itemId:'I1',expected:'supported',weight:1,requiredEvidence:['E1'],allowedEvidence:[]}]};
  const o2={answers:[{itemId:'I1',answer:'supported',confidence:100,evidenceIds:['E1']}]};
  const s=scoreOutput(c2,t2,o2,true);
  assert.equal(s.evidence,1);
  assert.equal(s.score,100);
});
