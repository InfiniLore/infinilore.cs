// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Database.Models.Content.Data.User;
using Microsoft.AspNetCore.Identity;

namespace InfiniLore.Database.Models.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable CollectionNeverUpdated.Global
public class InfiniLoreUser : IdentityUser<Guid> {
    public ICollection<LorescopeModel> Lorescopes { get; init; } = [];
    public ICollection<Guid> LorescopeIds { get; init; } = []; // Regularly stored as JSON, but this is easier to work with.
    
    public ICollection<MultiverseModel> Multiverses { get; init; } = [];
    public ICollection<Guid> MultiverseIds { get; init; } = []; // Regularly stored as JSON, but this is easier to work with.
    
    public ICollection<UniverseModel> Universes { get; init; } = [];
    public ICollection<Guid> UniverseIds { get; init; } = []; // Regularly stored as JSON, but this is easier to work with.
    
    public ICollection<JwtRefreshTokenModel> JwtRefreshTokens { get; init; } = [];
    public ICollection<UserContentAccessModel> ContentAccesses { get; init; } = [];
    public ICollection<InfiniLorePermission> Permissions { get; init; } = [];
}
