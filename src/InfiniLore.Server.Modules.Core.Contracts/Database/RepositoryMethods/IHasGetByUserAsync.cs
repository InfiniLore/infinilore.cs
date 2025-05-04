// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database.Models;

namespace InfiniLore.Server.Modules.Core.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByUserAsync<T> where T : IUserData {
    ValueTask<Result<T[]>> GetByUserAsync(Guid userId, QueryConfig config = default, CancellationToken ct = default);

    ValueTask<PaginatedResult<T>> GetByUserAsync(Guid userId, PaginationInfo pageInfo, QueryConfig config = default, CancellationToken ct = default);
}
