# State Transition Model — MINI-LAYER2
# Mô hình chuyển trạng thái — MINI-LAYER2

## Purpose | Mục đích
Define the STF model and link to actual implementation.
*Định nghĩa mô hình STF và liên kết tới code thực tế.*

## STF (abstract) | STF (trừu tượng)
Input *(đầu vào)*:
- S0 — initial state *(trạng thái ban đầu)* → `Ledger.Accounts` trước khi apply
- T[] — ordered tx list *(danh sách giao dịch có thứ tự)* → `Transaction` objects

Output *(đầu ra)*:
- S1 — final state *(trạng thái cuối)* → `Ledger.Accounts` sau khi apply
- R1 — state root derived from S1 → `Ledger.GetStateRoot()`
- errors[] — list of skipped invalid txs → `TxReceipt` with `Status=Failed`

## Required properties | Thuộc tính bắt buộc
- **Deterministic** *(tất định)* — cùng đầu vào luôn cho cùng đầu ra
- **Replayable** *(phát lại được)* — ai cũng có thể chạy lại và ra kết quả giống nhau
- **Explicit failure handling** *(xử lý lỗi tường minh)* — hành vi khi gặp lỗi được xác định rõ

## Implementation (M0+M1) | Triển khai thực tế

### Validation Pipeline (fail-fast)
Thứ tự kiểm tra trong `TxValidator.Validate()`:
1. ChainId đúng → `INVALID_CHAIN_ID`
2. Signature hợp lệ (mock) → `INVALID_SIGNATURE`
3. Replay check (txId chưa thấy) → `REPLAY_TX`
4. Amount > 0 → `INVALID_AMOUNT`
5. from ≠ to → `SELF_TRANSFER`
6. Sender tồn tại → `ACCOUNT_NOT_FOUND`
7. Nonce khớp → `INVALID_NONCE`
8. Balance đủ → `INSUFFICIENT_BALANCE`

### Apply Logic
Trong `Ledger.SubmitTransaction()`:
```
sender.Balance -= amount
receiver.Balance += amount  (auto-create nếu chưa có)
sender.Nonce += 1
seenTxIds.Add(txId)
→ TxReceipt(Success)
```

### Mock Signature
Format: `SIG(from|to|amount|nonce|chainId)`  
→ `MockSigner.Sign()` / `MockSigner.Verify()`

## State root derivation | Cách tính gốc trạng thái
**✅ DECIDED: hash(canonical_json(sorted_state))** — ref: M0 DECISION-2

```
R1 = SHA-256(canonical_json(sorted_state))
```

State shape:
```json
{"balances":{"alice":90,"bob":10},"nonces":{"alice":1}}
```
- `balances` trước `nonces` (alphabetical)
- Bên trong mỗi object: address sorted alphabetical
- Zero values có thể bỏ qua

→ Xem chi tiết: [DATA_FORMATS.md](../architecture/DATA_FORMATS.md)

## Failure handling | Xử lý lỗi
**✅ DECIDED: skip-invalid** — ref: M0 DECISION-1
- Ghi nhận lỗi (TxReceipt Failed), tiếp tục xử lý giao dịch tiếp theo
- State KHÔNG thay đổi khi tx bị reject
- TxId KHÔNG được thêm vào seenTxIds khi fail

## Resolved Decisions | Quyết định đã giải quyết
- ~~canonical address format~~: ✅ lowercase string (e.g. "alice", "bob")
- ~~state root derivation method~~: ✅ SHA-256(canonical_json(sorted_state))
- ~~serialization rules~~: ✅ canonical JSON — ref: M0 DECISION-3
- ~~failure handling~~: ✅ skip-invalid with failed receipt

## Key Source Files
| File | Role |
|------|------|
| [Ledger.cs](../../src/MiniChain/MiniChain.Core/State/Ledger.cs) | State management + submit flow |
| [TxValidator.cs](../../src/MiniChain/MiniChain.Core/Validation/TxValidator.cs) | Fail-fast validation |
| [CanonicalJson.cs](../../src/MiniChain/MiniChain.Core/Serialization/CanonicalJson.cs) | Deterministic serialization |
| [Hasher.cs](../../src/MiniChain/MiniChain.Core/Crypto/Hasher.cs) | SHA-256 |
| [MockSigner.cs](../../src/MiniChain/MiniChain.Core/Crypto/MockSigner.cs) | Mock sign/verify |

## Changelog
- v0: initial draft
- v1: resolved all unknowns — skip-invalid, hash canonical state, canonical JSON, lowercase addresses
- v2: linked to actual M0+M1 implementation, added validation pipeline detail and apply logic
