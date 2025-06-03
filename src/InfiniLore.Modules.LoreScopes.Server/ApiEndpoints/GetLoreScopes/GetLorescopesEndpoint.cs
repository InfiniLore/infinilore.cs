// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LoreScopes.Server.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<LoreScopesResponse>,
    // Default Included Results
    NotFound,
    UnauthorizedHttpResult,
    BadRequest,
    ForbidHttpResult,
    ProblemDetails
>;

public class GetLoreScopesEndpoint(
    ILogger<GetLoreScopesEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : Endpoint<GetLoreScopesEndpointRequest, Response, LoreScopesMapper> {
    public override void Configure() {
        Get("/data-user/{UserId:guid}/lorescope");
        Permissions(PermissionsStoreConstants.LorescopeRead);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(GetLoreScopesEndpointRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        Core.Shared.Outcome<PaginatedData<LoreScopeModel>> outcome = await messageBroker.GetLoreScopesByOwnerAsync(
            req.UserId,
            QueryConfig.From(req),
            Pagination.From(req),
            ct:ct
        );

        // Verify Response
        if (!outcome.TryGetAsData(out PaginatedData<LoreScopeModel> paginatedResult)) {
            logger.Warning("Failed to get LoreScopes for user {userId} because '{reason}'", req.UserId, outcome.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved LoreScopes for userId {id}", req.UserId);

        // Return
        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        LoreScopesResponse response = Map.FromEntity(paginatedResult);

        List<Task<LoreScopeResponse>> updateTasks = response.Items.Select(async item => {
            Core.Shared.Outcome<string> imageUrlResponse = await messageBroker.GetLorescopePosterImageAsync(item.Id, ct: ct);
            if (imageUrlResponse.TryGetAsData(out string? imageUrl)) {
                item.ImageUrl = imageUrl;
            }
            return item;
        }).ToList();

        // Wait for all tasks to complete
        response.Items = await Task.WhenAll(updateTasks);
        
        return TypedResults.Ok(response);
    }
}
