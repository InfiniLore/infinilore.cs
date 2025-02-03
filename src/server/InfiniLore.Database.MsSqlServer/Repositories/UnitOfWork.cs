// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace InfiniLore.Database.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IUnitOfWork>(ServiceLifetime.Scoped)]
public class UnitOfWork(IDbContextFactory<MsSqlDbContext> dbContextFactory, IServiceProvider provider) : IUnitOfWork{
    private readonly AsyncLazy<MsSqlDbContext> _msSqlDb = new(async ct => await dbContextFactory.CreateDbContextAsync(ct));
    private IDbContextTransaction? _msSqlTransaction;
    private ConcurrentDictionary<Type, IRepository> AttachedRepositories { get; } = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask SaveChangesAsync(CancellationToken ct = default) {
        MsSqlDbContext dbContext = await _msSqlDb.GetValueAsync(ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async ValueTask<bool> TryCommitTransactionAsync(CancellationToken ct = default) {
        if (_msSqlTransaction == null) return false;

        await _msSqlTransaction.CommitAsync(ct);
        _msSqlTransaction.Dispose();

        return true;
    }

    public async ValueTask<bool> TryCreateTransactionAsync(CancellationToken ct = default) {
        if (_msSqlTransaction != null) return false;

        _msSqlTransaction = await _msSqlDb.GetValueAsync(ct).AsTask()
            .ContinueWith(continuationFunction: db => db.Result.Database.BeginTransactionAsync(ct), ct)
            .Unwrap();

        return true;
    }

    public async ValueTask<bool> TryRollbackTransactionAsync(CancellationToken ct = default) {
        if (_msSqlTransaction == null) return false;

        await _msSqlTransaction.RollbackAsync(ct);
        _msSqlTransaction.Dispose();

        return true;
    }

    public async ValueTask<bool> TryRollbackToSavepointAsync(Guid id, CancellationToken ct = default) {
        if (_msSqlTransaction == null) return false;
        if (!_msSqlTransaction.SupportsSavepoints) return false;

        await _msSqlTransaction.RollbackToSavepointAsync(id.ToString("N"), ct);
        return true;
    }

    public async ValueTask<bool> TryCreateSavepointAsync(Guid id, CancellationToken ct = default) {
        if (_msSqlTransaction == null) return false;
        if (!_msSqlTransaction.SupportsSavepoints) return false;

        await _msSqlTransaction.CreateSavepointAsync(id.ToString("N"), ct);
        return true;
    }

    public async ValueTask<T> GetDbContextAsync<T>(CancellationToken ct = default) where T : DbContext {
        if (typeof(T) != typeof(MsSqlDbContext)) throw new NotSupportedException($"DbContext type '{typeof(T)}' is not supported by this UnitOfWork.");

        MsSqlDbContext dbContext = await _msSqlDb.GetValueAsync(ct);
        return dbContext as T ?? throw new InvalidCastException($"Cannot cast DbContext of type '{dbContext.GetType()}' to '{typeof(T)}'");
    }

    public TRepo GetRepository<TRepo>() where TRepo : class, IRepository {
        if (AttachedRepositories.TryGetValue(typeof(TRepo), out IRepository? cachedRepo) && cachedRepo is TRepo castedCachedRepo) return castedCachedRepo;
        
        // Cache miss so we create a new instance
        var repo = provider.GetRequiredService<TRepo>();
        repo.TryAttach(this);
        AttachedRepositories.AddOrUpdate(typeof(TRepo), repo);
        return repo;
    }

    public async ValueTask DisposeAsync() {
        await _msSqlDb.DisposeAsync();
        if (_msSqlTransaction != null) {
            await TryRollbackTransactionAsync();
            await _msSqlTransaction.DisposeAsync();
        }

        if (!AttachedRepositories.IsEmpty) {
            // First detach all references to this unit of work
            foreach ((_, IRepository repo) in AttachedRepositories) {
                repo.TryDetach(this);
            }
            
            // Then clear our own reference to them
            AttachedRepositories.Clear();
        } 

        GC.SuppressFinalize(this);
    }
}
