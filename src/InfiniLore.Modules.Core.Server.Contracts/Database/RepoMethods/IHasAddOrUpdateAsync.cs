// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------=
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddOrUpdateAsync<in T> where T : BasicModel {
    ValueTask<RepoOutcome> AddOrUpdateAsync(T model, CancellationToken ct = default);
    ValueTask<RepoOutcome> AddOrUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
