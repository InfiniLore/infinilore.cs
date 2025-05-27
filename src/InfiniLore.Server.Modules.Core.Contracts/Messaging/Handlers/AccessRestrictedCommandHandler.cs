// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.Core.Messaging.Handlers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AccessRestrictedCommandHandler<TCommand, TResult>(
    ILogger<AccessRestrictedCommandHandler<TCommand, TResult>> logger
) : CommandHandler<TCommand, MessageResponse<TResult>> where TCommand : ICommand<MessageResponse<TResult>> {
    protected virtual MessageResponse<TResult> AccessDeniedResult { get; } = MessageResponse.FromErrorString("Access denied");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<MessageResponse<TResult>> ExecuteAsync(TCommand command, CancellationToken ct = new()) {
        if (!await ValidateAccessAsync(command, ct)) {
            logger.LogWarning("Access denied for command {Command}", typeof(TCommand).Name);
            return AccessDeniedResult;
        }
        logger.LogInformation("Access granted for command {Command}", typeof(TCommand).Name);
        return await HandleCommandAsync(command, ct);
    }

    protected abstract Task<MessageResponse<TResult>> HandleCommandAsync(TCommand command, CancellationToken ct = default);
    protected abstract ValueTask<bool> ValidateAccessAsync(TCommand command, CancellationToken ct = default);
}

#region AccessRestrictedCommandHandler without result
public abstract class AccessRestrictedCommandHandler<TCommand>(
    ILogger<AccessRestrictedCommandHandler<TCommand>> logger
) : CommandHandler<TCommand, MessageResponse> where TCommand : ICommand<MessageResponse> {
    protected virtual MessageResponse AccessDeniedResult { get; } = MessageResponse.FromErrorString("Access denied");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<MessageResponse> ExecuteAsync(TCommand command, CancellationToken ct = new()) {
        if (!await ValidateAccessAsync(command, ct)) {
            logger.LogWarning("Access denied for command {Command}", typeof(TCommand).Name);
            return AccessDeniedResult;
        }
        logger.LogInformation("Access granted for command {Command}", typeof(TCommand).Name);
        return await HandleCommandAsync(command, ct);
    }

    protected abstract Task<MessageResponse> HandleCommandAsync(TCommand command, CancellationToken ct = default);
    protected abstract ValueTask<bool> ValidateAccessAsync(TCommand command, CancellationToken ct = default);
}
#endregion
