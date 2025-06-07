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
public class DeleteLorescopeEndpoint(
    ILogger<DeleteLorescopeEndpoint> logger, 
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : InfiniLoreEndpointWithEmptyResponse<DeleteLorescopeEndpointRequest, LoreScopeMapper> {

    public override void Configure() {
        Delete("/data-user/{UserId:guid}/lorescope/{LoreScopeId:guid}");
        Permissions(PermissionsStore.LorescopeDelete);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(DeleteLorescopeEndpointRequest req, CancellationToken ct) {
        Outcome outcome = await messageBroker.DeleteLoreScopeAsync(req.LoreScopeId, ct: ct);
        var result = outcome.Match<IResult>(
            TypedResults.Ok,
            TypedResults.BadRequest,
            error => {
                logger.Error("Failed to delete lorescope with id {id} because '{reason}'", req.LoreScopeId, error);
                return TypedResults.BadRequest();
            }
        );
        await SendResultAsync(result);
    }
}
