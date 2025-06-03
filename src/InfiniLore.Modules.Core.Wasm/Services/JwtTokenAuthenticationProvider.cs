// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Shared;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace InfiniLore.Modules.Core.Wasm.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IAuthenticationProvider>("jwtToken")]
public class JwtTokenAuthenticationProvider(IJsSecureStorage tokenProvider) : IAuthenticationProvider {

    public async Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = new CancellationToken()) {
        var token = await tokenProvider.GetTokenAsync(cancellationToken);
        
        if (!string.IsNullOrEmpty(token)) {
            // Add the Bearer token to the Authorization header
            request.Headers.Add("Authorization", $"Bearer {token}");
        }

    }
}
