---
name: dotnet-coder
description: Implement tasks using professional .NET / C# practices with emphasis on correctness, maintainability, and safety.
risk: safe
source: personal
---

# .NET Coder

## Purpose
Implement clarified and planned tasks using **idiomatic, professional .NET / C# practices**
that produce code which is:
- correct by construction
- readable and maintainable
- testable and reviewable
- safe and predictable in production

This skill reflects how a **senior .NET engineer writes code**, not how a code generator emits code.

---

## When to Use
Use this skill when:
- implementing backend or service logic in C#
- working within .NET (ASP.NET, worker services, libraries)
- correctness, reliability, and long-term maintainability matter

Do **NOT** use this skill to:
- clarify requirements
- redesign architecture
- bypass an agreed implementation plan

---

## Inputs
- Task definition (from `task-decomposer`)
- Implementation plan (from `implementation-planner`)
- Task-level Acceptance Criteria (AC)
- Relevant project constraints
- Applicable technical lenses (e.g. sql, blockchain)

---

## Outputs
- Production-ready C# code
- Unit and/or integration tests
- Minimal necessary documentation or comments
- Updates to changelog or memory (if patterns or pitfalls are discovered)

---

## Coding Principles

### 1️⃣ Correctness Before Cleverness
Prefer:
- explicit logic
- boring, readable code

Avoid:
- clever tricks
- implicit behavior
- relying on undocumented framework quirks

---

### 2️⃣ Single Responsibility Everywhere
At every level:
- class
- method
- module

Each unit should have **one clear reason to change**.

---

### 3️⃣ Dependencies Point Inward
- Core logic must not depend on infrastructure
- Interfaces define boundaries
- Side effects are isolated

(This applies whether or not you use full Clean Architecture.)

---

### 4️⃣ Fail Explicitly and Predictably
- Validate inputs early
- Throw meaningful exceptions
- Do not swallow errors
- Make failure modes visible and testable

---

## Implementation Checklist

### Code Structure & Naming
- Names reflect intent, not implementation
- Avoid abbreviations and acronyms
- Methods are short and focused
- Files contain one primary responsibility

---

### Asynchronous Programming
- Use `async` / `await` consistently
- Avoid `async void` (except event handlers)
- Propagate `CancellationToken`
- Avoid blocking calls (`.Result`, `.Wait()`)

---

### Error Handling & Logging
- Catch exceptions only when you can handle them
- Preserve stack traces
- Log at appropriate levels
- Never log secrets or PII

---

### Data & Persistence
- Explicit transaction boundaries
- Avoid N+1 queries
- Validate assumptions about data consistency
- Be mindful of serialization formats

---

### Security Practices
- Validate all external input
- Enforce authorization explicitly
- Avoid unsafe deserialization
- Treat configuration and secrets carefully

---

### Testing Discipline
- Unit tests cover core logic and edge cases
- Tests map clearly to AC
- Tests assert behavior, not implementation details
- Deterministic tests for deterministic logic

---

## Example (Simplified)

**Task:** Implement deterministic state transition

Good practice:
- pure function for state transition
- no hidden mutable state
- explicit ordering
- unit tests asserting same input → same output

Avoid:
- relying on dictionary iteration order
- hidden static state
- implicit defaults

---

## Red Flags
- Large methods doing multiple things
- Swallowed exceptions
- Magic values without explanation
- Tight coupling to framework or infrastructure
- Tests written after bugs appear

---

## References (Community Alignment)

This capability is **derived from and aligned with** the following community skills
(from `antigravity-awesome-skills`, used as read-only reference):

- `skills/clean-code`
- `skills/csharp-pro`
- `skills/backend-development-feature-development`
- `skills/backend-security-coder`
- `skills/code-review-checklist`
- `SKILL_ANATOMY.md`
- `QUALITY_BAR.md`

These references inform:
- coding standards
- architectural discipline
- security awareness
- test-first mindset
- professional code review expectations

---

## Limitations
- This skill does not redefine requirements.
- This skill does not redesign architecture.
- This skill does not perform planning or review.

---

## Related Capabilities
- clarify-work-item
- task-decomposer
- implementation-planner
- task-review
- project-memory-curation
