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
public abstract class BaseCommandHandler<TCommand, TResult>(IServiceScopeFactory serviceScopeFactory) : ICommandHandler<TCommand, Outcome<TResult>>
    where TCommand : BaseCommand<TResult> {

    public async Task<Outcome<TResult>> ExecuteAsync(TCommand command, CancellationToken ct) {
        await using AsyncServiceScope scope = serviceScopeFactory.CreateAsyncScope();
        return await ExecuteAsync(scope.ServiceProvider, command, ct);
    }
    
    protected abstract Task<Outcome<TResult>> ExecuteAsync(IServiceProvider provider, TCommand command, CancellationToken ct);
}
