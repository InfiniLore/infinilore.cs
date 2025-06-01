// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging;
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
) : CommandHandler<DeleteLsMarkdownFileRequest, MessageResponse> {

    public override async Task<MessageResponse> ExecuteAsync(DeleteLsMarkdownFileRequest command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = await unitOfWorkFactory.CreateWithTransactionAsync(ct);
        var markdownFileRepo = await unitOfWork.GetRepositoryAsync<ILsMarkdownFileRepository>(ct);
        var s3FileMetaDataRepo = await unitOfWork.GetRepositoryAsync<IS3FileRepository>(ct);

        // Try and find an existing model
        Result<LsMarkdownFileModel> existingModel = await markdownFileRepo.GetByIdAsync(command.MarkdownFileId, ct: ct);
        if (!existingModel.TryGetAsSuccess(out LsMarkdownFileModel? markdownFileModel)) {
            logger.Warning("Failed to find markdown file with id {MarkdownFileId}", command.MarkdownFileId);
            return MessageResponse.FromErrorString("Failed to find markdown file with id");
        }

        // Delete the file and the metadata
        if (markdownFileModel.S3FileMetaData is {} metaData) {
            string bucketName = markdownFileModel.GetLoreScopeBucketName();
            Result s3DeleteResult = await fileStorage.TryDeleteFileAsync(bucketName, metaData.FileName, ct);
            if (!s3DeleteResult.TryGetAsState(out bool? success) || success is false) {
                logger.Warning("Failed to delete file from S3");
            }
            
            Result metaDataDeleteResult = await s3FileMetaDataRepo.DeleteAsync(metaData, ct);
            if (!metaDataDeleteResult.TryGetAsState(out success) || success is false) {
                logger.Warning("Failed to delete S3FileMetaDataModel");
            }
        }
        
        // Delete the registrations
        Result deleteResult = await markdownFileRepo.DeleteAsync(markdownFileModel, ct);
        if (!deleteResult.TryGetAsState(out bool? deleted) || deleted is false) {
            logger.Warning("Failed to delete markdown file");
            return MessageResponse.FromErrorString("Failed to delete markdown file");       
        }
        await unitOfWork.TryCommitTransactionAsync(ct);
        return true;
    }
} 