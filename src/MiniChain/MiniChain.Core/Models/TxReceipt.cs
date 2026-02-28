using MiniChain.Core.Validation;

namespace MiniChain.Core.Models;

/// <summary>
/// Receipt recording the result of processing a transaction.
/// Biên nhận ghi lại kết quả xử lý một giao dịch.
/// 
/// Both successful and failed transactions generate receipts for traceability.
/// Cả giao dịch thành công và thất bại đều tạo biên nhận để theo dõi.
/// </summary>
public class TxReceipt
{
    /// <summary>Transaction ID / ID giao dịch</summary>
    public string TxId { get; }

    /// <summary>Success or Failed / Thành công hoặc Thất bại</summary>
    public TxStatus Status { get; }

    /// <summary>
    /// Reason for failure (null if success).
    /// Lý do thất bại (null nếu thành công).
    /// </summary>
    public ValidationError? FailureReason { get; }

    /// <summary>Timestamp when tx was processed / Thời điểm giao dịch được xử lý</summary>
    public DateTime AppliedAt { get; }

    /// <summary>
    /// Block height this tx was included in. Set later during block commit (M2/M3).
    /// Chiều cao block chứa giao dịch này. Được gán sau khi commit block (M2/M3).
    /// </summary>
    public ulong? BlockHeight { get; set; }

    public TxReceipt(string txId, TxStatus status, ValidationError? failureReason = null)
    {
        TxId = txId;
        Status = status;
        FailureReason = failureReason;
        AppliedAt = DateTime.UtcNow;
    }

    public override string ToString() =>
        Status == TxStatus.Success
            ? $"Receipt({TxId[..8]}..., SUCCESS)"
            : $"Receipt({TxId[..8]}..., FAILED: {FailureReason})";
}

/// <summary>
/// Transaction processing result status.
/// Trạng thái kết quả xử lý giao dịch.
/// </summary>
public enum TxStatus
{
    /// <summary>Transaction applied successfully / Giao dịch được áp dụng thành công</summary>
    Success,

    /// <summary>Transaction rejected by validation / Giao dịch bị từ chối bởi kiểm tra</summary>
    Failed
}
