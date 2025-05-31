// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
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
public class GetUserByIdHandler(
    IReadonlyUnitOfWorkFactory factory,
    IAccessProtectionRules protectionRules,
    ILogger<GetUserByIdHandler> logger
) : AccessProtectedCommandHandler<GetUserByIdQuery, InfiniLoreUserModel>(logger) {
    protected override MessageResponse<InfiniLoreUserModel> AccessDeniedResult => MessageResponse.FromErrorString("Cannot get user by id. Access denied.");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<MessageResponse<InfiniLoreUserModel>> HandleCommandAsync(GetUserByIdQuery command, CancellationToken ct = default) {
        if (command.UserId == Guid.Empty) return MessageResponse.FromErrorString("Cannot get user by id.  id is empty.");

        try {
            await using IReadonlyUnitOfWork unitOfWork = factory.Create();
            var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);
            
            Result<InfiniLoreUserModel> result = await userRepository.GetByIdAsync(command.UserId, ct: ct);
            return result.Match(
                successCase: MessageResponse.FromSuccess,
                errorCase: error => {
                    logger.Error("Failed to get user by id. {Error}", error);
                    return MessageResponse.FromErrorString("Cannot get user by id.");
                }
            );
        }
        catch (Exception e) {
            logger.Error(e, "Failed to get user by id.");
            return MessageResponse.FromErrorString("Failed to get user by id.");
        }
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetUserByIdQuery command, CancellationToken ct = default)
        => protectionRules.IsOwnerAsync(command.AccessingUser, command.UserId , ct: ct);
}
