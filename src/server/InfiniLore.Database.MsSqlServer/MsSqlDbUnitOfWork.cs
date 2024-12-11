// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Types;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Database.MsSqlServer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------/// <inheritdoc cref="IDbUnitOfWork{T}" />
[InjectableService<IDbUnitOfWork<MsSqlDbContext>>(ServiceLifetime.Scoped)]
public class MsSqlDbUnitOfWork(IDbContextFactory<MsSqlDbContext> dbContextFactory) : IDbUnitOfWork<MsSqlDbContext> {
    private readonly AsyncLazy<MsSqlDbContext> _msSqlDb = new(async () => await dbContextFactory.CreateDbContextAsync());
    private IDbContextTransaction? _transaction;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask SaveChangesAsync(CancellationToken ct = default) {
        MsSqlDbContext dbContext = await _msSqlDb.GetValueAsync();
        await dbContext.SaveChangesAsync(ct);
    }

    public async ValueTask<bool> TryCommitTransactionAsync(CancellationToken ct = default) {
        if (_transaction == null) return false;

        await _transaction.CommitAsync(ct);
        _transaction.Dispose();

        return true;
    }

    public async ValueTask<bool> TryCreateTransactionAsync(CancellationToken ct = default) {
        if (_transaction != null) return false;

        _transaction = await _msSqlDb.GetValueAsync()
            .ContinueWith(continuationFunction: db => db.Result.Database.BeginTransactionAsync(ct), ct)
            .Unwrap();

        return true;
    }

    public async ValueTask<bool> TryRollbackTransactionAsync(CancellationToken ct = default) {
        if (_transaction == null) return false;

        await _transaction.RollbackAsync(ct);
        _transaction.Dispose();

        return true;
    }

    public async ValueTask<bool> TryRollbackToSavepointAsync(Guid id, CancellationToken ct = default) {
        if (_transaction == null) return false;
        if (!_transaction.SupportsSavepoints) return false;

        await _transaction.RollbackToSavepointAsync(id.ToString("N"), ct);
        return true;
    }

    public async ValueTask<bool> TryCreateSavepointAsync(Guid id, CancellationToken ct = default) {
        if (_transaction == null) return false;
        if (!_transaction.SupportsSavepoints) return false;

        await _transaction.CreateSavepointAsync(id.ToString("N"), ct);
        return true;
    }

    public async ValueTask<MsSqlDbContext> GetDbContextAsync(CancellationToken ct = default) => await _msSqlDb.GetValueAsync();

    public async ValueTask DisposeAsync() {
        if (_transaction != null) {
            await TryRollbackTransactionAsync();
            await _transaction.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}
