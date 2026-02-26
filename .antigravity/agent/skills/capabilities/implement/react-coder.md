---
name: react-coder
description: Implement tasks using professional React practices with emphasis on clarity, correctness, performance, accessibility, and security.
risk: safe
source: personal
---

# React Coder

## Purpose
Implement clarified and planned frontend tasks using **professional React practices**
to produce code that is:
- correct and predictable
- maintainable and readable
- performant enough by design (no accidental slow UI)
- accessible by default
- safe against common frontend security risks

This skill reflects how a **senior frontend engineer** implements features—not how a UI code generator dumps components.

---

## When to Use
Use this skill when:
- implementing UI features or frontend logic in React
- working with component-based design
- correctness, maintainability, accessibility, and security matter

Do **NOT** use this skill to:
- clarify requirements
- redesign APIs or backend contracts
- bypass the implementation plan or acceptance criteria
- introduce “quick hacks” that become permanent

---

## Inputs
- Task definition (from `task-decomposer`)
- Implementation plan (from `implementation-planner`)
- Task-level Acceptance Criteria (AC)
- Relevant UX constraints (if any)
- Applicable technical lenses (e.g. security, api, sql, domain)

---

## Outputs
- Production-ready React code (components + hooks + state)
- Tests where logic or critical behavior exists
- Minimal necessary documentation (README notes or inline comments)
- Updates to changelog or memory if patterns/pitfalls were discovered

---

## Implementation Principles

### 1️⃣ Component Responsibility Is Sacred
Each component should have:
- one primary responsibility
- clear inputs (props) and outputs (callbacks/events)

Avoid:
- “god components”
- components that fetch data, manage complex state, and render everything

---

### 2️⃣ Prefer Predictable State Over Clever State
- keep state minimal
- derive values instead of storing duplicates
- ensure state ownership is clear (local vs shared)

Avoid:
- storing computed values that can drift
- “prop drilling” without intent
- uncontrolled side effects

---

### 3️⃣ Side Effects Must Be Explicit and Contained
- use effects only for side effects
- keep dependencies correct
- cleanup resources reliably

Avoid:
- effects that compute derived UI state
- missing dependency arrays
- hidden subscriptions

---

### 4️⃣ Security Is Not Optional (Frontend Edition)
- treat all external strings as untrusted
- avoid unsafe HTML injection
- validate inputs before sending to backend
- do not leak secrets in the client

Avoid:
- `dangerouslySetInnerHTML` unless strictly required and sanitized
- building HTML strings manually
- trusting query params without validation

---

### 5️⃣ Accessibility by Default
- keyboard navigation should work
- semantic HTML first
- ARIA only when needed

Avoid:
- div-as-button
- missing labels
- inaccessible interactive elements

---

## React Implementation Checklist

### Component Design
- Components have clear names and single responsibility
- Props are explicit and typed (TypeScript strongly recommended)
- Event handlers are named consistently (`onSave`, `onCancel`)

---

### Hooks Usage
- Follow Rules of Hooks (no conditional hooks)
- Extract reusable logic into custom hooks
- Keep dependency arrays correct

---

### State Management
Use a decision guide:
- Local component state for local UI concerns
- Context for cross-tree shared state (small, stable)
- Dedicated state lib only if complexity demands it

Avoid:
- “global state by default”
- deeply nested contexts without boundaries

---

### Performance Basics (No Premature Micro-Optimizing)
- avoid unnecessary re-renders caused by unstable props
- memoize only when measured or clearly necessary
- keep lists keyed correctly

Red flags:
- inline object/array props passed deep
- re-render storms
- expensive computations inside render without memoization

---

### Error Handling & UX
- show clear error states
- loading states are explicit
- avoid silent failures

---

### Testing Strategy
- test behavior, not implementation details
- prefer React Testing Library patterns:
  - user interactions
  - rendered output
- add tests for:
  - validation logic
  - critical flows
  - regressions

---

## Example (Simplified)

**Task:** Build a transaction submission form

Good practice:
- component split:
  - `TxForm` (form + validation)
  - `TxSubmitButton` (interaction)
  - `useSubmitTx` (side effect hook)
- acceptance criteria:
  - invalid input blocked
  - success state visible
  - error surfaced clearly
- tests:
  - “user enters invalid amount → error message appears”
  - “user submits → calls submit handler”

Avoid:
- a single component doing fetch + state + render + validation + navigation
- silent errors
- unsafe HTML rendering

---

## Red Flags
- “just add state here” without ownership clarity
- `useEffect` used as a general computation engine
- uncontrolled inputs without validation
- `dangerouslySetInnerHTML` without sanitization
- inaccessible interactive UI

---

## References (Community Alignment)

This capability is **derived from and aligned with** the following community skills
(from `antigravity-awesome-skills`, used as read-only reference):

- `skills/clean-code`
- `skills/frontend-coding-standards`
- `skills/react-pro`
- `skills/xss-html-injection`
- `skills/ui-accessibility-basics`
- `skills/code-review-checklist`
- `SKILL_ANATOMY.md`
- `QUALITY_BAR.md`

These references inform:
- component and hook discipline
- maintainable UI coding standards
- baseline security expectations in frontend work
- accessibility and usability practices
- test strategies aligned with acceptance criteria

---

## Limitations
- This skill does not redefine requirements.
- This skill does not redesign architecture or API contracts.
- This skill does not perform planning or review.

---

## Related Capabilities
- clarify-work-item
- task-decomposer
- implementation-planner
- task-review
- project-memory-curation
