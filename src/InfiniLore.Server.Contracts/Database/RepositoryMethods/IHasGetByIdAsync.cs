// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByIdAsync<T> where T : IBasicData {
    ValueTask<Result<T>> GetByIdAsync(Guid id, QueryConfig config = default, CancellationToken ct = default);
}
