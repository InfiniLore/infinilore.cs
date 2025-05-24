// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IKeyValueEntryRepository>]
public class KeyValueEntryRepository : UnitOfWorkRepository<ContentDb>, IKeyValueEntryRepository {
    public async ValueTask<Result> TryAddOrUpdateAsync(KeyValueEntryModel model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();

        // Query & Retrieve
        KeyValueEntryModel? existing = await dbSet.FindAsync([model.Key], ct);
        if (existing is null) dbSet.Add(model);
        else dbContext.Entry(existing).CurrentValues.SetValues(model);

        await dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async ValueTask<Result<KeyValueEntryModel>> TryGetByKeyAsync(string key, CancellationToken ct = default) {
        // Access
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();

        // Query
        KeyValueEntryModel? result = await dbSet.AsNoTracking()
            .FirstOrDefaultAsync(predicate: ls => ls.Key == key, ct);

        // Retrieve
        if (result is null) return Result<KeyValueEntryModel>.FromError(RepositoryFailures.ModelNotFound);

        return Result<KeyValueEntryModel>.FromSuccess(result);
    }

    public async ValueTask<Result<int>> GetCountAsync(CancellationToken ct = default) {
        // Access
        DbSet<KeyValueEntryModel> dbSet = GetCachedDbSet<KeyValueEntryModel>();

        // Query
        int result = await dbSet.CountAsync(cancellationToken: ct);

        // Retrieve
        return Result<int>.FromSuccess(result);
    }
}
