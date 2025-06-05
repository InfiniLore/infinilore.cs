// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Server.Modules.LoreScopes.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LoreScopes.Server.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetLoreScopesEndpoint(
    ILogger<GetLoreScopesEndpoint> logger,
    [FromKeyedServices(IMessageBroker.FromJwtToken)] IMessageBroker messageBroker
) : InfiniLoreEndpoint<GetLoreScopesEndpointRequest, LoreScopesResponse, LoreScopesMapper> {
    public override void Configure() {
        Get("/data-user/{UserId:guid}/lorescope");
        Permissions(PermissionsStoreConstants.LorescopeRead);
        Policies(ApiPolicies.JwtProtected);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Handler
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task HandleAsync(GetLoreScopesEndpointRequest req, CancellationToken ct) {
        PaginatedOutcome<LoreScopeModel> outcome = await messageBroker.GetLoreScopesByOwnerAsync(
            req.UserId,
            QueryConfig.From(req),
            Pagination.From(req),
            ct:ct
        );

        await outcome.SwitchAsync(
            OnFoundDataAsync,
            (error, token) => OnErrorAsync(error, req, token),
            ct
        );
    }
    
    private async Task OnFoundDataAsync(PaginatedData<LoreScopeModel> data, CancellationToken ct) {
        logger.Information("Successfully retrieved LoreScopes for userId {id}", data.Items.FirstOrDefault()?.OwnerId);

        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        LoreScopesResponse response = Map.FromEntity(data);

        List<Task<LoreScopeResponse>> updateTasks = response.Items.Select(async item => {
            Outcome<string> imageUrlResponse = await messageBroker.GetLorescopePosterImageAsync(item.Id, ct: ct);
            if (imageUrlResponse.TryGetAsData(out string? imageUrl)) {
                item.ImageUrl = imageUrl;
            }
            return item;
        }).ToList();

        // Wait for all tasks to complete
        response.Items = await Task.WhenAll(updateTasks);
        Response = TypedResults.Ok(response);
    }

    private Task OnErrorAsync(string error, GetLoreScopesEndpointRequest req, CancellationToken _) {
        logger.Warning("Failed to get LoreScopes for user {userId} because '{reason}'", req.UserId, error);
        Response = TypedResults.NotFound();
        return Task.CompletedTask;
    }
}
