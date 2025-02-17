// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasTryGetByUserAsync<T> where T : UserData {
    ValueTask<RepoResult<T[]>> TryGetByUserAsync(Guid userId, CancellationToken ct = default);
    ValueTask<RepoResult<T[]>> TryGetByUserWithAutoIncludeAsync(Guid userId, CancellationToken ct = default);
    ValueTask<RepoResult<T[]>> TryGetByUserReverseAsync(Guid userId, CancellationToken ct = default);
    ValueTask<RepoResult<T[]>> TryGetByUserReverseWithAutoIncludeAsync(Guid userId, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T[]>> TryGetByUserAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T[]>> TryGetByUserWithAutoIncludeAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T[]>> TryGetByUserReverseAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T[]>> TryGetByUserReverseWithAutoIncludeAsync(Guid userId, PaginationInfo pageInfo, CancellationToken ct = default);
}
