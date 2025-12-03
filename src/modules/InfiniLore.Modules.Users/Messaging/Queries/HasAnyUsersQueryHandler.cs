// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Messaging;
using InfiniLore.Core.Outcomes;
using InfiniLore.Modules.Users.Database;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Modules.Users.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class HasAnyUsersQueryHandler(IServiceScopeFactory serviceScopeFactory) 
    : BaseQueryHandler<HasAnyUsersQuery>(serviceScopeFactory) {
    
    protected override async Task<Outcome> ExecuteAsync(
        IServiceProvider provider, 
        HasAnyUsersQuery query, 
        CancellationToken ct) {
        
        var repository = provider.GetRequiredService<UserModelRepository>();
        bool hasAny = await repository.AnyAsync(query.Config, ct);
        return hasAny 
            ? Outcome.Success
            : Outcome.Failure;
    }
}
