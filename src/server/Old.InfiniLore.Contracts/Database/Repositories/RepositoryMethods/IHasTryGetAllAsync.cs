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
/// Defines asynchronous operations to retrieve all instances of a specified content type from a repository.
/// </summary>
/// <typeparam name="T">
/// The type of content entity, constrained to be a class inheriting from <see cref="BasicContent" />.
/// </typeparam>
public interface IHasTryGetAllAsync<T> where T : BasicContent {
    /// <summary>
    /// Asynchronously attempts to retrieve all available items of type <typeparamref name="T" /> from the repository.
    /// </summary>
    /// <typeparam name="T">The type of content to retrieve, constrained to <see cref="BasicContent" />.</typeparam>
    /// <param name="reverse">Determines if the items should be retrieved in reverse order.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="ValueTask{RepoResult}" /> representing the asynchronous operation, which,
    /// on completion, returns a <see cref="RepoResult" /> containing an array of the retrieved items
    /// if successful, or an error message if the operation fails.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetAllAsync(bool reverse = false, CancellationToken ct = default);

    /// <summary>
    /// Attempts to asynchronously retrieve all items of type <typeparamref name="T" /> with pagination support.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve, constrained to <see cref="BasicContent" />.</typeparam>
    /// <param name="pageInfo">The pagination information that specifies the page number and the size of the page.</param>
    /// <param name="reverse">A boolean flag indicating whether the order of items should be reversed.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}" /> that represents the asynchronous operation. This task, upon completion,
    /// returns a <see cref="PaginatedRepoResult{T}" /> containing the paginated result of items if successful, or
    /// an error message if the operation fails.
    /// </returns>
    ValueTask<PaginatedRepoResult<T>> TryGetAllAsync(PaginationInfo pageInfo, bool reverse = false, CancellationToken ct = default);
}
