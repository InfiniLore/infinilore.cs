// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasDeleteAsync<in T> where T : BasicModel {
    ValueTask<Outcome> DeleteAsync(T model, CancellationToken ct = default);
    ValueTask<Outcome> DeleteByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<Outcome> DeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<Outcome> DeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
