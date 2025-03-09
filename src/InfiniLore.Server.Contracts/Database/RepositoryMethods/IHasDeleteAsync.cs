// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasDeleteAsync<in T> where T : BasicData {
    ValueTask<RepoResult> DeleteAsync(T model, CancellationToken ct = default);
    ValueTask<RepoResult> DeleteByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<RepoResult> DeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<RepoResult> DeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
