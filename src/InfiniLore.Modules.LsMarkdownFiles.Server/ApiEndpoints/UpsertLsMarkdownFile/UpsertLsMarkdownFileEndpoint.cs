// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Shared.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.ApiEndpoints;
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

public class UpsertLsMarkdownFileEndpoint(
    ILogger<UpsertLsMarkdownFileEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : Endpoint<UpsertLsMarkdownFileRequest, Response> {

    public override void Configure() {
        Post("/data-lorescope/{LoreScopeId:guid}/markdown-file");
        Permissions(PermissionsStore.LorescopePosterWrite, PermissionsStore.LorescopeWrite);
        Policies(ApiPolicies.JwtProtected);
        AllowFileUploads();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(UpsertLsMarkdownFileRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        if (req.ContentType != "text/markdown") {
            AddError("Invalid file type.");
            return new ProblemDetails(ValidationFailures);       
        }
        
        IFormFile file = req.File;
        await using Stream fileStream = file.OpenReadStream();
        MessageResponse result = await messageBroker.UpsertLsMarkdownFileAsync(req.LoreScopeId, req.FileName, fileStream, ct:ct);

        // Verify Response
        if (!result.TryGetState(out bool? successful)) {
            logger.Warning("FAILED, {@state}", result.AsError);
            AddError("Failed to upsert markdown file.");
            return new ProblemDetails(ValidationFailures);
        }

        // Return
        if (successful is false) return TypedResults.BadRequest();
        return TypedResults.Ok();
    }
}
