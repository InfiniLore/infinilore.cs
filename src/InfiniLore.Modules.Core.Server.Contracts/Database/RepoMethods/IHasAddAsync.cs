// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddAsync<in T> where T : BasicModel {
    ValueTask<Outcome> AddAsync(T model, CancellationToken ct = default);
    ValueTask<Outcome> AddRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
