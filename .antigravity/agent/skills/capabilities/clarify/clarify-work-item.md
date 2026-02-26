---
name: clarify-work-item
description: Clarify a milestone, epic, or task into explicit, testable business and technical requirements.
risk: safe
source: personal
---

# Clarify Work Item

## Purpose
Transform a vague or high-level **work item** (milestone, epic, or task)
into **clear, explicit, and testable requirements** that can be safely
decomposed, planned, implemented, and reviewed.

This skill ensures:
- everyone agrees on *what success means*
- requirements are explicit, not assumed
- downstream planning and coding are not forced to guess intent

---

## When to Use
Use this skill when:
- starting a new milestone, epic, or task
- requirements feel incomplete, ambiguous, or implicit
- before task decomposition or implementation planning
- disagreements exist about scope or expected behavior

Do **NOT** use this skill to:
- design architecture or implementation
- split work into tasks (use `task-decomposer`)
- write code or tests

---

## Inputs
- Work item description (milestone / epic / task)
- Relevant domain knowledge
- Existing decisions or constraints
- High-level business context (if available)

---

## Outputs
A **clarified work item** containing:
- One-sentence goal
- Business requirements
- Technical requirements (WHAT, not HOW)
- Acceptance Criteria (AC)
- Constraints
- Risks & unknowns

---

## Clarification Principles

### 1️⃣ No Guessing
If information is missing or unclear:
- write it down as an unknown
- ask for clarification
- do not fill gaps with assumptions

---

### 2️⃣ Separate WHAT from HOW
- **WHAT** = observable behavior or outcome
- **HOW** = implementation detail (explicitly excluded)

---

### 3️⃣ Make Acceptance Criteria Testable
If a criterion cannot be objectively verified,
it is not a valid acceptance criterion.

---

### 4️⃣ Prefer Explicit Scope Boundaries
Clearly state:
- what is included
- what is explicitly excluded

This prevents scope creep during implementation.

---

## Clarification Structure (Step-by-Step)

### Step 1: Restate the Goal
- One clear sentence describing success
- Written as if explaining to a new team member

Example:
> “Enable deterministic processing of transfer transactions in the Mini Layer2 node.”

---

### Step 2: Identify Stakeholders & Value
- Who cares about this work item?
- What problem does it solve?
- What failure would be unacceptable?

(This anchors technical decisions to purpose.)

---

### Step 3: Business Requirements
Describe expected behavior in business terms:
- user-visible outcomes
- system guarantees
- success conditions

Avoid:
- implementation language
- internal technical structure

---

### Step 4: Technical Requirements (WHAT)
Describe system behavior from a technical perspective:
- inputs and outputs
- rules and constraints
- error conditions
- performance or safety expectations

Still avoid:
- algorithms
- class or method names
- frameworks

---

### Step 5: Acceptance Criteria (AC)
Define **objective, testable criteria**.

Use Given / When / Then style where appropriate.

Example:
- Given a valid transaction  
  When it is processed  
  Then the resulting state root is deterministic

Each AC should be:
- unambiguous
- verifiable
- tied to observable behavior

---

### Step 6: Constraints
List known constraints, such as:
- technology choices
- performance limits
- security requirements
- backward compatibility

---

### Step 7: Risks & Unknowns
Explicitly list:
- open questions
- assumptions requiring confirmation
- areas of technical or domain uncertainty

Unknowns are **not failures** — hiding them is.

---

## Example (Simplified)

**Work Item:**  
“Support deterministic transfer execution”

**Clarified Output (Excerpt):**
- Goal: Ensure transfer execution is deterministic across runs
- Business requirement: Same inputs must always produce same outputs
- Technical requirement: Transaction processing order must be canonical
- AC: Given identical initial state and transactions, resulting state root is identical
- Risk: Map iteration order may introduce nondeterminism

---

## Red Flags
- Requirements embedded in code suggestions
- AC that rely on subjective interpretation
- Silent assumptions not written down
- Mixing future ideas into current scope

---

## References (Community Alignment)

This capability is **derived from and aligned with** the following community skills
(from `antigravity-awesome-skills`, used as read-only reference):

- `skills/clarity-gate`
- `skills/business-analyst`
- `skills/concise-planning`
- `skills/context-fundamentals`
- `SKILL_ANATOMY.md`
- `QUALITY_BAR.md`

These references inform:
- requirement clarity standards
- separation of intent vs implementation
- acceptance-criteria discipline
- ambiguity detection

---

## Limitations
- This skill does not decompose work items.
- This skill does not plan implementation.
- This skill does not implement solutions.

---

## Related Capabilities
- clarify-concept
- task-decomposer
- implementation-planner
- task-review
