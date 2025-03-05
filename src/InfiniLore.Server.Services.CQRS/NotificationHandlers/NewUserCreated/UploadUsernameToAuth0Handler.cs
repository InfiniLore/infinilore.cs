// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Credentials.Auth0.Utility;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Services.CQRS.Notifications.Account;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.CQRS.NotificationHandlers.NewUserCreated;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UploadUsernameToAuth0Handler(IReadonlyUnitOfWorkFactory unitOfWorkFactory, ILogger<UploadUsernameToAuth0Handler> logger, IAuth0UserUtility auth0UserUtility) : INotificationHandler<NewUserCreatedNotification> {
    public async Task Handle(NewUserCreatedNotification notification, CancellationToken ct) {
        Guid userId = notification.UserId;
        if (userId == Guid.Empty) return;
        
        await using IReadonlyUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var userRepo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);
        
        RepoResult<InfiniLoreUser> userResult = await userRepo.TryGetByIdAsync(userId, ct);
        if (!userResult.TryGetAsSuccess(out InfiniLoreUser? user)) {
            logger.Warning("Could not find user with id {UserId} in database.", userId);
            return;
        }

        foreach (string auth0UserId in user.GetAuth0Ids()) {
            UtilityResult result = await auth0UserUtility.TryAddOrUpdateAppMetadataAsync(auth0UserId, "username", user.Username, ct);
            if (result.TryGetAsFailureValue(out string? failureReason)) {
                logger.Warning("Failed to update user in auth0: {Reason}", failureReason);
                continue;
            }
            
            logger.Information("Updated user {userId} in auth0: {Username}", user.Id, user.Username);
        }
    }
}
