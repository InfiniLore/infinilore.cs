// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace InfiniLore.Server.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

/// <inheritdoc cref="InfiniLore.Server.Contracts.Data.IDbUnitOfWork{T}" />
[RegisterService<IDbUnitOfWork<InfiniLoreDbContext>>(LifeTime.Scoped)]
public class InfiniLoreDbUnitOfWork(IDbContextFactory<InfiniLoreDbContext> dbContextFactory, ILogger logger) : IDbUnitOfWork<InfiniLoreDbContext> { 
    private readonly InfiniLoreDbContext _db = dbContextFactory.CreateDbContext();
    private IDbContextTransaction? _transaction;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc/>
    public async Task Commit(CancellationToken ct = default) {
        if (_transaction == null) {
            await _db.SaveChangesAsync(ct);
            return;
        }

        await _transaction.CommitAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<bool> TryCommit(CancellationToken ct = default) {
        try {
            if (_transaction == null) {
                await _db.SaveChangesAsync(ct);
                return true;
            }

            await _transaction.CommitAsync(ct);
            return true;
            
        } catch (Exception ex) {
            logger.Error(ex, "Error while committing transaction");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task BeginTransactionAsync(CancellationToken ct = default) {
        _transaction = await _db.Database.BeginTransactionAsync(ct);
    }

    /// <inheritdoc/>
    public Task RollbackTransactionAsync(CancellationToken ct = default) => TryRollbackTransactionAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> TryRollbackTransactionAsync(CancellationToken ct = default) {
        if (_transaction == null) return false;
        await _transaction.RollbackAsync(ct);
        return true;
    }

    /// <inheritdoc/>
    public InfiniLoreDbContext GetDbContext() => _db;

    /// <summary>
    /// Disposes the resources used by the <see cref="InfiniLoreDbUnitOfWork"/>.
    /// </summary>
    /// <remarks>
    /// This method disposes the underlying <see cref="InfiniLoreDbContext"/> and
    /// the current transaction if it exists. It also suppresses the finalization of this instance
    /// by calling <see cref="GC.SuppressFinalize(object)"/>.
    /// </remarks>
    public void Dispose() {
        _db.Dispose();
        _transaction?.Dispose();

        GC.SuppressFinalize(this);
    }
}
