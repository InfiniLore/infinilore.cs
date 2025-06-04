// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddAsync<in T> where T : BasicModel {
    ValueTask<RepoOutcome> AddAsync(T model, CancellationToken ct = default);
    ValueTask<RepoOutcome> AddRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
