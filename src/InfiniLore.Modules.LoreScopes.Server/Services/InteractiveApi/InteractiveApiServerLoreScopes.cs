// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.LoreScopes.Shared.Database;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.Core.Shared.Extensions;
using InfiniLore.Modules.LoreScopes.Shared;
using InfiniLore.Server.Modules.LoreScopes.Database;
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
    private const int MaxFileSize = 5 * 1024 * 1024; // 5MB in bytes
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Outcome> DeleteLoreScopesAsync(string userId, string loreScopeId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Outcome.FromError("Invalid LoreScope Id");
        
        Outcome outcome = await interactiveApi.MessageBroker.DeleteLoreScopeAsync(parsedLoreScopeId, ct: ct);
        
        if (!outcome.TryGetAsState(out bool success)) Outcome.FromError("Failed to delete lorescope");
        return Outcome.FromState(success);
    }
    
    public async ValueTask<Outcome> UpdateLoreScopeMetaDataAsync(string userId, string loreScopeId, string? newLoreScopeName = null, string? newDescription = null, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Outcome.FromError("Invalid lorescopeId");
        if (newLoreScopeName.IsNullOrWhiteSpace()) return Outcome.FromError("Invalid lorescope name");
        
        Outcome outcome = await interactiveApi.MessageBroker.UpsertLoreScopeMetadataAsync(
            parsedLoreScopeId,
            name: newLoreScopeName,
            description: newDescription,
            ct: ct
        );
        return outcome;
    }

    public async ValueTask<Outcome<ILoreScopeModel>> GetLoreScopeAsync(string userId, string loreScopeId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Outcome<ILoreScopeModel>.FromError("Invalid userId");
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Outcome<ILoreScopeModel>.FromError("Invalid lorescopeId");

        Outcome<LoreScopeModel> outcome = await interactiveApi.MessageBroker.GetLorescopeByIdAsync(parsedLoreScopeId, parsedUserId, ct: ct);
        
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (!outcome.TryGetAsData(out LoreScopeModel? loreScope)) return Outcome<ILoreScopeModel>.FromError("Failed to get lorescope");
        
        if (loreScope.PosterImageMetaDataId is null) return loreScope;

        Outcome<string> imageUrlResponse = await interactiveApi.MessageBroker.GetLorescopePosterImageAsync(loreScope.Id, ct);
        if (!imageUrlResponse.TryGetAsData(out string? imageUrl)) {
            logger.Warning("Failed to get lorescope poster image for lorescope {lorescopeId} because '{reason}'", loreScopeId, imageUrlResponse.AsError.Value);
            return loreScope;
        }
        
        loreScope.S3PosterImageUrl = imageUrl;
        return loreScope;
    }
    
    public async ValueTask<PaginatedOutcome<ILoreScopeModel>> GetLoreScopesAsync(string userId, Pagination pagination, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) 
            return PaginatedOutcome<ILoreScopeModel>.FromError("Invalid userId");

        // Form and Execute Query
        PaginatedOutcome<LoreScopeModel> outcome = await interactiveApi.MessageBroker.GetLoreScopesByOwnerAsync(
            parsedUserId,
            pagination:pagination,
            ct: ct
        );

        // Verify Response
        if (!outcome.TryGetAsData(out PaginatedData<LoreScopeModel>? paginatedData)) {
            logger.Warning("Failed to get LoreScopes for user {userId} because '{reason}'", userId, outcome.AsError.Value);
            return PaginatedOutcome<ILoreScopeModel>.FromError($"Failed to get LoreScopes for user {userId}");
        }
    
        // Get image URLs for all lorescopes that have poster images
        foreach (LoreScopeModel loreScope in paginatedData.Items) {
            if (loreScope.PosterImageMetaDataId is null) continue;

            Outcome<string> imageUrlResponse = await interactiveApi.MessageBroker.GetLorescopePosterImageAsync(loreScope.Id, ct);
            if (!imageUrlResponse.TryGetAsData(out string? imageUrl)) continue;
            loreScope.S3PosterImageUrl = imageUrl;
        }

        PaginatedData<ILoreScopeModel> casted = paginatedData.CastTo<ILoreScopeModel>();
        return PaginatedOutcome<ILoreScopeModel>.FromData(casted);
    }


    public async ValueTask<Outcome> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Outcome.FromError("Invalid userId");


        Outcome<Guid> createOutcome = await interactiveApi.MessageBroker.CreateLoreScopeAsync(parsedUserId, newLoreScopeName, ct: ct);
        if (createOutcome.TryGetAsData(out Guid _)) return Outcome.True;

        logger.Warning("Failed to create lorescope for user {userId} because '{reason}'", userId, createOutcome.AsError.Value);
        return Outcome.FromError($"Failed to create lorescope for user {userId}");
    }
    
    public async ValueTask<Outcome> UpsertLoreScopeImageAsync(string userId, string loreScopeId, IBrowserFile fileStream, CancellationToken ct = default) {
        // if (!Guid.TryParse(userId, out Guid parsedUserId)) return Outcome.FromError("Invalid userId");
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Outcome.FromError("Invalid lorescopeId");

        await using MemoryStream stream = await fileStream.ToMemoryStreamAsync(MaxFileSize, ct: ct);
        Outcome outcome = await interactiveApi.MessageBroker.UpsertLoreScopeImageAsync(
            parsedLoreScopeId,
            fileStream.Name,
            fileStream.ContentType,
            stream,
            ct: ct);
        if (outcome.TryGetAsState(out bool success)) return Outcome.FromState(success);
        
        logger.Warning("Failed to upsert lorescope image for user {userId} because '{reason}'", userId, outcome.AsError.Value);
        return Outcome.FromError($"Failed to upsert lorescope image for user {userId}");
    }
}
