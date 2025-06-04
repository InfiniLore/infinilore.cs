// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using InfiniLore.Server.Modules.LsMarkdownFiles.Messaging.Commands;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.Messaging.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class DeleteLsMarkdownFileHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    IS3FileStorage fileStorage,
    ILogger<DeleteLsMarkdownFileHandler> logger
) : CommandHandler<DeleteLsMarkdownFileRequest, Outcome> {

    public override async Task<Outcome> ExecuteAsync(DeleteLsMarkdownFileRequest command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = await unitOfWorkFactory.CreateWithTransactionAsync(ct);
        var markdownFileRepo = await unitOfWork.GetRepositoryAsync<ILsMarkdownFileRepository>(ct);
        var s3FileMetaDataRepo = await unitOfWork.GetRepositoryAsync<IS3FileRepository>(ct);

        // Try and find an existing model
        RepoOutcome<LsMarkdownFileModel> existingModel = await markdownFileRepo.GetByIdAsync(command.MarkdownFileId, ct: ct);
        if (!existingModel.TryGetAsData(out LsMarkdownFileModel? markdownFileModel)) {
            logger.Warning("Failed to find markdown file with id {MarkdownFileId}", command.MarkdownFileId);
            return Outcome.FromError("Failed to find markdown file with id");
        }

        // Delete the file and the metadata
        if (markdownFileModel.S3FileMetaData is {} metaData) {
            string bucketName = S3BucketNames.GetLoreScopeBucket(markdownFileModel.OwnerId);
            Outcome s3DeleteResult = await fileStorage.TryRemoveFileAsync(bucketName, metaData.FileName, ct);
            if (!s3DeleteResult.TryGetAsState(out bool success) || !success ) {
                logger.Warning("Failed to delete file from S3");
            }
            
            RepoOutcome metaDataDeleteResult = await s3FileMetaDataRepo.DeleteAsync(metaData, ct);
            if (!metaDataDeleteResult.TryGetAsState(out success) || !success) {
                logger.Warning("Failed to delete S3FileMetaDataModel");
            }
        }
        
        // Delete the registrations
        RepoOutcome deleteResult = await markdownFileRepo.DeleteAsync(markdownFileModel, ct);
        if (!deleteResult.TryGetAsState(out bool deleted) || !deleted) {
            logger.Warning("Failed to delete markdown file");
            return Outcome.FromError("Failed to delete markdown file");       
        }
        await unitOfWork.TryCommitTransactionAsync(ct);
        return Outcome.True;
    }
} 