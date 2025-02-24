// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Account;

namespace InfiniLore.Server.Contracts.Database.Repositories.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserRepository : IBasicDataRepository<InfiniLoreUser> {
    ValueTask<RepoResult<InfiniLoreUser>> TryGetByAuth0IdAsync(string auth0Id, CancellationToken ct = default);
    ValueTask<RepoResult<InfiniLoreUser>> TryGetByAuth0IdWithAutoIncludeAsync(string auth0Id, CancellationToken ct = default);
}
