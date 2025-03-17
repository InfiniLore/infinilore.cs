// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetAllAsync<T> where T : BasicData {
    ValueTask<RepoResult<T[]>> GetAllAsync(QueryConfig config = default, CancellationToken ct = default);

    ValueTask<PaginatedRepoResult<T>> GetAllAsync(PaginationInfo pageInfo,QueryConfig config = default,  CancellationToken ct = default);
}
