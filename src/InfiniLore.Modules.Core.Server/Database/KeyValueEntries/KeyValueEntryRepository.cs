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
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();

        // Query & Retrieve
        KeyValueEntryModel? existing = await dbSet.FindAsync([model.Key], ct);
        if (existing is null) dbSet.Add(model);
        else dbContext.Entry(existing).CurrentValues.SetValues(model);

        await dbContext.SaveChangesAsync(ct);
        return RepoOutcome.True;
    }

    public async ValueTask<RepoOutcome<KeyValueEntryModel>> TryGetByKeyAsync(string key, CancellationToken ct = default) {
        // Access
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();

        // Query
        KeyValueEntryModel? result = await dbSet.AsNoTracking()
            .FirstOrDefaultAsync(predicate: ls => ls.Key == key, ct);

        // Retrieve
        if (result is null) return RepoOutcome<KeyValueEntryModel>.FromError(RepositoryFailures.ModelNotFound);

        return RepoOutcome<KeyValueEntryModel>.FromData(result);
    }

    public async ValueTask<RepoOutcome<int>> GetCountAsync(CancellationToken ct = default) {
        // Access
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();

        // Query
        int result = await dbSet.CountAsync(cancellationToken: ct);

        // Retrieve
        return RepoOutcome<int>.FromData(result);
    }
}
