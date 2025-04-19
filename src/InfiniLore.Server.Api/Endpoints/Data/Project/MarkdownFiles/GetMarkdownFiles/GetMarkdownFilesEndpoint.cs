// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Api.Mappers.Data.Project.MarkdownFiles;
using InfiniLore.Server.Api.Responses.Data.Project.MarkdownFiles;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Contracts.Services.Auth0;
using InfiniLore.Server.Database.Models.Data.Project;
using InfiniLore.Server.Services.Messaging;
using InfiniLore.Server.Services.Messaging.Queries.Data.Project;
using InfiniLore.ServerClient.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Api.Endpoints.Data.Project.MarkdownFiles.GetMarkdownFiles;

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
    IJwtTokenHelper jwtTokenHelper
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
            req.LoreScopeId
        ) {
            AccessData = await RequestAccessData.FromJwtTokenAsync(jwtTokenHelper, ct)
        };

        // Execute Query
        MessageResponse<PaginatedData<MarkdownFile>> result = await query.ExecuteAsync(ct);

        // Verify Response
        if (!result.TryGetAsSuccess(out PaginatedData<MarkdownFile> loreScope)) {
            logger.Warning("Failed to get lorescope with id {id} because '{reason}'", req.LoreScopeId, result.AsError.Value);
            return TypedResults.NotFound();
        }

        logger.Information("Successfully retrieved lorescope with id {id}", req.LoreScopeId);

        // Return
        MarkdownFilesResponse response = Map.FromEntity(loreScope);
        return TypedResults.Ok(response);
    }
    
}
