// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;
using InfiniLore.Server.Modules.Users.Database;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserData : BasicData, IUserData {
    public Guid OwnerId { get; set; } = Guid.Empty;
    public IInfiniLoreUser? Owner { get; set; } = null;
    [MemberNotNullWhen(true, nameof(Owner))] public bool IsOwnerIncluded => Owner != null; 
}
