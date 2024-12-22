// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Types;

namespace InfiniLore.Server.Contracts.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Provides an interface for accessing user data from the database.
///     Under the hood this interface should be implemented with UserManager to actually handle most of its methods.
/// </summary>
public interface IUserRepository {
    ValueTask<RepoResult> UserHasAllRolesAsync(Guid userId, IEnumerable<string> roles, CancellationToken ct = default);
}
