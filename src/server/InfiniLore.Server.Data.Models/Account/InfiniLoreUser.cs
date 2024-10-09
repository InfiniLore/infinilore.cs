// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.UserData;
using Microsoft.AspNetCore.Identity;

namespace InfiniLore.Server.Data.Models.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreUser : IdentityUser {
    public ICollection<LoreScopeModel> LoreScopes { get; init; } = [];
    public ICollection<MultiverseModel> Multiverses { get; init; } = [];
    public ICollection<UniverseModel> Universes { get; init; } = [];
}
