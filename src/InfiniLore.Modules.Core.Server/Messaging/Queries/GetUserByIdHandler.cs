// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Modules.Core.Shared;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.Messaging.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetUserByIdHandler(
    IReadonlyUnitOfWorkFactory factory,
    IAccessProtectionRules protectionRules,
    ILogger<GetUserByIdHandler> logger,
    IS3FileStorage fileStorage   
) : AccessProtectedCommandHandler<GetUserByIdQuery, InfiniLoreUserModel>(logger) {
    protected override Outcome<InfiniLoreUserModel> AccessDeniedOutcome => Outcome.FromError("Cannot get user by id. Access denied.");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<Outcome<InfiniLoreUserModel>> HandleCommandAsync(GetUserByIdQuery command, CancellationToken ct = default) {
        if (command.UserId == Guid.Empty) return Outcome.FromError("Cannot get user by id.  id is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        Outcome<InfiniLoreUserModel> result = await userRepository.GetByIdAsync(command.UserId, ct: ct);
        if (!result.TryGetAsData(out InfiniLoreUserModel? user)) {
            logger.Error("Failed to get user by id. {Error}", result.AsError.Value);
            return Outcome.FromError("Cannot get user by id.");
        }

        // Skip if there is no profile image.
        if (user.ProfileImageMetaDataId is null || user.ProfileImageMetaData is null) return Outcome.FromData(user);

        Outcome<string> imageUrlResult = await fileStorage.GetFileUrlAsync(
            S3BucketNames.UserProfileImages,
            user.ProfileImageMetaData.FileName,
            expiry: TimeSpan.FromDays(1),
            ct: ct
        );
            
        imageUrlResult.Switch(
            imageUrl => user.ProfileImageUrl = imageUrl,
            error => logger.Error("Failed to get url from S3Bucket. {Error}", error)
        );
            
        return Outcome.FromData(user);
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetUserByIdQuery command, CancellationToken ct = default)
        => protectionRules.IsOwnerAsync(command.AccessingUser, command.UserId , ct: ct);
}
