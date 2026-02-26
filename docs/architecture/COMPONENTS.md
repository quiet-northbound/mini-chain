# Components (High Level) — MINI-LAYER2

## Purpose
Describe conceptual components without committing to code structure.

## Components
1) Execution Engine
- implements STF and state root derivation

2) Sequencer (prototype)
- orders transactions into a batch

3) Batch Store (prototype)
- stores batches for replay (local storage ok)

4) Commitment Producer
- produces commitment from metadata and state root
- posts to L1 (mock ok)

5) Verifier / Replayer
- replays batch to verify commitment

## Non-goals
- networking architecture
- production scalability
