// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasUpdateAsync<in T> where T : BasicModel {
    ValueTask<RepoOutcome> UpdateAsync(T model, CancellationToken ct = default);
    ValueTask<RepoOutcome> UpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
