// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetAllAsync<T> where T : BasicModel {
    ValueTask<RepoOutcome<T[]>> GetAllAsync(QueryConfig config = default, CancellationToken ct = default);

    ValueTask<PaginatedRepoOutcome<T>> GetAllAsync(Pagination pageInfo, QueryConfig config = default, CancellationToken ct = default);
}
