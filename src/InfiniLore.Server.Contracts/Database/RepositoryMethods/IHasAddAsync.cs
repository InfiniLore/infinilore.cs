// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasAddAsync<in T> where T : BasicData {
    ValueTask<RepoResult> AddAsync(T model, CancellationToken ct = default);
    ValueTask<RepoResult> AddRangeAsync(IEnumerable<T> models, CancellationToken ct = default);
}
