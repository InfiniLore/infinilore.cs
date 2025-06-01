// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Shared;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetUsersHandler(
    IReadonlyUnitOfWorkFactory factory,
    IAccessProtectionRules protectionRules,
    ILogger<GetUsersHandler> logger
) : AccessProtectedCommandHandler<GetUsersQuery, PaginatedData<InfiniLoreUserModel>>(logger) {
    protected override MessageResponse<PaginatedData<InfiniLoreUserModel>> AccessDeniedResult => MessageResponse.FromErrorString("Cannot get users. Access denied.");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<MessageResponse<PaginatedData<InfiniLoreUserModel>>> HandleCommandAsync(GetUsersQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);
            
        PaginatedResult<InfiniLoreUserModel> result = await userRepository.GetAllAsync(command.PaginationInfo,command.QueryConfig, ct: ct);
        return result.Match(
            dataCase: MessageResponse.FromSuccess,
            errorCase: error => {
                logger.Error("Failed to get users. {Error}", error);
                return MessageResponse.FromErrorString("Cannot get users.");
            }
        );
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetUsersQuery command, CancellationToken ct = default)
        => protectionRules.IsServerAsync(command.AccessingUser, ct: ct);
}
