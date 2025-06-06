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
public class GetUserProfilesEndpoint(
    ILogger<GetUserProfileEndpoint> logger,
    [FromKeyedServices(IMessageBroker.FromServer)] IMessageBroker messageBroker
) : InfiniLoreEndpoint<GetUserProfilesEndpointRequest, UserProfilesResponse, UserProfilesMapper> {
    
    public override void Configure() {
        Get("/account/profile");
        Permissions(PermissionsStore.AccountRead, PermissionsStore.ProfileRead);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(GetUserProfilesEndpointRequest req, CancellationToken ct) {
        PaginatedOutcome<InfiniLoreUserModel> outcome = await messageBroker.GetUsersAsync(
            QueryConfig.From(req),
            Pagination.From(req),
            ct: ct
        );
        
        await outcome.SwitchAsync(
            async data => {
                logger.Information("Successfully retrieved users");
                await SendResultAsync(TypedResults.Ok(Map.FromEntity(data)));
            },
            async reason => {
                logger.Warning("Failed to get users because '{reason}'", reason);
                await SendResultAsync(TypedResults.NotFound());
            }
        );
    }
}
