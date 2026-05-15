# Gas South — Copilot Instructions

## Stack Context
- Backend: ASP.NET Core Web API (.NET 8), Clean Architecture pattern
- Frontend: Angular 17
- Database: SQLite (dev), SQL Server (prod), accessed via EF Core 8
- Pattern: CQRS with MediatR — commands return primitive IDs, queries return DTOs
- Validation: FluentValidation only — never validate in command handlers
- Mapping: AutoMapper with ProjectTo<> for query projections
- Tests: NUnit + FluentAssertions + Moq — one test class per handler, Arrange/Act/Assert structure

## Naming Conventions
- Commands: `VerbNounCommand` / `VerbNounCommandHandler` in `Commands/VerbNoun/` folder
- Queries: `GetNounQuery` / `GetNounQueryHandler` in `Queries/GetNoun/` folder
- Validators: `VerbNounCommandValidator` in the same folder as the command
- DTOs: `NounBriefDto` (list views) / `NounDetailDto` (single item views)

## Code Generation Rules
- All new handlers must implement IRequestHandler<TRequest, TResponse>
- Always inject IApplicationDbContext, never DbContext directly
- Async methods must use CancellationToken
- Use Ardalis.GuardClauses for null/range guard checks
- Return int (entity ID) from create commands

## Test Patterns
- Test file name: `VerbNounCommandTests.cs` in the matching test project folder
- Cover: happy path, null input, invalid foreign key reference
- Use `new CreateTodoItemCommand { ... }` style initialization
