using MiniChain.Core.Crypto;
using MiniChain.Core.Models;
using MiniChain.Core.Serialization;
using MiniChain.Core.Validation;

namespace MiniChain.Core.State;

/// <summary>
/// The Ledger maintains the canonical state of the mini chain.
/// Sổ cái duy trì trạng thái chuẩn tắc của mini chain.
/// 
/// Responsibilities / Trách nhiệm:
/// - Account balances and nonces / Số dư và nonce của tài khoản
/// - Transaction history (receipts) / Lịch sử giao dịch (biên nhận)
/// - Seen transaction IDs (anti-replay) / Danh sách TxId đã thấy (chống phát lại)
/// 
/// Core flow / Luồng chính: validate → apply → record receipt
/// </summary>
public class Ledger
{
    // Bản đồ tài khoản: địa chỉ → Account
    private readonly Dictionary<string, Account> _accounts = new();

    // Tập hợp TxId đã xử lý thành công (dùng để chống replay)
    private readonly HashSet<string> _seenTxIds = new();

    // Lịch sử biên nhận (cả thành công và thất bại)
    private readonly List<TxReceipt> _txHistory = new();

    // Bộ kiểm tra tính hợp lệ giao dịch
    private readonly TxValidator _validator;

    /// <summary>
    /// The chain identifier for this ledger.
    /// Định danh chuỗi của sổ cái này.
    /// </summary>
    public string ChainId { get; }

    /// <summary>
    /// Read-only view of all accounts.
    /// Chế độ xem chỉ-đọc của tất cả tài khoản.
    /// </summary>
    public IReadOnlyDictionary<string, Account> Accounts => _accounts;

    /// <summary>
    /// Read-only view of all seen transaction IDs.
    /// Chế độ xem chỉ-đọc của tất cả TxId đã thấy.
    /// </summary>
    public IReadOnlySet<string> SeenTxIds => _seenTxIds;

    /// <summary>
    /// Read-only view of transaction history (receipts).
    /// Chế độ xem chỉ-đọc của lịch sử giao dịch (biên nhận).
    /// </summary>
    public IReadOnlyList<TxReceipt> TxHistory => _txHistory;

    public Ledger(string chainId = "mini-chain-v1")
    {
        ChainId = chainId;
        _validator = new TxValidator(chainId);
    }

    /// <summary>
    /// Create or initialize an account with the given balance.
    /// If the account already exists, its balance is set to the given value.
    /// 
    /// Tạo hoặc khởi tạo tài khoản với số dư cho trước.
    /// Nếu tài khoản đã tồn tại, số dư sẽ được cập nhật.
    /// </summary>
    public Account CreateAccount(string address, ulong balance = 0)
    {
        var addr = address.ToLowerInvariant();
        if (_accounts.TryGetValue(addr, out var existing))
        {
            existing.Balance = balance;
            return existing;
        }

        var account = new Account(addr, balance);
        _accounts[addr] = account;
        return account;
    }

    /// <summary>
    /// Get an account by address, or null if not found.
    /// Lấy tài khoản theo địa chỉ, hoặc null nếu không tìm thấy.
    /// </summary>
    public Account? GetAccount(string address)
    {
        _accounts.TryGetValue(address.ToLowerInvariant(), out var account);
        return account;
    }

    /// <summary>
    /// Submit a transaction for validation and execution.
    /// Returns a TxReceipt indicating success or failure.
    /// 
    /// Gửi giao dịch để kiểm tra và thực thi.
    /// Trả về TxReceipt cho biết thành công hay thất bại.
    /// 
    /// Flow / Luồng: validate → apply (if valid / nếu hợp lệ) → append receipt
    /// Failed transactions also generate a receipt for traceability (skip-invalid strategy).
    /// Giao dịch thất bại cũng tạo biên nhận để theo dõi (chiến lược skip-invalid).
    /// </summary>
    public TxReceipt SubmitTransaction(Transaction tx)
    {
        // Kiểm tra tính hợp lệ / Validate
        var error = _validator.Validate(tx, _accounts, _seenTxIds);

        if (error != null)
        {
            // Thất bại — ghi biên nhận nhưng KHÔNG thay đổi trạng thái
            // Failed — record receipt but do NOT modify state
            var failedReceipt = new TxReceipt(tx.TxId, TxStatus.Failed, error);
            _txHistory.Add(failedReceipt);
            return failedReceipt;
        }

        // Áp dụng: trừ tiền người gửi, cộng tiền người nhận, tăng nonce
        // Apply: debit sender, credit receiver, increment nonce
        Apply(tx);

        // Ghi nhận thành công / Record success
        _seenTxIds.Add(tx.TxId);
        var receipt = new TxReceipt(tx.TxId, TxStatus.Success);
        _txHistory.Add(receipt);

        return receipt;
    }

    /// <summary>
    /// Apply a validated transaction to state.
    /// Áp dụng giao dịch đã kiểm tra hợp lệ vào trạng thái.
    /// 
    /// Steps / Các bước:
    /// - Debit sender balance / Trừ số dư người gửi
    /// - Credit receiver balance (auto-create if needed) / Cộng số dư người nhận (tự tạo nếu cần)
    /// - Increment sender nonce / Tăng nonce người gửi
    /// </summary>
    private void Apply(Transaction tx)
    {
        var sender = _accounts[tx.From];
        sender.Balance -= tx.Amount;
        sender.Nonce += 1;

        // Tự động tạo tài khoản người nhận nếu chưa tồn tại
        // Auto-create receiver account if it doesn't exist
        if (!_accounts.TryGetValue(tx.To, out var receiver))
        {
            receiver = new Account(tx.To, 0);
            _accounts[tx.To] = receiver;
        }

        receiver.Balance += tx.Amount;
    }

    /// <summary>
    /// Compute the state root: SHA-256 of canonical JSON of sorted state.
    /// Tính state root: SHA-256 của canonical JSON trạng thái đã sắp xếp.
    /// 
    /// Format: {"balances":{"alice":90,"bob":110},"nonces":{"alice":1}}
    /// </summary>
    public string GetStateRoot()
    {
        var stateJson = CanonicalJson.SerializeState(_accounts);
        return Hasher.Hash(stateJson);
    }
}
