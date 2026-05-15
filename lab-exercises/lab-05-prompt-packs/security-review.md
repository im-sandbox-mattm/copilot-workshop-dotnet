# Prompt Pack — Security Review

Use this pack when you want to find vulnerabilities in a piece of code before it ships — or to understand what CodeQL has already flagged.

---

## Prompt 1 — Generic

> **Open any controller or service file, then run this in Copilot Chat.**

```
Review this code for security vulnerabilities.
```

Run it. What does Copilot flag?

---

## Prompt 2 — Structured

> **Same file — replace with this version.**

```
Review this C# code for OWASP Top 10 vulnerabilities. Focus on:
- Injection (SQL, command, path traversal)
- Broken access control (missing authorization checks)
- Security misconfiguration (overly permissive endpoints, exposed stack traces)
- Insecure input handling (user-controlled data used without validation or encoding)
- Sensitive data exposure (credentials, PII logged or returned in responses)

For each finding: state the vulnerability type, the affected line or pattern, and a one-line fix recommendation.
```

Compare to Prompt 1 — is the output more actionable?

---

## Prompt 3 — Adapted (your turn)

> **Add 2–3 constraints from the list below, then run it.**

```
Review this C# code for OWASP Top 10 vulnerabilities. Focus on:
- Injection (SQL, command, path traversal)
- Broken access control (missing authorization checks)
- Security misconfiguration (overly permissive endpoints, exposed stack traces)
- Insecure input handling (user-controlled data used without validation or encoding)
- Sensitive data exposure (credentials, PII logged or returned in responses)

For each finding: state the vulnerability type, the affected line or pattern, and a one-line fix recommendation.
[ADD YOUR CONSTRAINTS HERE]
```

---

## Constraints to Try

- `All controller endpoints must have an explicit [Authorize] or [AllowAnonymous] attribute — flag any that are missing.`
- `This project uses EF Core — raw SQL string concatenation is never acceptable. Flag any SqlCommand or string.Format usage in queries.`
- `File path operations must never use user-supplied input directly. Flag any System.IO calls that receive request parameters.`
- `ILogger calls must use structured logging placeholders (e.g. _logger.LogInformation("User {UserId}", id)) — flag string interpolation in log calls.`
- `Endpoints that accept file uploads must validate MIME type and file size — flag any that don't.`
- `No secrets, passwords, or tokens should appear in log output — flag any log statements that include sensitive fields.`

---

## Suggested File to Use

Open `src/Web/Controllers/ReportController.cs` or `src/Web/Controllers/FileController.cs` — these contain real CodeQL findings you can compare against Copilot's output.
