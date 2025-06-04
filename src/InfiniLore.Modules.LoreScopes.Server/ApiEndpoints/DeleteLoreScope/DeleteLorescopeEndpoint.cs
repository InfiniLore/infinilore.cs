// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PermissionsStore=InfiniLore.Modules.Core.Shared.PermissionsStore;

namespace InfiniLore.Modules.LoreScopes.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok,
    // Default Included Results
    NotFound,
    UnauthorizedHttpResult,
    BadRequest,
    ForbidHttpResult,
    ProblemDetails
>;

public class DeleteLorescopeEndpoint(
    ILogger<DeleteLorescopeEndpoint> logger, 
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : Endpoint<DeleteLorescopeEndpointRequest, Response, LoreScopeMapper> {

    public override void Configure() {
        Delete("/data-user/{UserId:guid}/lorescope/{LoreScopeId:guid}");
        Permissions(PermissionsStore.LorescopeDelete);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(DeleteLorescopeEndpointRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();
        
        Outcome outcome = await messageBroker.DeleteLoreScopeAsync(req.LoreScopeId, ct: ct);

        // Verify Response
        if (!outcome.TryGetAsState(out bool successful)) {
            logger.Warning("FAILED, {@state}", outcome.AsError);
            AddError("Failed to delte lorescope");
            return new ProblemDetails(ValidationFailures);
        }

        // Return
        if (successful is false) return TypedResults.BadRequest();
        return TypedResults.Ok();
    }
}
