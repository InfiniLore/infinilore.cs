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
    public DbSet<LoreScopeModel> LoreScopes { get; set; }

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
        
        Assembly currentAssembly = typeof(IAssemblyEntry).Assembly;
        IEnumerable<Type> softDeletableTypes = currentAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ISoftDeletable)));
        foreach (Type softDeletableType in softDeletableTypes) {
            builder.Entity(softDeletableType).HasQueryFilter((ISoftDeletable x) => x.SoftDeleteDate == null);
        }

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole("admin"),
            new IdentityRole("user")
        );

        #region Test User
        var testUser = new InfiniLoreUser {
            UserName = "testuser",
            Email = "testuser@example.com",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var hasher = new PasswordHasher<InfiniLoreUser>();
        testUser.PasswordHash = hasher.HashPassword(testUser, "Test@1234");

        builder.Entity<InfiniLoreUser>().HasData(testUser);
        #endregion

    }
}
