// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Configuration;

namespace Tools.InfiniLore.Auth0.Setup;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class CommandEnvironmentFactory {
    public static IServiceProvider Create() {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<IAssemblyEntrypoint>()
            .Build();

        IServiceProvider provider = ServiceProviderFactory.CreateProvider(config);
        return provider;
    }
}
