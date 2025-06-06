// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DeleteLsMarkdownFileEndpoint(
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : InfiniLoreEndpointWithEmptyResponse<DeleteLsMarkdownFileEndpointRequest> {

    public override void Configure() {
        Delete("/data-lorescope/{LoreScopeId:guid}/markdown-file/{MarkdownFileId:guid}");
        Permissions(PermissionsStore.LorescopeDelete);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(DeleteLsMarkdownFileEndpointRequest req, CancellationToken ct) {
        Outcome outcome = await messageBroker.DeleteLsMarkdownFileAsync(req.MarkdownFileId, ct: ct);
        var result = outcome.Match<IResult>(
            TypedResults.Ok,
            TypedResults.NotFound,
            _ => TypedResults.NotFound()
        );
        await SendResultAsync(result);
    }
}
