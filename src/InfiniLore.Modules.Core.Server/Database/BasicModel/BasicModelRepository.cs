// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BasicModelRepository<TModel> : UnitOfWorkRepository<ContentDb>, IBasicModelRepository<TModel> 
    where TModel : BasicModel
{
    protected virtual IQueryable<TModel> OptionalInclude(IQueryable<TModel> query) => query;
    protected virtual IQueryable<TModel> AlwaysInclude(IQueryable<TModel> query) => query;

    protected IQueryable<TModel> GetConfiguredQueryable(IQueryable<TModel> baseQuery, QueryConfig config) {
        IQueryable<TModel> query = baseQuery
            .AsNoTracking()
            .With(AlwaysInclude)
            .ConditionalWith(config.OptionalInclude, OptionalInclude)
            .ConditionalReverse(config.Reverse)
            .ConditionalWith(config.RetrieveSoftDeleted, model => model.IgnoreQueryFilters());

        return query;
    }
    
    protected async ValueTask<TModel?> TryFindAsync(Guid id, CancellationToken ct)
        => await GetCachedDbSet<TModel>().FindAsync([id], ct);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<RepoOutcome<TModel>> GetByIdAsync(Guid id, QueryConfig config = default, CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        
        TModel? result = await GetConfiguredQueryable(dbSet, config)
            .Where(ls => ls.Id == id)
            .FirstOrDefaultAsync(cancellationToken: ct);
        
        if (result is null) return RepoOutcome<TModel>.FromError(RepositoryFailures.ModelNotFound);
        return RepoOutcome<TModel>.FromData(result);
    }

    public async ValueTask<RepoOutcome<TModel[]>> GetAllAsync(QueryConfig config = default, CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        
        IOrderedQueryable<TModel> query = GetConfiguredQueryable(dbSet, config)
            .OrderByDescending(ls => ls.Id);
        TModel[] data = await query.ToArrayAsync(cancellationToken: ct);
        
        return RepoOutcome<TModel[]>.FromData(data);
    }

    public async ValueTask<PaginatedRepoOutcome<TModel>> GetAllAsync(Pagination pageInfo, QueryConfig config = default, CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        IQueryable<TModel> baseQuery = GetConfiguredQueryable(dbSet, config);
        
        int totalCount = await baseQuery.CountAsync(ct);
        if (totalCount == 0) return PaginatedData<TModel>.Empty;
       
        IQueryable<TModel> query = baseQuery
            .OrderByDescending(ls => ls.Id)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);
        
        TModel[] data = await query.ToArrayAsync(cancellationToken: ct);
        return new PaginatedData<TModel>(
            data,
            totalCount,
            pageInfo.PageNumber,
            (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }

    public async ValueTask<RepoOutcome<int>> GetCountAsync(CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        
        int data = await dbSet
            .AsNoTracking()
            .CountAsync(ct);
        
        return RepoOutcome<int>.FromData(data);
    }

    public async ValueTask<RepoOutcome> IsIdTakenAsync(Guid id, CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        
        bool result = await dbSet
            .AsNoTracking()
            .AnyAsync(ls => ls.Id == id, ct);
        
        return RepoOutcome.FromState(result);
    }

    public async ValueTask<RepoOutcome> IsIdNotTakenAsync(Guid id, CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        
        bool result = await dbSet
            .AsNoTracking()
            .AnyAsync(ls => ls.Id == id, ct);
        
        return RepoOutcome.FromState(!result);
    }

    protected virtual async ValueTask<bool> IsNotUniqueAsync(TModel[] modelsToValidate, CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        
        Guid[] ids = modelsToValidate.Select(m => m.Id).ToArray();
        bool result = await dbSet
            .AsNoTracking()
            .AnyAsync(predicate: foundModel => ids.Contains(foundModel.Id), ct);
        
        return result;
    }

    private ValueTask<bool> IsNotUniqueAsync(TModel originalModel, CancellationToken ct = default)
        => IsNotUniqueAsync([originalModel], ct);

    // -----------------------------------------------------------------------------------------------------------------
    // Repository Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region CRUD Operations
    public async ValueTask<RepoOutcome> AddAsync(TModel model, CancellationToken ct = default) {
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        
        if (await IsNotUniqueAsync(model, ct)) return RepoOutcome.FromError(RepositoryFailures.ModelFailedUniqueConstraint);
        
        model.UpdateLastModifiedDate();
        await dbSet.AddAsync(model, ct);
        await dbContext.SaveChangesAsync(ct);
        
        return RepoOutcome.True;
    }

    public async ValueTask<RepoOutcome> AddRangeAsync(IEnumerable<TModel> models, CancellationToken ct = default) {
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        
        TModel[] content = models as TModel[] ?? models.ToArray();
        if (await IsNotUniqueAsync(content, ct)) return RepoOutcome.FromError(RepositoryFailures.ModelFailedUniqueConstraint);
        
        foreach (TModel model in content) {
            model.UpdateLastModifiedDate();
            await dbSet.AddAsync(model, ct);
        }
        
        await dbContext.SaveChangesAsync(ct);
        
        return RepoOutcome.True;
    }

    public async ValueTask<RepoOutcome> UpdateAsync(TModel model, CancellationToken ct = default) {
        TModel? existing = await TryFindAsync(model.Id, ct);
        if (existing is null) return RepoOutcome.FromError(RepositoryFailures.ModelNotFound);
        
        ContentDb dbContext = GetDbContext();
        
        model.UpdateLastModifiedDate();
        dbContext.Entry(existing).CurrentValues.SetValues(model);
        await dbContext.SaveChangesAsync(ct);
        
        return RepoOutcome.True;
    }

    public async ValueTask<RepoOutcome> UpdateRangeAsync(IEnumerable<TModel> models, CancellationToken ct = default) {
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        
        TModel[] modelArray = models as TModel[] ?? models.ToArray();
        Guid[] idsToUpdate = modelArray.Select(m => m.Id).ToArray();
        TModel[] existingEntities = await dbSet
            .AsNoTracking()
            .Where(m => idsToUpdate.Contains(m.Id))
            .ToArrayAsync(ct);
        
        if (existingEntities.Length != modelArray.Length) return RepoOutcome.FromError(RepositoryFailures.ModelNotFound);
        
        Dictionary<Guid, TModel> updates = modelArray.ToDictionary(m => m.Id);
        foreach (TModel existing in existingEntities) {
            TModel updatedModel = updates[existing.Id];
            existing.UpdateLastModifiedDate();
            dbContext.Entry(existing).CurrentValues.SetValues(updatedModel);
        }
        
        await dbContext.SaveChangesAsync(ct);
        return RepoOutcome.True;
    }

    public async ValueTask<RepoOutcome> AddOrUpdateAsync(TModel model, CancellationToken ct = default) {
        if (model.Id == Guid.Empty) return await AddAsync(model, ct);
        if (await IsNotUniqueAsync(model, ct)) return await UpdateAsync(model, ct);

        return await AddAsync(model, ct);
    }

    public async ValueTask<RepoOutcome> AddOrUpdateRangeAsync(IEnumerable<TModel> models, CancellationToken ct = default) {
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        
        TModel[] userContents = models as TModel[] ?? throw new ArgumentNullException(nameof(models));
        Guid[] modelIds = userContents.Select(m => m.Id).ToArray();
        
        List<TModel> existingModels = await dbSet
            .AsNoTracking()
            .Where(m => modelIds.Contains(m.Id))
            .ToListAsync(ct);
        
        HashSet<Guid> existingModelIds = existingModels.Select(m => m.Id).ToHashSet();
        IEnumerable<TModel> modelsToUpdate = userContents.Where(m => existingModelIds.Contains(m.Id));
        IEnumerable<TModel> modelsToAdd = userContents.Where(m => !existingModelIds.Contains(m.Id));
        
        foreach (TModel modelToUpdate in modelsToUpdate) {
            TModel existingModel = existingModels.First(em => em.Id == modelToUpdate.Id);
            modelToUpdate.UpdateLastModifiedDate();
            dbContext.Entry(existingModel).CurrentValues.SetValues(modelToUpdate);
        }

        await dbSet.AddRangeAsync(modelsToAdd, ct);
        await dbContext.SaveChangesAsync(ct);
        return RepoOutcome.True;
    }


    public async ValueTask<RepoOutcome> DeleteAsync(TModel model, CancellationToken ct = default) {
        TModel? existing = await TryFindAsync(model.Id, ct);
        if (existing == null) return RepoOutcome.FromError(RepositoryFailures.ModelNotFound);

        ContentDb dbContext = GetDbContext();
        
        existing.SoftDelete();
        await dbContext.SaveChangesAsync(ct);
   
        return RepoOutcome.True;
    }

    public async ValueTask<RepoOutcome> DeleteByIdAsync(Guid id, CancellationToken ct = default) {
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        TModel? existing = await dbSet.FindAsync([id], ct);
        if (existing == null) return RepoOutcome.FromError(RepositoryFailures.ModelNotFound);

        existing.SoftDelete();
        await dbContext.SaveChangesAsync(ct);
   
        return RepoOutcome.True;

    }

    public async ValueTask<RepoOutcome> DeleteRangeAsync(IEnumerable<TModel> models, CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        Guid[] ids = models.Select(model => model.Id).ToArray();

        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteUpdateAsync(setPropertyCalls: s => BasicModel.SoftDelete(s), ct);

        return RepoOutcome.True;
    }

    public async ValueTask<RepoOutcome> DeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteUpdateAsync(setPropertyCalls: s => BasicModel.SoftDelete(s), ct);

        return RepoOutcome.True;
    }

    public async ValueTask<RepoOutcome> RemoveAsync(TModel model, CancellationToken ct = default) {
        TModel? existing = await TryFindAsync(model.Id, ct);
        if (existing == null) return RepoOutcome.FromError(RepositoryFailures.ModelNotFound);

        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        
        dbSet.Remove(existing);
        await dbContext.SaveChangesAsync(ct);

        return RepoOutcome.True;
    }

    public async ValueTask<RepoOutcome> RemoveByIdAsync(Guid id, CancellationToken ct = default) {
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        TModel? existing = await dbSet.FindAsync([id], ct);
        if (existing == null) return RepoOutcome.FromError(RepositoryFailures.ModelNotFound);

        dbSet.Remove(existing);
        await dbContext.SaveChangesAsync(ct);

        return RepoOutcome.True;
    }

    public async ValueTask<RepoOutcome> RemoveRangeAsync(IEnumerable<TModel> models, CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        Guid[] ids = models.Select(model => model.Id).ToArray();

        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);

        return RepoOutcome.True;
    }

    public async ValueTask<RepoOutcome> RemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);

        return RepoOutcome.True;
    }
    #endregion
}
