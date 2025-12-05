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
        public IServiceCollection AddInfiniModuleProvider(Action<InfiniModuleCollection> config, out InfiniModuleProvider moduleProvider) {
            var collection = InfiniModuleCollection.Create();
            config(collection);
            
            moduleProvider = collection.Build(services);
            
            services.AddSingleton(moduleProvider);
            return services;
        }

        public IServiceCollection AddInfiniModuleProvider(Action<InfiniModuleCollection> config) 
            => services.AddInfiniModuleProvider(config, out _);
    }
}
