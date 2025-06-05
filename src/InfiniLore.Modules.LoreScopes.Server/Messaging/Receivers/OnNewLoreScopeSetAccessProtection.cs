// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Notifications;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LoreScopes.Server.Messaging.Receivers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class OnNewLoreScopeSetAccessProtection(
    ILogger<OnNewLoreScopeSetAccessProtection> logger,
    IUnitOfWorkFactory unitOfWorkFactory
) : EventReceiver<NewLoreScopeCreatedEvent>(logger) {

    protected override async Task ExecuteAsync(NewLoreScopeCreatedEvent eventModel, CancellationToken ct) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var repo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        var accessProtectionRepo = await unitOfWork.GetRepositoryAsync<IAccessProtectionRepository>(ct);
        
        RepoOutcome<LoreScopeModel> outcome = await repo.GetByIdAsync(eventModel.LoreScopeId, ct: ct);
        if (!outcome.TryGetAsData(out LoreScopeModel? loreScope)) {
            logger.Warning("Failed to get lorescope with id {LoreScopeId}", eventModel.LoreScopeId);
            return;
        }
    
        Guid owner = loreScope.OwnerId;
        var accessProtection = new AccessProtectionModel {
            ModelOwnerId = owner,
            ProtectedModelId = loreScope.Id,
        };
        await accessProtectionRepo.AddAsync(accessProtection, ct);

        loreScope.AccessProtectionId = accessProtection.Id;
        loreScope.AccessProtection = accessProtection;
        
        await repo.UpdateAsync(loreScope, ct);
        logger.Information("Access protection set for lorescope {LoreScopeId}", loreScope.Id);
    }
}
