// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Database.Models.UserData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace InfiniLore.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreDbContext : IdentityDbContext<InfiniLoreUser, IdentityRole, string> {
    public DbSet<LoreScopeModel> LoreScopes { get; init; }
    public DbSet<MultiverseModel> Multiverses { get; init; }
    public DbSet<UniverseModel> Universes { get; init; }
    
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
        
        //FluentApiUsage
        
        #region InfiniLoreUser
        builder.Entity<InfiniLoreUser>(b => {
            b.HasMany(user => user.LoreScopes).WithOne(scope => scope.User).HasForeignKey(x => x.UserId);
            b.HasMany(user => user.Multiverses).WithOne(multiverse => multiverse.User).HasForeignKey(x => x.UserId);
            b.HasMany(user => user.Universes).WithOne(universe => universe.User).HasForeignKey(x => x.UserId);
        });
        #endregion
        #region LoreScopeModel
        builder.Entity<LoreScopeModel>(b => {
            b.HasQueryFilter(model => model.SoftDeleteDate == null);
            b.HasIndex(model => new { model.Name, model.UserId }).IsUnique();
            b.HasMany(model => model.Multiverses).WithOne(multiverse => multiverse.LoreScope).HasForeignKey(x => x.LoreScopeId);
        });
        #endregion
        #region MultiverseModel
        builder.Entity<MultiverseModel>(b => {
            b.HasQueryFilter(model => model.SoftDeleteDate == null);
            b.HasIndex(model => new { model.Name, model.LoreScopeId }).IsUnique();
            b.HasMany(model => model.Universes).WithOne(universe => universe.Multiverse).HasForeignKey(x => x.MultiverseId);
        });
        #endregion
        #region UniverseModel
        builder.Entity<UniverseModel>(b => {
            b.HasQueryFilter(model => model.SoftDeleteDate == null);
            b.HasIndex(model => new { model.Name, model.MultiverseId }).IsUnique();
        });
        #endregion
        
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
