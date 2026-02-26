# Antigravity Skill References

## Purpose
This document provides a **clear, explicit mapping** between:
- personal Antigravity skills & capabilities
- community skills from `antigravity-awesome-skills`

Its goals are to:
- make the system defensible and traceable
- show alignment with community standards
- avoid copying vendor content while still “standing on the shoulders” of it
- help future-you understand *why* each skill exists

This file is **descriptive**, not executable.

---

## Reference Rules

- Community skills are used as **read-only inspiration and alignment**
- No runtime dependency on `antigravity-awesome-skills`
- Personal skills may merge or refine multiple community skills
- Personal skills may intentionally diverge (with rationale)

---

## Capability Reference Map

### Core & Orchestration

| Personal Skill | Community References |
|---------------|----------------------|
| ag-constitution | `SKILL_ANATOMY.md`, `QUALITY_BAR.md`, `SECURITY_GUARDRAILS.md` |
| ag-session-bootstrap | `skills/agent-orchestration`, `skills/context-initialization` |
| ag-router | `skills/intent-detection`, `skills/agent-orchestration`, `skills/quality-gates`, `skills/clarity-gate` |

---

### Clarify Capabilities

| Personal Skill | Community References |
|---------------|----------------------|
| clarify-concept | `skills/clarity-gate`, `skills/context-fundamentals`, `skills/domain-modeling`, `skills/architecture-thinking` |
| clarify-work-item | `skills/business-analyst`, `skills/concise-planning`, `skills/clarity-gate`, `skills/context-fundamentals` |

---

### Decomposition & Planning

| Personal Skill | Community References |
|---------------|----------------------|
| task-decomposer | `skills/feature-decomposition`, `skills/work-breakdown-structure`, `skills/engineering-task-slicing`, `skills/concise-planning` |
| implementation-planner | `skills/architecture`, `skills/architecture-patterns`, `skills/engineering-design-review`, `skills/risk-analysis`, `skills/concise-planning` |

---

### Implementation (Coder Skills)

| Personal Skill | Community References |
|---------------|----------------------|
| dotnet-coder | `skills/clean-code`, `skills/csharp-pro`, `skills/backend-development-feature-development`, `skills/backend-security-coder`, `skills/code-review-checklist` |
| react-coder | `skills/clean-code`, `skills/react-pro`, `skills/frontend-coding-standards`, `skills/xss-html-injection`, `skills/ui-accessibility-basics`, `skills/code-review-checklist` |

---

### Review & Memory

| Personal Skill | Community References |
|---------------|----------------------|
| task-review | `skills/code-review-excellence`, `skills/code-review-checklist`, `skills/architect-review`, `skills/security-guardrails` |
| project-memory-curation | `skills/agent-memory-systems`, `skills/architecture-decision-records`, `skills/context-compression` |

---

## Notes on Divergence

Some personal skills intentionally **merge multiple community skills** into one:
- `task-review` merges intent review + code review + risk review
- `implementation-planner` combines planning, architecture thinking, and risk analysis

This reflects **real senior-engineer workflows**, not isolated job roles.

---

## Versioning Guidance

- When updating personal skills:
  - update this mapping if references change
- When updating vendor repo:
  - re-evaluate alignment, do not auto-sync

---

## Non-Goals

- This file does not describe execution flow
- This file does not duplicate community content
- This file does not lock versions of vendor skills

---

## Status
Current alignment status: **intentional, explicit, stable**

Last reviewed: _(fill when you want to version)_
