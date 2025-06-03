// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
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
    ILogger<LoreScopeDeletedReceiver> logger
) : IEventHandler<LoreScopeDeletedEvent> {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task HandleAsync(LoreScopeDeletedEvent eventModel, CancellationToken ct) {
        await using IUnitOfWork unitOfWork = await unitOfWorkFactory.CreateWithTransactionAsync(ct);
        var markdownFileRepository = await unitOfWork.GetRepositoryAsync<ILsMarkdownFileRepository>(ct);
        var s3FileMetaDataRepository = await unitOfWork.GetRepositoryAsync<IS3FileRepository>(ct);

        Result<LsMarkdownFileModel[]> markdownFilesResult = await markdownFileRepository.GetByOwnerAsync(eventModel.LoreScopeId, ct: ct);
        if (!markdownFilesResult.TryGetAsData(out LsMarkdownFileModel[]? markdownFiles)) {
            logger.Warning("Failed to get markdown files for lore scope {LoreScopeId}", eventModel.LoreScopeId);
            return;
        }
        
        IEnumerable<Task> s3MetaDataTasks = DeleteS3MetaDataModelsAsync(markdownFiles, s3FileMetaDataRepository, ct);
        await Task.WhenAll(s3MetaDataTasks);
        
        IEnumerable<Task> modelTasks = DeleteMarkdownFileModelsAsync(markdownFiles, markdownFileRepository, ct);
        await Task.WhenAll(modelTasks);

        await unitOfWork.TryCommitTransactionAsync(ct);
        
        // In Soft deletion we don't remove the resources from s3FileServer,
        //      we do that at a later stage using "LoreScopeRemovedEvent";
    }
    
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
    ) =>  markdownFileModels.Select(async model => {
            Result deletedResult = await markdownFileRepository.DeleteAsync(model, ct);
            if (!deletedResult.TryGetAsState(out bool? deleted) || deleted is false) {
                logger.Warning("Failed to delete markdown file");
                return;
            }

            logger.Information("Deleted markdown file {MarkdownFileId}", model.Id);
        }
    );
}
