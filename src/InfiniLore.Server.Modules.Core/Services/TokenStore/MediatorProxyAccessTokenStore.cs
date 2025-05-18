// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Credentials.Auth0;
using InfiniLore.Credentials.Auth0.Services;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Core.Messaging.Commands;
using InfiniLore.Server.Modules.Core.Messaging.Queries;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Modules.Core.TokenStore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IAuth0AccessTokenStore>]
public class MediatorProxyAccessTokenStore(ILogger<MediatorProxyAccessTokenStore> logger, IMessageAccessFactory messageAccessFactory) : IAuth0AccessTokenStore {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // ReSharper disable once InvertIf
    public async ValueTask<IAuth0AccessToken> GetAccessTokenAsync(CancellationToken ct = default) {
        MessageResponse<IAuth0AccessToken> mediatorResponse = await new GetAuth0AccessTokenQuery {
            Access = messageAccessFactory.Empty
        }.ExecuteAsync(ct);
        
        if (!mediatorResponse.TryGetAsSuccess(out IAuth0AccessToken token)) {
            ICollection<string> errors = mediatorResponse.AsError.Value;
            logger.Warning("Failed to retrieve access token. Errors: {Errors}", errors);
            return Auth0AccessToken.Empty;
        }

        return token;
    }

    public async ValueTask SetAccessTokenAsync(IAuth0AccessToken token, CancellationToken ct = default) {
        MessageResponse<bool> mediatorResponse = await new StoreAuth0AccessTokenRequest(token){
            Access = messageAccessFactory.Empty
        }.ExecuteAsync(ct);
        
        if (!mediatorResponse.TryGetAsSuccess(out bool success)) {
            ICollection<string> errors = mediatorResponse.AsError.Value;
            logger.Critical("Failed to retrieve access token. Errors: {Errors}", errors);
            throw new ApplicationException(errors.ToString());
        }

        if (!success) throw new Exception("Failed to store access token");
    }
}
