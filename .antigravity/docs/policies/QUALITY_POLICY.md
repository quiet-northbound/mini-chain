# Antigravity — Quality Policy (MINI-LAYER2)

## Principles
- Correctness > speed
- Clarity > cleverness
- Spec-first
- Small changes, safe progress
- Always preserve learning in docs/memory

## Minimum quality bar for specs
Every spec must:
- define terms or reference docs/knowledge
- define inputs/outputs/invariants
- list failure modes
- include acceptance criteria (testable)
- list UNKNOWNs

## Minimum quality bar for implementation (M4)
Every implementation slice must:
- map to one spec section
- include tests for AC
- avoid hidden nondeterminism
- document assumptions and deviations
