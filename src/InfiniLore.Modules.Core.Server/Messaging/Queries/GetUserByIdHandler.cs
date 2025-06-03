// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
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
    protected override Result<InfiniLoreUserModel> AccessDeniedResult => Result.FromError("Cannot get user by id. Access denied.");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<Result<InfiniLoreUserModel>> HandleCommandAsync(GetUserByIdQuery command, CancellationToken ct = default) {
        if (command.UserId == Guid.Empty) return Result.FromError("Cannot get user by id.  id is empty.");

        await using IReadonlyUnitOfWork unitOfWork = factory.Create();
        var userRepository = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        AterraEngine.Unions.Result<InfiniLoreUserModel> result = await userRepository.GetByIdAsync(command.UserId, ct: ct);
        if (!result.TryGetAsSuccess(out InfiniLoreUserModel? user)) {
            logger.Error("Failed to get user by id. {Error}", result.AsError.Value);
            return Result.FromError("Cannot get user by id.");
        }

        // Skip if there is no profile image.
        if (user.ProfileImageMetaDataId is null || user.ProfileImageMetaData is null) return Result.FromSuccess(user);

        AterraEngine.Unions.Result<string> imageUrlResult = await fileStorage.GetFileUrlAsync(
            S3BucketNames.UserProfileImages,
            user.ProfileImageMetaData.FileName,
            expiry: TimeSpan.FromDays(1),
            ct: ct
        );
            
        imageUrlResult.Switch(
            imageUrl => user.ProfileImageUrl = imageUrl,
            error => logger.Error("Failed to get url from S3Bucket. {Error}", error)
        );
            
        return Result.FromSuccess(user);
    }

    protected override ValueTask<bool> ValidateAccessAsync(GetUserByIdQuery command, CancellationToken ct = default)
        => protectionRules.IsOwnerAsync(command.AccessingUser, command.UserId , ct: ct);
}
