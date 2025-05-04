// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database.Models;

namespace InfiniLore.Server.Modules.Core.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasUpdateAsync<in T> where T : IBasicData {
    ValueTask<Result> UpdateAsync(T model, CancellationToken ct = default);
    ValueTask<Result> UpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
