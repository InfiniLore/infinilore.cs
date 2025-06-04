// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Kiota;
using InfiniLore.Kiota.Api.V1.DataUser.Item.Lorescope;
using InfiniLore.Kiota.Api.V1.DataUser.Item.Lorescope.Item;
using InfiniLore.Kiota.Api.V1.DataUser.Item.Lorescope.Item.PosterImage;
using InfiniLore.Kiota.Models;
using InfiniLore.Modules.Core.Shared;
using InfiniLore.Modules.Core.Shared.Extensions;
using InfiniLore.Modules.Core.Wasm.Contracts.Services;
using InfiniLore.Modules.LoreScopes.Shared.Database;
using InfiniLore.Modules.LoreScopes.Shared.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;

namespace InfiniLore.Modules.LoreScopes.Wasm.Services.InteractiveApi;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiLoreScopes>]
public class InteractiveApiWasmLoreScopes(
    ILogger<InteractiveApiWasmLoreScopes> logger,
    IInteractiveApiWasm interactiveApi
) : IInteractiveApiLoreScopes {
    private const int MaxFileSize = 5 * 1024 * 1024; // 5MB in bytes

    public async ValueTask<Outcome> DeleteLoreScopesAsync(string userId, string loreScopeId, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            WithLoreScopeItemRequestBuilder requestBuilder = client.Api.V1.DataUser[userId].Lorescope[loreScopeId];
            await using Stream? result = await requestBuilder.DeleteAsync(cancellationToken: ct);
            
            if (result is null) return Outcome.FromError(interactiveApi.DefaultApiError);
            return Outcome.FromState(true);
        }

        catch (Exception e) {
            logger.Warning(e, "Failed to delete LoreScope {loreScopeId} because '{reason}'", loreScopeId, e.Message);
            return Outcome.FromError(interactiveApi.DefaultApiError);
        }
    }

    public async ValueTask<Outcome<ILoreScopeModel>> GetLoreScopeAsync(string userId, string loreScopeId, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            WithLoreScopeItemRequestBuilder requestBuilder = client.Api.V1.DataUser[userId].Lorescope[loreScopeId];
            KiotaLoreScopeResponse? result = await requestBuilder.GetAsync(cancellationToken: ct);
            
            if (result is null) return Outcome.FromError(interactiveApi.DefaultApiError);
            return Outcome.FromData(WasmLoreScopeModel.FromKiotaModel(result));
        }
        catch (Exception e) {
            logger.Error(e, "Failed to get LoreScope {loreScopeId} because '{reason}'", loreScopeId, e.Message);
            return Outcome.FromError(interactiveApi.DefaultApiError);
        }
    }

    public async ValueTask<PaginatedOutcome<ILoreScopeModel>> GetLoreScopesAsync(string userId, Pagination pagination, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            LorescopeRequestBuilder? requestBuilder = client.Api.V1.DataUser[userId].Lorescope;

            KiotaLoreScopesResponse? result = await requestBuilder.GetAsync(requestConfiguration: config => {
                    config.QueryParameters.PageNumber = pagination.PageNumber;
                    config.QueryParameters.PageSize = pagination.PageSize;
                    config.QueryParameters.Reverse = false;
                },
                ct
            );
            
            if (result is null) return PaginatedOutcome<ILoreScopeModel>.FromError("Could not get data from API");

            // Ensure we always have a non-null array of items
            ILoreScopeModel[] items = (result.Items ?? Enumerable.Empty<KiotaLoreScopeResponse>())
                .Select(WasmLoreScopeModel.FromKiotaModel)
                .ToArray();

            return new PaginatedData<ILoreScopeModel>(
                items,
                result.TotalCount ?? items.Length,
                result.CurrentPage ?? 1,// Default to page 1 if null
                result.TotalPages ?? 1// Default to 1 page if null
            );
        }
        
        catch (Exception e) {
            logger.Error(e, "Failed to get LoreScopes for user {userId} because '{reason}'", userId, e.Message);
            return PaginatedOutcome<ILoreScopeModel>.FromError("Unknown failure");
        }
    }
    
    public async ValueTask<Outcome> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            LorescopeRequestBuilder? requestBuilder = client.Api.V1.DataUser[userId].Lorescope;
            var requestBody = new KiotaCreateLorescopeEndpointRequest {
                Name = newLoreScopeName
            };
            
            string? result = await requestBuilder.PostAsync(requestBody, cancellationToken: ct);
            
            if (result is null) return Outcome.FromError(interactiveApi.DefaultApiError);
            return Outcome.FromState(true);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to create LoreScope {newLoreScopeName} because '{reason}'", newLoreScopeName, e.Message);
            return Outcome.FromError(interactiveApi.DefaultApiError);
        }
    }

    public async ValueTask<Outcome> UpsertLoreScopeImageAsync(string userId, string loreScopeId, IBrowserFile file, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            PosterImageRequestBuilder requestBuilder = client.Api.V1.DataUser[userId].Lorescope[loreScopeId].PosterImage;
            
            // Read the stream into a memory stream first
            await using MemoryStream stream = await file.ToMemoryStreamAsync(MaxFileSize, ct: ct);
            
            var multipartBody = new MultipartBody();
            multipartBody.AddOrReplacePart(
                "File",  // This must match exactly with the server-side model property name
                file.ContentType,
                stream,
                file.Name
            );
            
            await using Stream? result = await requestBuilder.PostAsync(multipartBody, cancellationToken: ct);
            return Outcome.FromState(true);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to upload image for LoreScope {loreScopeId} because '{reason}'", loreScopeId, e.Message);
            return Outcome.FromError(interactiveApi.DefaultApiError);
        }
    }
}
