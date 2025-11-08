// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;
using InfiniLore.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceExtensions {
    public static IServiceCollection AddInfiniLoreCore(this IServiceCollection services) {
        services.RegisterServicesFromInfiniLoreCore();
        return services;
    }
}
