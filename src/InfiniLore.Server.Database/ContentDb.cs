// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Database.Models.Data.System;
using InfiniLore.Server.Database.Models.Data.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InfiniLore.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// Not an IdentityDbContext due to Auth0 handling all the auth & identity stuff
public class ContentDb : DbContext , IReadonlyCapableDbContext{
    public bool IsReadonly { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public ContentDb() {}
    public ContentDb(DbContextOptions<ContentDb> options) : base(options) {}
    // -----------------------------------------------------------------------------------------------------------------
    // DbSets
    // -----------------------------------------------------------------------------------------------------------------
    public DbSet<KeyValueStore> KeyValueStores { get; set; } = null!;

    public DbSet<InfiniLoreUser> Users { get; set; } = null!;
    public DbSet<LoreScope> LoreScopes { get; set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void SetAsReadonly() {
        if (IsReadonly) return; // Already readonly
        IsReadonly = true;

        // Since this is a DbContext, ensure it's configured to be read-only.
        // Prevent any transaction or modification logic, e.g., disabling change tracking.
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    
    public override int SaveChanges() {
        if (IsReadonly) throw new InvalidOperationException("The database context is in read-only mode.");
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        if (IsReadonly) throw new InvalidOperationException("The database context is in read-only mode.");
        return base.SaveChangesAsync(cancellationToken);
    }
    
    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        // Everything has been moved into ModelConfiguration files
        //      This is due to more extensibility and ease of use
        builder.ApplyConfigurationsFromAssembly(typeof(IInfiniLoreServerDatabaseEntrypoint).Assembly);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning));
    }

}
