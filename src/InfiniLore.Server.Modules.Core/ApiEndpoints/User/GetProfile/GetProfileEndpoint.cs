// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InfiniLore.Server.Modules.Core.ApiEndpoints.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<UserProfileResponse>,
    NotFound,
    ProblemDetails
>;

public class GetProfileEndpoint : Endpoint<GetProfileRequest, Results<Ok<UserProfileResponse>, NotFound, ProblemDetails>, UserProfileMapper> {
    public override void Configure() {
        Get("/account/profile/{UserId:guid}");
        Permissions(PermissionsStore.AccountRead, PermissionsStore.ProfileRead);
        Policies(ApiPolicies.JwtProtected);
    }

    public override Task<Response> ExecuteAsync(GetProfileRequest req, CancellationToken ct) => throw
        // TODO check the user for more than just the permissions
        //      We need to validate if the user is an admin, accessing themselves if they are just a user, etc...
        new NotImplementedException();
}
