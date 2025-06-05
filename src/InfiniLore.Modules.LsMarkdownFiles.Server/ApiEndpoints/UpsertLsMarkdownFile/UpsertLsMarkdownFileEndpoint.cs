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
        
        IFormFile file = req.File;
        await using Stream fileStream = file.OpenReadStream();
        
        Outcome outcome = await messageBroker.UpsertLsMarkdownFileAsync(
            req.LoreScopeId, 
            file.FileName,
            fileStream,
            req.KnownMarkdownFileId,
            ct:ct
        );

        outcome.Switch(
            () => Response = TypedResults.Ok(),
            () => Response = TypedResults.NotFound(),
            error => {
                AddError(error);
                ThrowError("Failed to upsert markdown file");
            }
        );
    }
}
