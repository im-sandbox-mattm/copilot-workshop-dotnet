# Prompt Pack — Code Explanation

Use this pack when you need to understand an unfamiliar file quickly — what it does, what pattern it uses, and what could go wrong.

---

## Prompt 1 — Generic

> **Open any unfamiliar file, then run this in Copilot Chat.**

```
Explain what this code does.
```

Run it. Is the explanation useful? At what level of detail?

---

## Prompt 2 — Structured

> **Same file — replace with this version.**

```
Explain this C# class:
1. What it does (one sentence)
2. What design pattern or architectural role it plays
3. What its key dependencies are and what each provides
4. What could go wrong at runtime (nulls, exceptions, race conditions)
5. Any code smells or areas that would need attention before shipping
```

Compare the output to Prompt 1.

---

## Prompt 3 — Adapted (your turn)

> **Add 2–3 constraints from the list below, then run it.**

```
Explain this C# class:
1. What it does (one sentence)
2. What design pattern or architectural role it plays
3. What its key dependencies are and what each provides
4. What could go wrong at runtime (nulls, exceptions, race conditions)
5. Any code smells or areas that would need attention before shipping
[ADD YOUR CONSTRAINTS HERE]
```

---

## Constraints to Try

- `This project uses Clean Architecture — identify which layer this class belongs to (Domain, Application, Infrastructure, or Presentation) and whether it's in the right layer.`
- `This project uses MediatR — if this is a handler, explain what pipeline behaviors run before it (see Infrastructure/MediatorExtensions.cs).`
- `Explain any EF Core patterns used: AsNoTracking, Include chains, lazy vs. eager loading.`
- `Flag any places where this class violates the Dependency Inversion Principle.`
- `Summarize at a level a junior .NET developer could understand — avoid jargon where possible.`
- `If this class has no unit tests, identify which behavior is most important to test first.`

---

## Suggested File to Use

Open `src/Infrastructure/Data/EfRepository.cs` or `src/ApplicationCore/Services/BasketService.cs` — both are central to the app and worth understanding deeply.
