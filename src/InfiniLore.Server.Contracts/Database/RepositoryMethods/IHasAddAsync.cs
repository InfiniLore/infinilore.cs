// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddAsync<in T> where T : BasicData {
    ValueTask<Result> AddAsync(T model, CancellationToken ct = default);
    ValueTask<Result> AddRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
