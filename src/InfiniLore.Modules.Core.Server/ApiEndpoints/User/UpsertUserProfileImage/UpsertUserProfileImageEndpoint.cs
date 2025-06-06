// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UpsertUserProfileImageEndpoint(
    ILogger<UpsertUserProfileImageEndpoint> logger,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : InfiniLoreEndpointWithEmptyResponse<UpsertUserProfileImageEndpointRequest> {

    public override void Configure() {
        Post("/account/profile/{UserId:guid}/profile-image");
        Permissions(PermissionsStore.AccountWrite);
        Policies(ApiPolicies.JwtProtected);
        AllowFileUploads();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(UpsertUserProfileImageEndpointRequest req, CancellationToken ct) {
        IFormFile file = req.File;
        await using Stream fileStream = file.OpenReadStream();
        
        Outcome outcome = await messageBroker.UpsertUserProfileImageAsync(
            req.UserId,
            file.ContentType,
            fileStream,
            ct: ct
        );

        var result = outcome.Match<IResult>(
            TypedResults.Ok,
            TypedResults.BadRequest,
            error => {
                logger.Warning("Failed to update user poster image. {@error}", error);
                return TypedResults.BadRequest();
            }
        );
        await SendResultAsync(result);
    }
}
