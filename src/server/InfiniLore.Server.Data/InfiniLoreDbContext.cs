// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data;
using InfiniLore.Server.Data.Models.Account;
using InfiniLore.Server.Data.Models.Base;
using InfiniLore.Server.Data.Models.UserData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace InfiniLore.Server.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreDbContext : IdentityDbContext<InfiniLoreUser, IdentityRole, string> {
    public DbSet<LoreScopeModel> LoreScopes { get; init; }
    public DbSet<MultiverseModel> Multiverses { get; init; }
    public DbSet<UniverseModel> Universes { get; init; }

    #region AuditLogs
    public DbSet<AuditLog<LoreScopeModel>> LoreScopeAuditLogs { get; init; }
    public DbSet<AuditLog<MultiverseModel>> MultiverseAuditLogs { get; init; }
    public DbSet<AuditLog<UniverseModel>> UniverseAuditLogs { get; init; }
    #endregion
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public InfiniLoreDbContext() {}
    public InfiniLoreDbContext(DbContextOptions<InfiniLoreDbContext> options) : base(options) {}
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        base.OnConfiguring(options);
        
        options.UseSqlite("Data Source=app.db");
    }
    
    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        
        // Everything has been moved into ModelConfiguration files
        builder.ApplyConfigurationsFromAssembly(typeof(IAssemblyEntry).Assembly);
        
        // SEEDING DATA
        // TODO Move seeding data to a proper service
        
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole("admin") {NormalizedName = "ADMIN"},
            new IdentityRole("user") {NormalizedName = "USER"}
        );
        
        #region Test User
        var testUser = new InfiniLoreUser {
            Id = "d957c0f8-e90e-4068-a968-4f4b49fc165c",
            UserName = "testuser",
            NormalizedUserName = "TESTUSER",
            Email = "testuser@example.com",
            NormalizedEmail = "TESTUSER@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = "d957c0f8-e90e-4068-a968-4f4b49fc165b"
        };

        var hasher = new PasswordHasher<InfiniLoreUser>();
        testUser.PasswordHash = hasher.HashPassword(testUser, "Test@1234");

        builder.Entity<InfiniLoreUser>().HasData(testUser);
        #endregion

    }
}
