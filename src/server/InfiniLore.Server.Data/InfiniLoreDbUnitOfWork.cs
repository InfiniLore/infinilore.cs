// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace InfiniLore.Server.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

/// <inheritdoc cref="InfiniLore.Server.Contracts.Data.IDbUnitOfWork{T}" />
[RegisterService<IDbUnitOfWork<InfiniLoreDbContext>>(LifeTime.Scoped)]
public class InfiniLoreDbUnitOfWork(IDbContextFactory<InfiniLoreDbContext> dbContextFactory) : IDbUnitOfWork<InfiniLoreDbContext> {
    /// <inheritdoc/>
    public InfiniLoreDbContext Db { get; } = dbContextFactory.CreateDbContext();

    /// <summary>
    /// Represents the current database transaction for the DbContext.
    /// This variable is used to manage transactions including beginning, committing, and rolling back as needed.
    /// It is nullable and will be null when there is no active transaction.
    /// </summary>
    private IDbContextTransaction? _transaction;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await Db.SaveChangesAsync(ct);

    /// <inheritdoc/>
    public async Task BeginTransactionAsync(CancellationToken ct = default) {
        _transaction = await Db.Database.BeginTransactionAsync(ct);
    }

    /// <inheritdoc/>
    public Task CommitTransactionAsync(CancellationToken ct = default) => TryCommitTransactionAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> TryCommitTransactionAsync(CancellationToken ct = default) {
        if (_transaction == null) return false;

        await _transaction.CommitAsync(ct);
        await _transaction.DisposeAsync();
        _transaction = null;
        return true;
    }

    /// <inheritdoc/>
    public Task RollbackTransactionAsync(CancellationToken ct = default) => TryRollbackTransactionAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> TryRollbackTransactionAsync(CancellationToken ct = default) {
        if (_transaction == null) return false;

        await _transaction.RollbackAsync(ct);
        await _transaction.DisposeAsync();
        _transaction = null;
        return true;
    }

    /// <inheritdoc/>
    public InfiniLoreDbContext GetDbContext() => Db;

    /// <summary>
    /// Disposes the resources used by the <see cref="InfiniLoreDbUnitOfWork"/>.
    /// </summary>
    /// <remarks>
    /// This method disposes the underlying <see cref="InfiniLoreDbContext"/> and
    /// the current transaction if it exists. It also suppresses the finalization of this instance
    /// by calling <see cref="GC.SuppressFinalize(object)"/>.
    /// </remarks>
    public void Dispose() {
        Db.Dispose();
        _transaction?.Dispose();

        GC.SuppressFinalize(this);
    }
}
