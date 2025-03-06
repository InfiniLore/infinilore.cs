// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Credentials.Auth0.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Tools.InfiniLore.Auth0.Setup;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceProviderFactory {
    public static IServiceProvider CreateProvider(IConfiguration configuration) {
        var services = new ServiceCollection();

        // Register the auth0 services
        services.AddAuth0ManagementApiServices(configuration, configure: config => {
            ArgumentNullException.ThrowIfNull(configuration["Auth0:Domain"]);
            config.Auth0Options.Domain = configuration["Auth0:Domain"]!;

            ArgumentNullException.ThrowIfNull(configuration["Auth0:ClientId-Management"]);
            config.Auth0Options.ClientId = configuration["Auth0:ClientId-Management"]!;

            ArgumentNullException.ThrowIfNull(configuration["Auth0:ClientSecret-Management"]);
            config.Auth0Options.ClientSecret = configuration["Auth0:ClientSecret-Management"]!;
        });

        // Add Serilog to the services
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .CreateLogger();
        services.AddLogging(static loggingBuilder =>
            loggingBuilder.AddSerilog(Log.Logger, true));


        return services.BuildServiceProvider();
    }
}
