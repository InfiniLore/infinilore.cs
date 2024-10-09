// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Contracts.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Implementation of the unit of work pattern specific to InfiniLore database context.
/// </summary>
public interface IDbUnitOfWork<out T> : IDisposable where T : DbContext {
    /// <summary>
    /// Provides access to the underlying database context in the InfiniLore system.
    /// This context is primarily used to interact with database models such as LoreScopes, Multiverses, and Universes.
    /// </summary>
    T Db { get; } 
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // ----------------------------------------------------------------------------------------------------------------
    
    /// <summary>
    /// Asynchronously saves all changes made in this context to the database.
    /// </summary>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Begins a new database transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task BeginTransactionAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Asynchronously commits all changes made in the current transaction.
    /// Silently fails if no transactions has been started yet.
    /// </summary>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    Task CommitTransactionAsync(CancellationToken ct = default);
    
        
    /// <summary>
    /// Attempts to commit the current database transaction asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is a boolean indicating whether the transaction was successfully committed.
    /// </returns>
    Task<bool> TryCommitTransactionAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Asynchronously rolls back all changes made in the current transaction.
    /// Silently fails if no transactions has been started yet.
    /// </summary>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    Task RollbackTransactionAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Attempts to rollback the current database transaction. If no transaction is active, returns false.
    /// </summary>
    /// <returns>
    /// Returns a boolean indicating whether the rollback was successful.
    /// Returns true if the rollback occurred, otherwise returns false.
    /// </returns>
    Task<bool> TryRollbackTransactionAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Retrieves the current InfiniLoreDbContext instance.
    /// </summary>
    /// <returns>The InfiniLoreDbContext instance.</returns>
    T GetDbContext();
}
