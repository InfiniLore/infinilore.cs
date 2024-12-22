// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using InfiniLore.Server.Types;

namespace InfiniLore.Server.Contracts.Database.Repositories.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Defines the contract for attempting to permanently remove all items associated with a specific user.
/// </summary>
public interface IHasTryPermanentRemoveAllForUserAsync {
    /// <summary>
    ///     Attempts to permanently remove all records associated with the specified user.
    /// </summary>
    /// <param name="userId">
    ///     The union representing the user whose records are to be removed. Can be an InfiniLoreUser,
    ///     Guid, or string.
    /// </param>
    /// <param name="ct">An optional cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing a <see cref="RepoResult" /> which indicates
    ///     success or failure of the operation.
    /// </returns>
    ValueTask<RepoResult> TryPermanentRemoveAllForUserAsync(Guid userId, CancellationToken ct = default);
}
