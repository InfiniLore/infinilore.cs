// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasDeleteAsync<in T> where T : BasicModel {
    ValueTask<Shared.Outcome> DeleteAsync(T model, CancellationToken ct = default);
    ValueTask<Shared.Outcome> DeleteByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<Shared.Outcome> DeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<Shared.Outcome> DeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
