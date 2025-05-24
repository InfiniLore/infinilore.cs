// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.ApiEndpoints;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Shared;
using InfiniLore.Shared.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<LoreScopesResponse>,
    NotFound,
    UnauthorizedHttpResult,
    ProblemDetails
>;

public class GetLoreScopesEndpoint(
    ILogger<GetLoreScopesEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.JwtToken)] IMessageBroker messageBroker
) : Endpoint<GetLoreScopesRequest, Response, LoreScopesMapper> {
    public override void Configure() {
        Get("/data-user/{UserId:guid}/lorescope");
        Permissions(PermissionsStoreConstants.LorescopeRead);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(GetLoreScopesRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        MessageResponse<PaginatedData<LoreScopeModel>> result = await messageBroker.GetLoreScopesAsync(req.UserId,ct:ct);

        // Verify Response
        if (!result.TryGetAsSuccess(out PaginatedData<LoreScopeModel> paginatedResult)) {
            logger.Warning("Failed to get LoreScopes for user {userId} because '{reason}'", req.UserId, result.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved LoreScopes for userId {id}", req.UserId);

        // Return
        LoreScopesResponse response = Map.FromEntity(paginatedResult);
        return TypedResults.Ok(response);
    }
}
