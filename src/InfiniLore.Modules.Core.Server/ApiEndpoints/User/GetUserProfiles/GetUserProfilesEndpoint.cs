// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PermissionsStore = InfiniLore.Shared.Auth.PermissionsStore;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<UserProfilesResponse>,
    
    // Default Included Results
    NotFound,
    UnauthorizedHttpResult,
    BadRequest,
    ForbidHttpResult,
    ProblemDetails
>;

public class GetUserProfilesEndpoint(
    ILogger<GetUserProfileEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.FromServer)] IMessageBroker messageBroker // TODO this needs a better fix than just showing the accessing user as the server
) : Endpoint<GetUserProfilesEndpointRequest, Response, UserProfilesMapper> {
    public override void Configure() {
        Get("/account/profile");
        Permissions(PermissionsStore.AccountRead, PermissionsStore.ProfileRead);
        Policies(ApiPolicies.JwtProtected);
    }

    public override async Task<Response> ExecuteAsync(GetUserProfilesEndpointRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        Result<PaginatedData<InfiniLoreUserModel>> result = await messageBroker.GetUsersAsync(
            QueryConfig.From(req),
            Pagination.From(req), 
            ct: ct
        );

        // MessageResponse<InfiniLoreUserModel> result = await messageBroker.GetUserByIdAsync(req.UserId, ct: ct);
        return result.Match<Response>(
            dataCase: model => {
                logger.Information("Successfully retrieved users");
                UserProfilesResponse mappedModel = Map.FromEntity(model);
                return TypedResults.Ok(mappedModel);
            },
            errorCase: error => {
                logger.Warning("Failed to get users because '{reason}'", error.Value);
                return TypedResults.NotFound();
            }
        );
    }
}
