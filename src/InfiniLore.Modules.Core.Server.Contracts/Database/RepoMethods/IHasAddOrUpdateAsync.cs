// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------=
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddOrUpdateAsync<in T> where T : BasicModel {
    ValueTask<Outcome> AddOrUpdateAsync(T model, CancellationToken ct = default);
    ValueTask<Outcome> AddOrUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
