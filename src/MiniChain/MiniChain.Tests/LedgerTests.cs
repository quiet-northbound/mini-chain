using MiniChain.Core.Crypto;
using MiniChain.Core.Models;
using MiniChain.Core.State;
using MiniChain.Core.Validation;

namespace MiniChain.Tests;

/// <summary>
/// 5 mandatory test scenarios per M2_M3 spec + invariant checks.
/// Each test follows: Setup → Create Tx → Sign → Submit → Assert state + receipt.
/// </summary>
public class LedgerTests
{
    private const string ChainId = "mini-chain-v1";

    /// <summary>Helper: create a signed transaction.</summary>
    private static Transaction CreateSignedTx(string from, string to, ulong amount, ulong nonce)
    {
        var tx = new Transaction(from, to, amount, nonce, ChainId);
        tx.Signature = MockSigner.Sign(tx);
        return tx;
    }

    // ──────────────────────────────────────────────
    // Test 1: Happy path — valid transfer
    // ──────────────────────────────────────────────
    [Fact]
    public void Test1_HappyPath_ValidTransfer()
    {
        // Setup: A=100, B=0, A.nonce=0
        var ledger = new Ledger(ChainId);
        ledger.CreateAccount("alice", 100);
        ledger.CreateAccount("bob", 0);

        // Create and sign tx: A→B, amount=10, nonce=0
        var tx = CreateSignedTx("alice", "bob", 10, 0);

        // Submit
        var receipt = ledger.SubmitTransaction(tx);

        // Assert: receipt is Success
        Assert.Equal(TxStatus.Success, receipt.Status);
        Assert.Null(receipt.FailureReason);
        Assert.Equal(tx.TxId, receipt.TxId);

        // Assert: state updated correctly
        var alice = ledger.GetAccount("alice")!;
        var bob = ledger.GetAccount("bob")!;
        Assert.Equal(90UL, alice.Balance);
        Assert.Equal(10UL, bob.Balance);
        Assert.Equal(1UL, alice.Nonce);
        Assert.Equal(0UL, bob.Nonce); // receiver nonce unchanged

        // Assert: txId recorded in seenTxIds
        Assert.Contains(tx.TxId, ledger.SeenTxIds);

        // Assert: receipt in history
        Assert.Single(ledger.TxHistory);

        // Invariant: total balance preserved
        Assert.Equal(100UL, alice.Balance + bob.Balance);
    }

    // ──────────────────────────────────────────────
    // Test 2: Invalid nonce
    // ──────────────────────────────────────────────
    [Fact]
    public void Test2_InvalidNonce()
    {
        // Setup: A.nonce=1 (simulate already sent 1 tx)
        var ledger = new Ledger(ChainId);
        var alice = ledger.CreateAccount("alice", 100);
        alice.Nonce = 1;
        ledger.CreateAccount("bob", 0);

        // Tx with wrong nonce=0 (should be 1)
        var tx = CreateSignedTx("alice", "bob", 10, 0);
        var receipt = ledger.SubmitTransaction(tx);

        // Assert: validation fails with INVALID_NONCE
        Assert.Equal(TxStatus.Failed, receipt.Status);
        Assert.Equal(ValidationError.INVALID_NONCE, receipt.FailureReason);

        // Assert: state unchanged
        Assert.Equal(100UL, ledger.GetAccount("alice")!.Balance);
        Assert.Equal(1UL, ledger.GetAccount("alice")!.Nonce);
        Assert.Equal(0UL, ledger.GetAccount("bob")!.Balance);

        // Assert: txId NOT added to seenTxIds (failed tx)
        Assert.DoesNotContain(tx.TxId, ledger.SeenTxIds);

        // Assert: but receipt IS recorded in history (for traceability)
        Assert.Single(ledger.TxHistory);
    }

    // ──────────────────────────────────────────────
    // Test 3: Insufficient balance
    // ──────────────────────────────────────────────
    [Fact]
    public void Test3_InsufficientBalance()
    {
        // Setup: A.balance=5
        var ledger = new Ledger(ChainId);
        ledger.CreateAccount("alice", 5);
        ledger.CreateAccount("bob", 0);

        // Tx: amount=10 (more than alice has)
        var tx = CreateSignedTx("alice", "bob", 10, 0);
        var receipt = ledger.SubmitTransaction(tx);

        // Assert: fails with INSUFFICIENT_BALANCE
        Assert.Equal(TxStatus.Failed, receipt.Status);
        Assert.Equal(ValidationError.INSUFFICIENT_BALANCE, receipt.FailureReason);

        // Assert: state unchanged
        Assert.Equal(5UL, ledger.GetAccount("alice")!.Balance);
        Assert.Equal(0UL, ledger.GetAccount("alice")!.Nonce);
        Assert.Equal(0UL, ledger.GetAccount("bob")!.Balance);
    }

    // ──────────────────────────────────────────────
    // Test 4: Invalid mock signature
    // ──────────────────────────────────────────────
    [Fact]
    public void Test4_InvalidSignature()
    {
        // Setup: valid accounts
        var ledger = new Ledger(ChainId);
        ledger.CreateAccount("alice", 100);
        ledger.CreateAccount("bob", 0);

        // Create tx with valid fields but TAMPERED signature
        var tx = CreateSignedTx("alice", "bob", 10, 0);
        tx.Signature = "TAMPERED_SIGNATURE";

        var receipt = ledger.SubmitTransaction(tx);

        // Assert: fails with INVALID_SIGNATURE
        Assert.Equal(TxStatus.Failed, receipt.Status);
        Assert.Equal(ValidationError.INVALID_SIGNATURE, receipt.FailureReason);

        // Assert: state unchanged
        Assert.Equal(100UL, ledger.GetAccount("alice")!.Balance);
        Assert.Equal(0UL, ledger.GetAccount("alice")!.Nonce);
        Assert.Equal(0UL, ledger.GetAccount("bob")!.Balance);
    }

    // ──────────────────────────────────────────────
    // Test 5: Replay transaction
    // ──────────────────────────────────────────────
    [Fact]
    public void Test5_ReplayTransaction()
    {
        // Setup
        var ledger = new Ledger(ChainId);
        ledger.CreateAccount("alice", 100);
        ledger.CreateAccount("bob", 0);

        // First tx succeeds
        var tx1 = CreateSignedTx("alice", "bob", 10, 0);
        var receipt1 = ledger.SubmitTransaction(tx1);
        Assert.Equal(TxStatus.Success, receipt1.Status);

        // Replay: exact same tx (same txId since same fields)
        var txReplay = CreateSignedTx("alice", "bob", 10, 0);
        Assert.Equal(tx1.TxId, txReplay.TxId); // same deterministic TxId

        var receipt2 = ledger.SubmitTransaction(txReplay);

        // Assert: fails with REPLAY_TX
        Assert.Equal(TxStatus.Failed, receipt2.Status);
        Assert.Equal(ValidationError.REPLAY_TX, receipt2.FailureReason);

        // Assert: state unchanged after replay attempt
        Assert.Equal(90UL, ledger.GetAccount("alice")!.Balance);
        Assert.Equal(1UL, ledger.GetAccount("alice")!.Nonce);
        Assert.Equal(10UL, ledger.GetAccount("bob")!.Balance);

        // Assert: 2 receipts in history (1 success + 1 failed replay)
        Assert.Equal(2, ledger.TxHistory.Count);
    }

    // ──────────────────────────────────────────────
    // Additional: Multiple sequential transfers
    // ──────────────────────────────────────────────
    [Fact]
    public void Test_MultipleSequentialTransfers_TotalBalancePreserved()
    {
        var ledger = new Ledger(ChainId);
        ledger.CreateAccount("alice", 100);
        ledger.CreateAccount("bob", 50);

        // Tx 1: alice → bob, 20
        var tx1 = CreateSignedTx("alice", "bob", 20, 0);
        Assert.Equal(TxStatus.Success, ledger.SubmitTransaction(tx1).Status);

        // Tx 2: bob → alice, 5
        var tx2 = CreateSignedTx("bob", "alice", 5, 0);
        Assert.Equal(TxStatus.Success, ledger.SubmitTransaction(tx2).Status);

        // Tx 3: alice → bob, 10
        var tx3 = CreateSignedTx("alice", "bob", 10, 1);
        Assert.Equal(TxStatus.Success, ledger.SubmitTransaction(tx3).Status);

        // Assert final balances
        Assert.Equal(75UL, ledger.GetAccount("alice")!.Balance);  // 100 - 20 + 5 - 10
        Assert.Equal(75UL, ledger.GetAccount("bob")!.Balance);    // 50 + 20 - 5 + 10

        // Invariant: total balance preserved (150 = 100 + 50)
        Assert.Equal(150UL, ledger.GetAccount("alice")!.Balance + ledger.GetAccount("bob")!.Balance);

        // Assert: nonces incremented correctly
        Assert.Equal(2UL, ledger.GetAccount("alice")!.Nonce);  // sent 2 txs
        Assert.Equal(1UL, ledger.GetAccount("bob")!.Nonce);    // sent 1 tx

        // Assert: 3 successful receipts
        Assert.Equal(3, ledger.TxHistory.Count);
        Assert.All(ledger.TxHistory, r => Assert.Equal(TxStatus.Success, r.Status));
    }

    // ──────────────────────────────────────────────
    // Additional: State root is deterministic
    // ──────────────────────────────────────────────
    [Fact]
    public void Test_StateRoot_Deterministic()
    {
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

        // Same operations → same state root
        Assert.Equal(ledger1.GetStateRoot(), ledger2.GetStateRoot());

        // State root is a non-empty hex string
        Assert.NotEmpty(ledger1.GetStateRoot());
        Assert.Equal(64, ledger1.GetStateRoot().Length); // SHA-256 = 64 hex chars
    }

    // ──────────────────────────────────────────────
    // Additional: Auto-create receiver account
    // ──────────────────────────────────────────────
    [Fact]
    public void Test_AutoCreateReceiverAccount()
    {
        var ledger = new Ledger(ChainId);
        ledger.CreateAccount("alice", 100);
        // "charlie" does NOT exist yet

        var tx = CreateSignedTx("alice", "charlie", 25, 0);
        var receipt = ledger.SubmitTransaction(tx);

        Assert.Equal(TxStatus.Success, receipt.Status);
        Assert.Equal(75UL, ledger.GetAccount("alice")!.Balance);

        // Charlie auto-created with balance=25
        var charlie = ledger.GetAccount("charlie");
        Assert.NotNull(charlie);
        Assert.Equal(25UL, charlie!.Balance);
        Assert.Equal(0UL, charlie.Nonce);
    }
}
