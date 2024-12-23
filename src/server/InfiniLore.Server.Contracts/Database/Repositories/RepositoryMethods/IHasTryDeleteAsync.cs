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
///     Provides asynchronous methods for attempting to delete entities of type <typeparamref name="T" /> from a
///     repository.
/// </summary>
/// <typeparam name="T">The type of entities to delete, which must derive from <see cref="BasicContent" />.</typeparam>
public interface IHasTryDeleteAsync<in T> where T : BasicContent {
    /// <summary>
    ///     Attempts to delete the specified model asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the model which extends BasicContent.</typeparam>
    /// <param name="model">The model instance to be deleted.</param>
    /// <param name="ct">Optional. A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A <see cref="RepoResult" /> indicating the result of the delete operation, which could be a success or
    ///     failure.
    /// </returns>
    ValueTask<RepoResult> TryDeleteAsync(T model, CancellationToken ct = default);

    /// <summary>
    ///     Attempts to delete a range of models asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the model which extends BasicContent.</typeparam>
    /// <param name="models">A collection of models to be deleted.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains a <see cref="RepoResult" />
    ///     indicating success or failure of the delete operation.
    /// </returns>
    ValueTask<RepoResult> TryDeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
