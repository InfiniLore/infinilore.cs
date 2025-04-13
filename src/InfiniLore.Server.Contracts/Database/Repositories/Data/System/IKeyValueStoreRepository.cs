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
/// <summary>
/// a repository of <see cref="KeyValueStore">KeyValueStores</see>
/// </summary>
public interface IKeyValueStoreRepository :
    IUnitOfWorkRepository,
    IHasGetCountAsync {
    /// <summary>
    /// Adds or updates <paramref name="store"/> with an optional <paramref name="cancellationToken"/>
    /// </summary>
    /// <param name="store">The store which should be added or updated.</param>
    /// <returns>a <see cref="Result"/> indicating if the operation was successful or not</returns>
    ValueTask<Result> TryAddOrUpdateAsync(KeyValueStore store, CancellationToken cancellationToken = default);
    /// <summary>
    /// Tries to retrieve a <see cref="KeyValueStore"/> with the <see cref="KeyValueStore.Key"/> equal to <paramref name="key"/>.
    /// </summary>
    /// <returns>a <see cref="Result{T}"/> with a <see cref="KeyValueStore"/> if successful.</returns>
    ValueTask<Result<KeyValueStore>> TryGetByKeyAsync(string key, CancellationToken cancellationToken = default);
}
