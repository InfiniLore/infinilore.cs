// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.ApiEndpoints;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
using InfiniLore.Server.Modules.MarkdownFiles.ApiEndpoints;
using InfiniLore.Server.Modules.MarkdownFiles.Messaging.Commands;
using InfiniLore.Server.Modules.MarkdownFiles.Messaging.Queries;
using InfiniLore.ServerClient.Services;
using System.Security.Claims;

namespace InfiniLore.Server.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IInteractiveApiAccess>(ServiceLifetime.Scoped)]
public class InteractiveApiAccessServerSide(
    ILogger<InteractiveApiAccessServerSide> logger,
    IHttpContextAccessor httpContextAccessor,
    LoreScopesMapper loreScopesMapper,
    MarkdownFilesMapper markdownFilesMapper,
    MarkdownFileMapper markdownFileMapper
) : IInteractiveApiAccess {
    public async ValueTask<Result<LoreScopesResponse>> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result<LoreScopesResponse>.FromError("Invalid userId");

        ClaimsPrincipal? claims = httpContextAccessor.HttpContext?.User;

        // Form Message
        var query = new GetLoreScopesQuery(
            parsedUserId,
            false,
            new PaginationInfo(1)
        ) {
            AccessData = RequestAccessData.FromClaims(claims, ct)
        };

        // Execute Message
        MessageResponse<PaginatedData<LoreScope>> result = await query.ExecuteAsync(ct);

        // Verify Response
        if (!result.TryGetAsSuccess(out PaginatedData<LoreScope> paginatedData)) {
            logger.Warning("Failed to get LoreScopes for user {userId} because '{reason}'", userId, result.AsError.Value);
            return Result<LoreScopesResponse>.FromError($"Failed to get LoreScopes for user {userId}");
        }

        LoreScopesResponse data = loreScopesMapper.FromEntity(paginatedData);
        return Result<LoreScopesResponse>.FromSuccess(data);
    }
    
    public async ValueTask<Result<MarkdownFilesResponse>> GetMarkdownFilesAsync(string loreScopeId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result<MarkdownFilesResponse>.FromError("Invalid loreScopeId");
        ClaimsPrincipal? claims = httpContextAccessor.HttpContext?.User;
        
        // Form message
        var query = new GetMarkdownFilesQuery(
            parsedLoreScopeId,
            new PaginationInfo(1)
        ) {
            AccessData = RequestAccessData.FromClaims(claims, ct)
        };

        // Execute Query
        Modules.Core.Messaging.MessageResponse<PaginatedData<MarkdownFile>> result = await query.ExecuteAsync(ct);
        if (!result.TryGetAsSuccess(out PaginatedData<MarkdownFile> paginatedData)) {
            logger.Warning("Failed to get MarkdownFiles for loreScopeId {loreScopeId} because '{reason}'", loreScopeId, result.AsError.Value);
            return Result<MarkdownFilesResponse>.FromError($"Failed to get MarkdownFiles for loreScopeId {loreScopeId}");
        }
        
        // Map to response
        MarkdownFilesResponse data = markdownFilesMapper.FromEntity(paginatedData);
        return Result<MarkdownFilesResponse>.FromSuccess(data);
    }
    public async ValueTask<Result<MarkdownFileResponse>> GetMarkdownFileAsync(string loreScopeId, string markdownFileId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result<MarkdownFileResponse>.FromError("Invalid loreScopeId");
        if (!Guid.TryParse(markdownFileId, out Guid parsedMarkdownFileId)) return Result<MarkdownFileResponse>.FromError("Invalid markdownFileId");
        
        // Form Query
        var query = new GetMarkdownFileByIdQuery(MarkdownFileId: parsedMarkdownFileId, LorescopeId: parsedLoreScopeId) {
            AccessData = RequestAccessData.FromClaims(httpContextAccessor.HttpContext?.User, ct)
        };
        
        // Execute Query
        Modules.Core.Messaging.MessageResponse<MarkdownFile> result = await query.ExecuteAsync(ct);
        if (!result.TryGetAsSuccess(out MarkdownFile markdownFile)) {
            logger.Warning("Failed to get MarkdownFile for loreScopeId {loreScopeId} and markdownFileId {markdownFileId} because '{reason}'", loreScopeId, markdownFileId, result.AsError.Value);
            return Result<MarkdownFileResponse>.FromError($"Failed to get MarkdownFile for loreScopeId {loreScopeId} and markdownFileId {markdownFileId}");
        }
        
        // Map to response
        MarkdownFileResponse data = markdownFileMapper.FromEntity(markdownFile);
        return Result<MarkdownFileResponse>.FromSuccess(data);
    }

    public async ValueTask<Result> UpsertMarkdownFileAsync(string loreScopeId, string markdownFileId, string fileName, string markdown, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result.FromError("Invalid loreScopeId");
        if (!Guid.TryParse(markdownFileId, out Guid parsedMarkdownFileId)) return Result.FromError("Invalid markdownFileId");
        
        // Form Command
        var command = new MarkdownFileAddOrUpdateRequest(
            parsedMarkdownFileId, parsedLoreScopeId, fileName, markdown
        ) {
            AccessData = RequestAccessData.FromClaims(httpContextAccessor.HttpContext?.User, ct)
        };
        
        await command.ExecuteAsync(ct);
        return true;
    }
}
