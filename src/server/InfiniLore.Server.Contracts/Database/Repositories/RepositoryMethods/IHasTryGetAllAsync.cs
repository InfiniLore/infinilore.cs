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
///     Provides asynchronous methods to retrieve collections of content entities from a repository.
/// </summary>
/// <typeparam name="T">
///     The type of content entity, which must inherit from <see cref="BaseContent" />.
/// </typeparam>
public interface IHasTryGetAllAsync<T> where T : BaseContent {
    /// <summary>
    ///     Asynchronously attempts to retrieve all available items of type <typeparamref name="T" /> from the repository.
    /// </summary>
    /// <typeparam name="T">The type of content to retrieve, constrained to <see cref="BaseContent" />.</typeparam>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A <see cref="ValueTask{RepoResult{T[]}}" /> representing the asynchronous operation, which,
    ///     on completion, returns a <see cref="RepoResult" /> containing an array of the retrieved items
    ///     if successful, or an error message if the operation fails.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetAllAsync(CancellationToken ct = default);

    /// <summary>
    ///     Attempts to retrieve all items of type <typeparamref name="T" /> asynchronously, with pagination support.
    /// </summary>
    /// <typeparam name="T">The type of items to be retrieved, which must derive from <see cref="BaseContent" />.</typeparam>
    /// <param name="pageInfo">The pagination information indicating the page number and the number of items per page.</param>
    /// <param name="ct">Cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> containing a <see cref="RepoResult{T[]}" /> which holds the array of items
    ///     of type <typeparamref name="T" /> on success, or an error message on failure.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetAllASync(PaginationInfo pageInfo, CancellationToken ct = default);
}
