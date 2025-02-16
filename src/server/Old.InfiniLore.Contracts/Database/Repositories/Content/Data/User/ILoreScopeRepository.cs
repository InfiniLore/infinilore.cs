// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models.Content.Data.User;
using Old.InfiniLore.Server.Types;

namespace Old.InfiniLore.Contracts.Database.Repositories.Content.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILorescopeRepository : IUserContentRepository<LorescopeModel> {
    ValueTask<RepoResult> IsValidNewNameAsync(Guid userId, string name, CancellationToken ct = default);
}
