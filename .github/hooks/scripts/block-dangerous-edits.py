#!/usr/bin/env python3
"""
block-dangerous-edits.py
========================
PreToolUse hook for the eShopOnWeb workshop.

How it works
------------
VS Code pipes a JSON object to stdin before every tool call.
This script reads that object, checks whether the proposed file content
contains dangerous patterns, and writes a JSON response to stdout.

Return paths
------------
  permissionDecision: "allow"  → edit proceeds normally
  permissionDecision: "deny"   → edit is BLOCKED; the reason is injected
                                  back into the model so it can self-correct
  exit code 2                  → hard block (stderr shown to the model)

Supported tools
---------------
  replace_string_in_file  — inspects newString
  create_file             — inspects content
  (all other tools are silently allowed)

To test manually
----------------
  echo '{"tool_name":"create_file","tool_input":{"filePath":"src/Foo.cs","content":"string conn = \\"Server=prod;Password=hunter2\\";"}}' \
    | python3 .github/hooks/scripts/block-dangerous-edits.py
"""

import json
import re
import sys

# ---------------------------------------------------------------------------
# Patterns to block — each entry is (regex, human-readable reason)
# Tailored to the eShopOnWeb ASP.NET Core / EF Core codebase.
# ---------------------------------------------------------------------------
BLOCKED_PATTERNS = [
    # ── Hardcoded secrets ────────────────────────────────────────────────────
    (
        r'(?i)(password|pwd)\s*=\s*"[^"]{3,}"',
        "Hardcoded password literal in source code. "
        "Use configuration (IConfiguration / environment variables) instead, "
        "as already done in src/Infrastructure/Dependencies.cs.",
    ),
    (
        r'(?i)Server\s*=\s*[^;]+;\s*(User\s*Id|Password)\s*=',
        "Hardcoded connection string detected. "
        "Read connection strings from IConfiguration.GetConnectionString() — "
        "see existing pattern in src/Web/Program.cs.",
    ),
    (
        r'(?i)(apikey|api_key|secret|token)\s*=\s*"[A-Za-z0-9+/=_\-]{8,}"',
        "Hardcoded API key / secret / token in source code. "
        "Move secrets to environment variables or Azure Key Vault.",
    ),

    # ── Removing authorization ───────────────────────────────────────────────
    (
        r'\[AllowAnonymous\]',
        "[AllowAnonymous] attribute detected. "
        "All API endpoints in PublicApi/ require [Authorize] with the ADMINISTRATORS "
        "role (see CreateCatalogItemEndpoint, DeleteCatalogItemEndpoint). "
        "Do not bypass authorization without an explicit architectural decision.",
    ),

    # ── Destructive database operations ─────────────────────────────────────
    (
        r'(?i)\bDROP\s+(TABLE|DATABASE|SCHEMA)\b',
        "Destructive DDL statement (DROP TABLE/DATABASE/SCHEMA) detected. "
        "EF Core migrations should use MigrationBuilder.DropTable only when "
        "intentional and reviewed. Use a rename or soft-delete instead.",
    ),
    (
        r'(?i)\.(ExecuteSqlRaw|FromSqlRaw|ExecuteSqlRawAsync|FromSqlRawAsync)\s*\(',
        "Raw SQL via ExecuteSqlRaw/FromSqlRaw is not permitted in this codebase. "
        "Use the Specification pattern already established via EfRepository<T> and ISpecification<T>. "
        "See src/Infrastructure/Data/EfRepository.cs and src/ApplicationCore/Specifications/ for examples.",
    ),

    # ── Debug / dev-only code leaking to production ──────────────────────────
    (
        r'(?i)Console\.Write(Line)?\s*\(.*(?:password|token|secret|connectionstring)',
        "Console output of sensitive data detected. "
        "Remove debug logging of passwords, tokens, or connection strings.",
    ),
    (
        r'app\.UseDeveloperExceptionPage\(\)',
        "UseDeveloperExceptionPage() must only run in the Development environment. "
        "Wrap it in: if (app.Environment.IsDevelopment()) { ... }",
    ),
]


def allow():
    print(json.dumps({
        "hookSpecificOutput": {
            "hookEventName": "PreToolUse",
            "permissionDecision": "allow",
        }
    }))
    sys.exit(0)


def deny(reason: str, file_path: str):
    print(json.dumps({
        "hookSpecificOutput": {
            "hookEventName": "PreToolUse",
            "permissionDecision": "deny",
            "permissionDecisionReason": (
                f"Security policy blocked the edit to '{file_path}':\n\n"
                f"{reason}\n\n"
                "Please revise the code to comply with the project's security policy "
                "and try again."
            ),
            "additionalContext": (
                "The edit was blocked by the PreToolUse security-guard hook "
                "(.github/hooks/security-guard.json). Fix the flagged issue "
                "and the hook will allow the change automatically."
            ),
        }
    }))
    sys.exit(0)


def main():
    try:
        payload = json.load(sys.stdin)
    except json.JSONDecodeError as exc:
        # Unreadable input — let the operation through rather than false-blocking.
        print(f"security-guard: could not parse stdin: {exc}", file=sys.stderr)
        allow()

    tool_name = payload.get("tool_name", "")
    tool_input = payload.get("tool_input", {})

    # ── Determine which content to inspect ───────────────────────────────────
    content = ""
    file_path = tool_input.get("filePath", tool_input.get("file_path", "<unknown>"))

    if tool_name == "replace_string_in_file":
        content = tool_input.get("newString", "")
    elif tool_name == "create_file":
        content = tool_input.get("content", "")
    else:
        # Not a file-editing tool (e.g. run_in_terminal, read_file) — allow.
        allow()

    # ── Only inspect C# / config / script files ───────────────────────────────
    watched_extensions = (".cs", ".json", ".xml", ".yaml", ".yml", ".sh", ".ps1", ".env", ".config")
    if not any(file_path.lower().endswith(ext) for ext in watched_extensions):
        allow()

    # ── Check each blocked pattern ────────────────────────────────────────────
    for pattern, reason in BLOCKED_PATTERNS:
        if re.search(pattern, content):
            deny(reason, file_path)

    allow()


if __name__ == "__main__":
    main()
