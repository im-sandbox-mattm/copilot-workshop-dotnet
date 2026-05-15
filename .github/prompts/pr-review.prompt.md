---
mode: ask
---

Review this pull request and provide structured feedback.

For each area below, give a rating (✅ Good / ⚠️ Needs attention / ❌ Issue found) and specific comments:

**1. Correctness**
- Does the code do what the PR description claims?
- Are there edge cases not handled?

**2. Security**
- Any OWASP Top 10 concerns? (injection, broken access control, insecure data exposure)
- Are user inputs validated and sanitized?
- Is logging using structured format (not string interpolation)?

**3. Architecture**
- Does it follow the Clean Architecture layer rules for this codebase?
- Is business logic in ApplicationCore, not in Web or Infrastructure?
- Are new interfaces defined before implementations?

**4. Tests**
- Are there tests for the new/changed code?
- Do tests follow Arrange/Act/Assert with `MethodName_StateUnderTest_ExpectedBehavior` naming?
- Are edge cases and failure paths covered?

**5. Code Quality**
- Is the code readable and consistent with the existing style?
- Are there any obvious performance concerns?
- Are there magic numbers or strings that should be constants?

Summarize with: top 1 thing to fix before merging, and 1 positive observation.
