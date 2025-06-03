// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasUpdateAsync<in T> where T : BasicModel {
    ValueTask<Shared.Outcome> UpdateAsync(T model, CancellationToken ct = default);
    ValueTask<Shared.Outcome> UpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
