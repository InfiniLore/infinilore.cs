// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Database.Models;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace InfiniLore.Database.Repositories.Content;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BasicContentRepository<T>(IUnitOfWork unitOfWork) : IBasicContentRepository<T> where T : BasicContent {
    protected async Task<DbSet<T>> GetDbSetAsync(CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        return dbContext.Set<T>();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Repository Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual async ValueTask<RepoResult> TryAddAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        if (await dbSet.AnyAsync(UniqueModelPredicate(model), ct)) return "Model already exists";

        model.UpdateLastModifiedDate();
        await dbSet.AddAsync(model, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return new Success();
    }

    public virtual async ValueTask<RepoResult<T>> TryAddWithResultAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        if (await dbSet.AnyAsync(UniqueModelPredicate(model), ct)) return "Model already exists";

        model.UpdateLastModifiedDate();
        EntityEntry<T> result = await dbSet.AddAsync(model, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return result.Entity;
    }

    public async ValueTask<RepoResult> TryUpdateAsync(T model, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        model.UpdateLastModifiedDate();
        dbContext.Entry(existing).CurrentValues.SetValues(model);
        await unitOfWork.SaveChangesAsync(ct);

        return new Success();
    }

    /// <inheritdoc />
    public async ValueTask<RepoResult<T>> TryUpdateWithResultAsync(T model, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        model.UpdateLastModifiedDate();
        dbContext.Entry(existing).CurrentValues.SetValues(model);
        await unitOfWork.SaveChangesAsync(ct);

        return existing;
    }

    /// <inheritdoc />
    public async ValueTask<RepoResult> TryUpdateAsync(IEnumerable<T> models, CancellationToken ct = default) {
        T[] modelArray = models as T[] ?? models.ToArray();
        
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        Guid[] idsToUpdate = modelArray.Select(m => m.Id).ToArray();

        // Fetch existing entities from the database
        List<T> existingEntities = await dbSet.Where(m => idsToUpdate.Contains(m.Id)).ToListAsync(ct);

        if (existingEntities.Count != modelArray.Length) {
            return "One or more Models do not exist";// Some entities are missing
        }

        // Update existing entities with new values
        foreach (T existingEntity in existingEntities) {
            T updatedModel = modelArray.First(m => m.Id == existingEntity.Id);
            existingEntity.UpdateLastModifiedDate();// Update individual properties
            dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedModel);// Map the changes
        }

        await unitOfWork.SaveChangesAsync(ct);
        return new Success();
    }

    /// <inheritdoc />
    public virtual async ValueTask<RepoResult> TryAddOrUpdateAsync(T model, CancellationToken ct = default) {
        if (model.Id == Guid.Empty) return await TryAddAsync(model, ct);// If no ID, always add
        
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        // Find the existing model in the database
        T? existingModel = await dbSet.FindAsync([model.Id], ct);

        if (existingModel is null) {
            await dbSet.AddAsync(model, ct);// If it doesn't exist, add it
            await unitOfWork.SaveChangesAsync(ct);
            return new Success();
        }

        // Update the tracked entity with new values
        existingModel.UpdateLastModifiedDate();// Update necessary fields
        dbContext.Entry(existingModel).CurrentValues.SetValues(model);// Map incoming values to tracked entity

        await unitOfWork.SaveChangesAsync(ct);
        return new Success();
    }

    /// <inheritdoc />
    public virtual async ValueTask<RepoResult> TryAddOrUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        T[] userContents = models as T[] ?? models.ToArray();
        Guid[] modelIds = userContents.Select(m => m.Id).ToArray();

        // Fetch all existing models from the database
        List<T> existingModels = await dbSet.Where(m => modelIds.Contains(m.Id)).ToListAsync(ct);
        Guid[] existingModelIds = existingModels.Select(m => m.Id).ToArray();

        // Separate models into new and updateable ones
        IEnumerable<T> modelsToUpdate = userContents.Where(m => existingModelIds.Contains(m.Id));
        IEnumerable<T> modelsToAdd = userContents.Where(m => !existingModelIds.Contains(m.Id));

        // Handle tracked updates for existing models
        foreach (T modelToUpdate in modelsToUpdate) {
            T existingModel = existingModels.First(em => em.Id == modelToUpdate.Id);
            existingModel.UpdateLastModifiedDate();// Update required fields
            dbContext.Entry(existingModel).CurrentValues.SetValues(modelToUpdate);// Map incoming changes to tracked entity
        }

        // Add new models
        await dbSet.AddRangeAsync(modelsToAdd, ct);

        // Save all changes
        await unitOfWork.SaveChangesAsync(ct);

        return new Success();
    }

    /// <inheritdoc />
    public virtual async ValueTask<RepoResult> TryDeleteAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        existing.SoftDelete();
        return await TryUpdateAsync(existing, ct);
    }

    /// <inheritdoc />
    public virtual async ValueTask<RepoResult> TryAddRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        IEnumerable<T> content = models as T[] ?? models.ToArray();
        Guid[] modelIds = content.Select(m => m.Id).ToArray();

        // Get all models in the db that match any Ids of the passed-in models
        List<T> existingModels = await dbSet.Where(m => modelIds.Contains(m.Id)).ToListAsync(ct);

        if (existingModels.Count > 0)
            return "One or more Models already exist";

        await dbSet.AddRangeAsync(content, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return new Success();
    }

    /// <inheritdoc />
    public virtual async ValueTask<RepoResult> TryDeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        Guid[] ids = models.Select(model => model.Id).ToArray();

        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteUpdateAsync(setPropertyCalls: s => BasicContent.SoftDeleteWithPropertyCalls(s), ct);

        return new Success();
    }

    /// <inheritdoc />
    public async ValueTask<RepoResult> TryRemoveAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        dbSet.Remove(existing);
        await unitOfWork.SaveChangesAsync(ct);

        return new Success();
    }

    /// <inheritdoc />
    public async ValueTask<RepoResult> TryRemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        Guid[] ids = models.Select(model => model.Id).ToArray();

        int recordsAffected = await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);

        if (recordsAffected <= 0 && ids.Length != 0) return "No models were deleted";

        return new Success();
    }

    /// <inheritdoc />
    public virtual async ValueTask<RepoResult<T>> TryGetByIdAsync(Guid id, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        T? result = await dbSet
            .FirstOrDefaultAsync(predicate: ls => ls.Id == id, ct);

        if (result is null) return "Content not found.";

        return result;
    }

    /// <inheritdoc />
    public virtual async ValueTask<RepoResult<T[]>> TryGetAllAsync(bool reverse = false, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        T[] result = await dbSet
            .ConditionalReverse(reverse)
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    /// <inheritdoc />
    public virtual async ValueTask<PaginatedRepoResult<T>> TryGetAllAsync(PaginationInfo pageInfo, bool reverse = false, CancellationToken ct = default) {
        if (pageInfo.IsNotValid(out Failure<string> pageInfoFailure)) return pageInfoFailure;

        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        int totalCount = await dbSet.CountAsync(ct);
        if (totalCount == 0) return new PaginatedResult<T>([], 0, 0, 0);
        
        T[] result = await dbSet
            .ConditionalReverse(reverse)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize)
            .ToArrayAsync(ct);

        return new PaginatedResult<T>(
            Items: result,
            TotalCount: totalCount,
            CurrentPage: pageInfo.PageNumber,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageInfo.PageSize)
        );
    }
    
    public virtual async ValueTask<RepoResult<int>> TryCountAsync(CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        
        int count = await dbSet.CountAsync(ct);
        
        return count;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected virtual Expression<Func<T, bool>> UniqueModelPredicate(T originalModel) {
        return dbModel => dbModel.Id == originalModel.Id;
    }
}
