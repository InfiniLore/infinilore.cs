// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Shared;
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
    protected abstract TOutput AccessDeniedOutcome { get; }
    protected abstract TOutput UncaughtErrorOutcome { get; } 

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<TOutput> ExecuteAsync(TInput command, CancellationToken ct = new()) {
        try {
            if (!await ValidateAccessAsync(command, ct)) {
                LogCommandAccess(command.AccessingUser, AccessType.Denied);
                return AccessDeniedOutcome;
            }

            LogCommandAccess(command.AccessingUser, AccessType.Granted);
            return await HandleCommandAsync(command, ct);
        }

        catch (Exception e) {
            logger.Error(e, "Uncaught error in command handler {Handler}", typeof(TInput).Name);
            return UncaughtErrorOutcome;
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
) : AccessProtectedCommandHandlerBase<TCommand, Outcome<TResult>>(logger)
    where TCommand : ICommand<Outcome<TResult>>, ICommonRequestData 
{
    protected override Outcome<TResult> AccessDeniedOutcome { get; } = Outcome<TResult>.FromAccessRefused("Access denied");
    protected override Outcome<TResult> UncaughtErrorOutcome { get; } = Outcome<TResult>.FromError("Uncaught error");
}

public abstract class AccessProtectedCommandHandler<TCommand>(
    ILogger<AccessProtectedCommandHandler<TCommand>> logger
) : AccessProtectedCommandHandlerBase<TCommand, Outcome>(logger)
    where TCommand : ICommand<Outcome>, ICommonRequestData 
{
    protected override Outcome AccessDeniedOutcome { get; } = Outcome.FromError("Access denied");
    protected override Outcome UncaughtErrorOutcome { get; } = Outcome.FromError("Uncaught error");
}

public abstract class PaginatedAccessProtectedCommandHandler<TCommand, TResult>(
    ILogger<PaginatedAccessProtectedCommandHandler<TCommand, TResult>> logger
) : AccessProtectedCommandHandlerBase<TCommand, PaginatedOutcome<TResult>>(logger)
    where TCommand : ICommand<PaginatedOutcome<TResult>>, ICommonRequestData 
    where TResult : class
{
    protected override PaginatedOutcome<TResult> AccessDeniedOutcome { get; } = Outcome.FromAccessRefused("Access denied");
    protected override PaginatedOutcome<TResult> UncaughtErrorOutcome { get; } = Outcome.FromError("Uncaught error");
}
#endregion
