// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Database;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Services;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using InfiniLore.Shared;
using System.Text;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.InteractiveApi;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiLsMarkdownFiles>]
public class InteractiveApiServerLsMarkdownFiles(IInteractiveApiServer interactiveApi) : IInteractiveApiLsMarkdownFiles {
    
    public async ValueTask<Result<ILsMarkdownFileModel>> GetLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result<ILsMarkdownFileModel>.FromError("Invalid LoreScope Id");
        if (!Guid.TryParse(lsMarkdownFileId, out Guid parsedLsMarkdownFileId)) return Result<ILsMarkdownFileModel>.FromError("Invalid LsMarkdownFile Id");
        
        MessageResponse<LsMarkdownFileModel> result = await interactiveApi.MessageBroker.GetLsMarkdownFileByIdAsync(parsedLsMarkdownFileId, ct: ct);
        return result.Match(
            model => model.OwnerId == parsedLoreScopeId ? model : Result<ILsMarkdownFileModel>.FromError("Failed to get valid MarkdownFile. OwnerId does not match"),
            _ => Result<ILsMarkdownFileModel>.FromError("Failed to get MarkdownFile")
        );
    }
    
    public async ValueTask<PaginatedResult<ILsMarkdownFileModel>> GetLsMarkdownFilesAsync(string loreScopeId, Pagination pagination, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return PaginatedResult<ILsMarkdownFileModel>.FromError("Invalid LoreScope Id");

        MessageResponse<PaginatedData<LsMarkdownFileModel>> result = await interactiveApi.MessageBroker.GetLsMarkdownFilesByOwnerAsync(
            parsedLoreScopeId,
            pagination:pagination,
            ct: ct
        );
        
        return result.Match(
            paginatedData => paginatedData.CastTo<ILsMarkdownFileModel>(),
            _ => PaginatedResult<ILsMarkdownFileModel>.FromError("Failed to get MarkdownFiles")
        );
    }
    
    public async ValueTask<Result> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, Stream fileData, Guid knownFileId = default, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result.FromError("Invalid LoreScope Id");

        MessageResponse result = await interactiveApi.MessageBroker.UpsertLsMarkdownFileAsync(parsedLoreScopeId, fileName, fileData, knownFileId, ct: ct);
        return result.Match(
            Result.FromState, 
            error => Result.FromError(string.Join(',', error.Value))
        );
    }
    public async ValueTask<Result> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, string fileData, Guid knownFileId = default, CancellationToken ct = default) {
        byte[] textBytes = Encoding.UTF8.GetBytes(fileData);
        await using var stream = new MemoryStream(textBytes);
        
        Result result = await UpsertLsMarkdownFileAsync(loreScopeId, fileName, stream, knownFileId, ct);
        return result;
    }

    public async ValueTask<Result> DeleteLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default) {
        // if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result.FromError("Invalid LoreScope Id");
        if (!Guid.TryParse(lsMarkdownFileId, out Guid parsedLsMarkdownFileId)) return Result.FromError("Invalid LsMarkdownFile Id");
        
        MessageResponse result = await interactiveApi.MessageBroker.DeleteLsMarkdownFileAsync(parsedLsMarkdownFileId, ct: ct);
        return result.Match(
            Result.FromState, 
            error => Result.FromError(string.Join(',', error.Value))
        );      
    }
}
