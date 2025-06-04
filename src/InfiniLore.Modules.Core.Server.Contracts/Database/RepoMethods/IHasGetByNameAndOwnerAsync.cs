// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared.Database;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByNameAndOwnerAsync<T> where T : BasicModel, IHasName, IHasOwnerId {
    ValueTask<RepoOutcome<T>> GetByNameAndOwnerAsync(string name, Guid ownerId, QueryConfig config = default, CancellationToken ct = default);
}
