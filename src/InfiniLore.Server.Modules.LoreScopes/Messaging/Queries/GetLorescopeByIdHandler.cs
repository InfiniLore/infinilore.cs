// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLorescopeByIdHandler(IReadonlyUnitOfWorkFactory factory, ILogger<GetLorescopeByIdHandler> logger) : CommandHandler<GetLorescopeByIdQuery, MessageResponse<LoreScopeModel>> {
    public override async Task<MessageResponse<LoreScopeModel>> ExecuteAsync(GetLorescopeByIdQuery command, CancellationToken ct = new()) {
        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        var queryConfig = new QueryConfig(AutoInclude: command.AutoInclude);
        Result<LoreScopeModel> response = await loreScopeRepository.GetByIdAsync(command.LorescopeId, queryConfig, ct);

        if (!response.TryGetAsSuccess(out LoreScopeModel value)) {
            logger.Warning("Failed to get lorescope");
            return MessageResponse<LoreScopeModel>.FromErrorString("Failed to get lorescope");
        }

        // ReSharper disable once InvertIf
        if (!command.IsLoreScopeOnly && value.OwnerId != command.UserId) {
            logger.Warning("User does not own this lorescope");
            return MessageResponse<LoreScopeModel>.FromErrorString("User does not own this lorescope");
        }

        return MessageResponse<LoreScopeModel>.FromSuccess(value);
    }
}
