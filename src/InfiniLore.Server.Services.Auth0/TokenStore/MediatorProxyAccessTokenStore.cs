// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Credentials.Auth0;
using InfiniLore.Credentials.Auth0.Services;
using InfiniLore.Server.Services.CQRS;
using InfiniLore.Server.Services.CQRS.Commands.Data.System;
using InfiniLore.Server.Services.CQRS.Queries.Data.System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.Auth0.TokenStore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IAuth0AccessTokenStore>(ServiceLifetime.Scoped)]
public class MediatorProxyAccessTokenStore(IMediator mediator, ILogger<MediatorProxyAccessTokenStore> logger) : IAuth0AccessTokenStore {
    private readonly GetAuth0AccessTokenQuery _request = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // ReSharper disable once InvertIf
    public async ValueTask<IAuth0AccessToken> GetAccessTokenAsync(CancellationToken ct = default) {
        MediatorResponse<IAuth0AccessToken> mediatorResponse = await mediator.Send(_request, ct);
        if (!mediatorResponse.TryGetAsSuccess(out IAuth0AccessToken token)) {
            ICollection<string> errors = mediatorResponse.AsError.Value;
            logger.Warning("Failed to retrieve access token. Errors: {Errors}", errors);
            return Auth0AccessToken.Empty;
        }

        return token;
    }

    public async ValueTask SetAccessTokenAsync(IAuth0AccessToken token, CancellationToken ct = default) {
        MediatorResponse<bool> response = await mediator.Send(new StoreAuth0AccessTokenRequest(token), ct);
        if (!response.TryGetAsSuccess(out bool success)) {
            ICollection<string> errors = response.AsError.Value;
            logger.Critical("Failed to retrieve access token. Errors: {Errors}", errors);
            throw new ApplicationException(errors.ToString());
        }

        if (!success) throw new Exception("Failed to store access token");
    }
}
