// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Database.Models.Content.Data.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace InfiniLore.Database.MsSqlServer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MsSqlDbContext : IdentityDbContext<InfiniLoreUser, IdentityRole<Guid>, Guid> {

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public MsSqlDbContext() {}
    public MsSqlDbContext(DbContextOptions<MsSqlDbContext> options) : base(options) {}
    
    // -----------------------------------------------------------------------------------------------------------------
    // DbSets
    // -----------------------------------------------------------------------------------------------------------------
    public DbSet<LorescopeModel> Lorescopes { get; init; }
    public DbSet<MultiverseModel> Multiverses { get; init; }
    public DbSet<UniverseModel> Universes { get; init; }
    public DbSet<JwtRefreshTokenModel> JwtRefreshTokens { get; init; }
    public DbSet<UserContentAccessModel> UserContentAccesses { get; init; }
    public DbSet<InfinilorePermission> Permissions { get; init; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        // Everything has been moved into ModelConfiguration files
        builder.ApplyConfigurationsFromAssembly(typeof(IAssemblyEntry).Assembly);

        // SEEDING DATA
        // ------------
        // TODO Long term: Change Seeding Data approach
        // this doesn't feel like the correct approach to seed data for the server in the long term.
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole("admin") { NormalizedName = "ADMIN", Id = "0b3715f5-d1d2-4ffb-b869-0ab2462ad504" },
            new IdentityRole("user") { NormalizedName = "USER", Id = "b693ab6e-5a2c-4093-947d-e1e1f3797294" }
        );

        builder.Entity<InfiniLoreUser>().HasData(new InfiniLoreUser {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            ConcurrencyStamp = "a22a94ae-95ae-41d9-9b76-69332a675474",
            UserName = "testuser",
            NormalizedUserName = "TESTUSER",
            Email = "testuser@example.com",
            NormalizedEmail = "TESTUSER@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = "d957c0f8-e90e-4068-a968-4f4b49fc165b",
            PasswordHash = "AQAAAAIAAYagAAAAEPcntIx4Y071oyt5g84a1kLZSkEA3/WG4dB8VJiyGcbZD2XUFHSqpWL9PqF+LL6aeQ=="// Test@1234
        });
    }
}
