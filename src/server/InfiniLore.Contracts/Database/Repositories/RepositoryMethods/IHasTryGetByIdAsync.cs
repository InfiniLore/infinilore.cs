// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Server.Types;

namespace InfiniLore.Contracts.Database.Repositories.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents an asynchronous method for attempting to retrieve an entity by its identifier.
/// </summary>
/// <typeparam name="T">
///     The type of entity being retrieved, which must derive from <see cref="BasicContent" />.
/// </typeparam>
public interface IHasTryGetByIdAsync<T> where T : BasicContent {
    /// Asynchronously attempts to retrieve an entity by its unique identifier from the repository.
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <param name="ct">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains a RepoResult object which can either be a successful retrieval of the entity or a failure
    ///     message.
    /// </returns>
    ValueTask<RepoResult<T>> TryGetByIdAsync(Guid id, CancellationToken ct = default);
}
