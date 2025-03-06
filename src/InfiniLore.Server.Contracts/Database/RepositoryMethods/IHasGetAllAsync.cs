// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetAllAsync<T> where T : BasicData {
    ValueTask<RepoResult<T[]>> GetAllAsync(CancellationToken ct = default);
    ValueTask<RepoResult<T[]>> GetAllWithAutoIncludeAsync(CancellationToken ct = default);
    ValueTask<RepoResult<T[]>> GetAllReverseAsync(CancellationToken ct = default);
    ValueTask<RepoResult<T[]>> GetAllReverseWithAutoIncludeAsync(CancellationToken ct = default);

    ValueTask<PaginatedRepoResult<T>> GetAllAsync(PaginationInfo pageInfo, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T>> GetAllWithAutoIncludeAsync(PaginationInfo pageInfo, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T>> GetAllReverseAsync(PaginationInfo pageInfo, CancellationToken ct = default);
    ValueTask<PaginatedRepoResult<T>> GetAllReverseWithAutoIncludeAsync(PaginationInfo pageInfo, CancellationToken ct = default);
}
