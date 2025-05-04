// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database.Models;

namespace InfiniLore.Server.Modules.Core.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByIdAsync<T> where T : IBasicData {
    ValueTask<Result<T>> GetByIdAsync(Guid id, QueryConfig config = default, CancellationToken ct = default);
}
