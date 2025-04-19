// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Api.Responses.Data.Project.MarkdownFiles;
using InfiniLore.Server.Api.Responses.Data.User.LoreScopes;

namespace InfiniLore.ServerClient.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInteractiveApiAccess {
    ValueTask<Result<LoreScopesResponse>> GetLoreScopesAsync(string userId, CancellationToken ct = default);
    
    ValueTask<Result<MarkdownFilesResponse>> GetMarkdownFilesAsync(string loreScopeId, CancellationToken ct = default);
    ValueTask<Result<MarkdownFileResponse>> GetMarkdownFileAsync(string loreScopeId, string markdownFileId, CancellationToken ct = default);
    ValueTask<Result> UpsertMarkdownFileAsync(string loreScopeId, string markdownFileId, string fileName, string markdown, CancellationToken ct = default);
}
