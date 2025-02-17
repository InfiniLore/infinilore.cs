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
public class KeyValueStoreRepository : SystemDataRepository<KeyValueStore>, IKeyValueStoreRepository{
    protected override async ValueTask<bool> IsNotUniqueAsync(KeyValueStore originalModel, CancellationToken ct = default) {
        ContentDb dbContext = GetDbContext();
        return await dbContext.KeyValueStores.AnyAsync(foundModel =>
            foundModel.Id == originalModel.Id
            && foundModel.Key == originalModel.Key ,
            ct
        );
    }

    protected override async ValueTask<bool> IsNotUniqueRangeAsync(KeyValueStore[] originalModels, CancellationToken ct = default) {
        ContentDb dbContext = GetDbContext();
        return await dbContext.KeyValueStores.AnyAsync(foundModel =>
            originalModels.Select(m => m.Id).Contains(foundModel.Id)
            && originalModels.Select(m => m.Key).Contains(foundModel.Key),
            ct
        );
    }
}
