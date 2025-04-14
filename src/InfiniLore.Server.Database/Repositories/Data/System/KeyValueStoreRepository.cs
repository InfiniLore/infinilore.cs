// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Data.System;
using InfiniLore.Server.Database.Models.Data.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Database.Repositories.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IKeyValueStoreRepository>(ServiceLifetime.Scoped)]
public class KeyValueStoreRepository : UnitOfWorkRepository<ContentDb>, IKeyValueStoreRepository {
    public async ValueTask<Result> TryAddOrUpdateAsync(KeyValueEntry model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<KeyValueEntry> dbSet = GetCachedDbSet<KeyValueEntry>();

        // Query & Retrieve
        KeyValueEntry? existing = await dbSet.FindAsync([model.Key], ct);
        if (existing is null) dbSet.Add(model);
        else dbContext.Entry(existing).CurrentValues.SetValues(model);

        await dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async ValueTask<Result<KeyValueEntry>> TryGetByKeyAsync(string key, CancellationToken ct = default) {
        // Access
        DbSet<KeyValueEntry> dbSet = GetCachedDbSet<KeyValueEntry>();

        // Query
        KeyValueEntry? result = await dbSet.AsNoTracking()
            .FirstOrDefaultAsync(predicate: ls => ls.Key == key, ct);

        // Retrieve
        if (result is null) return Result<KeyValueEntry>.FromError(RepositoryFailures.ModelNotFound);

        return Result<KeyValueEntry>.FromSuccess(result);
    }

    public async ValueTask<Result<int>> GetCountAsync(CancellationToken ct = default) {
        // Access
        DbSet<KeyValueEntry> dbSet = GetCachedDbSet<KeyValueEntry>();

        // Query
        int result = await dbSet.CountAsync(cancellationToken: ct);

        // Retrieve
        return Result<int>.FromSuccess(result);
    }
}
