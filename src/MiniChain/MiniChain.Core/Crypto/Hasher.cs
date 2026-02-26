using System.Security.Cryptography;
using System.Text;

namespace MiniChain.Core.Crypto;

/// <summary>
/// SHA-256 hashing utility.
/// Returns lowercase hex string as per architecture decisions.
/// </summary>
public static class Hasher
{
    /// <summary>
    /// Compute SHA-256 hash of a UTF-8 string and return as lowercase hex.
    /// </summary>
    public static string Hash(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = SHA256.HashData(bytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }
}
