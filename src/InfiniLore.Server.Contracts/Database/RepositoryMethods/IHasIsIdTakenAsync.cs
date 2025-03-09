// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasIsIdTakenAsync {
    ValueTask<RepoResult> IsIdTakenAsync(Guid id, CancellationToken ct = default);
    ValueTask<RepoResult> IsIdNotTakenAsync(Guid id, CancellationToken ct = default);
}
