# Prompt Pack — PR Review ⭐ Recommended

Use this pack when you want Copilot to help you review a code change before it merges.

---

## Prompt 1 — Generic

> **Copy this into Copilot Chat with a file open as context.**

```
Review this code for correctness, readability, and potential security issues.
```

Run it. Note what Copilot focuses on.

---

## Prompt 2 — Structured

> **Same file, same chat — replace with this version.**

```
Review this C# code as a senior .NET developer. Check for:
- Missing null guards or unhandled edge cases
- Violation of Single Responsibility Principle
- Unhandled exceptions that should be caught or documented
- Missing or insufficient test coverage for the changed logic
- Any security concerns (input validation, authorization, data exposure)

Summarize your findings as: Critical / Should Fix / Consider.
```

Compare the output to Prompt 1. What changed?

---

## Prompt 3 — Adapted (your turn)

> **Add 2–3 constraints from the list below that fit this codebase, then run it.**

```
Review this C# code as a senior .NET developer. Check for:
- Missing null guards or unhandled edge cases
- Violation of Single Responsibility Principle
- Unhandled exceptions that should be caught or documented
- Missing or insufficient test coverage for the changed logic
- Any security concerns (input validation, authorization, data exposure)
[ADD YOUR CONSTRAINTS HERE]

Summarize your findings as: Critical / Should Fix / Consider.
```

---

## Constraints to Try

Pick the ones that apply to your real codebase or to this repo:

- `This codebase uses MediatR — commands should have a corresponding handler and validator. Flag any missing validators.`
- `Controllers must not contain business logic — all logic belongs in command/query handlers.`
- `This project uses FluentValidation, not DataAnnotations. Flag any DataAnnotations-based validation.`
- `Any new endpoint must have an explicit [Authorize] or [AllowAnonymous] attribute — flag if missing.`
- `This project follows CQRS — reads use Query/QueryHandler pairs, writes use Command/CommandHandler pairs. Flag deviations.`
- `Check that any new EF Core queries avoid N+1 problems (look for missing Include() or AsNoTracking() where appropriate).`

---

## Suggested File to Use

Open `src/Web/Controllers/ReportController.cs` — it's a short, reviewable file with real findings CodeQL already flagged.
