// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Server.Contracts.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Can return the count of entries within it.
/// </summary>
public interface IHasGetCountAsync {
    /// <summary>
    /// Returns the count of entries within this entity.
    /// </summary>
    /// <remarks>
    /// Implementers should document this function on their own.
    /// </remarks>
    /// <returns>An <see langword="int"/> representing the count of entries within this entity.</returns>
    ValueTask<Result<int>> GetCountAsync(CancellationToken cancellationToken = default);
}
