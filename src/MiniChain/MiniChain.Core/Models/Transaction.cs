using MiniChain.Core.Crypto;
using MiniChain.Core.Serialization;

namespace MiniChain.Core.Models;

/// <summary>
/// A transfer transaction on the mini chain.
/// TxId is computed deterministically from the canonical JSON of payload fields.
/// </summary>
public class Transaction
{
    public string From { get; }
    public string To { get; }
    public ulong Amount { get; }
    public ulong Nonce { get; }
    public string ChainId { get; }
    public string Signature { get; set; } = string.Empty;

    /// <summary>
    /// Deterministic transaction ID = SHA-256(canonical JSON of {amount, chainId, from, nonce, to}).
    /// Sorted alphabetically by key as per DATA_FORMATS.md.
    /// </summary>
    public string TxId { get; }

    /// <summary>Transaction type, always "transfer" in v0.</summary>
    public string Type => "transfer";

    public Transaction(string from, string to, ulong amount, ulong nonce, string chainId)
    {
        From = from.ToLowerInvariant();
        To = to.ToLowerInvariant();
        Amount = amount;
        Nonce = nonce;
        ChainId = chainId;

        // Compute deterministic TxId from canonical payload (sorted keys: amount, chainId, from, nonce, to)
        var payloadJson = CanonicalJson.SerializeTxPayload(this);
        TxId = Hasher.Hash(payloadJson);
    }

    public override string ToString() =>
        $"Tx({From}->{To}, amount={Amount}, nonce={Nonce}, txId={TxId[..8]}...)";
}
