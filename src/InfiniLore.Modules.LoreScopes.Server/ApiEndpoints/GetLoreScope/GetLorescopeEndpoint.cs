// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LoreScopes.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PermissionsStore=InfiniLore.Modules.Core.Shared.PermissionsStore;
using ProblemDetails=FastEndpoints.ProblemDetails;

namespace InfiniLore.Modules.LoreScopes.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<LoreScopeResponse>,
    // Default Included Results
    NotFound,
    UnauthorizedHttpResult,
    BadRequest,
    ForbidHttpResult,
    ProblemDetails
>;

public class GetLorescopeEndpoint(
    ILogger<GetLorescopeEndpoint> logger, 
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : Endpoint<GetLorescopeEndpointRequest, Response, LoreScopeMapper> {

    public override void Configure() {
        Get("/data-user/{UserId:guid}/lorescope/{LoreScopeId:guid}");
        Permissions(PermissionsStore.LorescopeRead);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(GetLorescopeEndpointRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        Outcome<LoreScopeModel> outcome = await messageBroker.GetLorescopeByIdAsync(req.LoreScopeId, req.UserId, autoInclude:true, ct: ct);

        // Verify Response
        if (!outcome.TryGetAsData(out LoreScopeModel? loreScope)) {
            logger.Warning("Failed to get lorescope with id {id} because '{reason}'", req.LoreScopeId, outcome.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved lorescope with id {id}", req.LoreScopeId);

        // Return
        LoreScopeResponse response = Map.FromEntity(loreScope);

        Outcome<string> imageUrlResponse = await messageBroker.GetLorescopePosterImageAsync(loreScope.Id, ct: ct);
        if (imageUrlResponse.TryGetAsData(out string? imageUrl)) {
            response.ImageUrl = imageUrl;
        }
        
        return TypedResults.Ok(response);
    }
}
