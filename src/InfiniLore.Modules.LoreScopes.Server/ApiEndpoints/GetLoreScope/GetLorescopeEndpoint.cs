// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LoreScopes.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LoreScopes.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetLorescopeEndpoint(
    ILogger<GetLorescopeEndpoint> logger, 
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : InfiniLoreEndpoint<GetLorescopeEndpointRequest, LoreScopeResponse, LoreScopeMapper> {

    public override void Configure() {
        Get("/data-user/{UserId:guid}/lorescope/{LoreScopeId:guid}");
        Permissions(PermissionsStore.LorescopeRead);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(GetLorescopeEndpointRequest req, CancellationToken ct) {
        Outcome<LoreScopeModel> outcome = await messageBroker.GetLorescopeByIdAsync(req.LoreScopeId, req.UserId, autoInclude:true, ct: ct);
        
        await outcome.SwitchAsync(
            OnFoundDataAsync,
            (error, token) => OnErrorAsync(error, req, token),
            ct
        );
    }   
    
    private async Task OnFoundDataAsync(LoreScopeModel loreScope, CancellationToken ct = default) {
        Outcome<string> imageUrlResponse = await messageBroker.GetLorescopePosterImageAsync(loreScope.Id, ct: ct);
        if (imageUrlResponse.TryGetAsData(out string? imageUrl)) {
            loreScope.S3PosterImageUrl = imageUrl;
        }

        logger.Information("Successfully retrieved lorescope with id {id}", loreScope.Id);
        Response = TypedResults.Ok(Map.FromEntity(loreScope));
    }

    private Task OnErrorAsync(string error, GetLorescopeEndpointRequest req, CancellationToken _ = default) {
        logger.Warning("Failed to get lorescope with id {id} because '{reason}'", req.LoreScopeId, error);
        Response = TypedResults.NotFound();
        return Task.CompletedTask;
    }
}
