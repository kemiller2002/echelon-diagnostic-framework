import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import { discoverRunRecords } from '../scripts/edf-002-score.mjs';

test('scorer discovers analyzer metadata under manual-runs slot directories',()=>{
  const root=fs.mkdtempSync(path.join(os.tmpdir(),'edf-a003-runs-'));
  try{
    const runs=path.join(root,'manual-runs','openai','runs');
    fs.mkdirSync(runs,{recursive:true});
    const meta={
      runId:'ST-001-openai-neutral-r1',
      caseId:'ST-001',
      analyzerSlot:'openai',
      condition:'neutral',
      replicate:1,
      status:'valid-output'
    };
    fs.writeFileSync(path.join(runs,meta.runId+'.meta.json'),JSON.stringify(meta));
    fs.writeFileSync(path.join(runs,meta.runId+'.output.json'),'{}');
    const records=discoverRunRecords(root);
    assert.equal(records.length,1);
    assert.equal(records[0].run.runId,meta.runId);
    assert.equal(records[0].outputPath,path.join(runs,meta.runId+'.output.json'));
  }finally{
    fs.rmSync(root,{recursive:true,force:true});
  }
});
