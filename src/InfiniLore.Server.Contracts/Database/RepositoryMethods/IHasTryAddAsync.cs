// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasTryAddAsync<in T> where T : BasicData {
    ValueTask<RepoResult> TryAddAsync(T model, CancellationToken ct = default);
    ValueTask<RepoResult> TryAddRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
