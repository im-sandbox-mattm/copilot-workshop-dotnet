---
# The frontmatter block controls when this file is injected into Copilot's context.
# applyTo is a glob pattern matched against the file(s) currently open or being edited.
# When a file under src/PublicApi/ is in scope, Copilot automatically receives
# these instructions — no manual attachment needed.
applyTo: "src/PublicApi/**"
---

# Public API Endpoint Conventions

<!--
  WHY THIS FILE EXISTS
  Without this guidance Copilot defaults to generating either an ASP.NET
  controller class or a plain app.MapGet/MapPost lambda — neither of which
  matches the architectural pattern this project uses. These instructions
  tell Copilot about the project-specific abstraction so generated code fits
  in immediately without manual refactoring.
-->

All endpoints use the `MinimalApi.Endpoint` library — never use controllers or plain `app.Map*` lambdas directly.

## Structure

<!--
  Copilot also learns file/folder naming conventions from instructions, not
  just code patterns. Stating the three-file split here means Copilot will
  create the right files with the right names in one shot.
-->

Each endpoint consists of three files in a folder named after the resource (e.g. `CatalogItemEndpoints/`):

- `{Action}{Resource}Endpoint.cs` — the endpoint class
- `{Action}{Resource}Endpoint.{Action}{Resource}Request.cs` — the request record/class
- `{Action}{Resource}Endpoint.{Action}{Resource}Response.cs` — the response class

## Endpoint class

<!--
  Providing a concrete code template is the most reliable way to convey a
  structural pattern. Prose alone is often interpreted loosely; a template
  with the exact interface signature, attribute placement, and method
  signatures leaves no ambiguity.
-->

```csharp
public class CreateWidgetEndpoint : IEndpoint<IResult, CreateWidgetRequest, IRepository<Widget>>
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("api/widgets",
            [Authorize(Roles = Constants.Roles.ADMINISTRATORS,
                       AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            async (CreateWidgetRequest request, IRepository<Widget> repo) =>
                await HandleAsync(request, repo))
            .Produces<CreateWidgetResponse>()
            .WithTags("WidgetEndpoints");
    }

    public async Task<IResult> HandleAsync(CreateWidgetRequest request, IRepository<Widget> repo)
    {
        // implementation
    }
}
```

## Key rules

<!--
  Short, imperative rules ("never X", "always Y") are more reliably followed
  than explanatory prose. Each bullet targets a specific default behaviour
  Copilot would otherwise fall back to.
-->

- Always implement `IEndpoint<IResult, TRequest, TRepository>` — never raw minimal API handlers
- Request classes inherit from `BaseRequest` (provides `CorrelationId()`)
- Response classes inherit from `BaseResponse`
- Route format: `api/[kebab-case-plural-resource]`
- Protected endpoints require `[Authorize]` with `JwtBearerDefaults.AuthenticationScheme`
- Use `IRepository<T>` from `Ardalis.Specification` — never inject `DbContext` directly
