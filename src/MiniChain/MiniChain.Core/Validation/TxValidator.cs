using MiniChain.Core.Crypto;
using MiniChain.Core.Models;

namespace MiniChain.Core.Validation;

/// <summary>
/// Validates transactions in fail-fast order per M4_M5 spec.
/// Kiểm tra tính hợp lệ của giao dịch theo thứ tự fail-fast (dừng ngay lỗi đầu tiên).
/// 
/// Validation order / Thứ tự kiểm tra:
/// 1. ChainId matches / ChainId khớp
/// 2. Signature valid (mock) / Chữ ký hợp lệ (giả lập)
/// 3. Replay check (txId not seen) / Kiểm tra phát lại (txId chưa thấy)
/// 4. Amount > 0 and from != to / Số tiền > 0 và không tự chuyển
/// 5. Sender account exists / Tài khoản người gửi tồn tại
/// 6. Nonce matches sender's current nonce / Nonce khớp với nonce hiện tại
/// 7. Sufficient balance / Đủ số dư
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
    /// 
    /// Kiểm tra giao dịch với trạng thái hiện tại.
    /// Trả về null nếu hợp lệ, hoặc lỗi đầu tiên gặp phải (fail-fast).
    /// </summary>
    public ValidationError? Validate(
        Transaction tx,
        IReadOnlyDictionary<string, Account> accounts,
        IReadOnlySet<string> seenTxIds)
    {
        // 1. ChainId — giao dịch phải thuộc đúng chain này
        if (!string.Equals(tx.ChainId, _chainId, StringComparison.Ordinal))
            return ValidationError.INVALID_CHAIN_ID;

        // 2. Signature (mock verify) — chữ ký phải khớp format kỳ vọng
        if (!MockSigner.Verify(tx))
            return ValidationError.INVALID_SIGNATURE;

        // 3. Replay check — giao dịch này đã được xử lý trước đó chưa?
        if (seenTxIds.Contains(tx.TxId))
            return ValidationError.REPLAY_TX;

        // 4. Amount > 0 — số tiền chuyển phải dương
        if (tx.Amount == 0)
            return ValidationError.INVALID_AMOUNT;

        // 5. from != to — không được tự chuyển cho mình
        if (string.Equals(tx.From, tx.To, StringComparison.Ordinal))
            return ValidationError.SELF_TRANSFER;

        // 6. Sender account exists — tài khoản người gửi phải tồn tại
        if (!accounts.TryGetValue(tx.From, out var sender))
            return ValidationError.ACCOUNT_NOT_FOUND;

        // 7. Nonce matches — nonce phải khớp (đảm bảo thứ tự giao dịch)
        if (tx.Nonce != sender.Nonce)
            return ValidationError.INVALID_NONCE;

        // 8. Sufficient balance — số dư phải đủ để chuyển
        if (sender.Balance < tx.Amount)
            return ValidationError.INSUFFICIENT_BALANCE;

        return null; // Valid / Hợp lệ
    }
}
