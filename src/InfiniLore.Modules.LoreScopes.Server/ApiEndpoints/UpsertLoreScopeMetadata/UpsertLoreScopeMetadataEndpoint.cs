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
public class UpsertLoreScopeMetadataEndpoint(
    ILogger<UpsertLoreScopeMetadataEndpoint> logger,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : InfiniLoreEndpointWithEmptyResponse<UpsertLoreScopeMetadataEndpointRequest, LoreScopeMapper> {

    public override void Configure() {
        Post("/data-user/{UserId:guid}/lorescope/{LoreScopeId:guid}/metadata");
        Permissions(PermissionsStore.LorescopeWrite, PermissionsStore.LorescopeCreate);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(UpsertLoreScopeMetadataEndpointRequest req, CancellationToken ct) {
        Outcome outcome = await messageBroker.UpsertLoreScopeMetadataAsync(req.LoreScopeId, req.Name, req.Description, ct:ct);
        await outcome.SwitchAsync(
            async () => await SendResultAsync(TypedResults.Ok()),
            async () => await SendResultAsync(TypedResults.BadRequest()),
            async error => {
                logger.Error("Failed to lorescope metadata because '{reason}'", error);
                await SendResultAsync(TypedResults.BadRequest());
            }
        );
    }
}
