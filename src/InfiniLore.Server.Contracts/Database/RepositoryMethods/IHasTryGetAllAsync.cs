// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasTryGetAllAsync<T> where T : BasicData {
    ValueTask<RepoResult<T[]>> TryGetAllAsync(CancellationToken ct = default);
    ValueTask<RepoResult<T[]>> TryGetAllWithAutoIncludeAsync(CancellationToken ct = default);
    ValueTask<RepoResult<T[]>> TryGetAllReverseAsync(CancellationToken ct = default);
    ValueTask<RepoResult<T[]>> TryGetAllReverseWithAutoIncludeAsync(CancellationToken ct = default);

    ValueTask<PaginatedRepoResult<T>> TryGetAllAsync(PaginationInfo pageInfo, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T>> TryGetAllWithAutoIncludeAsync(PaginationInfo pageInfo, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T>> TryGetAllReverseAsync(PaginationInfo pageInfo, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T>> TryGetAllReverseWithAutoIncludeAsync(PaginationInfo pageInfo, CancellationToken ct = default);
}
