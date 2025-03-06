// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
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
    public async ValueTask<RepoResult> TryAddOrUpdateAsync(KeyValueStore model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<KeyValueStore> dbSet = dbContext.KeyValueStores;

        // Query & Retrieve
        KeyValueStore? existing = await dbSet.FindAsync([model.Key], ct);
        if (existing is null) dbSet.Add(model);
        else dbContext.Entry(existing).CurrentValues.SetValues(model);

        await dbContext.SaveChangesAsync(ct);
        return true;
    }
    
    public async ValueTask<RepoResult<KeyValueStore>> TryGetByKeyAsync(string key, CancellationToken ct = default) {
        // Access
        DbSet<KeyValueStore> dbSet = GetDbContext().KeyValueStores;

        // Query
        KeyValueStore? result = await dbSet.AsNoTracking()
            .FirstOrDefaultAsync(ls => ls.Key == key, cancellationToken: ct);

        // Retrieve
        if (result is null) return RepoResult<KeyValueStore>.FromError(RepositoryFailures.ModelNotFound);
        return RepoResult<KeyValueStore>.FromSuccess(result);
    }

    public async ValueTask<RepoResult<int>> TryCountAsync(CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<KeyValueStore> dbSet = dbContext.KeyValueStores;

        // Query
        int result = await dbSet.CountAsync(cancellationToken: ct);

        // Retrieve
        return RepoResult<int>.FromSuccess(result);
    }
}
