// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Database.Models.Data.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.CQRS.Queries.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetLorescopesHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetLorescopesHandler> logger) : IRequestHandler<GetLorescopesQuery, MediatorResponse<PaginatedResult<LoreScope>>> {

    public async Task<MediatorResponse<PaginatedResult<LoreScope>>> Handle(GetLorescopesQuery request, CancellationToken ct) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        PaginatedRepoResult<LoreScope> response = request switch {
            { Reverse: false, AutoInclude: false } => await loreScopeRepository.GetByUserAsync(request.UserId, request.PaginationInfo, ct),
            { Reverse: false, AutoInclude: true } => await loreScopeRepository.GetByUserWithAutoIncludeAsync(request.UserId, request.PaginationInfo, ct),

            { Reverse: true, AutoInclude: false } => await loreScopeRepository.GetByUserReverseAsync(request.UserId, request.PaginationInfo, ct),
            { Reverse: true, AutoInclude: true } => await loreScopeRepository.GetByUserReverseWithAutoIncludeAsync(request.UserId, request.PaginationInfo, ct),

            _ => PaginatedRepoResult<LoreScope>.FromError("Invalid query")
        };

        if (!response.TryGetAsSuccess(out PaginatedResult<LoreScope> paginatedResult)) {
            logger.Warning("Failed to get lorescopes");
            return MediatorResponse<PaginatedResult<LoreScope>>.FromErrorString("Failed to get lorescopes");
        }

        return MediatorResponse<PaginatedResult<LoreScope>>.FromSuccess(paginatedResult);
    }
}
