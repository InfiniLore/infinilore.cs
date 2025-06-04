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
            .With(AlwaysInclude)
            .ConditionalWith(config.OptionalInclude, OptionalInclude)
            .ConditionalReverse(config.Reverse)
            .ConditionalWith(config.RetrieveSoftDeleted, model => model.IgnoreQueryFilters());

        return query;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<RepoOutcome<TModel>> GetByIdAsync(Guid id, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        // Query
        TModel? result = await GetConfiguredQueryable(dbSet, config)
            .Where(ls => ls.Id == id)
            .FirstOrDefaultAsync(cancellationToken: ct);

        // Retrieve
        if (result is null) return Outcome<TModel>.FromError(RepositoryFailures.ModelNotFound);
        return Outcome<TModel>.FromData(result);
    }

    public async ValueTask<RepoOutcome<TModel[]>> GetAllAsync(QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        // Query
        IOrderedQueryable<TModel> query = GetConfiguredQueryable(dbSet, config)
            .OrderByDescending(ls => ls.Id);

        // Query & Retrieve
        TModel[] data = await query.ToArrayAsync(cancellationToken: ct);
        return Outcome<TModel[]>.FromData(data);
    }

    public async ValueTask<PaginatedOutcome<TModel>> GetAllAsync(Pagination pageInfo, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        // Query
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return PaginatedData<TModel>.Empty;

        IQueryable<TModel> query = GetConfiguredQueryable(dbSet, config)
            .OrderByDescending(ls => ls.Id)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize);

        // Retrieve
        TModel[] data = await query.ToArrayAsync(cancellationToken: ct);
        return new PaginatedData<TModel>(
            data,
            totalCount,
            pageInfo.PageNumber,
            (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }

    public async ValueTask<RepoOutcome<int>> GetCountAsync(CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        // Query & Retrieve
        int data = await dbSet
            .AsNoTracking()
            .CountAsync(cancellationToken: ct);

        return Outcome<int>.FromData(data);
    }

    public async ValueTask<RepoOutcome> IsIdTakenAsync(Guid id, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        // Query & Retrieve
        bool result = await dbSet
            .AsNoTracking()
            .AnyAsync(predicate: ls => ls.Id == id, ct);

        return Outcome.FromState(result);
    }

    public async ValueTask<Outcome> IsIdNotTakenAsync(Guid id, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        // Query & Retrieve
        bool result = await dbSet
            .AsNoTracking()
            .AnyAsync(predicate: ls => ls.Id == id, ct);

        return Outcome.FromState(!result);
    }

    protected virtual async ValueTask<bool> IsNotUniqueAsync(TModel[] modelsToValidate, CancellationToken ct = default) {
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();
        Guid[] ids = modelsToValidate.Select(m => m.Id).ToArray();
        bool result = await dbSet.AsNoTracking().AnyAsync(predicate: foundModel => ids.Contains(foundModel.Id), ct);
        return result;
    }

    private ValueTask<bool> IsNotUniqueAsync(TModel originalModel, CancellationToken ct = default)
        => IsNotUniqueAsync([originalModel], ct);

    // -----------------------------------------------------------------------------------------------------------------
    // Repository Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region CRUD Operations
    public async ValueTask<RepoOutcome> AddAsync(TModel model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        if (await IsNotUniqueAsync(model, ct)) return Outcome.FromError(RepositoryFailures.ModelFailedUniqueConstraint);

        model.UpdateLastModifiedDate();

        // Query
        await dbSet.AddAsync(model, ct);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return Outcome.True;
    }

    public async ValueTask<Outcome> AddRangeAsync(IEnumerable<TModel> models, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        TModel[] content = models as TModel[] ?? models.ToArray();
        if (await IsNotUniqueAsync(content, ct)) return Outcome.FromError(RepositoryFailures.ModelFailedUniqueConstraint);

        // Query
        foreach (TModel model in content) {
            model.UpdateLastModifiedDate();
            await dbSet.AddAsync(model, ct);
        }
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return Outcome.True;
    }

    public async ValueTask<RepoOutcome> UpdateAsync(TModel model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        TModel? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing is null) return Outcome.FromError(RepositoryFailures.ModelNotFound);

        // Query
        model.UpdateLastModifiedDate();
        dbContext.Entry(existing).CurrentValues.SetValues(model);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return Outcome.True;
    }

    public async ValueTask<Outcome> UpdateRangeAsync(IEnumerable<TModel> models, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        TModel[] modelArray = models as TModel[] ?? models.ToArray();
        Guid[] idsToUpdate = modelArray.Select(m => m.Id).ToArray();
        TModel[] existingEntities = await dbSet.Where(m => idsToUpdate.Contains(m.Id)).ToArrayAsync(ct);
        if (existingEntities.Length != modelArray.Length) return Outcome.FromError(RepositoryFailures.ModelNotFound);

        // Query
        foreach (TModel existingEntity in existingEntities) {
            TModel updatedModel = modelArray.First(m => m.Id == existingEntity.Id);
            existingEntity.UpdateLastModifiedDate();// Update individual properties
            dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedModel);// Map the changes
        }

        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return Outcome.True;
    }

    public async ValueTask<RepoOutcome> AddOrUpdateAsync(TModel model, CancellationToken ct = default) {
        if (model.Id == Guid.Empty) return await AddAsync(model, ct);// If no ID, always add
        if (await IsNotUniqueAsync(model, ct)) return await UpdateAsync(model, ct);// If ID exists, update

        return await AddAsync(model, ct);
    }

    public async ValueTask<Outcome> AddOrUpdateRangeAsync(IEnumerable<TModel> models, CancellationToken ct = default) {
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        TModel[] userContents = models as TModel[] ?? throw new ArgumentNullException(nameof(models));
        Guid[] modelIds = userContents.Select(m => m.Id).ToArray();

        // Fetch all existing models from the database
        List<TModel> existingModels = await dbSet.Where(m => modelIds.Contains(m.Id)).ToListAsync(ct);
        Guid[] existingModelIds = existingModels.Select(m => m.Id).ToArray();

        // Separate models into new and updateable ones
        IEnumerable<TModel> modelsToUpdate = userContents.Where(m => existingModelIds.Contains(m.Id));
        IEnumerable<TModel> modelsToAdd = userContents.Where(m => !existingModelIds.Contains(m.Id));

        // Handle tracked updates for existing models
        foreach (TModel modelToUpdate in modelsToUpdate) {
            TModel existingModel = existingModels.First(em => em.Id == modelToUpdate.Id);
            existingModel.UpdateLastModifiedDate();// Update required fields
            dbContext.Entry(existingModel).CurrentValues.SetValues(modelToUpdate);// Map incoming changes to tracked entity
        }

        // Add new models
        await dbSet.AddRangeAsync(modelsToAdd, ct);

        // Save all changes
        await dbContext.SaveChangesAsync(ct);

        return Outcome.True;
    }

    public async ValueTask<RepoOutcome> DeleteAsync(TModel model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        TModel? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return Outcome.FromError(RepositoryFailures.ModelNotFound);

        // Query
        existing.SoftDelete();
        dbContext.Entry(existing).CurrentValues.SetValues(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve   
        return Outcome.True;
    }

    public async ValueTask<Outcome> DeleteByIdAsync(Guid id, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        TModel? existing = await dbSet.FindAsync([id], ct);
        if (existing == null) return Outcome.FromError(RepositoryFailures.ModelNotFound);

        // Query
        existing.SoftDelete();
        dbContext.Entry(existing).CurrentValues.SetValues(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve   
        return Outcome.True;

    }

    public async ValueTask<RepoOutcome> DeleteRangeAsync(IEnumerable<TModel> models, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        Guid[] ids = models.Select(model => model.Id).ToArray();

        // Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteUpdateAsync(setPropertyCalls: s => BasicModel.SoftDelete(s), ct);

        // Retrieve
        return Outcome.True;
    }

    public async ValueTask<Outcome> DeleteRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        // Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteUpdateAsync(setPropertyCalls: s => BasicModel.SoftDelete(s), ct);

        // Retrieve
        return Outcome.True;
    }

    public async ValueTask<Outcome> RemoveAsync(TModel model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        TModel? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return Outcome.FromError(RepositoryFailures.ModelNotFound);

        // Query
        dbSet.Remove(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return Outcome.True;
    }

    public async ValueTask<RepoOutcome> RemoveByIdAsync(Guid id, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        TModel? existing = await dbSet.FindAsync([id], ct);
        if (existing == null) return Outcome.FromError(RepositoryFailures.ModelNotFound);

        // Query
        dbSet.Remove(existing);
        await dbContext.SaveChangesAsync(ct);

        // Retrieve
        return Outcome.True;
    }

    public async ValueTask<Outcome> RemoveRangeAsync(IEnumerable<TModel> models, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        Guid[] ids = models.Select(model => model.Id).ToArray();

        // Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);

        // Retrieve
        return Outcome.True;
    }

    public async ValueTask<RepoOutcome> RemoveRangeByIdAsync(IEnumerable<Guid> ids, CancellationToken ct = default) {
        // Access
        DbSet<TModel> dbSet = GetCachedDbSet<TModel>();

        // Query
        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);

        // Retrieve
        return Outcome.True;
    }
    #endregion
}
