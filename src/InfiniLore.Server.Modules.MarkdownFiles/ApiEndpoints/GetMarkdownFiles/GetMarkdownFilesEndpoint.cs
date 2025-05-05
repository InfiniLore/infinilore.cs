// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Core.Services;
using InfiniLore.Server.Modules.MarkdownFiles.Database;
using InfiniLore.Server.Modules.MarkdownFiles.Messaging.Queries;
using InfiniLore.ServerClient.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.MarkdownFiles.ApiEndpoints.GetMarkdownFiles;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<MarkdownFilesResponse>,
    NotFound,
    UnauthorizedHttpResult,
    ProblemDetails
>;

public class GetMarkdownFilesEndpoint(
    ILogger<GetMarkdownFilesEndpoint> logger,
    IJwtTokenHelper jwtTokenHelper,
    IRequestDataFactory requestDataFactory
) : Endpoint<GetMarkdownFilesRequest, Response, MarkdownFilesMapper> {

    public override void Configure() {
        Get("/data/project/{LoreScopeId:guid}/markdown-file");
        Permissions(PermissionsStore.MarkdownFileRead);
        Policies(ApiPolicies.JwtProtected);
    }
    
    public override async Task<Response> ExecuteAsync(GetMarkdownFilesRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        // Form Query
        var query = new GetMarkdownFilesQuery(
            req.LoreScopeId,
            new PaginationInfo(1)
        ) {
            AccessData = await requestDataFactory.FromJwtTokenAsync(ct)
        };

        // Execute Query
        MessageResponse<PaginatedData<MarkdownFileModel>> result = await query.ExecuteAsync(ct);

        // Verify Response
        if (!result.TryGetAsSuccess(out PaginatedData<MarkdownFileModel> loreScope)) {
            logger.Warning("Failed to get lorescope with id {id} because '{reason}'", req.LoreScopeId, result.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved lorescope with id {id}", req.LoreScopeId);

        // Return
        MarkdownFilesResponse response = Map.FromEntity(loreScope);
        return TypedResults.Ok(response);
    }
    
}
