---
name: ag-session-bootstrap
description: Establish session intent, role, inputs, outputs, and constraints before any capability is executed.
risk: safe
source: personal
---

# Session Bootstrap

## Purpose
Establish a **clear working contract** at the beginning of every session
so that all subsequent actions are:
- intentional
- role-correct
- aligned with quality and safety standards

This skill prevents:
- silent intent switching
- acting without sufficient context
- capabilities being used out of order

---

## When to Use
Use this skill:
- at the start of every new session
- when the user changes intent (e.g. from clarify → implement)
- when confusion arises about “what we are doing right now”

This skill is **mandatory** before executing any capability.

---

## Inputs
- User’s initial request (natural language)
- Existing project artifacts (if any)
- Current session state (if resuming work)

---

## Outputs
A **Session Contract** explicitly stating:
- Intent
- Active role
- Inputs
- Expected outputs
- Constraints and assumptions

No other capability should proceed without this contract being clear.

---

## Session Bootstrap Process

### Step 1: Declare Intent
Identify what the user wants to do *right now*.

Examples:
- clarify a concept
- clarify a work item
- decompose tasks
- plan implementation
- implement code
- review work
- record memory

If intent is ambiguous, **ask for clarification**.

---

### Step 2: Confirm Active Role
Map intent to role:
- Clarifier
- Planner
- Implementer
- Reviewer
- Memory curator

Roles must not be mixed within the same step.

---

### Step 3: Identify Inputs
Explicitly list:
- documents
- specs
- tasks
- code
- assumptions

Missing inputs must be called out.

---

### Step 4: Define Expected Outputs
Clarify what artifacts should be produced:
- clarified requirements
- task list
- implementation plan
- code
- review notes
- memory entries

---

### Step 5: Surface Constraints
Identify constraints such as:
- tech stack
- quality bar
- time or scope limits
- safety rules

---

## Session Contract Template

```text
Intent:
Role:
Inputs:
Expected Outputs:
Constraints / Assumptions:
