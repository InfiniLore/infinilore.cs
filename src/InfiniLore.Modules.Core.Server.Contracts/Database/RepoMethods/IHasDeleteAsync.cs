// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasDeleteAsync<in T> where T : BasicModel {
    ValueTask<Result> DeleteAsync(T model, CancellationToken ct = default);
    ValueTask<Result> DeleteByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<Result> DeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<Result> DeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
