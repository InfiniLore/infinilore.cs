// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server.Database.RepoMethods;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IKeyValueEntryRepository :
    IUnitOfWorkRepository,
    IHasGetCountAsync {
    ValueTask<RepoOutcome> TryAddOrUpdateAsync(KeyValueEntryModel model, CancellationToken ct = default);
    ValueTask<RepoOutcome<KeyValueEntryModel>> TryGetByKeyAsync(string key, CancellationToken ct = default);
    ValueTask<RepoOutcome> RemoveAsync(string key, CancellationToken ct = default);
}
