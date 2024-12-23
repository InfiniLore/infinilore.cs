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
/// Provides asynchronous methods to retrieve user-specific data stored in the repository.
/// </summary>
/// <typeparam name="T">A type that inherits from <see cref="UserContent" />.</typeparam>
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
    /// A <see cref="ValueTask{TResult}" /> that represents the asynchronous operation. The task result contains a
    /// <see cref="RepoResult" />
    /// indicating the success or failure of the operation. On success, it contains an array of the requested user content.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByUserWithUserAccessAsync(Guid ownerId, Guid accessorId, AccessKind level, CancellationToken ct = default);

    /// <summary>
    /// Attempts to retrieve user content by specifying the owner, accessor, access level, and pagination details.
    /// </summary>
    /// <typeparam name="T">The type of user content to be retrieved.</typeparam>
    /// <param name="ownerId">The identifier for the user owning the content.</param>
    /// <param name="accessorId">The identifier for the user attempting to access the content.</param>
    /// <param name="level">The required access level for the accessor to successfully retrieve the content.</param>
    /// <param name="pageInfo">Details for pagination including the page number and page size.</param>
    /// <param name="ct">Optional. A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}" /> representing the asynchronous operation. The task result contains a
    /// <see cref="RepoResult{T}" /> object, where T is the type of user content. The result indicates success or failure
    /// of the operation and, if successful, includes the requested user content.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByUserWithUserAccessAsync(Guid ownerId, Guid accessorId, AccessKind level, PaginationInfo pageInfo, CancellationToken ct = default);
}
