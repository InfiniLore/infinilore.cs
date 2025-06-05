// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Credentials.Auth0.Utility;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Modules.Core.Server.Messaging.Notifications;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.Messaging.Receivers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UploadUsernameToAuth0Handler(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<UploadUsernameToAuth0Handler> logger,
    IAuth0UserUtility auth0UserUtility
) : EventReceiver<InfiniLoreUserCreatedEvent>(logger) {
    protected override async Task ExecuteAsync(InfiniLoreUserCreatedEvent eventModel, CancellationToken ct) {
        Guid userId = eventModel.UserId;
        if (userId == Guid.Empty) return;

        using IServiceScope scope = serviceScopeFactory.CreateScope();
        var unitOfWorkFactory = scope.ServiceProvider.GetRequiredService<IReadonlyUnitOfWorkFactory>();

        await using IReadonlyUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var userRepo = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        RepoOutcome<InfiniLoreUserModel> userOutcome = await userRepo.GetByIdAsync(userId, ct: ct);
        if (!userOutcome.TryGetAsData(out InfiniLoreUserModel? user)) {
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
