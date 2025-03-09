// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasUpdateAsync<in T> where T : BasicData {
    ValueTask<RepoResult> UpdateAsync(T model, CancellationToken ct = default);
    ValueTask<RepoResult> UpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
