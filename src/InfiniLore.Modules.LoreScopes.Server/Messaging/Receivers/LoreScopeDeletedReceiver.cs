// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
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
public class LoreScopeDeletedReceiver(
    IUnitOfWorkFactory unitOfWorkFactory,
    ILogger<LoreScopeDeletedReceiver> logger
) : EventReceiver<LoreScopeDeletedEvent>(logger) {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task ExecuteAsync(LoreScopeDeletedEvent eventModel, CancellationToken ct) {
        await using IUnitOfWork unitOfWork = await unitOfWorkFactory.CreateWithTransactionAsync(ct);
        var s3FileMetaDataRepository = await unitOfWork.GetRepositoryAsync<IS3FileRepository>(ct);
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        Result<LoreScopeModel> modelResult = await loreScopeRepo.GetByIdAsync(
            eventModel.LoreScopeId,
            QueryConfig.WithRetrieveSoftDeleted,
            ct: ct
        );

        if (!modelResult.TryGetAsData(out LoreScopeModel? model)) {
            logger.Warning("Failed to find lorescope with id {LoreScopeId}", eventModel.LoreScopeId);
            return;
        }

        if (model.PosterImageMetaDataId is not {} posterImageMetaDataId) {
            logger.Warning("LoreScope {id} did not have a poster image ", eventModel.LoreScopeId);
            return;
        }
        
        Result result = await s3FileMetaDataRepository.DeleteByIdAsync(posterImageMetaDataId, ct);
        if (!result.TryGetAsState(out bool? deleted) || deleted is false) {
            logger.Warning("Failed to delete poster image for lorescope {LoreScopeId}", eventModel.LoreScopeId);
            return;
        }

        await unitOfWork.TryCommitTransactionAsync(ct);
    }
}
