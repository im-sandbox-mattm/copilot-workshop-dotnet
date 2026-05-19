# GitHub Copilot Instructions — eShopOnWeb

<!--
  BEST PRACTICES: copilot-instructions.md
  ════════════════════════════════════════
  • This file is automatically injected into every Copilot chat and agent
    session for this repository — no @file attachment needed.
  • Keep rules short and imperative. Copilot follows directives more
    reliably than explanatory prose.
  • Scoped rules (e.g. test conventions, API patterns) belong in
    .github/instructions/*.instructions.md with an applyTo glob — not here.
    Reserve this file for project-wide rules that apply everywhere.
  • Demonstrable contrast: each rule below was chosen because removing it
    causes Copilot to produce visibly wrong output, making the impact easy
    to show in a workshop demo.
-->

## Language & runtime

<!-- Without this, Copilot sometimes suggests Python or Java snippets when
     explaining concepts. This pins all code output to the project's stack. -->
- Always use C# and .NET. Never suggest alternative languages in code examples.
- Target .NET 8. Do not use APIs removed or deprecated before .NET 8.

## Code style

<!-- File-scoped namespaces are a strong visual signal — the diff between
     "namespace Foo { ... }" and "namespace Foo;" is immediately obvious
     in a demo and enforces the C# 10+ style used throughout this project. -->
- Use file-scoped namespace declarations (`namespace Foo.Bar;`), never block-scoped.

<!-- "var" vs explicit types is one of the easiest rules to demo: ask Copilot
     to write a method and watch whether it writes "CatalogItem item = ..." or
     "var item = ...". This project uses var consistently for local variables. -->
- Use `var` for local variable declarations when the type is evident from context.

<!-- Primary constructors (C# 12) reduce boilerplate significantly. Without this
     rule Copilot defaults to the older field + constructor pattern. -->
- Prefer C# 12 primary constructors for classes that only assign constructor
  parameters to fields (e.g. `public class Foo(IBar bar) { ... }`).

## Null handling & guard clauses

<!-- Without this rule Copilot writes manual null checks:
       if (x == null) throw new ArgumentNullException(nameof(x));
     With it, Copilot uses the Ardalis.GuardClauses library already in the project:
       Guard.Against.Null(x);
     The difference is immediately visible and teaches students about the library. -->
- Use `Guard.Against.*` (Ardalis.GuardClauses) for argument validation.
  Never write manual `if (x == null) throw` checks.

## Async

<!-- .Result and .Wait() cause deadlocks in ASP.NET Core. This rule prevents
     Copilot from taking the easy synchronous shortcut. -->
- All I/O operations must be async. Never use `.Result`, `.Wait()`, or blocking
  wrappers around async calls.
- Propagate `CancellationToken` through async call chains where the framework provides one.

## Architecture

<!-- The biggest "wrong default" in this project: Copilot will happily inject
     CatalogContext (DbContext) directly if not told otherwise, bypassing the
     repository/specification pattern the whole codebase is built around. -->
- Never inject `DbContext` (or `CatalogContext`) directly into application or API
  classes. Always use `IRepository<T>` from Ardalis.Specification.
- Domain logic belongs in `ApplicationCore`. Infrastructure concerns (EF Core,
  email, blob storage) belong in `Infrastructure`. Never mix them.

## General

<!-- Incomplete code is a common Copilot failure mode. This prevents it from
     leaving placeholder stubs and forcing the developer to finish the work. -->
- Never leave `TODO` comments or `throw new NotImplementedException()` in generated
  code. Always provide a complete implementation.
