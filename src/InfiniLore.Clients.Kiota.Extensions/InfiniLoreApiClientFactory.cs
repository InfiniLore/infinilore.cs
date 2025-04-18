// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace InfiniLore.Clients.Kiota.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreApiClientFactory(
    HttpClient httpClient, 
    [FromKeyedServices("jwtToken")] IAuthenticationProvider authenticationProvider
) {
    public InfiniLoreApiClient GetClient() => new(new HttpClientRequestAdapter(authenticationProvider, httpClient: httpClient));
}
