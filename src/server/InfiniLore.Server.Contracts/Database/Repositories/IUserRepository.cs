// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Server.Types;
using System.Security.Claims;

namespace InfiniLore.Server.Contracts.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Provides an interface for accessing user data from the database.
///     Under the hood this interface should be implemented with UserManager to actually handle most of its methods.
/// </summary>
public interface IUserRepository {
    /// <summary>
    ///     Attempts to retrieve an <see cref="InfiniLoreUser" /> based on the provided <see cref="ClaimsPrincipal" />.
    /// </summary>
    /// <param name="principal">The claims principal containing the user's identity information.</param>
    /// <param name="ct">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="RepoResult" /> which represents the result of the user retrieval operation,
    ///     containing either a successfully retrieved user or an error message.
    /// </returns>
    ValueTask<RepoResult<InfiniLoreUser>> TryGetByClaimsPrincipalAsync(ClaimsPrincipal principal, CancellationToken ct = default);

    /// <summary>
    ///     Attempts to retrieve a user by a specified user ID.
    /// </summary>
    /// <param name="userId">
    ///     A <c>Guid</c> representing the unique identifier of the user to be retrieved. This can be an instance of
    ///     <c>InfiniLoreUser</c>, a GUID, or a string.
    /// </param>
    /// <param name="ct">
    ///     A <c>CancellationToken</c> that can be used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A <c>ValueTask</c> containing a <c>RepoResult</c> of type <c>InfiniLoreUser</c>, which represents the result of
    ///     the repository operation. The result may indicate success or failure, and contain the user instance if successful.
    /// </returns>
    ValueTask<RepoResult<InfiniLoreUser>> TryGetByIdAsync(Guid userId, CancellationToken ct = default);

    ValueTask<RepoResult<InfiniLoreUser>> UserHasAllRolesAsync(Guid userId, IEnumerable<string> roles, CancellationToken ct = default);
}
