---
mode: ask
---

Generate a comprehensive xUnit test suite for the referenced file.

Follow these conventions for this codebase:
- Test framework: xUnit
- Mocking: Moq
- Method naming: `MethodName_StateUnderTest_ExpectedBehavior`
- Structure: Arrange / Act / Assert with blank line separators between sections
- One assertion concept per test — don't combine unrelated assertions

Cover the following cases:
1. **Happy path** — valid input produces expected output
2. **Null / empty inputs** — each required parameter independently
3. **Boundary values** — min/max valid values, values just outside the boundary
4. **Invalid input** — inputs that should trigger validation failures or exceptions
5. **Edge cases** — any business logic branches identified in the code

For each test:
- Use descriptive names that read as sentences
- Mock external dependencies (repositories, DbContext, external services)
- Don't test implementation details — test observable behavior

After generating tests, confirm: what is the estimated line coverage these tests provide?
