// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddAsync<in T> where T : BasicModel {
    ValueTask<Shared.Outcome> AddAsync(T model, CancellationToken ct = default);
    ValueTask<Shared.Outcome> AddRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
