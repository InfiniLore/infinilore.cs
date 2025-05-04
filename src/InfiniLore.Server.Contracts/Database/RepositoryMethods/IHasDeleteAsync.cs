// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasDeleteAsync<in T> where T : IBasicData {
    ValueTask<Result> DeleteAsync(T model, CancellationToken ct = default);
    ValueTask<Result> DeleteByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<Result> DeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<Result> DeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
