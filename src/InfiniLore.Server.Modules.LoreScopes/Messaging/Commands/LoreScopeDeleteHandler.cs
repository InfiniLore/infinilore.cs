// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LoreScopeDeleteHandler(IUnitOfWorkFactory unitOfWorkFactory, ILogger<LoreScopeDeleteHandler> logger) : CommandHandler<DeleteLoreScopeRequest, MessageResponse> {
    public override async Task<MessageResponse> ExecuteAsync(DeleteLoreScopeRequest command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        Result result = await loreScopeRepo.DeleteByIdAsync(command.LoreScopeId, ct);
        if (result.IsError) {
            logger.Warning("Failed to delete lorescope at id {id}", command.LoreScopeId);
            return MessageResponse.FromErrorString("Failed to delete lorescope");
        }
        return true;
    }
}
