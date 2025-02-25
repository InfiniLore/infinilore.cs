// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasTryRemoveAsync<in T> where T : BasicData {
    ValueTask<RepoResult> TryRemoveAsync(T model, CancellationToken ct = default);
    ValueTask<RepoResult> TryRemoveByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<RepoResult> TryRemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<RepoResult> TryRemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
