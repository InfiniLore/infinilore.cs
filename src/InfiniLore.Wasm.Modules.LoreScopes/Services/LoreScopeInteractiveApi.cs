// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Kiota;
using InfiniLore.Kiota.Api.V1.DataUser.Item.Lorescope;
using InfiniLore.Kiota.Models;
using InfiniLore.Shared;
using InfiniLore.Shared.Modules.LoreScopes.Database;
using InfiniLore.Shared.Modules.LoreScopes.Services;
using InfiniLore.Wasm.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Wasm.Modules.LoreScopes.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<ILoreScopeInteractiveApi>]
public class LoreScopeInteractiveApi(
    ILogger<LoreScopeInteractiveApi> logger,
    IInteractiveApiWasm interactiveApi
) : ILoreScopeInteractiveApi {

    public async ValueTask<PaginatedResult<ILoreScopeModel>> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        try {
            InfiniLoreApiClient client = interactiveApi.ApiClient;
            LorescopeRequestBuilder? requestBuilder = client.Api.V1.DataUser[userId].Lorescope;
            KiotaLoreScopesResponse? result  = await requestBuilder.GetAsync(cancellationToken: ct);

            logger.LogInformation("{@result}", result);
            
            if (result is null) return PaginatedResult<ILoreScopeModel>.FromError("Could not get data from API");
            
            return new PaginatedData<ILoreScopeModel>(
                result.Items?.Select(WasmLoreScopeModel.FromKiotaModel).ToArray() ?? [],
                result.TotalCount ?? result.Items?.Count ?? -1,
                result.CurrentPage ?? -1,
                result.TotalPages ?? -1
            );
        }
        
        catch (Exception e) {
            logger.Error(e, "Failed to get LoreScopes for user {userId} because '{reason}'", userId, e.Message);
            return PaginatedResult<ILoreScopeModel>.FromError("Unknown failure");
        }
    }
    
    public ValueTask<Result> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default) {
        throw new NotImplementedException();
    }
}
