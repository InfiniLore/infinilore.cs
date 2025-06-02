// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Database;
using InfiniLore.Shared;

namespace InfiniLore.Modules.LsMarkdownFiles.Shared.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInteractiveApiLsMarkdownFiles {
    ValueTask<Result<ILsMarkdownFileModel>> GetLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default);
    ValueTask<PaginatedResult<ILsMarkdownFileModel>> GetLsMarkdownFilesAsync(string loreScopeId, Pagination pagination, CancellationToken ct = default);
    
    ValueTask<Result> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, Stream fileData, Guid knownFileId = default, CancellationToken ct = default);
    ValueTask<Result> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, string fileData, Guid knownFileId = default, CancellationToken ct = default);
    
    ValueTask<Result> DeleteLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default);
}
