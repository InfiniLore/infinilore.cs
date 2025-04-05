// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Database.Models.Data.User;
using Microsoft.Extensions.Logging;
using Wolverine.Attributes;

namespace InfiniLore.Server.Services.Mediator.Queries.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[WolverineHandler]
public static class GetLoreScopesHandler {
    public static async Task<MediatorResponse<PaginatedData<LoreScope>>> HandleAsync(
        // Message
        GetLoreScopesQuery message,
        // Services
        IReadonlyUnitOfWorkFactory factory, ILogger logger,
        // CT
        CancellationToken ct
    ) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        var queryConfig = new QueryConfig(message.AutoInclude, message.Reverse);
        PaginatedResult<LoreScope> response = await loreScopeRepository.GetByUserAsync(message.UserId, message.PaginationInfo, queryConfig, ct);

        // Todo lorescopes can be hidden so only the owner can access view it.
        //      Do we do that in the config level, or here?

        if (!response.TryGetAsSuccess(out PaginatedData<LoreScope> paginatedResult)) {
            logger.Warning("Failed to get LoreScopes");
            return MediatorResponse<PaginatedData<LoreScope>>.FromErrorString("Failed to get LoreScopes");
        }

        return MediatorResponse<PaginatedData<LoreScope>>.FromSuccess(paginatedResult);
    }
}
