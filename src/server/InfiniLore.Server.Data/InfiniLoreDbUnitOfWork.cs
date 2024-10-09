// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace InfiniLore.Server.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Implementation of the unit of work pattern specific to InfiniLore database context.
/// </summary>
[RegisterService<IDbUnitOfWork<InfiniLoreDbContext>>(LifeTime.Scoped)]
public class InfiniLoreDbUnitOfWork(IDbContextFactory<InfiniLoreDbContext> dbContextFactory) : IDbUnitOfWork<InfiniLoreDbContext>, IDisposable {
    /// <summary>
    /// Provides access to the underlying database context in the InfiniLore system.
    /// This context is primarily used to interact with database models such as LoreScopes, Multiverses, and Universes.
    /// </summary>
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
    /// <summary>
    /// Asynchronously saves all changes made in this context to the database.
    /// </summary>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken ct = default) {
        return await Db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Begins a new database transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task BeginTransactionAsync() {
        _transaction = await Db.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Attempts to commit the current database transaction asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is a boolean indicating whether the transaction was successfully committed.
    /// </returns>
    public async Task<bool> TryCommitTransactionAsync() {
        if (_transaction == null) return false;

        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
        return true;
    }

    /// <summary>
    /// Attempts to rollback the current database transaction. If no transaction is active, returns false.
    /// </summary>
    /// <returns>
    /// Returns a boolean indicating whether the rollback was successful.
    /// Returns true if the rollback occurred, otherwise returns false.
    /// </returns>
    public async Task<bool> TryRollbackTransactionAsync() {
        if (_transaction == null) return false;
        
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
        return true;
    }

    /// <summary>
    /// Retrieves the current InfiniLoreDbContext instance.
    /// </summary>
    /// <returns>The InfiniLoreDbContext instance.</returns>
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
