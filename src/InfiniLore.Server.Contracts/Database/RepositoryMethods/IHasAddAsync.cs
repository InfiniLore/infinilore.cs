// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddAsync<in T> where T : IBasicData {
    ValueTask<Result> AddAsync(T model, CancellationToken ct = default);
    ValueTask<Result> AddRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
