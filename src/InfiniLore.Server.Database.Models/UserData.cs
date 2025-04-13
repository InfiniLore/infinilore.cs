// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Account;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Server.Database.Models;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Ownership data for objects which can be owned by a user.
/// </summary>
public class UserData : BasicData {
    /// <summary>
    /// The <see cref="Owner"/>'s <see cref="Guid">ID</see>
    /// </summary>
    /// <remarks>
    /// by default is equal to <see cref="Guid.Empty"/>.
    /// </remarks>
    public Guid OwnerId { get; set; } = Guid.Empty;

    /// <summary>
    /// The owner of this entity.
    /// </summary>
    [MaybeNull] public InfiniLoreUser Owner { get; set; } = null!;

    // public bool IsPublic { get; set; } = false;
}
