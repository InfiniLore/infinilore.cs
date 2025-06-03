// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.LoreScopes.Shared.Database;
using InfiniLore.Modules.LoreScopes.Shared.Services;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Shared;
using InfiniLore.Shared.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Result=InfiniLore.Modules.Core.Server.Result;

namespace InfiniLore.Modules.LoreScopes.Server.InteractiveApi;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiLoreScopes>]
public class InteractiveApiServerLoreScopes(
    ILogger<InteractiveApiServerLoreScopes> logger,
    IInteractiveApiServer interactiveApi
) : IInteractiveApiLoreScopes {
    private const int MaxFileSize = 5 * 1024 * 1024; // 5MB in bytes
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<AterraEngine.Unions.Result> DeleteLoreScopesAsync(string userId, string loreScopeId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return AterraEngine.Unions.Result.FromError("Invalid LoreScope Id");
        
        Result result = await interactiveApi.MessageBroker.DeleteLoreScopeAsync(parsedLoreScopeId, ct: ct);
        
        if (!result.TryGetAsState(out bool? success)) AterraEngine.Unions.Result.FromError("Failed to delete lorescope");
        return success ?? false;
    }

    public async ValueTask<AterraEngine.Unions.Result<ILoreScopeModel>> GetLoreScopeAsync(string userId, string loreScopeId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return AterraEngine.Unions.Result<ILoreScopeModel>.FromError("Invalid userId");
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return AterraEngine.Unions.Result<ILoreScopeModel>.FromError("Invalid lorescopeId");

        Core.Server.Result<LoreScopeModel> result = await interactiveApi.MessageBroker.GetLorescopeByIdAsync(parsedLoreScopeId, parsedUserId, ct: ct);
        
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (!result.TryGetAsSuccess(out LoreScopeModel? loreScope)) return AterraEngine.Unions.Result<ILoreScopeModel>.FromError("Failed to get lorescope");
        
        if (loreScope.PosterImageMetaDataId is null) return loreScope;

        Core.Server.Result<string> imageUrlResponse = await interactiveApi.MessageBroker.GetLorescopePosterImageAsync(loreScope.Id, ct);
        if (!imageUrlResponse.TryGetAsSuccess(out string? imageUrl)) {
            logger.Warning("Failed to get lorescope poster image for lorescope {lorescopeId} because '{reason}'", loreScopeId, imageUrlResponse.AsError.Value);
            return loreScope;
        }
        
        loreScope.S3PosterImageUrl = imageUrl;
        return loreScope;
    }
    
    public async ValueTask<InfiniLore.Shared.PaginatedResult<ILoreScopeModel>> GetLoreScopesAsync(string userId, Pagination pagination, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) 
            return InfiniLore.Shared.PaginatedResult<ILoreScopeModel>.FromError("Invalid userId");

        // Form and Execute Query
        Core.Server.Result<PaginatedData<LoreScopeModel>> result = await interactiveApi.MessageBroker.GetLoreScopesByOwnerAsync(
            parsedUserId,
            pagination:pagination,
            ct: ct
        );

        // Verify Response
        if (!result.TryGetAsSuccess(out PaginatedData<LoreScopeModel> paginatedData)) {
            logger.Warning("Failed to get LoreScopes for user {userId} because '{reason}'", userId, result.AsError.Value);
            return InfiniLore.Shared.PaginatedResult<ILoreScopeModel>.FromError($"Failed to get LoreScopes for user {userId}");
        }
    
        // Get image URLs for all lorescopes that have poster images
        foreach (LoreScopeModel loreScope in paginatedData.Items) {
            if (loreScope.PosterImageMetaDataId is null) continue;

            Core.Server.Result<string> imageUrlResponse = await interactiveApi.MessageBroker.GetLorescopePosterImageAsync(loreScope.Id, ct);
            if (!imageUrlResponse.TryGetAsSuccess(out string? imageUrl)) continue;
            loreScope.S3PosterImageUrl = imageUrl;
        }

        PaginatedData<ILoreScopeModel> casted = paginatedData.CastTo<ILoreScopeModel>();
        return InfiniLore.Shared.PaginatedResult<ILoreScopeModel>.FromData(casted);
    }


    public async ValueTask<AterraEngine.Unions.Result> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return AterraEngine.Unions.Result.FromError("Invalid userId");


        Core.Server.Result<Guid> createResult = await interactiveApi.MessageBroker.CreateLoreScopeAsync(parsedUserId, newLoreScopeName, ct: ct);
        if (createResult.TryGetAsSuccess(out Guid _)) return true;

        logger.Warning("Failed to create lorescope for user {userId} because '{reason}'", userId, createResult.AsError.Value);
        return AterraEngine.Unions.Result.FromError($"Failed to create lorescope for user {userId}");
    }
    
    public async ValueTask<AterraEngine.Unions.Result> UpsertLoreScopeImageAsync(string userId, string loreScopeId, IBrowserFile fileStream, CancellationToken ct = default) {
        // if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result.FromError("Invalid userId");
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return AterraEngine.Unions.Result.FromError("Invalid lorescopeId");

        await using MemoryStream stream = await fileStream.ToMemoryStreamAsync(MaxFileSize, ct: ct);
        Result result = await interactiveApi.MessageBroker.UpsertLoreScopeImageAsync(
            parsedLoreScopeId,
            fileStream.Name,
            fileStream.ContentType,
            stream,
            ct: ct);
        if (result.TryGetAsState(out bool? success)) return (AterraEngine.Unions.Result)success;
        
        logger.Warning("Failed to upsert lorescope image for user {userId} because '{reason}'", userId, result.AsError.Value);
        return AterraEngine.Unions.Result.FromError($"Failed to upsert lorescope image for user {userId}");
    }
}
