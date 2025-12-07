// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Pagination;

namespace InfiniLore.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBaseModelRepository<TModel> where TModel : class {
    ValueTask<TModel?> GetByIdAsync(Guid id, QueryConfig config = QueryConfig.None, CancellationToken ct = default);
    ValueTask<PaginatedData<TModel>> GetAllAsync(PaginationData pagination, QueryConfig config = QueryConfig.None, CancellationToken ct = default);
    ValueTask<bool> AddAsync(TModel model, CancellationToken ct = default);
    ValueTask<bool> AnyAsync(QueryConfig config = QueryConfig.None, CancellationToken ct = default);
}
