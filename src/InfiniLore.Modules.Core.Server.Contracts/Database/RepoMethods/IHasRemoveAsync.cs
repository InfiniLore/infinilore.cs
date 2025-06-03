// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasRemoveAsync<in T> where T : BasicModel {
    ValueTask<Shared.Outcome> RemoveAsync(T model, CancellationToken ct = default);
    ValueTask<Shared.Outcome> RemoveByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<Shared.Outcome> RemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<Shared.Outcome> RemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
