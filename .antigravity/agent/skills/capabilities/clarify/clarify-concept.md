---
name: clarify-concept
description: Clarify domain concepts, mental models, invariants, and assumptions before defining requirements.
risk: safe
source: personal
---

# Clarify Concept

## Purpose
Establish a **shared, precise, and durable understanding of a domain or problem space**
*before* requirements, planning, or implementation begin.

This skill exists to:
- align mental models across people and time
- eliminate ambiguity in terminology
- surface hidden assumptions early
- prevent premature design and coding

It encodes how **senior engineers reason about unfamiliar or complex domains**.

---

## When to Use
Use this skill when:
- entering a new domain or technology
- working on systems with complex behavior (blockchain, distributed systems, finance, security)
- key terms are overloaded or used inconsistently
- requirements discussions feel confused or circular
- future correctness depends on shared understanding

Do **NOT** use this skill to:
- define business requirements
- define acceptance criteria
- design architecture or implementation

---

## Inputs
- High-level problem statement
- Domain documents or references (if available)
- Stakeholder explanations
- Existing system descriptions or diagrams

---

## Outputs
A **concept clarification artifact** containing:
- Glossary of key terms
- Core concepts and mental model
- Invariants (must-always-hold truths)
- Assumptions and constraints
- Explicit out-of-scope boundaries

This artifact should be readable **months later** without additional context.

---

## Conceptual Principles

### 1️⃣ Language Precedes Design
If a concept cannot be explained clearly in words,
it is not yet ready to be designed or implemented.

---

### 2️⃣ Concepts Are Stable, Implementations Change
Focus on:
- *what something fundamentally is*
- *what guarantees it provides*

Avoid:
- algorithms
- frameworks
- code structures

---

### 3️⃣ Invariants Are Non-Negotiable
An invariant defines:
- correctness boundaries
- design constraints
- test oracles

If an invariant is unclear, the system cannot be trusted.

---

## Clarification Structure (Step-by-Step)

### Step 1: Identify and Define Key Terms
List all important domain terms and define them unambiguously.

For each term:
- what it means
- what it does NOT mean

Avoid circular definitions.

---

### Step 2: Describe the Mental Model
Explain how the system works at a **conceptual level**:
- major entities
- how they interact
- lifecycle or flow

This can be text-first; diagrams are optional but helpful.

---

### Step 3: Identify Core Invariants
State properties that must always hold true.

Examples:
- determinism
- consistency
- immutability
- idempotency

Each invariant should be:
- explicit
- testable in principle
- independent of implementation

---

### Step 4: List Assumptions & Constraints
Capture assumptions such as:
- trust boundaries
- environmental guarantees
- external system behavior

Assumptions should be explicit, not implicit.

---

### Step 5: Define Out-of-Scope
Clearly state what this clarification does **not** cover.

This protects focus and prevents scope creep.

---

## Example (Simplified)

**Domain:** Mini Layer 2 (concept level)

- Term: *State*
  - Meaning: canonical representation of balances and nonces
  - Not: in-memory object graph

- Invariant:
  - Given identical initial state and transactions, resulting state must be identical

- Assumption:
  - Single sequencer, no adversarial ordering

- Out of scope:
  - Fraud proofs
  - ZK validity proofs

---

## Red Flags
- Terms used inconsistently
- Jumping to data structures or algorithms
- Hidden assumptions discovered late
- Treating implementation constraints as concepts

---

## References (Community Alignment)

This capability is **derived from and aligned with** the following community skills
(from `antigravity-awesome-skills`, used as read-only reference):

- `skills/clarity-gate`
- `skills/context-fundamentals`
- `skills/architecture-thinking`
- `skills/domain-modeling`
- `SKILL_ANATOMY.md`
- `QUALITY_BAR.md`

These references inform:
- concept-first reasoning
- invariant identification
- assumption surfacing
- long-lived documentation standards

---

## Limitations
- This skill does not define requirements.
- This skill does not define tasks.
- This skill does not make technical design decisions.

---

## Related Capabilities
- clarify-work-item
- implementation-planner
- task-review
