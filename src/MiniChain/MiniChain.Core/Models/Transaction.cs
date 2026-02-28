using MiniChain.Core.Crypto;
using MiniChain.Core.Serialization;

namespace MiniChain.Core.Models;

/// <summary>
/// A transfer transaction on the mini chain.
/// Một giao dịch chuyển tiền trên mini chain.
/// 
/// TxId is computed deterministically from the canonical JSON of payload fields.
/// TxId được tính toán tất định từ canonical JSON của các trường payload.
/// </summary>
public class Transaction
{
    /// <summary>Sender address / Địa chỉ người gửi</summary>
    public string From { get; }

    /// <summary>Receiver address / Địa chỉ người nhận</summary>
    public string To { get; }

    /// <summary>Transfer amount (must be > 0) / Số tiền chuyển (phải > 0)</summary>
    public ulong Amount { get; }

    /// <summary>
    /// Sender's expected nonce. Must match account's current nonce.
    /// Nonce kỳ vọng của người gửi. Phải khớp với nonce hiện tại của tài khoản.
    /// </summary>
    public ulong Nonce { get; }

    /// <summary>
    /// Chain identifier to prevent cross-chain replay.
    /// Định danh chuỗi để ngăn replay xuyên chuỗi.
    /// </summary>
    public string ChainId { get; }

    /// <summary>
    /// Mock signature for verification.
    /// Chữ ký giả lập để xác thực.
    /// </summary>
    public string Signature { get; set; } = string.Empty;

    /// <summary>
    /// Deterministic transaction ID = SHA-256(canonical JSON of {amount, chainId, from, nonce, to}).
    /// Sorted alphabetically by key as per DATA_FORMATS.md.
    /// 
    /// ID giao dịch tất định = SHA-256(canonical JSON với key sắp xếp alphabet).
    /// Dùng để phát hiện giao dịch trùng lặp (anti-replay).
    /// </summary>
    public string TxId { get; }

    /// <summary>
    /// Transaction type, always "transfer" in v0.
    /// Loại giao dịch, luôn là "transfer" trong phiên bản v0.
    /// </summary>
    public string Type => "transfer";

    public Transaction(string from, string to, ulong amount, ulong nonce, string chainId)
    {
        From = from.ToLowerInvariant();
        To = to.ToLowerInvariant();
        Amount = amount;
        Nonce = nonce;
        ChainId = chainId;

        // Compute deterministic TxId from canonical payload (sorted keys: amount, chainId, from, nonce, to)
        // Tính TxId tất định từ canonical payload (key sắp xếp: amount, chainId, from, nonce, to)
        var payloadJson = CanonicalJson.SerializeTxPayload(this);
        TxId = Hasher.Hash(payloadJson);
    }

    public override string ToString() =>
        $"Tx({From}->{To}, amount={Amount}, nonce={Nonce}, txId={TxId[..8]}...)";
}
