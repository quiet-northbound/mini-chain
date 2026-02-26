# Blockchain Primer — MINI-LAYER2

## Purpose | Mục đích
Minimal shared vocabulary for this repo.
*Thuật ngữ chung tối thiểu cho repo này.*

## Core Concepts | Khái niệm cốt lõi

### State (Trạng thái)
- Tập hợp tất cả accounts với balance và nonce tại một thời điểm
- State thay đổi khi transaction được apply thành công
- **Trong project**: `Ledger.Accounts` = `Dictionary<address, Account>`

### Transaction (Giao dịch)
- Lệnh thay đổi state: chuyển coin từ A sang B
- Phải có: from, to, amount, nonce, chainId, signature
- **TxId** = SHA-256(canonical JSON payload) → deterministic, dùng để anti-replay
- **Trong project**: `Transaction.cs` → `Ledger.SubmitTransaction(tx)`

### Nonce (Bộ đếm)
- Số đếm tx thành công từ mỗi account (bắt đầu từ 0)
- Tx chỉ hợp lệ khi `tx.nonce == account.nonce` → ngăn replay và đảm bảo thứ tự
- Nonce tăng 1 sau mỗi tx thành công

### Determinism (Tính tất định)
- Same initial state + same ordered inputs ⇒ same final state and state commitment
- Đây là thuộc tính quan trọng nhất — cho phép bất kỳ ai replay và verify kết quả
- **Trong project**: `CanonicalJson` + `Hasher` đảm bảo serialize/hash luôn cho cùng kết quả

### State Root (Gốc trạng thái)
- Hash duy nhất đại diện cho toàn bộ state tại một thời điểm
- `StateRoot = SHA-256(canonical_json(sorted_state))`
- Dùng để so sánh nhanh: 2 state giống nhau ⇔ cùng state root

## Concepts for Later Milestones | Khái niệm cho milestone sau
- **Block**: gom nhiều tx vào 1 đơn vị, có hash liên kết (→ M2/M3)
- **Batch**: nhóm tx xử lý off-chain trên L2 (→ M4/M5)
- **Commitment**: tóm tắt batch gửi về L1 (→ M4/M5)
- **Replay verification**: chạy lại batch để verify kết quả (→ M5)

## Out of Scope (giai đoạn này)
- Decentralization and censorship resistance
- Proof systems (fraud/zk) — chỉ mock ở M5
- Production networking

## Changelog
- v0: initial draft
- v2: enriched with implementation references from M0+M1
