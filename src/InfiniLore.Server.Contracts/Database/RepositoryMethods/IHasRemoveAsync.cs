// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasRemoveAsync<in T> where T : BasicData {
    ValueTask<RepoResult> RemoveAsync(T model, CancellationToken ct = default);
    ValueTask<RepoResult> RemoveByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<RepoResult> RemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<RepoResult> RemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
