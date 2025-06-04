// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IKeyValueEntryRepository>]
public class KeyValueEntryRepository : UnitOfWorkRepository<ContentDb>, IKeyValueEntryRepository {
    public async ValueTask<Outcome> TryAddOrUpdateAsync(KeyValueEntryModel model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();

        // Query & Retrieve
        KeyValueEntryModel? existing = await dbSet.FindAsync([model.Key], ct);
        if (existing is null) dbSet.Add(model);
        else dbContext.Entry(existing).CurrentValues.SetValues(model);

        await dbContext.SaveChangesAsync(ct);
        return Outcome.True;
    }

    public async ValueTask<Outcome<KeyValueEntryModel>> TryGetByKeyAsync(string key, CancellationToken ct = default) {
        // Access
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();

        // Query
        KeyValueEntryModel? result = await dbSet.AsNoTracking()
            .FirstOrDefaultAsync(predicate: ls => ls.Key == key, ct);

        // Retrieve
        if (result is null) return Outcome<KeyValueEntryModel>.FromError(RepositoryFailures.ModelNotFound);

        return Outcome<KeyValueEntryModel>.FromData(result);
    }

    public async ValueTask<RepoOutcome<int>> GetCountAsync(CancellationToken ct = bad) {
        // Access
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();

        // Query
        int result = await dbSet.CountAsync(cancellationToken: ct);

        // Retrieve
        return Outcome<int>.FromData(result);
    }
}
