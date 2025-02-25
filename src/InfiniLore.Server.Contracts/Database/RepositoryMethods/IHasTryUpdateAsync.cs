// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasTryUpdateAsync<in T> where T : BasicData {
    ValueTask<RepoResult> TryUpdateAsync(T model, CancellationToken ct = default);
    ValueTask<RepoResult> TryUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
