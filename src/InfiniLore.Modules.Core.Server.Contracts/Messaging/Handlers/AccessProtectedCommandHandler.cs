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
    where TInput : ICommand<TOutput>, ICommonRequestData 
{
    protected abstract TOutput AccessDeniedResult { get; }
    protected abstract TOutput UncaughtErrorResult { get; } 

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<TOutput> ExecuteAsync(TInput command, CancellationToken ct = new()) {
        try {
            if (!await ValidateAccessAsync(command, ct)) {
                LogCommandAccess(command.AccessingUser, AccessType.Denied);
                return AccessDeniedResult;
            }

            LogCommandAccess(command.AccessingUser, AccessType.Granted);
            return await HandleCommandAsync(command, ct);
        }

        catch (Exception e) {
            logger.LogError(e, "Uncaught error in command handler {Handler}", typeof(TInput).Name);
            return UncaughtErrorResult;
        }
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
) : AccessProtectedCommandHandlerBase<TCommand, Server.Result<TResult>>(logger)
    where TCommand : ICommand<Server.Result<TResult>>, ICommonRequestData 
{
    protected override Server.Result<TResult> AccessDeniedResult { get; } = Server.Result.FromAccessRefused("Access denied");
    protected override Server.Result<TResult> UncaughtErrorResult { get; } = Server.Result.FromError("Uncaught error");
}

public abstract class AccessProtectedCommandHandler<TCommand>(
    ILogger<AccessProtectedCommandHandler<TCommand>> logger
) : AccessProtectedCommandHandlerBase<TCommand, Server.Result>(logger)
    where TCommand : ICommand<Server.Result>, ICommonRequestData 
{
    protected override Server.Result AccessDeniedResult { get; } = Server.Result.FromError("Access denied");
    protected override Server.Result UncaughtErrorResult { get; } = Server.Result.FromError("Uncaught error");
}

public abstract class PaginatedAccessProtectedCommandHandler<TCommand, TResult>(
    ILogger<PaginatedAccessProtectedCommandHandler<TCommand, TResult>> logger
) : AccessProtectedCommandHandlerBase<TCommand, Server.PaginatedResult<TResult>>(logger)
    where TCommand : ICommand<Server.PaginatedResult<TResult>>, ICommonRequestData 
    where TResult : class
{
    protected override Server.PaginatedResult<TResult> AccessDeniedResult { get; } = Server.Result.FromAccessRefused("Access denied");
    protected override Server.PaginatedResult<TResult> UncaughtErrorResult { get; } = Server.Result.FromError("Uncaught error");
}
#endregion
