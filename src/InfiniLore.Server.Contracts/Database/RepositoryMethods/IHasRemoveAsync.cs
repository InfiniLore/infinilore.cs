// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasRemoveAsync<in T> where T : BasicData {
    ValueTask<Result> RemoveAsync(T model, CancellationToken ct = default);
    ValueTask<Result> RemoveByIdAsync(Guid id, CancellationToken ct = default);

    ValueTask<Result> RemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
    ValueTask<Result> RemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
