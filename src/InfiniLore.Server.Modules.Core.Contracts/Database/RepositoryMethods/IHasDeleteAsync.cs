// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database.Models;

namespace InfiniLore.Server.Modules.Core.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasDeleteAsync<in T> where T : IBasicData {
    ValueTask<Result> DeleteAsync(T model, CancellationToken ct = default);
    ValueTask<Result> DeleteByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<Result> DeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<Result> DeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
