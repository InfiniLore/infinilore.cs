// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using InfiniLore.Credentials.Auth0.Utility;
using InfiniLore.Server.Modules.Users.Database;
using InfiniLore.Server.Modules.Users.Messaging.Noticiations;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.Users.Messaging.Receivers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UploadUsernameToAuth0Handler(IReadonlyUnitOfWorkFactory unitOfWorkFactory, ILogger<UploadUsernameToAuth0Handler> logger, IAuth0UserUtility auth0UserUtility) : IEventHandler<NewUserCreatedEvent> {
    public async Task HandleAsync(NewUserCreatedEvent eventModel, CancellationToken ct) {
        Guid userId = eventModel.UserId;
        if (userId == Guid.Empty) return;

        await using IReadonlyUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var userRepo = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        Result<InfiniLoreUserModel> userResult = await userRepo.GetByIdAsync(userId, ct: ct);
        if (!userResult.TryGetAsSuccess(out InfiniLoreUserModel user)) {
            logger.Warning("Could not find user with id {UserId} in database.", userId);
            return;
        }

        foreach (string auth0UserId in user.GetAuth0Ids()) {
            UtilityResult resultUsername = await auth0UserUtility.TryAddOrUpdateAppMetadataAsync(auth0UserId, "username", user.Username, ct);
            if (resultUsername.TryGetAsErrorValue(out string? failureReason)) {
                logger.Warning("Failed to update user {userId} at auth0: {Reason}", auth0UserId, failureReason);
            }

            UtilityResult resultUserId = await auth0UserUtility.TryAddOrUpdateAppMetadataAsync(auth0UserId, "infinilore_user_id", user.Id.ToString(), ct);
            if (resultUserId.TryGetAsErrorValue(out failureReason)) {
                logger.Warning("Failed to update user {userId} at auth0: {Reason}", auth0UserId, failureReason);
                continue;
            }

            logger.Information("Updated user {userId} in auth0 with new username: {Username}", user.Id, user.Username);
        }
    }
}
