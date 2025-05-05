// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Core.Services;
using InfiniLore.Server.Modules.MarkdownFiles.Database;
using InfiniLore.Server.Modules.MarkdownFiles.Messaging.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using PermissionsStore = InfiniLore.Shared.PermissionsStore;

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
    IJwtTokenHelper jwtTokenHelper,
    IRequestDataFactory requestDataFactory
) : Endpoint<GetMarkdownFileRequest, Response, MarkdownFileMapper> {

    public override void Configure() {
        Get("/data-lorescope/{LoreScopeId:guid}/markdown-file/{MarkdownFileId:guid}");
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
            AccessData = await requestDataFactory.FromJwtTokenAsync(ct)
        };

        // Execute Query
        MessageResponse<MarkdownFileModel> result = await query.ExecuteAsync(ct);

        // Verify Response
        if (!result.TryGetAsSuccess(out MarkdownFileModel loreScope)) {
            logger.Warning("Failed to get lorescope with id {id} because '{reason}'", req.LoreScopeId, result.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved lorescope with id {id}", req.LoreScopeId);

        // Return
        MarkdownFileResponse response = Map.FromEntity(loreScope);
        return TypedResults.Ok(response);
    }
    
}
