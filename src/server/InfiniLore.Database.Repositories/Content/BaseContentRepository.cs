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
    protected virtual Expression<Func<T, bool>> UniqueModelPredicate(T originalModel) {
        return dbModel => dbModel.Id == originalModel.Id;
    }
    
    public async virtual ValueTask<RepoResult> TryAddAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        if (await dbSet.AnyAsync(UniqueModelPredicate(model), ct)) return "Model already exists";

        model.UpdateLastModifiedDate();
        await dbSet.AddAsync(model, ct);
        return new Success();
    }

    public async virtual ValueTask<RepoResult<T>> TryAddWithResultAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        if (await dbSet.AnyAsync(UniqueModelPredicate(model), ct)) return "Model already exists";

        model.UpdateLastModifiedDate();
        EntityEntry<T> result = await dbSet.AddAsync(model, ct);
        return result;
    }

    public async ValueTask<RepoResult> TryUpdateAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        // All is done in one call to the db
        model.UpdateLastModifiedDate();
        dbSet.Update(model);

        return new Success();
    }

    /// <inheritdoc />
    public async ValueTask<RepoResult<T>> TryUpdateWithResultAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        // All is done in one call to the db
        model.UpdateLastModifiedDate();
        EntityEntry<T> result = dbSet.Update(model);

        return result;
    }

    /// <inheritdoc />
    public async ValueTask<RepoResult> TryUpdateAsync(IEnumerable<T> models, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T[] modelArray = models as T[] ?? models.ToArray();
        HashSet<Guid> idsToUpdate = modelArray.Select(m => m.Id).ToHashSet();
        if (!await dbSet.AllAsync(predicate: model => idsToUpdate.Contains(model.Id), ct)) return "One or more Models do not exist";

        foreach (T model in modelArray) {
            model.UpdateLastModifiedDate();
        }

        dbSet.UpdateRange(modelArray);

        // // Update the last modified date for all models that were updated
        // // Do this through property calls for performance reasons
        // await dbSet
        //     .Where(model => idsToUpdate.Contains(model.Id))
        //     .ExecuteUpdateAsync(setPropertyCalls: s => BaseContent.UpdateLastModifiedDateWithPropertyCalls(s), ct);

        return new Success();
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult> TryAddOrUpdateAsync(T model, CancellationToken ct = default) {
        if (model.Id == Guid.Empty) return await TryAddAsync(model, ct);// model wasn't assigned an id yet, so can always be added (normally)

        DbSet<T> dbSet = await GetDbSetAsync(ct);
        if (await dbSet.FindAsync([model.Id], ct) is null) {
            await dbSet.AddAsync(model, ct);
            return new Success();
        }

        model.UpdateLastModifiedDate();
        dbSet.Update(model);

        return new Success();
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult> TryAddOrUpdateRangeAsync(IEnumerable<T> models, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);

        T[] userContents = models as T[] ?? models.ToArray();
        HashSet<Guid> modelIds = userContents
            .Select(c => c.Id)
            .ToHashSet();

        T[] existingModels = await dbSet
            .Where(m => modelIds.Contains(m.Id))
            .ToArrayAsync(ct);

        HashSet<Guid> existingModelIds = existingModels
            .Select(m => m.Id)
            .ToHashSet();

        IEnumerable<T> modelsNotInDb = userContents
            .Where(model => !existingModelIds.Contains(model.Id));

        foreach (T existingModel in existingModels) {
            existingModel.UpdateLastModifiedDate();
        }

        await dbSet.AddRangeAsync(modelsNotInDb, ct);
        dbSet.UpdateRange(existingModels);

        return new Success();
    }

    /// <inheritdoc />
    public async virtual ValueTask<RepoResult> TryDeleteAsync(T model, CancellationToken ct = default) {
        DbSet<T> dbSet = await GetDbSetAsync(ct);
        T? existing = await dbSet.FindAsync([model.Id], ct);
        if (existing == null) return "Model does not exist";

        existing.SoftDelete();
        return new Success();
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
}
