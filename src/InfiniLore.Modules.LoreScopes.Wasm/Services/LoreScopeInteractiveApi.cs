// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Kiota;
using InfiniLore.Kiota.Api.V1.DataUser.Item.Lorescope;
using InfiniLore.Kiota.Models;
using InfiniLore.Shared;
using InfiniLore.Modules.LoreScopes.Shared.Database;
using InfiniLore.Modules.LoreScopes.Shared.Services;
using InfiniLore.Wasm.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.LoreScopes.Wasm.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<ILoreScopeInteractiveApi>]
public class LoreScopeInteractiveApi(
    ILogger<LoreScopeInteractiveApi> logger,
    IInteractiveApiWasm interactiveApi
) : ILoreScopeInteractiveApi {

    public ValueTask<Result> DeleteLoreScopesAsync(string loreScopeId, CancellationToken ct = default) {
        throw new NotImplementedException();
    }

    public ValueTask<Result<ILoreScopeModel>> GetLoreScopeAsync(string userId, string loreScopeId, CancellationToken ct = default) 
        => throw new NotImplementedException();
    
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
        
        catch (FastEndpointsProblemDetails problem) {
            logger.Error(problem, "Failed to get LoreScopes for user {userId} because '{reason}'", userId, problem.Detail);
            return PaginatedResult<ILoreScopeModel>.FromError(problem.Detail ?? "Unknown FastEndpoints failure");
        }
        
        catch (Exception e) {
            logger.Error(e, "Failed to get LoreScopes for user {userId} because '{reason}'", userId, e.Message);
            return PaginatedResult<ILoreScopeModel>.FromError("Unknown failure");
        }
    }

    
    public ValueTask<Result> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default) 
        => throw new NotImplementedException();

    public ValueTask<Result> UpsertLoreScopeImageAsync(string userId, string loreScopeId, string fileName, string contentType, Stream file, CancellationToken ct = default) 
        => throw new NotImplementedException();
}
