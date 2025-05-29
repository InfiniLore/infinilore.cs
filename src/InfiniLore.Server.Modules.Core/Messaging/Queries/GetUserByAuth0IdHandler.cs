// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Core.Messaging.Handlers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.Core.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetUserByAuth0IdHandler(
    IReadonlyUnitOfWorkFactory factory,
    IAccessProtectionRules protectionRules,
    ILogger<GetUserByAuth0IdHandler> logger
) : AccessProtectedCommandHandler<GetUserByAuth0IdQuery, InfiniLoreUserModel>(logger) {
    protected override MessageResponse<InfiniLoreUserModel> AccessDeniedResult => MessageResponse.FromErrorString("Cannot get user by auth0 id. Access denied.");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<MessageResponse<InfiniLoreUserModel>> HandleCommandAsync(GetUserByAuth0IdQuery command, CancellationToken ct = default) {
        if (command.Auth0Id.IsNullOrEmpty()) return MessageResponse.FromErrorString("Cannot get user id by auth0 id. Auth0 id is empty.");

        try {
            await using IReadonlyUnitOfWork unitOfWork = factory.Create();
            var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);
            
            Result<InfiniLoreUserModel> result = await userRepository.TryGetByAuth0IdAsync(command.Auth0Id, ct: ct);
            return !result.IsError
                ? MessageResponse.FromSuccess(result.AsSuccess)
                : MessageResponse.FromErrorString("Cannot get user id by auth0 id. Auth0 id not found.");
        }
        catch (Exception e) {
            logger.Error(e, "Failed to get user by auth0 id.");
            return MessageResponse.FromErrorString("Failed to get user by auth0 id.");
        }
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetUserByAuth0IdQuery command, CancellationToken ct = default) {
        return protectionRules.IsServerAsync(command.AccessingUser);
    }
}
