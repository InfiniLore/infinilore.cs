// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasUpdateAsync<in T> where T : BasicModel {
    ValueTask<Result> UpdateAsync(T model, CancellationToken ct = default);
    ValueTask<Result> UpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
