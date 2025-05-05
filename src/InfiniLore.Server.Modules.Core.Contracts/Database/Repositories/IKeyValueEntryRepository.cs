// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Modules.Core.Database.RepoMethods;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IKeyValueEntryRepository :
    IUnitOfWorkRepository,
    IHasGetCountAsync {
    ValueTask<Result> TryAddOrUpdateAsync(KeyValueEntryModel model, CancellationToken ct = default);
    ValueTask<Result<KeyValueEntryModel>> TryGetByKeyAsync(string key, CancellationToken ct = default);
}
