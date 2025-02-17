// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Database.Models.Data.System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ContentDb : IdentityDbContext<InfiniLoreUser, IdentityRole<Guid>, Guid>{
    // -----------------------------------------------------------------------------------------------------------------
    // DbSets
    // -----------------------------------------------------------------------------------------------------------------
    public DbSet<BasicData> BasicData { get; set; } = null!;
    public DbSet<UserData> UserData { get; set; } = null!;
    public DbSet<SystemData> SystemData { get; set; } = null!;
    
    public DbSet<JwtRefreshTokenData> JwtRefreshTokens { get; set; } = null!;
    
    public DbSet<KeyValueStore> KeyValueStores { get; set; } = null!;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public ContentDb() {}
    public ContentDb(DbContextOptions<ContentDb> options) : base(options) {}

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        // Everything has been moved into ModelConfiguration files
        //      This is due to more extensibility and ease of use
        builder.ApplyConfigurationsFromAssembly(typeof(IInfiniLoreServerDatabaseEntrypoint).Assembly);
    }
}
