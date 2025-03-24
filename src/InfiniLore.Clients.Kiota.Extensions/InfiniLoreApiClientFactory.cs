// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace InfiniLore.Clients.Kiota.Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InfiniLoreApiClientFactory(HttpClient httpClient) {
    private readonly IAuthenticationProvider _authenticationProvider = new AnonymousAuthenticationProvider();

    public InfiniLoreApiClient GetClient()
        => new(new HttpClientRequestAdapter(_authenticationProvider, httpClient: httpClient));
}
