// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LoreScopes.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UpsertLoreScopeImageEndpoint(
    ILogger<UpsertLoreScopeImageEndpoint> logger,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : InfiniLoreEndpointWithEmptyResponse<UpsertLoreScopeImageEndpointRequest, LoreScopeMapper> {

    public override void Configure() {
        Post("/data-user/{UserId:guid}/lorescope/{LoreScopeId:guid}/poster-image");
        Permissions(PermissionsStore.LorescopePosterWrite, PermissionsStore.LorescopeWrite);
        Policies(ApiPolicies.JwtProtected);
        AllowFileUploads();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(UpsertLoreScopeImageEndpointRequest req, CancellationToken ct) {
        IFormFile file = req.File;
        await using Stream fileStream = file.OpenReadStream();
        
        Outcome outcome = await messageBroker.UpsertLoreScopeImageAsync(req.LoreScopeId,
            file.FileName,
            file.ContentType,
            fileStream,
            ct: ct);
        
        outcome.Switch(
            () =>  Response = TypedResults.Ok(),
            () =>  Response = TypedResults.BadRequest(),
            error => {
                logger.Error("Failed to update lorescope poster image because '{reason}'", error);
                Response = TypedResults.BadRequest();
            }
        );
    }
}
