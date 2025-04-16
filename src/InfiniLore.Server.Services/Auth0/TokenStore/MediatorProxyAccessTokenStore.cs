// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Credentials.Auth0;
using InfiniLore.Credentials.Auth0.Services;
using InfiniLore.Server.Services.Messaging;
using InfiniLore.Server.Services.Messaging.Commands.Data.System;
using InfiniLore.Server.Services.Messaging.Queries.Data.System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.Auth0.TokenStore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IAuth0AccessTokenStore>(ServiceLifetime.Scoped)]
public class MediatorProxyAccessTokenStore(ILogger<MediatorProxyAccessTokenStore> logger) : IAuth0AccessTokenStore {
    private readonly GetAuth0AccessTokenQuery _request = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // ReSharper disable once InvertIf
    public async ValueTask<IAuth0AccessToken> GetAccessTokenAsync(CancellationToken ct = default) {
        MessageResponse<IAuth0AccessToken> mediatorResponse = await _request.ExecuteAsync(ct);
        if (!mediatorResponse.TryGetAsSuccess(out IAuth0AccessToken token)) {
            ICollection<string> errors = mediatorResponse.AsError.Value;
            logger.Warning("Failed to retrieve access token. Errors: {Errors}", errors);
            return Auth0AccessToken.Empty;
        }

        return token;
    }

    public async ValueTask SetAccessTokenAsync(IAuth0AccessToken token, CancellationToken ct = default) {
        MessageResponse<bool> mediatorResponse = await new StoreAuth0AccessTokenRequest(token).ExecuteAsync(ct);
        if (!mediatorResponse.TryGetAsSuccess(out bool success)) {
            ICollection<string> errors = mediatorResponse.AsError.Value;
            logger.Critical("Failed to retrieve access token. Errors: {Errors}", errors);
            throw new ApplicationException(errors.ToString());
        }

        if (!success) throw new Exception("Failed to store access token");
    }
}
