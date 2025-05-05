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
using InfiniLore.Server.Modules.MarkdownFiles.DataBase;
using InfiniLore.Server.Modules.MarkdownFiles.Messaging.Commands;
using InfiniLore.Server.Modules.MarkdownFiles.Messaging.Queries;
using InfiniLore.Shared.Contracts.Services;
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
    public async ValueTask<Result> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result.FromError("Invalid userId");

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
        MessageResponse<PaginatedData<ILoreScope>> result = await query.ExecuteAsync(ct);

        // Verify Response
        if (!result.TryGetAsSuccess(out PaginatedData<ILoreScope> paginatedData)) {
            logger.Warning("Failed to get LoreScopes for user {userId} because '{reason}'", userId, result.AsError.Value);
            return Result.FromError($"Failed to get LoreScopes for user {userId}");
        }

        LoreScopesResponse data = loreScopesMapper.FromEntity(paginatedData);
        return Result.FromState(true);
    }
    
    public async ValueTask<Result> GetMarkdownFilesAsync(string loreScopeId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result.FromError("Invalid loreScopeId");
        ClaimsPrincipal? claims = httpContextAccessor.HttpContext?.User;
        
        // Form message
        var query = new GetMarkdownFilesQuery(
            parsedLoreScopeId,
            new PaginationInfo(1)
        ) {
            AccessData = RequestAccessData.FromClaims(claims, ct)
        };

        // Execute Query
        MessageResponse<PaginatedData<IMarkdownFile>> result = await query.ExecuteAsync(ct);
        if (!result.TryGetAsSuccess(out PaginatedData<IMarkdownFile> paginatedData)) {
            logger.Warning("Failed to get MarkdownFiles for loreScopeId {loreScopeId} because '{reason}'", loreScopeId, result.AsError.Value);
            return Result.FromError($"Failed to get MarkdownFiles for loreScopeId {loreScopeId}");
        }
        
        // Map to response
        MarkdownFilesResponse data = markdownFilesMapper.FromEntity(paginatedData);
        return Result.FromState(true);
    }
    public async ValueTask<Result> GetMarkdownFileAsync(string loreScopeId, string markdownFileId, CancellationToken ct = default) {
        if (!Guid.TryParse(loreScopeId, out Guid parsedLoreScopeId)) return Result.FromError("Invalid loreScopeId");
        if (!Guid.TryParse(markdownFileId, out Guid parsedMarkdownFileId)) return Result.FromError("Invalid markdownFileId");
        
        // Form Query
        var query = new GetMarkdownFileByIdQuery(MarkdownFileId: parsedMarkdownFileId, LorescopeId: parsedLoreScopeId) {
            AccessData = RequestAccessData.FromClaims(httpContextAccessor.HttpContext?.User, ct)
        };
        
        // Execute Query
        MessageResponse<IMarkdownFile> result = await query.ExecuteAsync(ct);
        if (!result.TryGetAsSuccess(out IMarkdownFile markdownFile)) {
            logger.Warning("Failed to get MarkdownFile for loreScopeId {loreScopeId} and markdownFileId {markdownFileId} because '{reason}'", loreScopeId, markdownFileId, result.AsError.Value);
            return Result.FromError($"Failed to get MarkdownFile for loreScopeId {loreScopeId} and markdownFileId {markdownFileId}");
        }
        
        // Map to response
        MarkdownFileResponse data = markdownFileMapper.FromEntity(markdownFile);
        return Result.FromState(true);
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
