// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Database.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IUnitOfWork>(ServiceLifetime.Scoped)]
public class UnitOfWork(IDbContextFactory<MsSqlDbContext> dbContextFactory) : IUnitOfWork{
    private readonly AsyncLazy<MsSqlDbContext> _msSqlDb = new(async ct => await dbContextFactory.CreateDbContextAsync(ct));
    private IDbContextTransaction? _msSqlTransaction;

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

        _msSqlTransaction = await _msSqlDb.GetValueAsync(ct)
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

    public async ValueTask DisposeAsync() {
        await _msSqlDb.DisposeAsync();
        if (_msSqlTransaction != null) {
            await TryRollbackTransactionAsync();
            await _msSqlTransaction.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}
