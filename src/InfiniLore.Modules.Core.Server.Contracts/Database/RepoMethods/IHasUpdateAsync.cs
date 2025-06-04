// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasUpdateAsync<in T> where T : BasicModel {
    ValueTask<Outcome> UpdateAsync(T model, CancellationToken ct = default);
    ValueTask<Outcome> UpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
