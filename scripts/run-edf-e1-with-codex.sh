#!/usr/bin/env bash
set -uo pipefail
MODEL="${1:-gpt-5.6-terra}"
ROOT="$(git rev-parse --show-toplevel)"
cd "$ROOT"
HELPER="scripts/edf-manual-evaluator-run.mjs"
OUT="research/experiments/EX-EDF-001/manual-evaluations/E1"
fail(){ echo "ERROR: $*" >&2; exit 2; }
command -v codex >/dev/null 2>&1 || fail "Codex CLI is required."
[ "$(git branch --show-current)" = "experiment/edf-e1-openai-evaluator" ] || fail "Wrong branch."
node "$HELPER" verify >/dev/null || exit $?
if [ ! -f "$OUT/evaluator-packet.json" ]; then node "$HELPER" prepare; fi
if [ ! -f "$OUT/evaluator.json" ]; then
  node "$HELPER" bind "$MODEL" "Codex CLI via ChatGPT subscription"
  git add "$OUT/evaluator-packet.json" "$OUT/source-provenance.json" "$OUT/evaluator.json"
  git commit -m "research: freeze E1 blinded evaluation packet"
  git push
else
  BOUND="$(node -e "const fs=require('fs');const j=JSON.parse(fs.readFileSync(process.argv[1],'utf8'));process.stdout.write(j.modelLabel)" "$OUT/evaluator.json")"
  [ "$BOUND" = "$MODEL" ] || fail "E1 already bound to $BOUND, not $MODEL"
fi
while true; do
  NEXT="$(mktemp)"; node "$HELPER" next > "$NEXT"
  if grep -qx "COMPLETE" "$NEXT"; then rm -f "$NEXT"; break; fi
  LABEL="$(sed -n 's/^ANONYMOUS_LABEL=//p' "$NEXT" | head -n 1)"
  [ -n "$LABEL" ] || fail "Could not read anonymous label."
  PROMPT="$(mktemp)"
  awk '/^BEGIN_EXACT_EVALUATOR_PROMPT$/{inside=1;next}/^END_EXACT_EVALUATOR_PROMPT$/{inside=0}inside{print}' "$NEXT" > "$PROMPT"
  rm -f "$NEXT"
  TMP_REPO="$(mktemp -d)"; RAW="$(mktemp)"; ERR="$(mktemp)"
  git -C "$TMP_REPO" init -q
  echo "Evaluating $LABEL with $MODEL in a fresh ephemeral Codex session..."
  (cd "$TMP_REPO"; codex exec --ephemeral --ignore-user-config --ignore-rules --sandbox read-only --model "$MODEL" - < "$PROMPT") > "$RAW" 2> "$ERR"
  RC=$?; rm -rf "$TMP_REPO" "$PROMPT"
  if [ "$RC" -ne 0 ]; then echo "Codex execution failure for $LABEL; no evaluation recorded." >&2; cat "$ERR" >&2; rm -f "$RAW" "$ERR"; exit "$RC"; fi
  rm -f "$ERR"
  set +e; node "$HELPER" record "$LABEL" "$RAW"; REC=$?; set -e
  rm -f "$RAW"
  if [ "$REC" -ne 0 ] && [ "$REC" -ne 4 ]; then exit "$REC"; fi
  git add "$OUT/evaluations"; git commit -m "research: record E1 $LABEL"; git push
done
if [ ! -f "$OUT/completion.json" ]; then
  node "$HELPER" finish
  git add "$OUT/completion.json"; git commit -m "research: complete E1 blinded evaluation"; git push
fi
echo "E1 COMPLETE"; cat "$OUT/completion.json"
