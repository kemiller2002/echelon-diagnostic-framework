#!/usr/bin/env bash
set -uo pipefail
MODEL="${1:-gpt-5.6-sol}"
ROOT="$(git rev-parse --show-toplevel)"
cd "$ROOT"
BRANCH="experiment/edf-002-case-author-openai"
EXP="research/experiments/EX-EDF-2026-A003"
IDS=(ST-001 ST-003 ST-005 ST-007 ST-009 ST-011)
fail(){ echo "ERROR: $*" >&2; exit 2; }
command -v codex >/dev/null 2>&1 || fail "Codex CLI is required."
[ "$(git branch --show-current)" = "$BRANCH" ] || fail "Wrong branch."
for ID in "${IDS[@]}"; do
  CASE="$EXP/cases/$ID.case.json"
  TRUTH="$EXP/cases/$ID.truth.json"
  if [ -f "$CASE" ] && [ -f "$TRUTH" ]; then
    node scripts/edf-002-case-author.mjs validate "$ID" || fail "$ID exists but is invalid."
    echo "$ID already valid; skipping."
    continue
  fi
  PROMPT="$(mktemp)"
  node scripts/edf-002-case-author.mjs prompt "$ID" > "$PROMPT"
  cat >> "$PROMPT" <<'EOF'

ISOLATED-WORKSPACE OVERRIDE:
You are currently in an empty temporary Git repository. Do not attempt to access the EDF repository or any external source.
Write the analyzer-visible case to ./case.json and the hidden truth to ./truth.json instead of the repository paths named earlier.
Do not place Markdown fences around either file. Both files must be strict JSON.
EOF
  TMP="$(mktemp -d)"
  git -C "$TMP" init -q
  echo "Authoring $ID with $MODEL in a fresh isolated session..."
  (cd "$TMP" && codex exec --ephemeral --ignore-user-config --ignore-rules --sandbox workspace-write --model "$MODEL" - < "$PROMPT")
  RC=$?
  rm -f "$PROMPT"
  [ "$RC" -eq 0 ] || { rm -rf "$TMP"; exit "$RC"; }
  [ -f "$TMP/case.json" ] || { rm -rf "$TMP"; fail "$ID did not produce case.json"; }
  [ -f "$TMP/truth.json" ] || { rm -rf "$TMP"; fail "$ID did not produce truth.json"; }
  cp "$TMP/case.json" "$CASE"
  cp "$TMP/truth.json" "$TRUTH"
  rm -rf "$TMP"
  if ! node scripts/edf-002-case-author.mjs validate "$ID"; then
    echo "$ID failed mechanical validation. Files are left uncommitted at $CASE and $TRUTH." >&2
    exit 4
  fi
  git add "$CASE" "$TRUTH"
  git commit -m "research: author OpenAI stress case $ID"
  git push
done
echo "OPENAI CASE AUTHORING COMPLETE"
node scripts/edf-002-validate.mjs validate
