using MiniChain.Core.Models;

namespace MiniChain.Core.Crypto;

/// <summary>
/// Mock signature implementation per M4_M5 spec:
/// signature = "SIG(" + from + "|" + to + "|" + amount + "|" + nonce + "|" + chainId + ")"
/// 
/// This is NOT real cryptography — it's a deterministic mock that allows
/// testing the signature verification flow without actual key pairs.
/// </summary>
public static class MockSigner
{
    /// <summary>
    /// Generate a mock signature for a transaction.
    /// </summary>
    public static string Sign(Transaction tx)
    {
        return $"SIG({tx.From}|{tx.To}|{tx.Amount}|{tx.Nonce}|{tx.ChainId})";
    }

    /// <summary>
    /// Verify that a transaction's signature matches the expected mock signature.
    /// </summary>
    public static bool Verify(Transaction tx)
    {
        var expected = Sign(tx);
        return string.Equals(tx.Signature, expected, StringComparison.Ordinal);
    }
}
