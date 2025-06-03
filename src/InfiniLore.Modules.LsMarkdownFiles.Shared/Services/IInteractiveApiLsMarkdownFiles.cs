// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Database;
using InfiniLore.Shared;
using Result=AterraEngine.Unions.Result;

namespace InfiniLore.Modules.LsMarkdownFiles.Shared.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInteractiveApiLsMarkdownFiles {
    ValueTask<AterraEngine.Unions.Result<ILsMarkdownFileModel>> GetLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default);
    ValueTask<PaginatedOutcome<ILsMarkdownFileModel>> GetLsMarkdownFilesAsync(string loreScopeId, Pagination pagination, CancellationToken ct = default);
    
    ValueTask<Result> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, Stream fileData, Guid knownFileId = default, CancellationToken ct = default);
    ValueTask<Result> UpsertLsMarkdownFileAsync(string loreScopeId, string fileName, string fileData, Guid knownFileId = default, CancellationToken ct = default);
    
    ValueTask<Result> DeleteLsMarkdownFileAsync(string loreScopeId, string lsMarkdownFileId, CancellationToken ct = default);
}
