// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Shared.Modules.MarkdownFiles.Database;

namespace InfiniLore.Shared.Modules.MarkdownFiles.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownFileInteractiveApi{
    ValueTask<Result<PaginatedData<IMarkdownFileModel>>> GetMarkdownFilesAsync(string loreScopeId, CancellationToken ct = default);
    ValueTask<Result<IMarkdownFileModel>> GetMarkdownFileAsync(string loreScopeId, string markdownFileId, CancellationToken ct = default);
    ValueTask<Result> UpsertMarkdownFileAsync(string loreScopeId, string markdownFileId, string fileName, string markdown, CancellationToken ct = default);
}
