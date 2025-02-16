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
/// Provides asynchronous methods to retrieve user-specific data stored in the repository.
/// </summary>
/// <typeparam name="T">A type that inherits from <see cref="UserContent"/>.</typeparam>
public interface IHasTryGetByUserAsync<T> where T : UserContent {
    /// <summary>
    /// Attempts to retrieve user content associated with a specified user identifier.
    /// </summary>
    /// <param name="userId">The identifier of the user whose content is being retrieved.</param>
    /// <param name="ct">Optional. A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}" /> that represents the asynchronous operation.
    /// The task result contains a <see cref="RepoResult" /> indicating the success or failure of the operation.
    /// On success, it contains an array of the requested user content.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByUserAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Attempts to asynchronously retrieve user content associated with a specified user identifier.
    /// </summary>
    /// <typeparam name="T">The type of user content to be retrieved.</typeparam>
    /// <param name="userId">
    /// The unique identifier of the user whose content is being retrieved.
    /// </param>
    /// <param name="pageInfo">
    /// Pagination details, including page number and page size, for retrieving a subset of user content.
    /// </param>
    /// <param name="ct">
    /// An optional <see cref="CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}" /> representing the asynchronous operation. The result contains a
    /// <see cref="RepoResult{T}" /> which holds an array of the user content if the operation is successful.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByUserAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default);

    /// <summary>
    /// Attempts to retrieve user content associated with a specific owner and accessible by a specified accessor, based on the required access level.
    /// </summary>
    /// <typeparam name="T">The type of user content to be retrieved.</typeparam>
    /// <param name="ownerId">The identifier of the owner of the user content.</param>
    /// <param name="accessorId">The identifier of the accessor attempting to access the user content.</param>
    /// <param name="level">The access level required for the accessor to retrieve the content.</param>
    /// <param name="ct">Optional. A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}" /> that represents the asynchronous operation.
    /// The task result contains a <see cref="RepoResult" /> indicating the success or failure of the operation.
    /// On success, it contains an array of the requested user content.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByUserWithUserAccessAsync(Guid ownerId, Guid accessorId, AccessKind level, CancellationToken ct = default);

    /// <summary>
    /// Attempts to retrieve user content based on the specified ownership and access details, applying pagination and access level restrictions.
    /// </summary>
    /// <typeparam name="T">The type of user content being retrieved.</typeparam>
    /// <param name="ownerId">The unique identifier of the user who owns the content.</param>
    /// <param name="accessorId">The unique identifier of the user attempting to access the content.</param>
    /// <param name="level">The access level required for the accessor to retrieve the content.</param>
    /// <param name="pageInfo">Pagination details, including the page number and size.</param>
    /// <param name="ct">Optional. A cancellation token to signal cancellation of the operation.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}" /> representing the result of the asynchronous operation. The task result contains
    /// a <see cref="RepoResult{T}" /> that indicates whether the retrieval was successful. On success, it includes an array
    /// of the retrieved user content.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByUserWithUserAccessAsync(Guid ownerId, Guid accessorId, AccessKind level, PaginationInfo pageInfo, CancellationToken ct = default);
}
