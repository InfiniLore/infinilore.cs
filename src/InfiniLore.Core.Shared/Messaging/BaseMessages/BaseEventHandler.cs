// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Core.Messaging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseEventHandler<TEvent> : IEventHandler<TEvent> {
    protected abstract IServiceScopeFactory ScopeFactory { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task HandleAsync(TEvent eventModel, CancellationToken ct) {
        AsyncServiceScope scope = ScopeFactory.CreateAsyncScope();
        try {
            await HandleAsync(scope, eventModel, ct);
        }
        finally {
            await scope.DisposeAsync();
        }
    }

    protected abstract Task HandleAsync(IServiceScope scope, TEvent eventModel, CancellationToken ct);
}
