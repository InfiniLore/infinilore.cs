// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Database.Models.Data.User;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.Mediator.Queries.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class GetLorescopeByIdHandler {
    public static async Task<MediatorResponse<LoreScope>> HandleAsync(
        // Message
        GetLorescopeByIdQuery message,
        // Services
        IReadonlyUnitOfWorkFactory factory, ILogger logger,
        // CT
        CancellationToken ct
    ) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        var queryConfig = new QueryConfig(AutoInclude: message.AutoInclude);
        Result<LoreScope> response = await loreScopeRepository.GetByIdAsync(message.LorescopeId, queryConfig, ct);

        if (!response.TryGetAsSuccess(out LoreScope? value)) {
            logger.Warning("Failed to get lorescope");
            return MediatorResponse<LoreScope>.FromErrorString("Failed to get lorescope");
        }

        // ReSharper disable once InvertIf
        if (!message.IsLoreScopeOnly && value.OwnerId != message.UserId) {
            logger.Warning("User does not own this lorescope");
            return MediatorResponse<LoreScope>.FromErrorString("User does not own this lorescope");
        }

        return MediatorResponse<LoreScope>.FromSuccess(value);
    }
}
