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
public class LoreScopeDeletedReceiver(
    IUnitOfWorkFactory unitOfWorkFactory,
    IS3FileStorage fileStorage,
    ILogger<LoreScopeDeletedReceiver> logger
) : IEventHandler<LoreScopeDeletedEvent> {

    public async Task HandleAsync(LoreScopeDeletedEvent eventModel, CancellationToken ct) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var markdownFileRepository = await unitOfWork.GetRepositoryAsync<ILsMarkdownFileRepository>(ct);
        var s3FileMetaDataRepository = await unitOfWork.GetRepositoryAsync<IS3FileRepository>(ct);

        Result<LsMarkdownFileModel[]> markdownFilesResult = await markdownFileRepository.GetByOwnerAsync(eventModel.LoreScopeId, ct: ct);
        if (!markdownFilesResult.TryGetAsSuccess(out LsMarkdownFileModel[]? markdownFiles)) {
            logger.Warning("Failed to get markdown files for lore scope {LoreScopeId}", eventModel.LoreScopeId);
            return;
        }

        // Fetch all the data needed for the S3 files
        (Guid LoreScopeId, string fileName)[] s3Data = markdownFiles
            .Select(model => (model.OwnerId, model.S3FileMetaData!.FileName))
            .ToArray();

        IEnumerable<Task> s3MetaDataTasks = DeleteS3MetaDataModelsAsync(markdownFiles, s3FileMetaDataRepository, ct);
        await Task.WhenAll(s3MetaDataTasks);
        
        IEnumerable<Task> modelTasks = DeleteMarkdownFileModelsAsync(markdownFiles, markdownFileRepository, ct);
        await Task.WhenAll(modelTasks);

        await unitOfWork.TryCommitTransactionAsync(ct);
        
        // Only execute the s3File manipulation after the sal db has been commited.
        IEnumerable<Task> s3FileTasks = DeleteS3FilesAsync(s3Data, ct);
        await Task.WhenAll(s3FileTasks);
    }

    private IEnumerable<Task> DeleteS3FilesAsync(
        (Guid LoreScopeId, string FileName)[] markdownFileModels,
        CancellationToken ct
    ) => markdownFileModels.Select(async box => {
            string bucketName = S3BucketNames.GetLoreScopeBucket(box.LoreScopeId);
            
            Result deletedResult = await fileStorage.TryDeleteFileAsync(bucketName, box.FileName, ct);
            if (!deletedResult.TryGetAsState(out bool? deleted) || deleted is false) {
                logger.Warning("Failed to delete file from S3");
                return;
            }
            
            logger.Information("Deleted file {FileName} from bucket {BucketName}", box.FileName, bucketName);
        }
    );
    
    private IEnumerable<Task> DeleteS3MetaDataModelsAsync(
        LsMarkdownFileModel[] markdownFileModels,
        IS3FileRepository s3FileMetaDataRepository,
        CancellationToken ct
    ) => markdownFileModels.Select(
        async model => {
            if (model.S3FileMetaData is null) return;
            
            Result deletedResult = await s3FileMetaDataRepository.DeleteAsync(model.S3FileMetaData, ct);
            if (!deletedResult.TryGetAsState(out bool? deleted) || deleted is false) {
                logger.Warning("Failed to delete S3FileMetaDataModel");
                return;
            }
            
            logger.Information("Deleted S3FileMetaDataModel {S3FileMetaDataId}", model.S3FileMetaData.Id);
        } 
    );

    private IEnumerable<Task> DeleteMarkdownFileModelsAsync(
        LsMarkdownFileModel[] markdownFileModels,
        ILsMarkdownFileRepository markdownFileRepository,
        CancellationToken ct
    ) => markdownFileModels.Select(async model => {
            Result deletedResult = await markdownFileRepository.DeleteAsync(model, ct);
            if (!deletedResult.TryGetAsState(out bool? deleted) || deleted is false) {
                logger.Warning("Failed to delete markdown file");
                return;
            }

            logger.Information("Deleted markdown file {MarkdownFileId}", model.Id);
        }
    );
}
