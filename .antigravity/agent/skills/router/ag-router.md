---
name: ag-router
description: Route user intent to the correct capability while enforcing role separation and quality gates.
risk: safe
source: personal
---

# Antigravity Intent Router

## Purpose
Provide a **single, explicit routing mechanism** that translates *user intent*
into the **correct capability** while enforcing:
- separation of concerns
- senior engineering discipline
- quality and safety gates

This router prevents:
- premature coding
- role confusion (planner acting as coder, reviewer acting as owner)
- silent skipping of critical steps (clarification, planning, review)

---

## When to Use
This router is implicitly applied:
- at the start of every session
- whenever user intent changes
- when ambiguity exists about “what to do next”

It does **not** replace human decision-making;
it enforces *process correctness*, not *outcomes*.

---

## Inputs
- User request (natural language)
- Session state (current intent, artifacts available)
- Existing project artifacts (specs, tasks, code, reviews)

---

## Outputs
- Selected capability
- Preconditions check (what must exist before proceeding)
- Explicit redirection or blocking message if conditions are not met

---

## Core Routing Principles

### 1️⃣ Route by Intent, Not by Phase
Routing is based on **what the user wants to do**, not where the project “is”.

Examples:
- “Help me understand…” → clarify
- “Break this down…” → decompose
- “How should I implement…” → plan
- “Write the code…” → implement
- “Review this…” → review
- “Record this lesson…” → memory

---

### 2️⃣ Enforce Role Boundaries
Each capability has a **non-overlapping responsibility**.

The router must prevent:
- coding before clarification
- planning before requirements
- reviewing without acceptance criteria

---

### 3️⃣ Prefer Blocking Over Guessing
If prerequisites are missing:
- block the action
- explain what is missing
- redirect to the correct capability

Guessing intent or filling gaps is forbidden.

---

## Routing Rules (Authoritative)

### Intent: Understand domain / concepts
**Keywords:** understand, explain, what is, concept, mental model  
→ `clarify-concept`

**Prerequisites:** none

---

### Intent: Clarify milestone / epic / task
**Keywords:** clarify, define requirements, acceptance criteria, scope  
→ `clarify-work-item`

**Prerequisites:** domain concepts reasonably understood

---

### Intent: Break work into tasks
**Keywords:** split, decompose, break down, task list  
→ `task-decomposer`

**Prerequisites:**
- clarified work item
- acceptance criteria present

---

### Intent: Plan implementation
**Keywords:** plan, design steps, approach, how to implement  
→ `implementation-planner`

**Prerequisites:**
- task defined
- task-level acceptance criteria present

---

### Intent: Implement code
**Keywords:** code, implement, write, develop  
→ appropriate coder skill:
- `dotnet-coder`
- `react-coder`
- other stack-specific coder skills

**Prerequisites:**
- implementation plan exists
- acceptance criteria agreed

---

### Intent: Review work
**Keywords:** review, check, assess, approve  
→ `task-review`

**Prerequisites:**
- task specification
- acceptance criteria
- code or artifact to review

---

### Intent: Capture learning / memory
**Keywords:** record, remember, lesson learned, pitfall, decision  
→ `project-memory-curation`

**Prerequisites:** concrete outcome or experience exists

---

## Blocking Rules (Hard Stops)

The router MUST block and redirect when:

- **Code requested without clarified requirements**
  → redirect to `clarify-work-item`

- **Planning requested without acceptance criteria**
  → redirect to `clarify-work-item`

- **Review requested without spec or AC**
  → request missing artifacts before review

- **Reviewer asked to redesign or implement**
  → redirect to `implementation-planner` or coder skill

---

## Router Decision Transparency

Every routing decision should be explainable in one sentence:
- what intent was detected
- which capability was selected
- what prerequisite is missing (if blocked)

This ensures:
- user trust
- debuggability
- long-term maintainability of the system

---

## Example Routing Scenarios

**User:** “Help me code this feature”  
→ Block  
→ Reason: no clarified requirements  
→ Redirect: `clarify-work-item`

---

**User:** “Review my PR”  
→ Check prerequisites  
→ If AC missing → block  
→ Else → `task-review`

---

**User:** “How should we structure this implementation?”  
→ `implementation-planner`

---

## References (Community Alignment)

This router is **derived from and aligned with** the following community skills
(from `antigravity-awesome-skills`, used as read-only reference):

- `skills/intent-detection`
- `skills/agent-orchestration`
- `skills/quality-gates`
- `skills/clarity-gate`
- `SKILL_ANATOMY.md`
- `QUALITY_BAR.md`
- `SECURITY_GUARDRAILS.md`

These references inform:
- intent-first routing
- safe action gating
- non-guessing behavior
- role separation enforcement

---

## Limitations
- This router does not execute capabilities itself.
- This router does not override human decisions.
- This router does not optimize for speed at the cost of correctness.

---

## Related Capabilities
- ag-session-bootstrap
- clarify-concept
- clarify-work-item
- task-decomposer
- implementation-planner
- dotnet-coder
- react-coder
- task-review
- project-memory-curation
