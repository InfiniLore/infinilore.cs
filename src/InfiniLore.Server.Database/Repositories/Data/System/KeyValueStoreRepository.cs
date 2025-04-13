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
    /// <returns> <see langword="true"/> if the object was added or updated sucessfully.</returns>
    /// <inheritdoc/>
    public async ValueTask<Result> TryAddOrUpdateAsync(KeyValueStore model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<KeyValueStore> dbSet = GetCachedDbSet<KeyValueStore>();

        // Query & Retrieve
        KeyValueStore? existing = await dbSet.FindAsync([model.Key], ct);
        if (existing is null) dbSet.Add(model);
        else dbContext.Entry(existing).CurrentValues.SetValues(model);

        await dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async ValueTask<Result<KeyValueStore>> TryGetByKeyAsync(string key, CancellationToken ct = default) {
        // Access
        DbSet<KeyValueStore> dbSet = GetCachedDbSet<KeyValueStore>();

        // Query
        KeyValueStore? result = await dbSet.AsNoTracking()
            .FirstOrDefaultAsync(predicate: ls => ls.Key == key, ct);

        // Retrieve
        if (result is null) return Result<KeyValueStore>.FromError(RepositoryFailures.ModelNotFound);

        return Result<KeyValueStore>.FromSuccess(result);
    }
    /// <summary>
    /// Counts the amount of <see cref="KeyValueStore">KeyValueStores</see> in the database.
    /// </summary>
    /// <returns>Returns the amount of <see cref="KeyValueStore">KeyValueStores</see> in the database.</returns>
    public async ValueTask<Result<int>> GetCountAsync(CancellationToken ct = default) {
        // Access
        DbSet<KeyValueStore> dbSet = GetCachedDbSet<KeyValueStore>();

        // Query
        int result = await dbSet.CountAsync(cancellationToken: ct);

        // Retrieve
        return Result<int>.FromSuccess(result);
    }
}
