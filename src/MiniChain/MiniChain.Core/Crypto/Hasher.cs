using System.Security.Cryptography;
using System.Text;

namespace MiniChain.Core.Crypto;

/// <summary>
/// SHA-256 hashing utility.
/// Công cụ băm SHA-256.
/// 
/// Returns lowercase hex string as per architecture decisions.
/// Trả về chuỗi hex chữ thường theo quyết định kiến trúc.
/// </summary>
public static class Hasher
{
    /// <summary>
    /// Compute SHA-256 hash of a UTF-8 string and return as lowercase hex.
    /// Tính băm SHA-256 của chuỗi UTF-8 và trả về dạng hex chữ thường.
    /// 
    /// Example / Ví dụ: "hello" → "2cf24dba5fb0a30e..."
    /// </summary>
    public static string Hash(string input)
    {
        // Chuyển chuỗi thành mảng byte UTF-8
        var bytes = Encoding.UTF8.GetBytes(input);

        // Tính hash SHA-256 (trả về 32 byte = 256 bit)
        var hashBytes = SHA256.HashData(bytes);

        // Chuyển byte[] thành chuỗi hex chữ thường (64 ký tự)
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }
}
