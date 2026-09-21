import test from "node:test";
import assert from "node:assert/strict";
import { executeProvider } from "../scripts/edf-provider-adapter.mjs";

test("provider adapter rejects unsupported providers without network access", async () => {
  await assert.rejects(
    () => executeProvider({ provider: "unsupported", model: "none", maxOutputTokens: 1, prompt: "x" }),
    /Unsupported provider/
  );
});
