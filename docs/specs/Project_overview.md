📌 PROJECT OVERVIEW (SOURCE OF TRUTH)
Project Name

Build-a-Mini-Layer2

1) Purpose (Mục tiêu)

Project này được tạo ra để:

Hiểu bản chất blockchain, Layer 1, Layer 2

Phân biệt rõ coin vs token

Học bằng cách tự tay xây dựng hệ thống chạy được, không chỉ đọc lý thuyết

Lấy lại cảm hứng lập trình và tư duy thiết kế hệ thống

Biến các khái niệm trừu tượng như state, batch, finality, rollup thành luồng xử lý cụ thể

2) Scope (Phạm vi)
✅ In scope
Layer 1 – Mini Base Chain

Block, Transaction

State (balances, nonce)

Apply transaction → update state

Validate transaction (balance, nonce, signature mock)

Lưu chain history (in-memory, có thể ghi file)

Nhận commit / summary từ Layer 2

Token đơn giản (ERC20-like)

Token balance theo account

Transfer

Mint (admin)

❗ Không smart contract phức tạp, không VM

Layer 2 (giả lập)

Nhận transaction từ user

Thực thi off-chain

Gom batch transaction

Tạo batch summary:

batchId

txCount

preStateRoot

postStateRoot

metadata (timestamp, executor, …)

Commit kết quả về Layer 1

🎯 Trọng tâm: logic & flow, không tối ưu hiệu năng.

❌ Out of scope (giai đoạn này)

Mining / consensus phức tạp

Smart contract đầy đủ, VM, EVM

Cryptography chuẩn (chỉ dùng hash đơn giản hoặc mock)

P2P network, mainnet, decentralization thực sự

UI phức tạp

3) Approach (Cách tiếp cận)

Chia nhỏ thành từng bước: hiểu → build → kiểm chứng

Mỗi khái niệm đều phải:

Giải thích được bằng lời

Mô phỏng được bằng code hoặc pseudo-code

Có test scenario để tự kiểm tra

Nguyên tắc ưu tiên:

Hiểu bản chất hơn là đúng chuẩn blockchain thật

Rõ ràng hơn là giống whitepaper

4) High-Level Architecture
Layer 1 (Base Chain)

Lưu block & transaction

Duy trì canonical state

Áp dụng transaction hoặc verify L2 commit (tuỳ stage)

Lưu batch commitment từ Layer 2

Layer 2

Nhận transaction từ user

Execute off-chain

Gom batch

Tạo commitment:

batchId

txCount

preStateRoot

postStateRoot

metadata (timestamp, executor, …)

Submit commitment lên Layer 1

5) Tech & Constraints

Ngôn ngữ: C#, TypeScript, hoặc Python (chọn sau)

Chạy local, single node

Storage: in-memory → file (nếu cần)

Hash: SHA256 hoặc fake hash

Mục tiêu học tập, không dùng cho production

6) Learning Goals (Quan trọng nhất)

Sau project này, mong muốn:

Hiểu rõ:

Blockchain state là gì và thay đổi ra sao

Vì sao Layer 2 tồn tại (throughput, phí, latency)

Coin vs token khác nhau ở đâu (native asset vs asset do logic phát hành)

Rèn luyện:

Tư duy hệ thống

Chia module rõ ràng

Thiết kế có giới hạn và trade-off minh bạch

Lấy lại cảm giác:

“Mình vẫn có thể build được thứ phức tạp.”

7) Working Principles (Nguyên tắc làm việc)

Không vội

Không over-engineer

Mỗi bước phải giải thích lại được cho người khác

Nếu mơ hồ → dừng lại, làm rõ khái niệm trước khi code

8) Status

🚧 Learning Project — phát triển từng bước
📓 Dùng như sổ tay nghiên cứu + nơi thử nghiệm ý tưởng

9) Milestones (Roadmap)

M0: Ledger + accounts + signed transaction (mock signature)

M1: Layer 1 mini-chain + apply tx + validate nonce/balance

M2: Token module (ERC20-like)

M3: Layer 2 executor + mempool + batching

M4: Commit batch summary lên Layer 1 (state root)

M5: Fraud proof / mock proof (tuỳ chọn) + replay verification

📌 Ghi chú

Project này ưu tiên hiểu sâu và build từng phần chạy được, không chạy theo độ phức tạp hay chuẩn production.

📌 Yêu cầu

Luôn bám theo Project Overview này

Nếu có đề xuất vượt scope → phải chỉ rõ và hỏi lại

Ưu tiên giải thích hơn là code ngay