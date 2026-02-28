# 📓 Learning Tracker — Build-a-Mini-Layer2

## Giai đoạn 1: Bức tranh tổng thể
> Mục tiêu: Giải thích được blockchain state, nonce, determinism bằng lời của mình.

- [ ] Đọc `docs/knowledge/BLOCKCHAIN_PRIMER.md`
- [ ] Đọc `docs/knowledge/ROLLUP_FLOW_OVERVIEW.md`
- [ ] Đọc `docs/specs/Project_overview.md`
- [ ] ✍️ Ghi lại: Blockchain state là gì?
- [ ] ✍️ Ghi lại: Tại sao cần nonce?
- [ ] ✍️ Ghi lại: Determinism quan trọng ra sao?

---

## Giai đoạn 2: Đọc code (bottom-up)
> Mục tiêu: Hiểu từng module, giải thích được luồng xử lý SubmitTransaction.

### Models (Dữ liệu nền tảng)
- [ ] `Models/Account.cs` — Balance và Nonce đại diện cho gì? Tại sao setter là `internal`?
- [ ] `Models/Transaction.cs` — TxId tính thế nào? Tại sao dùng canonical JSON?
- [ ] `Models/TxReceipt.cs` — Tại sao giao dịch thất bại cũng có receipt?

### Crypto (Mật mã)
- [ ] `Crypto/Hasher.cs` — SHA-256 nhận gì, trả gì? Tại sao output hex lowercase?
- [ ] `Crypto/MockSigner.cs` — Mock signature khác crypto thật ở đâu? Tại sao vẫn cần?

### Serialization (Tuần tự hóa)
- [ ] `Serialization/CanonicalJson.cs` — Tại sao phải sort key? Không sort thì hậu quả gì?

### Validation (Kiểm tra hợp lệ)
- [ ] `Validation/ValidationError.cs` — 8 loại lỗi, thứ tự có ý nghĩa gì?
- [ ] `Validation/TxValidator.cs` — "Fail-fast" là gì? Tại sao check ChainId trước Balance?

### State (Trạng thái)
- [ ] `State/Ledger.cs` — Vẽ lại luồng `SubmitTransaction()` trên giấy
- [ ] ✍️ Ghi lại: State root dùng để làm gì?

---

## Giai đoạn 3: Thí nghiệm 🧪
> Mục tiêu: Phá code → xem test fail → hiểu tại sao rule đó tồn tại.

- [ ] Chạy `dotnet test` lần đầu — tất cả 8 test pass
- [ ] TN1: Comment nonce check → test nào fail? Tại sao?
- [ ] TN2: Comment replay check → chuyện gì xảy ra?
- [ ] TN3: Đảo thứ tự key trong CanonicalJson → StateRoot test fail?
- [ ] TN4: Bỏ `sender.Nonce += 1` → bao nhiêu test fail?
- [ ] TN5: Tạo test self-transfer `alice → alice` → lỗi gì?
- [ ] ⚠️ Git revert sau mỗi thí nghiệm!

---

## Giai đoạn 4: Kiến trúc sâu
> Mục tiêu: Hiểu lý do thiết kế, chuẩn bị cho milestone tiếp theo.

- [ ] Đọc `docs/knowledge/STATE_TRANSITION_MODEL.md`
- [ ] Đọc `docs/architecture/DATA_FORMATS.md`
- [ ] Đọc `docs/architecture/COMPONENTS.md`
- [ ] Preview `docs/specs/M2_M3.md` — Block & Chain History sẽ thêm gì?
- [ ] ✍️ Ghi lại: STF (State Transition Function) hoạt động ra sao?

---

## Ghi chú cá nhân
> Ghi lại những gì bạn học được, thắc mắc, hoặc aha-moments.

### State & Transaction


### Nonce & Replay Protection


### Determinism & Canonical JSON


### Câu hỏi chưa giải đáp


---

## Milestone Progress

| Milestone | Status | Ngày hoàn thành |
|-----------|--------|-----------------|
| M0 + M1 (Ledger Foundation) | ✅ Code done, đang học | |
| M2 + M3 (Block & Chain History) | 🔲 Chưa bắt đầu | |
| M4 + M5 (Layer2 + Commit) | 🔲 Chưa bắt đầu | |
