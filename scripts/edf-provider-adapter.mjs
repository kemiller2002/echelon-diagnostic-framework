import fs from 'node:fs';

function arg(name) {
  const i = process.argv.indexOf(name);
  return i >= 0 ? process.argv[i + 1] : null;
}

function required(value, message) {
  if (!value) throw new Error(message);
  return value;
}

function readPrompt() {
  return fs.readFileSync(0, 'utf8');
}

function metadata(value) {
  process.stderr.write('EDF_ADAPTER_META ' + JSON.stringify(value) + '\n');
}

async function postJson(url, headers, body) {
  const response = await fetch(url, {
    method: 'POST',
    headers: { 'content-type': 'application/json', ...headers },
    body: JSON.stringify(body)
  });
  const text = await response.text();
  let json;
  try { json = JSON.parse(text); }
  catch { throw new Error(`Provider returned non-JSON HTTP ${response.status}: ${text.slice(0, 500)}`); }
  if (!response.ok) {
    const message = json?.error?.message ?? json?.message ?? text.slice(0, 500);
    throw new Error(`Provider HTTP ${response.status}: ${message}`);
  }
  return { json, headers: response.headers, status: response.status };
}

function openAiText(json) {
  const blocks = [];
  for (const item of json.output ?? []) {
    if (item.type !== 'message') continue;
    for (const content of item.content ?? []) {
      if (content.type === 'output_text' && typeof content.text === 'string') blocks.push(content.text);
    }
  }
  return blocks.join('');
}

async function callOpenAI(prompt, model, maxOutputTokens) {
  const key = required(process.env.OPENAI_API_KEY, 'OPENAI_API_KEY is required.');
  const body = { model, input: prompt, max_output_tokens: maxOutputTokens };
  const effort = arg('--reasoning-effort');
  if (effort) body.reasoning = { effort };
  const { json, headers, status } = await postJson(
    'https://api.openai.com/v1/responses',
    { authorization: `Bearer ${key}` },
    body
  );
  const text = openAiText(json);
  if (!text) throw new Error('OpenAI response contained no output_text content.');
  return {
    text,
    meta: {
      provider: 'OpenAI',
      api: 'responses',
      httpStatus: status,
      requestId: headers.get('x-request-id'),
      responseId: json.id ?? null,
      returnedModel: json.model ?? model,
      usage: json.usage ?? null
    }
  };
}

async function callAnthropic(prompt, model, maxOutputTokens) {
  const key = required(process.env.ANTHROPIC_API_KEY, 'ANTHROPIC_API_KEY is required.');
  const apiVersion = arg('--anthropic-version') ?? '2023-06-01';
  const { json, headers, status } = await postJson(
    'https://api.anthropic.com/v1/messages',
    { 'x-api-key': key, 'anthropic-version': apiVersion },
    { model, max_tokens: maxOutputTokens, messages: [{ role: 'user', content: prompt }] }
  );
  const text = (json.content ?? []).filter(x => x.type === 'text').map(x => x.text ?? '').join('');
  if (!text) throw new Error('Anthropic response contained no text content.');
  return {
    text,
    meta: {
      provider: 'Anthropic',
      api: 'messages',
      apiVersion,
      httpStatus: status,
      requestId: headers.get('request-id'),
      responseId: json.id ?? null,
      returnedModel: json.model ?? model,
      usage: json.usage ?? null,
      stopReason: json.stop_reason ?? null
    }
  };
}

async function callGoogle(prompt, model, maxOutputTokens) {
  const key = required(process.env.GEMINI_API_KEY, 'GEMINI_API_KEY is required.');
  const encodedModel = encodeURIComponent(model);
  const { json, headers, status } = await postJson(
    `https://generativelanguage.googleapis.com/v1beta/models/${encodedModel}:generateContent`,
    { 'x-goog-api-key': key },
    {
      contents: [{ role: 'user', parts: [{ text: prompt }] }],
      generationConfig: { maxOutputTokens }
    }
  );
  const text = (json.candidates ?? [])
    .flatMap(x => x.content?.parts ?? [])
    .map(x => x.text ?? '')
    .join('');
  if (!text) throw new Error('Gemini response contained no text content.');
  return {
    text,
    meta: {
      provider: 'Google',
      api: 'generateContent-v1beta',
      httpStatus: status,
      requestId: headers.get('x-request-id'),
      responseId: json.responseId ?? null,
      returnedModel: json.modelVersion ?? model,
      usage: json.usageMetadata ?? null
    }
  };
}

export async function executeProvider({ provider, model, maxOutputTokens, prompt }) {
  if (provider === 'openai') return callOpenAI(prompt, model, maxOutputTokens);
  if (provider === 'anthropic') return callAnthropic(prompt, model, maxOutputTokens);
  if (provider === 'google') return callGoogle(prompt, model, maxOutputTokens);
  throw new Error('Unsupported provider: ' + provider);
}

async function main() {
  const provider = required(arg('--provider'), '--provider is required.').toLowerCase();
  const model = required(arg('--model'), '--model is required.');
  const maxOutputTokens = Number(arg('--max-output-tokens') ?? '2200');
  if (!Number.isInteger(maxOutputTokens) || maxOutputTokens < 1) throw new Error('--max-output-tokens must be a positive integer.');
  const prompt = readPrompt();
  const result = await executeProvider({ provider, model, maxOutputTokens, prompt });
  metadata(result.meta);
  process.stdout.write(result.text);
  if (!result.text.endsWith('\n')) process.stdout.write('\n');
}

if (import.meta.url === `file://${process.argv[1]}`) {
  main().catch(error => {
    process.stderr.write((error?.stack ?? String(error)) + '\n');
    process.exitCode = 1;
  });
}
