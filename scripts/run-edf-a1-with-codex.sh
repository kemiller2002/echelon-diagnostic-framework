#!/usr/bin/env bash
set -uo pipefail

MODEL="${1:-gpt-5.6-sol}"
ROOT="$(git rev-parse --show-toplevel)"
cd "$ROOT"

HELPER="scripts/edf-manual-agent-run.mjs"
OUT_DIR="research/experiments/EX-EDF-001/manual-runs/A1"
EXECUTOR="$OUT_DIR/executor.json"

fail() {
  echo "ERROR: $*" >&2
  exit 2
}

command -v node >/dev/null 2>&1 || fail "node is required"
command -v git >/dev/null 2>&1 || fail "git is required"
command -v codex >/dev/null 2>&1 || fail "Codex CLI is not installed. Install it, sign in with ChatGPT, then rerun."

BRANCH="$(git branch --show-current)"
[ "$BRANCH" = "experiment/edf-a1-openai-manual" ] || fail "Wrong branch: $BRANCH"

node "$HELPER" verify >/dev/null || exit $?

if [ ! -f "$EXECUTOR" ]; then
  node "$HELPER" bind "$MODEL" "Codex CLI via ChatGPT subscription"
  git add "$EXECUTOR"
  git commit -m "research: bind A1 OpenAI executor"
  git push
else
  BOUND_MODEL="$(node -e "const fs=require('fs');const j=JSON.parse(fs.readFileSync(process.argv[1],'utf8'));process.stdout.write(j.modelLabel)" "$EXECUTOR")"
  [ "$BOUND_MODEL" = "$MODEL" ] || fail "A1 is already bound to $BOUND_MODEL, not $MODEL"
fi

while true; do
  NEXT_FILE="$(mktemp)"
  node "$HELPER" next > "$NEXT_FILE"

  if grep -qx "COMPLETE" "$NEXT_FILE"; then
    rm -f "$NEXT_FILE"
    break
  fi

  RUN_ID="$(sed -n 's/^RUN_ID=//p' "$NEXT_FILE" | head -n 1)"
  [ -n "$RUN_ID" ] || fail "Could not read RUN_ID"

  PROMPT_FILE="$(mktemp)"
  awk '
    /^BEGIN_EXACT_ANALYZER_PROMPT$/ {inside=1; next}
    /^END_EXACT_ANALYZER_PROMPT$/ {inside=0}
    inside {print}
  ' "$NEXT_FILE" > "$PROMPT_FILE"
  rm -f "$NEXT_FILE"

  TMP_REPO="$(mktemp -d)"
  RAW_OUTPUT="$(mktemp)"
  STDERR_FILE="$(mktemp)"
  git -C "$TMP_REPO" init -q

  echo "Running $RUN_ID with $MODEL in a fresh ephemeral Codex session..."

  (
    cd "$TMP_REPO"
    codex exec       --ephemeral       --ignore-user-config       --ignore-rules       --sandbox read-only       --model "$MODEL"       - < "$PROMPT_FILE"
  ) > "$RAW_OUTPUT" 2> "$STDERR_FILE"
  CODEX_EXIT=$?

  rm -rf "$TMP_REPO"
  rm -f "$PROMPT_FILE"

  if [ "$CODEX_EXIT" -ne 0 ]; then
    echo "Codex transport/execution failure for $RUN_ID. No experiment output was recorded." >&2
    cat "$STDERR_FILE" >&2
    rm -f "$RAW_OUTPUT" "$STDERR_FILE"
    exit "$CODEX_EXIT"
  fi
  rm -f "$STDERR_FILE"

  set +e
  node "$HELPER" record "$RUN_ID" "$RAW_OUTPUT"
  RECORD_EXIT=$?
  set -e
  rm -f "$RAW_OUTPUT"

  if [ "$RECORD_EXIT" -ne 0 ] && [ "$RECORD_EXIT" -ne 4 ]; then
    exit "$RECORD_EXIT"
  fi

  git add "$OUT_DIR/runs"
  git commit -m "research: record A1 $RUN_ID"
  git push

  if [ "$RECORD_EXIT" -eq 4 ]; then
    echo "$RUN_ID produced schema-invalid output. It was preserved as terminal data; continuing."
  fi
done

if [ ! -f "$OUT_DIR/completion.json" ]; then
  node "$HELPER" finish
  git add "$OUT_DIR/completion.json"
  git commit -m "research: complete A1 analyzer execution"
  git push
fi

echo
echo "A1 COMPLETE"
node "$HELPER" list
cat "$OUT_DIR/completion.json"
