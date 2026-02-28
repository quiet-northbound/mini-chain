using MiniChain.Core.Models;

namespace MiniChain.Core.Crypto;

/// <summary>
/// Mock signature implementation per M4_M5 spec.
/// Triển khai chữ ký giả lập theo spec M4_M5.
/// 
/// Format: signature = "SIG(" + from + "|" + to + "|" + amount + "|" + nonce + "|" + chainId + ")"
/// 
/// This is NOT real cryptography — it's a deterministic mock that allows
/// testing the signature verification flow without actual key pairs.
/// 
/// Đây KHÔNG phải mật mã thật — chỉ là mock tất định để cho phép
/// kiểm thử luồng xác thực chữ ký mà không cần cặp khóa thật.
/// </summary>
public static class MockSigner
{
    /// <summary>
    /// Generate a mock signature for a transaction.
    /// Tạo chữ ký giả lập cho một giao dịch.
    /// 
    /// Example / Ví dụ: "SIG(alice|bob|10|0|mini-chain-v1)"
    /// </summary>
    public static string Sign(Transaction tx)
    {
        return $"SIG({tx.From}|{tx.To}|{tx.Amount}|{tx.Nonce}|{tx.ChainId})";
    }

    /// <summary>
    /// Verify that a transaction's signature matches the expected mock signature.
    /// Xác minh chữ ký giao dịch có khớp với chữ ký mock kỳ vọng hay không.
    /// 
    /// Returns true if valid, false if tampered.
    /// Trả về true nếu hợp lệ, false nếu bị giả mạo.
    /// </summary>
    public static bool Verify(Transaction tx)
    {
        var expected = Sign(tx);
        return string.Equals(tx.Signature, expected, StringComparison.Ordinal);
    }
}
