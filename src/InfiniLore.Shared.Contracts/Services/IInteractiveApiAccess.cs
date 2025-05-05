// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace InfiniLore.Shared.Contracts.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInteractiveApiAccess {
    ValueTask<Result> GetLoreScopesAsync(string userId, CancellationToken ct = default);
    
    ValueTask<Result> GetMarkdownFilesAsync(string loreScopeId, CancellationToken ct = default);
    ValueTask<Result> GetMarkdownFileAsync(string loreScopeId, string markdownFileId, CancellationToken ct = default);
    ValueTask<Result> UpsertMarkdownFileAsync(string loreScopeId, string markdownFileId, string fileName, string markdown, CancellationToken ct = default);
}
