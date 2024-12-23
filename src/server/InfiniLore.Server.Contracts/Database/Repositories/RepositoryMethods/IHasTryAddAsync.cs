// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Server.Types;

namespace InfiniLore.Server.Contracts.Database.Repositories.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Defines methods for attempting to add content asynchronously to a repository.
/// </summary>
/// <typeparam name="T">The type of content to be added, which must be derived from <see cref="BasicContent" />.</typeparam>
public interface IHasTryAddAsync<T> where T : BasicContent {
    /// Attempts to asynchronously add a model of type T to the repository.
    /// If successful, returns a result indicating success; otherwise, returns a result indicating failure.
    /// <param name="model">The model of type T that is to be added to the repository.</param>
    /// <param name="ct">A CancellationToken for cancelling the operation if needed. Defaults to CancellationToken.None.</param>
    /// <returns>
    ///     A ValueTask containing a RepoResult which indicates the outcome of the add operation,
    ///     either a Success or a Failure result.
    /// </returns>
    ValueTask<RepoResult> TryAddAsync(T model, CancellationToken ct = default);

    /// Attempts to add a new model to the database. If successful, returns a result containing the added model;
    /// otherwise, returns a failure result.
    /// <param name="model">The model instance to be added to the database.</param>
    /// <param name="ct">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a Result containing either:
    ///     a success result with the added model, or a failure result with the error message.
    /// </returns>
    ValueTask<RepoResult<T>> TryAddWithResultAsync(T model, CancellationToken ct = default);

    /// Asynchronously attempts to add a range of models to the repository. It returns a result indicating the
    /// success or failure of the operation.
    /// <param name="models">The collection of models to add to the repository.</param>
    /// <param name="ct">Optional. A token to monitor for cancellation requests.</param>
    /// <return>
    ///     A task that represents the asynchronous operation. The task result contains a <c>RepoResult</c>
    ///     indicating the outcome of the operation.
    /// </return>
    ValueTask<RepoResult> TryAddRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
