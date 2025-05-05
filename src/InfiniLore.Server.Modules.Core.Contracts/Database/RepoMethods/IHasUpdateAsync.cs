// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Server.Modules.Core.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasUpdateAsync<in T> where T : BasicModel {
    ValueTask<Result> UpdateAsync(T model, CancellationToken ct = default);
    ValueTask<Result> UpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
