using MiniChain.Core.Crypto;
using MiniChain.Core.Models;
using MiniChain.Core.State;
using MiniChain.Core.Validation;

namespace MiniChain.Tests;

/// <summary>
/// 5 mandatory test scenarios per M2_M3 spec + 3 additional invariant checks.
/// 5 kịch bản test bắt buộc theo spec M2_M3 + 3 kiểm tra bất biến bổ sung.
/// 
/// Each test follows: Setup → Create Tx → Sign → Submit → Assert state + receipt.
/// Mỗi test theo luồng: Khởi tạo → Tạo Tx → Ký → Gửi → Kiểm tra trạng thái + biên nhận.
/// </summary>
public class LedgerTests
{
    private const string ChainId = "mini-chain-v1";

    /// <summary>
    /// Helper: create a signed transaction.
    /// Hàm tiện ích: tạo giao dịch đã ký.
    /// </summary>
    private static Transaction CreateSignedTx(string from, string to, ulong amount, ulong nonce)
    {
        var tx = new Transaction(from, to, amount, nonce, ChainId);
        tx.Signature = MockSigner.Sign(tx);
        return tx;
    }

    // ──────────────────────────────────────────────
    // Test 1: Happy path — valid transfer
    // Test 1: Luồng chính — chuyển tiền hợp lệ
    // ──────────────────────────────────────────────
    [Fact]
    public void Test1_HappyPath_ValidTransfer()
    {
        // Setup: A=100, B=0, A.nonce=0
        // Khởi tạo: Alice có 100 coin, Bob có 0, nonce Alice = 0
        var ledger = new Ledger(ChainId);
        ledger.CreateAccount("alice", 100);
        ledger.CreateAccount("bob", 0);

        // Create and sign tx: A→B, amount=10, nonce=0
        // Tạo và ký giao dịch: Alice gửi Bob 10 coin
        var tx = CreateSignedTx("alice", "bob", 10, 0);

        // Submit / Gửi giao dịch
        var receipt = ledger.SubmitTransaction(tx);

        // Assert: receipt is Success / Biên nhận phải thành công
        Assert.Equal(TxStatus.Success, receipt.Status);
        Assert.Null(receipt.FailureReason);
        Assert.Equal(tx.TxId, receipt.TxId);

        // Assert: state updated correctly / Trạng thái cập nhật đúng
        var alice = ledger.GetAccount("alice")!;
        var bob = ledger.GetAccount("bob")!;
        Assert.Equal(90UL, alice.Balance);    // 100 - 10 = 90
        Assert.Equal(10UL, bob.Balance);      // 0 + 10 = 10
        Assert.Equal(1UL, alice.Nonce);       // nonce tăng 1
        Assert.Equal(0UL, bob.Nonce);         // nonce người nhận không đổi

        // Assert: txId recorded in seenTxIds / TxId được ghi vào danh sách đã thấy
        Assert.Contains(tx.TxId, ledger.SeenTxIds);

        // Assert: receipt in history / Biên nhận nằm trong lịch sử
        Assert.Single(ledger.TxHistory);

        // Invariant: total balance preserved / Bất biến: tổng số dư không đổi
        Assert.Equal(100UL, alice.Balance + bob.Balance);
    }

    // ──────────────────────────────────────────────
    // Test 2: Invalid nonce
    // Test 2: Nonce sai
    // ──────────────────────────────────────────────
    [Fact]
    public void Test2_InvalidNonce()
    {
        // Setup: A.nonce=1 (simulate already sent 1 tx)
        // Khởi tạo: Giả lập Alice đã gửi 1 tx (nonce = 1)
        var ledger = new Ledger(ChainId);
        var alice = ledger.CreateAccount("alice", 100);
        alice.Nonce = 1;
        ledger.CreateAccount("bob", 0);

        // Tx with wrong nonce=0 (should be 1)
        // Giao dịch với nonce sai = 0 (phải là 1)
        var tx = CreateSignedTx("alice", "bob", 10, 0);
        var receipt = ledger.SubmitTransaction(tx);

        // Assert: validation fails with INVALID_NONCE / Kiểm tra thất bại: nonce sai
        Assert.Equal(TxStatus.Failed, receipt.Status);
        Assert.Equal(ValidationError.INVALID_NONCE, receipt.FailureReason);

        // Assert: state unchanged / Trạng thái không đổi
        Assert.Equal(100UL, ledger.GetAccount("alice")!.Balance);
        Assert.Equal(1UL, ledger.GetAccount("alice")!.Nonce);
        Assert.Equal(0UL, ledger.GetAccount("bob")!.Balance);

        // Assert: txId NOT added to seenTxIds (failed tx)
        // TxId KHÔNG được thêm vào danh sách (vì tx thất bại)
        Assert.DoesNotContain(tx.TxId, ledger.SeenTxIds);

        // Assert: but receipt IS recorded in history (for traceability)
        // Nhưng biên nhận VẪN được ghi (để theo dõi)
        Assert.Single(ledger.TxHistory);
    }

    // ──────────────────────────────────────────────
    // Test 3: Insufficient balance
    // Test 3: Thiếu số dư
    // ──────────────────────────────────────────────
    [Fact]
    public void Test3_InsufficientBalance()
    {
        // Setup: A.balance=5 / Khởi tạo: Alice chỉ có 5 coin
        var ledger = new Ledger(ChainId);
        ledger.CreateAccount("alice", 5);
        ledger.CreateAccount("bob", 0);

        // Tx: amount=10 (more than alice has) / Chuyển 10 coin (nhiều hơn Alice có)
        var tx = CreateSignedTx("alice", "bob", 10, 0);
        var receipt = ledger.SubmitTransaction(tx);

        // Assert: fails with INSUFFICIENT_BALANCE / Thất bại: không đủ số dư
        Assert.Equal(TxStatus.Failed, receipt.Status);
        Assert.Equal(ValidationError.INSUFFICIENT_BALANCE, receipt.FailureReason);

        // Assert: state unchanged / Trạng thái không đổi
        Assert.Equal(5UL, ledger.GetAccount("alice")!.Balance);
        Assert.Equal(0UL, ledger.GetAccount("alice")!.Nonce);
        Assert.Equal(0UL, ledger.GetAccount("bob")!.Balance);
    }

    // ──────────────────────────────────────────────
    // Test 4: Invalid mock signature
    // Test 4: Chữ ký giả lập sai
    // ──────────────────────────────────────────────
    [Fact]
    public void Test4_InvalidSignature()
    {
        // Setup: valid accounts / Khởi tạo tài khoản hợp lệ
        var ledger = new Ledger(ChainId);
        ledger.CreateAccount("alice", 100);
        ledger.CreateAccount("bob", 0);

        // Create tx with valid fields but TAMPERED signature
        // Tạo tx đúng fields nhưng chữ ký bị giả mạo
        var tx = CreateSignedTx("alice", "bob", 10, 0);
        tx.Signature = "TAMPERED_SIGNATURE";

        var receipt = ledger.SubmitTransaction(tx);

        // Assert: fails with INVALID_SIGNATURE / Thất bại: chữ ký không hợp lệ
        Assert.Equal(TxStatus.Failed, receipt.Status);
        Assert.Equal(ValidationError.INVALID_SIGNATURE, receipt.FailureReason);

        // Assert: state unchanged / Trạng thái không đổi
        Assert.Equal(100UL, ledger.GetAccount("alice")!.Balance);
        Assert.Equal(0UL, ledger.GetAccount("alice")!.Nonce);
        Assert.Equal(0UL, ledger.GetAccount("bob")!.Balance);
    }

    // ──────────────────────────────────────────────
    // Test 5: Replay transaction
    // Test 5: Phát lại giao dịch (replay attack)
    // ──────────────────────────────────────────────
    [Fact]
    public void Test5_ReplayTransaction()
    {
        // Setup / Khởi tạo
        var ledger = new Ledger(ChainId);
        ledger.CreateAccount("alice", 100);
        ledger.CreateAccount("bob", 0);

        // First tx succeeds / Giao dịch đầu tiên thành công
        var tx1 = CreateSignedTx("alice", "bob", 10, 0);
        var receipt1 = ledger.SubmitTransaction(tx1);
        Assert.Equal(TxStatus.Success, receipt1.Status);

        // Replay: exact same tx (same txId since same fields)
        // Phát lại: giao dịch y hệt (cùng TxId vì cùng nội dung)
        var txReplay = CreateSignedTx("alice", "bob", 10, 0);
        Assert.Equal(tx1.TxId, txReplay.TxId); // same deterministic TxId / cùng TxId tất định

        var receipt2 = ledger.SubmitTransaction(txReplay);

        // Assert: fails with REPLAY_TX / Thất bại: giao dịch đã được phát lại
        Assert.Equal(TxStatus.Failed, receipt2.Status);
        Assert.Equal(ValidationError.REPLAY_TX, receipt2.FailureReason);

        // Assert: state unchanged after replay attempt / Trạng thái không đổi sau replay
        Assert.Equal(90UL, ledger.GetAccount("alice")!.Balance);
        Assert.Equal(1UL, ledger.GetAccount("alice")!.Nonce);
        Assert.Equal(10UL, ledger.GetAccount("bob")!.Balance);

        // Assert: 2 receipts in history (1 success + 1 failed replay)
        // 2 biên nhận trong lịch sử (1 thành công + 1 replay thất bại)
        Assert.Equal(2, ledger.TxHistory.Count);
    }

    // ──────────────────────────────────────────────
    // Additional: Multiple sequential transfers
    // Bổ sung: Nhiều giao dịch liên tiếp
    // ──────────────────────────────────────────────
    [Fact]
    public void Test_MultipleSequentialTransfers_TotalBalancePreserved()
    {
        var ledger = new Ledger(ChainId);
        ledger.CreateAccount("alice", 100);
        ledger.CreateAccount("bob", 50);

        // Tx 1: alice → bob, 20 coin
        var tx1 = CreateSignedTx("alice", "bob", 20, 0);
        Assert.Equal(TxStatus.Success, ledger.SubmitTransaction(tx1).Status);

        // Tx 2: bob → alice, 5 coin (bob gửi ngược lại)
        var tx2 = CreateSignedTx("bob", "alice", 5, 0);
        Assert.Equal(TxStatus.Success, ledger.SubmitTransaction(tx2).Status);

        // Tx 3: alice → bob, 10 coin (alice gửi lần 2, nonce=1)
        var tx3 = CreateSignedTx("alice", "bob", 10, 1);
        Assert.Equal(TxStatus.Success, ledger.SubmitTransaction(tx3).Status);

        // Assert final balances / Kiểm tra số dư cuối
        Assert.Equal(75UL, ledger.GetAccount("alice")!.Balance);  // 100 - 20 + 5 - 10 = 75
        Assert.Equal(75UL, ledger.GetAccount("bob")!.Balance);    // 50 + 20 - 5 + 10 = 75

        // Invariant: total balance preserved (150 = 100 + 50)
        // Bất biến: tổng số dư bảo toàn (150 = 100 + 50)
        Assert.Equal(150UL, ledger.GetAccount("alice")!.Balance + ledger.GetAccount("bob")!.Balance);

        // Assert: nonces incremented correctly / Nonce tăng đúng
        Assert.Equal(2UL, ledger.GetAccount("alice")!.Nonce);  // sent 2 txs / gửi 2 giao dịch
        Assert.Equal(1UL, ledger.GetAccount("bob")!.Nonce);    // sent 1 tx / gửi 1 giao dịch

        // Assert: 3 successful receipts / 3 biên nhận thành công
        Assert.Equal(3, ledger.TxHistory.Count);
        Assert.All(ledger.TxHistory, r => Assert.Equal(TxStatus.Success, r.Status));
    }

    // ──────────────────────────────────────────────
    // Additional: State root is deterministic
    // Bổ sung: State root phải tất định
    // ──────────────────────────────────────────────
    [Fact]
    public void Test_StateRoot_Deterministic()
    {
        // Tạo 2 ledger giống hệt nhau, thực hiện cùng thao tác
        // Create 2 identical ledgers, perform same operations
        var ledger1 = new Ledger(ChainId);
        ledger1.CreateAccount("alice", 100);
        ledger1.CreateAccount("bob", 0);
        var tx1 = CreateSignedTx("alice", "bob", 10, 0);
        ledger1.SubmitTransaction(tx1);

        var ledger2 = new Ledger(ChainId);
        ledger2.CreateAccount("alice", 100);
        ledger2.CreateAccount("bob", 0);
        var tx2 = CreateSignedTx("alice", "bob", 10, 0);
        ledger2.SubmitTransaction(tx2);

        // Same operations → same state root / Cùng thao tác → cùng state root
        Assert.Equal(ledger1.GetStateRoot(), ledger2.GetStateRoot());

        // State root is a non-empty hex string (64 chars = SHA-256)
        // State root là chuỗi hex không rỗng (64 ký tự = SHA-256)
        Assert.NotEmpty(ledger1.GetStateRoot());
        Assert.Equal(64, ledger1.GetStateRoot().Length);
    }

    // ──────────────────────────────────────────────
    // Additional: Auto-create receiver account
    // Bổ sung: Tự động tạo tài khoản người nhận
    // ──────────────────────────────────────────────
    [Fact]
    public void Test_AutoCreateReceiverAccount()
    {
        var ledger = new Ledger(ChainId);
        ledger.CreateAccount("alice", 100);
        // "charlie" does NOT exist yet / "charlie" chưa tồn tại

        var tx = CreateSignedTx("alice", "charlie", 25, 0);
        var receipt = ledger.SubmitTransaction(tx);

        Assert.Equal(TxStatus.Success, receipt.Status);
        Assert.Equal(75UL, ledger.GetAccount("alice")!.Balance);

        // Charlie auto-created with balance=25 / Charlie được tự tạo với số dư 25
        var charlie = ledger.GetAccount("charlie");
        Assert.NotNull(charlie);
        Assert.Equal(25UL, charlie!.Balance);
        Assert.Equal(0UL, charlie.Nonce);
    }
}
