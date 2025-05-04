// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Modules.Core.Database.RepositoryMethods;

namespace InfiniLore.Server.Modules.Core.Database.Models;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IKeyValueEntryRepository :
    IUnitOfWorkRepository,
    IHasGetCountAsync {
    ValueTask<Result> TryAddOrUpdateAsync(IKeyValueEntry model, CancellationToken ct = default);
    ValueTask<Result<IKeyValueEntry>> TryGetByKeyAsync(string key, CancellationToken ct = default);
}
