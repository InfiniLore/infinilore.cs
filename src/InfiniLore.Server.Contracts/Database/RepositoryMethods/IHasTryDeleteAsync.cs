// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasTryDeleteAsync<in T> where T : BasicData {
    ValueTask<RepoResult> TryDeleteAsync(T model, CancellationToken ct = default);
    ValueTask<RepoResult> TryDeleteByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<RepoResult> TryDeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<RepoResult> TryDeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
