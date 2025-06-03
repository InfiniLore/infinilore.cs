// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasIsIdTakenAsync {
    ValueTask<Shared.Outcome> IsIdTakenAsync(Guid id, CancellationToken ct = default);
    ValueTask<Shared.Outcome> IsIdNotTakenAsync(Guid id, CancellationToken ct = default);
}
