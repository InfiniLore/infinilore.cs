// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Shared;
using InfiniLore.Shared.Modules.LoreScopes.Database;
using InfiniLore.Shared.Modules.LoreScopes.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<ILoreScopeInteractiveApi>]
public class LoreScopeInteractiveApi(
    ILogger<LoreScopeInteractiveApi> logger,
    [FromKeyedServices(IMessageBroker.Claims)] IMessageBroker messageBroker
) : ILoreScopeInteractiveApi {
    public async ValueTask<PaginatedDataResult<ILoreScopeModel>> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return PaginatedDataResult<ILoreScopeModel>.FromError("Invalid userId");

        // Form and Execute Query
        MessageResponse<PaginatedData<LoreScopeModel>> result = await messageBroker.GetLoreScopesAsync(
            parsedUserId,
            ct: ct
        );

        // Verify Response
        // ReSharper disable once InvertIf
        if (!result.TryGetAsSuccess(out PaginatedData<LoreScopeModel> paginatedData)) {
            logger.Warning("Failed to get LoreScopes for user {userId} because '{reason}'", userId, result.AsError.Value);
            return PaginatedDataResult<ILoreScopeModel>.FromError($"Failed to get LoreScopes for user {userId}");
        }

        return PaginatedDataResult<ILoreScopeModel>.FromData(paginatedData.CastTo<ILoreScopeModel>());
    }

    public async ValueTask<Result> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result.FromError("Invalid userId");
        
        
        MessageResponse<Guid> createResult = await messageBroker.CreateLoreScopeAsync(parsedUserId, newLoreScopeName, ct: ct);
        if (createResult.TryGetAsSuccess(out Guid _)) return true;

        logger.Warning("Failed to create lorescope for user {userId} because '{reason}'", userId, createResult.AsError.Value);
        return Result.FromError($"Failed to create lorescope for user {userId}");

    }
}
