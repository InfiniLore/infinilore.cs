// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PermissionsStore=InfiniLore.Modules.Core.Shared.PermissionsStore;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok,
    // Default Included Results
    NotFound,
    UnauthorizedHttpResult,
    BadRequest,
    ForbidHttpResult,
    ProblemDetails
>;

public class UpsertUserProfileImageEndpoint(
    ILogger<UpsertUserProfileImageEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : Endpoint<UpsertUserProfileImageEndpointRequest, Response> {

    public override void Configure() {
        Post("/account/profile/{UserId:guid}/profile-image");
        Permissions(PermissionsStore.AccountWrite);
        Policies(ApiPolicies.JwtProtected);
        AllowFileUploads();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(UpsertUserProfileImageEndpointRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        IFormFile file = req.File;
        await using Stream fileStream = file.OpenReadStream();
        Outcome outcome = await messageBroker.UpsertUserProfileImageAsync(
            req.UserId,
            file.ContentType,
            fileStream,
            ct: ct
        );
        
        return outcome.Match<Response>(
            _ => TypedResults.Ok(),
            _ => TypedResults.BadRequest(),
            error => {
                logger.Warning("Failed to update user poster image. {@error}", error);
                return TypedResults.NotFound();
            }
        );
    }
}
