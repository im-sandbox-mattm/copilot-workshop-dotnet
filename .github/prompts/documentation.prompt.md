---
mode: ask
---

Generate XML documentation comments for all public members in the referenced file.

Follow these conventions:
- Use `<summary>` for every public class, method, property, and interface member
- Use `<param name="...">` for every parameter
- Use `<returns>` for non-void methods
- Use `<exception cref="...">` for documented exceptions
- Use `<remarks>` for non-obvious behavior, threading concerns, or important caveats
- Keep summaries to 1–2 sentences — be precise, not verbose

Additionally, generate a markdown documentation block for this class suitable for a wiki or README:

## [ClassName]

**Namespace:** `[full namespace]`  
**File:** `[relative file path]`

### Purpose
[1–2 sentence description of what this class does and why it exists]

### Public API
| Member | Description |
|--------|-------------|
| [method/property name] | [brief description] |

### Usage Example
```csharp
// Minimal working example
```

### Notes
- [Any important caveats, threading behavior, or dependencies]
