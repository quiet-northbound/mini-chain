# Rollup Flow Overview — MINI-LAYER2

## Purpose | Mục đích
Tổng quan luồng rollup từ user tx đến verification.
*High-level view of the rollup flow from user transaction to verification.*

## Minimal prototype flow

```
┌──────────┐     ┌──────────────┐     ┌───────────┐     ┌────────────┐     ┌──────────┐
│  1. User │────▶│ 2. Sequencer │────▶│ 3. STF    │────▶│ 4. Commit  │────▶│ 5. Verify│
│  creates │     │ orders txs   │     │ execute   │     │ to L1      │     │ replay   │
│  L2 txs  │     │ into batch   │     │ S0→S1, R1 │     │ (mock ok)  │     │ batch    │
└──────────┘     └──────────────┘     └───────────┘     └────────────┘     └──────────┘
```

| Step | Mô tả | Milestone | Status |
|------|--------|-----------|--------|
| 1 | User tạo signed transaction | M0+M1 | ✅ Done |
| 2 | Sequencer gom tx vào batch | M4+M5 | 🔲 |
| 3 | Execute STF: validate → apply → state root | M0+M1 | ✅ Done |
| 4 | Produce commitment, submit to L1 | M4+M5 | 🔲 |
| 5 | Verifier replay batch, verify commitment | M4+M5 | 🔲 |

## What this proves | Điều này chứng minh gì
- **Ordered execution**: tx được xử lý theo thứ tự → kết quả nhất quán
- **Deterministic state transition**: cùng input → cùng output (đã test: `Test_StateRoot_Deterministic`)
- **Verifiable commitment**: ai cũng có thể replay và so sánh state root

## Current Implementation (M0+M1)
Step 1 + 3 đã được implement:
- User tạo `Transaction` → `MockSigner.Sign()` → `Ledger.SubmitTransaction()`
- Ledger validate (fail-fast 8 checks) → apply (debit/credit/nonce) → receipt
- `Ledger.GetStateRoot()` → SHA-256 of canonical state

## Trust assumptions (prototype)
- Centralized sequencer for ordering (single-thread, single node)
- Batch data available for verifier (mock DA — data availability)
- No adversarial networking

## Changelog
- v0: initial draft
- v2: added flow diagram, milestone status tracking, M0+M1 implementation links
