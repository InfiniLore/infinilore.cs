// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasTryGetByIdAsync<T> where T : BasicData {
    ValueTask<RepoResult<T>> TryGetByIdAsync(Guid id, CancellationToken ct = default);
    ValueTask<RepoResult<T>> TryGetByIdWithAutoIncludeAsync(Guid id, CancellationToken ct = default);
}
