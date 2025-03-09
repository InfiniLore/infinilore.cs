// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database;
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

        RepoResult<LoreScope> response = request switch {
            { AutoInclude: false, LorescopeId: var lorescopeId } => await loreScopeRepository.GetByIdAsync(lorescopeId, ct),
            { AutoInclude: true, LorescopeId: var lorescopeId } => await loreScopeRepository.GetByIdWithAutoIncludeAsync(lorescopeId, ct),
            _ => RepoResult<LoreScope>.FromError("Invalid query")
        };

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
