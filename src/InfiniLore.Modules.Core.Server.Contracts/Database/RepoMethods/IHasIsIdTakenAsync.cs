// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasIsIdTakenAsync {
    ValueTask<Result> IsIdTakenAsync(Guid id, CancellationToken ct = default);
    ValueTask<Result> IsIdNotTakenAsync(Guid id, CancellationToken ct = default);
}
