// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Shared.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

public class UpsertLoreScopeMetadataEndpoint(
    ILogger<UpsertLoreScopeMetadataEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : Endpoint<UpsertLoreScopeMetadataRequest, Response, LoreScopeMapper> {

    public override void Configure() {
        Post("/data-user/{UserId:guid}/lorescope/{LoreScopeId:guid}/metadata");
        Permissions(PermissionsStore.LorescopeWrite, PermissionsStore.LorescopeCreate);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(UpsertLoreScopeMetadataRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();
        
        MessageResponse result = await messageBroker.UpsertLoreScopeMetadataAsync(req.LoreScopeId, req.Name, req.Description, ct:ct);

        // Verify Response
        if (!result.TryGetState(out bool successful)) {
            logger.Warning("FAILED, {@state}", result.AsError);
            AddError("Failed to update lorescope metadata");
            return new ProblemDetails(ValidationFailures);
        }

        // Return
        if (!successful) return TypedResults.BadRequest();
        return TypedResults.Ok();
    }
}
