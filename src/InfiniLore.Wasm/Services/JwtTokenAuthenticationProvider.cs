// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Shared.Services.JwtToken;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace InfiniLore.Wasm.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IAuthenticationProvider>("jwtToken")]
public class JwtTokenAuthenticationProvider(IJwtTokenJsSecureStorage tokenProvider) : IAuthenticationProvider {

    public async Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = new CancellationToken()) {
        var token = await tokenProvider.GetTokenAsync(cancellationToken);
        
        if (!string.IsNullOrEmpty(token)) {
            // Add the Bearer token to the Authorization header
            request.Headers.Add("Authorization", $"Bearer {token}");
        }

    }
}
