// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Outcomes;
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

    protected IQueryable<TModel> GetConfiguredQueryable(IQueryable<TModel> baseQuery, QueryConfig config) => baseQuery
        .AsNoTracking()
        .With(AlwaysInclude)
        .ConditionalWith(config.HasFlagFast(QueryConfig.IncludeOptionalReferences), OptionalInclude)
        .ConditionalReverse(config.HasFlagFast(QueryConfig.Reversed))
        .ConditionalWith(config.HasFlagFast(QueryConfig.IncludeSoftDeleted), query => query.IgnoreQueryFilters())
        .ConditionalWith(config.HasFlagFast(QueryConfig.SortByCreatedAt | QueryConfig.SortByModifiedAt), query => query.OrderBy(model => model.CreatedAt).ThenBy(model => model.ModifiedAt))
        .ConditionalOrderBy(config.HasFlagFast(QueryConfig.SortByCreatedAt), model => model.CreatedAt)
        .ConditionalOrderBy(config.HasFlagFast(QueryConfig.SortByModifiedAt), model => model.ModifiedAt);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region GetByIdAsync
    public async ValueTask<RepoOutcome<TModel>> GetByIdAsync(Guid id, QueryConfig config = QueryConfig.None, CancellationToken ct = default) {
        if (id == Guid.Empty) return RepoOutcome.Invalid;
        
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        TModel? result = await GetConfiguredQueryable(dbSet, config)
            .Where(model => model.Id == id)
            .FirstOrDefaultAsync(cancellationToken: ct);

        return result is not null
            ? RepoOutcome<TModel>.FromSuccess(result)
            : RepoOutcome.NotFound;
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
}
