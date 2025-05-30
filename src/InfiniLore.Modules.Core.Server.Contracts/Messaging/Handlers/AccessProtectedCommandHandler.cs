// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.Messaging.Handlers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AccessProtectedCommandHandlerBase<TInput, TOutput>(
    ILogger<AccessProtectedCommandHandlerBase<TInput, TOutput>> logger
) : CommandHandler<TInput, TOutput>
    where TInput : ICommand<TOutput>, ICommonRequestData {
    protected abstract TOutput AccessDeniedResult { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<TOutput> ExecuteAsync(TInput command, CancellationToken ct = new()) {
        if (!await ValidateAccessAsync(command, ct)) {
            LogCommandAccess(command.AccessingUser, AccessType.Denied);
            return AccessDeniedResult;
        }

        LogCommandAccess(command.AccessingUser, AccessType.Granted);
        return await HandleCommandAsync(command, ct);
    }

    protected abstract Task<TOutput> HandleCommandAsync(TInput command, CancellationToken ct = default);
    protected abstract ValueTask<bool> ValidateAccessAsync(TInput command, CancellationToken ct = default);

    private void LogCommandAccess(IAccessingUser access, string accessType) {
        logger.Log(
            accessType == AccessType.Denied ? LogLevel.Warning : LogLevel.Information,
            "Command {Command} access {type} for requesting user {UserId} with roles {Roles} and permissions {Permissions}",
            typeof(TInput).Name,
            accessType,
            access.UserId,
            access.Roles,
            access.Permissions
        );
    }

    private static class AccessType {
        public const string Denied = "denied";
        public const string Granted = "granted";
    }
}

#region Actual implementations
public abstract class AccessProtectedCommandHandler<TCommand, TResult>(
    ILogger<AccessProtectedCommandHandler<TCommand, TResult>> logger
) : AccessProtectedCommandHandlerBase<TCommand, MessageResponse<TResult>>(logger)
    where TCommand : ICommand<MessageResponse<TResult>>, ICommonRequestData {
    protected override MessageResponse<TResult> AccessDeniedResult { get; } = MessageResponse.FromErrorString("Access denied");
}

public abstract class AccessProtectedCommandHandler<TCommand>(
    ILogger<AccessProtectedCommandHandler<TCommand>> logger
) : AccessProtectedCommandHandlerBase<TCommand, MessageResponse>(logger)
    where TCommand : ICommand<MessageResponse>, ICommonRequestData {
    protected override MessageResponse AccessDeniedResult { get; } = MessageResponse.FromErrorString("Access denied");
}
#endregion
