// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server;
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
public class LoreScopeRemovedReceiver(
    IUnitOfWorkFactory unitOfWorkFactory,
    IS3FileStorage fileStorage,
    ILogger<LoreScopeRemovedReceiver> logger
) : EventReceiver<LoreScopeRemovedEvent>(logger) {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task ExecuteAsync(LoreScopeRemovedEvent eventModel, CancellationToken ct) {
        await using IUnitOfWork unitOfWork = await unitOfWorkFactory.CreateWithTransactionAsync(ct);
        var s3FileMetaDataRepository = await unitOfWork.GetRepositoryAsync<IS3FileRepository>(ct);
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        string bucketName = S3BucketNames.GetLoreScopeBucket(eventModel.LoreScopeId);

        Result<LoreScopeModel> modelResult = await loreScopeRepo.GetByIdAsync(
            eventModel.LoreScopeId,
            new QueryConfig {
                OptionalInclude = true,
                RetrieveSoftDeleted = true
            },
            ct: ct
        );

        if (!modelResult.TryGetAsData(out LoreScopeModel? model)) {
            logger.Warning("Failed to find lorescope with id {LoreScopeId}", eventModel.LoreScopeId);
            return;
        }

        if (model.PosterImageMetaData is not {} posterModel) {
            logger.Warning("LoreScope {id} did not have a poster image ", eventModel.LoreScopeId);
            return;
        }
        
        string fileName = posterModel.FileName;
        
        Result result = await s3FileMetaDataRepository.RemoveAsync(posterModel, ct);
        if (!result.TryGetAsState(out bool? removed) || removed is false) {
            logger.Warning("Failed to remove poster image for lorescope {LoreScopeId}", eventModel.LoreScopeId);
            return;
        }

        await unitOfWork.TryCommitTransactionAsync(ct);
        
        // Remove from S3, only after db has been commited
        Result s3Result = await fileStorage.TryRemoveFileAsync(
            bucketName,
            fileName,
            ct
        );
        
        if (!s3Result.TryGetAsState(out removed) || removed is false){
            logger.Warning("Failed to remove poster image for lorescope {LoreScopeId}", eventModel.LoreScopeId);
        }

        Result bucketRemovedResult = await fileStorage.TryRemoveBucketAsync(bucketName, ct);
        if (!bucketRemovedResult.TryGetAsState(out removed) || removed is false) {
            logger.Warning("Failed to remove bucket for lorescope {LoreScopeId}", eventModel.LoreScopeId);
        }
        
        logger.Information("Successfully removed lorescope {LoreScopeId} from S3 and database", eventModel.LoreScopeId);

    }
}
