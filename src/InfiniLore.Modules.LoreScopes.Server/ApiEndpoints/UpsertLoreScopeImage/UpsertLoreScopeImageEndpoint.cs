// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Shared.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Result=InfiniLore.Modules.Core.Server.Result;

namespace InfiniLore.Modules.LoreScopes.Server.ApiEndpoints;
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

public class UpsertLoreScopeImageEndpoint(
    ILogger<UpsertLoreScopeImageEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : Endpoint<UpsertLoreScopeImageEndpointRequest, Response, LoreScopeMapper> {

    public override void Configure() {
        Post("/data-user/{UserId:guid}/lorescope/{LoreScopeId:guid}/poster-image");
        Permissions(PermissionsStore.LorescopePosterWrite, PermissionsStore.LorescopeWrite);
        Policies(ApiPolicies.JwtProtected);
        AllowFileUploads();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(UpsertLoreScopeImageEndpointRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        IFormFile file = req.File;
        await using Stream fileStream = file.OpenReadStream();
        Result result = await messageBroker.UpsertLoreScopeImageAsync(req.LoreScopeId,
            file.FileName,
            file.ContentType,
            fileStream,
            ct: ct);

        // Verify Response
        if (!result.TryGetState(out bool? successful)) {
            logger.Warning("FAILED, {@state}", result.AsError);
            AddError("Failed to update lorescope poster image.");
            return new ProblemDetails(ValidationFailures);
        }

        // Return
        if (successful is false) return TypedResults.BadRequest();
        return TypedResults.Ok();
    }
}
