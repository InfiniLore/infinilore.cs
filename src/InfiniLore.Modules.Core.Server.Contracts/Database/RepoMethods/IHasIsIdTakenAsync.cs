// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasIsIdTakenAsync {
    ValueTask<RepoOutcome> IsIdTakenAsync(Guid id, CancellationToken ct = default);
    ValueTask<RepoOutcome> IsIdNotTakenAsync(Guid id, CancellationToken ct = default);
}
