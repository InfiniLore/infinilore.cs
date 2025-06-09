// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IKeyValueEntryRepository>]
public class KeyValueEntryRepository : UnitOfWorkRepository<ContentDb>, IKeyValueEntryRepository {
    public async ValueTask<RepoOutcome> TryAddOrUpdateAsync(KeyValueEntryModel model, CancellationToken ct = default) {
        if (string.IsNullOrWhiteSpace(model.Key))
            return RepoOutcome.FromError(RepositoryFailures.InvalidInput);

        ContentDb dbContext = GetDbContext();
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();

        KeyValueEntryModel? existing = await dbSet.FindAsync([model.Key], ct).ConfigureAwait(false);
        if (existing is null) dbSet.Add(model);
        else dbContext.Entry(existing).CurrentValues.SetValues(model);

        try {
            await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
            return RepoOutcome.True;
        }
        catch (DbUpdateConcurrencyException) {
            return RepoOutcome.FromError(RepositoryFailures.ConcurrencyConflict);
        }
    }

    public async ValueTask<RepoOutcome<KeyValueEntryModel>> TryGetByKeyAsync(string key, CancellationToken ct = default) {
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();

        KeyValueEntryModel? result = await dbSet.AsNoTracking()
            .FirstOrDefaultAsync(predicate: entry => entry.Key == key, ct).ConfigureAwait(false);

        if (result is null) return RepoOutcome<KeyValueEntryModel>.FromError(RepositoryFailures.ModelNotFound);
        return RepoOutcome<KeyValueEntryModel>.FromData(result);
    }

    public async ValueTask<RepoOutcome<int>> GetCountAsync(CancellationToken ct = default) {
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();
        int result = await dbSet.CountAsync(ct).ConfigureAwait(false);
        return RepoOutcome<int>.FromData(result);
    }

    public async ValueTask<RepoOutcome> RemoveAsync(string key, CancellationToken ct = default) {
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();
        KeyValueEntryModel? model = await dbSet.FindAsync([key], ct).ConfigureAwait(false);

        if (model is null)
            return RepoOutcome.FromError(RepositoryFailures.ModelNotFound);

        dbSet.Remove(model);
        await GetDbContext().SaveChangesAsync(ct).ConfigureAwait(false);
        return RepoOutcome.True;
    }
}
