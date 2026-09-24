#!/usr/bin/env bash
set -euo pipefail

report="$1"
[ -f "$report" ] || exit 0

total=$(jq '.results.summary.tests' "$report")

echo
echo "<details open><summary>All tests ($total)</summary>"
echo
echo "| | Test | Duration |"
echo "|---|---|---:|"
jq -r '
  .results.tests
  | sort_by(.extra.type, .name)[]
  | . as $t
  | [
      (if $t.status == "passed" then "✅" elif $t.status == "failed" then "❌" else "⚪" end),
      (($t.extra.type | split(".") | last) + " › " + ($t.name | ltrimstr($t.extra.type + ".")) | gsub("\\|"; "\\|")),
      "\($t.duration) ms"
    ]
  | "| " + join(" | ") + " |"
' "$report"
echo
echo "</details>"
