namespace MiniChain.Core.Models;

/// <summary>
/// Represents an account on the mini chain.
/// Đại diện cho một tài khoản trên mini chain.
/// 
/// Each account has: address (lowercase), balance (native coin), and nonce.
/// Mỗi tài khoản gồm: địa chỉ (chữ thường), số dư (coin gốc), và nonce.
/// </summary>
public class Account
{
    /// <summary>
    /// Unique lowercase identifier for this account.
    /// Định danh duy nhất của tài khoản (chữ thường).
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Native coin balance. Always >= 0.
    /// Số dư coin gốc. Luôn >= 0.
    /// </summary>
    public ulong Balance { get; internal set; }

    /// <summary>
    /// Next valid nonce for outgoing transactions.
    /// Incremented by 1 after each successful tx from this account.
    /// 
    /// Nonce hợp lệ tiếp theo cho giao dịch gửi đi.
    /// Tăng thêm 1 sau mỗi giao dịch thành công từ tài khoản này.
    /// </summary>
    public ulong Nonce { get; internal set; }

    public Account(string address, ulong balance = 0, ulong nonce = 0)
    {
        Address = address.ToLowerInvariant();
        Balance = balance;
        Nonce = nonce;
    }

    /// <summary>
    /// Creates a deep copy for snapshotting.
    /// Tạo bản sao sâu để chụp trạng thái (snapshot).
    /// </summary>
    public Account Clone() => new(Address, Balance, Nonce);

    public override string ToString() => $"Account({Address}, balance={Balance}, nonce={Nonce})";
}
