// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Identity;
using Old.InfiniLore.Database.Models.Content.Data.System;
using Old.InfiniLore.Database.Models.Content.Data.User;

namespace Old.InfiniLore.Database.Models.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable CollectionNeverUpdated.Global
public class InfiniLoreUser : IdentityUser<Guid> {
    public ICollection<LorescopeModel> Lorescopes { get; init; } = [];
    public ICollection<Guid> LorescopeIds { get; init; } = []; // Regularly stored as JSON, but this is easier to work with.
    
    public ICollection<JwtRefreshTokenModel> JwtRefreshTokens { get; init; } = [];
    public ICollection<UserContentAccessModel> ContentAccesses { get; init; } = [];
    public ICollection<InfiniLorePermission> Permissions { get; init; } = [];
}
