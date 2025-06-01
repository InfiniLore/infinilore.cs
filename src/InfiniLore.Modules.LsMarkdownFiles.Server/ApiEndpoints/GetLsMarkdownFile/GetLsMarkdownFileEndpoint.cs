// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using InfiniLore.Shared.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using Response=Results<
    Ok<LsMarkdownFileResponse>,
    // Default Included Results
    NotFound,
    UnauthorizedHttpResult,
    BadRequest,
    ForbidHttpResult,
    ProblemDetails
>;

public class GetLsMarkdownFileEndpoint(
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : Endpoint<GetLsMarkdownFileEndpointRequest, Response, LsMarkdownFileMapper> {

    public override void Configure() {
        Get("/data-lorescope/{LoreScopeId:guid}/markdown-file/{MarkdownFileId:guid}");
        Permissions(PermissionsStore.LorescopeRead);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(GetLsMarkdownFileEndpointRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();

        MessageResponse<LsMarkdownFileModel> result = await messageBroker.GetLsMarkdownFileByIdAsync(req.MarkdownFileId, ct: ct);
        return result.Match<Response>(
            model => TypedResults.Ok(Map.FromEntity(model)),
            _ => TypedResults.NotFound()       
        );
    }
}
