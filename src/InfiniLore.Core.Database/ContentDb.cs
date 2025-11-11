// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InfiniLore.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// Not an IdentityDbContext due to Auth0 handling all the auth & identity stuff
public class ContentDb : DbContext, IReadonlyCapableDbContext {
    public bool IsReadonly { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public ContentDb() {}
    public ContentDb(DbContextOptions<ContentDb> options) : base(options) {}

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void SetAsReadonly() {
        if (IsReadonly) return;

        IsReadonly = true;

        // Since this is a DbContext, ensure it's configured to be read-only.
        // Prevent any transaction or modification logic, e.g., disabling change tracking.
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        ContentDbFactory.ConfigureModel(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        // Required to disable the query filter warning we have one "soft deleted" content
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning));
    }

    private T WrapAsReadonly<T>(Func<T> action) {
        if (IsReadonly) throw new InvalidOperationException("The database context is in read-only mode.");

        return action.Invoke();
    }

    private void WrapAsReadonly(Action action) {
        if (IsReadonly) throw new InvalidOperationException("The database context is in read-only mode.");

        action.Invoke();
    }
    
    private Task<T> WrapAsReadonly<T>(Func<Task<T>> action) {
        if (IsReadonly) throw new InvalidOperationException("The database context is in read-only mode.");
        return action.Invoke();
    }

    private Task WrapAsReadonly(Func<Task> action) {
        if (IsReadonly) throw new InvalidOperationException("The database context is in read-only mode.");
        return action.Invoke();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Entity Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override int SaveChanges()
        => WrapAsReadonly(() => base.SaveChanges());

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => WrapAsReadonly(() => base.SaveChangesAsync(cancellationToken));

    public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        => WrapAsReadonly(() => base.Add(entity));

    public override EntityEntry Add(object entity)
        => WrapAsReadonly(() => base.Add(entity));

    public override void AddRange(params object[] entities)
        => WrapAsReadonly(() => base.AddRange(entities));

    public override void AddRange(IEnumerable<object> entities)
        => WrapAsReadonly(() => base.AddRange(entities));

    public override EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
        => WrapAsReadonly(() => base.Attach(entity));

    public override EntityEntry Attach(object entity)
        => WrapAsReadonly(() => base.Attach(entity));

    public override void AttachRange(params object[] entities)
        => WrapAsReadonly(() => base.AttachRange(entities));

    public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        => WrapAsReadonly(() => base.Remove(entity));

    public override EntityEntry Remove(object entity)
        => WrapAsReadonly(() => base.Remove(entity));

    public override void RemoveRange(params object[] entities)
        => WrapAsReadonly(() => base.RemoveRange(entities));

    public override void RemoveRange(IEnumerable<object> entities)
        => WrapAsReadonly(() => base.RemoveRange(entities));

    public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        => WrapAsReadonly(() => base.Update(entity));

    public override EntityEntry Update(object entity)
        => WrapAsReadonly(() => base.Update(entity));

    public override void UpdateRange(params object[] entities)
        => WrapAsReadonly(() => base.UpdateRange(entities));

    public override void UpdateRange(IEnumerable<object> entities)
        => WrapAsReadonly(() => base.UpdateRange(entities));

    public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) 
        => new ValueTask<EntityEntry<TEntity>>(WrapAsReadonly(() => base.Add(entity)));

    public override Task AddRangeAsync(params object[] entities)
        => WrapAsReadonly(() => base.AddRangeAsync(entities));

    public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
        => WrapAsReadonly(() => base.AddRangeAsync(entities, cancellationToken));

    public override EntityEntry Entry(object entity)
        => WrapAsReadonly(() => base.Entry(entity));

    public override EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        => WrapAsReadonly(() => base.Entry(entity));
}
