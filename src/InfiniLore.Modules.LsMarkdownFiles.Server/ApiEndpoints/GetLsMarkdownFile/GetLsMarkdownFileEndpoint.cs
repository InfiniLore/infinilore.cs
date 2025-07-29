// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetLsMarkdownFileEndpoint(
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : InfiniLoreEndpoint<GetLsMarkdownFileEndpointRequest, LsMarkdownFileResponse, LsMarkdownFileMapper> {

    public override void Configure() {
        Get("/data-lorescope/{LoreScopeId:guid}/markdown-file/{MarkdownFileId:guid}");
        Permissions(PermissionsStore.LorescopeRead);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(GetLsMarkdownFileEndpointRequest req, CancellationToken ct) {
        Outcome<LsMarkdownFileModel> outcome = await messageBroker.GetLsMarkdownFileByIdAsync(req.MarkdownFileId, ct: ct);
        var result = outcome.Match<IResult>(
            model => TypedResults.Ok(Map.FromEntity(model)),
            _ => TypedResults.NotFound()
        );
        await Send.ResultAsync(result);
    }
}
