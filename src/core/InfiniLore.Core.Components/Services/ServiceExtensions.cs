// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceExtensions {
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfiniLoreCoreComponents() {
            services.RegisterServicesFromInfiniLoreCoreComponents();
            return services;
        }
    }
}
