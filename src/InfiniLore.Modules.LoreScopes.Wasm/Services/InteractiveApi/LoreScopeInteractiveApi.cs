// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Kiota;
using InfiniLore.Kiota.Api.V1.DataUser.Item.Lorescope;
using InfiniLore.Kiota.Api.V1.DataUser.Item.Lorescope.Item;
using InfiniLore.Kiota.Api.V1.DataUser.Item.Lorescope.Item.PosterImage;
using InfiniLore.Kiota.Models;
using InfiniLore.Modules.Core.Wasm.Contracts.Services;
using InfiniLore.Modules.LoreScopes.Shared.Database;
using InfiniLore.Modules.LoreScopes.Shared.Services;
using InfiniLore.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;

namespace InfiniLore.Modules.LoreScopes.Wasm.Services.InteractiveApi;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IInteractiveApiLoreScopes>]
public class LoreScopeInteractiveApi(
    ILogger<LoreScopeInteractiveApi> logger,
    IInteractiveApiWasm interactiveApi
) : IInteractiveApiLoreScopes {

    public async ValueTask<Result> DeleteLoreScopesAsync(string userId, string loreScopeId, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            WithLoreScopeItemRequestBuilder requestBuilder = client.Api.V1.DataUser[userId].Lorescope[loreScopeId];
            await using Stream? result = await requestBuilder.DeleteAsync(cancellationToken: ct);
            
            if (result is null) return Result.FromError(interactiveApi.DefaultApiError);
            return Result.FromState(true);
        }

        catch (Exception e) {
            logger.Warning(e, "Failed to delete LoreScope {loreScopeId} because '{reason}'", loreScopeId, e.Message);
            return Result.FromError(interactiveApi.DefaultApiError);
        }
    }

    public async ValueTask<Result<ILoreScopeModel>> GetLoreScopeAsync(string userId, string loreScopeId, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            WithLoreScopeItemRequestBuilder requestBuilder = client.Api.V1.DataUser[userId].Lorescope[loreScopeId];
            KiotaLoreScopeResponse? result = await requestBuilder.GetAsync(cancellationToken: ct);
            
            if (result is null) return Result<ILoreScopeModel>.FromError(interactiveApi.DefaultApiError);
            return Result<ILoreScopeModel>.FromSuccess(WasmLoreScopeModel.FromKiotaModel(result));
        }
        catch (Exception e) {
            logger.Error(e, "Failed to get LoreScope {loreScopeId} because '{reason}'", loreScopeId, e.Message);
            return Result<ILoreScopeModel>.FromError(interactiveApi.DefaultApiError);
        }
    }

    public async ValueTask<PaginatedResult<ILoreScopeModel>> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            LorescopeRequestBuilder? requestBuilder = client.Api.V1.DataUser[userId].Lorescope;
            KiotaLoreScopesResponse? result = await requestBuilder.GetAsync(cancellationToken: ct);

            logger.LogInformation("{@result}", result);

            if (result is null) return PaginatedResult<ILoreScopeModel>.FromError("Could not get data from API");

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
            return PaginatedResult<ILoreScopeModel>.FromError("Unknown failure");
        }
    }


    public ValueTask<Result> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default) 
        => throw new NotImplementedException();

    public async ValueTask<Result> UpsertLoreScopeImageAsync(string userId, string loreScopeId, IBrowserFile file, CancellationToken ct = default) {
        try {
            
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            PosterImageRequestBuilder requestBuilder = client.Api.V1.DataUser[userId].Lorescope[loreScopeId].PosterImage;
            
            // Read the stream into a memory stream first
            await using Stream memoryStream = file.OpenReadStream(cancellationToken: ct);
            
            var multipartBody = new MultipartBody();
            multipartBody.AddOrReplacePart(
                "File",  // This must match exactly with the server-side model property name
                file.ContentType,
                memoryStream,
                file.Name
            );
            
            await using Stream? stream = await requestBuilder.PostAsync(multipartBody, cancellationToken: ct);
            if (stream is null) return Result.FromError(interactiveApi.DefaultApiError);
            return Result.FromState(true);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to upload image for LoreScope {loreScopeId} because '{reason}'", loreScopeId, e.Message);
            return Result.FromError(interactiveApi.DefaultApiError);
        }
    }
}
