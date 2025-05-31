// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Shared.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
) : Endpoint<UpsertUserProfileImageRequest, Response> {

    public override void Configure() {
        Post("/account/profile/{UserId:guid}/profile-image");
        Permissions(PermissionsStore.AccountWrite);
        Policies(ApiPolicies.JwtProtected);
        AllowFileUploads();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(UpsertUserProfileImageRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        IFormFile file = req.File;
        await using Stream fileStream = file.OpenReadStream();
        MessageResponse result = await messageBroker.UpsertUserProfileImageAsync(req.UserId, req.ContentType, fileStream, ct:ct);
        return result.Match<Response>(
            state => state 
                ? TypedResults.Ok()
                : TypedResults.BadRequest(), // This shouldn't happen, right?
            error => {
                logger.Warning("Failed to update user poster image. {@error}", error);
                return TypedResults.NotFound();
            }
        );
    }
}
