// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server.Database.RepoMethods;
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IKeyValueEntryRepository :
    IUnitOfWorkRepository,
    IHasGetCountAsync {
    ValueTask<Outcome> TryAddOrUpdateAsync(KeyValueEntryModel model, CancellationToken ct = default);
    ValueTask<Outcome<KeyValueEntryModel>> TryGetByKeyAsync(string key, CancellationToken ct = default);
}
