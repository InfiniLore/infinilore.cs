// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.ApiEndpoints;
using InfiniLore.Server.Modules.Core.Messaging;
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
    Ok,
    NotFound,
    BadRequest,
    UnauthorizedHttpResult,
    ProblemDetails
>;

public class UpsertLoreScopeImageEndpoint(
    ILogger<UpsertLoreScopeImageEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.JwtToken)] IMessageBroker messageBroker
) : Endpoint<UpsertLoreScopeImageRequest, Response, LoreScopeMapper> {

    public override void Configure() {
        Post("/data-user/{UserId:guid}/lorescope/{LoreScopeId:guid}/poster-image");
        Permissions(PermissionsStore.LorescopeImageWrite);
        Policies(ApiPolicies.JwtProtected);
        AllowFileUploads();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(UpsertLoreScopeImageRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        IFormFile file = req.File;
        await using Stream fileStream = file.OpenReadStream();
        MessageResponse result = await messageBroker.UpsertLoreScopeImageAsync(req.LoreScopeId, req.FileName, req.ContentType, fileStream, ct:ct);

        // Verify Response
        if (!result.TryGetState(out bool successful)) {
            logger.Warning("FAILED, {@state}", result.AsError);
            AddError("Failed to update lorescope poster image.");
            return new ProblemDetails(ValidationFailures);
        }

        // Return
        if (!successful) return TypedResults.BadRequest();
        return TypedResults.Ok();
    }
}
