// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByUserAsync<T> where T : UserData {
    ValueTask<Result<T[]>> GetByUserAsync(Guid userId, QueryConfig config = default, CancellationToken ct = default);

    ValueTask<PaginatedResult<T>> GetByUserAsync(Guid userId, PaginationInfo pageInfo, QueryConfig config = default, CancellationToken ct = default);
}
