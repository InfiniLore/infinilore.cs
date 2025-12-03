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
public abstract class BaseQueryHandler<TQuery, TResult>(IServiceScopeFactory serviceScopeFactory) : ICommandHandler<TQuery, Outcome<TResult>>
    where TQuery : BaseQuery<TResult>? {
    
    public async Task<Outcome<TResult>> ExecuteAsync(TQuery query, CancellationToken ct) {
        await using AsyncServiceScope scope = serviceScopeFactory.CreateAsyncScope();
        return await ExecuteAsync(scope.ServiceProvider, query, ct);
    }
    
    protected abstract Task<Outcome<TResult>> ExecuteAsync(IServiceProvider provider, TQuery query, CancellationToken ct);
}

public abstract class BaseQueryHandler<TQuery>(IServiceScopeFactory serviceScopeFactory) : ICommandHandler<TQuery, Outcome>
    where TQuery : BaseQuery {
    
    public async Task<Outcome> ExecuteAsync(TQuery query, CancellationToken ct) {
        await using AsyncServiceScope scope = serviceScopeFactory.CreateAsyncScope();
        return await ExecuteAsync(scope.ServiceProvider, query, ct);
    }
    
    protected abstract Task<Outcome> ExecuteAsync(IServiceProvider provider, TQuery query, CancellationToken ct);
}