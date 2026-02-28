using System.Text.Json;

namespace MiniChain.Core.Serialization;

/// <summary>
/// Canonical JSON serializer per DATA_FORMATS.md.
/// Bộ serialize JSON chuẩn tắc theo DATA_FORMATS.md.
/// 
/// Rules / Quy tắc:
/// - Keys sorted alphabetically at every nesting level / Key sắp xếp alphabet ở mọi cấp
/// - Integers only (no floats) / Chỉ số nguyên (không số thực)
/// - UTF-8, compact form (no extra whitespace) / UTF-8, dạng nén (không khoảng trắng thừa)
/// - No null values / Không giá trị null
/// </summary>
public static class CanonicalJson
{
    /// <summary>
    /// Serializes a Transaction's payload fields into canonical JSON for TxId derivation.
    /// Serialize các trường payload của Transaction thành canonical JSON để tính TxId.
    /// 
    /// Sorted keys: amount, chainId, from, nonce, to
    /// Key sắp xếp: amount, chainId, from, nonce, to
    /// 
    /// Example / Ví dụ: {"amount":10,"chainId":"mini-chain-v1","from":"alice","nonce":0,"to":"bob"}
    /// </summary>
    public static string SerializeTxPayload(Models.Transaction tx)
    {
        // Xây dựng thủ công để đảm bảo thứ tự key tất định
        // Manual construction ensures deterministic key order
        return $"{{\"amount\":{tx.Amount},\"chainId\":\"{tx.ChainId}\",\"from\":\"{tx.From}\",\"nonce\":{tx.Nonce},\"to\":\"{tx.To}\"}}";
    }

    /// <summary>
    /// Serializes the state (balances + nonces) into canonical JSON for state root derivation.
    /// Serialize trạng thái (số dư + nonce) thành canonical JSON để tính state root.
    /// 
    /// Format per DATA_FORMATS.md:
    /// {"balances":{"alice":90,"bob":110},"nonces":{"alice":1}}
    /// 
    /// Rules / Quy tắc:
    /// - "balances" before "nonces" (alphabetical) / "balances" trước "nonces" (theo alphabet)
    /// - Within each, addresses sorted alphabetically / Trong mỗi object, địa chỉ sắp xếp alphabet
    /// - Addresses with zero balance/nonce may be omitted / Địa chỉ có giá trị 0 có thể bỏ qua
    /// </summary>
    public static string SerializeState(IReadOnlyDictionary<string, Models.Account> accounts)
    {
        // Sắp xếp địa chỉ theo thứ tự alphabet (ordinal) để đảm bảo tất định
        var sortedAddresses = accounts.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList();

        // Xây dựng object balances (bỏ qua số dư bằng 0)
        // Build balances object (omit zero balances)
        var balanceParts = new List<string>();
        foreach (var addr in sortedAddresses)
        {
            var bal = accounts[addr].Balance;
            if (bal > 0)
                balanceParts.Add($"\"{addr}\":{bal}");
        }

        // Xây dựng object nonces (bỏ qua nonce bằng 0)
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

        // Kết hợp: balances trước nonces (theo alphabet)
        return $"{{\"balances\":{balancesJson},\"nonces\":{noncesJson}}}";
    }
}
