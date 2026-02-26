# Data Formats — MINI-LAYER2
# Định dạng dữ liệu — MINI-LAYER2

## Purpose | Mục đích
Central place to define canonical formats to avoid nondeterminism.
*Nơi tập trung định nghĩa định dạng chuẩn tắc để tránh tính không tất định.*

## Canonical Encoding | Mã hóa chuẩn tắc
**✅ DECIDED: Canonical JSON** — ref: M0 DECISION-3

Rules *(quy tắc)*:
| Rule / Quy tắc | Detail / Chi tiết |
|----------------|-------------------|
| Key ordering *(thứ tự key)* | Sorted alphabetically at every nesting level *(sắp xếp alphabet ở mọi cấp)* |
| Numbers *(số)* | Integers only, no floats, no scientific notation *(chỉ số nguyên, không số thực, không ký hiệu khoa học)* |
| Strings *(chuỗi)* | UTF-8, no BOM, lowercase for addresses |
| Whitespace *(khoảng trắng)* | No extra whitespace — compact form *(không khoảng trắng thừa — dạng nén)* |
| Encoding | UTF-8 |
| Null handling | No null values — omit key if no value *(không dùng null — bỏ key nếu không có giá trị)* |

## Transaction shape | Hình dạng giao dịch
```json
{
  "amount": 10,
  "from": "alice",
  "nonce": 0,
  "to": "bob",
  "type": "transfer"
}
```
> Keys sorted: `amount`, `from`, `nonce`, `to`, `type`

| Field | Type | Constraints / Ràng buộc |
|-------|------|------------------------|
| `type` | string | `"transfer"` (v0 chỉ có 1 loại) |
| `from` | string | lowercase, non-empty *(chữ thường, không rỗng)* |
| `to` | string | lowercase, non-empty |
| `amount` | integer | `> 0` |
| `nonce` | integer | `>= 0` |

## Batch shape | Hình dạng lô
```json
{
  "batch_id": 1,
  "metadata": {},
  "prev_state_root": "abc123...",
  "txs": [...]
}
```

| Field | Type | Constraints / Ràng buộc |
|-------|------|------------------------|
| `batch_id` | integer | `>= 0`, auto-increment |
| `prev_state_root` | string | hex-encoded SHA-256 hash |
| `txs` | array | ordered list of transactions *(danh sách giao dịch có thứ tự)* |
| `metadata` | object | reserved for future use *(để dành cho tương lai)* |

## State shape (for state root derivation) | Hình dạng trạng thái (để tính gốc)
**✅ ref: M0 DECISION-2**
```json
{"balances":{"alice":90,"bob":110},"nonces":{"alice":1}}
```
- `balances` before `nonces` (alphabetical)
- Within each, addresses sorted alphabetically
- Addresses with zero balance/nonce **may be omitted** *(có thể bỏ qua)*

State root = `SHA-256(canonical_json_string_above)`

## Determinism constraints | Ràng buộc tính tất định
- tx order is authoritative *(thứ tự giao dịch là chính thức)*
- stable serialization rules *(quy tắc serialize ổn định)*
- explicit integer bounds *(giới hạn số nguyên tường minh)*: values fit in 64-bit unsigned integer

## Resolved Decisions | Quyết định đã giải quyết
- ~~address representation~~: ✅ lowercase string
- ~~hash function selection~~: ✅ SHA-256 (prototype)

## Changelog | Nhật ký thay đổi
- v0: initial draft
- v1: finalized — canonical JSON with concrete rules, field constraints, state shape for root derivation
