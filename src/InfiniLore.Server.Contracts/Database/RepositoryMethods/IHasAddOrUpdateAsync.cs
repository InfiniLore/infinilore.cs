// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------=
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddOrUpdateAsync<in T> where T : BasicData {
    ValueTask<RepoResult> AddOrUpdateAsync(T model, CancellationToken ct = default);
    ValueTask<RepoResult> AddOrUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
