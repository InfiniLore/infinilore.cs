// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Server.Modules.Core.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddAsync<in T> where T : BasicModel {
    ValueTask<Result> AddAsync(T model, CancellationToken ct = default);
    ValueTask<Result> AddRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
