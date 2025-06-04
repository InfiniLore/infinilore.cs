// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Database;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Services;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using System.Text;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.InteractiveApi;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiLsMarkdownFiles>]
public class InteractiveApiServerLsMarkdownFiles(IInteractiveApiServer interactiveApi) : IInteractiveApiLsMarkdownFiles {
    
    public async ValueTask<Outcome<ILsMarkdownFileModel>> GetLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Outcome<ILsMarkdownFileModel>.FromError("Invalid LoreScope Id");
        if (!Guid.TryParse(lsMarkdownFileId, out Guid parsedLsMarkdownFileId)) return Outcome<ILsMarkdownFileModel>.FromError("Invalid LsMarkdownFile Id");

        Outcome<LsMarkdownFileModel> outcome = await interactiveApi.MessageBroker.GetLsMarkdownFileByIdAsync(parsedLsMarkdownFileId, ct: ct);
        return outcome.Match(
            model => model.OwnerId == parsedLoreScopeId ? model : Outcome<ILsMarkdownFileModel>.FromError("Failed to get valid MarkdownFile. OwnerId does not match"),
            _ => Outcome<ILsMarkdownFileModel>.FromError("Failed to get MarkdownFile")
        );
    }
    
    public async ValueTask<PaginatedOutcome<ILsMarkdownFileModel>> GetLsMarkdownFilesAsync(string loreScopeId, Pagination pagination, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return PaginatedOutcome<ILsMarkdownFileModel>.FromError("Invalid LoreScope Id");

        PaginatedOutcome<LsMarkdownFileModel> outcome = await interactiveApi.MessageBroker.GetLsMarkdownFilesByOwnerAsync(
            parsedLoreScopeId,
            pagination:pagination,
            ct: ct
        );
        
        return outcome.Match(
            paginatedData => paginatedData.CastTo<ILsMarkdownFileModel>(),
            _ => PaginatedOutcome<ILsMarkdownFileModel>.FromError("Failed to get MarkdownFiles")
        );
    }
    
    public async ValueTask<Outcome> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, Stream fileData, Guid knownFileId = default, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Outcome.FromError("Invalid LoreScope Id");

        return await interactiveApi.MessageBroker.UpsertLsMarkdownFileAsync(parsedLoreScopeId, fileName, fileData, knownFileId, ct: ct);
    }
    public async ValueTask<Outcome> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, string fileData, Guid knownFileId = default, CancellationToken ct = default) {
        byte[] textBytes = Encoding.UTF8.GetBytes(fileData);
        await using var stream = new MemoryStream(textBytes);
        
        Outcome result = await UpsertLsMarkdownFileAsync(loreScopeId, fileName, stream, knownFileId, ct);
        return result;
    }

    public async ValueTask<Outcome> DeleteLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default) {
        // if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Outcome.FromError("Invalid LoreScope Id");
        if (!Guid.TryParse(lsMarkdownFileId, out Guid parsedLsMarkdownFileId)) return Outcome.FromError("Invalid LsMarkdownFile Id");
        
        return await interactiveApi.MessageBroker.DeleteLsMarkdownFileAsync(parsedLsMarkdownFileId, ct: ct);
    }
}
