// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Core.Services;
using InfiniLore.Server.Modules.MarkdownFiles.Database;
using InfiniLore.Server.Modules.MarkdownFiles.Messaging.Commands;
using InfiniLore.Server.Modules.MarkdownFiles.Messaging.Queries;
using InfiniLore.Server.Services;
using InfiniLore.Shared;
using InfiniLore.Shared.Modules.MarkdownFiles.Database;
using InfiniLore.Shared.Modules.MarkdownFiles.Services;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.MarkdownFiles.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IMarkdownFileInteractiveApi>]
public class MarkdownFileInteractiveApi(
    ILogger<MarkdownFileInteractiveApi> logger,
    IInteractiveApiServer interactiveApi,
    IRequestDataFactory requestDataFactory
) : IMarkdownFileInteractiveApi {

    public async ValueTask<Result<PaginatedData<IMarkdownFileModel>>> GetMarkdownFilesAsync(string loreScopeId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result<PaginatedData<IMarkdownFileModel>>.FromError("Invalid loreScopeId");
        
        // Form message
        var query = new GetMarkdownFilesQuery(
            parsedLoreScopeId,
            PaginationInfo.Default
        ) {
            AccessData = requestDataFactory.FromClaims(ct)
        };

        // Execute Query
        MessageResponse<PaginatedData<MarkdownFileModel>> result = await query.ExecuteAsync(ct);
        if (!result.TryGetAsSuccess(out PaginatedData<MarkdownFileModel> paginatedData)) {
            logger.Warning("Failed to get MarkdownFiles for loreScopeId {loreScopeId} because '{reason}'", loreScopeId, result.AsError.Value);
            return Result<PaginatedData<IMarkdownFileModel>>.FromError($"Failed to get MarkdownFiles for loreScopeId {loreScopeId}");
        }
        
        // Map to response
        return Result<PaginatedData<IMarkdownFileModel>>.FromSuccess(paginatedData.CastTo<IMarkdownFileModel>());
    }
    
    public async ValueTask<Result<IMarkdownFileModel>> GetMarkdownFileAsync(string loreScopeId, string markdownFileId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result<IMarkdownFileModel>.FromError("Invalid loreScopeId");
        if (!Guid.TryParse(markdownFileId, out Guid parsedMarkdownFileId)) return Result<IMarkdownFileModel>.FromError("Invalid markdownFileId");
        
        // Form Query
        var query = new GetMarkdownFileByIdQuery(MarkdownFileId: parsedMarkdownFileId, LorescopeId: parsedLoreScopeId) {
            AccessData = requestDataFactory.FromClaims(ct)
        };
        
        // Execute Query
        MessageResponse<MarkdownFileModel> result = await query.ExecuteAsync(ct);
        
        // ReSharper disable once InvertIf
        if (!result.TryGetAsSuccess(out MarkdownFileModel markdownFile)) {
            logger.Warning("Failed to get MarkdownFile for loreScopeId {loreScopeId} and markdownFileId {markdownFileId} because '{reason}'", loreScopeId, markdownFileId, result.AsError.Value);
            return Result<IMarkdownFileModel>.FromError($"Failed to get MarkdownFile for loreScopeId {loreScopeId} and markdownFileId {markdownFileId}");
        }
        
        return Result<IMarkdownFileModel>.FromSuccess(markdownFile);
    }

    public async ValueTask<Result> UpsertMarkdownFileAsync(string loreScopeId, string markdownFileId, string fileName, string markdown, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result.FromError("Invalid loreScopeId");
        if (!Guid.TryParse(markdownFileId, out Guid parsedMarkdownFileId)) return Result.FromError("Invalid markdownFileId");
        
        // Form Command
        var command = new MarkdownFileAddOrUpdateRequest(
            parsedMarkdownFileId, parsedLoreScopeId, fileName, markdown
        ) {
            AccessData = requestDataFactory.FromClaims(ct)
        };
        
        await command.ExecuteAsync(ct);
        return true;
    }
}
