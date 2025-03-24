// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Database.Models.Data.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.CQRS.Queries.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetLoreScopesHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetLoreScopesHandler> logger) : IRequestHandler<GetLoreScopesQuery, MediatorResponse<PaginatedData<LoreScope>>> {

    public async Task<MediatorResponse<PaginatedData<LoreScope>>> Handle(GetLoreScopesQuery request, CancellationToken ct) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        
        var queryConfig = new QueryConfig(AutoInclude: request.AutoInclude, Reverse: request.Reverse);
        PaginatedResult<LoreScope> response = await loreScopeRepository.GetByUserAsync(request.UserId,request.PaginationInfo, queryConfig, ct);

        if (!response.TryGetAsSuccess(out PaginatedData<LoreScope> paginatedResult)) {
            logger.Warning("Failed to get LoreScopes");
            return MediatorResponse<PaginatedData<LoreScope>>.FromErrorString("Failed to get LoreScopes");
        }

        return MediatorResponse<PaginatedData<LoreScope>>.FromSuccess(paginatedResult);
    }
}
