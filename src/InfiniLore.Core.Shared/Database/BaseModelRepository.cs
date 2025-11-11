// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Core.Outcomes;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseModelRepository<TModel, TDbContext> : UnitOfWorkRepository<TDbContext>
    where TModel : BaseModel 
    where TDbContext : DbContext
{
    protected virtual IQueryable<TModel> OptionalInclude(IQueryable<TModel> query) => query;
    protected virtual IQueryable<TModel> AlwaysInclude(IQueryable<TModel> query) => query;
    
    protected IQueryable<TModel> GetConfiguredQueryable(IQueryable<TModel> baseQuery, QueryConfig config) {
        IQueryable<TModel> query = baseQuery
            .AsNoTracking()
            .With(AlwaysInclude)
            .ConditionalWith(config.HasFlagFast(QueryConfig.IncludeOptionalReferences), OptionalInclude)
            .ConditionalReverse(config.HasFlagFast(QueryConfig.Reversed))
            .ConditionalWith(config.HasFlagFast(QueryConfig.IncludeDeleted), model => model.IgnoreQueryFilters());

        return query;
    }
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Outcome<TModel>> GetByIdAsync(Guid id, QueryConfig config = QueryConfig.None, CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        IQueryable<TModel> query = GetConfiguredQueryable(dbSet, config)
            .Where(model => model.Id == id);
        
        TModel? result = await query.FirstOrDefaultAsync(cancellationToken: ct);
        
        return result is not null 
            ? Outcome<TModel>.FromSuccess(result)
            : Outcome<TModel>.FromError("Model not found");
    }
}
