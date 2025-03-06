// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByIdAsync<T> where T : BasicData {
    ValueTask<RepoResult<T>> GetByIdAsync(Guid id, CancellationToken ct = default);
    ValueTask<RepoResult<T>> GetByIdWithAutoIncludeAsync(Guid id, CancellationToken ct = default);
}
