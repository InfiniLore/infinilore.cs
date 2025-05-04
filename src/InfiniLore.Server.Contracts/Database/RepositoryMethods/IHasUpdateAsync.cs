// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasUpdateAsync<in T> where T : IBasicData {
    ValueTask<Result> UpdateAsync(T model, CancellationToken ct = default);
    ValueTask<Result> UpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
