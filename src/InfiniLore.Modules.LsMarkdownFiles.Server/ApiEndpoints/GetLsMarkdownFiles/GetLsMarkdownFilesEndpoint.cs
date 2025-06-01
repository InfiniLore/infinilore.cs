// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using InfiniLore.Shared;
using InfiniLore.Shared.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<LsMarkdownFilesResponse>,
    // Default Included Results
    NotFound,
    UnauthorizedHttpResult,
    BadRequest,
    ForbidHttpResult,
    ProblemDetails
>;

public class GetLsMarkdownFilesEndpoint(
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : Endpoint<GetLsMarkdownFilesEndpointRequest, Response, LsMarkdownFilesMapper> {

    public override void Configure() {
        Get("/data-lorescope/{LoreScopeId:guid}/markdown-file");
        Permissions(PermissionsStore.LorescopeRead);
        Policies(ApiPolicies.JwtProtected);
        AllowFileUploads();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(GetLsMarkdownFilesEndpointRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();
        
        MessageResponse<PaginatedData<LsMarkdownFileModel>> result = await messageBroker.GetLsMarkdownFilesByOwnerAsync(
            req.LoreScopeId,
            QueryConfig.From(req),
            PaginationInfo.From(req),
            ct: ct
        );
        return result.Match<Response>(
            data => TypedResults.Ok(Map.FromEntity(data)),
            _ => TypedResults.NotFound()       
        );
    }
}
