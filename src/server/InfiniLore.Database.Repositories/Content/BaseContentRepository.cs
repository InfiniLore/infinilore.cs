// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Database.Models;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace InfiniLore.Database.Repositories.Content;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseContentRepository<T>(IDbUnitOfWork<MsSqlDbContext> unitOfWork) : MsSqlRepository<T>(unitOfWork), IBaseContentRepository<T> where T : BaseContent {
    private readonly IDbUnitOfWork<MsSqlDbContext> _unitOfWork = unitOfWork;

    // -----------------------------------------------------------------------------------------------------------------
    // Repository Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async virtual ValueTask<RepoResult> TryAddAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        if (await dbSet.AnyAsync(UniqueModelPredicate(model), ct)) return "Model already exists";

        model.UpdateLastModifiedDate();
        await dbSet.AddAsync(model, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return new Success();
    }

    public async virtual ValueTask<RepoResult<T>> TryAddWithResultAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        if (await dbSet.AnyAsync(UniqueModelPredicate(model), ct)) return "Model already exists";

        model.UpdateLastModifiedDate();
        EntityEntry<T> result = await dbSet.AddAsync(model, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return result.Entity;
    }

    public async ValueTask<RepoResult> TryUpdateAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        MsSqlDbContext dbContext = await _unitOfWork.GetDbContextAsync(ct);

        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        model.UpdateLastModifiedDate();
        dbContext.Entry(existing).CurrentValues.SetValues(model);
        await _unitOfWork.SaveChangesAsync(ct);

        return new Success();
    }

    /// <inheritdoc />
    public async ValueTask<RepoResult<T>> TryUpdateWithResultAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        MsSqlDbContext dbContext = await _unitOfWork.GetDbContextAsync(ct);

        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        model.UpdateLastModifiedDate();
        dbContext.Entry(existing).CurrentValues.SetValues(model);
        await _unitOfWork.SaveChangesAsync(ct);

        return existing;
    }

    /// <inheritdoc />
    public async ValueTask<RepoResult> TryUpdateAsync(IEnumerable<T> models, CancellationToken ct = default) {
        T[] modelArray = models as T[] ?? models.ToArray();
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        MsSqlDbContext dbContext = await _unitOfWork.GetDbContextAsync(ct);
        HashSet<Guid> idsToUpdate = modelArray.Select(m => m.Id).ToHashSet();

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

        await _unitOfWork.SaveChangesAsync(ct);
        return new Success();
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult> TryAddOrUpdateAsync(T model, CancellationToken ct = default) {
        if (model.Id == Guid.Empty) return await TryAddAsync(model, ct);// If no ID, always add

        DbSet<T> dbSet = await GetDbSetAsync(ct);
        MsSqlDbContext dbContext = await _unitOfWork.GetDbContextAsync(ct);

        // Find the existing model in the database
        T? existingModel = await dbSet.FindAsync([model.Id], ct);

        if (existingModel is null) {
            await dbSet.AddAsync(model, ct);// If it doesn't exist, add it
            await _unitOfWork.SaveChangesAsync(ct);
            return new Success();
        }

        // Update the tracked entity with new values
        existingModel.UpdateLastModifiedDate();// Update necessary fields
        dbContext.Entry(existingModel).CurrentValues.SetValues(model);// Map incoming values to tracked entity

        await _unitOfWork.SaveChangesAsync(ct);
        return new Success();
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult> TryAddOrUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        MsSqlDbContext dbContext = await _unitOfWork.GetDbContextAsync(ct);

        T[] userContents = models as T[] ?? models.ToArray();
        HashSet<Guid> modelIds = userContents.Select(m => m.Id).ToHashSet();

        // Fetch all existing models from the database
        List<T> existingModels = await dbSet.Where(m => modelIds.Contains(m.Id)).ToListAsync(ct);
        HashSet<Guid> existingModelIds = existingModels.Select(m => m.Id).ToHashSet();

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
        await _unitOfWork.SaveChangesAsync(ct);

        return new Success();
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult> TryDeleteAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        existing.SoftDelete();
        return await TryUpdateAsync(existing, ct);
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult> TryAddRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        IEnumerable<T> userContents = models as T[] ?? models.ToArray();
        HashSet<Guid> modelIds = userContents.Select(m => m.Id).ToHashSet();

        // Get all models in the db that match any Ids of the passed-in models
        List<T> existingModels = await dbSet.Where(m => modelIds.Contains(m.Id)).ToListAsync(ct);

        if (existingModels.Count > 0)
            return "One or more Models already exist";

        await dbSet.AddRangeAsync(userContents, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return new Success();
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult> TryDeleteRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        HashSet<Guid> ids = models.Select(model => model.Id).ToHashSet();

        await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteUpdateAsync(setPropertyCalls: s => BaseContent.SoftDeleteWithPropertyCalls(s), ct);

        return new Success();
    }

    /// <inheritdoc />
    public async ValueTask<RepoResult> TryRemoveAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        dbSet.Remove(existing);
        await _unitOfWork.SaveChangesAsync(ct);

        return new Success();
    }

    /// <inheritdoc />
    public async ValueTask<RepoResult> TryRemoveRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        HashSet<Guid> ids = models.Select(model => model.Id).ToHashSet();

        int recordsAffected = await dbSet
            .Where(model => ids.Contains(model.Id))
            .ExecuteDeleteAsync(ct);

        if (recordsAffected <= 0 && ids.Count != 0) return "No models were deleted";

        return new Success();
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult<T>> TryGetByIdAsync(Guid id, CancellationToken ct) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T? result = await dbSet
            .FirstOrDefaultAsync(predicate: ls => ls.Id == id, ct);

        if (result is null) return "Content not found.";

        return new Success<T>(result);
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult<T[]>> TryGetAllAsync(CancellationToken ct) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T[] result = await dbSet
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult<T[]>> TryGetAllASync(PaginationInfo pageInfo, CancellationToken ct) {
        if (pageInfo.IsNotValid(out Failure<string> pageInfoFailure)) return pageInfoFailure;

        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T[] result = await dbSet
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize)
            .ToArrayAsync(ct);

        return result;
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult<T[]>> TryGetByCriteriaAsync(Expression<Func<T, bool>> predicate, CancellationToken ct) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T[] result = await dbSet
            .Where(predicate)
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult<T[]>> TryGetByCriteriaAsync(Expression<Func<T, int, bool>> predicate, CancellationToken ct) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T[] result = await dbSet
            .Where(predicate)
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult<T[]>> TryGetByCriteriaAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, PaginationInfo pageInfo, CancellationToken ct) {
        if (pageInfo.IsNotValid(out Failure<string> pageInfoFailure)) return pageInfoFailure;

        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T[] result = await dbSet
            .Where(predicate)
            .OrderBy(orderBy)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize)
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult<T[]>> TryGetByCriteriaAsync(Expression<Func<T, int, bool>> predicate, Expression<Func<T, object>> orderBy, PaginationInfo pageInfo, CancellationToken ct) {
        if (pageInfo.IsNotValid(out Failure<string> pageInfoFailure)) return pageInfoFailure;

        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T[] result = await dbSet
            .Where(predicate)
            .OrderBy(orderBy)
            .Skip(pageInfo.SkipAmount)
            .Take(pageInfo.PageSize)
            .ToArrayAsync(cancellationToken: ct);

        return result;
    }
    
    public async virtual ValueTask<RepoResult<int>> TryCountAsync(CancellationToken ct = default) {
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
