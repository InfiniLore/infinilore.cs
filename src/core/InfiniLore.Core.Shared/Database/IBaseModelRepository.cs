// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Outcomes;
using InfiniLore.Core.Pagination;

namespace InfiniLore.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBaseModelRepository<TModel> where TModel : class {
    ValueTask<RepoOutcome<TModel>> GetByIdAsync(Guid id, QueryConfig config = QueryConfig.None, CancellationToken ct = default);
    ValueTask<PaginatedRepoOutcome<TModel>> GetAllAsync(PaginationData pagination, QueryConfig config = QueryConfig.None, CancellationToken ct = default);
    ValueTask<RepoOutcome> AddAsync(TModel model, CancellationToken ct = default);
    ValueTask<bool> AnyAsync(QueryConfig config = QueryConfig.None, CancellationToken ct = default);
}
