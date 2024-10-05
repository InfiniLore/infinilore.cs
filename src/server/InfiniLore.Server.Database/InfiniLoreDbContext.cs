// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Contracts;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Database.Models.UserData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InfiniLore.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreDbContext : IdentityDbContext<InfiniLoreUser, IdentityRole, string> {
    public DbSet<LoreScopeModel> LoreScopes { get; init; }
    public IQueryable<LoreScopeModel> LoreScopesWithUsers => LoreScopes.Include(x => x.User); 

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public InfiniLoreDbContext() {}
    public InfiniLoreDbContext(DbContextOptions<InfiniLoreDbContext> options) : base(options) {}
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        base.OnConfiguring(optionsBuilder);
        
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
    
    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        
        builder.Entity<LoreScopeModel>().HasQueryFilter(model => model.SoftDeleteDate == null);

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
