// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Api.Responses.Data.User.LoreScopes;

namespace InfiniLore.ServerClient.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IApiData {
    ValueTask<Result<LoreScopesResponse>> GetLoreScopesAsync(Guid userId, CancellationToken ct = default);
}
