// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.Core.Messaging;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AccessRestrictedCommandHandler<TCommand, TResult>(
    ILogger<AccessRestrictedCommandHandler<TCommand, TResult>> logger
) : CommandHandler<TCommand, TResult> where TCommand : ICommand<TResult> {
    protected abstract TResult AccessDeniedResult { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<TResult> ExecuteAsync(TCommand command, CancellationToken ct = new()) {
        if (!await ValidateAccessAsync(command, ct)) {
            logger.LogWarning("Access denied for command {Command}", typeof(TCommand).Name);
            return AccessDeniedResult;
        }
        logger.LogInformation("Access granted for command {Command}", typeof(TCommand).Name);
        return await HandleCommandAsync(command, ct);
    }

    protected abstract Task<TResult> HandleCommandAsync(TCommand command, CancellationToken ct = default);
    protected abstract ValueTask<bool> ValidateAccessAsync(TCommand command, CancellationToken ct = default);
}
