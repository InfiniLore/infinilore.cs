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
public class UpsertLsMarkdownFileEndpoint(
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : InfiniLoreEndpointWithEmptyResponse<UpsertLsMarkdownFileEndpointRequest> {

    public override void Configure() {
        Post("/data-lorescope/{LoreScopeId:guid}/markdown-file");
        Permissions(PermissionsStore.LorescopePosterWrite, PermissionsStore.LorescopeWrite);
        Policies(ApiPolicies.JwtProtected);
        AllowFileUploads();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(UpsertLsMarkdownFileEndpointRequest req, CancellationToken ct) {
        if (req.File.ContentType != "text/markdown") {
            ThrowError("Invalid file type.");
        }

        if (!Guid.TryParse(req.KnownMarkdownFileId, out Guid knownMarkdownFileId)) {
            ThrowError("Invalid known markdown file id.");       
        }
        
        IFormFile file = req.File;
        await using Stream fileStream = file.OpenReadStream();
        
        Outcome outcome = await messageBroker.UpsertLsMarkdownFileAsync(
            req.LoreScopeId, 
            file.FileName,
            fileStream,
            knownMarkdownFileId,
            ct:ct
        );

        var result = outcome.Match<IResult>(
            TypedResults.Ok,
            TypedResults.NotFound,
            _ => TypedResults.BadRequest()
        );
        await SendResultAsync(result);
    }
}
