// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Account;

namespace InfiniLore.Server.Data.Models.Base;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserContentAccess<T> : BaseContent<T> where T : BaseContent<T> {
    public required InfiniLoreUser User { get; set; }
    public required AccessLevel AccessLevel { get; set; } = AccessLevel.None;
}

public enum AccessLevel {
    None,
    Read,
    Write,
    Owner
}