  {
  // ─────────────────────────────────────────────────────────────────────────
  // WHAT IS THIS FILE?
  //   A VS Code Copilot hook configuration. Files matching .github/hooks/*.json
  //   are auto-discovered and loaded by VS Code — no registration needed.
  //
  // HOW HOOKS WORK:
  //   At key lifecycle points (see event names below), VS Code runs the listed
  //   shell commands. Each command receives a JSON object via stdin describing
  //   what the agent is about to do, and can return JSON via stdout to allow,
  //   block, or modify that action.
  //
  // THIS FILE'S PURPOSE:
  //   Intercept every file edit BEFORE it reaches the codebase. If the script
  //   detects a dangerous pattern (hardcoded secrets, removed auth, etc.) it
  //   returns permissionDecision:'deny' and the reason is injected back into
  //   the model so it can self-correct — the file is never written.
  // ─────────────────────────────────────────────────────────────────────────

  // Required top-level wrapper. All hook event handlers live inside "hooks".
  "hooks": {

    // ── LIFECYCLE EVENT ────────────────────────────────────────────────────
    // "PreToolUse" fires BEFORE the agent invokes any tool.
    // Other available events:
    //   PostToolUse    — after a tool completes (run formatters, tests, etc.)
    //   UserPromptSubmit — when the user submits a prompt
    //   SessionStart   — when a new agent session begins
    //   Stop           — when the session ends
    //   SubagentStart / SubagentStop — when a subagent is spawned/completes
    "PreToolUse": [

      // Each entry in the array is one command to run.
      // Multiple entries run in order; the most restrictive result wins.
      {
        // Required. "command" is currently the only valid type.
        "type": "command",

        // Default command (used when no OS-specific override matches).
        "command": "python3 .github/hooks/scripts/block-dangerous-edits.py",

        // OS-specific overrides — VS Code picks the right one automatically.
        "osx": "python3 .github/hooks/scripts/block-dangerous-edits.py",
        "linux": "python3 .github/hooks/scripts/block-dangerous-edits.py",
        "windows": "python .github\\hooks\\scripts\\block-dangerous-edits.py",

        // Seconds before VS Code kills the process. Default is 30.
        "timeout": 15

        // Other available properties (not used here):
        //   "cwd"  — working directory relative to the repo root
        //   "env"  — extra environment variables, e.g. { "MY_VAR": "value" }
      }

      // ── HOW THE SCRIPT CONTROLS THE AGENT ─────────────────────────────
      // The script reads the incoming JSON from stdin, checks the content,
      // then writes one of the following to stdout:
      //
      //  ALLOW  → { "hookSpecificOutput": { "permissionDecision": "allow" } }
      //  BLOCK  → { "hookSpecificOutput": { "permissionDecision": "deny",
      //               "permissionDecisionReason": "..." } }
      //  ASK    → { "hookSpecificOutput": { "permissionDecision": "ask" } }
      //
      // Exit codes also matter:
      //   0  — success; parse stdout as JSON
      //   2  — hard block; stderr is shown to the model as context
      //   other — non-blocking warning shown to the user; processing continues
    ]
  }
}
