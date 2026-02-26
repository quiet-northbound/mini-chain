namespace MiniChain.Core.Validation;

/// <summary>
/// Error codes for transaction validation failures.
/// Ordered by validation priority (fail-fast sequence).
/// </summary>
public enum ValidationError
{
    /// <summary>ChainId in tx does not match ledger's chainId.</summary>
    INVALID_CHAIN_ID,

    /// <summary>Mock signature verification failed.</summary>
    INVALID_SIGNATURE,

    /// <summary>Transaction with this TxId was already applied (anti-replay).</summary>
    REPLAY_TX,

    /// <summary>Transfer amount must be > 0.</summary>
    INVALID_AMOUNT,

    /// <summary>Cannot transfer to self (from == to).</summary>
    SELF_TRANSFER,

    /// <summary>Sender account does not exist in state.</summary>
    ACCOUNT_NOT_FOUND,

    /// <summary>Transaction nonce does not match sender's current nonce.</summary>
    INVALID_NONCE,

    /// <summary>Sender does not have sufficient balance for the transfer.</summary>
    INSUFFICIENT_BALANCE
}
