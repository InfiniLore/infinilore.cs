// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database.Models;

namespace InfiniLore.Server.Modules.Core.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddAsync<in T> where T : IBasicData {
    ValueTask<Result> AddAsync(T model, CancellationToken ct = default);
    ValueTask<Result> AddRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
