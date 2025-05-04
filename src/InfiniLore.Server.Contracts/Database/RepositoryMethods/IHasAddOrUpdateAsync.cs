// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------=
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddOrUpdateAsync<in T> where T : IBasicData {
    ValueTask<Result> AddOrUpdateAsync(T model, CancellationToken ct = default);
    ValueTask<Result> AddOrUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
