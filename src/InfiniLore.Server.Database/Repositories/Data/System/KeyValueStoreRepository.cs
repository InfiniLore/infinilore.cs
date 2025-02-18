// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Database.Repositories.Data.System;
using InfiniLore.Server.Database.Models.Data.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Database.Repositories.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IKeyValueStoreRepository>(ServiceLifetime.Scoped)]
public class KeyValueStoreRepository : SystemDataRepository<KeyValueStore>, IKeyValueStoreRepository {
    protected override async ValueTask<bool> IsNotUniqueAsync(KeyValueStore[] modelsToValidate, CancellationToken ct = default) {
        HashSet<Guid> ids = modelsToValidate.Select(m => m.Id).ToHashSet();
        HashSet<string> keys = modelsToValidate.Select(m => m.Key).ToHashSet();

        IQueryable<KeyValueStore> query = GetDbContext().KeyValueStores
            .AsNoTracking()
            .Where(foundModel => ids.Contains(foundModel.Id) || keys.Contains(foundModel.Key)); // Avoids joins here

        return await query.AnyAsync(ct);
    }
}
