// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Database.Models.Data.System;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// Not an IdentityDbContext due to Auth0 handling all of the auth & identity stuff
public class ContentDb : DbContext {

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public ContentDb() {}
    public ContentDb(DbContextOptions<ContentDb> options) : base(options) {}
    // -----------------------------------------------------------------------------------------------------------------
    // DbSets
    // -----------------------------------------------------------------------------------------------------------------
    public DbSet<BasicData> BasicData { get; set; } = null!;
    public DbSet<UserData> UserData { get; set; } = null!;
    public DbSet<SystemData> SystemData { get; set; } = null!;

    public DbSet<KeyValueStore> KeyValueStores { get; set; } = null!;

    public DbSet<InfiniLoreUser> Users { get; set; } = null!;

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
