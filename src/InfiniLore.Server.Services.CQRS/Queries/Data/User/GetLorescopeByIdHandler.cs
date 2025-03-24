// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
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
public class GetLorescopeByIdHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetLorescopeByIdHandler> logger) : IRequestHandler<GetLorescopeByIdQuery, MediatorResponse<LoreScope>> {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task<MediatorResponse<LoreScope>> Handle(GetLorescopeByIdQuery request, CancellationToken ct) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        
        var queryConfig = new QueryConfig(AutoInclude: request.AutoInclude);
        Result<LoreScope> response = await loreScopeRepository.GetByIdAsync(request.LorescopeId, queryConfig, ct);

        if (!response.TryGetAsSuccess(out LoreScope? value)) {
            logger.Warning("Failed to get lorescope");
            return MediatorResponse<LoreScope>.FromErrorString("Failed to get lorescope");
        }

        // ReSharper disable once InvertIf
        if (!request.IsLoreScopeOnly && value.OwnerId != request.UserId) {
            logger.Warning("User does not own this lorescope");
            return MediatorResponse<LoreScope>.FromErrorString("User does not own this lorescope");
        }

        return MediatorResponse<LoreScope>.FromSuccess(value);
    }
}
