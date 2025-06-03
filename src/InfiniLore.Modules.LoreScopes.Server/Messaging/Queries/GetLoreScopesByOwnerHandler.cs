// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LoreScopes.Server.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLoreScopesByOwnerHandler(
    IReadonlyUnitOfWorkFactory factory,
    IAccessProtectionRules protectionRules,
    ILogger<GetLoreScopesByOwnerHandler> logger
) : AccessProtectedCommandHandler<GetLoreScopesByOwnerQuery, PaginatedData<LoreScopeModel>>(logger) {

    protected override async Task<Core.Shared.Outcome<PaginatedData<LoreScopeModel>>> HandleCommandAsync(GetLoreScopesByOwnerQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        InfiniLore.Shared.PaginatedResult<LoreScopeModel> response = await loreScopeRepository.GetByOwnerAsync(command.UserId, command.Pagination, command.QueryConfig, ct);

        // Todo lorescopes can be hidden so only the owner can access view it.
        //      Do we do that in the config level, or here?

        return response.Match(
            Outcome.FromSuccess,
            _ => {
                logger.Warning("Failed to get LoreScopes");
                return Outcome.FromError("Failed to get LoreScopes");
            }
        );
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetLoreScopesByOwnerQuery command, CancellationToken ct = default) 
        => protectionRules.IsOwnerAsync(command.AccessingUser, command.UserId, ct);
}
