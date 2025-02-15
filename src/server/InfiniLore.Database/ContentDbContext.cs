// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Database.Models.Content.Data.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace InfiniLore.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ContentDbContext : IdentityDbContext<InfiniLoreUser, IdentityRole<Guid>, Guid> {

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public ContentDbContext() {}
    public ContentDbContext(DbContextOptions<ContentDbContext> options) : base(options) {}

    // -----------------------------------------------------------------------------------------------------------------
    // DbSets
    // -----------------------------------------------------------------------------------------------------------------
    public DbSet<LorescopeModel> Lorescopes { get; init; }
    public DbSet<JwtRefreshTokenModel> JwtRefreshTokens { get; init; }
    public DbSet<UserContentAccessModel> UserContentAccesses { get; init; }
    public DbSet<InfiniLorePermission> Permissions { get; init; }
    public DbSet<SystemInformation> SystemInformation { get; init; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        // Everything has been moved into ModelConfiguration files
        builder.ApplyConfigurationsFromAssembly(typeof(IAssemblyEntry).Assembly);
    }
}
