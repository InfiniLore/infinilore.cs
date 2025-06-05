// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Credentials.Auth0.Services;
using InfiniLore.Credentials.Auth0.Utility;
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
public class SetUserRoleToAuth0Handler(
    IReadonlyUnitOfWorkFactory unitOfWorkFactory, 
    ILogger<UploadUsernameToAuth0Handler> logger,
    IAuth0ClientService auth0ClientFactory,
    IAuth0Utility auth0
) : EventReceiver<InfiniLoreUserCreatedEvent>(logger) {
    private static readonly string DevRole = RolesStore.InfiniloreDeveloper;
    
    protected override async Task ExecuteAsync(InfiniLoreUserCreatedEvent eventModel, CancellationToken ct) {
        Guid userId = eventModel.UserId;
        if (userId == Guid.Empty) return;

        await using IReadonlyUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var userRepo = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        RepoOutcome<InfiniLoreUserModel> userOutcome = await userRepo.GetByIdAsync(userId, ct: ct);
        if (!userOutcome.TryGetAsData(out InfiniLoreUserModel? user)) {
            logger.Warning("Could not find user with id {UserId} in database.", userId);
            return;
        }
        
        IManagementApiClient client = await auth0ClientFactory.GetClientAsync(ct);
        List<Role> roles = await auth0.Roles.GetAllRolesAsync();
        Role? devRole = roles.FirstOrDefault(r => r.Name == DevRole);
        if (devRole is null) {
            logger.Warning("Could not find role {RoleName} in auth0", DevRole);
            return;
        }
        
        foreach (string auth0UserId in user.GetAuth0Ids()) {
            var requestData = new AssignUsersRequest {
                Users = [auth0UserId]
            };
            await client.Roles.AssignUsersAsync(devRole.Id, requestData , ct);
            logger.Information("Assigned user {userId} to role {roleId}", user.Id, devRole.Id);
        }
    }
}
