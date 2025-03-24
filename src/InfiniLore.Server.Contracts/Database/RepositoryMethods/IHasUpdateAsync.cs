// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasUpdateAsync<in T> where T : BasicData {
    ValueTask<Result> UpdateAsync(T model, CancellationToken ct = default);
    ValueTask<Result> UpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
