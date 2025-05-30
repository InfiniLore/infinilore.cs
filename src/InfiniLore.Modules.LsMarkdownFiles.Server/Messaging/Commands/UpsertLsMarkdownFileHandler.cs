// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using InfiniLore.Server.Modules.LsMarkdownFiles.Messaging.Commands;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.Messaging.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UpsertLsMarkdownFileHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    IS3FileStorage fileStorage,
    IValidator<S3FileMetaDataModel> s3FileValidator,
    ILogger<UpsertLsMarkdownFileHandler> logger
) : CommandHandler<UpsertLsMarkdownFileRequest, MessageResponse> {

    public override async Task<MessageResponse> ExecuteAsync(UpsertLsMarkdownFileRequest command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = await unitOfWorkFactory.CreateWithTransactionAsync(ct);
        var loreScopeRepository = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        var markdownFileRepo = await unitOfWork.GetRepositoryAsync<ILsMarkdownFileRepository>(ct);
        var s3FileMetaDataRepo = await unitOfWork.GetRepositoryAsync<IS3FileRepository>(ct);
        
        Result idExistsResult = await loreScopeRepository.IsIdTakenAsync(command.LoreScopeId, ct:ct);
        if (!idExistsResult.TryGetAsState(out bool? success) || success is false) {
            logger.Warning("Failed to find lorescope with id {LoreScopeId}", command.LoreScopeId);
            return MessageResponse.FromErrorString("Failed to find lorescope with id");
        }

        var s3FileMetaData = new S3FileMetaDataModel {
            ContentType = "text/markdown",
            FileName = command.FileName
        };
        ValidationResult? validationResult = await s3FileValidator.ValidateAsync(s3FileMetaData, ct);
        if (!validationResult.IsValid) {
            logger.Warning("Failed to validate S3FileMetaDataModel");
            return MessageResponse.FromErrorString("Failed to validate S3FileMetaDataModel");
        }

        Result s3RepoResult = await s3FileMetaDataRepo.AddOrUpdateAsync(s3FileMetaData, ct);
        if (!s3RepoResult.TryGetAsState(out success) || success is false) {
            logger.Warning("Failed to add or update S3FileMetaDataModel");
            return MessageResponse.FromErrorString("Failed to add or update S3FileMetaDataModel");
        }
        
        Result<LsMarkdownFileModel> existingModelResult = await markdownFileRepo.GetByNameAndOwnerAsync(command.FileName, command.LoreScopeId, ct:ct);
        if (!existingModelResult.TryGetAsSuccess(out LsMarkdownFileModel? markdownFileModel)) {
            markdownFileModel = new LsMarkdownFileModel {
                OwnerId = command.LoreScopeId,
                
                LastUserToEditId = command.AccessingUser.UserId,
                S3FileMetaDataId = s3FileMetaData.Id,
                Name = command.FileName,
            };
        }
        else {
            markdownFileModel.LastUserToEditId = command.AccessingUser.UserId;
            markdownFileModel.S3FileMetaDataId = s3FileMetaData.Id;
            markdownFileModel.Name = command.FileName;
        }
        
        Result fileRepoResult = await markdownFileRepo.AddOrUpdateAsync(markdownFileModel, ct);
        if (!fileRepoResult.TryGetAsState(out success) || success is false) {
            logger.Warning("Failed to add or update LsMarkdownFileModel");
            return MessageResponse.FromErrorString("Failed to add or update LsMarkdownFileModel");
        }
        
        Result fileUploadResult = await fileStorage.TryUploadFileAsync(
            $"lorescope-{command.LoreScopeId.ToString().ToLowerInvariant()}", // TODO - make this a service or something globally handled
            s3FileMetaData.FileName,
            command.FileStream,
            "text/markdown", 
            ct
        );
        if (!fileUploadResult.TryGetAsState(out success) || success is false) {
            logger.Warning("Failed to upload file to S3");
            return MessageResponse.FromErrorString("Failed to upload file to S3");
        }
        
        // ReSharper disable once InvertIf
        if (!await unitOfWork.TryCommitTransactionAsync(ct)) {
            logger.Warning("Failed to commit transaction");
            return MessageResponse.FromErrorString("Failed to commit transaction");      
        }
        return true;
    }
} 