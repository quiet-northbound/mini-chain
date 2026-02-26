using MiniChain.Core.Validation;

namespace MiniChain.Core.Models;

/// <summary>
/// Receipt recording the result of processing a transaction.
/// Both successful and failed transactions generate receipts for traceability.
/// </summary>
public class TxReceipt
{
    public string TxId { get; }
    public TxStatus Status { get; }
    public ValidationError? FailureReason { get; }
    public DateTime AppliedAt { get; }

    /// <summary>Block height this tx was included in. Set later during block commit (M2/M3).</summary>
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

public enum TxStatus
{
    Success,
    Failed
}
