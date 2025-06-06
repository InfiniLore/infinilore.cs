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
public class CreateLorescopeEndpoint(
    ILogger<CreateLorescopeEndpoint> logger, 
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : InfiniLoreEndpoint<CreateLorescopeEndpointRequest, Guid, LoreScopeMapper> {

    public override void Configure() {
        Post("/data-user/{UserId:guid}/lorescope");
        Permissions(PermissionsStore.LorescopeCreate);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(CreateLorescopeEndpointRequest req, CancellationToken ct) {
        Outcome<Guid> outcome = await messageBroker.CreateLoreScopeAsync(req.UserId, req.Name, ct: ct);
        await outcome.SwitchAsync(
            async id => await SendResultAsync(TypedResults.Ok(id)),
            async error => {
                logger.Error("Failed to create lorescope for user with id {id} because '{reason}'", req.UserId, error);
                await SendResultAsync(TypedResults.BadRequest());
            }
        ); 
    }
}
