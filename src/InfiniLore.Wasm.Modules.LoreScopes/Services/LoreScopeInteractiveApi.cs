// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Shared;
using InfiniLore.Shared.Modules.LoreScopes.Database;
using InfiniLore.Shared.Modules.LoreScopes.Services;
using InfiniLore.Wasm.Contracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Serialization;
using System.Text.Json;

namespace InfiniLore.Wasm.Modules.LoreScopes.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<ILoreScopeInteractiveApi>]
public class LoreScopeInteractiveApi(
    ILogger<LoreScopeInteractiveApi> logger,
    IInteractiveApiWasm interactiveApi
) : ILoreScopeInteractiveApi {

    public async ValueTask<Result<PaginatedData<ILoreScopeModel>>> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        // ReSharper disable twice SuggestVarOrType_SimpleTypes
        try {
            var client = interactiveApi.ApiClient;
            var requestBuilder = client.Api.V1.DataUser[userId].Lorescope;
            var result = await requestBuilder
                .GetAsync(cancellationToken: ct);

            if (result is null) return Result<PaginatedData<ILoreScopeModel>>.FromError("Could not get data from API");
            Stream jsonStream = result.SerializeAsJsonStream();
            var response = await JsonSerializer.DeserializeAsync<PaginatedData<ILoreScopeModel>>(jsonStream, interactiveApi.JsonOptions, ct);
            return Result<PaginatedData<ILoreScopeModel>>.FromSuccess(response);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to get LoreScopes for user {userId} because '{reason}'", userId, e.Message);
            return Result<PaginatedData<ILoreScopeModel>>.FromError($"Unknown failure");
        }
    }
    public ValueTask<Result> CreateLoreScopeAsync(string userId, string newLoreScopeName, CancellationToken ct = default) {
        throw new NotImplementedException();
    }
}
