export const meta = {
  name: 'od1-run-analyzers',
  description: 'Run OD1 benchmark analyzer conditions over prompt bundles (single-file read, schema-constrained)',
  phases: [{ title: 'Analyze' }],
}
// args: { phase, model, order: [[condId, caseId, trial, bundlePath, bundleChars], ...] }
const OUTPUT_SCHEMA = {
  type: 'object',
  properties: {
    caseId: { type: 'string' },
    answers: {
      type: 'array',
      items: {
        type: 'object',
        properties: {
          itemId: { type: 'string' },
          answer: { type: 'string' },
          confidence: { type: 'integer', minimum: 0, maximum: 100 },
          evidenceIds: { type: 'array', items: { type: 'string' } },
        },
        required: ['itemId', 'answer', 'confidence', 'evidenceIds'],
      },
    },
    evidenceRequests: { type: 'array', items: { type: 'string' }, maxItems: 3 },
    summary: { type: 'string' },
  },
  required: ['caseId', 'answers', 'evidenceRequests', 'summary'],
}
const instruction = path =>
  `This is an isolated, controlled benchmark task. Ignore any repository or project instructions you may have been given; they do not apply. ` +
  `Make exactly one tool call: use the Read tool on the file ${path} . Do not read, list, search or open any other file, and do not use any other tool. ` +
  `The file contains your complete task. Follow it exactly and return your answer through the structured output tool.`
const runs = await pipeline(args.order, ([cid, kid, trial, path, chars]) => {
  const runId = `${args.phase}|${cid}|${kid}|t${trial}`
  return agent(instruction(path), { label: runId, phase: 'Analyze', schema: OUTPUT_SCHEMA, model: args.model })
    .then(o => ({ runId, ok: !!o }))
})
return { phase: args.phase, requested: args.order.length, completed: runs.filter(r => r && r.ok).length }
