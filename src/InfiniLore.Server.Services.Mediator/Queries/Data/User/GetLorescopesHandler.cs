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
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.Mediator.Queries.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetLoreScopesHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetLoreScopesHandler> logger) : ICommandHandler<GetLoreScopesQuery, MediatorResponse<PaginatedData<LoreScope>>> {

    // public void Configure() {
    //     RequiresPermission(); // requires a permission to be set on the AccessData 
    //     RequiresRole(); // requires a role to be set on the AccessData
    //     OwnerAccessOnly(); // check if OwnerId matches AccessData.UserId, if not a special role else error
    //     EnableAccessRelation(); // checks the db, or the cache for the relation between AccessData and the entity whom is the owner of the queried data.
    // }

    public async Task<MediatorResponse<PaginatedData<LoreScope>>> Handle(GetLoreScopesQuery request, CancellationToken ct) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        var queryConfig = new QueryConfig(request.AutoInclude, request.Reverse);
        PaginatedResult<LoreScope> response = await loreScopeRepository.GetByUserAsync(request.UserId, request.PaginationInfo, queryConfig, ct);

        // Todo lorescopes can be hidden so only the owner can access view it.
        //      Do we do that in the config level, or here?

        if (!response.TryGetAsSuccess(out PaginatedData<LoreScope> paginatedResult)) {
            logger.Warning("Failed to get LoreScopes");
            return MediatorResponse<PaginatedData<LoreScope>>.FromErrorString("Failed to get LoreScopes");
        }

        return MediatorResponse<PaginatedData<LoreScope>>.FromSuccess(paginatedResult);
    }
}
