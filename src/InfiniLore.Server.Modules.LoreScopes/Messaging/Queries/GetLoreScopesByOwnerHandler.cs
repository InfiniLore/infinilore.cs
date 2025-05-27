// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Core.Messaging.Handlers;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Shared;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLoreScopesByOwnerHandler(
    IReadonlyUnitOfWorkFactory factory,
    ILogger<GetLoreScopesByOwnerHandler> logger
) : AccessRestrictedCommandHandler<GetLoreScopesByOwnerQuery, PaginatedData<LoreScopeModel>>(logger) {

    protected override async Task<MessageResponse<PaginatedData<LoreScopeModel>>> HandleCommandAsync(GetLoreScopesByOwnerQuery command, CancellationToken ct = default) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        Core.Database.PaginatedResult<LoreScopeModel> response = await loreScopeRepository.GetByOwnerAsync(command.UserId, command.PaginationInfo, command.QueryConfig, ct);

        // Todo lorescopes can be hidden so only the owner can access view it.
        //      Do we do that in the config level, or here?

        if (!response.TryGetAsSuccess(out PaginatedData<LoreScopeModel> paginatedResult)) {
            logger.Warning("Failed to get LoreScopes");
            return MessageResponse.FromErrorString("Failed to get LoreScopes");
        }

        return MessageResponse.FromSuccess(paginatedResult);
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetLoreScopesByOwnerQuery command, CancellationToken ct = default) {
        // TODO find a way so that we can check if a user can access the data from another user, rather than just a resource.
        return ValueTask.FromResult(true);
    }
}
