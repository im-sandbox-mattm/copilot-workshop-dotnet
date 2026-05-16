# Copilot Workshop — Demo Codebase

This repository is used as the demo codebase for GitHub Copilot workshops targeting software development teams. It provides a realistic, running ASP.NET Core application with enough structural complexity to make Copilot features — inline completions, chat, GHAS integration, and agent workflows — immediately meaningful in a workshop setting.

---

## Getting Started

No database setup required. The app is pre-configured to use an in-memory database for development via the `UseOnlyInMemoryDatabase` flag in `appsettings.json` (already set to `true`).

```bash
cd src/Web
dotnet run
```

The web app starts at `https://localhost:44315`.  
The API is at `https://localhost:5099/api`.

Default credentials: `admin@microsoft.com` / `Pass@word1`

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- A GitHub Copilot license assigned in your GitHub org

---

## Purpose

This repo is used as the hands-on codebase for multi-day GitHub Copilot workshops. It provides:

- A realistic layered architecture (Clean Architecture, CQRS, MediatR) that responds well to Copilot's code generation and explanation features
- Pre-planted security findings in the `Controllers` layer for GHAS demo and lab exercises (SQL injection, XSS, path traversal, log injection)
- Lab exercise files in `lab-exercises/` with structured prompting tasks
- A `CONTRIBUTORS.md` for the attendee branching and PR workflow lab

The codebase is intentionally delivered **without** a `.github/copilot-instructions.md` — attendees build it themselves as part of a before/after lab.

---

## Stack

- ASP.NET Core 8 (Razor Pages + Blazor WebAssembly admin panel)
- EF Core 8 — in-memory for dev (no setup required), SQL Server for production
- Clean Architecture — Domain / ApplicationCore / Infrastructure / Web
- MediatR for CQRS-style request handling
- ASP.NET Core Identity with cookie authentication

---

## Attribution

This repository is a fork of [dotnet-architecture/eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb), a Microsoft reference application demonstrating ASP.NET Core architectural patterns.

Original project licensed under the [MIT License](LICENSE) — Copyright (c) .NET Foundation and Contributors.

Workshop additions (lab exercises, planted controllers, `CONTRIBUTORS.md`) are layered on top of that base and are covered under the same MIT License.
