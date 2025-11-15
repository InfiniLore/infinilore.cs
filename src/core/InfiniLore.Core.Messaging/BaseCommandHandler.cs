// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Core.Outcomes;

namespace InfiniLore.Core.Messaging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, Outcome<TResult>>
    where TCommand : BaseCommand<TResult> {
    
    public abstract Task<Outcome<TResult>> ExecuteAsync(TCommand command, CancellationToken ct);
}
