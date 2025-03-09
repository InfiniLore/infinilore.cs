// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database.RepositoryMethods;
using InfiniLore.Server.Database.Models.Data.System;

namespace InfiniLore.Server.Contracts.Database.Repositories.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IKeyValueStoreRepository :
    IUnitOfWorkRepository,
    IHasGetCountAsync {
    ValueTask<RepoResult> TryAddOrUpdateAsync(KeyValueStore model, CancellationToken ct = default);
    ValueTask<RepoResult<KeyValueStore>> TryGetByKeyAsync(string key, CancellationToken ct = default);
}
