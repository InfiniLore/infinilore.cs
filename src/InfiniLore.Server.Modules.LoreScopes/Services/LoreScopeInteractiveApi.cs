// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Core.Services;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
using InfiniLore.Server.Services;
using InfiniLore.Shared;
using InfiniLore.Shared.Modules.LoreScopes.Database;
using InfiniLore.Shared.Modules.LoreScopes.Services;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<ILoreScopeInteractiveApi>]
public class LoreScopeInteractiveApi(
    ILogger<LoreScopeInteractiveApi> logger,
    IInteractiveApiServer interactiveApi,
    IRequestDataFactory requestDataFactory
) : ILoreScopeInteractiveApi {
    public async ValueTask<Result<PaginatedData<ILoreScopeModel>>> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result<PaginatedData<ILoreScopeModel>>.FromError("Invalid userId");

        // Form Message
        var query = new GetLoreScopesQuery(
            parsedUserId,
            false,
            PaginationInfo.Default
        ) {
            AccessData = requestDataFactory.FromClaims(ct)
        };

        // Execute Message
        MessageResponse<PaginatedData<LoreScopeModel>> result = await query.ExecuteAsync(ct);

        // Verify Response
        // ReSharper disable once InvertIf
        if (!result.TryGetAsSuccess(out PaginatedData<LoreScopeModel> paginatedData)) {
            logger.Warning("Failed to get LoreScopes for user {userId} because '{reason}'", userId, result.AsError.Value);
            return Result<PaginatedData<ILoreScopeModel>>.FromError($"Failed to get LoreScopes for user {userId}");
        }

        return Result<PaginatedData<ILoreScopeModel>>.FromSuccess(paginatedData.CastTo<ILoreScopeModel>());
    }

}
