using MiniChain.Core.Crypto;
using MiniChain.Core.Models;
using MiniChain.Core.Serialization;
using MiniChain.Core.Validation;

namespace MiniChain.Core.State;

/// <summary>
/// The Ledger maintains the canonical state of the mini chain:
/// - Account balances and nonces
/// - Transaction history (receipts)
/// - Seen transaction IDs (anti-replay)
/// 
/// It orchestrates: validate → apply → record receipt.
/// </summary>
public class Ledger
{
    private readonly Dictionary<string, Account> _accounts = new();
    private readonly HashSet<string> _seenTxIds = new();
    private readonly List<TxReceipt> _txHistory = new();
    private readonly TxValidator _validator;

    /// <summary>The chain identifier for this ledger.</summary>
    public string ChainId { get; }

    /// <summary>Read-only view of all accounts.</summary>
    public IReadOnlyDictionary<string, Account> Accounts => _accounts;

    /// <summary>Read-only view of all seen transaction IDs.</summary>
    public IReadOnlySet<string> SeenTxIds => _seenTxIds;

    /// <summary>Read-only view of transaction history (receipts).</summary>
    public IReadOnlyList<TxReceipt> TxHistory => _txHistory;

    public Ledger(string chainId = "mini-chain-v1")
    {
        ChainId = chainId;
        _validator = new TxValidator(chainId);
    }

    /// <summary>
    /// Create or initialize an account with the given balance.
    /// If the account already exists, its balance is set to the given value.
    /// </summary>
    public Account CreateAccount(string address, ulong balance = 0)
    {
        var addr = address.ToLowerInvariant();
        if (_accounts.TryGetValue(addr, out var existing))
        {
            existing.Balance = balance;
            return existing;
        }

        var account = new Account(addr, balance);
        _accounts[addr] = account;
        return account;
    }

    /// <summary>
    /// Get an account by address, or null if not found.
    /// </summary>
    public Account? GetAccount(string address)
    {
        _accounts.TryGetValue(address.ToLowerInvariant(), out var account);
        return account;
    }

    /// <summary>
    /// Submit a transaction for validation and execution.
    /// Returns a TxReceipt indicating success or failure.
    /// 
    /// Flow: validate → apply (if valid) → append receipt
    /// Failed transactions also generate a receipt for traceability (skip-invalid strategy).
    /// </summary>
    public TxReceipt SubmitTransaction(Transaction tx)
    {
        // Validate
        var error = _validator.Validate(tx, _accounts, _seenTxIds);

        if (error != null)
        {
            // Failed — record receipt but do NOT modify state
            var failedReceipt = new TxReceipt(tx.TxId, TxStatus.Failed, error);
            _txHistory.Add(failedReceipt);
            return failedReceipt;
        }

        // Apply: debit sender, credit receiver, increment nonce
        Apply(tx);

        // Record success
        _seenTxIds.Add(tx.TxId);
        var receipt = new TxReceipt(tx.TxId, TxStatus.Success);
        _txHistory.Add(receipt);

        return receipt;
    }

    /// <summary>
    /// Apply a validated transaction to state.
    /// - Debit sender balance
    /// - Credit receiver balance (auto-create receiver account if needed)
    /// - Increment sender nonce
    /// </summary>
    private void Apply(Transaction tx)
    {
        var sender = _accounts[tx.From];
        sender.Balance -= tx.Amount;
        sender.Nonce += 1;

        // Auto-create receiver account if it doesn't exist
        if (!_accounts.TryGetValue(tx.To, out var receiver))
        {
            receiver = new Account(tx.To, 0);
            _accounts[tx.To] = receiver;
        }

        receiver.Balance += tx.Amount;
    }

    /// <summary>
    /// Compute the state root: SHA-256 of canonical JSON of sorted state.
    /// Format: {"balances":{"alice":90,"bob":110},"nonces":{"alice":1}}
    /// </summary>
    public string GetStateRoot()
    {
        var stateJson = CanonicalJson.SerializeState(_accounts);
        return Hasher.Hash(stateJson);
    }
}
