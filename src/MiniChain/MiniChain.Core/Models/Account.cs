namespace MiniChain.Core.Models;

/// <summary>
/// Represents an account on the mini chain.
/// Each account has an address (lowercase string), a balance (native coin), and a nonce.
/// </summary>
public class Account
{
    /// <summary>Unique lowercase identifier for this account.</summary>
    public string Address { get; }

    /// <summary>Native coin balance. Always >= 0.</summary>
    public ulong Balance { get; internal set; }

    /// <summary>
    /// Next valid nonce for outgoing transactions.
    /// Incremented by 1 after each successful tx from this account.
    /// </summary>
    public ulong Nonce { get; internal set; }

    public Account(string address, ulong balance = 0, ulong nonce = 0)
    {
        Address = address.ToLowerInvariant();
        Balance = balance;
        Nonce = nonce;
    }

    /// <summary>Creates a deep copy for snapshotting.</summary>
    public Account Clone() => new(Address, Balance, Nonce);

    public override string ToString() => $"Account({Address}, balance={Balance}, nonce={Nonce})";
}
