// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------=
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddOrUpdateAsync<in T> where T : BasicModel {
    ValueTask<Shared.Outcome> AddOrUpdateAsync(T model, CancellationToken ct = default);
    ValueTask<Shared.Outcome> AddOrUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
