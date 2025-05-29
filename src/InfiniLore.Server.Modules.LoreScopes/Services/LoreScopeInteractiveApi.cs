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
    [FromKeyedServices(IMessageBroker.FromClaims)] IMessageBroker messageBroker
) : ILoreScopeInteractiveApi {
    public async ValueTask<Result> DeleteLoreScopesAsync(string loreScopeId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result.FromError("Invalid LoreScope Id");
        
        MessageResponse result = await messageBroker.DeleteLoreScopeAsync(parsedLoreScopeId, ct: ct);
        
        if (!result.TryGetAsState(out bool success)) Result.FromError("Failed to delete lorescope");
        return success;
    }
    
    public async ValueTask<PaginatedResult<ILoreScopeModel>> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) 
            return PaginatedResult<ILoreScopeModel>.FromError("Invalid userId");

        // Form and Execute Query
        MessageResponse<PaginatedData<LoreScopeModel>> result = await messageBroker.GetLoreScopesByOwnerAsync(
            parsedUserId,
            ct: ct
        );

        // Verify Response
        if (!result.TryGetAsSuccess(out PaginatedData<LoreScopeModel> paginatedData)) {
            logger.Warning("Failed to get LoreScopes for user {userId} because '{reason}'", userId, result.AsError.Value);
            return PaginatedResult<ILoreScopeModel>.FromError($"Failed to get LoreScopes for user {userId}");
        }
    
        // Get image URLs for all lorescopes that have poster images
        foreach (LoreScopeModel loreScope in paginatedData.Items) {
            if (loreScope.PosterImageMetaDataId is null) continue;

            MessageResponse<string> imageUrlResponse = await messageBroker.GetLorescopePosterImageAsync(loreScope.Id, ct);
            if (!imageUrlResponse.TryGetAsSuccess(out string imageUrl)) continue;
            loreScope.S3PosterImageUrl = imageUrl;
        }

        PaginatedData<ILoreScopeModel> casted = paginatedData.CastTo<ILoreScopeModel>();
        return PaginatedResult<ILoreScopeModel>.FromData(casted);
    }


    public async ValueTask<Result> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result.FromError("Invalid userId");
        
        
        MessageResponse<Guid> createResult = await messageBroker.CreateLoreScopeAsync(parsedUserId, newLoreScopeName, ct: ct);
        if (createResult.TryGetAsSuccess(out Guid _)) return true;

        logger.Warning("Failed to create lorescope for user {userId} because '{reason}'", userId, createResult.AsError.Value);
        return Result.FromError($"Failed to create lorescope for user {userId}");

    }
}
