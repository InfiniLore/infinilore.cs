// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Shared;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLoreScopesHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetLoreScopesHandler> logger) : CommandHandler<GetLoreScopesQuery, MessageResponse<PaginatedData<LoreScopeModel>>> {
    public override async Task<MessageResponse<PaginatedData<LoreScopeModel>>> ExecuteAsync(GetLoreScopesQuery command, CancellationToken ct = new()) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        var queryConfig = new QueryConfig(command.AutoInclude, command.Reverse);
        PaginatedResult<LoreScopeModel> response = await loreScopeRepository.GetByOwnerAsync(command.UserId, command.PaginationInfo, queryConfig, ct);

        // Todo lorescopes can be hidden so only the owner can access view it.
        //      Do we do that in the config level, or here?

        if (!response.TryGetAsSuccess(out PaginatedData<LoreScopeModel> paginatedResult)) {
            logger.Warning("Failed to get LoreScopes");
            return MessageResponse<PaginatedData<LoreScopeModel>>.FromErrorString("Failed to get LoreScopes");
        }

        return MessageResponse<PaginatedData<LoreScopeModel>>.FromSuccess(paginatedResult);
    }
}
