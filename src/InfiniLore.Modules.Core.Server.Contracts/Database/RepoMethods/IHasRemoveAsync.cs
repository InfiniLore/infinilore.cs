// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasRemoveAsync<in T> where T : BasicModel {
    ValueTask<RepoOutcome> RemoveAsync(T model, CancellationToken ct = default);
    ValueTask<RepoOutcome> RemoveByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<RepoOutcome> RemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<RepoOutcome> RemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
