// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
using InfiniLore.Server.Services;
using InfiniLore.Shared;
using InfiniLore.Shared.Modules.LoreScopes.Database;
using InfiniLore.Shared.Modules.LoreScopes.Services;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<ILoreScopeInteractiveApi>]
public class LoreScopeInteractiveApi(
    ILogger<LoreScopeInteractiveApi> logger,
    IInteractiveApiServer interactiveApi,
    IMessageAccessFactory requestDataFactory
) : ILoreScopeInteractiveApi {
    public async ValueTask<Result<PaginatedData<ILoreScopeModel>>> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result<PaginatedData<ILoreScopeModel>>.FromError("Invalid userId");

        // Form Message
        var query = new GetLoreScopesQuery(
            parsedUserId,
            false,
            PaginationInfo.Default
        ) {
            Access = requestDataFactory.FromClaims(ct)
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

    public async ValueTask<Result> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result.FromError("Invalid userId");

        var request = new LoreScopeCreateRequest(parsedUserId, newLoreScopeName) {
            Access = requestDataFactory.FromClaims(ct)
        };
        MessageResponse<Guid> createResult = await request.ExecuteAsync(ct: ct);
        if (!createResult.TryGetAsSuccess(out Guid loreScopeId)) {
            logger.Warning("Failed to create lorescope for user {userId} because '{reason}'", userId, createResult.AsError.Value);
            return Result.FromError($"Failed to create lorescope for user {userId}");
        }

        return true;
    }
}
