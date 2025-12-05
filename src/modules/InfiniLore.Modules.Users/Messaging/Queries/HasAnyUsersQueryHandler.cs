// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Messaging;
using InfiniLore.Core.Outcomes;
using InfiniLore.Modules.Users.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Users.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class HasAnyUsersQueryHandler(IServiceScopeFactory serviceScopeFactory, ILogger<HasAnyUsersQueryHandler> logger)
    : BaseQueryHandler<HasAnyUsersQuery, bool>(serviceScopeFactory) {

    protected override async Task<Outcome<bool>> ExecuteAsync(
        IServiceProvider provider,
        HasAnyUsersQuery query,
        CancellationToken ct
    ) {
        try {
            var repository = provider.GetRequiredService<UserModelRepository>();
            bool hasAny = await repository.AnyAsync(query.Config, ct);
            return hasAny;
        }
        catch (Exception e) {
            logger.Error(e, "Failed to check if any users exist.");
            return ErrorOutcome.Failure;
        }
    }
}
