// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Core.Outcomes;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Core.Messaging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BasePaginatedQueryHandler<TQuery, TResult>(IServiceScopeFactory serviceScopeFactory) : ICommandHandler<TQuery, PaginatedOutcome<TResult>>
    where TQuery : BasePaginatedQuery<TResult> where TResult : class {
    
    public async Task<PaginatedOutcome<TResult>> ExecuteAsync(TQuery query, CancellationToken ct) {
        await using AsyncServiceScope scope = serviceScopeFactory.CreateAsyncScope();
        return await ExecuteAsync(scope.ServiceProvider, query, ct);
    }
    
    protected abstract Task<PaginatedOutcome<TResult>> ExecuteAsync(IServiceProvider provider, TQuery query, CancellationToken ct);
}
