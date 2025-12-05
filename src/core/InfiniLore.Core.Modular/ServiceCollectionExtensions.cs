// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Core.Modular;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceCollectionExtensions {
    extension(IServiceCollection services) {
        public IServiceCollection AddInfiniModuleProvider(Action<InfiniModuleCollection> config, out InfiniModuleCollection collection) {
            collection = new InfiniModuleCollection(services);
            config(collection);
            
            services.AddSingleton(collection);
           
            services.AddSingleton<InfiniModuleProvider>(static provider => provider.GetRequiredService<InfiniModuleCollection>().Build());
            services.AddSingleton<IInfiniModuleProvider>(static provider => provider.GetRequiredService<InfiniModuleProvider>());
            return services;
        }

        public IServiceCollection AddInfiniModuleProvider(Action<InfiniModuleCollection> config) 
            => services.AddInfiniModuleProvider(config, out _);
    }
}
