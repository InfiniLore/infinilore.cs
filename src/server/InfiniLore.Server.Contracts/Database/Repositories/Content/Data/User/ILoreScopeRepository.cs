// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Data.User;
using InfiniLore.Server.Types;

namespace InfiniLore.Server.Contracts.Database.Repositories.Content.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILorescopeRepository : IUserContentRepository<LorescopeModel> {
    ValueTask<RepoResult> IsValidNewNameAsync(Guid userId, string name, CancellationToken ct = default);
}
