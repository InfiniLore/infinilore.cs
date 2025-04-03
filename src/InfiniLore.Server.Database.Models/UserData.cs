// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Account;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Server.Database.Models;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserData : BasicData {
    public Guid OwnerId { get; set; } = Guid.Empty;
    [MaybeNull] public InfiniLoreUser Owner { get; set; } = null!;

    // public bool IsPublic { get; set; } = false;
}
