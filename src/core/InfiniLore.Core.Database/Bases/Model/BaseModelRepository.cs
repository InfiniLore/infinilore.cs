// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Outcomes;
using InfiniLore.Core.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseModelRepository<TModel> : UnitOfWorkRepository<InfiniLoreDb>
    where TModel : BaseModel
{
    protected virtual IQueryable<TModel> OptionalInclude(IQueryable<TModel> query) => query;
    protected virtual IQueryable<TModel> AlwaysInclude(IQueryable<TModel> query) => query;

    protected IQueryable<TModel> GetConfiguredQueryable(QueryConfig config) 
        => GetConfiguredQueryable( GetCachedDbSet<TModel>(), config);
    
    protected IQueryable<TModel> GetConfiguredQueryable(IQueryable<TModel> baseQuery, QueryConfig config) => baseQuery
        .AsNoTracking()
        .With(AlwaysInclude)
        .ConditionalWith(config.HasFlagFast(QueryConfig.IncludeOptionalReferences), OptionalInclude)
        .ConditionalReverse(config.HasFlagFast(QueryConfig.Reversed))
        .ConditionalWith(config.HasFlagFast(QueryConfig.IncludeSoftDeleted), query => query.IgnoreQueryFilters())
        .ConditionalWith(config.HasFlagFast(QueryConfig.SortByCreatedAt | QueryConfig.SortByModifiedAt), query => query.OrderBy(model => model.CreatedAt).ThenBy(model => model.ModifiedAt))
        .ConditionalOrderBy(config.HasFlagFast(QueryConfig.SortByCreatedAt), model => model.CreatedAt)
        .ConditionalOrderBy(config.HasFlagFast(QueryConfig.SortByModifiedAt), model => model.ModifiedAt)
        .ConditionalOrderBy(!config.HasFlagFast(QueryConfig.SortByCreatedAt) && !config.HasFlagFast(QueryConfig.SortByModifiedAt), model => model.Id)
    ;
    
    
    protected IQueryable<TModel> GetPaginatedQueryable(PaginationData pagination) 
        => GetPaginatedQueryable(GetCachedDbSet<TModel>(), pagination);
    
    protected IQueryable<TModel> GetPaginatedQueryable(IQueryable<TModel> baseQuery, PaginationData pagination) => baseQuery
        .Skip(pagination.SkipAmount)
        .Take(pagination.PageSize);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region GetByIdAsync
    public async ValueTask<RepoOutcome<TModel>> GetByIdAsync(Guid id, QueryConfig config = QueryConfig.None, CancellationToken ct = default) {
        if (id == Guid.Empty) return RepoOutcome.Invalid;
        
        TModel? result = await GetConfiguredQueryable(config)
            .Where(model => model.Id == id)
            .FirstOrDefaultAsync(cancellationToken: ct);

        return result is not null
            ? RepoOutcome<TModel>.FromSuccess(result)
            : RepoOutcome.NotFound;
    }
    #endregion
    
    #region GetAllAsync
    public async ValueTask<PaginatedRepoOutcome<TModel>> GetAllAsync(PaginationData pagination, QueryConfig config = QueryConfig.None, CancellationToken ct = default) {
        if (pagination.PageSize <= 0) return PaginatedData<TModel>.Empty;
        
        IQueryable<TModel> baseQuery = GetConfiguredQueryable(config);
        
        int totalCount = await baseQuery.CountAsync(ct);
        if (totalCount == 0) return PaginatedData<TModel>.Empty;
        
        IQueryable<TModel> paginatedQuery = GetPaginatedQueryable(baseQuery, pagination);

        TModel[] data = await paginatedQuery.ToArrayAsync(cancellationToken: ct);
        
        int totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);
        
        var paginatedData = new PaginatedData<TModel>(
            data,
            totalCount,
            pagination.PageNumber, 
            totalPages
        );
        
        return paginatedData;
    }
    #endregion

    #region AddAsync
    public async ValueTask<RepoOutcome> AddAsync(TModel model, CancellationToken ct = default) {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (model is null) return RepoOutcome.Invalid;
        
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        
        bool exists = await dbSet
            .AsNoTracking()
            .AnyAsync(m => m.Id == model.Id, cancellationToken: ct);
        if (exists) return RepoOutcome.AlreadyExists;
        
        await dbSet.AddAsync(model, cancellationToken: ct);
        return RepoOutcome.Success;
    }
    #endregion

    #region AnyAsync
    public async ValueTask<bool> AnyAsync(QueryConfig config = QueryConfig.None, CancellationToken ct = default) {
        IQueryable<TModel> query = GetConfiguredQueryable(config);
        
        return await query.AnyAsync(cancellationToken: ct);
    }
    #endregion
}
