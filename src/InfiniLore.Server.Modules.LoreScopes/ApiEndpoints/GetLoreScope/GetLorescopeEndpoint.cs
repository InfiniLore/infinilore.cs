// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
using InfiniLore.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<LoreScopeResponse>,
    NotFound,
    UnauthorizedHttpResult,
    ProblemDetails
>;

public class GetLorescopeEndpoint(
    ILogger<GetLorescopeEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper,
    IMessageAccessFactory requestDataFactory
) : Endpoint<GetLorescopeRequest, Response, LoreScopeMapper> {

    public override void Configure() {
        Get("/data-user/{UserId:guid}/lorescope/{LoreScopeId:guid}");
        Permissions(PermissionsStore.LorescopeRead);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(GetLorescopeRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        // Form Query
        var query = new GetLorescopeByIdQuery(
            req.LoreScopeId,
            req.UserId
        ) {
            Access = await requestDataFactory.FromJwtTokenAsync(ct)
        };

        // Execute Query
        MessageResponse<LoreScopeModel> result = await query.ExecuteAsync(ct);

        // Verify Response
        if (!result.TryGetAsSuccess(out LoreScopeModel loreScope)) {
            logger.Warning("Failed to get lorescope with id {id} because '{reason}'", req.LoreScopeId, result.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved lorescope with id {id}", req.LoreScopeId);

        // Return
        LoreScopeResponse response = Map.FromEntity(loreScope);
        return TypedResults.Ok(response);
    }
}
