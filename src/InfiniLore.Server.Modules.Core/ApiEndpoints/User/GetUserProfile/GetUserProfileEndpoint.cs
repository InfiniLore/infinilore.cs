// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using PermissionsStore = InfiniLore.Shared.Auth.PermissionsStore;

namespace InfiniLore.Server.Modules.Core.ApiEndpoints.User;
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

public class GetUserProfileEndpoint : Endpoint<GetUserProfileRequest, Response, UserProfileMapper> {
    public override void Configure() {
        Get("/account/profile/{UserId:guid}");
        Permissions(PermissionsStore.AccountRead, PermissionsStore.ProfileRead);
        Policies(ApiPolicies.JwtProtected);
    }

    public override Task<Response> ExecuteAsync(GetUserProfileRequest req, CancellationToken ct) => throw
        // TODO check the user for more than just the permissions
        //      We need to validate if the user is an admin, accessing themselves if they are just a user, etc...
        new NotImplementedException();
}
