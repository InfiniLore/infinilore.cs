// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.UserData;
using Microsoft.AspNetCore.Identity;

namespace InfiniLore.Server.Database.Models.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreUser : IdentityUser {
    public ICollection<LoreScopeModel> LoreScopes { get; init; } = [];
    public ICollection<MultiverseModel> Multiverses { get; init; } = [];
    public ICollection<UniverseModel> Universes { get; init; } = [];
}
