// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetUserByAuth0IdHandler(
    IReadonlyUnitOfWorkFactory factory,
    IAccessProtectionRules protectionRules,
    ILogger<GetUserByAuth0IdHandler> logger
) : AccessProtectedCommandHandler<GetUserByAuth0IdQuery, InfiniLoreUserModel>(logger) {
    protected override Result<InfiniLoreUserModel> AccessDeniedResult => Result.FromError("Cannot get user by auth0 id. Access denied.");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<Result<InfiniLoreUserModel>> HandleCommandAsync(GetUserByAuth0IdQuery command, CancellationToken ct = default) {
        if (command.Auth0Id.IsNullOrEmpty()) return Result.FromError("Cannot get user id by auth0 id. Auth0 id is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        AterraEngine.Unions.Result<InfiniLoreUserModel> result = await userRepository.TryGetByAuth0IdAsync(command.Auth0Id, ct: ct);
        return !result.IsError
            ? Result.FromSuccess(result.AsSuccess)
            : Result.FromError("Cannot get user id by auth0 id. Auth0 id not found.");
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetUserByAuth0IdQuery command, CancellationToken ct = default)
        => protectionRules.IsServerAsync(command.AccessingUser, ct);
}
