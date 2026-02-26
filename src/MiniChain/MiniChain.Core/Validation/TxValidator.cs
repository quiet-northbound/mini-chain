using MiniChain.Core.Crypto;
using MiniChain.Core.Models;

namespace MiniChain.Core.Validation;

/// <summary>
/// Validates transactions in fail-fast order per M4_M5 spec:
/// 1. ChainId matches
/// 2. Signature valid (mock)
/// 3. Replay check (txId not seen)
/// 4. Amount > 0 and from != to
/// 5. Sender account exists
/// 6. Nonce matches sender's current nonce
/// 7. Sufficient balance
/// </summary>
public class TxValidator
{
    private readonly string _chainId;

    public TxValidator(string chainId)
    {
        _chainId = chainId;
    }

    /// <summary>
    /// Validate a transaction against current state.
    /// Returns null if valid, or the first ValidationError encountered (fail-fast).
    /// </summary>
    public ValidationError? Validate(
        Transaction tx,
        IReadOnlyDictionary<string, Account> accounts,
        IReadOnlySet<string> seenTxIds)
    {
        // 1. ChainId
        if (!string.Equals(tx.ChainId, _chainId, StringComparison.Ordinal))
            return ValidationError.INVALID_CHAIN_ID;

        // 2. Signature (mock verify)
        if (!MockSigner.Verify(tx))
            return ValidationError.INVALID_SIGNATURE;

        // 3. Replay check
        if (seenTxIds.Contains(tx.TxId))
            return ValidationError.REPLAY_TX;

        // 4. Amount > 0
        if (tx.Amount == 0)
            return ValidationError.INVALID_AMOUNT;

        // 5. from != to
        if (string.Equals(tx.From, tx.To, StringComparison.Ordinal))
            return ValidationError.SELF_TRANSFER;

        // 6. Sender account exists
        if (!accounts.TryGetValue(tx.From, out var sender))
            return ValidationError.ACCOUNT_NOT_FOUND;

        // 7. Nonce matches
        if (tx.Nonce != sender.Nonce)
            return ValidationError.INVALID_NONCE;

        // 8. Sufficient balance
        if (sender.Balance < tx.Amount)
            return ValidationError.INSUFFICIENT_BALANCE;

        return null; // Valid
    }
}
