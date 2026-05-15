# Copilot Instructions — Gas South eShopOnWeb Workshop Repo

## Project Overview
This is an ASP.NET Core 8 e-commerce reference application used as the workshop base.
Architecture: Clean Architecture / Onion — ApplicationCore (domain + interfaces), Infrastructure (EF Core + Identity), Web (Razor Pages + Blazor WASM admin), PublicApi (minimal API).

## Key Conventions

### Namespaces
- Domain entities and interfaces: `Microsoft.eShopWeb.ApplicationCore.*`
- Infrastructure (EF Core, Identity): `Microsoft.eShopWeb.Infrastructure.*`
- Web (Razor Pages, MVC controllers): `Microsoft.eShopWeb.Web.*`
- Public API (minimal endpoints): `Microsoft.eShopWeb.PublicApi.*`

### Architecture Rules
- Business logic belongs in `ApplicationCore` — never in `Web` or `Infrastructure`
- New domain entities go in `src/ApplicationCore/Entities/`
- New interfaces go in `src/ApplicationCore/Interfaces/`
- Infrastructure implementations go in `src/Infrastructure/`
- Web pages use Razor Pages (`src/Web/Pages/`) — prefer Pages over MVC controllers for new UI
- New API endpoints go in `src/PublicApi/` following the existing minimal endpoint pattern

### Testing
- Test projects are in `/tests/` — Unit, Integration, Functional, PublicApiIntegration
- Use xUnit for all tests
- Use Moq for mocking
- Test method naming: `MethodName_StateUnderTest_ExpectedBehavior`
- Arrange/Act/Assert structure with blank line separators

### Logging
- Always use structured logging — never string interpolation in log calls
  - CORRECT: `_logger.LogInformation("Processing order {OrderId}", orderId);`
  - INCORRECT: `_logger.LogInformation($"Processing order {orderId}");`
- String interpolation in `ILogger` calls bypasses structured logging and can enable log injection (CWE-117)

### Security
- Never trust user-supplied file paths — always use `Path.GetFileName()` to strip directory components
- Validate that resolved file paths stay within intended directories
- Use parameterized queries — never concatenate user input into SQL
- Return 400 Bad Request for invalid input rather than exposing error details

### Dependency Injection
- Register services in `src/Web/Program.cs` or the appropriate `DependencyInjection` extension class
- Prefer constructor injection
- Use `ILogger<T>` for logging — inject via constructor

## Database
- Development uses in-memory database (`UseOnlyInMemoryDatabase: true` in `appsettings.json`)
- No SQL Server required for development or testing
- EF Core DbContext: `CatalogContext` (catalog data) and `AppIdentityDbContext` (identity)

## Running the App
```bash
dotnet run --project src/Web/Web.csproj
```
App runs at https://localhost:5001 (HTTPS) and http://localhost:5000 (HTTP).
Default admin: admin@microsoft.com / Pass@word1
Default user: demouser@microsoft.com / Pass@word1
