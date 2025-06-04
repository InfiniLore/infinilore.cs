// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Modules.Core.Shared;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.Messaging.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UpsertUserProfileImageHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    IS3FileStorage fileStorage,
    IValidator<S3FileMetaDataModel> s3FileValidator,
    ILogger<UpsertUserProfileImageHandler> logger,
    IAccessProtectionRules protectionRules   
) : AccessProtectedCommandHandler<UpsertUserProfileImageRequest>(logger) {
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async ValueTask<bool> ValidateAccessAsync(UpsertUserProfileImageRequest command, CancellationToken ct = default) 
        => await protectionRules.IsOwnerAsync(command.AccessingUser, command.UserId, ct);
    
    protected override async Task<Outcome> HandleCommandAsync(UpsertUserProfileImageRequest command, CancellationToken ct = default) {
        await using IUnitOfWork unitOfWork = await unitOfWorkFactory.CreateWithTransactionAsync(ct);
        var userRepo = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        Outcome<InfiniLoreUserModel> userModelResult = await userRepo.GetByIdAsync(command.UserId, QueryConfig.WithOptional, ct:ct);
        Outcome outcome =  await userModelResult.MatchAsync(
            async model => await ProcessUserModelAsync(command, model, unitOfWork, ct),
            _ => {
                logger.Warning("Failed to find user with id {UserId}", command.UserId);
                return Task.FromResult(Outcome.FromError("Failed to find user with id"));
            }
        );

        return outcome;
    }

    private async Task<Outcome> ProcessUserModelAsync(UpsertUserProfileImageRequest command, InfiniLoreUserModel userModel,IUnitOfWork unitOfWork, CancellationToken ct) {
        if (await TryCreateNewMetaDataAsync(command, userModel, unitOfWork, ct) is not {} metaData) {
            logger.Warning("Failed to create new lorescope image metadata");
            return Outcome.FromError("Failed to create new lorescope image metadata");
        }

        Outcome result = await fileStorage.TryUploadFileAsync(S3BucketNames.UserProfileImages, metaData.FileName, command.FileStream, command.ContentType, ct);
        Outcome outcome =  await result.MatchAsync(async state => {
            if (state is false) {
                logger.Warning("Failed to upload file to s3 bucket");
                return Outcome.FromError("Failed to upload file to s3 bucket");
            }

            // ReSharper disable once InvertIf
            if (!await unitOfWork.TryCommitTransactionAsync(ct)) {
                logger.Warning("Failed to commit transaction");
                return Outcome.FromError("Failed to commit transaction");
            }

            return Outcome.True;
        }, _ => {
            logger.Warning("Failed to upload file to s3 bucket");
            return Task.FromResult(Outcome.FromError("Failed to upload file to s3 bucket"));
        });
        
        return outcome;
    }
    
    private async Task<S3FileMetaDataModel?> TryCreateNewMetaDataAsync(UpsertUserProfileImageRequest command, InfiniLoreUserModel foundModel, IUnitOfWork unitOfWork, CancellationToken ct) {
        var userRepo = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);
        var s3FileMetaDataRepo = await unitOfWork.GetRepositoryAsync<IS3FileRepository>(ct);
        
        var metaData = new S3FileMetaDataModel {
            ContentType = command.ContentType,
            FileName = command.ContentType switch {
                "image/jpeg" => $"{foundModel.Id}.jpg",
                "image/png" => $"{foundModel.Id}.png",
                _ => throw new ArgumentOutOfRangeException(nameof(command), command, "Unknown content type")           
            }
        };
        
        ValidationResult validMetaData = await s3FileValidator.ValidateAsync(metaData, ct);
        if (!validMetaData.IsValid) {
            logger.Warning("Validation failed: {Reason}", validMetaData.Errors);
            return null;
        }
        
        foundModel.ProfileImageMetaDataId = metaData.Id;
        foundModel.ProfileImageMetaData = metaData;
        foundModel.UpdateLastModifiedDate();

        await s3FileMetaDataRepo.AddAsync(metaData, ct);
        await userRepo.UpdateAsync(foundModel, ct);
        
        logger.LogInformation("Created new user profile image metadata");
        return metaData;
    }
} 