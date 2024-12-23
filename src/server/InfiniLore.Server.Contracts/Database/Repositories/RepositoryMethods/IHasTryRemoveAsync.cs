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
///     Represents a contract for asynchronously attempting to remove entities.
/// </summary>
/// <typeparam name="T">The type of the entity, which must be a subclass of BasicContent.</typeparam>
public interface IHasTryRemoveAsync<in T> where T : BasicContent {
    /// <summary>
    ///     Attempts to remove the specified model asynchronously.
    /// </summary>
    /// <param name="model">The model to be removed.</param>
    /// <param name="ct">Optional. A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing a <see cref="RepoResult" /> indicating the
    ///     outcome of the operation.
    /// </returns>
    ValueTask<RepoResult> TryRemoveAsync(T model, CancellationToken ct = default);

    /// <summary>
    ///     Attempts to remove a range of content models from the repository asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the content model, must inherit from BasicContent.</typeparam>
    /// <param name="models">The collection of content models to be removed.</param>
    /// <param name="ct">Optional. A cancellation token to cancel the operation.</param>
    /// <returns>
    ///     A task representing the asynchronous operation.
    ///     The task result contains a <see cref="RepoResult" /> indicating the success or failure of the remove operation.
    /// </returns>
    ValueTask<RepoResult> TryRemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
