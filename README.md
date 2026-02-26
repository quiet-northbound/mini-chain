# MINI-LAYER2 (Learning Project)

## What this repo is
Tự tay xây dựng một mini blockchain (Layer 1) và Layer 2 rollup-like prototype để hiểu sâu:
- Deterministic state transition (STF)
- Transaction validation, apply, và replay protection
- Batching, commitment, và replay verification (mock)

## What this repo is NOT
- No fraud/zk proofs (chỉ mock proof ở M5)
- No decentralized sequencer
- No production networking / P2P
- No smart contract / VM

## Project Status

| Milestone | Mô tả | Status |
|-----------|--------|--------|
| **M0 + M1** | Ledger + accounts + signed tx + validate/apply | ✅ Done |
| **M2 + M3** | Block & chain history + token module | 🔲 Pending |
| **M4 + M5** | Layer 2 executor + commit to L1 + replay verify | 🔲 Pending |

## Tech Stack
- **C# / .NET 8** — single node, local execution
- **SHA-256** — hashing (prototype)
- **Canonical JSON** — deterministic serialization
- **xUnit** — testing

## Where to start

### 1) Kiến thức nền
- [docs/knowledge/BLOCKCHAIN_PRIMER.md](docs/knowledge/BLOCKCHAIN_PRIMER.md) — thuật ngữ cơ bản
- [docs/knowledge/STATE_TRANSITION_MODEL.md](docs/knowledge/STATE_TRANSITION_MODEL.md) — mô hình STF
- [docs/knowledge/ROLLUP_FLOW_OVERVIEW.md](docs/knowledge/ROLLUP_FLOW_OVERVIEW.md) — luồng rollup

### 2) Specs (source of truth)
- [docs/specs/Project_overview.md](docs/specs/Project_overview.md) — tổng quan project
- [docs/specs/M2_M3.md](docs/specs/M2_M3.md) — spec cho M0–M3
- [docs/specs/M4_M5.md](docs/specs/M4_M5.md) — spec cho M4–M5

### 3) Architecture
- [docs/architecture/DATA_FORMATS.md](docs/architecture/DATA_FORMATS.md) — canonical JSON, state shape
- [docs/architecture/COMPONENTS.md](docs/architecture/COMPONENTS.md) — component overview

### 4) Source code (M0+M1)
- [src/MiniChain/MiniChain.Core/](src/MiniChain/MiniChain.Core/) — thư viện chính
- [src/MiniChain/MiniChain.Tests/](src/MiniChain/MiniChain.Tests/) — 8 test cases

## Quick Start
```bash
cd src/MiniChain
dotnet build
dotnet test --verbosity normal
```
