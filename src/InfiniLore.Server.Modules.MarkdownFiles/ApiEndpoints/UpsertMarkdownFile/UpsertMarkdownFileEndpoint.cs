// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.Services;
using InfiniLore.Server.Modules.MarkdownFiles.Messaging.Commands;
using InfiniLore.ServerClient.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.MarkdownFiles.ApiEndpoints.UpsertMarkdownFile;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok,
    NotFound,
    UnauthorizedHttpResult,
    ProblemDetails
>;

public class UpsertMarkdownFileEndpoint(
    ILogger<UpsertMarkdownFileEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper
) : Endpoint<UpsertMarkdownFileRequest, Response, MarkdownFileMapper> {

    public override void Configure() {
        Post("/data/project/{LoreScopeId:guid}/markdown-file/{MarkdownFileId:guid}");
        Permissions(PermissionsStore.MarkdownFileWrite, PermissionsStore.MarkdownFileCreate);
        Policies(ApiPolicies.JwtProtected);
    }
    
    public override async Task<Response> ExecuteAsync(UpsertMarkdownFileRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        // Form Command
        var command = new MarkdownFileAddOrUpdateRequest(
            req.MarkdownFileId,
            req.LoreScopeId,
            req.FileName,
            req.Source
        );

        // Execute Command
        await command.ExecuteAsync(ct);

        // Return
        return TypedResults.Ok();
    }
    
}
