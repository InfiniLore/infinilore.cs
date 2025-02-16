// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models;
using Old.InfiniLore.Server.Types;

namespace Old.InfiniLore.Contracts.Database.Repositories.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Defines methods for attempting to update records of type <typeparamref name="T" /> asynchronously in the database.
/// </summary>
/// <typeparam name="T">
/// The type of the content to be updated, which must inherit from <see cref="BasicContent" />.
/// </typeparam>
public interface IHasTryUpdateAsync<T> where T : BasicContent {
    /// Asynchronously attempts to update the specified model in the repository.
    /// <param name="model">The model to be updated, which must be of a type derived from BasicContent.</param>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel the asynchronous operation, if needed. Defaults to
    /// CancellationToken.None.
    /// </param>
    /// <return>
    /// A ValueTask representing the asynchronous operation, which contains a RepoResult indicating the success or
    /// failure of the update operation.
    /// </return>
    ValueTask<RepoResult> TryUpdateAsync(T model, CancellationToken ct = default);

    /// Asynchronously attempts to update the specified model in the repository and returns the result,
    /// including the success status and the updated model if applicable.
    /// <param name="model">The model to be updated, which must be of a type derived from BasicContent.</param>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel the asynchronous operation, if needed. Defaults to
    /// CancellationToken.None.
    /// </param>
    /// <returns>
    /// A ValueTask containing a RepoResult of the specified type, which includes the outcome of the update
    /// operation and the updated model if the operation was successful.
    /// </returns>
    ValueTask<RepoResult<T>> TryUpdateWithResultAsync(T model, CancellationToken ct = default);

    /// Asynchronously attempts to update a collection of models in the repository.
    /// <param name="models">
    /// A collection of models to be updated. Each model must be of a type derived from BasicContent.
    /// </param>
    /// <param name="ct">
    /// A cancellation token to signal the asynchronous operation to cancel if required. Defaults to
    /// CancellationToken.None.
    /// </param>
    /// <return>
    /// A ValueTask representing the asynchronous operation that contains a RepoResult indicating the outcome of
    /// the update operation, whether successful or not.
    /// </return>
    ValueTask<RepoResult> TryUpdateAsync(IEnumerable<T> models, CancellationToken ct = default);
}
