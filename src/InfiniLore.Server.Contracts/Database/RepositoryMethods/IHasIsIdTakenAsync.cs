// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Server.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasIsIdTakenAsync {
    ValueTask<Result> IsIdTakenAsync(Guid id, CancellationToken ct = default);
    ValueTask<Result> IsIdNotTakenAsync(Guid id, CancellationToken ct = default);
}
