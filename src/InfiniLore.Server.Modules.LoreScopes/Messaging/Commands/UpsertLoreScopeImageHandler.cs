// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.LoreScopes.Database;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UpsertLoreScopeImageHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    IS3FileStorage fileStorage,
    IValidator<LoreScopeModel> loreScopeValidator,
    IValidator<S3FileMetaDataModel> s3FileValidator,
    ILogger<LoreScopeCreateHandler> logger
) : CommandHandler<UpsertLoreScopeImageRequest, MessageResponse> {

    public override async Task<MessageResponse> ExecuteAsync(UpsertLoreScopeImageRequest command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = await unitOfWorkFactory.CreateWithTransactionAsync(ct);
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        
        Result<LoreScopeModel> loreScope = await loreScopeRepo.GetByIdAsync(command.LoreScopeId, QueryConfig.WithOptional, ct:ct);
        if (!loreScope.TryGetAsSuccess(out LoreScopeModel foundModel)) {
            logger.Warning("Failed to find lorescope with id {LoreScopeId}", command.LoreScopeId);
            return MessageResponse.FromErrorString("Failed to find lorescope with id");
        }
        
        S3FileMetaDataModel? metaData = await TryCreateNewMetaDataAsync(command, foundModel, unitOfWork, ct);
        if (metaData is null) {
            logger.Warning("Failed to create new lorescope image metadata");
            return MessageResponse.FromErrorString("Failed to create new lorescope image metadata");
        }

        if (!await fileStorage.TryUploadFileAsync(foundModel.S3BucketName, metaData.Id, command.FileStream, command.ContentType, ct)) {
            logger.Warning("Failed to upload file to s3 bucket");
            return MessageResponse.FromErrorString("Failed to upload file to s3 bucket");       
        }
        
        logger.LogInformation("Uploaded file to s3 bucket");
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
    
    private async Task<S3FileMetaDataModel?> TryCreateNewMetaDataAsync(UpsertLoreScopeImageRequest command, LoreScopeModel foundModel, IUnitOfWork unitOfWork, CancellationToken ct) {
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);
        var s3FileMetaDataRepo = await unitOfWork.GetRepositoryAsync<IS3FileRepository>(ct);
        
        var metaData = new S3FileMetaDataModel {
            ContentType = command.ContentType,
            FileName = command.FileName
        };
        
        ValidationResult validMetaData = await s3FileValidator.ValidateAsync(metaData, ct);
        if (!validMetaData.IsValid) {
            logger.Warning("Validation failed: {Reason}", validMetaData.Errors);
            return null;
        }
        
        foundModel.PosterImageMetaDataId = metaData.Id;
        foundModel.PosterImageMetaData = metaData;
        foundModel.UpdateLastModifiedDate();
        
        if (await loreScopeValidator.ValidateAsync(foundModel, ct) is {IsValid: false} validModel ) {
            logger.Warning("Validation failed: {Reason}", validModel.Errors);
            return null;       
        }

        await s3FileMetaDataRepo.AddAsync(metaData, ct);
        await loreScopeRepo.UpdateAsync(foundModel, ct);
        
        logger.LogInformation("Created new lorescope image metadata");
        return metaData;
    }
} 