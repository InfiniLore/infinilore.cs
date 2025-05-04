// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database;
using InfiniLore.Server.Modules.Core.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IKeyValueEntryRepository>(ServiceLifetime.Scoped)]
public class KeyValueEntryRepository : UnitOfWorkRepository<ContentDb>, IKeyValueEntryRepository {
    public async ValueTask<Result> TryAddOrUpdateAsync(IKeyValueEntry model, CancellationToken ct = default) {
        // Access
        ContentDb dbContext = GetDbContext();
        DbSet<IKeyValueEntry> dbSet = GetCachedDbSet<IKeyValueEntry>();

        // Query & Retrieve
        IKeyValueEntry? existing = await dbSet.FindAsync([model.Key], ct);
        if (existing is null) dbSet.Add(model);
        else dbContext.Entry(existing).CurrentValues.SetValues(model);

        await dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async ValueTask<Result<IKeyValueEntry>> TryGetByKeyAsync(string key, CancellationToken ct = default) {
        // Access
        DbSet<IKeyValueEntry> dbSet = GetCachedDbSet<IKeyValueEntry>();

        // Query
        IKeyValueEntry? result = await dbSet.AsNoTracking()
            .FirstOrDefaultAsync(predicate: ls => ls.Key == key, ct);

        // Retrieve
        if (result is null) return Result<IKeyValueEntry>.FromError(RepositoryFailures.ModelNotFound);

        return Result<IKeyValueEntry>.FromSuccess(result);
    }

    public async ValueTask<Result<int>> GetCountAsync(CancellationToken ct = default) {
        // Access
        DbSet<IKeyValueEntry> dbSet = GetCachedDbSet<IKeyValueEntry>();

        // Query
        int result = await dbSet.CountAsync(cancellationToken: ct);

        // Retrieve
        return Result<int>.FromSuccess(result);
    }
}
