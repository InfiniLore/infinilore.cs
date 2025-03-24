// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Api.Mappers.Account;
using InfiniLore.Server.Api.Responses.Account;
using InfiniLore.ServerClient.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InfiniLore.Server.Api.Endpoints.Account.GetProfile;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<UserProfileResponse>,
    NotFound,
    ProblemDetails
>;

public class GetProfileEndpoint : Endpoint<GetProfileRequest, Response, UserProfileMapper> {
    public override void Configure() {
        Get("/account/profile/{UserId:guid}");
        Permissions(PermissionsStore.AccountRead, PermissionsStore.ProfileRead);
        Policies(ApiPolicies.JwtProtected);
    }

    public override Task<Response> ExecuteAsync(GetProfileRequest req, CancellationToken ct) {
        // TODO check the user for more than just the permissions
        //      We need to validate if the user is an admin, accessing themselves if they are just a user, etc...
        
        throw new NotImplementedException();
    }
}
