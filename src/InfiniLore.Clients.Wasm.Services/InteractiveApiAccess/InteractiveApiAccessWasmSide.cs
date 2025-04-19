// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Clients.Kiota;
using InfiniLore.Server.Api.Responses.Data.User.LoreScopes;
using InfiniLore.ServerClient.Services;
using InfiniLore.ServerClient.Shared.JwtToken;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Serialization;
using System.Text.Json;

namespace InfiniLore.Clients.Wasm.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IInteractiveApiAccess>(ServiceLifetime.Scoped)]
public class InteractiveApiAccessWasmSide(
    ILogger<InteractiveApiAccessWasmSide> logger,
    IJwtTokenJsSecureStorage tokenProvider,
    InfiniLoreApiClient apiClient
) : IInteractiveApiAccess {
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<Result<LoreScopesResponse>> GetLoreScopesAsync(string userId, CancellationToken ct = default) {
        // ReSharper disable twice SuggestVarOrType_SimpleTypes
        try {
            var requestBuilder = apiClient.Api.V1.Data.User[userId].Lorescope;
            var result = await requestBuilder
                .GetAsync(cancellationToken: ct);

            if (result is null) return Result<LoreScopesResponse>.FromError("Could not get data from API");

            Stream jsonStream = result.SerializeAsJsonStream();
            var response = await JsonSerializer.DeserializeAsync<LoreScopesResponse>(jsonStream, Options, ct);

            if (response is null) return Result<LoreScopesResponse>.FromError("Could not deserialize data from API");

            return Result<LoreScopesResponse>.FromSuccess(response);
        }
        catch (Exception e) {
            logger.Error(e, "Failed to get LoreScopes for user {userId} because '{reason}'", userId, e.Message);
            return Result<LoreScopesResponse>.FromError($"Unknown failure");
        }
    }
}
