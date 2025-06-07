// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetLsMarkdownFilesEndpoint(
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : InfiniLoreEndpoint<GetLsMarkdownFilesEndpointRequest, LsMarkdownFilesResponse, LsMarkdownFilesMapper> {

    public override void Configure() {
        Get("/data-lorescope/{LoreScopeId:guid}/markdown-file");
        Permissions(PermissionsStore.LorescopeRead);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(GetLsMarkdownFilesEndpointRequest req, CancellationToken ct) {
        PaginatedOutcome<LsMarkdownFileModel> outcome = await messageBroker.GetLsMarkdownFilesByOwnerAsync(
            req.LoreScopeId,
            QueryConfig.From(req),
            Pagination.From(req),
            ct: ct
        );

        var result = outcome.Match<IResult>(
            model => TypedResults.Ok(Map.FromEntity(model)),
            _ => TypedResults.NotFound()
        );
        await SendResultAsync(result);
    }
}
