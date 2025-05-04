// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Database.Models;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLorescopeByIdHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetLorescopeByIdHandler> logger) : CommandHandler<GetLorescopeByIdQuery, MessageResponse<LoreScope>> {
    public override async Task<MessageResponse<LoreScope>> ExecuteAsync(GetLorescopeByIdQuery command, CancellationToken ct = new()) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        var queryConfig = new QueryConfig(AutoInclude: command.AutoInclude);
        Result<LoreScope> response = await loreScopeRepository.GetByIdAsync(command.LorescopeId, queryConfig, ct);

        if (!response.TryGetAsSuccess(out LoreScope? value)) {
            logger.Warning("Failed to get lorescope");
            return MessageResponse<LoreScope>.FromErrorString("Failed to get lorescope");
        }

        // ReSharper disable once InvertIf
        if (!command.IsLoreScopeOnly && value.OwnerId != command.UserId) {
            logger.Warning("User does not own this lorescope");
            return MessageResponse<LoreScope>.FromErrorString("User does not own this lorescope");
        }

        return MessageResponse<LoreScope>.FromSuccess(value);
    }
}
