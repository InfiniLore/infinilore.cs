// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByUserAsync<T> where T : UserData {
    ValueTask<RepoResult<T[]>> GetByUserAsync(Guid userId, QueryConfig config = default, CancellationToken ct = default);
    
    ValueTask<PaginatedRepoResult<T>> GetByUserAsync(Guid userId, PaginationInfo pageInfo, QueryConfig config = default, CancellationToken ct = default);
}
