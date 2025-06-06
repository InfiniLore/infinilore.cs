// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetUserProfileEndpoint(
    ILogger<GetUserProfileEndpoint> logger,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : InfiniLoreEndpoint<GetUserProfileEndpointRequest, UserProfileResponse, UserProfileMapper> {
    
    public override void Configure() {
        Get("/account/profile/{UserId:guid}");
        Permissions(PermissionsStore.AccountRead, PermissionsStore.ProfileRead);
        Policies(ApiPolicies.JwtProtected);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(GetUserProfileEndpointRequest req, CancellationToken ct) { 
        Outcome<InfiniLoreUserModel> outcome = await messageBroker.GetUserByIdAsync(req.UserId, ct: ct);
        await outcome.SwitchAsync(
            async model => {
                logger.Information("Successfully retrieved user with id {id}", req.UserId);
                UserProfileResponse mappedModel = Map.FromEntity(model);
                await SendResultAsync(TypedResults.Ok(mappedModel));
                
            },
            async refused => {
                logger.Warning("Access was refused because : {reason}", refused.Reason);
                await SendResultAsync(TypedResults.Forbid());
            },
            async error => {
                logger.Warning("Failed to get user with id {id} because '{reason}'", req.UserId, error.Value);
                await SendResultAsync(TypedResults.NotFound());
            }
        );
    }
}
