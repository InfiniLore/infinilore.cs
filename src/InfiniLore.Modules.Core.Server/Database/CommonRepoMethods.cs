// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Modules.Core.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class CommonRepoMethods {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static async ValueTask<Result> IsNameTakenAsync<TModel>(
        DbSet<TModel> dbSet,
        string name,
        Guid ownerId,
        Guid notIncludedId = default,
        CancellationToken ct = default
    ) where TModel : BasicModel, IHasName, IHasOwnerId {
        if (name.IsNullOrWhiteSpace() || ownerId == Guid.Empty) return Result.FromError(RepositoryFailures.ModelFailedValidation);

        // Query
        IQueryable<TModel> query = dbSet.Where(l =>
            l.Name == name
            && l.OwnerId == ownerId
            && l.Id != notIncludedId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return Result.FromState(result);
    }

    public static async ValueTask<Result> IsNameNotTakenAsync<TModel>(
        DbSet<TModel> dbSet,
        string name,
        Guid ownerId,
        Guid notIncludedId = default,
        CancellationToken ct = default
    ) where TModel : BasicModel, IHasName, IHasOwnerId {
        Result result = await IsNameTakenAsync(dbSet, name, ownerId, notIncludedId, ct);
        if (result.IsError) return result;
        return !result.AsState;
    }
}
