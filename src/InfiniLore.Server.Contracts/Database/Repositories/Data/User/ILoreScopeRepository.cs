// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Data.User;

namespace InfiniLore.Server.Contracts.Database.Repositories.Data.User;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopeRepository : IUserDataRepository<LoreScope> {
    ValueTask<RepoResult> IsLoreScopeNameTakenAsync(string loreScopeName, Guid ownerId, CancellationToken ct = default);
    ValueTask<RepoResult> IsLoreScopeNameNotTakenAsync(string loreScopeName, Guid ownerId, CancellationToken ct = default);
}
