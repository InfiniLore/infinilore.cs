// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Database.Models.Data.User;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.Messaging.Queries.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLoreScopesHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetLoreScopesHandler> logger) : CommandHandler<GetLoreScopesQuery, MessageResponse<PaginatedData<LoreScope>>> {
    public override async Task<MessageResponse<PaginatedData<LoreScope>>> ExecuteAsync(GetLoreScopesQuery command, CancellationToken ct = new()) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        var queryConfig = new QueryConfig(command.AutoInclude, command.Reverse);
        PaginatedResult<LoreScope> response = await loreScopeRepository.GetByUserAsync(command.UserId, command.PaginationInfo, queryConfig, ct);

        // Todo lorescopes can be hidden so only the owner can access view it.
        //      Do we do that in the config level, or here?

        if (!response.TryGetAsSuccess(out PaginatedData<LoreScope> paginatedResult)) {
            logger.Warning("Failed to get LoreScopes");
            return MessageResponse<PaginatedData<LoreScope>>.FromErrorString("Failed to get LoreScopes");
        }

        return MessageResponse<PaginatedData<LoreScope>>.FromSuccess(paginatedResult);
    }

}
