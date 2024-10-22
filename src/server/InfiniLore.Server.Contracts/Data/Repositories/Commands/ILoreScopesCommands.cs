// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.UserData;
using InfiniLoreLib.Results;

namespace InfiniLore.Server.Contracts.Data.Repositories.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopesCommands {
    Task<Result<bool>> DeleteAsync(Guid loreScopeId, CancellationToken ct = default);
    Task<Result<bool>> DeleteAsync(LoreScopeModel model, CancellationToken ct = default);
}
