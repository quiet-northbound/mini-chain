# Threat Model (Mini) — MINI-LAYER2

## Purpose
State trust assumptions and key failure risks for the prototype.

## Trust assumptions (prototype)
- Sequencer is centralized and trusted for ordering (for now)
- Batch data is available to verifier (mock DA)
- No adversarial networking in scope

## Threats we care about
- Replay mismatch due to nondeterminism
- Ambiguous serialization causing different hashes
- Tampered batch payload leading to incorrect verification outcome

## Mitigations
- deterministic STF (M1)
- canonical formats (M2)
- explicit commitment verification procedure (M3)

## Non-goals
- censorship resistance
- MEV mitigation
- economic security analysis
