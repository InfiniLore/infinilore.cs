// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Shared.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        Core.Server.Result<LoreScopeModel> result = await messageBroker.GetLorescopeByIdAsync(req.LoreScopeId, req.UserId, autoInclude:true, ct: ct);

        // Verify Response
        if (!result.TryGetAsSuccess(out LoreScopeModel? loreScope)) {
            logger.Warning("Failed to get lorescope with id {id} because '{reason}'", req.LoreScopeId, result.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved lorescope with id {id}", req.LoreScopeId);

        // Return
        LoreScopeResponse response = Map.FromEntity(loreScope);

        Core.Server.Result<string> imageUrlResponse = await messageBroker.GetLorescopePosterImageAsync(loreScope.Id, ct: ct);
        if (imageUrlResponse.TryGetAsSuccess(out string? imageUrl)) {
            response.ImageUrl = imageUrl;
        }
        
        return TypedResults.Ok(response);
    }
}
