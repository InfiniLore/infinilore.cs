// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Contracts.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Implementation of the unit of work pattern specific to InfiniLore database context.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // ----------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Saves all changes made in this context to the database asynchronously.
    /// </summary>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous save operation. The task result contains the number of state entries
    ///     written to the database.
    /// </returns>
    ValueTask SaveChangesAsync(CancellationToken ct = default);

    /// <summary>
    ///     Attempts to save all changes made in this context to the database.
    /// </summary>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a boolean value indicating whether
    ///     the commit was successful.
    /// </returns>
    ValueTask<bool> TryCommitTransactionAsync(CancellationToken ct = default);

    /// <summary>
    ///     Begins a new database transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    ValueTask<bool> TryCreateTransactionAsync(CancellationToken ct = default);

    /// <summary>
    ///     Attempts to rollback the current database transaction. If no transaction is active, returns false.
    /// </summary>
    /// <returns>
    ///     Returns a boolean indicating whether the rollback was successful.
    ///     Returns true if the rollback occurred, otherwise returns false.
    /// </returns>
    ValueTask<bool> TryRollbackTransactionAsync(CancellationToken ct = default);

    /// <summary>
    ///     Asynchronously attempts to roll back the transaction to a specified savepoint.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous rollback operation. The task result contains a boolean value
    ///     indicating whether the rollback to the savepoint was successful.
    /// </returns>
    ValueTask<bool> TryRollbackToSavepointAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    ///     Asynchronously creates a savepoint in the current database transaction.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask<bool> TryCreateSavepointAsync(Guid id, CancellationToken ct = default);
}
