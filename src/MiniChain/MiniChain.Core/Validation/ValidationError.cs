namespace MiniChain.Core.Validation;

/// <summary>
/// Error codes for transaction validation failures.
/// Mã lỗi cho các trường hợp kiểm tra giao dịch thất bại.
/// 
/// Ordered by validation priority (fail-fast sequence).
/// Sắp xếp theo thứ tự ưu tiên kiểm tra (dừng ngay khi gặp lỗi đầu tiên).
/// </summary>
public enum ValidationError
{
    /// <summary>
    /// ChainId in tx does not match ledger's chainId.
    /// ChainId trong giao dịch không khớp với chainId của sổ cái.
    /// </summary>
    INVALID_CHAIN_ID,

    /// <summary>
    /// Mock signature verification failed.
    /// Xác thực chữ ký giả lập thất bại.
    /// </summary>
    INVALID_SIGNATURE,

    /// <summary>
    /// Transaction with this TxId was already applied (anti-replay).
    /// Giao dịch với TxId này đã được áp dụng trước đó (chống phát lại).
    /// </summary>
    REPLAY_TX,

    /// <summary>
    /// Transfer amount must be > 0.
    /// Số tiền chuyển phải > 0.
    /// </summary>
    INVALID_AMOUNT,

    /// <summary>
    /// Cannot transfer to self (from == to).
    /// Không thể chuyển tiền cho chính mình (from == to).
    /// </summary>
    SELF_TRANSFER,

    /// <summary>
    /// Sender account does not exist in state.
    /// Tài khoản người gửi không tồn tại trong trạng thái.
    /// </summary>
    ACCOUNT_NOT_FOUND,

    /// <summary>
    /// Transaction nonce does not match sender's current nonce.
    /// Nonce giao dịch không khớp với nonce hiện tại của người gửi.
    /// </summary>
    INVALID_NONCE,

    /// <summary>
    /// Sender does not have sufficient balance for the transfer.
    /// Người gửi không có đủ số dư để thực hiện giao dịch.
    /// </summary>
    INSUFFICIENT_BALANCE
}
