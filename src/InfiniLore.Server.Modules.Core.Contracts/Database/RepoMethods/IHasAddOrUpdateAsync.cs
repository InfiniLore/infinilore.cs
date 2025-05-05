// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------=
using AterraEngine.Unions;

namespace InfiniLore.Server.Modules.Core.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddOrUpdateAsync<in T> where T : BasicModel {
    ValueTask<Result> AddOrUpdateAsync(T model, CancellationToken ct = default);
    ValueTask<Result> AddOrUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
