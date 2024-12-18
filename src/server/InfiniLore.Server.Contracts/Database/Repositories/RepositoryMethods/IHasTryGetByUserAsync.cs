// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Server.Contracts.Types;

namespace InfiniLore.Server.Contracts.Database.Repositories.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Provides asynchronous methods to retrieve user-specific data stored in the repository.
/// </summary>
/// <typeparam name="T">A type that inherits from <see cref="UserContent" />.</typeparam>
public interface IHasTryGetByUserAsync<T> where T : UserContent {
    /// <summary>
    ///     Attempts to retrieve user content associated with a specified user identifier.
    /// </summary>
    /// <typeparam name="T">The type of user content to be retrieved.</typeparam>
    /// <param name="userUnion">The user identifier used to locate the associated user content.</param>
    /// <param name="ct">Optional. A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the asynchronous operation. The task result contains a
    ///     <see cref="RepoResult{T}" />
    ///     indicating the success or failure of the retrieval operation. On success, it contains an array of the requested
    ///     user content.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByUserAsync(UserIdUnion userUnion, CancellationToken ct = default);

    /// <summary>
    ///     Asynchronously attempts to retrieve an array of user content associated with a specified user.
    /// </summary>
    /// <param name="userUnion">
    ///     A union struct that represents the identifier of the user whose content is being retrieved.
    /// </param>
    /// <param name="pageInfo">
    ///     An instance of <see cref="PaginationInfo" /> that contains paging information for the query.
    /// </param>
    /// <param name="ct">
    ///     An optional <see cref="CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> representing the asynchronous operation,
    ///     with a result of type <see cref="RepoResult{T}" />, which contains the array of user content.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByUserAsync(UserIdUnion userUnion, PaginationInfo pageInfo, CancellationToken ct = default);

    /// <summary>
    ///     Attempts to retrieve user content with specific access permissions.
    /// </summary>
    /// <typeparam name="T">The type of the user content.</typeparam>
    /// <param name="ownerUnion">The union representation of the owner's user identifier.</param>
    /// <param name="accessorUnion">The union representation of the accessor's user identifier.</param>
    /// <param name="level">The level of access required to retrieve the content.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing a repository result with an array of user
    ///     content or an error message.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByUserWithUserAccessAsync(UserIdUnion ownerUnion, UserIdUnion accessorUnion, AccessKind level, CancellationToken ct = default);

    /// <summary>
    ///     Attempts to retrieve user content by the specified owner and accessor with a given access level and pagination
    ///     information.
    /// </summary>
    /// <typeparam name="T">The type of user content.</typeparam>
    /// <param name="ownerUnion">A union representing the user who owns the content.</param>
    /// <param name="accessorUnion">A union representing the user trying to access the content.</param>
    /// <param name="level">The required access level for the accessor.</param>
    /// <param name="pageInfo">The pagination information for retrieving the content.</param>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing the result of the retrieval
    ///     in a <see cref="RepoResult{T}" /> structure where <c>T</c> is the type of user content.
    /// </returns>
    ValueTask<RepoResult<T[]>> TryGetByUserWithUserAccessAsync(UserIdUnion ownerUnion, UserIdUnion accessorUnion, AccessKind level, PaginationInfo pageInfo, CancellationToken ct = default);
}
