// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByUserAsync<T> where T : UserData {
    ValueTask<RepoResult<T[]>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    ValueTask<RepoResult<T[]>> GetByUserWithAutoIncludeAsync(Guid userId, CancellationToken ct = default);
    ValueTask<RepoResult<T[]>> GetByUserReverseAsync(Guid userId, CancellationToken ct = default);
    ValueTask<RepoResult<T[]>> GetByUserReverseWithAutoIncludeAsync(Guid userId, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T>> GetByUserAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T>> GetByUserWithAutoIncludeAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T>> GetByUserReverseAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T>> GetByUserReverseWithAutoIncludeAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default);
}
