// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Database.RepositoryMethods;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByLoreScopeAsync<T> where T : IUserData {
    ValueTask<Result<T[]>> GetByLoreScopeAsync(Guid loreScopeId, QueryConfig config = default, CancellationToken ct = default);
    ValueTask<PaginatedResult<T>> GetByLoreScopeAsync(Guid loreScopeId, PaginationInfo pageInfo, QueryConfig config = default, CancellationToken ct = default);
}
