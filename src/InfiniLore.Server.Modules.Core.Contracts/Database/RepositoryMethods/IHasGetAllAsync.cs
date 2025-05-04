// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database.Models;

namespace InfiniLore.Server.Modules.Core.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetAllAsync<T> where T : IBasicData {
    ValueTask<Result<T[]>> GetAllAsync(QueryConfig config = default, CancellationToken ct = default);

    ValueTask<PaginatedResult<T>> GetAllAsync(PaginationInfo pageInfo, QueryConfig config = default, CancellationToken ct = default);
}
