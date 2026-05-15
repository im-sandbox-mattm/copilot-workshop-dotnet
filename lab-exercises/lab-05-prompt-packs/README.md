# Lab 05 — Prompt Packs

## What is a Prompt Pack?

A prompt pack is a curated set of prompts for a specific developer workflow. Each pack gives you a generic starting point, a more structured version, and a blank template you adapt to this codebase.

## The Lab

1. **Pick one pack** — PR Review is recommended if you're unsure where to start
2. **Run the generic prompt** in Copilot Chat on any file in this repo
3. **Run the structured prompt** — notice how the output changes
4. **Fill in the adapted template** by adding constraints from the "Constraints to Try" section
5. **Compare outputs** — what did adding constraints change?

## The Four Packs

| File | Best for |
|---|---|
| [`pr-review.md`](pr-review.md) ⭐ | Reviewing code changes before merge |
| [`test-generation.md`](test-generation.md) | Writing unit tests for a handler or service |
| [`code-explanation.md`](code-explanation.md) | Understanding unfamiliar code |
| [`security-review.md`](security-review.md) | Finding vulnerabilities before they ship |

## Tips

- Open a file from `src/` as context before running a prompt (use `#file` or open it in the editor)
- The adapted prompt is what you keep — it's the one you'd use on your real codebase
- If a constraint doesn't apply, skip it — the goal is relevance, not completeness
