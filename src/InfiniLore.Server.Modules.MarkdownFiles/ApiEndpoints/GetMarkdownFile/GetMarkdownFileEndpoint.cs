// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Core.Services;
using InfiniLore.Server.Modules.MarkdownFiles.Database;
using InfiniLore.Server.Modules.MarkdownFiles.Messaging.Queries;
using InfiniLore.Server.Modules.Users.Services;
using InfiniLore.Server.Services.Messaging;
using InfiniLore.ServerClient.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.MarkdownFiles.ApiEndpoints.GetMarkdownFile;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<MarkdownFileResponse>,
    NotFound,
    UnauthorizedHttpResult,
    ProblemDetails
>;

public class GetMarkdownFileEndpoint(
    ILogger<GetMarkdownFileEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper
) : Endpoint<GetMarkdownFileRequest, Response, MarkdownFileMapper> {

    public override void Configure() {
        Get("/data/project/{LoreScopeId:guid}/markdown-file/{MarkdownFileId:guid}");
        Permissions(PermissionsStore.MarkdownFileRead);
        Policies(ApiPolicies.JwtProtected);
    }
    
    public override async Task<Response> ExecuteAsync(GetMarkdownFileRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        // Form Query
        var query = new GetMarkdownFileByIdQuery(
            req.MarkdownFileId,
            req.LoreScopeId
        ) {
            AccessData = await RequestAccessData.FromJwtTokenAsync(jwtTokenHelper, ct)
        };

        // Execute Query
        MessageResponse<MarkdownFile> result = await query.ExecuteAsync(ct);

        // Verify Response
        if (!result.TryGetAsSuccess(out MarkdownFile loreScope)) {
            logger.Warning("Failed to get lorescope with id {id} because '{reason}'", req.LoreScopeId, result.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved lorescope with id {id}", req.LoreScopeId);

        // Return
        MarkdownFileResponse response = Map.FromEntity(loreScope);
        return TypedResults.Ok(response);
    }
    
}
