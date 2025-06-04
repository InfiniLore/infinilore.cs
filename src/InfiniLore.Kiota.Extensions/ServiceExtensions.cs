// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace InfiniLore.Kiota;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceExtensions {
    /// <summary>
    /// Adds and configures the InfiniLore Kiota client to the specified service collection.
    /// This includes setting up the necessary HTTP client, default handlers, and the InfiniLore API client factory.
    /// </summary>
    /// <param name="services">The service collection instance to which the InfiniLore Kiota client should be added.</param>
    /// <returns>The same service collection instance with the InfiniLore Kiota client registered.</returns>
    public static IServiceCollection AddInfiniLoreKiotaClient(this IServiceCollection services) {
        services.AddTransient<InfiniLoreApiClient>(static sp => sp.GetRequiredService<InfiniLoreApiClientFactory>().GetClient());
        
        IHttpClientBuilder httpClientBuilder =  services.AddHttpClient<InfiniLoreApiClientFactory>("ServerAPI",
            configureClient: static client => client.BaseAddress = new Uri("https://localhost:7059/")
        );
        
        foreach (KiotaClientFactory.ActivatableType handler in  KiotaClientFactory.GetDefaultHandlerActivatableTypes()) {
            services.AddTransient(handler);
            httpClientBuilder.AddHttpMessageHandler(sp => (DelegatingHandler)sp.GetRequiredService(handler));
        }

        return services;
    }
}
