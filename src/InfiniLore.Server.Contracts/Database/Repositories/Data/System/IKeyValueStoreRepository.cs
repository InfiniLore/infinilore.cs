// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
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
    ValueTask<Result> TryAddOrUpdateAsync(KeyValueEntry model, CancellationToken ct = default);
    ValueTask<Result<KeyValueEntry>> TryGetByKeyAsync(string key, CancellationToken ct = default);
}
