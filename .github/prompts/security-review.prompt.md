---
mode: ask
---

Perform a security review of the code in the referenced file.

Analyze for the following OWASP Top 10 categories and report findings:

**A01 — Broken Access Control**
- Are authorization checks in place where needed?
- Can users access resources they shouldn't?

**A02 — Cryptographic Failures**
- Is sensitive data exposed in logs, responses, or storage?
- Are secrets hardcoded?

**A03 — Injection**
- SQL injection: Is user input ever concatenated into queries?
- Log injection (CWE-117): Is user input passed to ILogger via string interpolation?
- Path traversal (CWE-22): Is user input used in file paths without sanitization?

**A05 — Security Misconfiguration**
- Are error messages exposing stack traces or internal details?
- Are default credentials or endpoints left open?

**A07 — Identification and Authentication Failures**
- Are authentication checks bypassed or missing?

For each finding, provide:
- **Severity**: Critical / High / Medium / Low
- **CWE ID** if applicable
- **Vulnerable code snippet**
- **Recommended fix** with corrected code example

End with a prioritized remediation list — fix Critical and High before merging.
