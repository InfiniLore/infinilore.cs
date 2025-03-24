// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Api.Responses.Data.User.LoreScopes;

namespace InfiniLore.ServerClient.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInteractiveApiAccess {
    ValueTask<Result<LoreScopesResponse>> GetLoreScopesAsync(string userId, CancellationToken ct = default);
}
