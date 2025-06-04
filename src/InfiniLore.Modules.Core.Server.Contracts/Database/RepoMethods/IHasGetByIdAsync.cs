// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByIdAsync<T> where T : BasicModel {
    ValueTask<Outcome<T>> GetByIdAsync(Guid id, QueryConfig config = default, CancellationToken ct = default);
}
