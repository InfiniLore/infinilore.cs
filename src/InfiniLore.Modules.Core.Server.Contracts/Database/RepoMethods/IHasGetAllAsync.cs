// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetAllAsync<T> where T : BasicModel {
    ValueTask<Outcome<T[]>> GetAllAsync(QueryConfig config = default, CancellationToken ct = default);

    ValueTask<PaginatedOutcome<T>> GetAllAsync(Pagination pageInfo, QueryConfig config = default, CancellationToken ct = default);
}
