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

public class DeleteLsMarkdownFileEndpoint(
    IJwtTokenHelper jwtTokenHelper,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : Endpoint<DeleteLsMarkdownFileEndpointRequest, Response> {

    public override void Configure() {
        Delete("/data-lorescope/{LoreScopeId:guid}/markdown-file/{MarkdownFileId:guid}");
        Permissions(PermissionsStore.LorescopeDelete);
        Policies(ApiPolicies.JwtProtected);
        AllowFileUploads();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Execute Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Response> ExecuteAsync(DeleteLsMarkdownFileEndpointRequest req, CancellationToken ct) {
        if (jwtTokenHelper.IsNotAuthenticated) return TypedResults.Unauthorized();
        
        MessageResponse result = await messageBroker.DeleteLsMarkdownFileAsync(req.MarkdownFileId, ct: ct);
        return result.Match<Response>(
            data => data ? TypedResults.Ok() : TypedResults.NotFound(),
            _ => TypedResults.NotFound()       
        );
    }
}
