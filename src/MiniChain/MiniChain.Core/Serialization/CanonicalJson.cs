using System.Text.Json;

namespace MiniChain.Core.Serialization;

/// <summary>
/// Canonical JSON serializer per DATA_FORMATS.md:
/// - Keys sorted alphabetically at every nesting level
/// - Integers only (no floats)
/// - UTF-8, compact form (no extra whitespace)
/// - No null values
/// </summary>
public static class CanonicalJson
{
    /// <summary>
    /// Serializes a Transaction's payload fields into canonical JSON for TxId derivation.
    /// Sorted keys: amount, chainId, from, nonce, to
    /// </summary>
    public static string SerializeTxPayload(Models.Transaction tx)
    {
        // Manual construction ensures deterministic key order
        // Keys sorted: amount, chainId, from, nonce, to
        return $"{{\"amount\":{tx.Amount},\"chainId\":\"{tx.ChainId}\",\"from\":\"{tx.From}\",\"nonce\":{tx.Nonce},\"to\":\"{tx.To}\"}}";
    }

    /// <summary>
    /// Serializes the state (balances + nonces) into canonical JSON for state root derivation.
    /// Format per DATA_FORMATS.md:
    /// {"balances":{"alice":90,"bob":110},"nonces":{"alice":1}}
    /// - "balances" before "nonces" (alphabetical)
    /// - Within each, addresses sorted alphabetically  
    /// - Addresses with zero balance/nonce may be omitted
    /// </summary>
    public static string SerializeState(IReadOnlyDictionary<string, Models.Account> accounts)
    {
        var sortedAddresses = accounts.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList();

        // Build balances object (omit zero balances)
        var balanceParts = new List<string>();
        foreach (var addr in sortedAddresses)
        {
            var bal = accounts[addr].Balance;
            if (bal > 0)
                balanceParts.Add($"\"{addr}\":{bal}");
        }

        // Build nonces object (omit zero nonces)
        var nonceParts = new List<string>();
        foreach (var addr in sortedAddresses)
        {
            var nonce = accounts[addr].Nonce;
            if (nonce > 0)
                nonceParts.Add($"\"{addr}\":{nonce}");
        }

        var balancesJson = "{" + string.Join(",", balanceParts) + "}";
        var noncesJson = "{" + string.Join(",", nonceParts) + "}";

        return $"{{\"balances\":{balancesJson},\"nonces\":{noncesJson}}}";
    }
}
