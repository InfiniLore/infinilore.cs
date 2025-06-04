// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Credentials.Auth0;
using InfiniLore.Credentials.Auth0.Services;
using InfiniLore.Modules.Core.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Core.Server.TokenStore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IAuth0AccessTokenStore>]
public class MediatorProxyAccessTokenStore(
    ILogger<MediatorProxyAccessTokenStore> logger,
    [FromKeyedServices(IMessageBroker.FromServer)] IMessageBroker messageBroker
) : IAuth0AccessTokenStore {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // ReSharper disable once InvertIf
    public async ValueTask<IAuth0AccessToken> GetAccessTokenAsync(CancellationToken ct = default) {
        Outcome<IAuth0AccessToken> mediatorResponse = await messageBroker.GetAuth0AccessTokenAsync(ct);
        
        if (!mediatorResponse.TryGetAsData(out IAuth0AccessToken? token)) {
            string errors = mediatorResponse.AsError.Value;
            logger.Warning("Failed to retrieve access token. Errors: {Errors}", errors);
            return Auth0AccessToken.Empty;
        }

        return token;
    }

    public async ValueTask SetAccessTokenAsync(IAuth0AccessToken token, CancellationToken ct = default) {
        Outcome<bool> mediatorResponse = await messageBroker.StoreAuth0AccessTokenAsync(token, ct);
        
        if (!mediatorResponse.TryGetAsData(out bool success)) {
            string errors = mediatorResponse.AsError.Value;
            logger.Critical("Failed to retrieve access token. Errors: {Errors}", errors);
            throw new ApplicationException(errors);
        }

        if (!success) throw new Exception("Failed to store access token");
    }
}
