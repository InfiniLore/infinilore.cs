// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.UserData;
using InfiniLoreLib.Results;

namespace InfiniLore.Server.Contracts.Data.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopesRepository {
    Task<Result<bool>> DeleteAsync(Guid loreScopeId, CancellationToken ct = default);
    Task<Result<bool>> DeleteAsync(LoreScopeModel model, CancellationToken ct = default);
}
