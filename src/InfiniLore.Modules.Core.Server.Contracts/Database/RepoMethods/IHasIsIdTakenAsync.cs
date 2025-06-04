// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasIsIdTakenAsync {
    ValueTask<Outcome> IsIdTakenAsync(Guid id, CancellationToken ct = default);
    ValueTask<Outcome> IsIdNotTakenAsync(Guid id, CancellationToken ct = default);
}
