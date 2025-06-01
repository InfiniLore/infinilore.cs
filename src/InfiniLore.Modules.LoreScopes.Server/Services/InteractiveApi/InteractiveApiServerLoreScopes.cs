// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.LoreScopes.Shared.Database;
using InfiniLore.Modules.LoreScopes.Shared.Services;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LoreScopes.Server.InteractiveApi;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiLoreScopes>]
public class InteractiveApiServerLoreScopes(
    ILogger<InteractiveApiServerLoreScopes> logger,
    IInteractiveApiServer interactiveApi
) : IInteractiveApiLoreScopes {
    public async ValueTask<Result> DeleteLoreScopesAsync(string userId, string loreScopeId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result.FromError("Invalid LoreScope Id");
        
        MessageResponse result = await interactiveApi.MessageBroker.DeleteLoreScopeAsync(parsedLoreScopeId, ct: ct);
        
        if (!result.TryGetAsState(out bool? success)) Result.FromError("Failed to delete lorescope");
        return success ?? false;
    }

    public async ValueTask<Result<ILoreScopeModel>> GetLoreScopeAsync(string userId, string loreScopeId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result<ILoreScopeModel>.FromError("Invalid userId");
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result<ILoreScopeModel>.FromError("Invalid lorescopeId");
        
        MessageResponse<LoreScopeModel> result = await interactiveApi.MessageBroker.GetLorescopeByIdAsync(parsedLoreScopeId, parsedUserId, ct: ct);
        
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (!result.TryGetAsSuccess(out LoreScopeModel? loreScope)) return Result<ILoreScopeModel>.FromError("Failed to get lorescope");
        
        if (loreScope.PosterImageMetaDataId is null) return loreScope;

        MessageResponse<string> imageUrlResponse = await interactiveApi.MessageBroker.GetLorescopePosterImageAsync(loreScope.Id, ct);
        if (!imageUrlResponse.TryGetAsSuccess(out string? imageUrl)) {
            logger.Warning("Failed to get lorescope poster image for lorescope {lorescopeId} because '{reason}'", loreScopeId, imageUrlResponse.AsError.Value);
            return loreScope;
        }
        
        loreScope.S3PosterImageUrl = imageUrl;
        return loreScope;
    }
    
    public async ValueTask<PaginatedResult<ILoreScopeModel>> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) 
            return PaginatedResult<ILoreScopeModel>.FromError("Invalid userId");

        // Form and Execute Query
        MessageResponse<PaginatedData<LoreScopeModel>> result = await interactiveApi.MessageBroker.GetLoreScopesByOwnerAsync(
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

            MessageResponse<string> imageUrlResponse = await interactiveApi.MessageBroker.GetLorescopePosterImageAsync(loreScope.Id, ct);
            if (!imageUrlResponse.TryGetAsSuccess(out string? imageUrl)) continue;
            loreScope.S3PosterImageUrl = imageUrl;
        }

        PaginatedData<ILoreScopeModel> casted = paginatedData.CastTo<ILoreScopeModel>();
        return PaginatedResult<ILoreScopeModel>.FromData(casted);
    }


    public async ValueTask<Result> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result.FromError("Invalid userId");
        
        
        MessageResponse<Guid> createResult = await interactiveApi.MessageBroker.CreateLoreScopeAsync(parsedUserId, newLoreScopeName, ct: ct);
        if (createResult.TryGetAsSuccess(out Guid _)) return true;

        logger.Warning("Failed to create lorescope for user {userId} because '{reason}'", userId, createResult.AsError.Value);
        return Result.FromError($"Failed to create lorescope for user {userId}");
    }
    
    public async ValueTask<Result> UpsertLoreScopeImageAsync(string userId, string loreScopeId, IBrowserFile fileStream, CancellationToken ct = default) {
        // if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result.FromError("Invalid userId");
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result.FromError("Invalid lorescopeId");

        await using Stream stream = fileStream.OpenReadStream(cancellationToken: ct);
        MessageResponse result = await interactiveApi.MessageBroker.UpsertLoreScopeImageAsync(
            parsedLoreScopeId,
            fileStream.Name,
            fileStream.ContentType,
            stream,
            ct: ct);
        if (result.TryGetAsState(out bool? success)) return (Result)success;
        
        logger.Warning("Failed to upsert lorescope image for user {userId} because '{reason}'", userId, result.AsError.Value);
        return Result.FromError($"Failed to upsert lorescope image for user {userId}");
    }
}
