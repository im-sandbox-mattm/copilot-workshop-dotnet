# Gas South — Copilot Instructions

## Stack Context
<!-- Pre-filled — this is context Copilot can't infer from the code alone -->
- Backend: ASP.NET Core (.NET 8), Clean Architecture — ApplicationCore / Infrastructure / Web / PublicApi
- ORM: EF Core 8 — SQLite in development (`UseOnlyInMemoryDatabase: true` in appsettings.json)
- Pattern: Repository + Service (Ardalis.Specification)
- Validation: Ardalis.GuardClauses (inline in service methods)
- Mapping: AutoMapper
- Tests: xUnit + NSubstitute

## Naming Conventions
<!-- Discover these by browsing src/ApplicationCore/. Look at existing commands, queries, and DTOs.
     Hint: open src/ApplicationCore/Services/ and src/ApplicationCore/Entities/ and observe the patterns. -->

- Commands: <!-- e.g. VerbNounCommand -->
- Queries: <!-- e.g. GetNounQuery -->
- Validators: <!-- where do they live relative to the command? -->
- DTOs: <!-- what suffix pattern do you see? -->

## Code Generation Rules
<!-- Discover these by reading an existing handler end-to-end.
     Suggested file: src/ApplicationCore/Services/BasketService.cs or any handler in Web/
     Ask yourself: what interface is injected? what does the return type look like? what guard library is used? -->

- <!-- Handler interface pattern -->
- <!-- What to inject (hint: look at constructor params in existing services) -->
- <!-- Async signature requirement -->
- <!-- Guard clause library in use (check Directory.Packages.props) -->

## Test Patterns
<!-- Discover these by opening tests/UnitTests/ and reading one existing test class.
     Note the file naming, test method naming format, and AAA structure. -->

- Test file naming: <!-- -->
- Test method naming format: <!-- -->
- Mocking library: <!-- -->
- Assertion library: <!-- -->
- What cases to cover: <!-- -->
