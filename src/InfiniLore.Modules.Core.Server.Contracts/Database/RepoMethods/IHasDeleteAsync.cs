// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasDeleteAsync<in T> where T : BasicModel {
    ValueTask<RepoOutcome> DeleteAsync(T model, CancellationToken ct = default);
    ValueTask<RepoOutcome> DeleteByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<RepoOutcome> DeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<RepoOutcome> DeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
