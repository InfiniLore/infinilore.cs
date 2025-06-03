// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LoreScopes.Messaging.Commands;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Result=InfiniLore.Modules.Core.Server.Result;

namespace InfiniLore.Modules.LoreScopes.Server.Messaging.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UpsertLoreScopeImageHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    IS3FileStorage fileStorage,
    IValidator<LoreScopeModel> loreScopeValidator,
    IValidator<S3FileMetaDataModel> s3FileValidator,
    ILogger<UpsertLoreScopeImageHandler> logger
) : CommandHandler<UpsertLoreScopeImageRequest, Result> {

    public override async Task<Result> ExecuteAsync(UpsertLoreScopeImageRequest command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = await unitOfWorkFactory.CreateWithTransactionAsync(ct);
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        AterraEngine.Unions.Result<LoreScopeModel> loreScopeResult = await loreScopeRepo.GetByIdAsync(command.LoreScopeId, QueryConfig.WithOptional, ct:ct);
        if (!loreScopeResult.TryGetAsSuccess(out LoreScopeModel? loreScope)) {
            logger.Warning("Failed to find lorescope with id {LoreScopeId}", command.LoreScopeId);
            return Result.FromError("Failed to find lorescope with id");
        }
        
        S3FileMetaDataModel? metaData = await TryCreateNewMetaDataAsync(command, loreScope, unitOfWork, ct);
        if (metaData is null) {
            logger.Warning("Failed to create new lorescope image metadata");
            return Result.FromError("Failed to create new lorescope image metadata");
        }

        AterraEngine.Unions.Result result = await fileStorage.TryUploadFileAsync(
            S3BucketNames.GetLoreScopeBucket(loreScope.Id),
            metaData.FileName,
            command.FileStream,
            command.ContentType,
            ct
        );
        
        if (!result.TryGetAsState(out bool? success) || success is false) {
            logger.Warning("Failed to upload file to s3 bucket");
            return Result.FromError("Failed to upload file to s3 bucket");       
        }
        logger.LogInformation("Uploaded file to s3 bucket");

        
        // ReSharper disable once InvertIf
        if (!await unitOfWork.TryCommitTransactionAsync(ct)) {
            logger.Warning("Failed to commit transaction");
            return Result.FromError("Failed to commit transaction");      
        }
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