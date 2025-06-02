// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Notifications;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.Messaging.Receivers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LoreScopeRemovedReceiver(
    IUnitOfWorkFactory unitOfWorkFactory,
    IS3FileStorage fileStorage,
    ILogger<LoreScopeRemovedReceiver> logger
) : IEventHandler<LoreScopeRemovedEvent> {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task HandleAsync(LoreScopeRemovedEvent eventModel, CancellationToken ct) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var markdownFileRepository = await unitOfWork.GetRepositoryAsync<ILsMarkdownFileRepository>(ct);
        var s3FileMetaDataRepository = await unitOfWork.GetRepositoryAsync<IS3FileRepository>(ct);

        Result<LsMarkdownFileModel[]> markdownFilesResult = await markdownFileRepository.GetByOwnerAsync(
            eventModel.LoreScopeId,
            QueryConfig.WithRetrieveSoftDeleted,
            ct: ct
        );
        
        if (!markdownFilesResult.TryGetAsSuccess(out LsMarkdownFileModel[]? markdownFiles)) {
            logger.Warning("Failed to get markdown files for lore scope {LoreScopeId}", eventModel.LoreScopeId);
            return;
        }

        // Fetch all the data needed for the S3 files
        (Guid LoreScopeId, string fileName)[] s3Data = markdownFiles
            .Select(static model => (model.OwnerId, model.S3FileMetaData!.FileName))
            .ToArray();

        IEnumerable<Task> s3MetaDataTasks = RemoveS3MetaDataModelsAsync(markdownFiles, s3FileMetaDataRepository, ct);
        await Task.WhenAll(s3MetaDataTasks);
        
        IEnumerable<Task> modelTasks = RemoveMarkdownFileModelsAsync(markdownFiles, markdownFileRepository, ct);
        await Task.WhenAll(modelTasks);

        await unitOfWork.TryCommitTransactionAsync(ct);
        
        // Only execute the s3File manipulation after the sal db has been commited.
        IEnumerable<Task> s3FileTasks = RemoveS3FilesAsync(s3Data, ct);
        await Task.WhenAll(s3FileTasks);
        
        string bucketName = S3BucketNames.GetLoreScopeBucket(eventModel.LoreScopeId);
        Result result = await fileStorage.TryRemoveBucketAsync(bucketName, ct);
        result.Switch(
            state => logger.Information("Deletion bucket {BucketName} was {state}", bucketName, state),
            _ => logger.Warning("Failed to remove bucket {BucketName}", bucketName)
        );
    }

    private IEnumerable<Task> RemoveS3FilesAsync(
        (Guid LoreScopeId, string FileName)[] markdownFileModels,
        CancellationToken ct
    ) => markdownFileModels.Select(async box => {
            string bucketName = S3BucketNames.GetLoreScopeBucket(box.LoreScopeId);
            
            Result removedResult = await fileStorage.TryRemoveFileAsync(bucketName, box.FileName, ct);
            if (!removedResult.TryGetAsState(out bool? removed) || removed is false) {
                logger.Warning("Failed to remove file from S3");
                return;
            }
            
            logger.Information("Removed file {FileName} from bucket {BucketName}", box.FileName, bucketName);
        }
    );
    
    private IEnumerable<Task> RemoveS3MetaDataModelsAsync(
        LsMarkdownFileModel[] markdownFileModels,
        IS3FileRepository s3FileMetaDataRepository,
        CancellationToken ct
    ) => markdownFileModels.Select(
        async model => {
            if (model.S3FileMetaData is null) return;
            
            Result removedResult = await s3FileMetaDataRepository.RemoveAsync(model.S3FileMetaData, ct);
            if (!removedResult.TryGetAsState(out bool? removed) || removed is false) {
                logger.Warning("Failed to remove S3FileMetaDataModel");
                return;
            }
            
            logger.Information("Removed S3FileMetaDataModel {S3FileMetaDataId}", model.S3FileMetaData.Id);
        } 
    );

    private IEnumerable<Task> RemoveMarkdownFileModelsAsync(
        LsMarkdownFileModel[] markdownFileModels,
        ILsMarkdownFileRepository markdownFileRepository,
        CancellationToken ct
    ) =>  markdownFileModels.Select(async model => {
            Result removedResult = await markdownFileRepository.RemoveAsync(model, ct);
            if (!removedResult.TryGetAsState(out bool? removed) || removed is false) {
                logger.Warning("Failed to remove markdown file");
                return;
            }

            logger.Information("Removed markdown file {MarkdownFileId}", model.Id);
        }
    );
}
