// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Credentials.Auth0.Services;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Modules.Core.Server.Messaging.Notifications;
using InfiniLore.Modules.Core.Shared;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.Messaging.Receivers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class StoreKnownPictureIdHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    ILogger<UploadUsernameToAuth0Handler> logger,
    IAuth0ClientService auth0ClientFactory,
    IHttpClientFactory httpClientFactory,
    IS3FileStorage fileStorage,
    IValidator<S3FileMetaDataModel> s3FileValidator
) : EventReceiver<InfiniLoreUserCreatedEvent>(logger) {

    protected override async Task ExecuteAsync(InfiniLoreUserCreatedEvent eventModel, CancellationToken ct) {
        Guid userId = eventModel.UserId;
        if (userId == Guid.Empty) return;

        await using IUnitOfWork unitOfWork = await unitOfWorkFactory.CreateWithTransactionAsync(ct);
        var userRepo = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        RepoOutcome<InfiniLoreUserModel> userOutcome = await userRepo.GetByIdAsync(userId, ct: ct);
        if (!userOutcome.TryGetAsData(out InfiniLoreUserModel? user)) {
            logger.Warning("Could not find user with id {UserId} in database.", userId);
            return;
        }

        IManagementApiClient client = await auth0ClientFactory.GetClientAsync(ct);
        string? auth0Id = user.GetAuth0Ids().FirstOrDefault();
        if (auth0Id.IsNullOrWhiteSpace()) {
            logger.Warning("Could not find auth0 id for user {UserId}", userId);
            return;
        }

        User auth0User = await client.Users.GetAsync(auth0Id, cancellationToken: ct);
        string? auth0PictureId = auth0User.Picture;
        if (auth0PictureId.IsNullOrWhiteSpace()) {
            logger.Warning("Could not find picture id for user {UserId}", userId);
            return;
        }

        if (!Uri.TryCreate(auth0PictureId, UriKind.Absolute, out Uri? pictureUri)) {
            logger.Warning("Could not parse picture id for user {UserId}", userId);
            return;
        }

        using HttpClient httpClient = httpClientFactory.CreateClient();
        using HttpResponseMessage response = await httpClient.GetAsync(pictureUri, HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();

        string contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";// Default to JPEG if not specified
        if (!IsSupported(contentType)) {
            logger.Warning("Unsupported content type {ContentType} for user {UserId}", contentType, userId);
            return;
        }
        byte[] downloadedPicture = await response.Content.ReadAsByteArrayAsync(ct);

        // Create metadata for the file
        S3FileMetaDataModel? metaData = await TryCreateNewMetaDataAsync(user, unitOfWork, contentType, ct);
        if (metaData is null) {
            logger.Warning("Failed to create metadata for user profile image");
            return;
        }

        // Upload the file to S3
        using var memoryStream = new MemoryStream(downloadedPicture);
        Outcome outcome = await fileStorage.TryUploadFileAsync(
            S3BucketNames.UserProfileImages,
            metaData.FileName,
            memoryStream,
            contentType,
            ct
        );

        if (!outcome.TryGetAsState(out bool success) || !success) {
            logger.Warning("Failed to upload file to s3 bucket");
            return;
        }

        if (!await unitOfWork.TryCommitTransactionAsync(ct)) {
            logger.Warning("Failed to commit transaction");
            return;
        }

        logger.Information("Successfully stored profile picture for user {UserId}", userId);
    }

    private async Task<S3FileMetaDataModel?> TryCreateNewMetaDataAsync(
        InfiniLoreUserModel foundModel,
        IUnitOfWork unitOfWork,
        string contentType,
        CancellationToken ct
    ) {
        var userRepo = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);
        var s3FileMetaDataRepo = await unitOfWork.GetRepositoryAsync<IS3FileRepository>(ct);

        var metaData = new S3FileMetaDataModel {
            ContentType = contentType,
            FileName = GetFileNameFromContentType(foundModel.Id, contentType)
        };


        ValidationResult? validMetaData = await s3FileValidator.ValidateAsync(metaData, ct);
        if (!validMetaData.IsValid) {
            logger.Warning("Validation failed: {Reason}", validMetaData.Errors);
            return null;
        }

        foundModel.ProfileImageMetaDataId = metaData.Id;
        foundModel.ProfileImageMetaData = metaData;
        foundModel.UpdateLastModifiedDate();

        await s3FileMetaDataRepo.AddAsync(metaData, ct);
        await userRepo.UpdateAsync(foundModel, ct);

        logger.Information("Created new user profile image metadata");
        return metaData;
    }

    private static string GetFileNameFromContentType(Guid userId, string contentType) => contentType switch {
        "image/jpeg" => $"{userId}.jpg",
        "image/jpg" => $"{userId}.jpg",
        "image/png" => $"{userId}.png",
        _ => throw new ArgumentException($"Unsupported content type: {contentType}")
    };

    private static bool IsSupported(string contentType)
        => contentType is "image/jpeg" or "image/jpg" or "image/png";

}
