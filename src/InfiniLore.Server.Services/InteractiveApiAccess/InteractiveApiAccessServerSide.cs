// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Api.Mappers.Data.User.LoreScopes;
using InfiniLore.Server.Api.Responses.Data.User.LoreScopes;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Database.Models.Data.User;
using InfiniLore.Server.Services.Mediator;
using InfiniLore.Server.Services.Mediator.Queries.Data.User;
using InfiniLore.ServerClient.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Wolverine;

namespace InfiniLore.Server.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IInteractiveApiAccess>(ServiceLifetime.Scoped)]
public class InteractiveApiAccessServerSide(
    ILogger<InteractiveApiAccessServerSide> logger,
    IMessageBus messageBus,
    IHttpContextAccessor httpContextAccessor,
    LoreScopesMapper loreScopesMapper
) : IInteractiveApiAccess {
    public async ValueTask<Result<LoreScopesResponse>> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        if (!Guid.TryParse(userId, out Guid parsedUserId)) return Result<LoreScopesResponse>.FromError("Invalid userId");

        ClaimsPrincipal? claims = httpContextAccessor.HttpContext?.User;

        var query = new GetLoreScopesQuery(
            parsedUserId,
            false,
            new PaginationInfo(1)
        ) {
            AccessData = RequestAccessData.FromClaims(claims, ct)
        };

        // Execute Query
        var result = await messageBus.InvokeAsync<MediatorResponse<PaginatedData<LoreScope>>>(query, ct);

        // Verify Response
        if (!result.TryGetAsSuccess(out PaginatedData<LoreScope> paginatedData)) {
            logger.Warning("Failed to get LoreScopes for user {userId} because '{reason}'", userId, result.AsError.Value);
            return Result<LoreScopesResponse>.FromError($"Failed to get LoreScopes for user {userId}");
        }

        LoreScopesResponse data = loreScopesMapper.FromEntity(paginatedData);
        return Result<LoreScopesResponse>.FromSuccess(data);
    }
}
