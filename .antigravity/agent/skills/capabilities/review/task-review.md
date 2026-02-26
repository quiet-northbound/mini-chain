---
name: task-review
description: End-to-end review of a task, covering intent (spec), implementation, quality, and risk.
risk: safe
source: personal
---

# Task Review

## Purpose
Provide a **single, unified review capability** to assess a task end-to-end:
- whether the task intent is correct and complete
- whether the implementation matches that intent
- whether code quality, safety, and maintainability meet professional standards

This skill reflects how a **senior engineer reviews work in practice**, regardless of whether:
- the task was implemented by yourself (self-review), or
- the task was implemented by someone else (peer-review).

---

## When to Use
Use this skill when:
- reviewing a completed task before merge
- reviewing your own work to reduce blind spots
- reviewing another developer’s work to protect system quality
- validating that implementation truly satisfies agreed acceptance criteria

Do **NOT** use this skill to:
- redefine requirements
- redesign architecture
- implement fixes directly

---

## Inputs
Depending on task maturity, inputs may include:
- Task specification or work-item clarification
- Acceptance Criteria (AC)
- Code changes (PR diff, commit list)
- Tests (unit / integration)
- Relevant project documentation
- Applicable technical lenses (e.g. dotnet, react, sql)

---

## Outputs
- Review notes (blocking / non-blocking)
- Identified risks and gaps
- Questions for clarification (if intent is unclear)
- Suggestions for improvement
- Recommendation: approve / request changes / clarify intent

---

## Review Structure (Mandatory Order)

### 1️⃣ Intent Review (Spec & Requirements)
Goal: ensure **we are building the right thing**.

Checklist:
- Is the task goal clearly stated?
- Are acceptance criteria explicit and testable?
- Are business and technical requirements consistent?
- Are assumptions and constraints documented?
- Are there unresolved unknowns that affect correctness?

Red flags:
- implicit assumptions not written down
- AC that can be “interpreted” multiple ways
- scope creep hidden inside implementation

---

### 2️⃣ Correctness Review (Behavior vs AC)
Goal: ensure **the code actually does what was agreed**.

Checklist:
- Does implementation fully cover all AC?
- Are edge cases handled or explicitly rejected?
- Does behavior match spec in failure scenarios?
- Is behavior deterministic where required?

Red flags:
- “works for happy path only”
- silent failure or swallowed errors
- logic that depends on undocumented behavior

---

### 3️⃣ Quality & Maintainability Review
Goal: ensure **the solution will survive future change**.

Checklist:
- Clear separation of responsibilities
- Readable naming and structure
- No unnecessary complexity
- Reasonable abstractions (not over-engineered)
- Code is understandable without reading commit history

Red flags:
- god classes / god functions
- duplication without justification
- clever but fragile logic

---

### 4️⃣ Safety & Security Review
Goal: ensure **the solution does not introduce avoidable risk**.

Checklist:
- Input validation is explicit
- Error handling is consistent and intentional
- Sensitive data is not leaked via logs or exceptions
- Authorization and trust boundaries are respected
- No obvious security smells (injection, unsafe deserialization, etc.)

Red flags:
- assuming “trusted input” without stating why
- logging secrets or PII
- inconsistent error handling paths

---

### 5️⃣ Evidence Review (Tests & Proof)
Goal: ensure **claims are backed by evidence**.

Checklist:
- Tests exist for core logic and edge cases
- Tests map clearly to AC
- Failure scenarios are tested where risk is non-trivial
- Manual verification steps are documented if tests are not feasible

Red flags:
- “tested manually” without explanation
- tests that assert implementation details instead of behavior
- missing tests for known risky areas

---

## Self-Review vs Peer-Review Notes

### Self-Review (bias-aware)
When reviewing your own task:
- Actively question your own assumptions
- Look for shortcuts you justified to yourself
- Re-read AC as if written by someone else

### Peer-Review (quality gate)
When reviewing others:
- Focus on clarity and evidence, not personal style
- Ask for clarification instead of guessing intent
- Block only on correctness, safety, or missing requirements

---

## Output Format (Recommended)

- **Blocking issues**
  - must be resolved before merge
- **Non-blocking suggestions**
  - improvements or refactors
- **Questions**
  - items requiring clarification
- **Overall assessment**
  - approve / request changes / clarify intent

---

## References (Community Alignment)

This capability is **derived from and aligned with** the following community skills
(from `antigravity-awesome-skills`, used as read-only reference):

- `skills/code-review-excellence`
- `skills/code-review-checklist`
- `skills/architect-review`
- `skills/security-guardrails`
- `SKILL_ANATOMY.md`
- `QUALITY_BAR.md`

These references inform:
- review structure
- checklist depth
- safety boundaries
- expectations of a professional reviewer

---

## Limitations
- This skill does not implement fixes.
- This skill does not redefine scope or requirements.
- Architectural redesign requires a separate planning step.

---

## Related Capabilities
- clarify-work-item
- implementation-planner
- dotnet-coder
- react-coder
- project-memory-curation
