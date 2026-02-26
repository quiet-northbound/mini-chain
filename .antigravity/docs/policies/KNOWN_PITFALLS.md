# MINI-LAYER2 — Known Pitfalls

## P-001 Hidden nondeterminism
Signal:
- using timestamps, random values, unordered iteration
Impact:
- replay mismatch, unverifiable commitments
Mitigation:
- pure deterministic STF, stable serialization, explicit ordering

## P-002 Scope creep: proofs too early
Signal:
- designing fraud/zk proofs before STF and formats are stable
Impact:
- wasted effort, constant rewrites
Mitigation:
- freeze STF + formats first; proofs only as placeholder spec

## P-003 Vague trust assumptions
Signal:
- “we assume honest” without listing what is trusted
Impact:
- unclear security model; wrong expectations
Mitigation:
- write mini threat model and assumptions explicitly
