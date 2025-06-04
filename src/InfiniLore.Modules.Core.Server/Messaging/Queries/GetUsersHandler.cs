// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Modules.Core.Shared;
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
) : PaginatedAccessProtectedCommandHandler<GetUsersQuery, InfiniLoreUserModel>(logger) {
    protected override PaginatedOutcome<InfiniLoreUserModel> AccessDeniedOutcome => Outcome.FromError("Cannot get users. Access denied.");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<PaginatedOutcome<InfiniLoreUserModel>> HandleCommandAsync(GetUsersQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        PaginatedOutcome<InfiniLoreUserModel> result = await userRepository.GetAllAsync(command.Pagination,command.QueryConfig, ct: ct);
        return result.Match(
            dataCase: Outcome.FromData,
            errorCase: error => {
                logger.Error("Failed to get users. {Error}", error);
                return Outcome.FromError("Cannot get users.");
            }
        );
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetUsersQuery command, CancellationToken ct = default)
        => protectionRules.IsServerAsync(command.AccessingUser, ct: ct);
}
