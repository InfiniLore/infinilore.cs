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
    
    public async ValueTask<AterraEngine.Unions.Result<ILsMarkdownFileModel>> GetLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return AterraEngine.Unions.Result<ILsMarkdownFileModel>.FromError("Invalid LoreScope Id");
        if (!Guid.TryParse(lsMarkdownFileId, out Guid parsedLsMarkdownFileId)) return AterraEngine.Unions.Result<ILsMarkdownFileModel>.FromError("Invalid LsMarkdownFile Id");

        Core.Shared.Outcome<LsMarkdownFileModel> outcome = await interactiveApi.MessageBroker.GetLsMarkdownFileByIdAsync(parsedLsMarkdownFileId, ct: ct);
        return outcome.Match(
            model => model.OwnerId == parsedLoreScopeId ? model : AterraEngine.Unions.Result<ILsMarkdownFileModel>.FromError("Failed to get valid MarkdownFile. OwnerId does not match"),
            _ => AterraEngine.Unions.Result<ILsMarkdownFileModel>.FromError("Failed to get MarkdownFile")
        );
    }
    
    public async ValueTask<InfiniLore.Shared.PaginatedResult<ILsMarkdownFileModel>> GetLsMarkdownFilesAsync(string loreScopeId, Pagination pagination, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return InfiniLore.Shared.PaginatedResult<ILsMarkdownFileModel>.FromError("Invalid LoreScope Id");

        Core.Shared.Outcome<PaginatedData<LsMarkdownFileModel>> outcome = await interactiveApi.MessageBroker.GetLsMarkdownFilesByOwnerAsync(
            parsedLoreScopeId,
            pagination:pagination,
            ct: ct
        );
        
        return outcome.Match(
            paginatedData => paginatedData.CastTo<ILsMarkdownFileModel>(),
            _ => InfiniLore.Shared.PaginatedResult<ILsMarkdownFileModel>.FromError("Failed to get MarkdownFiles")
        );
    }
    
    public async ValueTask<AterraEngine.Unions.Result> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, Stream fileData, Guid knownFileId = default, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return AterraEngine.Unions.Result.FromError("Invalid LoreScope Id");

        Outcome outcome = await interactiveApi.MessageBroker.UpsertLsMarkdownFileAsync(parsedLoreScopeId, fileName, fileData, knownFileId, ct: ct);
        return outcome.Match(
            AterraEngine.Unions.Result.FromState, 
            error => AterraEngine.Unions.Result.FromError(string.Join(',', error.Value))
        );
    }
    public async ValueTask<AterraEngine.Unions.Result> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, string fileData, Guid knownFileId = default, CancellationToken ct = default) {
        byte[] textBytes = Encoding.UTF8.GetBytes(fileData);
        await using var stream = new MemoryStream(textBytes);
        
        AterraEngine.Unions.Result result = await UpsertLsMarkdownFileAsync(loreScopeId, fileName, stream, knownFileId, ct);
        return result;
    }

    public async ValueTask<AterraEngine.Unions.Result> DeleteLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default) {
        // if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result.FromError("Invalid LoreScope Id");
        if (!Guid.TryParse(lsMarkdownFileId, out Guid parsedLsMarkdownFileId)) return AterraEngine.Unions.Result.FromError("Invalid LsMarkdownFile Id");
        
        Outcome outcome = await interactiveApi.MessageBroker.DeleteLsMarkdownFileAsync(parsedLsMarkdownFileId, ct: ct);
        return outcome.Match(
            AterraEngine.Unions.Result.FromState, 
            error => AterraEngine.Unions.Result.FromError(string.Join(',', error.Value))
        );      
    }
}
