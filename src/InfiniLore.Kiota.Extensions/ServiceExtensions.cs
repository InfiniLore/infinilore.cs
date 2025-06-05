// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace InfiniLore.Kiota;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceExtensions {
    public static IServiceCollection AddInfiniLoreKiotaClient(this IServiceCollection services, string baseUrl) {
        services.AddTransient<InfiniLoreApiClient>(static sp => sp.GetRequiredService<InfiniLoreApiClientFactory>().GetClient());
        
        IHttpClientBuilder httpClientBuilder =  services.AddHttpClient<InfiniLoreApiClientFactory>(HttpClientNames.InfiniLoreApi,
            configureClient: client => client.BaseAddress = new Uri(baseUrl)
        );
        
        foreach (KiotaClientFactory.ActivatableType handler in  KiotaClientFactory.GetDefaultHandlerActivatableTypes()) {
            services.AddTransient(handler);
            httpClientBuilder.AddHttpMessageHandler(sp => (DelegatingHandler)sp.GetRequiredService(handler));
        }

        return services;
    }
}
