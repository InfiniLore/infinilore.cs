// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
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
    public static async ValueTask<Shared.Outcome> IsNameTakenAsync<TModel>(
        DbSet<TModel> dbSet,
        string name,
        Guid ownerId,
        Guid notIncludedId = default,
        CancellationToken ct = default
    ) where TModel : BasicModel, IHasName, IHasOwnerId {
        if (name.IsNullOrWhiteSpace() || ownerId == Guid.Empty) return Shared.Outcome.FromError(RepositoryFailures.ModelFailedValidation);

        // Query
        IQueryable<TModel> query = dbSet.Where(l =>
            l.Name == name
            && l.OwnerId == ownerId
            && l.Id != notIncludedId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return Shared.Outcome.FromState(result);
    }

    public static async ValueTask<Shared.Outcome> IsNameNotTakenAsync<TModel>(
        DbSet<TModel> dbSet,
        string name,
        Guid ownerId,
        Guid notIncludedId = default,
        CancellationToken ct = default
    ) where TModel : BasicModel, IHasName, IHasOwnerId {
        Shared.Outcome outcome = await IsNameTakenAsync(dbSet, name, ownerId, notIncludedId, ct);
        if (outcome.IsError) return outcome;
        return !outcome.AsState;
    }
}
