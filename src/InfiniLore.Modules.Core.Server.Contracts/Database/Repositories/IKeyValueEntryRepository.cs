// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server.Database.RepoMethods;
using InfiniLore.Modules.Core.Server.Messaging;

namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IKeyValueEntryRepository :
    IUnitOfWorkRepository,
    IHasGetCountAsync {
    ValueTask<Result> TryAddOrUpdateAsync(KeyValueEntryModel model, CancellationToken ct = default);
    ValueTask<Result<KeyValueEntryModel>> TryGetByKeyAsync(string key, CancellationToken ct = default);
}
