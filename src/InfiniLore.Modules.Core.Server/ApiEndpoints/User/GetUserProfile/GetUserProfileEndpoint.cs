// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PermissionsStore = InfiniLore.Modules.Core.Shared.PermissionsStore;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<UserProfileResponse>,
    
    // Default Included Results
    NotFound,
    UnauthorizedHttpResult,
    BadRequest,
    ForbidHttpResult,
    ProblemDetails
>;

public class GetUserProfileEndpoint(
    ILogger<GetUserProfileEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : Endpoint<GetUserProfileEndpointRequest, Response, UserProfileMapper> {
    public override void Configure() {
        Get("/account/profile/{UserId:guid}");
        Permissions(PermissionsStore.AccountRead, PermissionsStore.ProfileRead);
        Policies(ApiPolicies.JwtProtected);
    }

    public override async Task<Response> ExecuteAsync(GetUserProfileEndpointRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        Shared.Outcome<InfiniLoreUserModel> outcome = await messageBroker.GetUserByIdAsync(req.UserId, ct: ct);
        return outcome.Match<Response>(
            model => {
                logger.Information("Successfully retrieved user with id {id}", req.UserId);
                UserProfileResponse mappedModel = Map.FromEntity(model);
                return TypedResults.Ok(mappedModel);
                
            },
            refused => {
                logger.Warning("Access was refused because : {reason}", refused.Reason);
                return TypedResults.Forbid();
            },
            error => {
                logger.Warning("Failed to get user with id {id} because '{reason}'", req.UserId, error.Value);
                return TypedResults.NotFound();
            }
        );
    }
}
