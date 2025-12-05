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
public abstract class BaseQueryHandler<TQuery, TResult>(IServiceScopeFactory scopeFactory) : ICommandHandler<TQuery, Outcome<TResult>>
    where TQuery : BaseQuery<TResult> {
    
    public async Task<Outcome<TResult>> ExecuteAsync(TQuery input, CancellationToken ct) {
        await using AsyncServiceScope scope = scopeFactory.CreateAsyncScope();
        return await ExecuteAsync(scope.ServiceProvider, input, ct);
    }

    protected abstract Task<Outcome<TResult>> ExecuteAsync(IServiceProvider provider, TQuery input, CancellationToken ct);
}