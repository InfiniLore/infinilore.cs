// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Users.Database;
using System.Diagnostics.CodeAnalysis;

namespace InfiniLore.Server.Database.Models;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserData : IBasicData {
    public Guid OwnerId { get; set; }
    public IInfiniLoreUser? Owner { get; set; }
    [MemberNotNullWhen(true, nameof(Owner))] public bool IsOwnerIncluded { get; }
}
