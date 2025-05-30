// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddAsync<in T> where T : BasicModel {
    ValueTask<Result> AddAsync(T model, CancellationToken ct = default);
    ValueTask<Result> AddRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
