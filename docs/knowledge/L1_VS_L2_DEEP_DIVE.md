# Layer 1 & Layer 2 — Toàn cảnh

> Tài liệu tổng hợp dành cho learning project Build-a-Mini-Layer2.
> Tổng hợp từ các cuộc thảo luận về L1, L2, rollup, verify, và triển khai.

---

## 1) Định nghĩa

### Layer 1 (L1) — Main Chain

**Vai trò**: Tầng nền tảng, là "nguồn sự thật" (source of truth) của toàn hệ thống.

**Đặc điểm**:
- Lưu trữ block & transaction **chính thức**
- Duy trì canonical state (state chuẩn tắc)
- Phi tập trung — nhiều validator/node cùng vận hành
- Chậm nhưng **đáng tin cậy nhất**
- Ví dụ: Ethereum, Bitcoin

**Ẩn dụ**: L1 giống **tòa án** — mọi thứ ghi vào đây đều chính thức và không ai xoá được, nhưng xử lý chậm và tốn phí.

### Layer 2 (L2) — Scaling Solution

**Vai trò**: Tầng mở rộng, xử lý giao dịch nhanh và rẻ hơn L1.

**Đặc điểm**:
- Xử lý tx **off-chain** (ngoài L1)
- Có state riêng biệt, hoàn toàn tách khỏi L1
- Gửi **bản tóm tắt** (commit) lên L1 để lưu bằng chứng
- Nhanh hơn, rẻ hơn L1 rất nhiều
- Ví dụ: Arbitrum, Optimism, zkSync

**Ẩn dụ**: L2 giống **văn phòng xử lý** — giải quyết công việc hàng ngày nhanh chóng, rồi chỉ gửi **bản báo cáo tóm tắt** lên tòa án.

---

## 2) Sự khác nhau

| Tiêu chí | L1 | L2 |
|----------|----|----|
| **Tốc độ** | Chậm (~15 tx/giây ETH) | Nhanh (~1000-4000 tx/giây) |
| **Phí** | Đắt ($1-50/tx) | Rẻ ($0.01-0.1/tx) |
| **Bảo mật** | Tự bảo mật (consensus) | Kế thừa bảo mật từ L1 |
| **Phi tập trung** | Cao (nhiều validator) | Thấp hơn (thường 1 sequencer) |
| **State** | State riêng | State riêng, tách biệt L1 |
| **Ai vận hành** | Cộng đồng validator | Đội phát triển L2 |
| **Dữ liệu** | Lưu toàn bộ block+tx | Gửi tóm tắt lên L1 |

---

## 3) Mối liên hệ

### 3.1 State hoàn toàn độc lập

L1 và L2 có **state riêng biệt**, không chia sẻ:

```
L1 state:    Alice = 80 ETH      ← tiền trên L1
L2-A state:  Alice = 20 ETH      ← tiền trên L2-A
L2-B state:  Alice = 5 ETH       ← tiền trên L2-B
```

3 balance trên không tự động cộng lại. Giống bạn có 3 ví (ngân hàng, MoMo, ZaloPay) — biết số dư MoMo không cần biết gì về ngân hàng.

### 3.2 Khi nào state nào thay đổi?

| Hành động | L1 state | L2 state |
|-----------|----------|----------|
| Giao dịch trên L1 | ✅ Thay đổi | ❌ Không ảnh hưởng |
| Giao dịch trên L2 | ❌ Không ảnh hưởng | ✅ Thay đổi |
| Bridge L1→L2 | ✅ Khoá tiền | ✅ Mint tiền |
| Bridge L2→L1 | ✅ Mở khoá tiền | ✅ Burn tiền |
| L2 gửi commit lên L1 | ✅ Thêm record (balance không đổi) | ❌ Không đổi |

### 3.3 Bridge: Di chuyển tiền giữa L1 ↔ L2

```
Nạp tiền vào L2 (deposit):
  L1: Alice 100 → khoá 20 vào Bridge contract → Alice 80 (+ 20 khoá)
  L2: Alice 0 → mint 20 → Alice 20
  Tổng: 80 + 20 = 100 ← bảo toàn

Rút tiền về L1 (withdraw):
  L2: Alice 20 → burn 20 → Alice 0
  L1: Bridge mở khoá 20 → Alice 80 + 20 = 100
  Tổng: 100 + 0 = 100 ← bảo toàn
```

### 3.4 Commit: L2 báo cáo cho L1

L2 **không gửi tx chi tiết** lên L1. Chỉ gửi **bản tóm tắt**:

```
L2Commit gửi lên L1:
  batchId:       "batch-42"
  txCount:       500
  preStateRoot:  "aaa111..."     ← hash state trước batch
  postStateRoot: "bbb222..."     ← hash state sau batch
```

L1 chỉ **lưu bản ghi** này — không apply 500 tx, không thay đổi balance.

---

## 4) Vận hành: Rollup flow

### 4.1 Không có "đồng thời"

Dù 1000 người gửi tx cùng lúc, blockchain xếp tất cả thành **1 hàng dọc** — thực thi tuần tự từng cái.

```
1000 tx "cùng lúc" → xếp thành 1 chuỗi:
  Tx1 → Tx2 → Tx3 → ... → Tx1000
  Thứ tự cố định, không bao giờ đổi.
```

Ai quyết định thứ tự?
- **L1**: Validator chọn tx vào block
- **L2**: Sequencer sắp thứ tự

### 4.2 Luồng xử lý end-to-end

```
                User
                  │
         ┌────────┴────────┐
         ▼                 ▼
    Gửi tx lên L1     Gửi tx lên L2
         │                 │
         ▼                 ▼
  L1 Validator       L2 Sequencer
  sắp xếp, apply    sắp xếp, apply
         │                 │
         ▼                 │  Mỗi N tx:
   Tạo L1 Block           ▼
         │           Gom batch, tính
         │           pre/post stateRoot
         │                 │
         │                 ▼
         │           Gửi commit lên L1
         │                 │
         ▼◄────────────────┘
   L1 lưu commit
   (balance KHÔNG đổi)
         │
         ▼
   Ai muốn verify?
   Replay tx → so hash
```

### 4.3 Verify

| Verify | Dữ liệu cần | Cách làm | Thời gian |
|--------|-------------|----------|-----------|
| L1 state | Toàn bộ L1 blocks | Replay block by block từ genesis | 2-7 ngày |
| L2 batch | Commit (từ L1) + tx list (từ L2) | Replay batch tx → so hash | Vài giây |

**Verify L1** = replay toàn bộ lịch sử, kiểm tra mỗi block:
```
Genesis → Apply Block #1 → stateRoot₁ ✅
       → Apply Block #2 → stateRoot₂ ✅
       → ...
       → Apply Block #N → stateRootₙ ✅
```

**Verify L2** = replay 1 batch, so sánh hash:
```
preStateRoot + 500 tx → replay → tính postStateRoot
→ So với postStateRoot trên L1 commit
✅ Khớp → L2 trung thực
❌ Không khớp → L2 gian lận → gửi fraud proof
```

---

## 5) Triển khai: Cần những máy gì?

```
┌──────────────────────────────────────────────────────────────────┐
│                         NETWORK                                  │
│                                                                  │
│  ══════════════ L1 (nhiều máy) ══════════════                   │
│  ┌────────────┐  ┌────────────┐  ┌────────────┐                │
│  │ L1 Node #1 │  │ L1 Node #2 │  │ L1 Node #3 │                │
│  │ Validator  │  │ Validator  │  │ Validator  │                │
│  └─────┬──────┘  └─────┬──────┘  └─────┬──────┘                │
│        └────────────────┼───────────────┘                        │
│                         │                                        │
│        ┌────────────────┼───────────────┐                        │
│        ▼                ▼               ▼                        │
│  ┌───────────┐   ┌───────────┐   ┌───────────────┐             │
│  │ L2-A      │   │ L2-B      │   │ Máy của bạn   │             │
│  │ Sequencer │   │ Sequencer │   │ (Full Node /   │             │
│  │ (1 máy)   │   │ (1 máy)   │   │  Verifier)    │             │
│  └───────────┘   └───────────┘   └───────────────┘             │
└──────────────────────────────────────────────────────────────────┘
```

| Loại máy | Số lượng | Vai trò | Ai chạy |
|----------|----------|---------|---------|
| **L1 Validator** | Nhiều (1000+) | Tạo block, consensus, lưu state | Cộng đồng |
| **L2 Sequencer** | 1 per L2 | Nhận tx, sắp xếp, apply, gom batch, commit | Đội L2 |
| **Full Node** | Tuỳ | Tải data, replay, verify | Bất kỳ ai |

### Bạn muốn verify? Chạy Full Node:

**Verify L1**: Cài phần mềm L1 node → tự động tải blocks → replay → xác nhận state.

**Verify L2**: Đọc commit từ L1 + tải tx list từ L2 → replay → so sánh postStateRoot.

---

## 6) Tại sao cần L2? (Vấn đề L1 giải quyết không được)

```
L1 Ethereum:
  ~15 tx/giây × 86400 giây/ngày = ~1.3 triệu tx/ngày
  Phí: $5-50/tx

Thế giới cần:
  Visa xử lý ~1700 tx/giây = ~150 triệu tx/ngày
  Phí: gần $0

Gap: L1 chậm 100x, đắt 1000x so với yêu cầu thực tế
```

**L2 giải quyết** bằng cách:
1. Xử lý tx off-chain (nhanh, rẻ)
2. Chỉ gửi tóm tắt lên L1 (tiết kiệm phí)
3. Kế thừa bảo mật từ L1 (nhờ commit + verify)

---

## 7) Trustless — Giá trị cốt lõi

**Hệ thống truyền thống** (ngân hàng):
```
Bạn phải TIN ngân hàng → nếu họ sai, bạn kiện → tốn thời gian
```

**Blockchain + Rollup** (trustless):
```
Bạn KHÔNG CẦN tin ai → tự verify bằng toán học → có bằng chứng ngay
```

> **Trustless** = không cần tin ai cả, vì ai cũng có thể tự kiểm chứng. Toán học là thẩm phán.

---

## 8) Map vào project mini-chain

| Khái niệm | Đã build (M0+M1) | Sẽ build (M4+M5) |
|-----------|-------------------|-------------------|
| L1 State (Account, Balance, Nonce) | ✅ `Ledger.cs` | — |
| Transaction + TxId | ✅ `Transaction.cs` | — |
| Validate (fail-fast 8 bước) | ✅ `TxValidator.cs` | — |
| Apply (debit/credit/nonce) | ✅ `Ledger.Apply()` | — |
| StateRoot (hash of state) | ✅ `Ledger.GetStateRoot()` | — |
| L2 Sequencer | — | 🔲 `L2Executor` |
| Batch & Commit | — | 🔲 `L2Commit` |
| Verify (replay) | — | 🔲 `Verifier` |
| Bridge | ❌ Out of scope | ❌ Out of scope |
