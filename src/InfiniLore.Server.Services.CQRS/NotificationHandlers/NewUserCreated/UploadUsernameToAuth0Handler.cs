// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Credentials.Auth0.Services;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Services.CQRS.Notifications.Account;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InfiniLore.Server.Services.CQRS.NotificationHandlers.NewUserCreated;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UploadUsernameToAuth0Handler(IAuth0ClientService clientService, IReadonlyUnitOfWorkFactory unitOfWorkFactory, ILogger<UploadUsernameToAuth0Handler> logger) : INotificationHandler<NewUserCreatedNotification> {
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

        try {
            IManagementApiClient client = await clientService.GetClientAsync(ct);
            string[] ids = user.GetAuth0Ids();
      
            foreach (string t in ids) {
                User? foundUserData = await client.Users.GetAsync(t, cancellationToken: ct);
                if (foundUserData is null) continue; // User does not exist in auth0 ???

                Dictionary<string, string> foundDict = foundUserData.AppMetadata switch {
                    null => new Dictionary<string, string>(),
                    string s => JsonSerializer.Deserialize<Dictionary<string, string>>(s) ?? new Dictionary<string, string>(),
                    JsonElement j => JsonSerializer.Deserialize<Dictionary<string, string>>(j.GetRawText()) ?? new Dictionary<string, string>(),
                    _ => new Dictionary<string, string>()
                };
                
                foundDict.AddOrUpdate("username", user.Username);
                
                logger.LogInformation("Updating user in auth0: {@user}", foundUserData);
                var request = new UserUpdateRequest {
                    AppMetadata = foundDict
                };
                
                await client.Users.UpdateAsync(t, request, ct);
            }
        }
        catch (Exception e) {
            logger.Error(e, "Failed to update user in auth0");
        }
    }
}
