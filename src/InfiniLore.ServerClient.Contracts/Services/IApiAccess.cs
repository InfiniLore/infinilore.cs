// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Api.Responses.Data.User.LoreScopes;

namespace InfiniLore.ServerClient.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IApiAccess {
    public ValueTask<Result<LoreScopesResponse>> GetLoreScopesAsync(Guid userId, CancellationToken ct = default);
}
