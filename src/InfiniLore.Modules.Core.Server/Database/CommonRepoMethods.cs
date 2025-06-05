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
    public static async ValueTask<RepoOutcome> IsNameTakenAsync<TModel>(
        DbSet<TModel> dbSet,
        string name,
        Guid ownerId,
        Guid notIncludedId = default,
        CancellationToken ct = default
    ) where TModel : BasicModel, IHasName, IHasOwnerId {
        if (name.IsNullOrWhiteSpace() || ownerId == Guid.Empty) return RepoOutcome.FromError(RepositoryFailures.ModelFailedValidation);

        // Query
        IQueryable<TModel> query = dbSet.Where(l =>
            l.Name == name
            && l.OwnerId == ownerId
            && l.Id != notIncludedId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return RepoOutcome.FromState(result);
    }

    public static async ValueTask<RepoOutcome> IsNameNotTakenAsync<TModel>(
        DbSet<TModel> dbSet,
        string name,
        Guid ownerId,
        Guid notIncludedId = default,
        CancellationToken ct = default
    ) where TModel : BasicModel, IHasName, IHasOwnerId {
        RepoOutcome outcome = await IsNameTakenAsync(dbSet, name, ownerId, notIncludedId, ct);
        if (outcome.IsError) return outcome;
        return RepoOutcome.FromState(!outcome.IsTrue);
    }
}
