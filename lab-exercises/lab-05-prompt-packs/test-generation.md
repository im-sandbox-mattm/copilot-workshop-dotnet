# Prompt Pack — Test Generation

Use this pack when you need to write unit tests for a handler, service, or controller method.

---

## Prompt 1 — Generic

> **Open a handler or service file, then run this in Copilot Chat.**

```
Write unit tests for this method.
```

Run it. Note what framework and structure Copilot assumes.

---

## Prompt 2 — Structured

> **Same file — replace with this version.**

```
Write xUnit unit tests for this C# method. Include:
- Happy path (valid inputs, expected output)
- Null or missing inputs
- Boundary conditions or edge cases
- Any exception paths

Use Arrange-Act-Assert structure. Name each test:
MethodName_Scenario_ExpectedBehavior
```

Compare the output to Prompt 1.

---

## Prompt 3 — Adapted (your turn)

> **Add 2–3 constraints from the list below, then run it.**

```
Write xUnit unit tests for this C# method. Include:
- Happy path (valid inputs, expected output)
- Null or missing inputs
- Boundary conditions or edge cases
- Any exception paths

Use Arrange-Act-Assert structure. Name each test:
MethodName_Scenario_ExpectedBehavior
[ADD YOUR CONSTRAINTS HERE]
```

---

## Constraints to Try

- `Use NSubstitute for all mocking — Substitute.For<T>(). Do not use Moq.`
- `The test class should be named [ClassName]Tests and live in the same folder structure as the class under test.`
- `Handlers take a request object and a CancellationToken. Mock all constructor dependencies via NSubstitute.`
- `Do not test EF Core directly — mock IRepository<T> or IAsyncRepository<T> at the boundary.`
- `FluentValidation validators should be tested separately using TestValidate(), not inline in handler tests.`
- `Each test should be independent — no shared mutable state between tests.`

---

## Suggested File to Use

Open `src/ApplicationCore/Services/OrderService.cs` or any handler under `src/Web/Features/` — both have clear dependencies you can mock.
