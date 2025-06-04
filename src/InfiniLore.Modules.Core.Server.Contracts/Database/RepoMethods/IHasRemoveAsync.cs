// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasRemoveAsync<in T> where T : BasicModel {
    ValueTask<Outcome> RemoveAsync(T model, CancellationToken ct = default);
    ValueTask<Outcome> RemoveByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<Outcome> RemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<Outcome> RemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
