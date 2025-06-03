// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasRemoveAsync<in T> where T : BasicModel {
    ValueTask<Result> RemoveAsync(T model, CancellationToken ct = default);
    ValueTask<Result> RemoveByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<Result> RemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<Result> RemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
