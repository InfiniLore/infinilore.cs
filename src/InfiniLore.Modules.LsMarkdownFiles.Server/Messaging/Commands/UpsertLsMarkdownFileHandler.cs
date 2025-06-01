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
        
        Result loreScopeExistsResult = await loreScopeRepository.IsIdTakenAsync(command.LoreScopeId, ct:ct);
        if (!loreScopeExistsResult.TryGetAsState(out bool? success) || success is false) {
            logger.Warning("Failed to find lorescope with id {LoreScopeId}", command.LoreScopeId);
            return MessageResponse.FromErrorString("Failed to find lorescope with id");
        }
        
        Result<LsMarkdownFileModel> knownFileResult = await markdownFileRepo.GetByIdAsync(command.KnownLsMarkdownFileId, ct:ct);
        
        // If the Known is set to default, it will be empty and thus result in a new file being created.
        S3FileMetaDataModel s3FileMetaData = knownFileResult.Match(
            model => model.S3FileMetaData ?? throw new InvalidOperationException("Known file model does not have a S3FileMetaDataModel"),
            _ => new S3FileMetaDataModel {
                ContentType = "text/markdown",
                FileName = command.FileName
            }
        );
        
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

        LsMarkdownFileModel markdownFileModel = knownFileResult.Match(
            model => {
                model.LastUserToEditId = command.AccessingUser.UserId;
                model.S3FileMetaDataId = s3FileMetaData.Id;
                model.Name = command.FileName;
                return model;
            },
            _ => new LsMarkdownFileModel {
                OwnerId = command.LoreScopeId,

                LastUserToEditId = command.AccessingUser.UserId,
                S3FileMetaDataId = s3FileMetaData.Id,
                Name = command.FileName,
            });
        
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