---
name: implementation-planner
description: Plan how a task should be implemented safely, clearly, and reviewably without writing code.
risk: safe
source: personal
---

# Implementation Planner

## Purpose
Provide a **clear, risk-aware implementation plan** that bridges
*clarified requirements* and *actual coding*.

This skill encodes the **tech-lead / senior engineer mindset**:
- decide *what to do and in what order*
- define *boundaries and responsibilities*
- surface *risks before code exists*
- make implementation and review predictable

The output of this skill is a **plan**, not code.

---

## When to Use
Use this skill when:
- starting implementation of a task with non-trivial logic
- correctness, determinism, security, or data integrity matter
- multiple components or steps are involved
- you want to keep PRs small and reviewable

Do **NOT** use this skill to:
- clarify requirements (use `clarify-work-item`)
- decompose work items into tasks (use `task-decomposer`)
- write code or choose variable names

---

## Inputs
- Task definition (from `task-decomposer`)
- Task-level Acceptance Criteria (AC)
- Relevant project documentation
- Known constraints (tech stack, performance, security, legacy)
- Applicable technical lenses (e.g. dotnet, react, sql, blockchain)

---

## Outputs
An **implementation plan** containing:
- Ordered implementation steps (WHAT, not HOW)
- Component responsibilities and boundaries
- Interfaces and contracts between components
- Identified risk points
- Test strategy aligned with AC
- Explicit decisions that must be made before coding

---

## Planning Principles

### 1️⃣ Plan for Reviewability
A good plan makes it obvious:
- what will be implemented
- where code will live
- how reviewers can verify correctness

If reviewers must reverse-engineer intent from code,
the plan is insufficient.

---

### 2️⃣ Separate Responsibilities Explicitly
Each component or step should have:
- a single primary responsibility
- clearly defined inputs and outputs

Avoid:
- hidden coupling
- shared mutable state without ownership
- “helper” components with unclear purpose

---

### 3️⃣ Identify Risk Before Code Exists
Common risk categories:
- correctness (edge cases, invariants)
- determinism (ordering, serialization)
- data integrity (partial updates, retries)
- security (trust boundaries, validation)
- performance (hot paths, N+1, memory)

If a risk is known, it must be written down.

---

### 4️⃣ Optimize for Small, Safe Steps
Prefer:
- multiple small PRs
- reversible changes
- incremental validation

Avoid:
- big-bang implementation
- tightly coupled changes that must land together

---

## Planning Structure (Step-by-Step)

### Step 1: Restate Task Intent
- One short paragraph describing *what success looks like*
- Reference the AC explicitly

Purpose: anchor the plan to agreed intent.

---

### Step 2: Identify Components & Responsibilities
List the components involved and define:
- responsibility
- owned data
- boundaries

Example:
- Validator: input validation and rule enforcement
- Executor: apply state changes
- Persistence: store results

(No class names, no method signatures.)

---

### Step 3: Define Interfaces & Contracts
For each boundary:
- expected inputs
- expected outputs
- error conditions

Focus on *behavioral contracts*, not implementation details.

---

### Step 4: Order the Implementation Steps
Define a clear sequence:
1. implement X
2. then implement Y
3. then integrate X and Y

Each step should:
- be independently testable
- ideally fit into one PR

---

### Step 5: Identify Risks & Mitigations
For each known risk:
- describe the failure mode
- describe mitigation or validation strategy

Example:
- Risk: nondeterministic iteration order
- Mitigation: enforce canonical ordering before processing

---

### Step 6: Define Test Strategy
For each AC:
- which test level applies? (unit / integration / e2e)
- what behavior must be asserted?

Avoid:
- testing implementation details
- relying solely on manual testing

---

### Step 7: Capture Pre-Code Decisions
List decisions that must be made *before coding*, such as:
- fail-fast vs partial success
- sync vs async behavior
- retry vs abort semantics

If a decision is not made, call it out as an **explicit unknown**.

---

## Example (Simplified)

**Task:** Implement deterministic state transition for transfers

**Plan Summary:**
- Components:
  - Validator (rules)
  - Executor (state update)
- Interfaces:
  - apply(state, tx) → newState | error
- Steps:
  1. Implement validation logic
  2. Implement pure state transition
  3. Add determinism tests
- Risks:
  - map iteration order
- Tests:
  - unit tests asserting same input → same output

---

## Red Flags
- Writing code in the plan
- Using class or method names prematurely
- Planning only “happy path”
- Ignoring test strategy
- Mixing requirement clarification into planning

---

## References (Community Alignment)

This capability is **derived from and aligned with** the following community skills
(from `antigravity-awesome-skills`, used as read-only reference):

- `skills/architecture`
- `skills/architecture-patterns`
- `skills/concise-planning`
- `skills/engineering-design-review`
- `skills/risk-analysis`
- `SKILL_ANATOMY.md`
- `QUALITY_BAR.md`

These references inform:
- responsibility and boundary thinking
- design-before-code discipline
- risk-first planning
- reviewable implementation planning

---

## Limitations
- This skill does not write code.
- This skill does not change requirements.
- This skill does not perform task decomposition.

---

## Related Capabilities
- clarify-work-item
- task-decomposer
- dotnet-coder
- react-coder
- task-review
