---
name: task-decomposer
description: Decompose a clarified work item into small, executable, and reviewable tasks.
risk: safe
source: personal
---

# Task Decomposer

## Purpose
Provide a **systematic way to break down a clarified work item**
(milestone / epic / large task) into **small, independent, and testable tasks**
that can be safely planned, implemented, and reviewed.

This skill ensures:
- work is sliced at the right boundaries
- each task has clear ownership and acceptance criteria
- implementation and review remain manageable and predictable

---

## When to Use
Use this skill when:
- a work item has been clarified (via `clarify-work-item`)
- the scope is too large to implement or review in one step
- you want to reduce risk by delivering in small increments

Do **NOT** use this skill to:
- clarify requirements (use `clarify-work-item`)
- design architecture in detail (use `implementation-planner`)
- write code

---

## Inputs
- Clarified work item (business + technical requirements)
- Acceptance Criteria (AC) at work-item level
- Known constraints (time, tech stack, dependencies)

---

## Outputs
A list of **tasks**, where each task includes:
- Task goal (one clear outcome)
- Scope (what is included / excluded)
- Acceptance Criteria (task-level, testable)
- Dependencies (other tasks or external factors)
- Suggested implementation skill (e.g. dotnet-coder, react-coder)

---

## Decomposition Principles

### 1️⃣ Slice by Outcome, Not by Layer
Prefer:
- “Implement balance validation for transfers”
Over:
- “Backend validation layer”

Each task should deliver a **meaningful, verifiable outcome**.

---

### 2️⃣ Keep Tasks Independently Reviewable
Each task should ideally:
- fit into a single PR
- be reviewable without understanding the entire system
- have its own AC

---

### 3️⃣ Respect Natural Boundaries
Good boundaries include:
- distinct behaviors
- different risk profiles
- different tech stacks (backend vs frontend)
- different data ownership

---

### 4️⃣ Preserve End-to-End Flow
Ensure that, when tasks are completed in sequence:
- the original work-item AC can be satisfied
- no “missing glue” is left unassigned

---

## Decomposition Process (Step-by-Step)

### Step 1: Identify Major Responsibilities
List the major responsibilities implied by the work item:
- validation
- execution
- persistence
- presentation
- integration

---

### Step 2: Split Responsibilities into Tasks
For each responsibility:
- define a task with a single clear goal
- ensure the task can be verified independently

---

### Step 3: Define Task-Level AC
For each task:
- translate relevant work-item AC into task-level AC
- ensure AC is objective and testable

---

### Step 4: Identify Dependencies
For each task:
- list prerequisite tasks
- note external dependencies or assumptions

---

### Step 5: Assign Suggested Skill
Indicate which implementation capability is most appropriate:
- dotnet-coder
- react-coder
- other stack-specific skills

(This is guidance, not assignment.)

---

## Example (Simplified)

**Work Item:**  
“Support deterministic balance transfers in Mini Layer2”

**Decomposed Tasks:**
1. Define and validate transfer transaction rules  
   - Skill: dotnet-coder  
   - AC: invalid nonce or insufficient balance is rejected deterministically

2. Implement state update logic for valid transfers  
   - Skill: dotnet-coder  
   - AC: balances and nonces updated correctly

3. Add unit tests for deterministic execution  
   - Skill: dotnet-coder  
   - AC: same input always produces same output

---

## Red Flags
- Tasks that are “do everything”
- Tasks without acceptance criteria
- Tasks coupled so tightly they must be merged together
- Decomposing by file or folder structure instead of behavior

---

## References (Community Alignment)

This capability is **derived from and aligned with** the following community skills
(from `antigravity-awesome-skills`, used as read-only reference):

- `skills/concise-planning`
- `skills/feature-decomposition`
- `skills/work-breakdown-structure`
- `skills/engineering-task-slicing`
- `SKILL_ANATOMY.md`
- `QUALITY_BAR.md`

These references inform:
- task slicing principles
- task sizing heuristics
- acceptance-criteria-driven decomposition
- reviewability standards

---

## Limitations
- This skill does not clarify requirements.
- This skill does not design architecture.
- This skill does not implement tasks.

---

## Related Capabilities
- clarify-work-item
- implementation-planner
- task-review
