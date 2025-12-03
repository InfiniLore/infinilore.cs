// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Messaging;
using InfiniLore.Core.Outcomes;
using InfiniLore.Modules.Users.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace InfiniLore.Modules.Users.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetCurrentUserQueryHandler(IServiceScopeFactory serviceScopeFactory)
    : BaseQueryHandler<GetCurrentUserQuery, UserModel>(serviceScopeFactory) {

    protected override async Task<Outcome<UserModel>> ExecuteAsync(
        IServiceProvider provider,
        GetCurrentUserQuery query,
        CancellationToken ct
    ) {

        var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        HttpContext? httpContext = httpContextAccessor.HttpContext;

        // No HTTP context available (shouldn't happen in normal scenarios)
        if (httpContext == null) {
            return Outcome.Failure;
        }

        // User is not authenticated
        if (httpContext.User.Identity?.IsAuthenticated != true) {
            return Outcome.Failure;
        }

        // Extract user ID from claims
        Claim? userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId)) {
            return Outcome.Failure;
        }

        // Get the user from the database
        var uow = provider.GetRequiredService<IReadonlyUnitOfWork<InfiniLoreDb>>();
        var repository = uow.GetRepository<UserModelRepository>();
        RepoOutcome<UserModel> outcome = await repository.GetByIdAsync(userId, ct:ct);

        if (!outcome.TryGetAsSuccess(out UserModel? user)) {
            return Outcome.Failure;
        }

        return user;
    }
}
